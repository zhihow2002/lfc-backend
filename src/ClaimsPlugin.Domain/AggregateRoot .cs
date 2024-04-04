// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace ClaimsPlugin.Domain
// {
//     public class AggregateRoot
//     {
//           private readonly List<IEvent> events = new List<IEvent>();

//         public IReadOnlyList<IEvent> Events => this.events;

//         public void ClearEvents()
//         {
//             this.events.Clear();
//         }

//         protected void AddEvent(IEvent @event)
//         {
//             this.events.Add(@event);
//         }
//     }
// }
