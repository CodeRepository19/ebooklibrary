using EbookDomain.Interfaces;
using EbookDomain.Models;
using EbookInfraData.Context;
using System.Collections.Generic;

namespace EbookInfraData.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ebooklibraryDBcontext _ctx;

        public BookRepository(ebooklibraryDBcontext ctx)
        {
            this._ctx = ctx;
        }

        public IEnumerable<Book> GetBooks()
        {
            return _ctx.Books;
        }


        public IEnumerable<Reviewes> GetReviews()
        {
            return _ctx.reviewes;
        }

        public Book GetBookDetailsById(int bookId)
        {
            return _ctx.Books.Find(bookId);
        }
        
        public Reviewes Add(Reviewes reviewDetails)
        {
            _ctx.reviewes.Add(reviewDetails);
            _ctx.SaveChanges();
            return reviewDetails;
        }

        public Book Add(Book bookDetails)
        {
            _ctx.Books.Add(bookDetails);
            _ctx.SaveChanges();
            return bookDetails;
        }

        public Book Update(Book bookDetails)
        {
            _ctx.Update(bookDetails);
            //var book = _ctx.Books.Attach(bookDetails);
            //book.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _ctx.SaveChanges();
            return bookDetails;
        }

        public Book Delete(int bookId)
        {
            Book book = _ctx.Books.Find(bookId);
            if (book != null)
            {
                _ctx.Books.Remove(book);
                _ctx.SaveChanges();
            }

            return book;
        }

        public IEnumerable<Technology> GetTechnologys()
        {
            return _ctx.Technologys;
        }

        public Technology GetTechnologyDetailsById(int technologyId)
        {
            return _ctx.Technologys.Find(technologyId);
        }
             
    }
}