using EPAM.Persistence.UnitOfWork.Interfaces;
using System.Data;

namespace EPAM.Persistence.UnitOfWork
{
    public sealed class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDbConnection _dbConnection;
        private IUnitOfWork _unitOfWork;

        public UnitOfWorkFactory(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IUnitOfWork Create()
        {
            //_unitOfWork = new UnitOfWork(_dbConnection);
            _dbConnection.Open();
            return _unitOfWork;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
            _dbConnection.Dispose();
        }
    }
}
