using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository ImageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            ImageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
            [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUploaded(file);

            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = fileName,
                    Title = title
                };

                blogImage = await ImageRepository.Upload(file, blogImage);

                var blogImageDto = new BlogImageDto
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    FileName = blogImage.FileName,
                    FileExtension = blogImage.FileExtension,
                    Url = blogImage.Url,
                    DateCreated = blogImage.DateCreated
                };

                return Ok(blogImageDto);
            }

            return BadRequest(ModelState);

        }

        private void ValidateFileUploaded(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                ModelState.AddModelError("file", "Unsupported file format");

            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB");
            }
        }
    }
}
