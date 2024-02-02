using CodePulse.API.Data;
using CodePulse.API.Repository.Interface;
using CodePulse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repository.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext DbContext;
        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            DbContext = applicationDbContext;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await DbContext.Categories.AddAsync(category);
            await DbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await DbContext.Categories.FindAsync(id);

            if (existingCategory == null)
            {
                return null;
            }

            DbContext.Categories.Remove(existingCategory);
            await DbContext.SaveChangesAsync();

            return existingCategory;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await DbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            var category = await DbContext.Categories.FindAsync(id);

            return category;

        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await DbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == category.Id);

            if (existingCategory != null)
            {
                DbContext.Entry(existingCategory).CurrentValues.SetValues(category);
                DbContext.SaveChanges();
                return category;
            }

            return null;
        }
    }
}
