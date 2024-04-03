public interface IProductRepository
{
    public Task<List<Product>> GetAll();
    public Task<Product> GetById(Guid id);
    public Task<List<Product>> GetByName(string name);
    public void Create(ProductRequest product);
    public void Update(ProductDTO product);
    public void Delete(Guid id);
}
