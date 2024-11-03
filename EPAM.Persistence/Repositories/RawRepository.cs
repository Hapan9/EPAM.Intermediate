using Dapper;
using EPAM.Persistence.Entities;
using EPAM.Persistence.Repositories.Abstraction;
using System.Data;

namespace EPAM.Persistence.Repositories
{
    public sealed class RawRepository : BaseRepository//, IRepository<Raw>
    {
        public RawRepository(IDbConnection dbConnection, IDbTransaction? dbTransaction) : base(dbConnection, dbTransaction)
        {
        }

        public async Task CreateAsync(Raw entity, CancellationToken cancellationToken)
        {
            #region sql
            const string Sql = @"
INSERT INTO [DbF].[Raws]
    ([Number], [SectionId])
VALUES
    (@number, @sectionId)
";
            #endregion

            var param = new
            {
                entity.Number,
                entity.SectionId
            };

            await DbConnection.QueryAsync(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            #region sql
            const string Sql = @"
DELETE FROM
    [DbF].[Raws]
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

        public async Task<IEnumerable<Raw>> GetAllAsync(CancellationToken cancellationToken)
        {
            #region sql
            const string Sql = @"
SELECT
    [Id],
    [Number],
    [SectionId]
FROM 
    [DbF].[Raws]
";
            #endregion

            return await DbConnection.QueryAsync<Raw>(Sql, null, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }

        public async Task<Raw> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            #region sql
            const string Sql = @"
SELECT
    [Id],
    [Number],
    [SectionId]
FROM 
    [DbF].[Raws]
WHERE
    [Id] = @id 
";
            #endregion

            var param = new
            {
                id
            };

            var result = await DbConnection.QueryAsync<Raw>(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
            return result.First();
        }

        public async Task UpdateAsync(Raw entity, CancellationToken cancellationToken)
        {
            #region sql
            const string Sql = @"
UPADTE
    [DbF].[Raws]
SET
    [Number] = @number
    [SectionId] = @sectionId
WHERE
    [Id] = @id
";
            #endregion

            var param = new
            {
                entity.Id,
                entity.Number,
                entity.SectionId
            };

            await DbConnection.QueryAsync(Sql, param, DbTransaction, Timeout, CommandType.Text).ConfigureAwait(false);
        }
    }
}
