# Arquitetura da API — Oficina

Documentação DDD completa: [docs/ddd/README.md](../docs/ddd/README.md).

## 1. Visão geral

A Oficina API é uma aplicação monolítica em camadas, desenvolvida em C# com ASP.NET Core Web API e .NET 10. A solução gerencia clientes, usuários, veículos, itens de catálogo, ordens de serviço, orçamentos, estoque, históricos e relatórios.

O fluxo principal é:

```text
Cliente HTTP
    ↓
Controller
    ↓
Service
    ↓
Repository / DbContext
    ↓
Entity Framework Core
    ↓
PostgreSQL
```

As regras de negócio ficam principalmente no domínio e nos services. Os controllers são responsáveis por receber requisições, encaminhar chamadas e produzir respostas HTTP.

## 2. Projetos e dependências

```text
Oficina.Api
└── Oficina.Application
    ├── Oficina.Domain
    ├── Oficina.Infra.Data
    │   └── Oficina.Domain
    └── Oficina.Infra.Data.Postgres
        └── Oficina.Infra.Data
```

### Oficina.Api

Ponto de entrada e composição da aplicação. Contém:

- `Program.cs` e `Startup.cs`;
- configurações de autenticação, CORS, banco e Swagger;
- registro de controllers, services e repositories;
- middleware de exceções;
- execução automática das migrations no início da aplicação.

### Oficina.Application

Camada de aplicação e entrada HTTP. Contém:

- controllers;
- requests de entrada;
- services;
- orquestração de casos de uso;
- conversão entre entidades e DTOs.

### Oficina.Domain

Núcleo do negócio, sem dependência de infraestrutura ou de HTTP. Contém:

- entidades e regras de domínio;
- enums;
- DTOs;
- interfaces de services e repositories;
- exceções e objetos comuns do domínio.

### Oficina.Infra.Data

Implementa o acesso a dados:

- `OficinaDbContext`;
- mapeamentos Fluent API;
- filtros globais para exclusão lógica;
- repositories com consultas LINQ to Entities;
- consultas de ordens, orçamentos, itens e históricos.

### Oficina.Infra.Data.Postgres

Contém o provider PostgreSQL, o assembly de migrations e o snapshot do modelo do Entity Framework Core.

## 3. Controllers e endpoints

### AuthController

Rota base: `/api/auth`

- `POST /usuarios`: cadastra usuários e, para o perfil Cliente, cria o cliente vinculado.
- `POST /login`: valida credenciais e retorna token JWT.

### ClienteController

Rota base: `/api/clientes`

- `GET /`: lista clientes com busca e paginação.
- `GET /{id}`: obtém um cliente.
- `POST /`: cria cliente.
- `PUT /{id}`: atualiza cliente.
- `DELETE /{id}`: realiza exclusão lógica.

### VeiculoController

Rota base: `/api/veiculos`

- `GET /`: lista veículos.
- `GET /{id}`: obtém um veículo.
- `POST /`: cria veículo.
- `PUT /{id}`: atualiza veículo.
- `DELETE /{id}`: realiza exclusão lógica.

### ItemCatalogoController

Rota base: `/api/itens-catalogo`

- `GET /`: lista serviços e peças.
- `GET /{id}`: obtém item do catálogo.
- `GET /{id}/historico`: consulta o histórico do item.
- `POST /`: cria item e registra auditoria.
- `PUT /{id}`: atualiza item e registra auditoria.
- `POST /{id}/estoque`: adiciona quantidade ao estoque.
- `DELETE /{id}`: realiza exclusão lógica e registra auditoria.

### OrdemServicoController

Rota base: `/api/ordens-servico`

- `GET /`: lista ordens para Admin e Atendente.
- `GET /{id}`: obtém a ordem, incluindo os itens utilizados.
- `POST /`: cria ordem simples ou ordem com orçamento, conforme autenticação e conteúdo da requisição.
- `GET /minhas`: lista as ordens do cliente autenticado.
- `GET /por-cliente`: pesquisa ordens pelo nome do cliente.
- `PATCH /{id}/status`: altera o status e registra histórico.
- `GET /{id}/historico`: consulta histórico da ordem.
- `GET /{id}/orcamento`: consulta orçamento da ordem.
- `POST /{id}/orcamento/decisao`: aprova ou recusa orçamento.
- `PUT /{id}`: atualiza dados da ordem.
- `DELETE /{id}`: realiza exclusão lógica.

### RelatorioController

Rota base: `/api/relatorios`

- `GET /ordens-servico/tempo`: duração das ordens até finalização ou entrega.
- `GET /orcamentos`: duração dos orçamentos e resumo de aprovados, recusados e pendentes.
- `GET /ordens-servico/status`: quantidade de ordens por status.
- `GET /estoque`: itens com menor quantidade disponível.

Todos os endpoints protegidos utilizam autorização baseada em roles: `Admin`, `Atendente` e `Cliente`.

## 4. Entidades e regras de negócio

### Usuário e autenticação

