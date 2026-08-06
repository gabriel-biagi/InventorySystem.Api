# InventorySystem — Web API

Sistema de gestão de estoque em ASP.NET Core Web API, construído sobre a mesma base de domínio do [InventorySystemConsole](https://github.com/gabriel-biagi/InventorySystemConsole) — migrado para arquitetura em camadas com foco em escalabilidade e boas práticas .NET.

---

## Arquitetura

O projeto segue uma arquitetura em camadas inspirada em Clean Architecture:

```
InventorySystem/
├── InventorySystem.Api           → Endpoints HTTP, Controllers, configuração
├── InventorySystem.Application   → Casos de uso, lógica de aplicação, DTOs (em implementação)
├── InventorySystem.Domain        → Entidades, enums, interfaces de repositório
└── InventorySystem.Infrastructure → Persistência com EF Core + MySQL
```

---

## Estrutura do Projeto

```
InventorySystem/
├── InventorySystem.Api/
│   ├── Controllers/
│   │   ├── InventoryItensController.cs
│   │   └── ProductsController.cs
│   ├── appsettings.json
│   └── Program.cs
├── InventorySystem.Application/
├── InventorySystem.Domain/
│   ├── Entities/
│   │   ├── Employee.cs
│   │   ├── InventoryItem.cs
│   │   ├── Location.cs
│   │   └── Product.cs
│   ├── Enums/
│   │   ├── Role.cs
│   │   └── UnitType.cs
│   └── Interfaces/
│       ├── IEmployeeRepository.cs
│       ├── IInventoryRepository.cs
│       └── IProductRepository.cs
└── InventorySystem.Infrastructure/
    ├── Context/
    │   └── AppDbContext.cs
    └── Migrations/
```

---

## Domínio

**Entidades:**
- `Product` — produto com nome, ID e tipo de unidade; validação de nome no construtor (mínimo 5 caracteres)
- `InventoryItem` — item de estoque com produto, localização e quantidade; métodos `AddQuantity` e `RemoveQuantity` com validação de domínio
- `Location` — posição física no almoxarifado (coluna, prateleira, item); validação de coordenadas; mapeada como Owned Type do EF Core
- `Employee` — funcionário com nome, matrícula e cargo; validação de nome no construtor

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
- Entity Framework Core + MySQL (Pomelo)
- Swagger / OpenAPI
- Git / GitHub

---

## Endpoints disponíveis

**Products** — `/products`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/products` | Lista todos os produtos |
| GET | `/products/{id}` | Busca produto por ID |
| POST | `/products` | Cadastra produto |
| PUT | `/products/{id}` | Atualiza nome do produto |
| DELETE | `/products/{id}` | Remove produto |

**Inventory Items** — `/inventoryitens`

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/inventoryitens` | Lista todos os itens de estoque |
| GET | `/inventoryitens/{id}` | Busca item por ID |
| POST | `/inventoryitens/products/{productId}` | Cadastra item vinculado a um produto |
| PUT | `/inventoryitens/{id}/add-quantity` | Adiciona quantidade ao estoque |
| PUT | `/inventoryitens/{id}/remove-quantity` | Remove quantidade do estoque |
| DELETE | `/inventoryitens/{id}` | Remove item do estoque |

---

## Como rodar

```bash
git clone https://github.com/gabriel-biagi/InventorySystem
cd InventorySystem
dotnet ef database update --project InventorySystem.Infrastructure --startup-project InventorySystem.Api
dotnet run --project InventorySystem.Api
```

Configure a connection string via User Secrets no projeto `InventorySystem.Api`:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=InventoryDB;Uid=root;Pwd=suasenha"
```

Acesse o Swagger em `https://localhost:{porta}/swagger`.

---

## Status

Em desenvolvimento ativo.

**Concluído:**
- ✅ Domínio com entidades e validações de negócio
- ✅ Entity Framework Core com MySQL
- ✅ Migrations e seed de dados iniciais (8 produtos de almoxarifado)
- ✅ ProductsController com CRUD completo
- ✅ InventoryItemsController com CRUD e endpoints de movimentação de quantidade

**Próximos passos:**
- Repository Pattern com injeção de dependência
- Camada Application com Services
- Actions assíncronas (`async/await`)
- DTOs com AutoMapper
- Middleware de tratamento de erros global
- Autenticação JWT com controle de acesso por cargo
- Testes unitários com xUnit
