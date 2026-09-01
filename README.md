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
│   └── Services/
│       ├── Interfaces/
│       │   ├── IInventoryItemService.cs
│       │   └── IProductService.cs
│       ├── InventoryItemService.cs
│       └── ProductService.cs
├── InventorySystem.Domain/
│   ├── Entities/
│   │   ├── Employee.cs
│   │   ├── InventoryItem.cs
│   │   ├── Location.cs
│   │   └── Product.cs
│   ├── Enums/
│   │   ├── Role.cs
│   │   └── UnitType.cs
│   ├── Exceptions/
│   │   ├── BusinessException.cs
│   │   ├── DomainException.cs
│   │   └── NotFoundException.cs
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

## Testes

**Framework:** xUnit + Moq

**Executar testes:**
```bash
dotnet test
```

**Cobertura:** 20 testes unitários cobrindo Domain entities e Application Services.

### Domain Entity Tests (15 testes)

**ProductTests (8 testes)**
- ✅ `UpdateName_WhenNameIsInvalid_ThrowsArgumentException` — **[Business Rule]** Rejeita nomes < 5 caracteres
- ✅ `UpdateName_WhenNameIsWhiteSpace_ThrowsArgumentException` — Rejeita whitespace
- ✅ `UpdateName_WhenNameIsNull_ThrowsArgumentException` — Rejeita null
- ✅ `UpdateName_WhenNameHasMoreThan80Characters_ThrowsArgumentException` — **[Business Rule]** Rejeita nomes > 80 caracteres
- ✅ `UpdateName_WhenNameIsInMinimumAndMaximumCharacterLimit_UpdatesNameSuccessfully` (3 cenários via `[Theory]`) — **[Business Rule]** Aceita nomes entre 5-80 caracteres

**InventoryItemTests (5 testes)**
- ✅ `AddQuantity_WhenQuantityIsNegative_ThrowsArgumentException` — **[Business Rule - CRÍTICO]** Rejeita quantidade ≤ 0
- ✅ `AddQuantity_WhenQuantityAreValid_AddQuantitySuccessfully` — **[Business Rule - CRÍTICO]** Adiciona quantidade corretamente ao estoque
- ✅ `RemoveQuantity_WhenQuantityIsNegative_ThrowsArgumentException` — **[Business Rule - CRÍTICO]** Rejeita remoção ≤ 0
- ✅ `RemoveQuantity_WhenQuantityIsGreaterThanInStock_ThrowsArgumentException` — **[Business Rule - CRÍTICO]** Rejeita remoção > estoque disponível
- ✅ `RemoveQuantity_WhenQuantityAreValid_RemoveQuantitySuccessfully` — **[Business Rule - CRÍTICO]** Remove quantidade corretamente do estoque

**LocationTests (2 testes)**
- ✅ `CreateLocation_WhenLocationLessThan0_ThrowsArgumentException` — Rejeita coordenadas < 1
- ✅ `CreateLocation_WhenAllValuesAreValid_CreatesSuccessfully` — Cria localização com coordenadas válidas

### Service Tests (5 testes)

**ProductServiceTests (3 testes)**
- ✅ `GetByIdAsync_WhenIdIsInvalid_ThrowsArgumentException` — **[Business Rule]** Rejeita ID ≤ 0
- ✅ `GetByIdAsync_WhenProductIsNull_ThrowsNotFoundException` — **[Business Rule]** Produto não encontrado lança exceção
- ✅ `DeleteAsync_WhenProductIsInStock_ThrowsBusinessException` — **[Business Rule - CRÍTICO]** Impede deletar produto com itens em estoque

**InventoryItemServiceTests (2 testes)**
- ✅ `UpdateAsync_WhenQuantityIsNotIntenger_ThrowsBusinessException` — **[Business Rule - CRÍTICO]** Unit/Package types devem ter quantidade inteira
- ✅ `AddAsync_WhenParametersAreValid_AddAsyncSuccessfully` — Cria InventoryItem com sucesso retornando DTO mapeado

---

## Status

Em desenvolvimento ativo.

**Concluído:**
- ✅ Domínio com entidades e regras de negócio
- ✅ API REST com CRUD de produtos e itens de estoque
- ✅ Movimentação de entrada e saída de estoque
- ✅ Persistência com EF Core + MySQL
- ✅ Repository Pattern e arquitetura em camadas
- ✅ DTOs + AutoMapper
- ✅ Services para orquestração da lógica de aplicação
- ✅ Middleware global para tratamento de exceções
- ✅ Logging de requisições e tempo de execução
- ✅ Testes unitários com xUnit + Moq, cobrindo regras críticas de negócio

**Próximos passos:**
- FluentValidation para validação robusta de DTOs
- Autenticação JWT com controle de acesso por cargo
- HttpPatch para atualizações parciais
- Paginação e filtros nos endpoints de listagem
- Testes de integração (Controllers)
