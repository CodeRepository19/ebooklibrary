using EbookApplication.interfaces;
using EbookApplication.ViewModels;
using EbookDomain.Interfaces;

namespace EbookApplication.Services
{
    public class EventService : IEventService
    {
        private IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public EventViewModel GetEvents()
        {
            return new EventViewModel()
            {
                Events = _eventRepository.GetEvents()
            };
        }
    }
}