using ShopGeneral.Data;

namespace ShopGeneral.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Category> GetTrendingCategories(int cnt)
    {
        return _context.Categories.Take(cnt);
    }
}