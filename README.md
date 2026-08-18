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
│   ├── Extensions/
│   │   └── ApiExceptionMiddlewareExtensions.cs
│   ├── Filters/
│   │   └── ApiLoggingFilter.cs
│   ├── Middlewares/
│   │   └── ErrorDetails.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── Program.cs
├── InventorySystem.Application/
│   ├── DTOs/
│   │   ├── Mappings/
│   │   │   ├── InventoryItemRequestMappingProfile.cs
│   │   │   ├── InventoryItemResponseMappingProfile.cs
│   │   │   ├── ProductRequestMappingProfile.cs
│   │   │   └── ProductResponseMappingProfile.cs
│   │   ├── Request/
│   │   │   ├── InventoryItemRequest.cs
│   │   │   └── ProductRequest.cs
│   │   └── Response/
│   │       ├── InventoryItemResponse.cs
│   │       └── ProductResponse.cs
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
    │   ├── AppDbContext.cs
    │   └── AppDbContextModelSnapshot.cs
    ├── Migrations/
    │   ├── 20260803183539_Initial.cs
    │   ├── 20260803203016_RenameIdsToPascalCase.cs
    │   ├── 20260803205557_AddDataAnnotationsToEntities.cs
    │   └── 20260803211320_SeedInitialProducts.cs
    └── Repositories/
        ├── EfInventoryRepository.cs
        └── EfProductRepository.cs
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

**Language & Framework:**
- C# / .NET 8
- ASP.NET Core Web API

**Database & ORM:**
- Entity Framework Core 8.0.11
- Pomelo.EntityFrameworkCore.MySql 8.0.2
- MySQL

**API & Documentation:**
- Swagger / OpenAPI (Swashbuckle.AspNetCore 6.6.2)

**Mapping & AutoMapper:**
- AutoMapper 12.0.1
- AutoMapper.Extensions.Microsoft.DependencyInjection 12.0.1

**Development Tools:**
- Microsoft.EntityFrameworkCore.Design 8.0.11
- Git / GitHub

---

## Endpoints disponíveis

**Products** — `/api/products`

| Método | Rota | Descrição | Body/Query |
|--------|------|-----------|-----------|
| GET | `/api/products` | Lista todos os produtos | — |
| GET | `/api/products/{id}` | Busca produto por ID | — |
| POST | `/api/products` | Cadastra novo produto | `ProductRequest` (JSON) |
| PUT | `/api/products/{id}` | Atualiza nome do produto | `name` (query string) |
| DELETE | `/api/products/{id}` | Remove produto | — |

**Inventory Items** — `/api/inventoryitens`

| Método | Rota | Descrição | Body/Query |
|--------|------|-----------|-----------|
| GET | `/api/inventoryitens` | Lista todos os itens de estoque | — |
| GET | `/api/inventoryitens/{id}` | Busca item por ID do inventário | — |
| GET | `/api/inventoryitens/products/{productId}` | Lista itens por ProductID | — |
| POST | `/api/inventoryitens/{productId}` | Cadastra item vinculado a um produto | `InventoryItemRequest` (JSON) |
| PUT | `/api/inventoryitens/{id}/add-quantity` | Adiciona quantidade ao estoque | `quantity` (query string) |
| PUT | `/api/inventoryitens/{id}/remove-quantity` | Remove quantidade do estoque | `quantity` (query string) |
| DELETE | `/api/inventoryitens/{id}` | Remove item do estoque | — |

**DTOs:**

`ProductRequest`:
```json
{
  "name": "string (80 chars max)",
  "unitType": "Unit | Package | Kg | Liter"
}
```

`InventoryItemRequest`:
```json
{
  "column": "int (min: 1)",
  "shelf": "int (min: 1)",
  "item": "int (min: 1)",
  "quantity": "decimal"
}
```

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
- ✅ Entity Framework Core com MySQL 8.0.2
- ✅ Migrations e seed de dados iniciais (8 produtos de almoxarifado)
- ✅ ProductsController e InventoryItensController com CRUD completo
- ✅ Endpoints de movimentação de quantidade (add/remove)
- ✅ Actions assíncronas (`async/await`) em todos os endpoints
- ✅ Middleware global de tratamento de erros com `ErrorDetails` e ambiente-aware StackTrace
- ✅ ApiLoggingFilter para logging de requisições e tempo de execução
- ✅ Repository Pattern (EFProductRepository, EFInventoryRepository) com injeção de dependência
- ✅ Camada Application com DTOs (Request/Response)
- ✅ AutoMapper 12.0.1 para mapeamento automático de entidades ↔ DTOs
- ✅ Validação de null em repositórios com fallback para controllers

**Próximos passos:**
- Camada Application com Application Services
- HttpPatch para atualizações parciais
- Autenticação JWT com controle de acesso por cargo
- Testes unitários com xUnit
- Melhorias no tratamento de exceções customizadas
