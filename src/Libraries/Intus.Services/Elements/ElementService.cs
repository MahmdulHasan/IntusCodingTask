using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intus.Services.Elements
{
    public class ElementService : IElementService
    {
        private readonly IRepository<Element> _elementRepository;

        public ElementService(IRepository<Element> windowRepository)
        {
            _elementRepository = windowRepository;
        }
        public async Task<Element> GetElementById(int windowId)
        {
            if (windowId == 0)
                return null;

            return await _elementRepository.GetByIdAsync(windowId);
        }

        public async Task<IList<Element>> GetAllElements(string type = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showDeleted = false)
        {
            var query = _elementRepository.EntityWithNoTracking;

            if (!string.IsNullOrWhiteSpace(type))
                query = query.Where(v => v.Type.Contains(type));

            if (!showDeleted)
                query = query.Where(v => !v.IsDeleted);

            return await query.ToListAsync();
        }

        public async Task InsertElement(Element Element)
        {
            ArgumentNullException.ThrowIfNull(nameof(Element));

            await _elementRepository.AddAsync(Element);
        }

        public async Task UpdateElement(Element Element)
        {
            ArgumentNullException.ThrowIfNull(nameof(Element));

            await _elementRepository.UpdateAsync(Element);
        }

        public async Task DeleteElement(Element Element)
        {
            ArgumentNullException.ThrowIfNull(nameof(Element));

            await _elementRepository.DeleteAsync(Element);
        }
    }
}
