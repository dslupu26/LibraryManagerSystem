using DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Common.Repositories;


namespace Repositories
{
    public class LibraryContext: DbContext, IUnitOfWork
    {
        private readonly IConfiguration configuration;
     

        public LibraryContext(IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

            this.configuration = configuration;
            this.ChangeTracker.AutoDetectChangesEnabled = false;
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Rent> Rents { get; set; }
        

        public void DropTable()
        {
            var queries = SQLInstructionProvider.GetQueries("DropTables.sql");

            foreach (var query in queries)
            {
                try
                {
                    this.Database.ExecuteSqlRaw(query);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error executing: {query}. Ex: {ex.Message}");
                }
            }

        }

        public void BeginTransaction()
        {
            this.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            this.Database.CommitTransaction();
        }

        public void ForceCreate()
        {
            this.Database.EnsureCreated();
            this.SaveChanges();
            this.ExecuteAdditionalStatementOnCreateDB();
        }

        private void ExecuteAdditionalStatementOnCreateDB()
        {
            var queries = SQLInstructionProvider.GetQueries("CreateDBAdditional.sql");

            foreach (var query in queries) 
            {
                try
                {
                    this.Database.ExecuteSqlRaw(query);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error executing: {query}. Ex: {ex.Message}");
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection"));            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().Property(e => e.TS).IsRowVersion();
            modelBuilder.Entity<Book>().Property(e => e.ModifiedBy).IsRequired(false);

            modelBuilder.Entity<Customer>().Property(e => e.TS).IsRowVersion();
            modelBuilder.Entity<Customer>().Property(e => e.ModifiedBy).IsRequired(false);            
            modelBuilder.Entity<Rent>().Property(e => e.TS).IsRowVersion();
            modelBuilder.Entity<Rent>().Property(e => e.ModifiedBy).IsRequired(false);
                        
            modelBuilder.Entity<Rent>()
                .HasOne(p => p.Book)
                .WithMany(p => p.Rents)
                .HasForeignKey(p => p.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rent>()
                 .HasOne(p => p.Customer)
                 .WithMany(p => p.Rents)    
                 .HasForeignKey(p => p.CustomerId)
                 .OnDelete(DeleteBehavior.Restrict);
        }

        void IUnitOfWork.SaveChanges()
        {
            this.SaveChanges(true);
        }
    }
}
