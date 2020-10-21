using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class CatalogService : ICacheTheCatalog
    {
        private readonly IDistributedCache _cache;

        public CatalogService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<CatalogModel> GetCatalogAsync()
        {
            // ask the cache for the thing.
            var catalog = await _cache.GetAsync("catalog");
            string newCatalog = null;

            // it there is a catalog in the cache - return that.
            if (catalog == null)
            {
                newCatalog = $"This catalog was created at {DateTime.Now.ToLongTimeString()}";
                var encodedCatalog = Encoding.UTF8.GetBytes(newCatalog);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddSeconds(15));
                await _cache.SetAsync("catalog", encodedCatalog, options);
            }
            else
            {
                newCatalog = Encoding.UTF8.GetString(catalog);
            }
            return new CatalogModel { Data = newCatalog };
        }
    }
    public class CatalogModel
    {
        public string Data { get; set; }
    }
}
