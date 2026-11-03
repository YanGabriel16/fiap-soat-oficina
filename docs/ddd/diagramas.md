# Diagramas de domínio

Os diagramas abaixo representam o código atual. Os arquivos `.mmd` são fontes Mermaid editáveis.

## Context Map

```mermaid
flowchart LR
    Auth[Identidade e Acesso]
    Atendimento[Atendimento e Ordem de Serviço]
    Catalogo[Catálogo e Estoque]
    Relatorios[Relatórios]
    Banco[(PostgreSQL)]

    Auth -->|JWT e roles| Atendimento
    Auth -->|JWT e roles| Catalogo
    Atendimento -->|consome itens e movimenta estoque| Catalogo
    Relatorios -->|consulta OS, orçamentos e estoque| Banco
    Auth --> Banco
    Atendimento --> Banco
    Catalogo --> Banco
```

Fonte: [context-map.mmd](./diagramas/context-map.mmd).

## Agregados e entidades

```mermaid
classDiagram
    class EntidadeBase {
      +Guid Id
      +DateTimeOffset DataCriacao
      +DateTimeOffset DataUltimaModificacao
      +DateTimeOffset DataExclusao
      +bool Excluida
      +AtualizarDataModificacao()
      +Excluir()
    }
    class Usuario {
      +string Nome
      +string Email
      +string SenhaHash
      +RoleUsuario Role
      +Cliente Cliente
      +AlterarRole(RoleUsuario role)
      +AtualizarSenha(string senhaHash)
    }
    class Token {
      +Guid UsuarioId
      +string Valor
      +DateTimeOffset ExpiraEm
      +bool Revogado
      +Revogar()
    }
    class Cliente {
      +string Nome
      +string Documento
      +Guid UsuarioId
      +Usuario Usuario
      +ICollection~Veiculo~ Veiculos
      +Atualizar(string nome, string documento)
    }
    class Veiculo {
      +Guid ClienteId
      +Cliente Cliente
      +string Placa
      +string Marca
      +string Modelo
      +int Ano
      +Atualizar(Guid clienteId, string placa, string marca, string modelo, int ano)
    }
    class ItemCatalogo {
      +string Nome
      +decimal Preco
      +TipoItemCatalogo Tipo
      +int QuantidadeEstoque
      +Atualizar(string nome, decimal preco, TipoItemCatalogo tipo, int quantidadeEstoque)
      +AdicionarEstoque(int quantidade)
      +RemoverEstoque(int quantidade)
    }
    class OrdemServico {
      +Guid ClienteId
      +Guid VeiculoId
      +StatusOrdemServico Status
      +decimal ValorTotal
      +Guid AtendenteId
      +string Descricao
      +decimal ValorMaoDeObra
      +Cliente Cliente
      +Usuario Atendente
      +ICollection~OrdemServicoItem~ Itens
      +Orcamento Orcamento
      +ICollection~HistoricoOrdemServico~ Historico
      +Atualizar(Guid clienteId, Guid veiculoId, StatusOrdemServico status, decimal valorTotal)
      +DefinirOrcamento(decimal valorMaoDeObra, decimal valorTotal)
      +AlterarStatus(StatusOrdemServico novoStatus)
    }
    class OrdemServicoItem {
      +Guid OrdemServicoId
      +Guid ItemCatalogoId
      +int Quantidade
      +decimal ValorUnitario
      +decimal ValorTotal
      +ItemCatalogo ItemCatalogo
    }
    class Orcamento {
      +Guid OrdemServicoId
      +Guid ClienteId
      +decimal ValorMaoDeObra
      +decimal ValorTotal
      +StatusOrcamento Status
      +DateTimeOffset? DataDecisao
      +OrdemServico OrdemServico
      +Cliente Cliente
      +ICollection~OrcamentoItem~ Itens
      +AdicionarItem(Guid itemCatalogoId, int quantidade, decimal valorUnitario)
      +Decidir(StatusOrcamento status)
    }
    class OrcamentoItem {
      +Guid OrcamentoId
      +Guid ItemCatalogoId
      +int Quantidade
      +decimal ValorUnitario
      +decimal ValorTotal
      +ItemCatalogo ItemCatalogo
    }
    class HistoricoOrdemServico {
      +Guid OrdemServicoId
      +StatusOrdemServico? StatusAnterior
      +StatusOrdemServico NovoStatus
      +string Observacao
      +Guid UsuarioId
      +TipoHistoricoOrdem Tipo
      +Usuario Usuario
      +OrdemServico OrdemServico
    }
    class HistoricoItemCatalogo {
      +Guid Id
      +Guid ItemCatalogoId
      +AcaoHistoricoItemCatalogo Acao
      +string Nome
      +decimal Preco
      +TipoItemCatalogo Tipo
      +int QuantidadeEstoque
      +DateTimeOffset DataEvento
    }
    EntidadeBase <|-- Usuario
    EntidadeBase <|-- Token
    EntidadeBase <|-- Cliente
    EntidadeBase <|-- Veiculo
    EntidadeBase <|-- ItemCatalogo
    EntidadeBase <|-- OrdemServico
    EntidadeBase <|-- OrdemServicoItem
    EntidadeBase <|-- Orcamento
    EntidadeBase <|-- OrcamentoItem
    EntidadeBase <|-- HistoricoOrdemServico
    Usuario "1" --> "0..1" Cliente
    Usuario "1" --> "0..*" Token
    Usuario "1" --> "0..*" HistoricoOrdemServico
    Cliente "1" --> "0..*" Veiculo
    Cliente "1" --> "0..*" OrdemServico
    Cliente "1" --> "0..*" Orcamento
    Veiculo "1" --> "1" Cliente
    OrdemServico "1" *-- "0..*" OrdemServicoItem
    OrdemServico "1" *-- "0..1" Orcamento
    OrdemServico "1" *-- "0..*" HistoricoOrdemServico
    OrdemServico "*" --> "1" Cliente
    OrdemServico "*" --> "1" Veiculo
    OrdemServico "*" --> "1" Usuario : atendente
    OrdemServicoItem "*" --> "1" ItemCatalogo
    Orcamento "1" *-- "0..*" OrcamentoItem
    Orcamento "*" --> "1" OrdemServico
    OrcamentoItem "*" --> "1" ItemCatalogo
    ItemCatalogo "1" --> "0..*" HistoricoItemCatalogo
```

