using EbookApplication.ViewModels;
using EbookDomain.Interfaces;
using EbookDomain.Models;
using IronPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EbookUI.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHostingEnvironment objHostingEnvironment;
        private readonly IBookRepository objRepositoty;

        public BooksController(IHostingEnvironment hostingEnvironment, IBookRepository repositoty)
        {
            this.objHostingEnvironment = hostingEnvironment;
            objRepositoty = repositoty;
        }

        public IActionResult Index()
        {
            return View(objRepositoty.GetBooks());
        }

        public async Task<IActionResult> SearchBook(string searchString)
        {
            var tehonlolgyId = from m in objRepositoty.GetTechnologys().Where(a => a.TechnologyName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1) select m.TechnologyId;

            if (tehonlolgyId.ToList().Count > 0)
            {
                int techID = Convert.ToInt32(tehonlolgyId.ToList()[0]);

                var Books = from m in objRepositoty.GetBooks().Where(a => a.TechnologyId == techID) select m;

                if (techID > 0)
                {
                    //  Books = Books.Where(s => s.BookName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
                    Books = Books.Where(s => s.TechnologyId == techID);
                }
                else
                    //return View("Error/500");
                    Index();

                if (Books.ToList().Count == 0)
                {
                    return RedirectToAction("HandleErrorCode",
                              "Error",
                              new { statusCode = 264 });
                }
                else
                    return await Task.FromResult(View("Index", Books.ToList()));
            }
            else
            {
                var Books = from m in objRepositoty.GetBooks() select m;
                if (!String.IsNullOrEmpty(searchString))
                {

                    Books = Books.Where(s => s.BookName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1);
                }
                else
                    //return View("Error/500");
                    Index();

                if (Books.ToList().Count == 0)
                {
                    return RedirectToAction("HandleErrorCode",
                              "Error",
                              new { statusCode = 264 });
                }
                else
                    return await Task.FromResult(View("Index", Books.ToList()));
            }


        }


        // GET: Courses/Details/5
        public IActionResult Details(int Id)
        {

            if (Id == 0)
                return NotFound();

            BookViewModel objBookDetails = GetBookDetails(Id);
            if (objBookDetails == null)
                return NotFound();
            else
                return View(objBookDetails);
        }

        private BookViewModel GetBookDetails(int intBookId)
        {
            BookViewModel objBookDetails = null;
            var bookDetails = objRepositoty.GetBookDetailsById(intBookId);
            if (bookDetails == null)
                return objBookDetails;

            var technologyDetails = objRepositoty.GetTechnologyDetailsById(bookDetails.TechnologyId);

            if (bookDetails != null && technologyDetails != null)
            {
                objBookDetails = new BookViewModel
                {
                    book = new Book(),
                    technology = new Technology()
                };

                objBookDetails.book.BookId = bookDetails.BookId;
                objBookDetails.book.BookName = bookDetails.BookName;
                objBookDetails.book.Description = bookDetails.Description;
                objBookDetails.book.ImageUrl = bookDetails.ImageUrl;
                objBookDetails.book.TechnologyId = bookDetails.TechnologyId;
                objBookDetails.technology.TechnologyName = technologyDetails.TechnologyName;
                objBookDetails.book.ImageUrl = bookDetails.ImageUrl;
                objBookDetails.ExistingImageUrl = bookDetails.ImageUrl;
            }

            return objBookDetails;
        }
        // GET: Courses/Create
        [Authorize]
        public IActionResult Create()
        {
            TechnologyList();
            return View();
        }

        private IEnumerable<Technology> TechnologyList()
        {
            ViewBag.technologyList = objRepositoty.GetTechnologys();
            return objRepositoty.GetTechnologys();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookViewModel objbookDetails) //[Bind("BookId,BookName,Description,ImageUrl,TechnologyId")] Book book)
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

                objRepositoty.Add(objNewBook);
                return RedirectToAction(nameof(Index));
            }

            TechnologyList();
            return View();
        }

        private string ProcessUploadFile(BookViewModel objbookDetails)
        {
            string uniquePdfFileName = null;
            string uniqueImageFileName = null;
            if (objbookDetails.Image != null)
            {
                string UploadBookFolder = Path.Combine(objHostingEnvironment.WebRootPath, "uploadBooks");
                string UploadImageFolder = Path.Combine(objHostingEnvironment.WebRootPath, "uploadImages");

                uniquePdfFileName = Guid.NewGuid().ToString() + "_" + objbookDetails.Image.FileName;

                var splitFileName = uniquePdfFileName.Split("_");
                uniqueImageFileName = splitFileName[0].ToString() + ".png";

                string filePath = Path.Combine(UploadBookFolder, splitFileName[0].ToString() + ".pdf");

                using (var FileStream = new FileStream(filePath, FileMode.Create))
                    objbookDetails.Image.CopyTo(FileStream);


                byte[] bytes = Convert.FromBase64String(objbookDetails.book.ImageUrl);

                Image pdfImage;

                using (MemoryStream ms = new MemoryStream(bytes))
                    pdfImage = Image.FromStream(ms);

                pdfImage.Save(UploadImageFolder + "\\" + uniqueImageFileName, System.Drawing.Imaging.ImageFormat.Png);

                //var pdf = PdfDocument.FromFile(filePath);
                //var opdf = pdf.CopyPage(0);
                // opdf.RasterizeToImageFiles(UploadImageFolder + "\\" + uniqueImageFileName, 250, 150);
            }

            return uniqueImageFileName;
        }

        // GET: books/Edit/5
        [Authorize]
        public IActionResult Edit(int Id)
        {
            TechnologyList();

            if (Id == 0)
                return NotFound();

            BookViewModel objNewBook = GetBookDetails(Id);

            if (objNewBook != null)
            {
                ViewBag.FileName = objNewBook.ExistingImageUrl;
                return View(objNewBook);
            }
            else
                return NotFound();

        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BookViewModel objbookDetails)
        {
            if (id != objbookDetails.book.BookId)
                return NotFound();


            if (ModelState.IsValid)
            {
                try
                {
                    if (objbookDetails.Image != null)
                    {
                        if (objbookDetails.ExistingImageUrl != null)
                        {
                            var URLPath = objbookDetails.ExistingImageUrl;
                            var splitPath = URLPath.Split(".");
                            var bookPath = splitPath[0].ToString() + ".pdf";

                            string imageFilePath = Path.Combine(objHostingEnvironment.WebRootPath, "uploadImages", objbookDetails.ExistingImageUrl);
                            string bookFilePath = Path.Combine(objHostingEnvironment.WebRootPath, "uploadBooks", bookPath);
                            System.IO.File.Delete(imageFilePath);
                            System.IO.File.Delete(bookFilePath);
                        }

                        objbookDetails.book.ImageUrl = ProcessUploadFile(objbookDetails);
                    }
                    else
                        objbookDetails.book.ImageUrl = objbookDetails.ExistingImageUrl;

                    Book objEditBook = new Book
                    {
                        BookId = objbookDetails.book.BookId,
                        BookName = objbookDetails.book.BookName,
                        Description = objbookDetails.book.Description,
                        TechnologyId = objbookDetails.book.TechnologyId,
                        ImageUrl = objbookDetails.book.ImageUrl
                    };

                    objRepositoty.Update(objEditBook);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(objbookDetails.book.BookId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(objbookDetails.book);
        }

        private bool BookExists(int intBookId)
        {
            var ExistingBookDetails = objRepositoty.GetBookDetailsById(intBookId);
            if (ExistingBookDetails != null)
                return true;
            else
                return false;
        }
    }
}