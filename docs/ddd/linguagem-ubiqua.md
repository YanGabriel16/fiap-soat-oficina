# Linguagem Ubíqua

## Propósito

Este glossário mantém os termos usados pelo negócio e pelo código com o mesmo significado. Os nomes abaixo foram pegos das entidades, enums, services, controllers e migrations existentes.

## Glossário

| Termo | Definição no domínio | Implementação |
|---|---|---|
| Cliente | Pessoa que possui cadastro e solicita serviços para um veículo. | `Cliente`, `ClienteService`, `ClienteController` |
| Usuário | Identidade autenticável que possui um perfil de acesso. | `Usuario`, `AuthService`, JWT |
| Admin | Perfil com acesso administrativo aos recursos protegidos. | `RoleUsuario.Admin` |
| Atendente | Perfil que opera clientes, catálogo e ordens. | `RoleUsuario.Atendente` |
| Veículo | Automóvel vinculado a um cliente. | `Veiculo`, `VeiculoService` |
| Item de catálogo | Serviço, peça ou insumo que pode ser usado em uma ordem. | `ItemCatalogo`, `TipoItemCatalogo` |
| Serviço | Item do catálogo que representa mão de obra ou atividade. | `TipoItemCatalogo.Servico` |
| Peça | Item físico utilizado na manutenção. | `TipoItemCatalogo.Peca` |
| Insumo | Material consumível do catálogo. | `TipoItemCatalogo.Insumo` |
| Estoque | Quantidade disponível de cada item de catálogo. | `ItemCatalogo.QuantidadeEstoque` |
| Ordem de serviço | Agregado que representa o atendimento de um cliente e veículo. | `OrdemServico` |
| Item da ordem | Item de catálogo e quantidade usados na ordem, com preço unitário congelado. | `OrdemServicoItem` |
| Orçamento | Proposta de valor associada a uma ordem e aguardando decisão do cliente. | `Orcamento` |
| Item do orçamento | Item de catálogo que compõe o orçamento. | `OrcamentoItem` |
| Aprovação | Decisão que aceita o orçamento e leva a OS para execução. | `StatusOrcamento.Aprovado` |
| Recusa | Decisão que rejeita o orçamento e devolve itens ao estoque. | `StatusOrcamento.Recusado` |
| Histórico da OS | Registro de mudança de status ou decisão de orçamento. | `HistoricoOrdemServico` |
| Histórico do catálogo | Snapshot de alteração ou movimentação de estoque. | `HistoricoItemCatalogo`, schema `auditoria` |
| Exclusão lógica | Inativação por `DataExclusao`, sem remoção física. | `EntidadeBase`, `BaseRepository` |

## Status da ordem

`Recebida`, `EmDiagnostico`, `AguardandoAprovacao`, `EmExecucao`, `Finalizada` e `Entregue` são os estados persistidos em `StatusOrdemServico`.

As transições implementadas são:

```text
Recebida → EmDiagnostico
Recebida → AguardandoAprovacao
EmDiagnostico → AguardandoAprovacao
AguardandoAprovacao → EmExecucao
AguardandoAprovacao → Recebida
EmExecucao → Finalizada
Finalizada → Entregue
```

## Termos técnicos que não são conceitos de domínio

- `DTO`, `Request`, `Controller`, `Service` e `Repository` são conceitos de implementação.
- `Guid`, paginação, JWT e migration são mecanismos técnicos.
- Não há classes explícitas de Value Object no projeto. Documento, placa, dinheiro e período são representados por `string`, `decimal` e `DateTimeOffset` com validações nas entidades ou services.

## Limites identificados

- Não existe um módulo formal separado por bounded context; os contextos abaixo são uma divisão lógica da aplicação.
- Não existe publicação de eventos para outros sistemas.
- Não existe fluxo implementado de cancelamento ou conclusão financeira do orçamento.
