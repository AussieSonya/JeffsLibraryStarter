using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class SystemTime : ISystemTime
    {
        public DateTime GetCurrent()
        {
            // this is a real example. but this is what you do with ANYTHING
            // that talks to the "outside" world. like DBs, like MessageQueues, FileSystems,
            // other APIs, etc.
            return DateTime.Now;
        }
    }
}
