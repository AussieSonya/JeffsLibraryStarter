using LibraryApi.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public interface IQueryForBooks
    {
        Task<GetBooksResponse> GetAllBooks();
        Task<GetBookDetailsResponse> GetBookById(int bookId);
    }
}
