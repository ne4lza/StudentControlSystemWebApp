using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Hubs;
using EYOkulProjectWebUI.Models;
using Microsoft.AspNetCore.SignalR;
using TableDependency.SqlClient;

namespace EYOkulProjectWebUI.Subscription.Concreate
{

    public interface IService_Transaction
    {
        void configure(string TableName = ""); // veya null
    }

    public class Service_Transaction<T> : IService_Transaction where T : class, new()

    {
        IConfiguration _configuration;
        IHubContext<TransactionHub> _HubContext;

        public Service_Transaction (IConfiguration configuration, IHubContext<TransactionHub> hubContext )
        {

            _configuration = configuration;
            _HubContext = hubContext;
        }

        SqlTableDependency<T> _tableDependency;
        public void configure(string TableName)
        {
            _tableDependency = new SqlTableDependency<T>(_configuration.GetConnectionString("SQL"), TableName);
            _tableDependency.OnChanged += async (o, e) =>
            {



                EYOkulDbContext context = new EYOkulDbContext();
                var data = from trmodel in context.TBL_TRANSACTIONS
                           join ogrmodel in context.TBL_STUDENTS on
                           trmodel.StudentId equals ogrmodel.Id
                           join grdModel in context.TBL_CARDS on
                           trmodel.GuardianId equals grdModel.Id
                           join clssmodel in context.TBL_CLASS on
                           trmodel.ClassId equals clssmodel.Id
                           select new
                           {
                               TransactionId = trmodel.Id,
                               StudentName = ogrmodel.StudentName,
                               StudentSurname = ogrmodel.StudentSurName,
                               GuardianName = grdModel.GuardianName,
                               GuardianSurName = grdModel.GuardianSurName,
                               ClassName = clssmodel.ClassName,
                               Image = trmodel.Image,
                           };

                data.ToList();

                await _HubContext.Clients.All.SendAsync("receiveMessage",e.Entity);




            };

            _tableDependency.OnError += (o, e) => { };

            _tableDependency.Start();
        }
        ~Service_Transaction()
        {
            _tableDependency.Stop();
        }
    }
}
