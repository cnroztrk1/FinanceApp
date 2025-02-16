using FinanceApp.Business.Services;
using FinanceApp.Common;
using FinanceApp.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FinanceApp.Presentation.Controllers
{
    public class FinanceReportController : BaseController
    {
        private readonly IJobsService _jobService;
        private readonly IAgreementService _agreementService;
        private readonly IRiskAnalysisService _riskAnalysisService;
        private readonly ITenantProvider _tenantProvider;

        public FinanceReportController(
            IJobsService jobService,
            IAgreementService agreementService,
            IRiskAnalysisService riskAnalysisService,
            ITenantProvider tenantProvider)
        {
            _jobService = jobService;
            _agreementService = agreementService;
            _riskAnalysisService = riskAnalysisService;
            _tenantProvider = tenantProvider;
        }

        public async Task<IActionResult> Index()
        {
            var tenantId = _tenantProvider.TenantId;

            // Mali Verileri Al
            var jobs = await _jobService.GetAllJobsAsync();
            var agreements = await _agreementService.GetAllAgreementsAsync();
            var risks = await _riskAnalysisService.GetAllRiskAnalysesAsync();

            var viewModel = new FinanceReportViewModel
            {
                TotalJobs = jobs.Count(j => j.TenantId == tenantId),
                TotalAgreements = agreements.Count(a => a.TenantId == tenantId),
                TotalRiskAmount = risks.Where(r => r.TenantId == tenantId).Sum(r => r.RiskAmount),
                RiskAnalysisData = risks.Where(r => r.TenantId == tenantId)
                                        .GroupBy(r => r.AnalysisDate.Year)
                                        .Select(g => new RiskAnalysisYearlyData
                                        {
                                            Year = g.Key,
                                            TotalRisk = g.Sum(r => r.RiskAmount)
                                        }).ToList()
            };

            return View(viewModel);
        }
    }
}
