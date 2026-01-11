# Product Catalog API (ASP.NET Core + Azure Cosmos DB)

A production‑grade REST API built with **ASP.NET Core 8** and backed by **Azure Cosmos DB**.  
This project was created as a hands‑on deep dive into building cloud‑ready microservices, focusing on:

- Real Cosmos DB integration (not an emulator)
- Partition keys, containers, and throughput considerations
- Async repository patterns
- Clean controller design
- Dependency injection
- Swagger/OpenAPI documentation

This was **heavy work**, especially around Cosmos DB’s SDK, partitioning model, and async data access patterns — but the result is a clean, scalable API ready for real cloud deployment.

---

## 🚀 Features

- Full CRUD operations for Products
- Cosmos DB integration using `CosmosClient`
- Repository pattern for clean data access
- Swagger UI for interactive API testing
- Async/await for high scalability
- Clean separation of Models, Services, and Controllers

---

## 🧱 Tech Stack

- **ASP.NET Core 8**
- **Azure Cosmos DB (Core SQL API)**
- **Microsoft.Azure.Cosmos SDK**
- **Newtonsoft.Json**
- **Swagger / OpenAPI**

---

## 📦 Endpoints

### `GET /api/Products`
Returns all products.

### `GET /api/Products/{id}/{category}`
Returns a single product by ID and partition key.

### `POST /api/Products`
Creates a new product.

### `PUT /api/Products/{id}/{category}`
Updates an existing product.

### `DELETE /api/Products/{id}/{category}`
Deletes a product.

---


