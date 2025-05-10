using AutoMapper;
using Business.Dtos;
using Bussiness.Common;
using Common;
using Common.DomainModels;
using Common.Repositories;
using Common.Utils;
using System.Linq.Expressions;


namespace Bussiness.Generic
{
    public class GenericDtoManager<T, TDto> : 
        GenericManager<T> where T : class, IDomainModel where TDto : BaseDto        
    {
        private ICurrentUserProvider currentUserProvider;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IMapper mapper;
      

        public GenericDtoManager(ICurrentUserProvider currentUserProvider, 
            IUnitOfWorkFactory unitOfWorkFactory, 
            IRepositoryFactory repositoryFactory, 
            IMapper mapper
         
            ) : base(currentUserProvider, unitOfWorkFactory, repositoryFactory)
        {
            ArgumentNullException.ThrowIfNull(currentUserProvider, nameof(currentUserProvider));
            ArgumentNullException.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentNullException.ThrowIfNull(repositoryFactory, nameof(repositoryFactory));

            this.currentUserProvider = currentUserProvider;
            this.repositoryFactory = repositoryFactory;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.mapper = mapper;
        }

        public IEnumerable<TDto> GetAllItems(params Expression<Func<T, object>>[] includeProperties)
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                var repo = repositoryFactory.GetNew<T>(unitOfWork);

                var products = repo.GetIncluding(null, includeProperties.ToList());   

                return products.Select(it => this.mapper.Map<TDto>(it)).ToList();
            }
        }

        public IEnumerable<TDto> GetAllItems(IEnumerable<Expression<Func<T, bool>>> criteriaList, params Expression<Func<T, object>>[] includeProperties)
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                var repo = repositoryFactory.GetNew<T>(unitOfWork);

                var products = repo.GetIncluding(criteriaList, includeProperties.ToList());

                return products.Select(it => this.mapper.Map<TDto>(it)).ToList();
            }
        }


        protected virtual void BeforeUpdate(IUnitOfWork unitOfWork,T model)
        {
        }

        protected virtual void AfterUpdate(IUnitOfWork unitOfWork,T model)
        {
        }

        public void Update(TDto dtoModel)
        {
            ArgumentNullException.ThrowIfNull(dtoModel, nameof(dtoModel));

            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                unitOfWork.BeginTransaction();
                var repo = repositoryFactory.GetNew<T>(unitOfWork);

                var model = repo.Get(dtoModel.Id);
                var tsCheck = new ArrayUtils();
                if(!tsCheck.IsEqual(model.TS,dtoModel.TS))
                {
                    throw new BusinessException($"Concurrency error, The {model.GetType().Name}" +
                        $" with the id: {model.Id} was modified from another source!");
                }
                mapper.Map<TDto, T>(dtoModel, model);

                this.BeforeUpdate(unitOfWork,model);
                repo.Update(model);
                this.AfterUpdate(unitOfWork,model);

                unitOfWork.SaveChanges();
                unitOfWork.CommitTransaction();
            }
        }

        protected virtual void BeforeAdd(IUnitOfWork unitOfWork, T model) { }

        protected virtual void AfterAdd(IUnitOfWork unitOfWork, T model)
        {
        }
        public TDto Add(TDto modelDto)
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                unitOfWork.BeginTransaction();
                var repo = repositoryFactory.GetNew<T>(unitOfWork);
                var modelToAdd = mapper.Map<T>(modelDto);
                repo.Add(modelToAdd);

                this.BeforeAdd(unitOfWork,modelToAdd);
                unitOfWork.SaveChanges();
                this.AfterAdd(unitOfWork,modelToAdd);

                unitOfWork.CommitTransaction();

                return mapper.Map<TDto>(modelToAdd);
            }
        }

        public TDto GetById(int id)
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                unitOfWork.BeginTransaction();
                var repo = repositoryFactory.GetNew<T>(unitOfWork);
                var model = repo.Get(id);
                var result = mapper.Map<TDto>(model);
                return result;
            }
        }
    }
}

