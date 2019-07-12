using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EbookApplication.ViewModels;
using EbookDomain.Interfaces;
using EbookDomain.Models;
using EbookInfraData.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EbookUI.Controllers
{
    public class BooksController : Controller
    {
        private readonly ebooklibraryDBcontext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public BooksController(ebooklibraryDBcontext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            BookViewModel objNewBook = null;

            if (id == null)
            {
                return NotFound();
            }

            var bookDetails = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (bookDetails == null)
            {
                return NotFound();
            }

            var technologyDetails = await _context.Technologys
                .FirstOrDefaultAsync(m => m.TechnologyId == bookDetails.TechnologyId);

            if (bookDetails != null && technologyDetails != null)
            {
                objNewBook = new BookViewModel();
                objNewBook.book = new Book();
                objNewBook.technology = new Technology();
                objNewBook.book.BookId = bookDetails.BookId;
                objNewBook.book.BookName = bookDetails.BookName;
                objNewBook.book.Description = bookDetails.Description;
                objNewBook.technology.TechnologyName = technologyDetails.TechnologyName;
                objNewBook.book.ImageUrl = bookDetails.ImageUrl;
            }

            return View(objNewBook);
        }

        // GET: Courses/Create
        [Authorize]
        public IActionResult Create()
        {
            lstTechnology();
            return View();
        }

        private List<Technology> lstTechnology()
        {
            BookViewModel objNewBook = new BookViewModel();
            List<Technology> objTechnology = new List<Technology>();
            objTechnology = (from x in _context.Technologys select x).ToList();
            objTechnology.Insert(0, new Technology { TechnologyId = 0, TechnologyName = "Select" });
            ViewBag.technologyList = objTechnology;
            return objTechnology;
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

                _context.Add(objNewBook);
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(int? id)
        {
            BookViewModel objNewBook = null;

            if (id == null)
            {
                return NotFound();
            }

            lstTechnology();

            var bookDetails = await _context.Books.FindAsync(id);
            if (bookDetails == null)
            {
                return NotFound();
            }

            if (bookDetails != null)
            {
                objNewBook = new BookViewModel();
                objNewBook.book = new Book();
                objNewBook.technology = new Technology();
                objNewBook.book.BookId = bookDetails.BookId;
                objNewBook.book.BookName = bookDetails.BookName;
                objNewBook.book.Description = bookDetails.Description;
                objNewBook.book.TechnologyId = bookDetails.TechnologyId;
                objNewBook.book.ImageUrl = bookDetails.ImageUrl;
                objNewBook.ExistingImageUrl = bookDetails.ImageUrl;
            }
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

                    _context.Update(objEditBook);
                    await _context.SaveChangesAsync();
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
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}