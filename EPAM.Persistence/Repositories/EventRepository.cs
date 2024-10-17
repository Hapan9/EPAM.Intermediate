using Dapper;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Abstraction;
using EPAM.Persistence.Repositories.Interfaces;
using System.Data;

namespace EPAM.Persistence.Repositories
{
    public sealed class EventRepository : BaseRepository, IRepository<Event>
    {
        public EventRepository(IDbConnection dbConnection, IDbTransaction? dbTransaction) : base(dbConnection, dbTransaction)
        {
        }

        public async Task CreateAsync(Event entity)
        {
            #region sql
            const string Sql = @"
INSERT INTO [DbF].[Events]
    ([Name], [Date], [VenueId])
VALUES
    (@name, @date, @venueId)
";
            #endregion

            var param = new
            {
                entity.Name,
                entity.Date,
                entity.VenueId
            };

            await DbConnection.QueryAsync(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id)
        {
            #region sql
            const string Sql = @"
DELETE FROM
    [DbF].[Events]
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

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            #region sql
            const string Sql = @"
SELECT
    [Id],
    [Name],
    [Date],
    [VenueId]
FROM 
    [DbF].[Events]
";
            #endregion

            return await DbConnection.QueryAsync<Event>(Sql, null, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }

        public async Task<Event> GetAsync(Guid id)
        {
            #region sql
            const string Sql = @"
SELECT
    [Id],
    [Name],
    [Date],
    [VenueId]
FROM 
    [DbF].[Events]
WHERE
    [Id] = @id 
";
            #endregion

            var result = await DbConnection.QueryAsync<Event>(Sql, null, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
            return result.First();
        }

        public async Task UpdateAsync(Event entity)
        {
            #region sql
            const string Sql = @"
UPADTE
    [DbF].[Events]
SET
    [Name] = @name,
    [Date] = @date,
    [VenueId] = @venueId
WHERE
    [Id] = @id
";
            #endregion

            var param = new
            {
                entity.Id,
                entity.Name,
                entity.Date,
                entity.VenueId
            };

            await DbConnection.QueryAsync(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }
    }
}
