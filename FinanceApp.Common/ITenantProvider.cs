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

        public int TenantId
        {
            get
            {
                try
                {
                    var tenantId = _httpContextAccessor.HttpContext?.Session.GetInt32("TenantId");

                    if (tenantId == null || tenantId == 0)
                    {
                        return _defaultTenantId;
                    }

                    return tenantId.Value;
                }
                catch (Exception)
                {
                    return _defaultTenantId;
                }
            }
        }
    }
}
