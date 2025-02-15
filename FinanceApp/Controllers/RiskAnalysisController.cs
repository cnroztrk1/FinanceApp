using FinanceApp.Business.Services;
using FinanceApp.Common;
using FinanceApp.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceApp.Presentation.Controllers
{
    public class RiskAnalysisController : Controller
    {
        private readonly IRiskAnalysisService _riskService;
        private readonly int _tenantId;

        public RiskAnalysisController(IRiskAnalysisService riskService, ITenantProvider tenantProvider)
        {
            _riskService = riskService;
            _tenantId = tenantProvider.TenantId;
        }

        public async Task<IActionResult> Index()
        {
            var risks = await _riskService.GetAllRiskAnalysesAsync();
            return View(risks.Where(r => r.TenantId == _tenantId));
        }

        public IActionResult Create()
        {
            ViewBag.TenantId = _tenantId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RiskAnalysis riskAnalysis)
        {
            if (ModelState.IsValid)
            {
                riskAnalysis.TenantId = _tenantId;
                await _riskService.CreateRiskAnalysisAsync(riskAnalysis);
                return RedirectToAction(nameof(Index));
            }
            return View(riskAnalysis);
        }
    }
}
