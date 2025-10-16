# Architecture Documentation

## System Overview

The Enterprise Notification Service (ENS) is built using a clean, layered architecture that separates concerns and promotes maintainability.

```
┌─────────────────────────────────────────────────────────────────┐
│                         User Browser                            │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                    React Frontend (Port 3000)                   │
│  - TypeScript + Vite                                            │
│  - Components: Login, AdminPanel, NotificationBanner            │
│  - Services: API client with Axios                              │
└────────────────────────────┬────────────────────────────────────┘
                             │ HTTP/REST
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                   .NET API Layer (Port 5000)                    │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  Controllers                                            │   │
│  │  - AuthController (JWT tokens)                          │   │
│  │  - NotificationsController (CRUD operations)            │   │
│  │  - TenantsController (Multi-tenant management)          │   │
│  └─────────────────────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  Services                                               │   │
│  │  - JwtService (Token generation/validation)             │   │
│  └─────────────────────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  Models (DTOs)                                          │   │
│  │  - NotificationDto, TenantDto, AuthModels               │   │
│  └─────────────────────────────────────────────────────────┘   │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Infrastructure Layer                         │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  Repositories (Interface Implementation)                │   │
│  │  - NotificationRepository                               │   │
│  │  - TenantRepository                                     │   │
│  └─────────────────────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  Data Access                                            │   │
│  │  - NotificationDbContext (EF Core)                      │   │
│  │  - Migrations                                           │   │
│  └─────────────────────────────────────────────────────────┘   │
└────────────────────────────┬────────────────────────────────────┘
                             │ EF Core
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Domain Layer                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  Entities                                               │   │
│  │  - Notification (Business entity)                       │   │
│  │  - Tenant (Business entity)                             │   │
│  │  - NotificationType (Enum)                              │   │
│  └─────────────────────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  Interfaces                                             │   │
│  │  - INotificationRepository                              │   │
│  │  - ITenantRepository                                    │   │
│  └─────────────────────────────────────────────────────────┘   │
└────────────────────────────┬────────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────────┐
│               PostgreSQL Database (Port 5432)                   │
│  - Tables: Tenants, Notifications                               │
│  - Indexes: TenantId, Code, IsActive                            │
└─────────────────────────────────────────────────────────────────┘
```

## Layered Architecture

### 1. Domain Layer (`NotificationService.Domain`)

**Purpose:** Contains core business logic and entities

**Components:**
- **Entities:**
  - `Notification`: Represents a system notification/alert
  - `Tenant`: Represents a tenant in the multi-tenant system
  - `NotificationType`: Enum for notification severity (Info, Warning, Error, Success)

- **Interfaces:**
  - `INotificationRepository`: Contract for notification data operations
  - `ITenantRepository`: Contract for tenant data operations

**Dependencies:** None (pure domain logic)

**Key Principles:**
- No dependencies on other layers
- Contains only business entities and interfaces
- Framework-agnostic
- Represents the core business domain

### 2. Infrastructure Layer (`NotificationService.Infrastructure`)

**Purpose:** Implements data access and external dependencies

**Components:**
- **Data Access:**
  - `NotificationDbContext`: EF Core database context
  - Database Migrations

- **Repositories:**
  - `NotificationRepository`: Implements `INotificationRepository`
  - `TenantRepository`: Implements `ITenantRepository`

**Dependencies:**
- Domain Layer (references entities and interfaces)
- Entity Framework Core
- Npgsql (PostgreSQL provider)

**Key Responsibilities:**
- Database schema management
- CRUD operations implementation
- Query optimization
- Data persistence

### 3. API Layer (`NotificationService.Api`)

**Purpose:** Exposes HTTP endpoints and handles web concerns

**Components:**
- **Controllers:**
  - `AuthController`: Authentication endpoints
  - `NotificationsController`: Notification CRUD operations
  - `TenantsController`: Tenant management

- **Models (DTOs):**
  - Data Transfer Objects for API communication
  - Separate from domain entities for API versioning

- **Services:**
  - `JwtService`: JWT token generation and validation

**Dependencies:**
- Infrastructure Layer
- Domain Layer
- ASP.NET Core
- Swashbuckle (Swagger)

**Key Responsibilities:**
- HTTP request/response handling
- Input validation
- Authentication/Authorization
- API documentation (Swagger)
- CORS configuration

### 4. Frontend (`client/`)

**Purpose:** User interface for notification management

**Components:**
- **Components:**
  - `Login`: Authentication UI
  - `AdminPanel`: Main dashboard
  - `NotificationBanner`: Notification display component

- **Services:**
  - `api.ts`: HTTP client for API communication

- **Types:**
  - TypeScript interfaces matching API contracts

**Dependencies:**
- React 18
- TypeScript
- Axios (HTTP client)
- Vite (build tool)

