using Dapper;
using EPAM.EF.Entities;
using EPAM.Persistence.Repositories.Abstraction;
using EPAM.Persistence.Repositories.Interfaces;
using System.Data;

namespace EPAM.Persistence.Repositories
{
    public sealed class SeatRepository : BaseRepository, IRepository<Seat>
    {
        public SeatRepository(IDbConnection dbConnection, IDbTransaction? dbTransaction) : base(dbConnection, dbTransaction)
        {

        }

        public async Task CreateAsync(Seat entity)
        {
            #region sql
            const string Sql = @"
INSERT INTO [DbF].[Seats]
    ([Number], [RawId])
VALUES
    (@number, @rawId)
";
            #endregion

            var param = new
            {
                entity.Number,
                entity.RawId
            };

            await DbConnection.QueryAsync(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id)
        {
            #region sql
            const string Sql = @"
DELETE FROM
    [DbF].[Seats]
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

        public async Task<IEnumerable<Seat>> GetAllAsync()
        {
            #region sql
            const string Sql = @"
SELECT
    [Id],
    [Number],
    [RawId]
FROM 
    [DbF].[Seats]
";
            #endregion

            return await DbConnection.QueryAsync<Seat>(Sql, null, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }

        public async Task<Seat> GetAsync(Guid id)
        {
            #region sql
            const string Sql = @"
SELECT
    [Id],
    [Number],
    [RawId]
FROM 
    [DbF].[Seats]
WHERE
    [Id] = @id 
";
            #endregion

            var param = new
            {
                id
            };

            var result = await DbConnection.QueryAsync<Seat>(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
            return result.First();
        }

        public async Task UpdateAsync(Seat entity)
        {
            #region sql
            const string Sql = @"
UPADTE
    [DbF].[Seats]
SET
    [Number] = @number
    [RawId] = @rawId
WHERE
    [Id] = @id
";
            #endregion

            var param = new
            {
                entity.Id,
                entity.Number,
                entity.RawId
            };

            await DbConnection.QueryAsync(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }
    }
}
