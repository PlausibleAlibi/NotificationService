# 🧭 Project Prompt: Enterprise Notification Service (ENS)

## Build Request
Create a complete, production-quality scaffold for an **Enterprise Notification Service (ENS)** — a system that allows different enterprise applications to **register, manage, and display downtime alerts, technical issues, or service disruptions** across the organization.

The initial use case focuses on **in-app banners**, but the system should be designed to later extend to **email, PagerDuty**, or other delivery channels.

---

## 🧱 Tech Stack
- **Backend:** .NET 8 / C# (Clean Architecture + MediatR)
- **Frontend:** React + TypeScript + Material UI v6
- **Database:** PostgreSQL (EF Core)
- **Auth:** JWT-based authentication (Azure AD B2C compatible)
- **Observability:** OpenTelemetry + Application Insights or Grafana
- **Containerization:** Docker + Docker Compose for local and development environments
- **Deployment Target:** Azure Kubernetes Service (AKS)

---

## 🧩 Code Quality & Maintainability Guidelines
The codebase must be **clean, modular, and easy to understand**, suitable for engineers of mixed experience levels.

- Prioritize **readability and maintainability** over cleverness or over-abstraction.
- Follow **Clean Architecture** and **SOLID principles** in a simple, approachable way.
- Use consistent naming conventions (PascalCase for classes, camelCase for methods/variables).
- Include **XML / JSDoc comments** for all public classes, methods, and data models.
- Favor **explicit, descriptive code** over advanced generics or unnecessary abstraction.
- Keep the **project structure flat and discoverable**; avoid excessive layers.
- Include **unit and integration tests** following Arrange / Act / Assert.
- Add **inline comments** where developer intent may not be obvious.
- Provide **sample data**, **seed scripts**, and clear **README documentation** for onboarding.
- Follow **Microsoft .NET and Airbnb TypeScript style guides**.

---

## 🎯 Functional Requirements

### Core Features
1. **Message Registry:** APIs for registering or querying enterprise notifications.
2. **Banner Delivery:** Clients can request active messages filtered by tenant/app/environment.
3. **Templates:** Reusable notification templates (HTML/Markdown).
4. **Schedules:** Start/end times, recurrence, and expiration handling.
5. **Targeting Rules:** Message targeting by app, tenant, or user group.
6. **Audit & Delivery Tracking:** Store history, acknowledgments, and changes.

---

## 🧠 Architecture
- **Backend:** .NET 8 with EF Core, MediatR, FluentValidation.
- **Frontend:** React + TypeScript + Material UI v6.
- **SDKs:**
  - Vanilla JS SDK for legacy web apps.
  - React SDK wrapper with theme token support for consistent branding.
- **Auth & Security:** JWT integration (Azure AD B2C / Auth0).
- **Observability:** OpenTelemetry traces and metrics.
- **Health Checks:** `/health/live` and `/health/ready` endpoints.

---

## 🔒 Non-Functional Requirements
- Scalable multi-tenant design (Tenants → Applications → Environments).
- 99.9% uptime SLO for API; 2s p95 latency target.
- Include **runbooks** for incident response.
- Role-based access control for admins and app owners.
- Secure configuration management using environment variables and secrets.

---

## 🚀 MVP Scope

### Included
- Clean domain model (Tenants, Applications, Templates, Instances, Schedules, Audit).
- .NET REST API with Swagger / OpenAPI spec.
- React Admin UI for message management.
- Vanilla JS and React SDKs for consuming banners.
- JWT authentication and RBAC.
- Unit & integration tests.
- **Dockerfile** and **docker-compose.yml** for full local environment (API, DB, UI).
- CI/CD with GitHub Actions (build, test, deploy).
- Documentation and sample configuration files.

### Deferred (Phase 2)
- Email, PagerDuty, Slack integrations.
- Advanced targeting workflows.
- SSO Admin portal.
- Real-time updates (SignalR/WebSocket).



## 📁 Repository Structure
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

## ⚙️ Getting Started
1. Clone repository and start containers:
   ```bash
   git clone https://github.com/<org>/<repo>.git
   cd <repo>
   docker compose up --build
2.	Access Swagger UI: https://localhost:5001/swagger
	3.	Access Admin UI: http://localhost:3000
	4.	Use seeded demo credentials for testing.
🔭 Future Enhancements
	•	ServiceNow or Jira Ops integration.
	•	WebSocket (SignalR) live banner updates.
	•	SLA dashboard and Ops tooling.
	•	CLI / PowerShell command-line support.
	•	Multi-brand theming system.

⸻

✅ Deliverables
	•	Full .NET + React repository scaffold.
	•	Working local environment with Docker and Compose.
	•	Clean, documented, maintainable source code.
	•	CI/CD GitHub Actions workflow.
	•	README with setup, architecture overview, and contribution guide.
  
