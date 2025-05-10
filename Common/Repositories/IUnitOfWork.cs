namespace Common.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        public void SaveChanges();
        public void BeginTransaction();
        public void CommitTransaction();
        public void ForceCreate();

        public void DropTable();
        
    }
}
