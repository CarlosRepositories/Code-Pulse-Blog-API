using CodePulse.API.Models.Domain;

namespace CodePulse.API.Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        Task<Category?> GetCategoryByIdAsync(Guid id);

        Task<Category?> UpdateCategoryAsync(Category category);

        Task<Category?> DeleteCategoryAsync(Guid id);
    }
}
