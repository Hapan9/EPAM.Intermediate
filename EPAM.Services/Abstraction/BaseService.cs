using AutoMapper;
using EPAM.EF.UnitOfWork.Interfaces;
using Microsoft.Extensions.Logging;

namespace EPAM.Services.Abstraction
{
    public abstract class BaseService<T>
    {
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;
        protected readonly ILogger<T> Logger;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<T> logger)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
            Logger = logger;
        }
    }
}
