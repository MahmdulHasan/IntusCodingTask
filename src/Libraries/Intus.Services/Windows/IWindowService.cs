using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Services.Windows
{
    public interface IWindowService
    {
        Task<Window> GetWindowById(int windowId);
        Task<IList<Window>> GetAllWindows(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showDeleted = false);
        Task DeleteWindow(Window window);
        Task InsertWindow(Window window);
        Task UpdateWindow(Window window);
    }
}
