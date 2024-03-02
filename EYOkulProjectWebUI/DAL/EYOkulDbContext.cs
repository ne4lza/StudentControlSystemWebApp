using EYOkulProjectWebUI.Models;
using Microsoft.EntityFrameworkCore;

namespace EYOkulProjectWebUI.DAL
{
    public class EYOkulDbContext : DbContext
    {
        public EYOkulDbContext()
        {
        }

        public EYOkulDbContext(DbContextOptions<EYOkulDbContext> options)
            : base(options)
        {
        }
        public DbSet<CardModel> TBL_CARDS { get; set; }
        public DbSet<StudentsModel> TBL_STUDENTS { get; set; }
        public DbSet<ClassModel> TBL_CLASS { get; set; }
        public DbSet<ScoolModel> TBL_SCOOLS { get; set; }
        public DbSet<TransactionsModel> TBL_TRANSACTIONS { get; set; }
        public DbSet<TerminalModel> TBL_TERMINALS { get; set; }
        public DbSet<UserModel> TBL_A_USERS { get; set; }
        public DbSet<UserTypeModel> TBL_TYPES { get; set; }
        public DbSet<TBL_MESSAGE> TBL_MESSAGE { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable(x => x.HasTrigger("TrackUserChanges"));
            modelBuilder.Entity<CardModel>().ToTable(x => x.HasTrigger("TrackCardChanges"));
            modelBuilder.Entity<ClassModel>().ToTable(x => x.HasTrigger("TrackClassChanges"));
            modelBuilder.Entity<StudentsModel>().ToTable(x => x.HasTrigger("TrackStudentChanges"));
            modelBuilder.Entity<TerminalModel>().ToTable(x => x.HasTrigger("TrackTerminalChanges"));
            base.OnModelCreating(modelBuilder);
        }


    }
}
