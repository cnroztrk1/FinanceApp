using FinanceApp.Business.Services;
using FinanceApp.Data.Entities;
using FinanceApp.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using FinanceApp.API.Hubs;
using System;
using System.Threading.Tasks;

namespace FinanceApp.API.Controllers
{
    [ApiController]
    [Route("api/jobrequest")]
    public class JobRequestController : ControllerBase
    {
        private readonly IJobsService _jobService;
        private readonly IRiskAnalysisService _riskAnalysisService;
        private readonly IHubContext<RiskNotificationHub> _hubContext;
        private readonly ITenantProvider _tenantProvider;

        public JobRequestController(
            IJobsService jobService,
            IRiskAnalysisService riskAnalysisService,
            IHubContext<RiskNotificationHub> hubContext,
            ITenantProvider tenantProvider)
        {
            _jobService = jobService;
            _riskAnalysisService = riskAnalysisService;
            _hubContext = hubContext;
            _tenantProvider = tenantProvider;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateJob([FromBody] Jobs job)
        {
            if (job == null)
                return BadRequest("Invalid job request.");

            job.TenantId = _tenantProvider.TenantId; // TenantId artık sistemden alınıyor

            await _jobService.CreateJobAsync(job);

            var riskAnalysis = new RiskAnalysis
            {
                JobId = job.Id,
                AgreementId = job.AgreementId ?? 0,
                RiskAmount = new Random().Next(1000, 5000),
                AnalysisDate = DateTime.UtcNow,
                Comments = "Otomatik risk analizi oluşturuldu.",
                TenantId = job.TenantId
            };

            await _riskAnalysisService.CreateRiskAnalysisAsync(riskAnalysis);

            await _hubContext.Clients.Group(job.TenantId.ToString())
                .SendAsync("ReceiveRiskNotification", $"Job ID: {job.Id}, Risk Amount: {riskAnalysis.RiskAmount}");

            return Ok(new { message = "Job received and risk analysis completed.", riskAmount = riskAnalysis.RiskAmount });
        }
    }
}
