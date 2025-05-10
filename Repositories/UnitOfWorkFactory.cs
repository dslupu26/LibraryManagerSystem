using Common.Repositories;
using Microsoft.Extensions.Configuration;

namespace Repositories
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {     
        private readonly IConfiguration configuration;

        

        public UnitOfWorkFactory(IConfiguration configuration) 
        {            
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

            this.configuration = configuration;    
        }

        public IUnitOfWork GetNew()
        {
             return (IUnitOfWork)Activator.CreateInstance(typeof (LibraryContext), this.configuration);
        }
    }
}
