using EYOkulProjectWebUI.Models;
using Microsoft.EntityFrameworkCore;

namespace EYOkulProjectWebUI.DAL
{
    public class EYOkulDbContext : DbContext
    {
            public DbSet<GuardiansModel> TBL_CARDS { get; set; }
            public DbSet<StudentsModel> TBL_STUDENTS { get; set; }
            public DbSet<ClassModel> TBL_CLASS { get; set; }
            public DbSet<ScoolModel> TBL_SCOOLS { get; set; }
            public DbSet<TransactionsModel> TBL_TRANSACTIONS { get; set; }
            public DbSet<TerminalModel> TBL_TERMINALS { get; set; }
            public DbSet<UserModel> TBL_A_USERS { get; set; }
            public DbSet<UserTypeModel> TBL_TYPES { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {

                //optionsBuilder.UseSqlServer("Server =DESKTOP-OIDPVCJ;Database =YeDaSis;User Id=sa; Password=wsxEDCrfv123; TrustServerCertificate=true;");
                optionsBuilder.UseSqlServer("Server =176.33.15.4;Database =EYOkulProject;User Id=enes; Password=wsxEDCrfv123; TrustServerCertificate=true;");



                //    Server =DESKTOP-3PAJ411; Database = NORTHWND; User Id = sa; Password = wsxEDCrfv123;
            }
        

    }
}
