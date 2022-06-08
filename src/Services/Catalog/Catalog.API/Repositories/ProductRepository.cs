using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{

    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext catalogContext;
        public ProductRepository(ICatalogContext context)
        {
            catalogContext=context?? throw new ArgumentNullException(nameof(context));
        }


        public async Task CreateProduct(Product product)
        {
          await catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter =Builders<Product>.Filter.Eq(p=>p.Id, id);
           var result= await catalogContext.Products.DeleteOneAsync(filter);
           return result.IsAcknowledged && result.DeletedCount>0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await catalogContext.Products.Find(p=>p.Id==id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter =Builders<Product>.Filter.Eq(p=>p.Name, name);
            return await catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await catalogContext.Products.Find(p=>true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductyCategory(string category)
        {
            FilterDefinition<Product> filter =Builders<Product>.Filter.Eq(p=>p.Category, category);
            return await catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult= await catalogContext.Products.ReplaceOneAsync(filter: g=>g.Id==product.Id, replacement: product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount>0;
        }
    }
}