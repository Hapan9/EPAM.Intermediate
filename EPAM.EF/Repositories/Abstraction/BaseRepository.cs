namespace EPAM.EF.Repositories.Abstraction
{
    public class BaseRepository
    {
        protected readonly SystemContext Context;

        public BaseRepository(SystemContext context)
        {
            Context = context;
        }
    }
}
