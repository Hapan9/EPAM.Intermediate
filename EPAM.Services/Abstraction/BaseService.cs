using AutoMapper;
using EPAM.Persistence.UnitOfWork.Interface;
using Microsoft.Extensions.Logging;

namespace EPAM.Services.Abstraction
{
    public abstract class BaseService<T>
    {
        protected readonly IUnitOfWorkFactory UnitOfWorkFactory;
        protected readonly IMapper Mapper;
        protected readonly ILogger<T> Logger;

        public BaseService(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper, ILogger<T> logger)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
            Mapper = mapper;
            Logger = logger;
        }
    }
}
