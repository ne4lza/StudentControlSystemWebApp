using EYOkulProjectWebUI.DAL;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EYOkulProjectWebUI.Hubs
{
    public class DashboardHub : Hub
    {
        private readonly EYOkulDbContext dbContext;

        public DashboardHub(EYOkulDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task SendProducts()
        {
            var query = await dbContext.TBL_MESSAGE.OrderByDescending(x=>x.Id).ToListAsync();
            foreach (var emp in query)
            {
                dbContext.Entry(emp).Reload();
            }
            await Clients.All.SendAsync("ReceivedProducts", query);
        }
    }
}
