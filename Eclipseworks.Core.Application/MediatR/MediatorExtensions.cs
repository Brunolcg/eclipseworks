namespace MediatR;

public static class MediatorExtensions
{
    public static async Task PublishDomainEventsAsync<T>(this IMediator mediator, Queue<T> domainEvents, CancellationToken cancellationToken = default)
        where T : DomainEvent
    {
        while (domainEvents.Any())
            await mediator.Publish(domainEvents.Dequeue(), cancellationToken);
    }
}