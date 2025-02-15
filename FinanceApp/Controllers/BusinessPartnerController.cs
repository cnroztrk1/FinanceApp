using FinanceApp.Business.Services;
using FinanceApp.Common;
using FinanceApp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceApp.Presentation.Controllers
{
    public class BusinessPartnerController : Controller
    {
        private readonly IPartnersService _partnerService;
        private readonly int _tenantId;

        public BusinessPartnerController(IPartnersService partnerService, ITenantProvider tenantProvider)
        {
            _partnerService = partnerService;
            _tenantId = tenantProvider.TenantId;
        }

        public async Task<IActionResult> Index()
        {
            var partners = await _partnerService.GetAllPartnersAsync();
            return View(partners.Where(p => p.TenantId == _tenantId));
        }

        public IActionResult Create()
        {
            ViewBag.TenantId = _tenantId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partners partner)
        {
            if (ModelState.IsValid)
            {
                partner.TenantId = _tenantId;
                await _partnerService.CreatePartnerAsync(partner);
                return RedirectToAction(nameof(Index));
            }
            return View(partner);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(id);
            if (partner == null || partner.TenantId != _tenantId)
                return NotFound();
            return View(partner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Partners partner)
        {
            if (ModelState.IsValid && partner.TenantId == _tenantId)
            {
                await _partnerService.UpdatePartnerAsync(partner);
                return RedirectToAction(nameof(Index));
            }
            return View(partner);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(id);
            if (partner == null || partner.TenantId != _tenantId)
                return NotFound();
            return View(partner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(id);
            if (partner != null && partner.TenantId == _tenantId)
            {
                await _partnerService.DeletePartnerAsync(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
