# Oficina API — Tech Challenge Fase 1

Monólito em camadas para gestão de clientes, veículos, itens de catálogo e ordens de serviço de uma oficina mecânica.

## Arquitetura

```text
Oficina.Api
├── Program/Startup, configurações globais, DI e middlewares
│
Oficina.Application
├── Controllers, Requests e Services
│
Oficina.Domain
├── Entities, Enums, Dtos e Interfaces
│
Oficina.Infra.Data
├── Context, Mappings e Repositories
│
Oficina.Infra.Data.Postgres
└── Migrations geradas pelo EF Core
```

As controllers não acessam o banco diretamente. O fluxo é `Controller → Service → Repository → PostgreSQL`. A API apenas faz composição e configuração; as regras de negócio ficam em `Application` e `Domain`.

## Endpoints

Cada entidade possui criação, consulta por id, listagem paginada, atualização e exclusão lógica:

```text
GET    /api/clientes?term=&offset=0&limit=20
POST   /api/clientes
GET    /api/clientes/{id}
PUT    /api/clientes/{id}
DELETE /api/clientes/{id}

GET    /api/veiculos?term=&offset=0&limit=20
GET    /api/itens-catalogo?term=&offset=0&limit=20
GET    /api/ordens-servico?term=&offset=0&limit=20
```

O `POST` e o `PUT` retornam o `Guid` do registro. O `DELETE` grava `DataExclusao`; registros excluídos não aparecem nas consultas normais. O limite de página é 100.

## Executar

Pré-requisitos: .NET SDK 8 e Docker Desktop.

```bash
docker compose up -d postgres
dotnet restore
dotnet ef database update --context Oficina.Infra.Data.Context.OficinaDbContext --project src/Oficina.Infra.Data.Postgres --startup-project src/Oficina.Api
dotnet run --project src/Oficina.Api
```

Swagger: `http://localhost:5080/swagger`.

PostgreSQL está definido em [docker-compose.yml](./docker-compose.yml), usando `postgres:16-alpine`, volume persistente e healthcheck.

Para gerar uma migration:

```bash
dotnet ef migrations add NomeDaMigration --context Oficina.Infra.Data.Context.OficinaDbContext --project src/Oficina.Infra.Data.Postgres --startup-project src/Oficina.Api --output-dir Migrations
```
