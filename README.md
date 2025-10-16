# Enterprise Notification Service (ENS)

A clean, maintainable .NET 8 + React + PostgreSQL application for managing multi-tenant system notifications and alerts displayed as UI banners.

## Features

- **Multi-Tenant Support**: Separate notifications for different tenants/organizations
- **REST API**: Full CRUD operations for notifications and tenants
- **JWT Authentication**: Secure API endpoints with JSON Web Tokens
- **React Admin UI**: Modern, responsive interface for managing notifications
- **Real-time Banner Display**: Show active notifications as UI banners
- **OpenAPI/Swagger**: Interactive API documentation
- **Docker Support**: Complete containerization with docker-compose
- **Clean Architecture**: Separation of concerns with Domain, Infrastructure, and API layers
- **PostgreSQL Database**: Reliable, enterprise-grade data storage
- **Entity Framework Core**: Type-safe database access with migrations

## Technology Stack

### Backend
- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core 8.0
- PostgreSQL with Npgsql
- JWT Bearer Authentication
- Swashbuckle (Swagger/OpenAPI)

### Frontend
- React 18
- TypeScript
- Vite
- Axios
- CSS (inline styles for simplicity)

### DevOps
- Docker
- docker-compose
- PostgreSQL 16

## Project Structure

```
NotificationService/
├── src/
│   ├── NotificationService.Api/          # REST API project
│   │   ├── Controllers/                  # API endpoints
│   │   ├── Models/                       # DTOs
│   │   ├── Services/                     # Business services
│   │   └── Program.cs                    # App configuration
│   ├── NotificationService.Domain/       # Domain models
│   │   ├── Entities/                     # Domain entities
│   │   └── Interfaces/                   # Repository interfaces
│   └── NotificationService.Infrastructure/ # Data access
│       ├── Data/                         # DbContext
│       ├── Migrations/                   # EF migrations
│       └── Repositories/                 # Repository implementations
├── client/                               # React frontend
│   ├── src/
│   │   ├── components/                   # React components
│   │   ├── services/                     # API client
│   │   └── types/                        # TypeScript types
│   └── Dockerfile
├── tests/
│   └── NotificationService.Tests/        # Unit tests
├── docker-compose.yml                    # Docker orchestration
└── README.md
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (for containerized setup)

### Option 1: Run with Docker Compose (Recommended)

This is the easiest way to get started. All services will run in containers.

```bash
# Clone the repository
git clone https://github.com/PlausibleAlibi/NotificationService.git
cd NotificationService

# Start all services
docker-compose up --build

# The services will be available at:
# - Frontend: http://localhost:3000
# - API: http://localhost:5000
# - Swagger: http://localhost:5000/swagger
# - PostgreSQL: localhost:5432
```

### Option 2: Run Locally (Development)

#### 1. Start PostgreSQL

```bash
docker run -d \
  --name postgres-dev \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=notificationservice \
  -p 5432:5432 \
  postgres:16-alpine
```

#### 2. Run the Backend API

```bash
cd src/NotificationService.Api

# Restore dependencies
dotnet restore

# Run migrations (first time only)
dotnet ef database update --project ../NotificationService.Infrastructure

# Run the API
dotnet run

# API will be available at http://localhost:5000
# Swagger UI at http://localhost:5000/swagger
```

#### 3. Run the Frontend

```bash
cd client

# Install dependencies
npm install

# Start development server
npm run dev

# Frontend will be available at http://localhost:3000
```

## Usage

### Login Credentials

Default admin credentials:
- **Username**: `admin`
- **Password**: `admin123`

> **Security Note**: These are demo credentials. In production, implement proper user management with hashed passwords.

### API Endpoints

#### Authentication
- `POST /api/auth/login` - Login and get JWT token

#### Tenants
- `GET /api/tenants` - Get all tenants
- `GET /api/tenants/{id}` - Get tenant by ID
- `GET /api/tenants/code/{code}` - Get tenant by code
- `POST /api/tenants` - Create tenant (requires auth)
- `DELETE /api/tenants/{id}` - Delete tenant (requires auth)

#### Notifications
- `GET /api/notifications/tenant/{tenantId}` - Get all notifications for a tenant
- `GET /api/notifications/tenant/{tenantId}/active` - Get active notifications for a tenant
- `GET /api/notifications/{id}` - Get notification by ID
- `POST /api/notifications` - Create notification (requires auth)
- `PUT /api/notifications/{id}` - Update notification (requires auth)
- `DELETE /api/notifications/{id}` - Delete notification (requires auth)

### Using the Admin UI

1. **Login**: Enter your credentials on the login page
2. **Select Tenant**: Choose a tenant from the dropdown
3. **Create Notification**: Click "Create Notification" button
4. **Fill Form**: Enter title, message, and select type (Info/Warning/Error/Success)
5. **Submit**: Click "Create Notification" to save
6. **Manage**: View all notifications, toggle active status, or delete

### Using the API

#### Example: Create a notification

```bash
# 1. Login to get token
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'

