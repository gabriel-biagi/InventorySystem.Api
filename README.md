# InventorySystem — Web API

Sistema de gestão de estoque em ASP.NET Core Web API, construído sobre a mesma base de domínio do [InventorySystemConsole](https://github.com/gabriel-biagi/InventorySystemConsole) — migrado para arquitetura em camadas com foco em escalabilidade e boas práticas .NET.

---

## Arquitetura

O projeto segue uma arquitetura em camadas inspirada em Clean Architecture:

```
InventorySystem/
├── InventorySystem.Api           → Endpoints HTTP, Controllers, configuração
├── InventorySystem.Application   → Casos de uso, lógica de aplicação, DTOs
├── InventorySystem.Domain        → Entidades, enums, interfaces de repositório
└── InventorySystem.Infrastructure → Implementações de persistência (EF Core + MySQL)
```

---

## Domínio

**Entidades:**
- `Product` — produto com nome, ID e tipo de unidade; validação no construtor
- `InventoryItem` — item de estoque com produto, localização e quantidade
- `Location` — posição física no almoxarifado (coluna, prateleira, item); validação de coordenadas; mapeada como Owned Type do EF Core
- `Employee` — funcionário com nome, matrícula e cargo; validação no construtor

**Enums:**
- `Role` — `Operator`, `Manager`
- `UnitType` — `Unit`, `Package`, `Kg`, `Liter`

**Interfaces de repositório:**
- `IProductRepository` — GetById, Add, Update, Delete
- `IInventoryRepository` — GetByProductId, Add, Update, GetAll
- `IEmployeeRepository` — GetByRegistration, Add

---

## Stack

- C# / .NET 8
- ASP.NET Core Web API
- Entity Framework Core + MySQL
- Swagger / OpenAPI
- Git / GitHub

---

## Endpoints disponíveis

**Products**
- `GET /products` — lista todos os produtos
- `GET /products/{id}` — busca produto por ID
- `POST /products` — cadastra produto
- `PUT /products/{id}` — atualiza nome do produto
- `DELETE /products/{id}` — remove produto

---

## Como rodar

```bash
git clone https://github.com/gabriel-biagi/InventorySystem
cd InventorySystem
dotnet ef database update --project InventorySystem.Infrastructure --startup-project InventorySystem.Api
dotnet run --project InventorySystem.Api
```

Configure a connection string via User Secrets no projeto `InventorySystem.Api`.

---

## Status

Em desenvolvimento ativo.

**Concluído:**
- ✅ Domínio com entidades e validações
- ✅ Entity Framework Core com MySQL
- ✅ Migrations e seed de dados iniciais
- ✅ ProductsController com CRUD completo

**Próximos passos:**
- Repository Pattern e separação de responsabilidades
- Camada Application com Services
- DTOs com AutoMapper
- InventoryItems endpoints
- Autenticação JWT com controle de acesso por cargo
- Testes unitários com xUnit
