using Microsoft.AspNetCore.Mvc;

namespace bestpricesale.Pages.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveScriptsController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public SaveScriptsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ScriptSaveModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Slug) || string.IsNullOrWhiteSpace(model.ScriptContent))
            {
                return BadRequest("Invalid input");
            }

            // Sanitize the slug as needed.
            var fileName = $"{model.Slug}.js";
            var jsFolder = Path.Combine(_env.WebRootPath, "js");
            if (!Directory.Exists(jsFolder))
            {
                Directory.CreateDirectory(jsFolder);
            }
            var filePath = Path.Combine(jsFolder, fileName);

            System.IO.File.WriteAllText(filePath, model.ScriptContent);
            return Ok();
        }
    }

    public class ScriptSaveModel
    {
        public string Slug { get; set; }
        public string ScriptContent { get; set; }
    }

}
