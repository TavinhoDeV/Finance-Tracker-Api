#  Finance Tracker API

A personal finance management REST API built with **Clean Architecture + CQRS + MediatR** in ASP.NET Core 8.

##  Architecture

```
src/
├── FinanceTracker.Domain/          # Entities, Enums, Base classes
├── FinanceTracker.Application/     # CQRS Commands/Queries, MediatR, Validators
├── FinanceTracker.Infrastructure/  # EF Core, SQL Server, JWT, BCrypt
└── FinanceTracker.Api/             # Controllers, Middleware, Swagger
tests/
└── FinanceTracker.Tests/           # xUnit + Moq + FluentAssertions
```

##  Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 |
| Architecture | Clean Architecture |
| CQRS | MediatR 12 |
| Validation | FluentValidation (pipeline behavior) |
| ORM | Entity Framework Core 8 |
| Database | SQL Server |
| Auth | JWT Bearer |
| Password | BCrypt.Net |
| Docs | Swagger / OpenAPI |
| Tests | xUnit + Moq + FluentAssertions |

##  Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (local or Docker)

### Setup

**1. Clone the repository**
```bash
git clone git remote add origin https://github.com/TavinhoDeV/Finance-Tracker-Api.git
cd finance-tracker-api
```

**2. Configure the connection string**

Edit `src/FinanceTracker.Api/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FinanceTrackerDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "your-secret-key-minimum-32-characters!!",
    "Issuer": "FinanceTrackerApi",
    "Audience": "FinanceTrackerClient"
  }
}
```

**3. Run migrations**
```bash
dotnet ef migrations add InitialCreate --project src/FinanceTracker.Infrastructure --startup-project src/FinanceTracker.Api
dotnet ef database update --project src/FinanceTracker.Infrastructure --startup-project src/FinanceTracker.Api
```

**4. Run the API**
```bash
dotnet run --project src/FinanceTracker.Api
```

Access Swagger at: `https://localhost:5001/swagger`

### Run with Docker (SQL Server)

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Your_password123" \
  -p 1433:1433 --name sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Update connection string:
```
Server=localhost,1433;Database=FinanceTrackerDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;
```

##  API Endpoints

### Auth
| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/register` | Register new user |
| POST | `/api/auth/login` | Login and get JWT |

### Accounts
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/accounts` | List all accounts |
| GET | `/api/accounts/{id}` | Get account by ID |
| POST | `/api/accounts` | Create account |
| DELETE | `/api/accounts/{id}` | Delete account |

### Transactions
| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/accounts/{id}/transactions` | List transactions (paginated, filterable) |
| GET | `/api/accounts/{id}/transactions/summary/{year}/{month}` | Monthly summary |
| POST | `/api/accounts/{id}/transactions` | Create transaction |
| DELETE | `/api/accounts/{id}/transactions/{txId}` | Delete transaction |

### Query Filters (GET /transactions)
```
?from=2024-01-01&to=2024-01-31&type=Expense&category=Food&page=1&pageSize=20
```

##  Example Requests

**Register**
```json
POST /api/auth/register
{
  "name": "João Silva",
  "email": "joao@email.com",
  "password": "MinhaS3nha!"
}
```

**Create Account**
```json
POST /api/accounts
Authorization: Bearer {token}
{
  "name": "Conta Corrente",
  "type": 1,
  "initialBalance": 2500.00,
  "currency": "BRL"
}
```

**Create Transaction**
```json
POST /api/accounts/{accountId}/transactions
Authorization: Bearer {token}
{
  "description": "Salário Junho",
  "amount": 5000.00,
  "type": 1,
  "category": 1,
  "date": "2024-06-05T00:00:00Z"
}
```

##  Running Tests

```bash
dotnet test
```

##  Design Patterns

- **Clean Architecture** — dependency flow from outer layers to inner
- **CQRS** — commands and queries in separate classes
- **MediatR Pipeline Behaviors** — cross-cutting concerns (validation)
- **Repository Pattern** — abstracted via `IApplicationDbContext`
- **Result Pattern** — for wrapping operation outcomes
- **Domain-Driven Design** — rich entities with private setters and factory methods
- **Global Exception Handling Middleware** — centralized error responses

##  Account Types

| Value | Type |
|---|---|
| 1 | Checking |
| 2 | Savings |
| 3 | CreditCard |
| 4 | Investment |

##  Transaction Categories

| Value | Category |
|---|---|
| 1 | Salary |
| 2 | Food |
| 3 | Transport |
| 4 | Health |
| 5 | Education |
| 6 | Entertainment |
| 7 | Housing |
| 8 | Utilities |
| 9 | Shopping |
| 10 | Investment |
| 99 | Other |
