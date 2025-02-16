using FinanceApp.API.Models;
using FinanceApp.Business.Services;
using FinanceApp.Common;
using FinanceApp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using FinanceApp.API.Hubs;

namespace FinanceApp.API.Controllers
{
    [ApiController]
    [Route("api/jobrequest")]
    public class JobRequestController : ControllerBase
    {
        private readonly IJobsService _jobService;
        private readonly IRiskAnalysisService _riskAnalysisService;
        private readonly IAgreementService _agreementService;
        private readonly IPartnersService _partnerService;
        private readonly IHubContext<RiskNotificationHub> _hubContext;
        private readonly ITenantProvider _tenantProvider;

        public JobRequestController(
            IJobsService jobService,
            IRiskAnalysisService riskAnalysisService,
            IAgreementService agreementService,
            IPartnersService partnerService,
            IHubContext<RiskNotificationHub> hubContext,
            ITenantProvider tenantProvider)
        {
            _jobService = jobService;
            _riskAnalysisService = riskAnalysisService;
            _agreementService = agreementService;
            _partnerService = partnerService;
            _hubContext = hubContext;
            _tenantProvider = tenantProvider;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateJob([FromBody] JobRequestModel jobRequest)
        {

            int tenantId = (int)HttpContext.Items["TenantId"];

            if (jobRequest == null)
                return BadRequest("Invalid job request.");

            var businessPartner = await _partnerService.GetPartnerByIdAsync(jobRequest.BusinessPartnerId);
            if (businessPartner == null)
            {
                return BadRequest(new { message = "Belirtilen BusinessPartnerId bu TenantId için mevcut değil!" });
            }

            // Agreement kontrolü
            var agreement = await _agreementService.GetAgreementByIdAsync(jobRequest.AgreementId);
            if (agreement == null)
            {
                return BadRequest(new { message = "Belirtilen AgreementId bu TenantId için mevcut değil!" });
            }

            var job = new Jobs
            {
                Title = jobRequest.Title,
                Description = jobRequest.Description,
                BusinessPartnerId = jobRequest.BusinessPartnerId,
                AgreementId = jobRequest.AgreementId,
                ReceivedDate = DateTime.UtcNow,
                Status = "Beklemede",
                TenantId = tenantId
            };

            await _jobService.CreateJobAsync(job);

            var riskAnalysis = new RiskAnalysis
            {
                JobId = job.Id,
                AgreementId = job.AgreementId.Value,
                RiskAmount = new Random().Next(1000, 10000),
                AnalysisDate = DateTime.UtcNow,
                Comments = "Otomatik risk analizi oluşturuldu.",
                TenantId = job.TenantId
            };

            await _riskAnalysisService.CreateRiskAnalysisAsync(riskAnalysis);

            string notificationMessage = $"Yeni Risk Analizi Yapıldı! Job ID: {job.Id}, Risk Amount: {riskAnalysis.RiskAmount}";
            await _hubContext.Clients.All.SendAsync("ReceiveGlobalNotification", notificationMessage);

            return Ok(new { message = "İş konusu oluşturuldu ve Risk analizi yapıldı.", riskAmount = riskAnalysis.RiskAmount });
        }
    }
}
