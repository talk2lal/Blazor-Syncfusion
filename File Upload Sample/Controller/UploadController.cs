using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadSample.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save(List<IFormFile> UploadFiles)
        {
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadPath);

            foreach (var file in UploadFiles)
            {
                var filePath = Path.Combine(uploadPath, file.FileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
            }

            return Ok(new { message = "Files uploaded successfully" });
        }

        [HttpPost("remove")]
        public IActionResult Remove([FromForm] string fileNames)
        {
            var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
            var filePath = Path.Combine(uploadPath, fileNames);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return Ok(new { message = "File removed successfully" });
        }
    }

}
