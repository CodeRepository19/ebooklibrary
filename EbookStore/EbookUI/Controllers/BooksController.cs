using EbookApplication.ViewModels;
using EbookDomain.Interfaces;
using EbookDomain.Models;
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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace EbookUI.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHostingEnvironment objHostingEnvironment;
        private readonly IBookRepository objRepositoty;
        private readonly IEventRepository objEventRepository;

        public BooksController(IHostingEnvironment hostingEnvironment, IBookRepository repositoty, IEventRepository eventrepositoty)
        {
            this.objHostingEnvironment = hostingEnvironment;
            objRepositoty = repositoty;
            objEventRepository = eventrepositoty;
        }

        public IActionResult Home()
        {
            TempData["Home"] = "true";
            return RedirectToAction(nameof(Index));
        }

        public IEnumerable<Event> EventsList()
        {
            var objEventList = objEventRepository.GetEvents().OrderByDescending(s => s.EventDate);
            return objEventList;
        }

        public IActionResult Index()
        {
            var BVM = new BookViewModel();
            BVM.EventsList = EventsList();

            if ((string)TempData["Home"] == "false" || (string)TempData["Home"] == null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction(nameof(Books), BVM);
                }
                else
                {
                    BVM.ApprovedList = objRepositoty.GetBooks().Where(s => s.StatusId == 2).OrderByDescending(s => s.ApprovedDate);
                    ViewBag.approvedsubtitle = "Books";
                }
            }
            else
            {
                BVM.ApprovedList = objRepositoty.GetBooks().Where(s => s.StatusId == 2).OrderByDescending(s => s.ApprovedDate);
                ViewBag.approvedsubtitle = "Books";

                TempData["Home"] = "false";
            }

            return View(BVM);
        }

        public IActionResult GetApprovedBooks()
        {
            var BVM = new BookViewModel();
            BVM.ApprovedList = objRepositoty.GetBooks().Where(s => s.StatusId == 2).OrderByDescending(s => s.ApprovedDate);
            ViewBag.approvedsubtitle = "Books";

            return PartialView("_ApprovedBooksList", BVM);
        }

        public IActionResult GetPendingApprovedBooks()
        {
            var BVM = new BookViewModel();
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    BVM.ApprovedList = objRepositoty.GetBooks().Where(s => s.StatusId == 2 && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(s => s.ApprovedDate);
                    ViewBag.approvedsubtitle = "My Approved Books";

                    BVM.PendingList = objRepositoty.GetBooks().Where(s => (s.StatusId == 1 || s.StatusId == 3)).OrderByDescending(s => s.CreatedDate);
                    ViewBag.pendingsubtitle = "Pending Books";
                }
                else
                {
                    BVM.ApprovedList = objRepositoty.GetBooks().Where(s => s.StatusId == 2 && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(s => s.ApprovedDate);
                    ViewBag.approvedsubtitle = "My Approved Books";

                    BVM.PendingList = objRepositoty.GetBooks().Where(s => (s.StatusId == 1 || s.StatusId == 3) && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(s => s.CreatedDate);
                    ViewBag.pendingsubtitle = "My Pending / Rejected Books";
                }
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }

            return PartialView("_PendingApprovedBooksList", BVM);
        }

        public IActionResult Books()
        {
            var BVM = new BookViewModel();
            BVM.EventsList = EventsList();

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    BVM.ApprovedList = objRepositoty.GetBooks().Where(s => s.StatusId == 2 && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(s => s.ApprovedDate);
                    ViewBag.approvedsubtitle = "Approved Books";

                    BVM.PendingList = objRepositoty.GetBooks().Where(s => (s.StatusId == 1 || s.StatusId == 3)).OrderByDescending(s => s.CreatedDate);
                    ViewBag.pendingsubtitle = "Pending Books";
                }
                else
                {
                    BVM.ApprovedList = objRepositoty.GetBooks().Where(s => s.StatusId == 2 && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(s => s.ApprovedDate);

                    ViewBag.approvedsubtitle = "Approved Books";

                    BVM.PendingList = objRepositoty.GetBooks().Where(s => (s.StatusId == 1 || s.StatusId == 3) && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(s => s.CreatedDate);

                    ViewBag.pendingsubtitle = "Pending / Rejected Books";
                }
            }
            else
            {
                return Redirect("/Identity/Account/Login");
            }

            return View(BVM);
        }

        public async Task<IActionResult> SearchBook(string searchString)
        {
            var tehonlolgyId = from m in objRepositoty.GetTechnologys().Where(a => a.TechnologyName.Equals(searchString, StringComparison.OrdinalIgnoreCase)) select m.TechnologyId;
            var BVM = new BookViewModel();
            ViewBag.searchLetter = searchString;
            BVM.EventsList = EventsList();
            if (tehonlolgyId.ToList().Count > 0)
            {
                int techID = Convert.ToInt32(tehonlolgyId.ToList()[0]);

                var Books = from m in objRepositoty.GetBooks().Where(a => a.TechnologyId == techID) select m;
                if (techID > 0)
                {
                    BVM.ApprovedList = Books.Where(s => s.TechnologyId == techID && s.StatusId == 2 && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier)); // objRepositoty.GetBooks().Where(s => s.StatusId == 2 && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(s => s.ApprovedDate);

                    ViewBag.approvedsubtitle = "Approved Books";

                    if (User.IsInRole("Admin"))
                    {
                        BVM.PendingList = Books.Where(s => s.TechnologyId == techID && s.StatusId == 1);
                    }
                    else
                    {
                        BVM.PendingList = Books.Where(s => s.TechnologyId == techID && (s.StatusId == 1 || s.StatusId == 3) && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier));
                    }

                    ViewBag.pendingsubtitle = "Pending Books";
                }
                else
                    return RedirectToAction(nameof(Books));

                if (Books.ToList().Count == 0)
                {
                    return RedirectToAction("HandleErrorCode",
                              "Error",
                              new { statusCode = 264 });
                }
                else
                    return await Task.FromResult(View("Books", BVM));
            }
            else
            {
                var Books = from m in objRepositoty.GetBooks() select m;
                if (!String.IsNullOrEmpty(searchString))
                {
                    //BVM.ApprovedList = Books.Where(s => s.BookName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1 && s.StatusId == 2); // objRepositoty.GetBooks().Where(s => s.StatusId == 2 && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(s => s.ApprovedDate);
                    BVM.ApprovedList = Books.Where(s => s.BookName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1 && s.StatusId == 2);

                    ViewBag.approvedsubtitle = "Approved Books";

                    if (User.IsInRole("Admin"))
                    {
                        BVM.PendingList = Books.Where(s => s.BookName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1 && s.StatusId == 1);
                    }
                    else
                    {
                        BVM.PendingList = Books.Where(s => s.BookName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1 && (s.StatusId == 1 || s.StatusId == 3) && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier));
                    }

                    ViewBag.pendingsubtitle = "Pending Books";
                }
                else
                    //return View("Error/500");
                    return RedirectToAction(nameof(Books));

                if (Books.ToList().Count == 0)
                {
                    return RedirectToAction("HandleErrorCode",
                              "Error",
                              new { statusCode = 264 });
                }
                else
                    return await Task.FromResult(View("Books", BVM));
            }
        }

        public async Task<IActionResult> SearchBookLanding(string searchString)
        {
            var tehonlolgyId = from m in objRepositoty.GetTechnologys().Where(a => a.TechnologyName.Equals(searchString, StringComparison.OrdinalIgnoreCase)) select m.TechnologyId;
            var BVM = new BookViewModel();
            BVM.EventsList = EventsList();
            ViewBag.searchLetter = searchString;
            if (tehonlolgyId.ToList().Count > 0)
            {
                int techID = Convert.ToInt32(tehonlolgyId.ToList()[0]);
                TempData["Home"] = "true";

                var Books = from m in objRepositoty.GetBooks().Where(a => a.TechnologyId == techID) select m;
                if (techID > 0)
                {
                    BVM.ApprovedList = Books.Where(s => s.TechnologyId == techID && s.StatusId == 2); // objRepositoty.GetBooks().Where(s => s.StatusId == 2 && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(s => s.ApprovedDate);
                    ViewBag.approvedsubtitle = "Books";
                }
                else
                    return RedirectToAction(nameof(Books));

                if (Books.ToList().Count == 0)
                {
                    return RedirectToAction("HandleErrorCode",
                              "Error",
                              new { statusCode = 264 });
                }
                else
                    return await Task.FromResult(View("Index", BVM));
            }
            else
            {
                TempData["Home"] = "true";
                var Books = from m in objRepositoty.GetBooks() select m;
                if (!String.IsNullOrEmpty(searchString))
                {
                    BVM.ApprovedList = Books.Where(s => s.BookName.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) != -1 && s.StatusId == 2); // objRepositoty.GetBooks().Where(s => s.StatusId == 2 && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier)).OrderByDescending(s => s.ApprovedDate);
                    ViewBag.approvedsubtitle = "Books";
                }
                else
                    //return View("Error/500");
                    return RedirectToAction(nameof(Index));
                //return await Task.FromResult(View("Index"));

                if (Books.ToList().Count == 0)
                {
                    return RedirectToAction("HandleErrorCode",
                              "Error",
                              new { statusCode = 264 });
                }
                else
                    return await Task.FromResult(View("Index", BVM));
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
            {
                ApprovalStatusList();
                objBookDetails.EventsList = objEventRepository.GetEvents().OrderByDescending(s => s.EventDate);
                return View(objBookDetails);
            }
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
                objBookDetails.book.CreatedBy = bookDetails.CreatedBy;
                objBookDetails.book.CreatedDate = bookDetails.CreatedDate;
                objBookDetails.book.ApprovedBy = bookDetails.ApprovedBy;
                objBookDetails.book.ApprovedDate = bookDetails.ApprovedDate;
                objBookDetails.book.Remarks = bookDetails.Remarks;
                objBookDetails.book.StatusId = bookDetails.StatusId;
                objBookDetails.ExistingImageUrl = bookDetails.ImageUrl;
                objBookDetails.book.Author = bookDetails.Author;
                objBookDetails.book.PublishedDate = bookDetails.PublishedDate;
            }



            return objBookDetails;
        }

        // GET: Courses/Create
        [Authorize]
        public IActionResult Create()
        {
            var BVM = new BookViewModel();
            BVM.EventsList = EventsList();
            TechnologyList();
            return View(BVM);
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookViewModel objbookDetails) //[Bind("BookId,BookName,Description,ImageUrl,TechnologyId")] Book book)
        {
            if (!objRepositoty.GetBooks().Any(a => a.BookName == objbookDetails.book.BookName && a.Author == objbookDetails.book.Author.Trim() && a.TechnologyId == objbookDetails.book.TechnologyId && a.PublishedDate == objbookDetails.book.PublishedDate))
            {
                Book objNewBook = null;
                if (ModelState.IsValid)
                {
                    string uniqueFileNmae = ProcessUploadFile(objbookDetails);
                    if (uniqueFileNmae != null)
                    {
                        objNewBook = new Book
                        {
                            BookName = objbookDetails.book.BookName != null ? objbookDetails.book.BookName.Trim() : objbookDetails.book.BookName,
                            Description = objbookDetails.book.Description != null ? objbookDetails.book.Description.Trim() : objbookDetails.book.Description,
                            TechnologyId = objbookDetails.book.TechnologyId,
                            ImageUrl = uniqueFileNmae,
                            CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier),
                            CreatedDate = DateTime.Now,
                            StatusId = 1,
                            Author = objbookDetails.book.Author != null ? objbookDetails.book.Author.Trim() : objbookDetails.book.Author,
                            PublishedDate = objbookDetails.book.PublishedDate
                        };

                        objRepositoty.Add(objNewBook);
                        return RedirectToAction(nameof(Index));
                    }

                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Book already exists.");
            }

            TechnologyList();
            return View();
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
                objNewBook.EventsList = EventsList();
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

            if (!BookExists(objbookDetails))
            {
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
                            objbookDetails.book.StatusId = 1;
                        }
                        else
                        {
                            objbookDetails.book.ImageUrl = objbookDetails.ExistingImageUrl;
                            objbookDetails.book.StatusId = objbookDetails.book.StatusId;
                        }

                        Book bookObj = objRepositoty.GetBooks().Where(x => x.BookId == objbookDetails.book.BookId).FirstOrDefault();

                        if (bookObj != null)
                        {
                            bookObj.BookId = objbookDetails.book.BookId;
                            bookObj.BookName = objbookDetails.book.BookName != null ? objbookDetails.book.BookName.Trim() : objbookDetails.book.BookName;
                            bookObj.Description = objbookDetails.book.Description != null ? objbookDetails.book.Description.Trim() : objbookDetails.book.Description;
                            bookObj.TechnologyId = objbookDetails.book.TechnologyId;
                            bookObj.ImageUrl = objbookDetails.book.ImageUrl != null ? objbookDetails.book.ImageUrl.Trim() : objbookDetails.book.ImageUrl;
                            bookObj.StatusId = objbookDetails.book.StatusId;
                            bookObj.Remarks = objbookDetails.book.Remarks != null ? objbookDetails.book.Remarks.Trim() : objbookDetails.book.Remarks;
                            bookObj.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                            bookObj.CreatedDate = objbookDetails.book.CreatedDate;
                            bookObj.ApprovedBy = objbookDetails.book.ApprovedBy != null ? objbookDetails.book.ApprovedBy.Trim() : objbookDetails.book.ApprovedBy;
                            bookObj.ApprovedDate = objbookDetails.book.ApprovedDate;
                            bookObj.Author = objbookDetails.book.Author != null ? objbookDetails.book.Author.Trim() : objbookDetails.book.Author;
                            bookObj.PublishedDate = objbookDetails.book.PublishedDate;
                        };

                        objRepositoty.Update(bookObj);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BookExist(objbookDetails.book.BookId))
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

            TechnologyList();
            return View(objbookDetails);
        }

        private bool BookExist(int intBookId)
        {
            var ExistingBookDetails = objRepositoty.GetBookDetailsById(intBookId);
            if (ExistingBookDetails != null)
                return true;
            else
                return false;
        }

        private bool BookExists(BookViewModel objbookDetails)
        {
            var Books = from m in objRepositoty.GetBooks() where (m.BookName == objbookDetails.book.BookName && m.Author == objbookDetails.book.Author.Trim() && m.TechnologyId == objbookDetails.book.TechnologyId && m.PublishedDate == objbookDetails.book.PublishedDate && m.BookId != objbookDetails.book.BookId) select m;
            if (Books.ToList().Count > 0)
                return true;
            else
                return false;
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

        private IEnumerable<Technology> TechnologyList()
        {
            ViewBag.technologyList = objRepositoty.GetTechnologys();
            return objRepositoty.GetTechnologys();
        }

        private IEnumerable<ApprovalStatus> ApprovalStatusList()
        {
            ViewBag.approvalStatusList = objRepositoty.GetApprovalStatus();
            return objRepositoty.GetApprovalStatus();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveReview(BookViewModel objReviewDetails)
        {
            Reviewes objNewReview = null;

            if (ModelState.IsValid)
            {
                objNewReview = new Reviewes
                {
                    ReviewText = objReviewDetails.review.ReviewText,
                    Rating = objReviewDetails.review.Rating,
                    BookId = objReviewDetails.review.BookId,
                };
                objRepositoty.Add(objNewReview);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        public IActionResult ApprovalStatus(BookViewModel objbookDetails)
        {
            string ImagePath = string.Empty;

            if (objbookDetails.book.StatusId == 3)
            {
                string UploadImageFolder = Path.Combine(objHostingEnvironment.WebRootPath, "uploadImages");
                string ExistringImgPath = UploadImageFolder + "\\" + objbookDetails.book.ImageUrl;
                string NewImgName = Guid.NewGuid().ToString();
                ImagePath = NewImgName + ".png";
                string NewImagePath = UploadImageFolder + "\\" + NewImgName + ".png";
                ChangeToRejectedImg(ExistringImgPath, "REJECTED!", NewImagePath);
                System.IO.File.Delete(ExistringImgPath);
            }
            else
                ImagePath = objbookDetails.book.ImageUrl;

            Book objEditBook = new Book
            {
                BookId = objbookDetails.book.BookId,
                BookName = objbookDetails.book.BookName,
                ImageUrl = ImagePath,
                TechnologyId = objbookDetails.book.TechnologyId,
                Description = objbookDetails.book.Description,
                StatusId = objbookDetails.book.StatusId,
                Remarks = objbookDetails.book.Remarks,
                CreatedBy = objbookDetails.book.CreatedBy,
                CreatedDate = objbookDetails.book.CreatedDate,
                ApprovedBy = User.FindFirstValue(ClaimTypes.NameIdentifier),
                ApprovedDate = DateTime.Now,
                Author = objbookDetails.book.Author,
                PublishedDate = objbookDetails.book.PublishedDate
            };

            objRepositoty.Update(objEditBook);
            return RedirectToAction(nameof(Index));
        }

        public void ChangeToRejectedImg(string imgpath, string text, string UploadImageFolder)
        {
            //Load the Image to be written on.
            Bitmap bitMapImage1 = new System.Drawing.Bitmap(imgpath);
            Bitmap bitMapImage = (Bitmap)bitMapImage1.Clone(new Rectangle(0, 0, bitMapImage1.Width, bitMapImage1.Height), bitMapImage1.PixelFormat);
            bitMapImage1.Dispose();

            using (Graphics graphicImage = Graphics.FromImage(bitMapImage))
            {
                //Smooth graphics is nice.
                graphicImage.SmoothingMode = SmoothingMode.AntiAlias;

                //I am drawing a oval around my text.
                //graphicImage.DrawArc(new Pen(Color.Red, 3), 90, 235, 150, 50, 0, 360);

                //Write your text.
                //graphicImage.DrawString("That's my boy!", new Font("Arial", 12, FontStyle.Bold), SystemBrushes.WindowText, new Point(100, 250));
                graphicImage.DrawString(text, new Font("Arial", 16, FontStyle.Bold), new SolidBrush(Color.Red), new Point(10, 100));

                //Set the content type
                Response.ContentType = "image/png";
                bitMapImage.Save(UploadImageFolder, ImageFormat.Png);

                //Clean house.
                graphicImage.Dispose();
                bitMapImage.Dispose();
            }
        }

        public async Task<IActionResult> ShowByLetter(string id)
        {
            var GBooks = from m in objRepositoty.GetBooks().Where(a => a.BookName.StartsWith(id)) select m;
            GBooks = GBooks.ToList();
            var BVM = new BookViewModel();
            BVM.EventsList = EventsList();
            BVM.ApprovedList = GBooks.Where(s => s.StatusId == 2 && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewBag.searchLetter = id;

            ViewBag.approvedsubtitle = "Approved Books";

            if (User.IsInRole("Admin"))
            {
                BVM.PendingList = GBooks.Where(s => s.StatusId == 1);
            }
            else
            {
                BVM.PendingList = GBooks.Where(s => (s.StatusId == 1 || s.StatusId == 3) && s.CreatedBy == User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            ViewBag.pendingsubtitle = "Pending Books";

            return await Task.FromResult(View("Books", BVM));
        }

        public async Task<IActionResult> ShowByLetterLanding(string id)
        {
            var GBooks = from m in objRepositoty.GetBooks().Where(a => a.BookName.StartsWith(id)) select m;
            GBooks = GBooks.ToList();
            var BVM = new BookViewModel();
            BVM.EventsList = EventsList();
            BVM.ApprovedList = GBooks.Where(s => s.StatusId == 2);
            ViewBag.searchLetter = id;
            ViewBag.approvedsubtitle = "Books";

            return await Task.FromResult(View("Index", BVM));
        }
    }
}