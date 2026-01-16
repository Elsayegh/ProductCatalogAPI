using Microsoft.Azure.Cosmos;
using ProductCatalogAPI.Models;
using System.Drawing;

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
            Console.WriteLine($"Request Charge: {response.RequestCharge} RU");


            return response.Resource;

        }

        public async Task<Product> GetAsync(string id, string category)
        {
            var response = await _container.ReadItemAsync<Product>(id, new PartitionKey(category));
            Console.WriteLine($"Request Charge: {response.RequestCharge} RU");

            return response.Resource;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var query = _container.GetItemQueryIterator<Product>("SELECT * FROM C");

            var result = new List<Product>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                Console.WriteLine($"Request Charge: {response.RequestCharge} RU");
                result.AddRange(response);
            }

            return result;
        }

        public async Task<Product> UpdateAsnyc(Product product)
        {
            var response = await _container.UpsertItemAsync<Product>(product, new PartitionKey(product.Category));
            Console.WriteLine($"Request Charge: {response.RequestCharge} RU");

            return response.Resource;
        }

        public async Task DeleteAsync(string id, string category)
        {
            await _container.DeleteItemAsync<Product>(id, new PartitionKey(category));
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.category = @category").WithParameter("@category", category);

            var iterator = _container.GetItemQueryIterator<Product>(
                query,
                requestOptions: new QueryRequestOptions
                {
                    PartitionKey = new PartitionKey(category),
                });

            var result = new List<Product>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                Console.WriteLine($"Request Charge: {response.RequestCharge} RU");

                result.AddRange(response);
            }

            return result;
        }

        public async Task<IEnumerable<Product>> GetPagedAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;

            var query = new QueryDefinition(
                "SELECT * FROM c OFFSET @skip LIMIT @limit"
            )
            .WithParameter("@skip", skip)
            .WithParameter("@limit", pageSize);

            var iterator = _container.GetItemQueryIterator<Product>(query);

            var results = new List<Product>();

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();

                Console.WriteLine($"Request Charge: {response.RequestCharge} RU");
                Console.WriteLine(response.Diagnostics.ToString());

                results.AddRange(response);
            }

            return results;
        }

        public async Task<int> BatchInsertAsync(string category, IEnumerable<Product> products)
        {
            var partitionKey = new PartitionKey(category);
            var response = await _container.Scripts.ExecuteStoredProcedureAsync<dynamic>(
                "spBatchInsert", partitionKey,
                new object[] { products });

            return (int)response.Resource.inserted;
        }
    }
}
