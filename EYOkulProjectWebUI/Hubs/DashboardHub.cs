using EYOkulProjectWebUI.DAL;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EYOkulProjectWebUI.Hubs
{
    public class DashboardHub : Hub
    {
        private readonly EYOkulDbContext _dbContext;

        public DashboardHub(EYOkulDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }

        public async Task SendProducts()
        {
            var query = await _dbContext.TBL_MESSAGE.OrderByDescending(x=>x.Id).Take(2).ToListAsync();
            //foreach (var emp in query)
            //{
            //    _dbContext.Entry(emp).Reload();
            //}
            await Clients.All.SendAsync("ReceivedProducts", query);
        }
    }
}
