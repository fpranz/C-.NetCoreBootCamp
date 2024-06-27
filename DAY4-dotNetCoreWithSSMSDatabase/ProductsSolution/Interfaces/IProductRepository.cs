// Interfaces/IProductRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsSolution.Models;

namespace ProductsSolution.Interfaces
{
    public interface IProductRepository
    {
        // this is an interface to get all products
        Task<IEnumerable<Product>> GetAllProducts();

        // Add Interface to Add new product
        Task AddProduct(Product product);
        
        // Add Interface to delete product
        Task DeleteProduct(int id);
    }
}