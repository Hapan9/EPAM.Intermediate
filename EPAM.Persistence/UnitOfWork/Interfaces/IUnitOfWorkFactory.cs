namespace EPAM.Persistence.UnitOfWork.Interfaces
{
    public interface IUnitOfWorkFactory : IDisposable
    {
        IUnitOfWork Create();
    }
}
