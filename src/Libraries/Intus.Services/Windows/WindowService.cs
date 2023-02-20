
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IRepository<Window> _windowRepository;

        public WindowService(IRepository<Window> windowRepository)
        {
            _windowRepository = windowRepository;
        }
        public async Task<Window> GetWindowById(int windowId)
        {
            if (windowId == 0)
                return null;

            return await _windowRepository.GetByIdAsync(windowId);
        }

        public async Task<IList<Window>> GetAllWindows(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showDeleted = false)
        {
            var query = _windowRepository.EntityWithNoTracking;

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(v => v.Name.Contains(name));

            if (!showDeleted)
                query = query.Where(v => !v.IsDeleted);

            return await query.ToListAsync();
        }

        public async Task InsertWindow(Window window)
        {
            ArgumentNullException.ThrowIfNull(nameof(Window));

            await _windowRepository.AddAsync(window);
        }

        public async Task UpdateWindow(Window window)
        {
            ArgumentNullException.ThrowIfNull(nameof(Window));

            await _windowRepository.UpdateAsync(window);
        }

        public async Task DeleteWindow(Window window)
        {
            ArgumentNullException.ThrowIfNull(nameof(Window));

            await _windowRepository.DeleteAsync(window);
        }
    }
}
