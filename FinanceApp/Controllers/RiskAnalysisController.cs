using FinanceApp.Business.Services;
using FinanceApp.Data.Entities;
using FinanceApp.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceApp.Presentation.Controllers
{
    public class RiskAnalysisController : Controller
    {
        private readonly IRiskAnalysisService _riskService;
        private readonly IJobsService _jobService;
        private readonly IAgreementService _agreementService;

        public RiskAnalysisController(IRiskAnalysisService riskService, IJobsService jobService, IAgreementService agreementService)
        {
            _riskService = riskService;
            _jobService = jobService;
            _agreementService = agreementService;
        }

        // Liste ekranı: /RiskAnalysis/Index
        public async Task<IActionResult> Index()
        {
            var risks = await _riskService.GetAllRiskAnalysesAsync();
            return View(risks);
        }

        // Yeni risk analizi oluşturma ekranı (GET): /RiskAnalysis/Create
        public async Task<IActionResult> Create()
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

        // Yeni risk analizi kaydı (POST): /RiskAnalysis/Create
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

        // Risk analizi düzenleme (POST): /RiskAnalysis/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RiskAnalysisCreateViewModel viewModel)
        {
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

        // Silme onayı için Delete ekranı (GET): /RiskAnalysis/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var risk = await _riskService.GetRiskAnalysisByIdAsync(id);
            if (risk == null)
                return NotFound();
            return View(risk);
        }

        // Silme işlemi (POST): /RiskAnalysis/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _riskService.DeleteRiskAnalysisAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
