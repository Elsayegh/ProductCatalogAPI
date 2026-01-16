using Microsoft.Azure.Cosmos;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Services
{
    public class ChangeFeedService
    {
        private readonly Container _container;
        private readonly Container _leaseContainer;

        public ChangeFeedService(CosmosClient client)
        {
            var database = client.GetDatabase("Products");
            _container = database.GetContainer("ProductDB");
            _leaseContainer = database.GetContainer("leases");
        }

        public void Start()
        {
            var processor = _container
                .GetChangeFeedProcessorBuilder<Product>(
                processorName: "productChangeFeed",
                onChangesDelegate: async (context, changes, CancellationToken) =>
                {
                    foreach (var product in changes)
                    {
                        Console.WriteLine($"Detected change: {product.Id} - {product.Name}");
                    }
                })
                .WithInstanceName("ProductCatalogAPI")
                .WithLeaseContainer(_leaseContainer)
                .Build();

            processor.StartAsync();
        }
    }
}
