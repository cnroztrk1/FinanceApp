﻿using Microsoft.AspNetCore.SignalR;

namespace FinanceApp.API.Hubs
{
    public class RiskNotificationHub : Hub //İstekleri gnöderecek olan hub
    {
        public override async Task OnConnectedAsync()
        {
            var tenantId = Context.GetHttpContext()?.Request.Cookies["TenantId"];
            if (!string.IsNullOrEmpty(tenantId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, tenantId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var tenantId = Context.GetHttpContext()?.Request.Cookies["TenantId"];
            if (!string.IsNullOrEmpty(tenantId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, tenantId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendRiskNotification(string tenantId, string message)
        {
            await Clients.Group(tenantId).SendAsync("ReceiveRiskNotification", message);
        }

    }
}
