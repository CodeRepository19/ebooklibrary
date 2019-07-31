using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EbookApplication.ViewModels;
using EbookDomain.Interfaces;
using EbookDomain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EbookUI.Controllers
{
    public class EventsController : Controller
    {
        private readonly IHostingEnvironment objHostingEnvironment;
        private readonly IEventRepository objRepository;

        public EventsController(IHostingEnvironment hostingEnvironment, IEventRepository repositoty)
        {
            this.objHostingEnvironment = hostingEnvironment;
            objRepository = repositoty;
        }

        public IActionResult Index()
        {
            var EVM = new EventViewModel();
            EVM.Events = objRepository.GetEvents().OrderByDescending(s => s.EventDate);
            return View(EVM);
        }

        //// GET: Courses/Details/5
        //public IActionResult Details(int Id)
        //{
        //    if (Id == 0)
        //        return NotFound();

        //    EventViewModel objEventDetails = GetEventDetails(Id);
        //    if (objEventDetails == null)
        //        return NotFound();
        //    else
        //    {
        //        return View(objEventDetails);
        //    }
        //}

        private EventViewModel GetEventDetails(int intEventId)
        {
            EventViewModel objEventDetails = null;
            var eventDetails = objRepository.GetEventDetailsById(intEventId);
            if (eventDetails == null)
                return objEventDetails;

            if (eventDetails != null)
            {
                objEventDetails = new EventViewModel
                {
                    events = new Event()
                };

                objEventDetails.events.EventId = eventDetails.EventId;
                objEventDetails.events.EventName = eventDetails.EventName;
                objEventDetails.events.EventDate = eventDetails.EventDate;
                objEventDetails.events.EventDescription = eventDetails.EventDescription;
                objEventDetails.events.CreatedBy = eventDetails.CreatedBy;
                objEventDetails.events.CreatedDate = eventDetails.CreatedDate;
            }

            return objEventDetails;
        }

        // GET: Events/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventViewModel objeventDetails) //[Bind("BookId,BookName,Description,ImageUrl,TechnologyId")] Book book)
        {
            if (!objRepository.GetEvents().Any(a => a.EventName == objeventDetails.events.EventName && a.EventDate == objeventDetails.events.EventDate))
            {
                Event objNewEvent = null;
                if (ModelState.IsValid)
                {
                    objNewEvent = new Event
                    {
                        EventName = objeventDetails.events.EventName != null ? objeventDetails.events.EventName.Trim() : objeventDetails.events.EventName,
                        EventDescription = objeventDetails.events.EventDescription != null ? objeventDetails.events.EventDescription.Trim() : objeventDetails.events.EventDescription,
                        CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier),
                        CreatedDate = DateTime.Now,
                    };

                    objRepository.Add(objNewEvent);
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Event already exists.");
            }

            return View();
        }

        // GET: Events/Edit/5
        [Authorize]
        public IActionResult Edit(int Id)
        {
            if (Id == 0)
                return NotFound();

            EventViewModel objNewEvent = GetEventDetails(Id);

            if (objNewEvent != null)
            {
                return View(objNewEvent);
            }
            else
                return NotFound();
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EventViewModel objeventDetails)
        {
            if (id != objeventDetails.events.EventId)
                return NotFound();

            if (!EventExists(objeventDetails))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Event eventObj = objRepository.GetEvents().Where(x => x.EventId == objeventDetails.events.EventId).FirstOrDefault();

                        if (eventObj != null)
                        {
                            eventObj.EventId = objeventDetails.events.EventId;
                            eventObj.EventName = objeventDetails.events.EventName != null ? objeventDetails.events.EventName.Trim() : objeventDetails.events.EventName;
                            eventObj.EventDate = objeventDetails.events.EventDate;
                            eventObj.EventDescription = objeventDetails.events.EventDescription != null ? objeventDetails.events.EventDescription.Trim() : objeventDetails.events.EventDescription;
                            eventObj.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                            eventObj.CreatedDate = objeventDetails.events.CreatedDate;                            
                        };

                        objRepository.Update(eventObj);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EventExist(objeventDetails.events.EventId))
                            return NotFound();
                        else
                            throw;
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Book already exists.");
            }

            return View(objeventDetails);
        }

        private bool EventExist(int intEventId)
        {
            var ExistingEventDetails = objRepository.GetEventDetailsById(intEventId);
            if (ExistingEventDetails != null)
                return true;
            else
                return false;
        }

        private bool EventExists(EventViewModel objeventDetails)
        {
            var Events = from m in objRepository.GetEvents() where (m.EventName == objeventDetails.events.EventName && m.EventDate == objeventDetails.events.EventDate && m.EventId != objeventDetails.events.EventId) select m;
            if (Events.ToList().Count > 0)
                return true;
            else
                return false;
        }

    }
}