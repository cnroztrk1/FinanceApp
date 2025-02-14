using FinanceApp.Business.Services;
using FinanceApp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceApp.Presentation.Controllers
{
    public class AgreementController : Controller
    {
        private readonly IAgreementService _agreementService;

        public AgreementController(IAgreementService agreementService)
        {
            _agreementService = agreementService;
        }

        // Liste ekranı: /Agreement/Index
        public async Task<IActionResult> Index()
        {
            var agreements = await _agreementService.GetAllAgreementsAsync();
            return View(agreements);
        }

        // Yeni anlaşma oluşturma ekranı (GET): /Agreement/Create
        public IActionResult Create()
        {
            return View();
        }

        // Yeni anlaşma kaydı (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agreement agreement)
        {
            if (ModelState.IsValid)
            {
                await _agreementService.CreateAgreementAsync(agreement);
                return RedirectToAction(nameof(Index));
            }
            return View(agreement);
        }

        // Anlaşma güncelleme ekranı (GET): /Agreement/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var agreement = await _agreementService.GetAgreementByIdAsync(id);
            if (agreement == null)
                return NotFound();
            return View(agreement);
        }

        // Anlaşma güncelleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Agreement agreement)
        {
            if (ModelState.IsValid)
            {
                await _agreementService.UpdateAgreementAsync(agreement);
                return RedirectToAction(nameof(Index));
            }
            return View(agreement);
        }

        // Silme onayı için Delete ekranı (GET): /Agreement/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var agreement = await _agreementService.GetAgreementByIdAsync(id);
            if (agreement == null)
                return NotFound();
            return View(agreement);
        }

        // Silme işlemi (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _agreementService.DeleteAgreementAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Otomatik tamamlama örneği: /Agreement/Search?term=...
        public async Task<IActionResult> Search(string term)
        {
            var agreements = await _agreementService.GetAllAgreementsAsync();
            var results = agreements
                          .Where(a => a.Name.Contains(term, StringComparison.OrdinalIgnoreCase))
                          .Select(a => a.Name)
                          .ToList();
            return Json(results);
        }
    }
}
