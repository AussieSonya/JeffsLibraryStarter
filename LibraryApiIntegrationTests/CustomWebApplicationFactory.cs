
using LibraryApi;
using LibraryApi.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace LibraryApiIntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

            builder.ConfigureServices(services =>
            {
                // go find the thing that implements the service you want to isolate.
                var systemTimeDescriptor = services.Single(
                    d => d.ServiceType == typeof(ISystemTime)
                   );

                services.Remove(systemTimeDescriptor); // gone!
                services.AddTransient<ISystemTime, FakeSystemTime>();
                // Just wanted to make a change
            });
        }
    }

    public class FakeSystemTime : ISystemTime
    {
        public DateTime GetCurrent()
        {
            return new DateTime(1969, 4, 20, 23, 59, 00);
        }
    }
}
