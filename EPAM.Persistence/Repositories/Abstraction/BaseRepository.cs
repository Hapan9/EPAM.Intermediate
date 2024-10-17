using System.Data;

namespace EPAM.Persistence.Repositories.Abstraction
{
    public abstract class BaseRepository
    {
        protected readonly IDbConnection DbConnection;
        protected readonly IDbTransaction? DbTransaction;
        protected const int Timeout = 300;

        //protected IDbConnection DbConnection
        //{
        //    get
        //    {
        //        if(_dbTransaction == null)
        //        {
        //            return _dbConnection;
        //        }

        //        if(_dbTransaction.Connection == null)
        //        {
        //            return _dbConnection;
        //        }

        //        return _dbTransaction.Connection;
        //    }
        //}

        protected BaseRepository(IDbConnection dbConnection, IDbTransaction? dbTransaction)
        {
            DbConnection = dbConnection;
            DbTransaction = dbTransaction;
        }
    }
}
