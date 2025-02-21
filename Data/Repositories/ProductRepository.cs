

using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProductRepository 
{
    private readonly DataContext _context;

    public ProductRepository(DataContext context)
    {
        _context = context;
    }

    //  CREATE (Add a product)
    public async Task<ProductEntity> CreateAsync(ProductEntity product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    //  READ (Get all products)
    public async Task<List<ProductEntity>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    //  READ (Get product by Id)
    public async Task<ProductEntity?> GetByIdAsync(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    //  UPDATE
    public async Task<bool> UpdateAsync(ProductEntity product)
    {
        var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        if (existingProduct != null)
        {
            existingProduct.ProductName = product.ProductName;
            existingProduct.Price = product.Price;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    //  DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}


