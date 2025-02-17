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

        public async Task<IActionResult> Index() //Listeleme
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

        public async Task<IActionResult> Create()//Oluşturma
        {
            var jobs = await _jobService.GetAllJobsAsync();
            // Oluşturulan iş listesi üzerinden, jobId->agreementId mappingini elde ediyoruz.
            var jobAgreementMapping = jobs.ToDictionary(j => j.Id, j => j.AgreementId);
            var viewModel = new RiskAnalysisCreateViewModel
            {
                RiskAnalysis = new RiskAnalysis(),
                Jobs = jobs.Select(j => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = j.Id.ToString(),
                    Text = j.Title
                }),
                JobAgreementsJson = System.Text.Json.JsonSerializer.Serialize(jobAgreementMapping)
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RiskAnalysisCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // RiskAnalysis.AgreementId, client-side script tarafından job seçimine göre doldurulmuş olmalı
                await _riskService.CreateRiskAnalysisAsync(viewModel.RiskAnalysis);
                return RedirectToAction(nameof(Index));
            }
            // Hata durumunda, dropdown listesini ve mapping bilgisini tekrar dolduralım.
            var jobs = await _jobService.GetAllJobsAsync();
            viewModel.Jobs = jobs.Select(j => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = j.Id.ToString(),
                Text = j.Title
            });
            viewModel.JobAgreementsJson = System.Text.Json.JsonSerializer.Serialize(jobs.ToDictionary(j => j.Id, j => j.AgreementId));
            return View(viewModel);
        }
        // Risk analizi düzenleme ekranı (GET): /RiskAnalysis/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var risk = await _riskService.GetRiskAnalysisByIdAsync(id);
            if (risk == null)
                return NotFound();
            var jobs = await _jobService.GetAllJobsAsync();
            var jobAgreementMapping = jobs.ToDictionary(j => j.Id, j => j.AgreementId);
            var viewModel = new RiskAnalysisCreateViewModel
            {
                RiskAnalysis = risk,
                Jobs = jobs.Select(j => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = j.Id.ToString(),
                    Text = j.Title
                }),
                JobAgreementsJson = System.Text.Json.JsonSerializer.Serialize(jobAgreementMapping)
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RiskAnalysisCreateViewModel viewModel)
        {
            viewModel.RiskAnalysis.TenantId= _tenantProvider.TenantId;
            if (ModelState.IsValid)
            {
                await _riskService.UpdateRiskAnalysisAsync(viewModel.RiskAnalysis);
                return RedirectToAction(nameof(Index));
            }
            var jobs = await _jobService.GetAllJobsAsync();
            viewModel.Jobs = jobs.Select(j => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = j.Id.ToString(),
                Text = j.Title
            });
            viewModel.JobAgreementsJson = System.Text.Json.JsonSerializer.Serialize(jobs.ToDictionary(j => j.Id, j => j.AgreementId));
            return View(viewModel);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var risk = await _riskService.GetRiskAnalysisByIdAsync(id);
            if (risk == null)
                return NotFound();
            return View(risk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)//Silme Onay
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
