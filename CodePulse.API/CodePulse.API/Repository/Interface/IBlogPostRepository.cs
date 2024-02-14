using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repository.Interface
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> CreateAsync(BlogPost blogPost);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost?> GetByIdAsync(Guid id);
        Task<BlogPost?> UpdateByIdAsync(BlogPost blogPost);
        Task<BlogPost?> DeleteAsync(Guid id);

        Task<BlogPost?> GetByUrlHandleAsync(string urlHandle);
    }
}
