


using Intus.Core.Entities;
using Intus.Services.Windows;
using Intus.Web.Framework.Contracts.V1.Window;

namespace Intus.Web.API.Controllers
{
    [Route("api/v1/windows")]
    [ApiController]
    public class WindowController : ControllerBase
    {
        private readonly IWindowService _windowService;

        public WindowController(IWindowService windowService)
        {
            _windowService = windowService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var windows= await _windowService.GetAllWindows();

            var windowList= windows.Select(window => new WindowModel
            {
                Id = window.Id,
                Name = window.Name
            });

            return Ok(windowList);
        }
     
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] WindowModel model)
        {
           await _windowService.InsertWindow(new Window
            {
                Name= model.Name,
                CreateDate = DateTime.UtcNow
            });

            return Ok();
        }
      
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] WindowModel model)
        {
            var window = await _windowService.GetWindowById(id);

            window.Name = model.Name;
            window.UpdateDate = DateTime.UtcNow;

            await _windowService.UpdateWindow(window);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var window = await _windowService.GetWindowById(id);

            window.IsDeleted = true;

            await _windowService.UpdateWindow(window);

            return Ok();
        }
    }
}
