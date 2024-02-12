using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repository.Implementation
{
    public class ImagesRepository : IImageRepository
    {
        private readonly ApplicationDbContext DbContext;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly IHttpContextAccessor HttpContextAccessor;

        public ImagesRepository(ApplicationDbContext applicationDbContext, 
                               IWebHostEnvironment webHostEnvironment,
                               IHttpContextAccessor httpContextAccessor) 
        {
            DbContext = applicationDbContext;
            WebHostEnvironment = webHostEnvironment;
            HttpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<BlogImage>> GetAll()
        {
            return await DbContext.BlogImages.ToListAsync();
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            // 1 - Copy Image to a local directory
            var localPath = Path.Combine(WebHostEnvironment.ContentRootPath, "Images", $"{blogImage.FileName}{blogImage.FileExtension}");
            var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            // 2 - Save BlogImag in Database

            var httpRequest = HttpContextAccessor.HttpContext.Request;
            var urlPath = 
                $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

            await DbContext.BlogImages.AddAsync(blogImage);

            await DbContext.SaveChangesAsync();

            return blogImage;
        }

        

    }
}
