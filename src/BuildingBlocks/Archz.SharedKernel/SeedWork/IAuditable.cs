namespace Archz.SharedKernel.SeedWork;
public interface IAuditable
{
    DateTime CreatedOnUtc { get; }

    DateTime? ModifiedOnUtc { get; }
}
