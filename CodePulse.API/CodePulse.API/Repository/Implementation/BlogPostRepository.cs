using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repository.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDbContext DbContext;

        public BlogPostRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<BlogPost> CreateAsync(BlogPost blogPost)
        {
            await DbContext.BlogPosts.AddAsync(blogPost);
            await DbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await DbContext.BlogPosts
                .Include(c => c.Categories).ToListAsync();
        }

        public async Task<BlogPost?> GetByIdAsync(Guid id)
        {
            var existingBlogPost = await DbContext.BlogPosts
                .Include(c => c.Categories).FirstAsync(b => b.Id == id);
            if (existingBlogPost == null)
            {
                return null;
            }
            return existingBlogPost;
        }

        public async Task<BlogPost?> UpdateByIdAsync(BlogPost blogPost)
        {
            var existingBlogPost = await DbContext.BlogPosts
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(b => b.Id == blogPost.Id);

            if (existingBlogPost is null)
            {
                return null;
            }

            DbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            existingBlogPost.Categories = blogPost.Categories;

            await DbContext.SaveChangesAsync();

            return existingBlogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existinBlogPost = await DbContext.BlogPosts.Include(c => c.Categories)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (existinBlogPost is not null)
            {
                DbContext.BlogPosts.Remove(existinBlogPost);
                await DbContext.SaveChangesAsync();
                return existinBlogPost;
            }

            return null;
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            var existingBlogPost = await DbContext.BlogPosts
                .Include(c => c.Categories)
                .FirstAsync(b => b.UrlHandle == urlHandle);

            if (existingBlogPost == null)
            {
                return null;
            }
            return existingBlogPost;

        }
    }
}
