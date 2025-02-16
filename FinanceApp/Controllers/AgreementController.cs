using FinanceApp.Business.Services;
using FinanceApp.Common;
using FinanceApp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FinanceApp.Presentation.Controllers
{
    public class AgreementController : BaseController
    {
        private readonly IAgreementService _agreementService;
        private readonly int _tenantId;

        public AgreementController(IAgreementService agreementService, ITenantProvider tenantProvider)
        {
            _agreementService = agreementService;
            _tenantId = tenantProvider.TenantId;
        }

        public async Task<IActionResult> Index()
        {
            var agreements = await _agreementService.GetAllAgreementsAsync();
            return View(agreements);
        }

        public IActionResult Create()
        {
            ViewBag.TenantId = _tenantId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agreement agreement)
        {
            if (ModelState.IsValid)
            {
                agreement.TenantId = _tenantId;
                await _agreementService.CreateAgreementAsync(agreement);
                return RedirectToAction(nameof(Index));
            }
            return View(agreement);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var agreement = await _agreementService.GetAgreementByIdAsync(id);
            if (agreement == null || agreement.TenantId != _tenantId)
                return NotFound();
            return View(agreement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Agreement agreement)
        {
            if (ModelState.IsValid && agreement.TenantId == _tenantId)
            {
                await _agreementService.UpdateAgreementAsync(agreement);
                return RedirectToAction(nameof(Index));
            }
            return View(agreement);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var agreement = await _agreementService.GetAgreementByIdAsync(id);
            if (agreement == null || agreement.TenantId != _tenantId)
                return NotFound();
            return View(agreement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agreement = await _agreementService.GetAgreementByIdAsync(id);
            if (agreement != null && agreement.TenantId == _tenantId)
            {
                await _agreementService.DeleteAgreementAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
