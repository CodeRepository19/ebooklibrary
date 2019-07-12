using EbookApplication.ViewModels;

namespace EbookApplication.interfaces
{
    public interface IBookService
    {
        BookViewModel GetBooks();
    }
}