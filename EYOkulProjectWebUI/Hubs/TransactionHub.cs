using EYOkulProjectWebUI.DAL;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace EYOkulProjectWebUI.Hubs
{
    public class TransactionHub : Hub
    {
        private readonly EYOkulDbContext _context;

        public TransactionHub(EYOkulDbContext context)
        {
            _context = context;
        }

        public async Task SendTransactions()
        {
            await Clients.All.SendAsync("TransactionsReceived", "Hello");
        }
    }
}
