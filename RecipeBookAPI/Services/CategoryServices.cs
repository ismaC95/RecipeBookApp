using RecipeBookAPI.Models;
using RecipeBookAPI.Repositories;

namespace RecipeBookAPI.Services
{
    public class CategoryServices
    {
        private readonly CategoryRepository _repo;

        public CategoryServices(CategoryRepository repo)
        {
            _repo = repo;
        }


        public void CreateCategory(Category category)
        {
            if(GetByName(category.Name) == null)
                _repo.Insert(category);
            else
            {
                throw new InvalidOperationException("Category already exists");
            }
        }

        public Category? GetCategoryById(int id)
        {
            return _repo.GetByID(id);
        }

        public List<Category> GetAllCategories()
        {
            return _repo.GetAll();
        }

        public void DeleteCategory(Category c)
        {
            _repo.DeleteCategory(c);
        }

        public Category? GetByName(string name)
        {
            return _repo.GetByName(name);
        }
    }
}
