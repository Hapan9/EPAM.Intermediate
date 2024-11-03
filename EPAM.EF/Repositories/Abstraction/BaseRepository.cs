namespace EPAM.EF.Repositories.Abstraction
{
    public abstract class BaseRepository
    {
        protected readonly SystemContext Context;

        public BaseRepository(SystemContext context)
        {
            Context = context;
        }
    }
}