# Response: {"token":"eyJhbGc...", "username":"admin", "expiresAt":"..."}

# 2. Create notification (use token from step 1)
curl -X POST http://localhost:5000/api/notifications \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "tenantId": 1,
    "title": "System Maintenance",
    "message": "Scheduled maintenance tonight from 10 PM to 2 AM",
    "type": "Warning",
    "isActive": true
  }'
```

#### Example: Get active notifications

```bash
curl http://localhost:5000/api/notifications/tenant/1/active
```

## Configuration

### Backend Configuration (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Database=notificationservice;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "NotificationService",
    "Audience": "NotificationServiceClient",
    "ExpiryMinutes": "60"
  },
  "Auth": {
    "DemoUsername": "admin",
    "DemoPassword": "admin123"
  }
}
```

### Frontend Configuration

Set environment variables in `.env` file (create in `client/` directory):

```
VITE_API_URL=http://localhost:5000
```

## Development

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Database Migrations

```bash
# Create a new migration
cd src/NotificationService.Infrastructure
dotnet ef migrations add MigrationName --startup-project ../NotificationService.Api

# Apply migrations
dotnet ef database update --startup-project ../NotificationService.Api
```

### Building for Production

#### Backend
```bash
dotnet publish src/NotificationService.Api/NotificationService.Api.csproj -c Release -o ./publish
```

#### Frontend
```bash
cd client
npm run build
# Output in client/dist/
```

## Architecture

### Clean Architecture Layers

1. **Domain Layer** (`NotificationService.Domain`)
   - Contains business entities (Notification, Tenant)
   - Defines repository interfaces
   - No dependencies on other layers

2. **Infrastructure Layer** (`NotificationService.Infrastructure`)
   - Implements data access with EF Core
   - Contains DbContext and migrations
   - Implements repository interfaces

3. **API Layer** (`NotificationService.Api`)
   - REST API endpoints (Controllers)
   - DTOs for data transfer
   - JWT authentication setup
   - Swagger configuration

### Design Patterns

- **Repository Pattern**: Abstraction for data access
- **Dependency Injection**: Built-in ASP.NET Core DI
- **DTO Pattern**: Separate API models from domain entities
- **Multi-Tenant Pattern**: Tenant isolation at data level

## Notification Types

The system supports four notification types:

- **Info** (Blue): General information messages
- **Warning** (Yellow): Important warnings that need attention
- **Error** (Red): Critical errors or system failures
- **Success** (Green): Successful operations or positive updates

## Security Considerations

### For Development
- Demo credentials are hardcoded for ease of testing
- JWT secret key is in appsettings.json

### For Production
1. **Use Secrets Management**: Store JWT keys in Azure Key Vault, AWS Secrets Manager, etc.
2. **Implement User Management**: Replace demo auth with proper user system
3. **Hash Passwords**: Use bcrypt or similar for password hashing
4. **HTTPS Only**: Enforce HTTPS in production
5. **CORS Configuration**: Restrict allowed origins
6. **Rate Limiting**: Add rate limiting to prevent abuse
7. **Input Validation**: Validate and sanitize all inputs
8. **SQL Injection Protection**: EF Core protects against this by default

## Troubleshooting

### Database Connection Issues
```bash
# Check if PostgreSQL is running
docker ps | grep postgres

# View PostgreSQL logs
docker logs notification-postgres
```

### API Not Starting
```bash
# Check API logs
docker logs notification-api

# Verify database connection string in appsettings.json
```

### Frontend Not Loading
```bash
# Check web server logs
docker logs notification-web

# Verify API URL in frontend environment variables
```

## Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License.

## Support

For issues and questions:
- Open an issue on GitHub
- Check existing documentation
- Review API documentation at `/swagger`

## Roadmap

Future enhancements:
- [ ] Email notifications
- [ ] WebSocket support for real-time updates
- [ ] Notification scheduling
- [ ] User management system
- [ ] Role-based access control
- [ ] Notification templates
- [ ] Analytics and reporting
- [ ] Mobile app
- [ ] Multi-language support
