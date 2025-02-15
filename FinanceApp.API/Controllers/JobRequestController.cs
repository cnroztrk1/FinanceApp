using FinanceApp.API.Models;
using FinanceApp.Business.Services;
using FinanceApp.Common;
using FinanceApp.Data.Entities;
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
        public async Task<IActionResult> CreateJob([FromBody] JobRequestModel jobRequest)
        {
            if (jobRequest == null)
                return BadRequest("Invalid job request.");

            var job = new Jobs
            {
                Title = jobRequest.Title,
                Description = jobRequest.Description,
                BusinessPartnerId = jobRequest.BusinessPartnerId,
                AgreementId = jobRequest.AgreementId,
                ReceivedDate = DateTime.UtcNow, 
                Status = "Beklemede", 
                TenantId = _tenantProvider.TenantId 
            };

            // İş kaydını oluştur
            await _jobService.CreateJobAsync(job);

            // Risk analizi hesapla
            var riskAnalysis = new RiskAnalysis
            {
                JobId = job.Id,
                AgreementId = job.AgreementId.Value,
                RiskAmount = new Random().Next(1000, 5000),
                AnalysisDate = DateTime.UtcNow,
                Comments = "Otomatik risk analizi oluşturuldu.",
                TenantId = job.TenantId
            };

            await _riskAnalysisService.CreateRiskAnalysisAsync(riskAnalysis);

            // SignalR ile anlık bildirim gönder
            await _hubContext.Clients.Group(job.TenantId.ToString())
                .SendAsync("ReceiveRiskNotification", $"Job ID: {job.Id}, Risk Amount: {riskAnalysis.RiskAmount}");

            return Ok(new { message = "Job received and risk analysis completed.", riskAmount = riskAnalysis.RiskAmount });
        }
    }
}
