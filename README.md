# InventorySystem — Web API

API REST para gestão de estoque, desenvolvida com **ASP.NET Core Web API**, **Entity Framework Core** e **MySQL**.

O projeto evoluiu da base de domínio do [InventorySystemConsole](https://github.com/gabriel-biagi/InventorySystemConsole) para uma aplicação web estruturada em camadas, com autenticação JWT, autorização por roles/policies, persistência relacional, DTOs e testes unitários.

---

## Visão geral

- CRUD de produtos e itens de estoque
- Entrada e saída de quantidade em estoque
- Validações e regras de negócio no domínio
- Persistência com Entity Framework Core + MySQL
- Repository Pattern
- DTOs + AutoMapper
- Autenticação com ASP.NET Identity + JWT
- Refresh Token e revogação de sessão
- Autorização baseada em Roles e Policies
- Middleware global para tratamento de exceções
- Logging de requisições
- Swagger / OpenAPI
- Testes unitários com xUnit, Moq e FluentAssertions

---

## Arquitetura

Arquitetura em camadas inspirada em **Clean Architecture**, separando responsabilidades entre domínio, aplicação, infraestrutura e API.

```text
InventorySystem/
├── InventorySystem.Api           → Controllers, HTTP e configuração
├── InventorySystem.Application   → Services, DTOs e casos de uso
├── InventorySystem.Domain        → Entidades, enums, regras e interfaces
└── InventorySystem.Infrastructure → EF Core, Identity, MySQL e repositórios
```

### Responsabilidades

**Domain**
- `Product`, `InventoryItem` e `Location`
- Regras de negócio e validações
- Interfaces dos repositórios
- `UnitType` e exceções de domínio

**Application**
- `ProductService`, `InventoryItemService` e `TokenService`
- DTOs de Request/Response
- Mapeamentos com AutoMapper
- Orquestração dos casos de uso

**Infrastructure**
- `AppDbContext`
- Entity Framework Core + Pomelo MySQL
- ASP.NET Core Identity
- `ApplicationUser`
- Implementações dos repositórios
- Migrations

**Api**
- Controllers
- Autenticação e autorização
- Swagger
- Middleware de exceções
- Filtro de logging

---

## Domínio

### Entidades

- **Product** — produto com nome e tipo de unidade. Nome entre 5 e 80 caracteres.
- **InventoryItem** — produto armazenado em uma localização, com operações de entrada e saída de quantidade.
- **Location** — posição física composta por coluna, prateleira e item.
- **ApplicationUser** — usuário baseado em `IdentityUser`, com matrícula e dados para controle de Refresh Token.

### Regras de negócio

- Nome de produto deve possuir entre 5 e 80 caracteres.
- Quantidade adicionada ou removida deve ser maior que zero.
- Não é possível remover quantidade superior ao estoque disponível.
- Para produtos `Unit` ou `Package`, a quantidade deve ser inteira.
- Uma localização não pode ser ocupada por mais de um item do mesmo produto.
- Produto com itens em estoque não pode ser excluído.
- IDs inválidos são rejeitados pelos Services e pelas rotas da API.

---

## Autenticação e autorização

A autenticação utiliza **ASP.NET Core Identity + JWT Bearer**.

### Fluxo

1. Registro de usuário com matrícula, usuário, e-mail e senha.
2. Usuário autenticado recebe `AccessToken` e `RefreshToken` no login.
3. O Access Token possui validade configurada de **30 minutos**.
4. O Refresh Token possui validade configurada de **3 dias** e é armazenado no usuário.
5. O Refresh Token pode ser utilizado para gerar novos tokens.
6. O endpoint de revoke invalida o Refresh Token armazenado.

O JWT utiliza **HMAC-SHA256** e inclui claims de nome, matrícula, role e `Jti`.

### Roles

| Role | Acesso |
|------|--------|
| `Gestor` | Operações de leitura, criação, atualização e exclusão |
| `Manager` | Leitura de produtos/estoque, atualização de produtos e movimentação/cadastro de estoque |

As políticas configuradas são `GestorOnly`, `ManagerOnly` e `ManagerOrGestor`.

> A criação de Roles e a associação de usuários às Roles também são protegidas pela policy `GestorOnly`.

---

## Endpoints

### Authentication — `/api/auth`

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| POST | `/register` | Público | Registra usuário |
| POST | `/login` | Público | Retorna Access Token + Refresh Token |
| POST | `/refresh-token` | `GestorOnly` | Gera novos tokens |
| POST | `/revoke` | Público | Revoga Refresh Token |
| POST | `/CreateRole` | `GestorOnly` | Cria uma Role |
| POST | `/AddUserToRole` | `GestorOnly` | Adiciona usuário a uma Role |

### Products — `/api/products`

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| GET | `/` | `ManagerOrGestor` | Lista produtos |
| GET | `/{id}` | `ManagerOrGestor` | Busca produto |
| POST | `/` | `GestorOnly` | Cadastra produto |
| PUT | `/{id}` | `ManagerOrGestor` | Atualiza nome |
| DELETE | `/{id}` | `GestorOnly` | Remove produto |

### Inventory Items — `/api/inventoryitens`

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| GET | `/` | `ManagerOrGestor` | Lista itens |
| GET | `/{id}` | `ManagerOrGestor` | Busca item |
| GET | `/products/{productId}` | `ManagerOrGestor` | Lista itens de um produto |
| POST | `/{productId}` | `ManagerOrGestor` | Cadastra item |
| PUT | `/{id}/add-quantity` | `ManagerOrGestor` | Adiciona quantidade |
| PUT | `/{id}/remove-quantity` | `ManagerOrGestor` | Remove quantidade |
| DELETE | `/{id}` | `GestorOnly` | Remove item |

---

## Stack

| Categoria | Tecnologias |
|---|---|
| Linguagem | C# / .NET 8 |
| API | ASP.NET Core Web API |
| ORM | Entity Framework Core 8.0.11 |
| Banco | MySQL + Pomelo.EntityFrameworkCore.MySql 8.0.2 |
| Identidade | ASP.NET Core Identity |
| Autenticação | JWT Bearer |
| Mapping | AutoMapper 12.0.1 |
| Documentação | Swagger / OpenAPI — Swashbuckle 6.6.2 |
| Testes | xUnit, Moq, FluentAssertions |
| Controle de versão | Git / GitHub |

---

## Configuração e execução

### 1. Clone o projeto

```bash
git clone https://github.com/gabriel-biagi/InventorySystem.Api.git
cd InventorySystem.Api
```

### 2. Configure User Secrets

Entre no projeto da API:

```bash
cd InventorySystem.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=CatalogDB;Uid=root;Pwd=suasenha"
dotnet user-secrets set "JWT:SecretKey" "sua_chave_secreta_forte"
```

A chave JWT não deve ser armazenada no repositório.

### 3. Aplique as migrations

Na raiz da solução:

```bash
dotnet ef database update --project ./InventorySystem.Infrastructure/InventorySystem.Infrastructure.csproj --startup-project ./InventorySystem.Api/InventorySystem.Api.csproj
```

### 4. Execute a API

```bash
dotnet run --project InventorySystem.Api
```

Em ambiente de desenvolvimento, o Swagger fica disponível em:

```text
https://localhost:<porta>/swagger
```

### 5. Execute os testes

```bash
dotnet test
```

---

## Testes

O projeto possui **20 casos de teste unitário**, distribuídos entre Domain e Application Services.

**Domain — 15 casos**
- `Product`: validação de nome
- `InventoryItem`: entrada, saída e disponibilidade de estoque
- `Location`: validação das coordenadas

**Application Services — 5 casos**
- `ProductService`: validação de ID, produto inexistente e exclusão com estoque
- `InventoryItemService`: compatibilidade de quantidade e criação de item

Os testes priorizam regras de negócio e comportamentos que possuem impacto direto no domínio e nos Services.

---

## Migrations

As principais migrations atualmente presentes no projeto são:

- `Initial`
- `RenameIdsToPascalCase`
- `AddDataAnnotationsToEntities`
- `SeedInitialProducts`
- `AddUniqueIndexToProductName`
- `CriaTabelasIdentity`
- `AddApplicationUserProperties`

---

## Estrutura atual

```text
InventorySystem.Api/
├── Controllers/
├── Extensions/
├── Filters/
├── Middlewares/
└── Program.cs

InventorySystem.Application/
├── DTOs/
│   ├── Mappings/
│   ├── Request/
│   └── Response/
└── Services/

InventorySystem.Domain/
├── Entities/
├── Enums/
├── Exception/
└── Interfaces/

InventorySystem.Infrastructure/
├── Context/
├── Identity/
├── Migrations/
└── Repositories/

InventorySystem.UnitTests/
├── Domain/
└── Services/
```
