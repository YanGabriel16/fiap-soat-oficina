# Oficina API — Tech Challenge Fase 1

API REST para gestão do atendimento de uma oficina mecânica, cobrindo clientes, veículos, catálogo de serviços e peças, ordens de serviço, orçamentos, estoque, histórico e relatórios operacionais.

## Objetivo do projeto

Digitalizar o fluxo de uma oficina: cadastrar clientes e veículos, abrir ordens de serviço, elaborar orçamentos, controlar aprovação ou recusa, movimentar o estoque e acompanhar o histórico de cada operação.

## Descrição da solução

A aplicação implementa uma API REST com autenticação por JWT e autorização por perfis (`Admin`, `Atendente` e `Cliente`). O fluxo principal é:

1. Cadastro e autenticação do usuário.
2. Cadastro do cliente, veículo e itens do catálogo.
3. Abertura da ordem de serviço com orçamento.
4. Baixa automática dos itens utilizados no estoque.
5. Aprovação ou recusa do orçamento pelo cliente.
6. Reposição automática do estoque quando o orçamento é recusado.
7. Controle do ciclo de status da ordem e consulta dos históricos.
8. Consulta de relatórios de tempo, orçamento, status e estoque.

## Arquitetura utilizada

O projeto utiliza uma arquitetura monolítica em camadas, com separação inspirada em DDD:

```text
Controller → Service → Repository → Entity Framework Core → PostgreSQL
```

- `Api`: inicialização, configurações, autenticação, Swagger e middlewares.
- `Application`: controllers, requests, services e regras de aplicação.
- `Domain`: entidades, enums, DTOs, interfaces e regras de domínio.
- `Infra.Data`: contexto EF Core, mapeamentos e repositories.
- `Infra.Data.Postgres`: provider PostgreSQL e migrations.

Os controllers não acessam o banco diretamente. As regras ficam nos services e no domínio, enquanto os repositories concentram o acesso a dados.

Documentação de arquitetura e DDD: [context/arquitetura.md](./context/arquitetura.md).

## Tecnologias utilizadas

- C#
- .NET 10
- ASP.NET Core Web API com Controllers
- Entity Framework Core 10
- PostgreSQL 16
- Npgsql
- JWT Bearer Authentication
- Swagger/OpenAPI com Swashbuckle
- Docker e Docker Compose
- NUnit e Moq para testes unitários
- Migrations do Entity Framework Core

## Pré-requisitos

Para execução local:

- .NET SDK 10
- PostgreSQL 16 ou Docker Desktop
- Git

Para execução conteinerizada:

- Docker Desktop com Docker Compose habilitado

## Execução com Docker

Na raiz do projeto, execute:

```powershell
docker compose up -d --build
```

Os serviços disponibilizados são:

| Serviço | Endereço |
|---|---|
| API | `http://localhost:8080` |
| Swagger | `http://localhost:8080/swagger` |
| PostgreSQL | `localhost:5432` |

O Docker Compose utiliza:

```text
Database: meubanco
Username: postgres
Password: 123456
```

Para acompanhar os logs:

```powershell
docker compose logs -f api
docker compose logs -f postgres
```

Para parar os containers:

```powershell
docker compose down
```

As migrations são aplicadas automaticamente durante a inicialização da API.

## Execução local

Com um PostgreSQL disponível e a connection string configurada em `appsettings.json`:

```powershell
dotnet restore
dotnet run --project src/Oficina.Api/Oficina.Api.csproj
```

A API local utiliza, por padrão, `http://localhost:5080`.

## Execução dos testes

Executar todos os testes da solução:

```powershell
dotnet test Oficina.sln
```

Executar apenas os testes da aplicação:

```powershell
dotnet test tests/Oficina.Application.Tests/Oficina.Application.Tests.csproj
```

Executar somente os testes dos controllers:

```powershell
dotnet test tests/Oficina.Application.Tests/Oficina.Application.Tests.csproj --filter "FullyQualifiedName~Controllers"
```

Os testes utilizam NUnit e Moq e cobrem respostas HTTP, autorização, validações, regras de orçamento, estoque, históricos e relatórios.

## Swagger e documentação da API

Com a aplicação local:

- Swagger: [http://localhost:5080/swagger](http://localhost:5080/swagger)
- Em Docker: [http://localhost:8080/swagger](http://localhost:8080/swagger)

Também estão disponíveis exemplos de requisições no [Postman](./postman/) e no arquivo [item-catalogo.http](./item-catalogo.http), além da collection de relatórios [Relatorios.postman_collection.json](./postman/Relatorios.postman_collection.json).

## Decisão sobre o banco de dados

Foi escolhido o PostgreSQL por ser um banco relacional open source, estável e adequado ao domínio da oficina, que possui relacionamentos claros entre clientes, veículos, ordens, orçamentos, itens e históricos.

O modelo relacional também facilita:

- Integridade referencial.
- Transações para criação da OS e movimentação de estoque.
- Consultas com joins e filtros por período.
- Uso de migrations versionadas pelo Entity Framework Core.
- Execução padronizada via Docker.

## Autenticação e credenciais de demonstração

A autenticação é feita pelo endpoint:

```text
POST /api/auth/login
```

O ambiente do Postman utiliza as seguintes senhas de demonstração:

| Perfil | E-mail | Senha |
|---|---|---|
| Admin | Gerado automaticamente pelo Postman | `Admin@123456` |
| Cliente | Gerado automaticamente pelo Postman | `Cliente@123456` |

Os e-mails são gerados com timestamp para evitar duplicidade. O fluxo completo está em [Oficina.postman_collection.json](./postman/Oficina.postman_collection.json) e o ambiente em [Oficina.postman_environment.json](./postman/Oficina.postman_environment.json).

Após o login, envie o token recebido no header:

```text
Authorization: Bearer {token}
```

## Estrutura dos principais diretórios

```text
src/
├── Oficina.Api/
│   ├── Configurations/
│   ├── Middleware/
│   ├── Program.cs
│   └── Startup.cs
├── Oficina.Application/
│   ├── Controllers/
│   ├── Requests/
│   └── Services/
├── Oficina.Domain/
│   ├── Common/
│   ├── Dtos/
│   ├── Entities/
│   ├── Enums/
│   └── Interfaces/
├── Oficina.Infra.Data/
│   ├── Context/
│   ├── Mappings/
│   └── Repositories/
└── Oficina.Infra.Data.Postgres/
    └── Migrations/

tests/
└── Oficina.Application.Tests/
    ├── Controllers/
    └── Services/

context/
└── arquitetura.md
```

## Documentação DDD

A documentação de arquitetura, camadas, entidades, fluxo e decisões relacionadas ao domínio está em [context/arquitetura.md](./context/arquitetura.md).

## Vídeo da entrega

> Inserir aqui o link público e clicável do vídeo da entrega antes da submissão final.

Formato esperado:

```text
Link do vídeo: https://...
```
