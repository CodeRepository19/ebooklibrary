using EbookDomain.Interfaces;
using EbookDomain.Models;
using EbookInfraData.Context;
using System.Collections.Generic;

namespace EbookInfraData.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly ebooklibraryDBcontext _ctx;

        public EventRepository(ebooklibraryDBcontext ctx)
        {
            this._ctx = ctx;
        }

        public Event Add(Event eventDetails)
        {
            _ctx.Events.Add(eventDetails);
            _ctx.SaveChanges();
            return eventDetails;
        }

        public Event Delete(int eventId)
        {
            Event events = _ctx.Events.Find(eventId);
            if (events != null)
            {
                _ctx.Events.Remove(events);
                _ctx.SaveChanges();
            }

            return events;
        }

        public Event GetEventDetailsById(int eventId)
        {
            return _ctx.Events.Find(eventId);
        }

        public IEnumerable<Event> GetEvents()
        {
            return _ctx.Events;
        }

        public Event Update(Event eventDetails)
        {
            _ctx.Update(eventDetails);
            _ctx.SaveChanges();
            return eventDetails;
        }
    }
}
