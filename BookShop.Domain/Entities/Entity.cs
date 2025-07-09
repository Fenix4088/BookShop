using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BookShop.Domain.Abstractions;

namespace BookShop.Domain.Entities;

public abstract class Entity
{
    [NotMapped]
    private readonly List<IDomainEvent> _domainEvents = new();
    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    protected void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}