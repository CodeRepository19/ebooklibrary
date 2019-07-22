using EbookDomain.Models;
using System.Collections.Generic;

namespace EbookDomain.Interfaces
{
    /// <summary>
    /// This is the Definition of Interface IBookRepository
    /// we need to implement these methods in n number of  classes if needed
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// This is the Method to get all the Books Uploaded
        /// </summary>
        /// <returns>It is List Of Book objects</returns>
        IEnumerable<Book> GetBooks();
        /// <summary>
        /// This Method Find the Book Based on its it ID
        /// </summary>
        /// <param name="bookId">This is parameter to find the Book</param>
        /// <returns>This Method Returns a Book Object</returns>
        Book GetBookDetailsById(int bookId);
        /// <summary>
        /// This Method Will add the supplied Book Model Object to the Books collection
        /// </summary>
        /// <param name="bookDetails">bookDetails is an object of Book type which is taken as Input from Presetation Layer</param>
        /// <returns>This is the Result Book object</returns>
        Book Add(Book bookDetails);
        /// <summary>
        /// This Method Will Update the supplied Book Model Object to the Books collection
        /// </summary>
        /// <param name="bookDetails">bookDetails is the Modified Book Object which will be supplied from Presentaion Layer</param>
        /// <returns>This is the Updated Book object<</returns>
        Book Update(Book bookDetails);
        /// <summary>
        /// This Method is us used to Delete the Book from Book Table based on its bookId
        /// </summary>
        /// <param name="bookId">bookId is the parameter based on which book will be deleted</param>
        /// <returns>This is the Deleted Book object<</returns>
        Book Delete(int bookId);
        /// <summary>
        /// This Method is used to get all the Technologies list
        /// </summary>
        /// <returns>It is a list of Technologies</returns>
        IEnumerable<Technology> GetTechnologys();
        Technology GetTechnologyDetailsById(int technologyId);
        /// <summary>
        /// This method will return the list of all the reviews
        /// </summary>
        /// <returns></returns>
        IEnumerable<Reviewes> GetReviews();
        /// <summary>
        /// This will add the review of the Book to the review table
        /// </summary>
        /// <param name="reviewDetails"></param>
        /// <returns></returns>
        Reviewes Add(Reviewes reviewDetails);
    }
}