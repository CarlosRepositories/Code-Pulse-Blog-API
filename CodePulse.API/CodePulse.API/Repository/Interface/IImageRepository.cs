using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repository.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);

        Task<IEnumerable<BlogImage>> GetAll();
    }
}
