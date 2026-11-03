# Event Storming — Oficina API

## Convenções

- **Ator:** quem inicia ou autoriza uma ação.
- **Comando:** intenção enviada à API.
- **Evento:** mudança de negócio persistida pela aplicação.
- **Política:** regra que reage a um evento e produz uma ação.
- **Read model:** consulta ou relatório derivado do estado persistido.

Os eventos abaixo não são classes de eventos publicadas em um barramento. Na implementação atual, eles são representados por mudanças nas entidades e registros de histórico.

## Fluxo 1 — Criação da Ordem de Serviço

### Linha do tempo

```text
Admin ou Atendente
  → CriarOrdemComOrcamento
  → ItensDoCatalogoVerificados
  → EstoqueDosItensRemovido
  → OrdemServicoCriada
  → OrcamentoCriado
  → HistoricoDaOrdemRegistrado
```

### Comandos, eventos e políticas

| Tipo | Elemento | Implementação real |
|---|---|---|
| Ator | Admin ou Atendente | Token com `RoleUsuario.Admin` ou `RoleUsuario.Atendente` |
| Comando | Criar ordem com orçamento | `POST /api/ordens-servico` autenticado |
| Política | Validar item existente | `OrdemServicoService.CriarComOrcamentoAsync` |
| Política | Impedir quantidade maior que o estoque | `ItemCatalogo.RemoverEstoque` |
| Evento | Ordem criada | `OrdemServico` + histórico inicial |
| Evento | Estoque removido | `HistoricoItemCatalogo` com `EstoqueRemovido` |
| Evento | Orçamento criado | `Orcamento` e `OrcamentoItem` |

### Resultado

A ordem é montada com os itens, preço unitário do momento da criação, valor de mão de obra e valor total somente depois que todos os itens foram encontrados e os estoques validados. Se qualquer item não existir ou não possuir estoque suficiente, a transação não é concluída.

## Fluxo 2 — Acompanhamento da Ordem de Serviço

### Linha do tempo

```text
Admin ou Atendente
  → AlterarStatus
  → StatusDaOrdemAlterado
  → HistoricoDaOrdemRegistrado

Cliente
  → ConsultarOrcamento
  → AprovarOrcamento ou RecusarOrcamento
  → OrcamentoDecidido
```

### Comandos, eventos e políticas

| Tipo | Elemento | Implementação real |
|---|---|---|
| Comando | Alterar status | `PATCH /api/ordens-servico/{id}/status` |
| Política | Validar transição | `OrdemServico.AlterarStatus` |
| Evento | Status alterado | `HistoricoOrdemServico` |
| Comando | Consultar histórico | `GET /api/ordens-servico/{id}/historico` |
| Política | Cliente só acessa a própria OS | `HistoricoOrdemServicoService` |
| Comando | Consultar orçamento | `GET /api/ordens-servico/{id}/orcamento` |
| Comando | Decidir orçamento | `POST /api/ordens-servico/{id}/orcamento/decisao` |
| Política | Somente proprietário decide | `OrcamentoService.DecidirAsync` |
| Evento | Orçamento aprovado ou recusado | `Orcamento.DataDecisao` e histórico |

### Transições implementadas

```text
Recebida → EmDiagnostico → AguardandoAprovacao → EmExecucao → Finalizada → Entregue
                                      └──────────────→ Recebida (recusa)
```

## Fluxo 3 — Gestão de peças e insumos

### Linha do tempo

```text
Atendente/Admin
  → CriarOuAtualizarItem
  → ItemCatalogoCriado ou ItemCatalogoAtualizado
  → HistoricoDoCatalogoRegistrado

Atendente/Admin
  → AdicionarEstoque
  → EstoqueAdicionado

Ordem criada
  → EstoqueRemovido

Orçamento recusado
  → EstoqueReposto
```

### Comandos, eventos e políticas

| Tipo | Elemento | Implementação real |
|---|---|---|
| Comando | Criar item | `POST /api/itens-catalogo` |
| Comando | Atualizar item | `PUT /api/itens-catalogo/{id}` |
| Comando | Adicionar estoque | `POST /api/itens-catalogo/{id}/estoque` |
| Comando | Consultar histórico | `GET /api/itens-catalogo/{id}/historico` |
| Política | Quantidade adicionada deve ser positiva | `ItemCatalogo.AdicionarEstoque` |
| Política | Quantidade removida deve ser positiva e disponível | `ItemCatalogo.RemoverEstoque` |
| Evento | Estoque adicionado | `AcaoHistoricoItemCatalogo.EstoqueAdicionado` |
| Evento | Estoque removido | `AcaoHistoricoItemCatalogo.EstoqueRemovido` |
| Evento | Estoque reposto | Reposição na recusa do orçamento, dentro da transação de `OrcamentoService.DecidirAsync` |

## Atores

- **Admin:** administra cadastros e pode consultar os recursos administrativos.
- **Atendente:** opera catálogo, veículos, clientes e ordens.
- **Cliente:** consulta suas ordens e decide seus orçamentos.
- **Sistema:** executa transações, aplica migrations e registra históricos durante os casos de uso.

## Limitações do modelo atual

Os eventos são persistidos como histórico, mas não há event bus, integração assíncrona, sagas, publicação externa ou projeções CQRS. A criação da OS com orçamento e a decisão do orçamento usam transações explícitas; o CRUD administrativo do catálogo registra cada alteração por operações de repositório separadas. Esses itens são possibilidades de evolução, não comportamentos existentes.
