

namespace Intus.Services.Elements
{
    public interface IElementService
    {
        Task<Element> GetElementById(int windowId);
        Task<IList<Element>> GetAllElements(string type = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showDeleted = false);
        Task DeleteElement(Element element);
        Task InsertElement(Element element);
        Task UpdateElement(Element element);
    }
}
