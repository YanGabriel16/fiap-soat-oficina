# Documentação DDD — Oficina API

Esta pasta documenta o domínio conforme ele está implementado no código atual. Os PDFs são versões de distribuição; os arquivos Markdown e Mermaid são as fontes editáveis.

## Documentos

- [Event Storming](./event-storming.md)
- [Diagramas de domínio](./diagramas.md)
- [Linguagem ubíqua](./linguagem-ubiqua.md)
- [PDF do Event Storming](./event-storming.pdf)
- [PDF dos diagramas](./diagramas.pdf)

## Fontes dos diagramas

- [Context Map](./diagramas/context-map.mmd)
- [Agregados](./diagramas/agregados.mmd)
- [Fluxo da ordem de serviço](./diagramas/fluxo-ordem-servico.mmd)
- [Estoque e auditoria](./diagramas/estoque-auditoria.mmd)

## Critério de fidelidade

Eventos descritos como eventos de domínio representam mudanças efetivamente persistidas pela aplicação, principalmente por meio de `HistoricoOrdemServico` e `HistoricoItemCatalogo`. A solução não possui um barramento de eventos, publicação assíncrona ou handlers de domínio independentes. Essa limitação é indicada nos documentos para separar o que existe no código de uma possível evolução arquitetural.
