

using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class CustomerRepository
{
    private readonly DataContext _context;

    public CustomerRepository(DataContext context)
    {
        _context = context;
    }

    //  CREATE (Add new customer)
    public async Task<CustomerEntity> CreateAsync(CustomerEntity customer)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    //  READ (Get by Id)
    public async Task<CustomerEntity?> GetByIdAsync(int id)
    {
        return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
    }

    //  READ (Get all customers)
    public async Task<List<CustomerEntity>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    //  UPDATE
    public async Task<bool> UpdateAsync(CustomerEntity customer)
    {
        var existingCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == customer.Id);
        if (existingCustomer != null)
        {
            existingCustomer.CustomerName = customer.CustomerName;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    //  DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

}
