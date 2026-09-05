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

## Autenticação & Autorização

**Framework:** ASP.NET Identity + JWT Bearer Tokens

**Fluxo:**
1. Usuário registra (cria conta com RegistrationNumber, UserName, Email, Password)
2. Usuário faz login (valida credenciais, retorna AccessToken + RefreshToken)
3. Cliente armazena tokens e usa AccessToken em requisições `[Authorize]`
4. AccessToken expira (30 min) → cliente usa RefreshToken pra gerar novo
5. Usuário faz logout (Revoke) → invalida RefreshToken

**Segurança:**
- Senhas hasheadas com salt (Identity padrão: PBKDF2)
- AccessToken: 30 minutos (curto)
- RefreshToken: 7 dias (longo), armazenado no BD em ApplicationUser
- SecretKey armazenada em User Secrets (não no Git)
- JWT assinado com HMAC-SHA256
- Claims incluem: Name, RegistrationNumber, Role, Jti

**Configuração:**

User Secrets (rodar em `InventorySystem.Api`):
```bash
dotnet user-secrets set "JWT:SecretKey" "sua_chave_gerada_com_128_bytes"
```

appsettings.json:
```json
{
  "JWT": {
    "ValidAudience": "http://localhost:5014",
    "ValidIssuer": "http://localhost:5014",
    "TokenValidityInMinutes": 30,
    "RefreshTokenValidityInDays": 7
  }
}
```

---

## Endpoints disponíveis

**Authentication** — `/api/auth`

| Método | Rota | Descrição | Body |
|--------|------|-----------|------|
| POST | `/api/auth/register` | Registra novo usuário | `RegisterRequest` |
| POST | `/api/auth/login` | Faz login e retorna tokens | `LoginRequest` |
| POST | `/api/auth/refresh-token` | Renova AccessToken com RefreshToken | `RefreshTokenRequest` |
| POST | `/api/auth/revoke` | Invalida token (logout) | — |

**Products** — `/api/products`

| Método | Rota | Descrição | Auth | Body/Query |
|--------|------|-----------|------|-----------|
| GET | `/api/products` | Lista todos os produtos | ✅ `[Authorize]` | — |
| GET | `/api/products/{id}` | Busca produto por ID | ✅ `[Authorize]` | — |
| POST | `/api/products` | Cadastra novo produto | ✅ `[Authorize]` | `ProductRequest` |
| PUT | `/api/products/{id}` | Atualiza nome do produto | ✅ `[Authorize]` | `name` (query) |
| DELETE | `/api/products/{id}` | Remove produto | ✅ `[Authorize]` | — |

**Inventory Items** — `/api/inventoryitens`

| Método | Rota | Descrição | Auth | Body/Query |
|--------|------|-----------|------|-----------|
| GET | `/api/inventoryitens` | Lista todos os itens | ✅ `[Authorize]` | — |
| GET | `/api/inventoryitens/{id}` | Busca item por ID | ✅ `[Authorize]` | — |
| GET | `/api/inventoryitens/products/{productId}` | Lista itens por ProductID | ✅ `[Authorize]` | — |
| POST | `/api/inventoryitens/{productId}` | Cadastra item | ✅ `[Authorize]` | `InventoryItemRequest` |
| PUT | `/api/inventoryitens/{id}/add-quantity` | Adiciona quantidade | ✅ `[Authorize]` | `quantity` (query) |
| PUT | `/api/inventoryitens/{id}/remove-quantity` | Remove quantidade | ✅ `[Authorize]` | `quantity` (query) |
| DELETE | `/api/inventoryitens/{id}` | Remove item | ✅ `[Authorize]` | — |

**DTOs:**

`RegisterRequest`:
```json
{
  "registrationNumber": 2052897,
  "userName": "gabriel",
  "email": "gabriel@example.com",
  "password": "Password@123"
}
```

`LoginRequest`:
```json
{
  "registrationNumber": 2052897,
  "password": "Password@123"
}
```

`LoginResponse`:
```json
{
  "success": true,
  "message": "Login successful",
  "token": {
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "V0FTLzBiMzQyZDM0MzZlZjMwYTMwYTcyNjE3NTc3ODQzNjQ2..."
  }
}
```

