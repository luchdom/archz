namespace Archz.SharedKernel.SeedWork;
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
