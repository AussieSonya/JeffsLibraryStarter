using LibraryApi.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public interface IDoBookCommands
    {
        Task<GetBookDetailsResponse> AddBook(PostBookCreate bookToAdd);
        Task RemoveBook(int bookId);
        Task<bool> UpdateTitle(int bookId, string title);
    }
}
