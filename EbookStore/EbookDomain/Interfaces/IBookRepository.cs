using EbookDomain.Models;
using System.Collections.Generic;

namespace EbookDomain.Interfaces
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetBooks();
        Book GetBookDetailsById(int bookId);
        Book Add(Book bookDetails);
        Book Update(Book bookDetails);
        Book Delete(int bookId);
        IEnumerable<Technology> GetTechnologys();
        Technology GetTechnologyDetailsById(int technologyId);
    }
}