`RefreshTokenRequest`:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "V0FTLzBiMzQyZDM0MzZlZjMwYTMwYTcyNjE3NTc3ODQzNjQ2..."
}
```

`ProductRequest`:
```json
{
  "name": "string (5-80 chars)",
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

**1. Clone o repositório:**
```bash
git clone https://github.com/gabriel-biagi/InventorySystem.api
cd InventorySystem.api/InventorySystem.api
```

**2. Configure User Secrets (chaves sensíveis):**

Navegue até `InventorySystem.Api`:
```bash
cd InventorySystem.Api
```

Configure a connection string:
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=CatalogDB;Uid=root;Pwd=suasenha"
```

Configure a chave secreta JWT (gere uma chave aleatória segura de 128 bytes):
```bash
dotnet user-secrets set "JWT:SecretKey" "sua_chave_super_secreta_aqui_com_128_caracteres_minimo"
```

**3. Rode as migrations:**

De volta na raiz do projeto:
```bash
cd ..
dotnet ef database update --project ./InventorySystem.Infrastructure/InventorySystem.Infrastructure.csproj --startup-project ./InventorySystem.Api/InventorySystem.Api.csproj
```

**4. Rode a API:**
```bash
dotnet run --project InventorySystem.Api
```

**5. Acesse o Swagger:**
Abra no navegador: `https://localhost:7102/swagger` (a porta pode variar)

**6. Teste o fluxo de autenticação:**
- POST `/api/auth/register` → cria usuário
- POST `/api/auth/login` → retorna tokens
- Copie o `accessToken`
- Clique no botão "Authorize" no Swagger
- Cole: `Bearer {accessToken}`
- Faça requisição em `/api/products` (agora autenticada)

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

Em desenvolvimento ativo. Pronto para júnior com mentorado.

**Concluído:**
- ✅ Domínio com entidades e regras de negócio (Product, InventoryItem, Location, Employee)
- ✅ API REST com CRUD de produtos e itens de estoque
- ✅ Movimentação de entrada e saída de estoque (AddQuantity, RemoveQuantity)
- ✅ Persistência com EF Core 8 + MySQL 8 (Pomelo)
- ✅ Repository Pattern (IProductRepository, IInventoryRepository)
- ✅ Arquitetura em camadas (Domain → Application → Infrastructure → Api)
- ✅ DTOs + AutoMapper 12.0.1
- ✅ Application Services orquestrando lógica (ProductService, InventoryItemService)
- ✅ Middleware global para tratamento de exceções (ErrorDetails)
- ✅ Logging de requisições e tempo de execução (ApiLoggingFilter)
- ✅ **Autenticação JWT** — Register, Login, RefreshToken, Revoke
  - ApplicationUser customizado (RegistrationNumber, RefreshToken, RefreshTokenExpires)
  - TokenService: GenerateAccessToken, GenerateRefreshToken, GetPrincipalFromExpiredToken
  - AuthController com 4 endpoints de autenticação
  - Claims: Name, RegistrationNumber, Role, Jti
  - SecretKey em User Secrets (seguro)
- ✅ Testes unitários com xUnit + Moq (20 testes)
  - 15 testes Domain (Product, InventoryItem, Location)
  - 5 testes Services (ProductService, InventoryItemService)
  - Cobertura de regras críticas de negócio

**Em Progress:**
- 🔄 Autorização baseada em Roles/Policies (próximas aulas 144-151)

**Próximos passos:**
- Role-based authorization (`[Authorize(Roles = "Admin")]`)
- Policy-based authorization (regras customizadas)
- FluentValidation para validação robusta de DTOs
- Testes de integração (Controllers)
- HttpPatch para atualizações parciais
- Paginação e filtros nos endpoints de listagem
- Rate limiting
- Criptografia de RefreshToken no BD (não apenas hash)

---

## Aprendizado & Desenvolvimento

Este projeto é fruto de **mentoria estruturada em autenticação JWT**.

**Conceitos cobertos:**
- Hash de senha one-way + salt (PBKDF2)
- Estrutura JWT (Header.Payload.Signature)
- AccessToken (curto) vs RefreshToken (longo)
- Claims e sua extração
- Validação de Signature com chave secreta
- Fluxo completo: Login → Token generation → Refresh → Revoke

**Nível:** Júnior com fundação sólida em segurança

**Lacunas documentadas:**
- ❌ Experiência zero em produção (nunca debugou sob pressão)
- ❌ Segurança conhecida superficialmente (não profunda)
- ❌ Nunca implementou criptografia de tokens
- ❌ Defesa contra ataque limitada (pensa pequeno ainda)

**Recomendações para Sênior:**
- Code review rigoroso em autenticação/autorização
- Mentor dedicado para primeiros 12-18 meses
- Exposição gradual a segurança em produção
