using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsNews.Data
{
    interface IItemRepository<T> : IDisposable
    {
        IEnumerable<T> GetItems();
        T GetItemByID(int id);

        void InsertItem(T item);
        void UpdateItem(T item);
        void DeleteItem(int id);
    }
}
