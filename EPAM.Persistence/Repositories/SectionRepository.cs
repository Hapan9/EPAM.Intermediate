using Dapper;
using EPAM.EF.Entities;
using EPAM.Persistence.Repositories.Abstraction;
using EPAM.Persistence.Repositories.Interfaces;
using System.Data;

namespace EPAM.Persistence.Repositories
{
    public sealed class SectionRepository : BaseRepository, IRepository<Section>
    {
        public SectionRepository(IDbConnection dbConnection, IDbTransaction? dbTransaction) : base(dbConnection, dbTransaction)
        {
        }

        public async Task CreateAsync(Section entity)
        {
            #region sql
            const string Sql = @"
INSERT INTO [DbF].[Sections]
    ([Name], [VenueId])
VALUES
    (@name, @venueId)
";
            #endregion

            var param = new
            {
                entity.Name,
                entity.VenueId
            };

            await DbConnection.QueryAsync(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id)
        {
            #region sql
            const string Sql = @"
DELETE FROM
    [DbF].[Sections]
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

        public async Task<IEnumerable<Section>> GetAllAsync()
        {
            #region sql
            const string Sql = @"
SELECT
    [Id],
    [Name],
    [VenueId]
FROM 
    [DbF].[Sections]
";
            #endregion

            return await DbConnection.QueryAsync<Section>(Sql, null, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }

        public async Task<Section> GetAsync(Guid id)
        {
            #region sql
            const string Sql = @"
SELECT
    [Id],
    [Name],
    [VenueId]
FROM 
    [DbF].[Sections]
WHERE
    [Id] = @id 
";
            #endregion

            var result = await DbConnection.QueryAsync<Section>(Sql, null, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
            return result.First();
        }

        public async Task UpdateAsync(Section entity)
        {
            #region sql
            const string Sql = @"
UPADTE
    [DbF].[Sections]
SET
    [Name] = @name
    [VenueId] = @venueId
WHERE
    [Id] = @id
";
            #endregion

            var param = new
            {
                entity.Id,
                entity.Name,
                entity.VenueId
            };

            await DbConnection.QueryAsync(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }
    }
}
