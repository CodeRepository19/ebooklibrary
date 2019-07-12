using EbookDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EbookDomain.Interfaces
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetBooks();
        Book GetBookDetailsById(int bookId);
        Book Add(Book bookDetails);
        Book Update(Book bookDetails);
        Book Delete(int bookId);
    }
}
