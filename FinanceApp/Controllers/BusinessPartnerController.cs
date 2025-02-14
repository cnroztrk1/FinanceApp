using FinanceApp.Business.Services;
using FinanceApp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinanceApp.Presentation.Controllers
{
    public class BusinessPartnerController : Controller
    {
        private readonly IPartnersService _partnerService;

        public BusinessPartnerController(IPartnersService partnerService)
        {
            _partnerService = partnerService;
        }

        // Liste ekranı: /BusinessPartner/Index
        public async Task<IActionResult> Index()
        {
            var partners = await _partnerService.GetAllPartnersAsync();
            return View(partners);
        }

        // Yeni iş ortağı oluşturma ekranı (GET): /BusinessPartner/Create
        public IActionResult Create()
        {
            return View();
        }

        // Yeni iş ortağı kaydı (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partners partner)
        {
            if (ModelState.IsValid)
            {
                await _partnerService.CreatePartnerAsync(partner);
                return RedirectToAction(nameof(Index));
            }
            return View(partner);
        }

        // İş ortağı düzenleme ekranı (GET): /BusinessPartner/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(id);
            if (partner == null)
                return NotFound();
            return View(partner);
        }

        // İş ortağı düzenleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Partners partner)
        {
            if (ModelState.IsValid)
            {
                await _partnerService.UpdatePartnerAsync(partner);
                return RedirectToAction(nameof(Index));
            }
            return View(partner);
        }

        // Silme onayı için Delete ekranı (GET): /BusinessPartner/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var partner = await _partnerService.GetPartnerByIdAsync(id);
            if (partner == null)
                return NotFound();
            return View(partner);
        }

        // Silme işlemi (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _partnerService.DeletePartnerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
