# 📈 API RESTful - Gestão de Ações e Comentários

Este projeto é uma API RESTful desenvolvida com ASP.NET Core 8.0 para gerenciamento de ações e comentários de usuários. Utiliza autenticação JWT, ASP.NET Identity, Entity Framework Core com Code First, e segue boas práticas de arquitetura e desenvolvimento de software.

---

## 🚀 Tecnologias Utilizadas

- **.NET 8 (ASP.NET Core Web API)**
- **Entity Framework Core (Migrations + SQL Server)**
- **ASP.NET Identity** (controle de usuários e papéis)
- **JWT (JSON Web Token)** para autenticação
- **AutoMapper**
- **Swagger / Swashbuckle** (documentação interativa)
- **Newtonsoft.Json**
- **Filtros personalizados (Execution Timer)**
- **Middleware de tratamento de erros**
- **Repository Pattern + Service Layer**
- **Boas práticas: SOLID, Clean Architecture, DTOs, versionamento**

---

## 📁 Estrutura de Pastas

```
.
├── Controllers/           # Endpoints da API
├── Data/                  # DbContext e configurações do banco
├── Dtos/                  # Data Transfer Objects organizados
├── Helpers/               # Utilitários (JWT, Claims, Responses)
├── Interfaces/            # Abstrações de Repositórios e Serviços
├── Mappers/               # Extensões para conversão entre modelos e DTOs
├── Middlewares/           # Tratamento de erros e autorização
├── Models/                # Entidades de domínio
├── Repositories/          # Implementações de acesso a dados
├── Services/              # Lógica de negócio
├── Migrations/            # Histórico de migrações
├── Program.cs             # Configuração geral da aplicação
└── appsettings.json       # Configurações de ambiente
```

---

## 🧪 Como Rodar o Projeto

### ✅ Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- SQL Server local instalado (ou via Docker)
- Visual Studio ou Visual Studio Code
- Postman ou navegador para testar via Swagger

### ⚙️ Passo a Passo

1. **Clone o repositório:**

```bash
git clone https://github.com/victor-macielmf/api-restful.git
cd api-restful
```

2. **Configure a string de conexão no `appsettings.json`:**

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=Senai;Trusted_Connection=True;"
}
```

3. **Restaure os pacotes:**

```bash
dotnet restore
```

4. **Execute as migrações:**

```bash
dotnet ef database update
```

5. **Rode a aplicação:**

```bash
dotnet run
```

6. **Acesse a documentação no navegador:**

```
https://localhost:7057/swagger
```

Use o botão **Authorize** para inserir seu token JWT e testar endpoints protegidos.

---

## 🔐 Segurança e Autenticação

- Autenticação com JWT
- Tokens gerados via `POST /api/v2/sessions`
- Papéis de usuário (`Admin`, `User`)
- Proteção via `[Authorize]` e `[Authorize(Roles = "Admin")]`

---

## 🌐 Principais Endpoints

| Método  | Rota                            | Acesso  | Descrição                      |
|---------|----------------------------------|---------|--------------------------------|
| POST    | `/api/v2/accounts`              | Público | Criar uma conta                |
| POST    | `/api/v2/sessions`              | Público | Login e geração de token       |
| GET     | `/api/v2/stocks`                | Público | Listar ações                   |
| POST    | `/api/v2/stocks`                | Admin   | Cadastrar nova ação            |
| PUT     | `/api/v2/stocks/{id}`           | Admin   | Atualizar uma ação existente   |
| DELETE  | `/api/v2/stocks/{id}`           | Admin   | Deletar uma ação               |
| POST    | `/api/v2/comments/{stockId}`    | User    | Comentar em uma ação           |
| GET     | `/api/v2/accountstocks`         | User    | Listar ações salvas pelo usuário |

---

## 📏 Boas Práticas Aplicadas

- ✅ Separação em camadas (Controller, Service, Repository)
- ✅ Uso de DTOs e AutoMapper
- ✅ Middleware global de tratamento de erros
- ✅ Filtro de tempo de execução via `ExecutionTimeFilter`
- ✅ Respostas padronizadas com `DataResponse<T>`
- ✅ Swagger com autenticação configurada

## 🤝 Contato

📧 **joaovictormacieldefreitas@gmail.com**  
🔗 [linkedin.com/in/victormacielmf](https://www.linkedin.com/in/joao-victor-maciel-de-freitas/)
