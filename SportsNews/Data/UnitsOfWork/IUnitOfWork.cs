using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    interface IUnitOfWork : IDisposable
    {
        void Save();
        Task SaveAsync();
    }
}
