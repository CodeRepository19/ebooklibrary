using EbookDomain.Models;
using System.Collections.Generic;

namespace EbookDomain.Interfaces
{
    public interface IEventRepository
    {
        /// <summary>
        /// This is the Method to get all the Events Uploaded
        /// </summary>
        /// <returns>It is List Of Events objects</returns>
        IEnumerable<Event> GetEvents();
        /// <summary>
        /// This Method Find the Event Based on its it ID
        /// </summary>
        /// <param name="eventId">This is parameter to find the Event</param>
        /// <returns>This Method Returns a Event Object</returns>
        Event GetEventDetailsById(int eventId);
        /// <summary>
        /// This Method Will add the supplied Event Model Object to the Events collection
        /// </summary>
        /// <param name="eventDetails">eventDetails is an object of Event type which is taken as Input from Presetation Layer</param>
        /// <returns>This is the Result Event object</returns>
        Event Add(Event eventDetails);
        /// <summary>
        /// This Method Will Update the supplied Event Model Object to the Events collection
        /// </summary>
        /// <param name="eventDetails">eventDetails is the Modified Event Object which will be supplied from Presentaion Layer</param>
        /// <returns>This is the Updated Event object<</returns>
        Event Update(Event eventDetails);
        /// <summary>
        /// This Method is us used to Delete the Event from Event Table based on its eventId
        /// </summary>
        /// <param name="eventId">eventId is the parameter based on which book will be deleted</param>
        /// <returns>This is the Deleted Event object<</returns>
        Event Delete(int eventId);
    }
}
