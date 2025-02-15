using Microsoft.AspNetCore.Http;
using System;

namespace FinanceApp.Common
{
    public interface ITenantProvider
    {
        int TenantId { get; }
    }

    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly int _defaultTenantId = 1; // Varsayılan Tenant ID

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int TenantId // Burada şimdilik http header dan aldım. Daha sonrasında domaine göre veya herhangi bir auth sistemi ile burası değiştirilebilir.
                            // Firma bazlı Program.cs de belirlenip test edilebilir.
        {
            get
            {
                try
                {
                    var tenantIdString = _httpContextAccessor.HttpContext?.Request.Headers["TenantId"];
                    if (string.IsNullOrEmpty(tenantIdString))
                    {
                        return _defaultTenantId;
                    }

                    if (int.TryParse(tenantIdString, out var tenantId))
                    {
                        return tenantId;
                    }

                    return _defaultTenantId;
                }
                catch (Exception ex)
                {
                    return _defaultTenantId;
                }
            }
        }
    }
}
