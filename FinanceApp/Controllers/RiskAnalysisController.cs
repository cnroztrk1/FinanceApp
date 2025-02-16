using FinanceApp.Business.Services;
using FinanceApp.Common;
using FinanceApp.Data.Entities;
using FinanceApp.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FinanceApp.Presentation.Controllers
{
    public class RiskAnalysisController : BaseController
    {
        private readonly IRiskAnalysisService _riskService;
        private readonly IJobsService _jobService;
        private readonly IPartnersService _partnerService;
        private readonly IAgreementService _agreementService;
        private readonly ITenantProvider _tenantProvider;

        public RiskAnalysisController(
            IRiskAnalysisService riskService,
            IJobsService jobService,
            IPartnersService partnerService,
            IAgreementService agreementService,
            ITenantProvider tenantProvider)
        {
            _riskService = riskService;
            _jobService = jobService;
            _partnerService = partnerService;
            _agreementService = agreementService;
            _tenantProvider = tenantProvider;
        }

        public async Task<IActionResult> Index()
        {
            var risks = await _riskService.GetAllRiskAnalysesAsync();
            var tenantId = _tenantProvider.TenantId;

            var riskViewModels = risks
                .Where(r => r.TenantId == tenantId)
                .Select(r => new RiskAnalysisViewModel
                {
                    Id = r.Id,
                    RiskAmount = r.RiskAmount,
                    Comments = r.Comments,
                    AnalysisDate = r.AnalysisDate,
                    JobTitle = _jobService.GetJobByIdAsync(r.JobId).Result?.Title ?? "Bilinmiyor",
                    BusinessPartnerName = _partnerService.GetPartnerByIdAsync(
                        _jobService.GetJobByIdAsync(r.JobId).Result?.BusinessPartnerId ?? 0
                    ).Result?.Name ?? "Bilinmiyor",
                    AgreementName = _agreementService.GetAgreementByIdAsync(r.AgreementId).Result?.Name ?? "Bilinmiyor"
                }).ToList();

            return View(riskViewModels);
        }

        public IActionResult Create()
        {
            ViewBag.TenantId = _tenantProvider.TenantId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RiskAnalysis riskAnalysis)
        {
            if (ModelState.IsValid)
            {
                riskAnalysis.TenantId = _tenantProvider.TenantId;
                await _riskService.CreateRiskAnalysisAsync(riskAnalysis);
                return RedirectToAction(nameof(Index));
            }
            return View(riskAnalysis);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var riskAnalysis = await _riskService.GetRiskAnalysisByIdAsync(id);
            if (riskAnalysis == null || riskAnalysis.TenantId != _tenantProvider.TenantId)
                return NotFound();

            var viewModel = new RiskAnalysisCreateViewModel
            {
                RiskAnalysis = riskAnalysis
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RiskAnalysisCreateViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel.RiskAnalysis.TenantId == _tenantProvider.TenantId)
            {
                await _riskService.UpdateRiskAnalysisAsync(viewModel.RiskAnalysis);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var riskAnalysis = await _riskService.GetRiskAnalysisByIdAsync(id);
            if (riskAnalysis == null || riskAnalysis.TenantId != _tenantProvider.TenantId)
                return NotFound();

            return View(riskAnalysis);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var riskAnalysis = await _riskService.GetRiskAnalysisByIdAsync(id);
            if (riskAnalysis != null && riskAnalysis.TenantId == _tenantProvider.TenantId)
            {
                await _riskService.DeleteRiskAnalysisAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
