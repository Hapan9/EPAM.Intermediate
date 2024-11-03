using Dapper;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Abstraction;
using System.Data;

namespace EPAM.Persistence.Repositories
{
    public sealed class VenueRepository : BaseRepository//, IRepository<Venue>
    {
        public VenueRepository(IDbConnection dbConnection, IDbTransaction? dbTransaction) : base(dbConnection, dbTransaction)
        {

        }

        public async Task CreateAsync(Venue entity, CancellationToken cancellationToken)
        {
            #region sql
            const string Sql = @"
INSERT INTO [DbF].[Venues]
    ([Name])
VALUES
    (@name)
";
            #endregion

            var param = new
            {
                entity.Name
            };

            await DbConnection.QueryAsync(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            #region sql
            const string Sql = @"
DELETE FROM
    [DbF].[Venues]
WHERE
    [Id] = @id
";
            #endregion

            var param = new
            {
                id
            };

            await DbConnection.QueryAsync(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Venue>> GetAllAsync(CancellationToken cancellationToken)
        {
            #region sql
            const string Sql = @"
SELECT
    [Id],
    [Name]
FROM
    [DbF].[Venues]
";
            #endregion

            return await DbConnection.QueryAsync<Venue>(Sql, null, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }

        public async Task<Venue> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            #region sql
            const string Sql = @"
SELECT
    [Id],
    [Name]
FROM
    [DbF].[Venues]
WHERE
    [Id] = @id
";
            #endregion


            var param = new
            {
                id
            };

            var result = await DbConnection.QueryAsync<Venue>(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
            return result.First();
        }

        public async Task UpdateAsync(Venue entity, CancellationToken cancellationToken)
        {
            #region sql
            const string Sql = @"
UPADTE
    [DbF].[Venues]
SET
    [Name] = @name
WHERE
    [Id] = @id
";
            #endregion

            var param = new
            {
                entity.Id,
                entity.Name
            };

            await DbConnection.QueryAsync(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }
    }
}
