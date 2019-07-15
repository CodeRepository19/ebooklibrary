using EbookApplication.ViewModels;
using EbookDomain.Interfaces;
using EbookDomain.Models;
using EbookInfraData.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EbookUI.Controllers
{
    public class BooksController : Controller
    {
        //private readonly ebooklibraryDBcontext _context;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IBookRepository _ctx;

        //public BooksController(ebooklibraryDBcontext context, IHostingEnvironment hostingEnvironment, IBookRepository ctx)
        //{
        //    _context = context;
        //    this.hostingEnvironment = hostingEnvironment;
        //    _ctx = ctx;
        //}

        public BooksController(IHostingEnvironment hostingEnvironment, IBookRepository ctx)
        {
            this.hostingEnvironment = hostingEnvironment;
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            return View(_ctx.GetBooks());
        }

        // GET: Courses/Details/5
        public IActionResult Details(int id)
        {
            BookViewModel objNewBook = null;

            if (id == 0)
            {
                return NotFound();
            }

            objNewBook = getDetails(id);

            return View(objNewBook);
        }

        private BookViewModel getDetails(int id)
        {
            BookViewModel objNewBook = null;
            var bookDetails = _ctx.GetBookDetailsById(id);            
            var technologyDetails = _ctx.GetTechnologyDetailsById(bookDetails.TechnologyId);
            
            if (bookDetails != null && technologyDetails != null)
            {
                objNewBook = new BookViewModel();
                objNewBook.book = new Book();
                objNewBook.technology = new Technology();
                objNewBook.book.BookId = bookDetails.BookId;
                objNewBook.book.BookName = bookDetails.BookName;
                objNewBook.book.Description = bookDetails.Description;
                objNewBook.book.ImageUrl = bookDetails.ImageUrl;
                objNewBook.book.TechnologyId = bookDetails.TechnologyId;
                objNewBook.technology.TechnologyName = technologyDetails.TechnologyName;
                objNewBook.book.ImageUrl = bookDetails.ImageUrl;
                objNewBook.ExistingImageUrl = bookDetails.ImageUrl;
            }

            return objNewBook;
        }
        // GET: Courses/Create
        [Authorize]
        public IActionResult Create()
        {
            lstTechnology();
            return View();
        }

        private IEnumerable<Technology> lstTechnology()
        {
            ViewBag.technologyList = _ctx.GetTechnologys();
            return _ctx.GetTechnologys();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel objbookDetails) //[Bind("BookId,BookName,Description,ImageUrl,TechnologyId")] Book book)
        {
            Book objNewBook = null;
            if (ModelState.IsValid)
            {
                string uniqueFileNmae = ProcessUploadFile(objbookDetails);
                objNewBook = new Book
                {
                    BookName = objbookDetails.book.BookName,
                    Description = objbookDetails.book.Description,
                    TechnologyId = objbookDetails.book.TechnologyId,
                    ImageUrl = uniqueFileNmae
                };

                _ctx.Add(objNewBook);
                return RedirectToAction(nameof(Index));
            }

            return View(objNewBook);
        }

        private string ProcessUploadFile(BookViewModel objbookDetails)
        {
            string uniqueFileNmae = null;
            if (objbookDetails.Image != null)
            {
                string UploadFolders = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileNmae = Guid.NewGuid().ToString() + "_" + objbookDetails.Image.FileName;
                string filePath = Path.Combine(UploadFolders, uniqueFileNmae);
                using (var FileStream = new FileStream(filePath, FileMode.Create))
                {
                    objbookDetails.Image.CopyTo(FileStream);
                }
            }

            return uniqueFileNmae;
        }

        // GET: books/Edit/5
        [Authorize]
        public IActionResult Edit(int id)
        {
            BookViewModel objNewBook = null;
            lstTechnology();
            if (id == 0)
            {
                return NotFound();
            }

            objNewBook = getDetails(id);
            
            return View(objNewBook);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookViewModel objbookDetails)
        {
            if (id != objbookDetails.book.BookId)
            {
                return NotFound();
            }

            Book objEditBook = null;
            if (ModelState.IsValid)
            {
                try
                {
                    if (objbookDetails.Image != null)
                    {
                        if (objbookDetails.ExistingImageUrl != null)
                        {
                            string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", objbookDetails.ExistingImageUrl);
                            System.IO.File.Delete(filePath);
                        }

                        objbookDetails.book.ImageUrl = ProcessUploadFile(objbookDetails);
                    }
                    else
                        objbookDetails.book.ImageUrl = objbookDetails.ExistingImageUrl;

                    objEditBook = new Book
                    {
                        BookId = objbookDetails.book.BookId,
                        BookName = objbookDetails.book.BookName,
                        Description = objbookDetails.book.Description,
                        TechnologyId = objbookDetails.book.TechnologyId,
                        ImageUrl = objbookDetails.book.ImageUrl
                    };

                    _ctx.Update(objEditBook);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(objbookDetails.book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(objbookDetails.book);
        }

        private bool BookExists(int id)
        {
            var details = _ctx.GetBookDetailsById(id);
            if (details != null)
                return true;
            else
                return false;
        }
    }
}