# Desafio Técnico Eclipseworks

## O Desafio trata-se do desenvolvimento de uma API para o gerenciamento de atividades diárias de usuários em projetos.

### Pré-requisitos

Antes de começar, você vai precisar ter instalado em sua máquina as seguintes ferramentas:
[Docker](https://www.docker.com/).
Certifique-se que não tenha nenhum serviço local executando nas portas 1433 e 5000.
Além disto é bom ter um editor para trabalhar com o código como [Visual Studio](https://visualstudio.microsoft.com/pt-br/), 

### 🎲 Rodando o Back End (servidor)

```bash
# No diretório raiz do projeto, realize o build do projeto pelo docker compose
$ docker-compose build

# Suba o servidor de banco de dados pelo docker
$ docker-compose up sqlserver

# O servidor de banco de dados inciará na porta:1433

# Depois suba o servidor de aplicação pelo docker
$ docker-compose up eclipseworks

# O servidor de apricação inciará na porta:5000 - acesse o Swagger pelo link <http://localhost:5000/swagger/index.html>
```

### 🛠 Tecnologias Utilizadas

As seguintes ferramentas foram usadas na construção do projeto:

- [dotnet](https://dotnet.microsoft.com/pt-br/)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server)
- [Docker](https://www.docker.com/)

### Arquitetura

#### Domínio

Nesta camada de foi implementado as entidades do sistema seguindo o conceito de dóminio Rico.
Entidades Criadas:
- User -> Pessoa que utiliza o sistema. 
- Project -> Entitdade Agregadora de tarefas.
- ProjectTask -> Entidade que representa uma tarefa associada a um projeto.
- ProjectTaskComment -> Comentário de uma Tarefa do sistema.
- HistoryUpdate -> Representa um Histórico de alterações de uma entidade.
- HistoryUpdateChange -> Propriedades alteradas de um histórico de uma entidade.

#### Infraestrutura

Camanda responsável por prover a infraestrutura necessária de comunicação entre o banco de dados e a aplicação.
Foi utilizado o ORM EF Core que é um padrão de mercado

#### Aplicação

Nesta camada de foi implementado sobre o conceito de CQRS, onde temos a segregação dos comandos, consultas e eventos de domínio separadas do sistema.

#### WebApi (Apresentação)

Camada de Apresentação do sistema que provê os endpoints do sistema.

#### WebApi Testes

Camada que consome os endpoints do sistema e realiza as asserções com base nas regras de negócio e sistema.

### Refinamento

#### Dúvidas Sobre a demanda solicitada

- Restrições de Remoção de Projetos -> Nos endpoints solicitados, não existia um que possuísse o objetivo de excluir um projeto. Desta forma, foi desenvolvido um endpoint que tivesse essa responsabilidade. 
- Comentários nas Tarefas -> Não ficou claro se os comentários das tarefas seriam adicionados no endpoint de update das tarefas ou criado um novo endpoint. Segui pelo caminho de criar um endpoint específico para adicionar um comentário a uma tarefa.
- Relatórios de Desempenho -> Esta regra de negócio não ficou bem entendida por mim, desta forma acabei não implementando e entraria em uma segunda sprint para desenvolvimento.
  - Somente neste item de negócio é falado sobre um usuário estar associado a uma tarefa (número médio de tarefas concluídas por usuário). Prevendo isso, foi adicionado as propriedades de responsável e data de conclusão da tarefa, contudo a regra de negócio dessas propriedades devem ser revistas em uma sprint futura.

### Melhoria no Projeto

- Usuário logado -> Como existiam alguns requisitos que necessitavam pegar o usuário logado (Histórico de alterações, por exemplo) e a API não possui autenticação ainda, foi atribuido o primeiro usuário do banco de dados como o usuário logado. Contudo nas próximas sprints este comportamento deve ser alterado para pegar o usuário logado com base no token de autenticação do sistema.
- Remoção de projetos -> exisita a regra de somente poder remover projetos quando não possuísse tarefas pendentes, contudo ao meu ver não deveria poder excluir um projeto que tivesse tarefas em andamento também.
- Os campos textos poderiam ter tamanhos máximos definidos, pois deixando livre poderemos ter problemas de desempenho da API quando estiver em produção.