Fonte: [agregados.mmd](./diagramas/agregados.mmd).

## Fluxo de ordem e orçamento

```mermaid
sequenceDiagram
    actor A as Atendente
    actor C as Cliente
    participant API
    participant OS as OrdemServicoService
    participant DB as PostgreSQL

    A->>API: POST /api/ordens-servico
    API->>OS: CriarComOrcamentoAsync
    OS->>DB: Verifica itens e estoque
    OS->>DB: Salva OS, orçamento, histórico e baixa
    DB-->>API: Ordem criada
    C->>API: POST /{id}/orcamento/decisao
    API->>DB: Valida proprietário e decisão
    alt Aprovado
        DB->>DB: Avança para EmExecucao
    else Recusado
        DB->>DB: Repõe itens no estoque
    end
```

Fonte: [fluxo-ordem-servico.mmd](./diagramas/fluxo-ordem-servico.mmd).

## Estoque e auditoria

```mermaid
flowchart TD
    Criar[Criação da OS com orçamento] --> Baixa[RemoverEstoque]
    Baixa --> AuditoriaSaida[Histórico: EstoqueRemovido]
    Recusa[Orçamento recusado] --> Repor[AdicionarEstoque]
    Repor --> AuditoriaEntrada[Histórico: EstoqueAdicionado]
    Cadastro[CRUD do item] --> AuditoriaCRUD[Histórico: Criado, Atualizado ou Excluído]
    AuditoriaSaida --> Tabela[(auditoria.historico_itens_catalogo)]
    AuditoriaEntrada --> Tabela
    AuditoriaCRUD --> Tabela
```

Fonte: [estoque-auditoria.mmd](./diagramas/estoque-auditoria.mmd).

## Relação com a implementação

| Conceito | Componentes |
|---|---|
| Entrada HTTP | Controllers em `src/Oficina.Application/Controllers` |
| Caso de uso | Services em `src/Oficina.Application/Services` |
| Estado do domínio | Entidades em `src/Oficina.Domain/Entities` |
| Persistência | Repositories em `src/Oficina.Infra.Data/Repositories` |
| Mapeamento | `src/Oficina.Infra.Data/Mappings` |
| Banco e migrations | `src/Oficina.Infra.Data.Postgres/Migrations` |
| Testes | `tests/Oficina.Application.Tests` |