**Key Responsibilities:**
- User interaction
- API consumption
- State management
- Responsive UI

## Data Flow

### Creating a Notification

```
1. User fills form in AdminPanel
   ↓
2. Form data sent to API via axios
   POST /api/notifications
   ↓
3. NotificationsController receives request
   - Validates input
   - Checks JWT token
   - Maps DTO to Entity
   ↓
4. Calls NotificationRepository.CreateAsync()
   ↓
5. Repository uses DbContext to save
   - Sets timestamps
   - Validates relationships
   ↓
6. EF Core executes SQL INSERT
   ↓
7. PostgreSQL stores data
   ↓
8. Entity returned with ID
   ↓
9. Controller maps to DTO
   ↓
10. Response sent to frontend
    ↓
11. UI updates with new notification
```

### Getting Active Notifications (Public)

```
1. User browser requests active notifications
   GET /api/notifications/tenant/1/active
   ↓
2. NotificationsController.GetActiveByTenant()
   - No authentication required (public endpoint)
   ↓
3. Calls NotificationRepository.GetActiveByTenantAsync()
   ↓
4. Repository queries with filters:
   - IsActive = true
   - StartDate <= now OR null
   - EndDate >= now OR null
   ↓
5. EF Core executes SQL SELECT with WHERE
   ↓
6. PostgreSQL returns matching rows
   ↓
7. Entities mapped to DTOs
   ↓
8. JSON response sent to frontend
   ↓
9. NotificationBanner components render
```

## Authentication Flow

```
1. User enters credentials in Login component
   ↓
2. POST /api/auth/login
   ↓
3. AuthController validates credentials
   - Hardcoded demo auth (admin/admin123)
   ↓
4. JwtService.GenerateToken()
   - Creates JWT with claims
   - Sets expiration (60 minutes)
   ↓
5. Token returned to frontend
   ↓
6. Frontend stores token in localStorage
   ↓
7. Subsequent requests include:
   Authorization: Bearer <token>
   ↓
8. ASP.NET JWT middleware validates
   - Signature verification
   - Expiration check
   - Claims extraction
   ↓
9. [Authorize] attribute grants access
```

## Database Schema

### Tenants Table

```sql
CREATE TABLE "Tenants" (
    "Id" SERIAL PRIMARY KEY,
    "Code" VARCHAR(100) NOT NULL UNIQUE,
    "Name" VARCHAR(200) NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX "IX_Tenants_Code" ON "Tenants" ("Code");
```

### Notifications Table

```sql
CREATE TABLE "Notifications" (
    "Id" SERIAL PRIMARY KEY,
    "TenantId" INTEGER NOT NULL,
    "Title" VARCHAR(200) NOT NULL,
    "Message" VARCHAR(2000) NOT NULL,
    "Type" INTEGER NOT NULL,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "StartDate" TIMESTAMP NULL,
    "EndDate" TIMESTAMP NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP NULL,
    "CreatedBy" VARCHAR(100),
    
    CONSTRAINT "FK_Notifications_Tenants" 
        FOREIGN KEY ("TenantId") 
        REFERENCES "Tenants" ("Id") 
        ON DELETE CASCADE
);

CREATE INDEX "IX_Notifications_TenantId_IsActive" 
    ON "Notifications" ("TenantId", "IsActive");
```

## Design Patterns

### 1. Repository Pattern

**Purpose:** Abstract data access logic

**Implementation:**
- Interface defined in Domain layer
- Implementation in Infrastructure layer
- Controllers depend on interface, not implementation

**Benefits:**
- Testability (can mock repositories)
- Flexibility (can swap data sources)
- Separation of concerns

### 2. Dependency Injection

**Implementation:** ASP.NET Core built-in DI

```csharp
// Program.cs
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();

// Controller
public NotificationsController(
    INotificationRepository repository,
    ILogger<NotificationsController> logger)
{
    _repository = repository;
    _logger = logger;
}
```

**Benefits:**
- Loose coupling
- Easy testing
- Centralized configuration

### 3. DTO Pattern

**Purpose:** Separate API contracts from domain entities

**Implementation:**
- `CreateNotificationDto` for input
- `NotificationDto` for output
- Manual mapping in controllers

**Benefits:**
- API versioning flexibility
- Hide internal structure
- Validation at API boundary

### 4. Clean Architecture

**Principles:**
- Domain layer has no dependencies
- Dependencies point inward
- Business logic isolated from infrastructure

**Benefits:**
- Framework independence
- Testability
- Maintainability

## Technology Choices

### Backend: .NET 8

**Why:**
- Modern, high-performance
- Cross-platform
- Excellent tooling
- Strong typing
- Built-in DI and middleware

### Frontend: React + TypeScript

**Why:**
- Component-based architecture
- Large ecosystem
- TypeScript for type safety
- Excellent developer experience

