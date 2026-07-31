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
└── InventorySystem.Infrastructure → Implementações de persistência (JSON → futuramente EF Core)
```

---

## Domínio

**Entidades:**
- `Product` — produto com nome, ID e tipo de unidade; validação no construtor
- `InventoryItem` — item de estoque com produto, localização e quantidade
- `Location` — posição física no almoxarifado (coluna, prateleira, item); validação de coordenadas
- `Employee` — funcionário com nome, matrícula e cargo; validação no construtor

**Enums:**
- `Role` — `Operator`, `Manager`
- `UnitType` — `Unit`, `Package`, `Kg`, `Liter`

**Interfaces de repositório:**
- `IProductRepository` — GetById, Add, Update, Delete
- `IInventoryRepository` — GetByProductId, Add, Update, GetAll
- `IEmployeeRepository` — GetByRegistration, Add

---

## Infraestrutura

Implementações JSON provisórias enquanto Entity Framework Core não é integrado:

- `JsonProductRepository`
- `JsonInventoryRepository`
- `JsonEmployeeRepository`

---

## Stack

- C# / .NET 8
- ASP.NET Core Web API
- `System.Text.Json` (persistência provisória)
- Git / GitHub

---

## Status

Em desenvolvimento. Camada de Application e Controllers ainda não implementados.

**Próximos passos:**
- Implementar Controllers e endpoints REST
- Adicionar camada Application com casos de uso
- Integrar Entity Framework Core + SQL Server
- Autenticação JWT com controle de acesso por cargo
- Documentação via Swagger
- Testes unitários com xUnit
