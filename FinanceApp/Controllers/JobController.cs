using FinanceApp.Business.Services;
using FinanceApp.Data.Entities;
using FinanceApp.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Linq;

namespace FinanceApp.Presentation.Controllers
{
    public class JobController : Controller
    {
        private readonly IJobsService _jobService;
        private readonly IPartnersService _businessPartnerService;
        private readonly IAgreementService _agreementService;

        public JobController(IJobsService jobService, IPartnersService businessPartnerService, IAgreementService agreementService)
        {
            _jobService = jobService;
            _businessPartnerService = businessPartnerService;
            _agreementService = agreementService;
        }

        // Liste ekranı: /Job/Index
        public async Task<IActionResult> Index()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return View(jobs);
        }

        // Yeni iş konusu oluşturma ekranı (GET): /Job/Create
        public async Task<IActionResult> Create()
        {
            var businessPartners = await _businessPartnerService.GetAllPartnersAsync();
            var agreements = await _agreementService.GetAllAgreementsAsync();

            var viewModel = new JobCreateViewModel
            {
                Job = new Jobs(),
                BusinessPartners = businessPartners.Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                }),
                Agreements = agreements.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                })
            };

            return View(viewModel);
        }

        // Yeni iş konusu kaydı (POST): /Job/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _jobService.CreateJobAsync(viewModel.Job);
                return RedirectToAction(nameof(Index));
            }
            // Model geçerli değilse, dropdown'ları tekrar dolduralım:
            var businessPartners = await _businessPartnerService.GetAllPartnersAsync();
            var agreements = await _agreementService.GetAllAgreementsAsync();
            viewModel.BusinessPartners = businessPartners.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            });
            viewModel.Agreements = agreements.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });
            return View(viewModel);
        }

        // İş konusu düzenleme ekranı (GET): /Job/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
                return NotFound();

            var businessPartners = await _businessPartnerService.GetAllPartnersAsync();
            var agreements = await _agreementService.GetAllAgreementsAsync();

            var viewModel = new JobCreateViewModel
            {
                Job = job,
                BusinessPartners = businessPartners.Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                }),
                Agreements = agreements.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                })
            };

            return View(viewModel);
        }

        // İş konusu düzenleme (POST): /Job/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _jobService.UpdateJobAsync(viewModel.Job);
                return RedirectToAction(nameof(Index));
            }
            var businessPartners = await _businessPartnerService.GetAllPartnersAsync();
            var agreements = await _agreementService.GetAllAgreementsAsync();
            viewModel.BusinessPartners = businessPartners.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            });
            viewModel.Agreements = agreements.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            });
            return View(viewModel);
        }

        // Silme onayı için Delete ekranı (GET): /Job/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
                return NotFound();
            return View(job);
        }

        // Silme işlemi (POST): /Job/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _jobService.DeleteJobAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
