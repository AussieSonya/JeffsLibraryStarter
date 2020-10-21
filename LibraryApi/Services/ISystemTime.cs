using System;

namespace LibraryApi.Services
{
    public interface ISystemTime
    {
        DateTime GetCurrent();
    }
}