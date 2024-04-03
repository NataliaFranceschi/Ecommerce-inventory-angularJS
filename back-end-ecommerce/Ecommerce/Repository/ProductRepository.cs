using Microsoft.EntityFrameworkCore;

public class ProductRepository : IProductRepository
{
    private readonly IAppDbContext _context;

    public ProductRepository(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAll()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetById(Guid id)
    {
        var entity = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null)
        {
            throw new Exception("Produto não encontrado.");
        }

        return entity;
    }

    public async Task<List<Product>> GetByName(string name)
    {
        return await _context.Products
                    .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                    .ToListAsync();
    }

    public void Create(ProductRequest product)
    {
        var entity = new Product
        {
            Name = product.Name,
            Description = product.Description,
            Image = product.Image,
            Quantity = product.Quantity,
            Price = product.Price,
            DateCreated = DateTimeOffset.Now,
            DateUpdated = DateTimeOffset.Now,
        };

        _context.Products.Add(entity);
        _context.SaveChanges();

    }

    public void Update(ProductDTO product)
    {
        var entity = _context.Products.FirstOrDefault(p => p.Id == product.Id);

        if (entity == null)
        {
            throw new Exception("Produto não encontrado.");
        }

        entity.Name = product.Name;
        entity.Description = product.Description;
        entity.Image = product.Image;
        entity.Quantity = product.Quantity;
        entity.Price = product.Price;
        entity.DateUpdated = DateTimeOffset.Now;

        _context.Products.Update(entity);
        _context.SaveChanges();

    }

    public void Delete(Guid id)
    {
        var entity = _context.Products.FirstOrDefault(p => p.Id == id);

        if (entity == null)
        {
            throw new Exception("Produto não encontrado.");
        }

        _context.Products.Remove(entity);
        _context.SaveChanges();
    }
}
