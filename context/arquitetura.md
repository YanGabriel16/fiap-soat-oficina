# Arquitetura da API — Oficina

API .NET organizada em camadas, com separação clara entre apresentação, aplicação, domínio e infraestrutura.

---

## Entidades

- Cliente
- ItemCatalogo
- OrdemServico
- Veiculo

---

## Visão geral das camadas

```
Oficina.Api
Oficina.Application
Oficina.Domain
Oficina.Infra.Data
Oficina.Infra.Data.Postgres
```

### Dependências entre projetos

```
Oficina.Api
    ├── Oficina.Application
    │       └── Oficina.Domain
    │
    ├── Oficina.Infra.Data
    │       └── Oficina.Domain
    │
    └── Oficina.Infra.Data.Postgres
            └── Oficina.Infra.Data
```

- `Domain` não depende de nenhum outro projeto.
- `Application` depende apenas de `Domain`.
- `Infra.Data` implementa as interfaces definidas em `Domain`.
- `Infra.Data.Postgres` contém somente as migrations.
- `Api` é a raiz de composição: registra tudo no DI.

---

## 1. Oficina.Api

Ponto de entrada da aplicação. Contém apenas configurações globais.

```
Oficina.Api/
├── Program.cs
├── Startup.cs
├── appsettings.json
├── appsettings.Development.json
└── Properties/
    └── launchSettings.json
```

**Responsabilidades**

- `Program` / `Startup`: configuração global (DI, CORS, Swagger, autenticação, middlewares, logging).
- Registro do `DbContext`, repositories e services.
- Configuração do provider Postgres e do assembly de migrations.
- Nenhuma regra de negócio e nenhuma controller.

---

## 2. Oficina.Application

Camada de entrada HTTP e de regras de negócio.

```
Oficina.Application/
├── Controllers/
│   ├── ClienteController.cs
│   ├── ItemCatalogoController.cs
│   ├── OrdemServicoController.cs
│   └── VeiculoController.cs
│
├── Requests/
│   ├── Cliente/
│   │   ├── CriarClienteRequest.cs
│   │   └── AtualizarClienteRequest.cs
│   │
│   ├── ItemCatalogo/
│   │   ├── CriarItemCatalogoRequest.cs
│   │   └── AtualizarItemCatalogoRequest.cs
│   │
│   ├── OrdemServico/
│   │   ├── CriarOrdemServicoRequest.cs
│   │   └── AtualizarOrdemServicoRequest.cs
│   │
│   └── Veiculo/
│       ├── CriarVeiculoRequest.cs
│       └── AtualizarVeiculoRequest.cs
│
└── Services/
    ├── ClienteService.cs
    ├── ItemCatalogoService.cs
    ├── OrdemServicoService.cs
    └── VeiculoService.cs
```

**Regras**

- Uma controller por entidade.
- Um objeto de request por arquivo.
- Controllers são finas: recebem o request, chamam a service e devolvem o resultado.
- As services concentram a lógica e orquestram os repositories.

---

## 3. Oficina.Domain

Núcleo do sistema. Não conhece infraestrutura nem framework web.

```
Oficina.Domain/
├── Entities/
│   ├── Cliente.cs
│   ├── ItemCatalogo.cs
│   ├── OrdemServico.cs
│   └── Veiculo.cs
│
├── Enums/
│   └── StatusOrdemServico.cs
│
├── Dtos/
│   ├── ClienteDto.cs
│   ├── ItemCatalogoDto.cs
│   ├── OrdemServicoDto.cs
│   └── VeiculoDto.cs
│
└── Interfaces/
    ├── Repositories/
    │   ├── IClienteRepository.cs
    │   ├── IItemCatalogoRepository.cs
    │   ├── IOrdemServicoRepository.cs
    │   └── IVeiculoRepository.cs
    │
    └── Services/
        ├── IClienteService.cs
        ├── IItemCatalogoService.cs
        ├── IOrdemServicoService.cs
        └── IVeiculoService.cs
```

**Contém**

- Entidades
- Enums
- DTOs (retorno das consultas)
- Interfaces de repositories e services

---

## 4. Oficina.Infra.Data

Acesso a dados: contexto, mapeamentos e repositories.

