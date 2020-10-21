using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models.Books
{
    public class GetBooksResponse
    {
        public List<GetBooksResponseItem> Data { get; set; }
    }
}
