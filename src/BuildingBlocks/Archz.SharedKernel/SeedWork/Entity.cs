namespace Archz.SharedKernel.SeedWork;
public abstract class Entity
{
    public virtual int Id { get; protected set; }

    private List<IDomainEvent> _notifications = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _notifications.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent eventItem)
    {
        _notifications ??= new List<IDomainEvent>();
        _notifications.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
        _notifications?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _notifications?.Clear();
    }
}
