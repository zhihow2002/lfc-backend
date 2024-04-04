// using ClaimsPlugin.Domain;
// using ClaimsPlugin.Domain.Events;

// namespace ClaimsPlugin.Infrastructure.Extensions
// {
//     internal static class MediatorExtensions
//     {
//          public static async Task DispatchDomainEventsAsync(this IMediator mediator, BackendDbContext context)
//         {
//             var domainEntities = context.ChangeTracker
//                 .Entries<AggregateRoot>()
//                 .Where(x => x.Entity.Events != null && x.Entity.Events.Any());

//             var domainEvents = domainEntities
//                 .SelectMany(x => x.Entity.Events)
//                 .Where(x => x is IDomainEvent)
//                 .ToList();

//             domainEntities.ToList()
//                 .ForEach(entity => entity.Entity.ClearEvents());

//             foreach (var domainEvent in domainEvents)
//             {
//                 await mediator.Publish(domainEvent);
//             }
//         }
//         public static async Task DispatchIntegrationEventsAsync(this IMediator mediator, BackendDbContext ctx)
//         {
//             var domainEntities = ctx.ChangeTracker
//                 .Entries<AggregateRoot>()
//                 .Where(x => x.Entity.Events != null && x.Entity.Events.Any());


//             var integrationEvents = domainEntities
//                 .SelectMany(x => x.Entity.Events)
//                 .Where(x => x is IIntegrationEvent)
//                 .ToList();

//             domainEntities.ToList()
//                 .ForEach(entity => entity.Entity.ClearEvents());

//             foreach (var integrationEvent in integrationEvents)
//             {
//                 try
//                 {
//                     await mediator.Publish(integrationEvent);
//                 }
//                 catch (Exception)
//                 {
//                     // Fail silently.
//                 }
//             }
//         }

//     }
// }
