# Postman — Oficina

## Preparação

1. Inicie a API e o PostgreSQL.
2. Importe `Oficina.postman_collection.json` no Postman.
3. Importe e selecione o ambiente `Oficina - Local`.
4. Execute a collection pela pasta `01 - Fluxo principal`, na ordem apresentada.

Para executar o fluxo novamente, apague os valores das variáveis `adminEmail`, `clienteEmail` e dos IDs/tokens no ambiente, ou reimporte o ambiente. A collection gera novos e-mails automaticamente na primeira execução.

A API local usa `http://localhost:5080`. Esse valor pode ser alterado pela variável `baseUrl` do ambiente.

## Fluxo principal

1. Cadastra um usuário Admin e faz login.
2. Cadastra um usuário Cliente e faz login.
3. Lista clientes e identifica o cliente criado pelo nome.
4. Cria um item de catálogo e um veículo.
5. Cria uma ordem de serviço com orçamento e item.
6. Consulta OS, orçamento e histórico.
7. Altera o status para `EmDiagnostico` e depois `AguardandoAprovacao`.
8. Consulta e aprova o orçamento usando o token do Cliente.
9. Consulta as OS do cliente e avança a OS para `Finalizada` e `Entregue`.

Os testes da collection salvam automaticamente tokens e IDs nas variáveis do ambiente. Os valores dos enums são:

- Role: `Admin=1`, `Atendente=2`, `Cliente=3`.
- Status da OS: `Recebida=1`, `EmDiagnostico=2`, `AguardandoAprovacao=3`, `EmExecucao=4`, `Finalizada=5`, `Entregue=6`.
- Tipo de item: `Servico=1`, `Peca=2`.
- Decisão de orçamento: `Aprovado=2`, `Recusado=3`.

As pastas `02 - Consultas` e `03 - CRUD` contêm as demais requisições disponíveis da API.
