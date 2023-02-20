using Intus.Core.Entities;
using Intus.Services.Elements;
using Intus.Web.Framework.Contracts.V1.Element;

namespace Intus.Web.API.Controllers
{
    [Route("api/v1/elements")]
    [ApiController]
    public class ElementController : ControllerBase
    {
        private readonly IElementService _elementService;

        public ElementController(IElementService elementService)
        {
            _elementService = elementService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var elements = await _elementService.GetAllElements();

            var elementList = elements.Select(element => new ElementModel
            {
                Id = element.Id,
                Type = element.Type
            }); ;

            return Ok(elementList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var element = await _elementService.GetElementById(id);

            var elementModel = new ElementModel
            {
                Id = element.Id,
                Type = element.Type
            };

            return Ok(elementModel);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ElementModel model)
        {
            await _elementService.InsertElement (new Element
            {
                Type = model.Type,
                CreateDate = DateTime.UtcNow
            });

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ElementModel model)
        {
            var element = await _elementService.GetElementById(id);

            element.Type = model.Type;
            element.UpdateDate = DateTime.UtcNow;

            await _elementService.UpdateElement(element);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var element = await _elementService.GetElementById(id);

            element.IsDeleted = true;

            await _elementService.UpdateElement(element);

            return Ok();
        }
    }
}
