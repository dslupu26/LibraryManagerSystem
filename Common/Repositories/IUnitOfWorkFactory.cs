
namespace Common.Repositories
{
    public interface IUnitOfWorkFactory
    {
        public IUnitOfWork GetNew();
    }
}
