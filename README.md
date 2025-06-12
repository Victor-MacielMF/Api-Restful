# 📈 API RESTful - Gestão de Ações e Comentários

Este projeto foi **refatorado** a partir de uma versão inicial que não seguia boas práticas nem princípios RESTful. Agora, trata-se de uma API RESTful moderna desenvolvida com ASP.NET Core 8.0 para gerenciamento de ações e comentários de usuários. O sistema utiliza autenticação JWT, ASP.NET Identity, Entity Framework Core com Code First e adota padrões de arquitetura limpos, SOLID e práticas recomendadas de desenvolvimento de software.

Se desejar, você pode consultar o histórico do repositório e visualizar como o projeto era no [primeiro commit](https://github.com/victor-macielmf/api-restful/commits/main) para comparar a evolução e as melhorias aplicadas.

---

## 🚀 Tecnologias Utilizadas

- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core (Migrations + SQL Server)
- ASP.NET Identity (controle de usuários e papéis)
- JWT (JSON Web Token) para autenticação
- AutoMapper
- Swagger / Swashbuckle (documentação interativa)
- Newtonsoft.Json
- Filtros personalizados (Execution Timer)
- Middleware de tratamento de erros e autorização customizada
- Repository Pattern + Service Layer
- Boas práticas: SOLID, Clean Architecture, DTOs, versionamento
- Testes automatizados com xUnit e FluentAssertions

---

## 📁 Estrutura de Pastas

```text
.
├── Api/                       # Projeto principal da API
│   ├── Controllers/           # Endpoints da API
│   ├── Data/                  # DbContext e configurações do banco
│   ├── Dtos/                  # Data Transfer Objects organizados
│   ├── Helpers/               # Utilitários (JWT, Claims, Responses, QueryParameters)
│   ├── Interfaces/            # Abstrações de Repositórios e Serviços
│   ├── Mappers/               # Extensões para conversão entre modelos e DTOs
│   ├── Middlewares/           # Tratamento de erros e autorização customizada
│   ├── Models/                # Entidades de domínio
│   ├── Repositories/          # Implementações de acesso a dados
│   ├── Services/              # Lógica de negócio
│   ├── Migrations/            # Histórico de migrações
│   ├── Program.cs             # Configuração geral da aplicação
│   └── appsettings.json       # Configurações de ambiente
└── Api.Tests/                 # Testes automatizados (xUnit, FluentAssertions)
```
---

## 🧪 Como Rodar o Projeto

### ✅ Pré-requisitos

- .NET 8 SDK (https://dotnet.microsoft.com/en-us/download)
- SQL Server local instalado (ou via Docker)
- Visual Studio 2022 ou Visual Studio Code
- Postman ou navegador para testar via Swagger

### ⚙️ Passo a Passo

1. **Clone o repositório:**
git clone https://github.com/victor-macielmf/api-restful.git cd api-restful

2. **Configure a string de conexão no `Api/appsettings.json`:**
"ConnectionStrings": { "DefaultConnection": "Server=localhost;Database=SeuBanco;User Id=usuario;Password=senha;" }

3. **Restaure os pacotes:**
dotnet restore

4. **Execute as migrações:**
dotnet ef database update --project Api

5. **Rode a aplicação:**
dotnet watch --project Api run

Acesse o Swagger em `https://localhost:<porta>/swagger` e use o botão **Authorize** para inserir seu token JWT e testar endpoints protegidos.

---

## 🧪 Como Rodar os Testes

1. **Execute os testes automatizados:** dotnet test

Os testes estão localizados no diretório `Api.Tests/` e cobrem helpers, mappers e services.

---

## 🔐 Segurança e Autenticação

- Autenticação com JWT
- Tokens gerados via POST /api/v2/sessions
- Papéis de usuário (Admin, User)
- Proteção via [Authorize] e [Authorize(Roles = "Admin")]

---

## 🌐 Principais Endpoints

| Método  | Rota                            | Acesso  | Descrição                      |
|---------|----------------------------------|---------|--------------------------------|
| POST    | /api/v2/accounts                | Público | Criar uma conta                |
| POST    | /api/v2/sessions                | Público | Login e geração de token       |
| GET     | /api/v2/stocks               | Público | Listar ações                   |
| POST    | /api/v2/stocks                  | Admin   | Cadastrar nova ação         |
| PUT     | /api/v2/stocks/{id}          | Admin   | Atualizar uma ação existente   |
| DELETE | /api/v2/stocks/{id}          | Admin   | Deletar uma ação               |
| POST    | /api/v2/comments/{stockId}   | User    | Comentar em uma ação           |
| GET     | /api/v2/accountstocks        | User    | Listar ações salvas pelo usuário |

---

## 📏 Boas Práticas Aplicadas

- Separação em camadas (Controller, Service, Repository)
- Uso de DTOs e AutoMapper
- Middleware global de tratamento de erros e autorização customizada
- Filtro de tempo de execução via ExecutionTimeFilter
- Respostas padronizadas com DataResponse<T>
- Swagger com autenticação configurada

## 🤝 Contato

Email: joaovictormacieldefreitas@gmail.com  
LinkedIn: https://www.linkedin.com/in/joao-victor-maciel-de-freitas/