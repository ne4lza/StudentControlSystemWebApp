using EYOkulProjectWebUI.DAL;
using EYOkulProjectWebUI.Hubs;
using EYOkulProjectWebUI.Models;
using TableDependency.SqlClient;

namespace EYOkulProjectWebUI.SubscribeTableDependencies
{
    public class SubscribeProductTableDependency : ISubscribeTableDependency
    {
        SqlTableDependency<TBL_MESSAGE> tableDependency;
        DashboardHub dashboardHub;
        EYOkulDbContext _context;
        public SubscribeProductTableDependency(DashboardHub dashboardHub)
        {
            this.dashboardHub = dashboardHub;
        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<TBL_MESSAGE>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();
        }

        private async void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<TBL_MESSAGE> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                await dashboardHub.SendProducts();
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(TBL_MESSAGE)} SqlTableDependency error: {e.Error.Message}");
        }
    }
}
