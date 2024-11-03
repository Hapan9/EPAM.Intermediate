using System.Data;

namespace EPAM.Persistence.Repositories.Abstraction
{
    public abstract class BaseRepository
    {
        protected readonly IDbConnection DbConnection;
        protected readonly IDbTransaction? DbTransaction;
        protected const int Timeout = 300;

        protected BaseRepository(IDbConnection dbConnection, IDbTransaction? dbTransaction)
        {
            DbConnection = dbConnection;
            DbTransaction = dbTransaction;
        }
    }
}
