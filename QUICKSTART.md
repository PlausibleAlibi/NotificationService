# Quick Start Guide

This guide will help you get the Enterprise Notification Service up and running in minutes.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop) installed and running

That's it! Docker Compose will handle everything else.

## Start the Application

1. **Clone the repository:**
   ```bash
   git clone https://github.com/PlausibleAlibi/NotificationService.git
   cd NotificationService
   ```

2. **Start all services:**
   ```bash
   docker-compose up --build
   ```

   This will start:
   - PostgreSQL database on port 5432
   - .NET API on port 5000
   - React frontend on port 3000

3. **Wait for services to start** (first time takes 2-3 minutes to build)

   You'll see logs indicating services are ready:
   ```
   notification-postgres | database system is ready to accept connections
   notification-api      | Now listening on: http://0.0.0.0:8080
   notification-web      | start worker process
   ```

## Access the Application

### Web UI
Open your browser and go to: **http://localhost:3000**

Login with demo credentials:
- **Username:** `admin`
- **Password:** `admin123`

### API Documentation
Interactive Swagger UI: **http://localhost:5000/swagger**

## Quick Demo

Once logged in, you can:

1. **View existing tenant:**
   - The system starts with a "Default Tenant" pre-configured

2. **Create a notification:**
   - Click "Create Notification" button
   - Fill in:
     - Title: "System Maintenance"
     - Message: "Scheduled maintenance tonight from 10 PM to 2 AM"
     - Type: "Warning"
   - Click "Create Notification"

3. **View the notification:**
   - The notification appears as a banner with the warning style (yellow)
   - You can toggle it active/inactive
   - You can delete it

4. **Test the public API:**
   ```bash
   # Get active notifications (no auth required)
   curl http://localhost:5000/api/notifications/tenant/1/active
   ```

## Using the API

### 1. Login to get JWT token:

```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "admin",
  "expiresAt": "2025-10-16T13:30:00Z"
}
```

### 2. Create a notification (requires auth):

```bash
curl -X POST http://localhost:5000/api/notifications \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "tenantId": 1,
    "title": "Scheduled Downtime",
    "message": "System will be down for maintenance",
    "type": "Error",
    "isActive": true
  }'
```

### 3. Get active notifications (public, no auth):

```bash
curl http://localhost:5000/api/notifications/tenant/1/active
```

### 4. Get all tenants:

```bash
curl http://localhost:5000/api/tenants
```

## Stopping the Application

```bash
# Stop services (keeps data)
docker-compose down

# Stop and remove all data
docker-compose down -v
```

## Restarting

```bash
# Start existing containers
docker-compose up

# Rebuild if you made code changes
docker-compose up --build
```

## Development Mode

If you want to develop locally without Docker:

### 1. Start PostgreSQL only:
```bash
docker run -d \
  --name postgres-dev \
  -e POSTGRES_USER=postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=notificationservice \
  -p 5432:5432 \
  postgres:16-alpine
```

### 2. Start the API:
```bash
cd src/NotificationService.Api
dotnet run
```

### 3. Start the frontend:
```bash
cd client
npm install
npm run dev
```

Frontend will be at http://localhost:3000
API will be at http://localhost:5000

## Troubleshooting

### Database connection issues:
```bash
# Check if PostgreSQL is running
docker ps | grep postgres

# View PostgreSQL logs
docker logs notification-postgres
```

### API not starting:
```bash
# Check API logs
docker logs notification-api

# Restart just the API
docker-compose restart api
```

### Frontend not loading:
```bash
# Check web logs
docker logs notification-web

# Rebuild frontend
docker-compose up --build web
```

### Clear all data and start fresh:
```bash
docker-compose down -v
docker-compose up --build
```

## Next Steps

- Read the full [README.md](README.md) for detailed documentation
- Explore the API at http://localhost:5000/swagger
- Review the code architecture in the README
- Check out the tests: `dotnet test`

## Support

For issues or questions:
- Check the [README.md](README.md)
- Open an issue on GitHub
- Review the Swagger documentation

## Security Note

This quick start uses demo credentials. For production:
- Change JWT secret key
- Implement proper user authentication
- Use environment variables for secrets
- Enable HTTPS
