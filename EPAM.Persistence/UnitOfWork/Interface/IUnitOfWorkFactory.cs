namespace EPAM.Persistence.UnitOfWork.Interface
{
    public interface IUnitOfWorkFactory : IDisposable
    {
        IUnitOfWork Create();
    }
}