- E-mails são normalizados para minúsculas e sem espaços externos.
- Não são permitidos usuários duplicados pelo e-mail.
- Clientes precisam informar CPF ou CNPJ válido.
- Senhas são armazenadas com hash.
- O login retorna JWT com o perfil do usuário.

### Cliente

- O cliente deve estar vinculado a um usuário.
- Documento é validado como CPF ou CNPJ.
- A exclusão é lógica por meio de `DataExclusao`.

### Veículo

- A placa é validada e armazenada em maiúsculas.
- O veículo pertence a um cliente.

### Item de catálogo e estoque

- Um item pode representar serviço ou peça.
- A quantidade nunca pode ficar negativa.
- A criação, atualização, exclusão e movimentação de estoque geram registros em `auditoria.historico_itens_catalogo`.

### Ordem de serviço

- Uma ordem pertence a um cliente e a um veículo.
- Ao criar uma ordem com orçamento, os itens são verificados no estoque.
- Os itens utilizados são baixados do estoque dentro de uma transação.
- A criação registra o primeiro evento do histórico da ordem.
- As transições de status são controladas pela entidade `OrdemServico`.
- O cliente consulta apenas as próprias ordens, resolvendo o `ClienteId` a partir do usuário autenticado.

### Orçamento

- O orçamento inicia como `Pendente`.
- Somente o cliente proprietário pode decidir.
- As decisões válidas são `Aprovado` e `Recusado`.
- Ao aprovar, a ordem avança para `EmExecucao`.
- Ao recusar, os itens retornam ao estoque.
- A decisão e a reposição de estoque ocorrem em transação.

### Exclusão lógica

Entidades de negócio que herdam de `EntidadeBase` são excluídas logicamente. Consultas normais filtram registros com `DataExclusao == null`, mantendo o histórico e evitando remoção física indevida.

## 5. Status da ordem de serviço

As transições permitidas são:

```text
Recebida → EmDiagnostico
Recebida → AguardandoAprovacao
EmDiagnostico → AguardandoAprovacao
AguardandoAprovacao → EmExecucao
AguardandoAprovacao → Recebida   (orçamento recusado)
EmExecucao → Finalizada
Finalizada → Entregue
```

Transições inválidas ou repetição do mesmo status geram conflito de negócio.

## 6. Persistência e auditoria

O PostgreSQL é acessado pelo Entity Framework Core com Npgsql. As migrations ficam em `Oficina.Infra.Data.Postgres/Migrations` e são aplicadas automaticamente na inicialização da API.

Principais tabelas e agrupamentos:

- entidades operacionais no schema `oficina`;
- entidades relacionadas ao cliente ficam no schema `clientes`
- entidades relacionadas a autenticação ficam no schema `auth`
- histórico de status das ordens no schema `oficina`;
- histórico de itens do catálogo no schema `auditoria`;
- tabela de controle das migrations do EF Core.

O histórico de itens guarda o snapshot do nome, preço, tipo, quantidade e ação realizada, permitindo rastrear entradas e saídas de estoque.

## 7. Autenticação e autorização

O sistema usa JWT Bearer. Os perfis são:

```text
Admin     = 1
Atendente = 2
Cliente   = 3
```

Após o login, o token deve ser enviado em:

```text
Authorization: Bearer {token}
```

As credenciais e o fluxo de demonstração estão documentados em `postman/Oficina.postman_environment.json` e `postman/Oficina.postman_collection.json`.

## 8. Tratamento de erros

A API utiliza tratamento global de exceções e Problem Details. As principais conversões são:

| Exceção ou situação | HTTP |
|---|---:|
| Requisição inválida ou argumento inválido | 400 |
| Recurso não encontrado | 404 |
| Conflito de negócio ou acesso indevido | 409 |
| Usuário não autenticado | 401 |
| Usuário sem permissão | 403 |
| Erro não tratado | 500 |

## 9. Testes automatizados

Os testes ficam em `tests/Oficina.Application.Tests`, utilizando NUnit e Moq.

Há testes de controllers para autenticação, clientes, itens de catálogo, ordens, relatórios e veículos. Também existem testes específicos para regras de relatório e autorização do histórico de ordens.

Os cenários cobertos incluem:

- respostas HTTP de sucesso e erro;
- autenticação e autorização;
- recursos inexistentes;
- cadastro de cliente vinculado a usuário;
- criação de ordem autenticada com orçamento;
- decisão de orçamento;
- histórico de ordens e itens;
- cálculo e agrupamento dos relatórios;
- validação de períodos e limites.

Execução:

```powershell
dotnet test Oficina.sln
```

## 10. Documentação complementar

- README principal: [README.md](../README.md)
- Requisitos e critérios da entrega: [context.md](./context.md)
- Documentação DDD e arquitetura: este arquivo
- Collection Postman: [Oficina.postman_collection.json](../postman/Oficina.postman_collection.json)
- Collection de relatórios: [Relatorios.postman_collection.json](../postman/Relatorios.postman_collection.json)
