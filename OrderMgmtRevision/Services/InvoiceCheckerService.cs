using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using OrderMgmtRevision.Data;
using Stripe;

namespace OrderMgmtRevision.Services
{
    public class InvoiceCheckerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(6); // run every 6 hours


        public InvoiceCheckerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var gracePeriod = TimeSpan.FromDays(7);
                    var cutoffDate = DateTime.UtcNow - gracePeriod;
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                    var overdueInvoices = await db.UserInvoices
                        .Include(i => i.User)
                        .Where(i => !i.IsPaid && i.DateDue < cutoffDate && i.User.IsActive && i.User.UserName != "admin")
                        .ToListAsync(stoppingToken);

                    foreach (var invoice in overdueInvoices)
                    {
                        invoice.User.IsActive = false;
                        await logService.LogUserActivityAdmin($"Disabled user ${invoice.User.UserName} for overdue invoice.", "BackgroundService");
                    }

                    await db.SaveChangesAsync(stoppingToken);
                    await logService.LogUserActivityAdmin($"Ran InvoiceCheckerService.", "BackgroundService");
                }
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