```
Oficina.Infra.Data/
├── Context/
│   └── OficinaDbContext.cs
│
├── Mappings/
│   ├── ClienteMap.cs
│   ├── ItemCatalogoMap.cs
│   ├── OrdemServicoMap.cs
│   └── VeiculoMap.cs
│
└── Repositories/
    ├── ClienteRepository.cs
    ├── ItemCatalogoRepository.cs
    ├── OrdemServicoRepository.cs
    └── VeiculoRepository.cs
```

**Regras**

- Um mapeamento por arquivo, implementando `IEntityTypeConfiguration<T>`.
- Um repository por entidade, implementando a interface definida em `Domain`.
- O `DbContext` aplica os mapeamentos via `ApplyConfigurationsFromAssembly`.

---

## 5. Oficina.Infra.Data.Postgres

Projeto dedicado somente às migrations.

```
Oficina.Infra.Data.Postgres/
└── Migrations/
    ├── 20261101000000_Inicial.cs
    ├── 20261101000000_Inicial.Designer.cs
    └── OficinaDbContextModelSnapshot.cs
```

**Regras**

- Nenhum código além das migrations geradas pelo EF Core.
- Configurado em `Program` com `MigrationsAssembly("Oficina.Infra.Data.Postgres")`.

---

## Fluxo geral da requisição

```
Cliente HTTP
    │
    ▼
Controller
    │
    ▼
Service
    │
    ▼
Repository
    │
    ▼
PostgreSQL
```

O retorno faz o caminho inverso: Repository → Service → Controller → resposta HTTP.

### Regras do fluxo

1. A controller nunca acessa o repository diretamente; sempre passa pela service.
2. A service trabalha a lógica (validações, regras, conversões) antes de chamar o repository.
3. Operações GET sempre retornam DTOs, nunca entidades.
4. Requests (entrada) ficam em `Application`; DTOs (saída) ficam em `Domain`.
5. Todo `Id` de entidade é sempre `Guid`.
6. Toda criação (POST) retorna o `Guid` do registro criado.
7. Todo update (PUT) retorna o `Guid` do registro atualizado.

### Padrão de retorno

| Operação | Verbo | Retorno |
|---|---|---|
| Consulta | GET | DTO (ou lista de DTOs) |
| Criação | POST | `Guid` do registro criado |
| Atualização | PUT | `Guid` do registro atualizado |

---

## Exemplo de fluxo (Cliente)

### Controller

```csharp
[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClienteController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDto>> ObterPorId(Guid id)
    {
        var cliente = await _clienteService.ObterPorIdAsync(id);

        if (cliente is null)
        {
            return NotFound();
        }

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Criar(CriarClienteRequest request)
    {
        var id = await _clienteService.CriarAsync(request);

        return Ok(id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Guid>> Atualizar(Guid id, AtualizarClienteRequest request)
    {
        var idAtualizado = await _clienteService.AtualizarAsync(id, request);

        return Ok(idAtualizado);
    }
}
```

### Service

```csharp
public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<ClienteDto?> ObterPorIdAsync(Guid id)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);

        if (cliente is null)
        {
            return null;
        }

        return new ClienteDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome
        };
    }

    public async Task<Guid> CriarAsync(CriarClienteRequest request)
    {
        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome
        };

        await _clienteRepository.AdicionarAsync(cliente);

        return cliente.Id;
    }

    public async Task<Guid> AtualizarAsync(Guid id, AtualizarClienteRequest request)
    {
        var cliente = await _clienteRepository.ObterPorIdAsync(id);

        if (cliente is null)
        {
            throw new KeyNotFoundException("Cliente não encontrado.");
        }

        cliente.Nome = request.Nome;

        await _clienteRepository.AtualizarAsync(cliente);

        return cliente.Id;
    }
}
```

### Repository

```csharp
public class ClienteRepository : IClienteRepository
{
    private readonly OficinaDbContext _context;

    public ClienteRepository(OficinaDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente?> ObterPorIdAsync(Guid id)
    {
        return await _context.Clientes
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AdicionarAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
    }
}
```

---

## Resumo por camada

| Projeto | Conteúdo | Regra principal |
|---|---|---|
| `Oficina.Api` | Program/Startup, configurações globais | Sem regra de negócio |
| `Oficina.Application` | Controllers, Requests, Services | 1 controller por entidade, 1 request por arquivo |
| `Oficina.Domain` | Entities, Enums, Dtos, Interfaces | Sem dependências externas |
| `Oficina.Infra.Data` | DbContext, Mappings, Repositories | 1 mapping e 1 repository por entidade |
| `Oficina.Infra.Data.Postgres` | Migrations | Somente migrations |
