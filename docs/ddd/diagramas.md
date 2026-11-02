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
    class OrdemServico {
      +Guid Id
      +Guid ClienteId
      +Guid VeiculoId
      +StatusOrdemServico Status
      +DefinirOrcamento()
      +AlterarStatus()
    }
    class OrdemServicoItem {
      +Guid ItemCatalogoId
      +int Quantidade
      +decimal ValorUnitario
    }
    class Orcamento {
      +StatusOrcamento Status
      +DateTimeOffset DataDecisao
      +Decidir()
    }
    class OrcamentoItem
    class ItemCatalogo {
      +TipoItemCatalogo Tipo
      +int QuantidadeEstoque
      +AdicionarEstoque()
      +RemoverEstoque()
    }
    class Cliente
    class Veiculo
    class HistoricoOrdemServico
    class HistoricoItemCatalogo

    OrdemServico "1" *-- "0..*" OrdemServicoItem
    OrdemServico "1" *-- "0..1" Orcamento
    OrdemServico "1" *-- "0..*" HistoricoOrdemServico
    Orcamento "1" *-- "0..*" OrcamentoItem
    Cliente "1" --> "0..*" OrdemServico
    Cliente "1" --> "0..*" Veiculo
    OrdemServicoItem "*" --> "1" ItemCatalogo
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
