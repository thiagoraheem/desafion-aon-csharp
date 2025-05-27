# TechCorp - Gerenciador de Usuários

Este projeto consiste em uma aplicação web para gerenciamento de usuários, composta por um backend em ASP.NET Core, um frontend em React e um banco de dados PostgreSQL. A aplicação permite criar, listar, atualizar e excluir usuários, além de importar usuários a partir de um arquivo CSV.

## Estrutura do Projeto

O projeto está organizado da seguinte forma:

- **TechCorpApi**: Backend em ASP.NET Core 9.0
- **frontend**: Frontend em React 19
- **arquivos**: Contém arquivos de suporte como o script SQL e dados de exemplo em CSV

## Requisitos

- [Docker](https://www.docker.com/products/docker-desktop/)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Configuração e Execução

### 1. Clonando o Repositório

Clone o repositório para sua máquina local:

```bash
git clone <url-do-repositorio>
cd projeto2
```

### 2. Executando a Aplicação com Docker Compose

Para iniciar todos os serviços (backend, frontend e banco de dados), execute o seguinte comando na raiz do projeto:

```bash
docker-compose up -d
```

Este comando irá:
1. Construir as imagens Docker para o backend e frontend
2. Iniciar o banco de dados PostgreSQL
3. Iniciar o backend ASP.NET Core
4. Iniciar o frontend React

### 3. Acessando a Aplicação

Após a inicialização dos contêineres, você pode acessar:

- **Frontend**: http://localhost:3000
- **API Backend**: http://localhost:3001
- **Swagger (Documentação da API)**: http://localhost:3001/swagger

### 4. Parando a Aplicação

Para parar todos os serviços, execute:

```bash
docker-compose down
```

Para parar e remover volumes (dados do banco), execute:

```bash
docker-compose down -v
```

## Detalhes dos Serviços

### Backend (ASP.NET Core)

- **Porta**: 3001 (mapeada para 8080 no contêiner)
- **Tecnologias**: ASP.NET Core 9.0, Entity Framework Core, PostgreSQL
- **Funcionalidades**:
  - CRUD de usuários
  - Upload de arquivo CSV com dados de usuários
  - Migrations automáticas do banco de dados

### Frontend (React)

- **Porta**: 3000
- **Tecnologias**: React 19, Bootstrap, Axios
- **Funcionalidades**:
  - Formulário para adicionar/editar usuários
  - Listagem de usuários
  - Upload de arquivo CSV

### Banco de Dados (PostgreSQL)

- **Porta**: 5432
- **Versão**: PostgreSQL 15
- **Credenciais**:
  - Usuário: postgres
  - Senha: senhaForte
  - Banco de dados: techcorp

## Estrutura do Banco de Dados

O banco de dados possui uma tabela principal:

```sql
CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100),
    email VARCHAR(100) UNIQUE,
    idade INT
);
```

## Importação de Dados via CSV

A aplicação suporta a importação de usuários através de arquivos CSV. O formato esperado é:

```
Nome,Email,Idade
João Silva,joao@email.com,28
Maria Souza,maria@email.com,35
```

Você pode usar o arquivo de exemplo localizado em `arquivos/usuarios.csv`.

## Desenvolvimento

### Reconstruindo as Imagens

Se você fizer alterações no código e precisar reconstruir as imagens, execute:

```bash
docker-compose build
docker-compose up -d
```

### Logs dos Contêineres

Para visualizar os logs de um serviço específico:

```bash
docker-compose logs -f backend  # para o backend
docker-compose logs -f frontend  # para o frontend
docker-compose logs -f db        # para o banco de dados
```

## Solução de Problemas

### Banco de Dados Não Disponível

O backend foi configurado para tentar se conectar ao banco de dados várias vezes durante a inicialização. Se você encontrar problemas, verifique os logs do backend:

```bash
docker-compose logs backend
```

### Problemas de Conexão com a API

Verifique se todos os serviços estão em execução:

```bash
docker-compose ps
```

Todos os serviços devem estar com o status "Up".