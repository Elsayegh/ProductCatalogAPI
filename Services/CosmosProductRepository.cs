using Microsoft.Azure.Cosmos;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Services
{
    public class CosmosProductRepository
    {
        private readonly Container _container;

        public CosmosProductRepository(CosmosClient client, IConfiguration config)
        {
            var databaseName = config["Cosmos:Database"];
            var containerName = config["Cosmos:Container"];

            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task<Product> CreateAsnyc(Product product)
        {
            var response = await _container.CreateItemAsync<Product>(product, new PartitionKey(product.Category));

            return response.Resource;
        }

        public async Task<Product> GetAsync(string id, string category)
        {
            var response = await _container.ReadItemAsync<Product>(id, new PartitionKey(category));

            return response.Resource;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Product>("SELECT * FROM C");

            var result = new List<Product>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                result.AddRange(response);
            }

            return result;
        }

        public async Task<Product> UpdateAsnyc(Product product)
        {
            var response = await _container.UpsertItemAsync<Product>(product, new PartitionKey(product.Category));

            return response.Resource;
        }

        public async Task DeleteAsync(string id, string category)
        {
            await _container.DeleteItemAsync<Product>(id, new PartitionKey(category));
        }
    }
}
