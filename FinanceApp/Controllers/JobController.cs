using FinanceApp.Business.Services;
using FinanceApp.Common;
using FinanceApp.Data.Entities;
using FinanceApp.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceApp.Presentation.Controllers
{
    public class JobController : BaseController
    {
        private readonly IJobsService _jobService;
        private readonly IPartnersService _businessPartnerService;
        private readonly IAgreementService _agreementService;
        private readonly int _tenantId;

        public JobController(
            IJobsService jobService,
            IPartnersService businessPartnerService,
            IAgreementService agreementService,
            ITenantProvider tenantProvider)
        {
            _jobService = jobService;
            _businessPartnerService = businessPartnerService;
            _agreementService = agreementService;
            _tenantId = tenantProvider.TenantId;
        }

        public async Task<IActionResult> Index()
        {
            var jobs = await _jobService.GetAllJobsAsync();
            return View(jobs);
        }

        public async Task<IActionResult> Create()
        {
            var businessPartners = await _businessPartnerService.GetAllPartnersAsync();
            var agreements = await _agreementService.GetAllAgreementsAsync();

            var viewModel = new JobCreateViewModel
            {
                Job = new Jobs { TenantId = _tenantId }, // TenantId önceden atanıyor
                BusinessPartners = businessPartners
                    .Where(bp => bp.TenantId == _tenantId)
                    .Select(bp => new SelectListItem
                    {
                        Value = bp.Id.ToString(),
                        Text = bp.Name
                    }),
                Agreements = agreements
                    .Where(a => a.TenantId == _tenantId)
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name
                    })
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Job.TenantId = _tenantId;
                await _jobService.CreateJobAsync(viewModel.Job);
                return RedirectToAction(nameof(Index));
            }

            var businessPartners = await _businessPartnerService.GetAllPartnersAsync();
            var agreements = await _agreementService.GetAllAgreementsAsync();

            viewModel.BusinessPartners = businessPartners
                .Where(bp => bp.TenantId == _tenantId)
                .Select(bp => new SelectListItem
                {
                    Value = bp.Id.ToString(),
                    Text = bp.Name
                });

            viewModel.Agreements = agreements
                .Where(a => a.TenantId == _tenantId)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                });

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null || job.TenantId != _tenantId)
                return NotFound();

            var businessPartners = await _businessPartnerService.GetAllPartnersAsync();
            var agreements = await _agreementService.GetAllAgreementsAsync();

            var viewModel = new JobCreateViewModel
            {
                Job = job,
                BusinessPartners = businessPartners
                    .Where(bp => bp.TenantId == _tenantId)
                    .Select(bp => new SelectListItem
                    {
                        Value = bp.Id.ToString(),
                        Text = bp.Name
                    }),
                Agreements = agreements
                    .Where(a => a.TenantId == _tenantId)
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name
                    })
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobCreateViewModel viewModel)
        {
            if (ModelState.IsValid && viewModel.Job.TenantId == _tenantId)
            {
                await _jobService.UpdateJobAsync(viewModel.Job);
                return RedirectToAction(nameof(Index));
            }

            var businessPartners = await _businessPartnerService.GetAllPartnersAsync();
            var agreements = await _agreementService.GetAllAgreementsAsync();

            viewModel.BusinessPartners = businessPartners
                .Where(bp => bp.TenantId == _tenantId)
                .Select(bp => new SelectListItem
                {
                    Value = bp.Id.ToString(),
                    Text = bp.Name
                });

            viewModel.Agreements = agreements
                .Where(a => a.TenantId == _tenantId)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                });

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null || job.TenantId != _tenantId)
                return NotFound();
            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job != null && job.TenantId == _tenantId)
            {
                await _jobService.DeleteJobAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