### Database: PostgreSQL

**Why:**
- Open source
- ACID compliant
- Great performance
- JSON support for future needs
- Strong community

### ORM: Entity Framework Core

**Why:**
- Type-safe queries (LINQ)
- Automatic migrations
- Change tracking
- Supports PostgreSQL well

### Authentication: JWT

**Why:**
- Stateless
- Works well with SPA
- Industry standard
- Easy to implement

## Security Considerations

### Current Implementation

1. **JWT Authentication:**
   - Token-based auth for protected endpoints
   - 60-minute expiration

2. **CORS:**
   - Configured for localhost development
   - Restricts origins

3. **Authorization:**
   - `[Authorize]` attribute on sensitive endpoints
   - Public endpoints for viewing active notifications

### Production Recommendations

1. **User Management:**
   - Replace hardcoded credentials
   - Implement user database
   - Hash passwords (bcrypt)

2. **Secrets Management:**
   - Use environment variables
   - Azure Key Vault / AWS Secrets Manager
   - Don't commit secrets

3. **HTTPS:**
   - Force HTTPS in production
   - Use proper certificates

4. **Rate Limiting:**
   - Prevent abuse
   - Protect against DoS

5. **Input Validation:**
   - Sanitize all inputs
   - Validate DTOs
   - Prevent SQL injection (EF Core helps)

6. **Logging:**
   - Log security events
   - Monitor failed login attempts

## Scalability

### Current Architecture Supports:

1. **Horizontal Scaling:**
   - Stateless API (JWT tokens)
   - Can run multiple API instances

2. **Database Scaling:**
   - PostgreSQL replication
   - Read replicas for queries
   - Connection pooling

3. **Caching Opportunities:**
   - Redis for active notifications
   - CDN for frontend static assets

### Future Improvements:

1. **Message Queue:**
   - RabbitMQ/Kafka for async operations
   - Notification distribution

2. **WebSockets:**
   - Real-time updates
   - Server-sent events

3. **Microservices:**
   - Separate tenant service
   - Notification delivery service

## Testing Strategy

### Unit Tests

**Current:** Repository layer tests

**Coverage:**
- CRUD operations
- Business logic
- Edge cases

**Future:**
- Service layer tests
- Controller tests
- Frontend component tests

### Integration Tests

**Potential:**
- API endpoint tests
- Database integration tests
- End-to-end tests

## Deployment Architecture

### Docker Compose (Development)

```
┌──────────────────────────────────────┐
│  Docker Host                         │
│  ┌────────────────────────────────┐  │
│  │  notification-web (nginx)      │  │
│  │  Port: 3000                    │  │
│  └────────────────────────────────┘  │
│  ┌────────────────────────────────┐  │
│  │  notification-api              │  │
│  │  Port: 5000                    │  │
│  └────────────────────────────────┘  │
│  ┌────────────────────────────────┐  │
│  │  notification-postgres         │  │
│  │  Port: 5432                    │  │
│  │  Volume: postgres_data         │  │
│  └────────────────────────────────┘  │
│                                      │
│  Network: notification-network       │
└──────────────────────────────────────┘
```

### Production (Example: Kubernetes)

```
┌─────────────────────────────────────────┐
│  Load Balancer                          │
└────────────┬────────────────────────────┘
             │
             ▼
┌─────────────────────────────────────────┐
│  Ingress Controller                     │
└────────────┬────────────────────────────┘
             │
     ┌───────┴────────┐
     ▼                ▼
┌─────────┐      ┌─────────┐
│ Web Pod │      │ Web Pod │
│ (Nginx) │      │ (Nginx) │
└─────────┘      └─────────┘
     │                │
     └────────┬───────┘
              ▼
┌──────────────────────────────────┐
│  API Service                     │
│  ┌────────┐  ┌────────┐         │
│  │API Pod │  │API Pod │         │
│  └────────┘  └────────┘         │
└──────────────────────────────────┘
              │
              ▼
┌──────────────────────────────────┐
│  PostgreSQL StatefulSet          │
│  - Primary + Replicas            │
│  - Persistent Volumes            │
└──────────────────────────────────┘
```

## Monitoring and Observability

### Logging

**Current:**
- ASP.NET Core logging
- Console output

**Production:**
- Structured logging (Serilog)
- Log aggregation (ELK/CloudWatch)
- Error tracking (Sentry)

### Metrics

**Recommendations:**
- Application metrics (Prometheus)
- Database metrics
- API latency tracking

### Health Checks

**Current:** Basic startup checks

**Future:**
- `/health` endpoint
- Database connectivity check
- Dependency health

## Conclusion

The Enterprise Notification Service uses modern, proven technologies and patterns to create a maintainable, scalable system. The clean architecture allows for easy testing, modification, and extension as requirements evolve.
