using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{

    public class ProjectRepository
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;
       

        public ProjectRepository(IDbContextFactory<DataContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        // ✅ GET ALL CUSTOMERS (Fixing 'GetCustomersAsync' Error)
        public async Task<List<CustomerEntity>> GetCustomersAsync()
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Customers.ToListAsync();
        }

        // ✅ GET ALL PRODUCTS (Fixing 'GetProductsAsync' Error)
        public async Task<List<ProductEntity>> GetProductsAsync()
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Products.ToListAsync();
        }

        // ✅ GET ALL STATUSES (Fixing 'GetStatusesAsync' Error)
        public async Task<List<StatusTypeEntity>> GetStatusesAsync()
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.StatusType.ToListAsync();
        }
        
        // ✅ GET ALL PROJECTS
        public async Task<List<ProjectEntity>> GetAllAsync()
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Projects
                .Include(p => p.Status)
                .Include(p => p.Customer)
                .Include(p => p.User)
                .Include(p => p.Product)
                .ToListAsync();
        }

        // ✅ CREATE A PROJECT (Fixing 'CreateAsync' Error)

        public async Task<ProjectEntity> CreateAsync(ProjectEntity project)
        {
            await using var context = _contextFactory.CreateDbContext();

            var existingCustomer = await context.Customers.FindAsync(project.Customer?.Id);
            var existingProduct = await context.Products.FindAsync(project.Product?.Id);
            var existingUser = await context.Users.FindAsync(project.User?.Id);

            if (existingCustomer == null || existingProduct == null || existingUser == null)
        throw new Exception("Error: Customer, Product, or User does not exist in the database.");

            project.Customer = existingCustomer;
            project.Product = existingProduct;
            project.User = existingUser;

            await context.Projects.AddAsync(project);
            await context.SaveChangesAsync();

            return project;
        }

        public async Task<List<UserEntity>> GetUsersAsync()
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Users.ToListAsync(); //  Fetch all employees (users)
        }
        

        // ✅ GET PROJECT BY ID
        public async Task<ProjectEntity?> GetByIdAsync(int id)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Projects
                .Include(p => p.Status)
                .Include(p => p.Customer)
                .Include(p => p.User)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // ✅ UPDATE PROJECT
        public async Task<bool> UpdateAsync(ProjectEntity project)
        {
            await using var context = _contextFactory.CreateDbContext();
            var existingProject = await context.Projects.FindAsync(project.Id);
            if (existingProject != null)
            {
                context.Entry(existingProject).CurrentValues.SetValues(project);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // ✅ DELETE PROJECT
        public async Task<bool> DeleteAsync(int id)
        {
            await using var context = _contextFactory.CreateDbContext();
            var project = await context.Projects.FindAsync(id);
            if (project != null)
            {
                context.Projects.Remove(project);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }


    }
}
