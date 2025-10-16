# API Examples

Complete examples for interacting with the Enterprise Notification Service API.

## Base URL

- Local: `http://localhost:5000/api`
- With Docker: `http://localhost:5000/api`

## Authentication

### Login

Get a JWT token for authenticated operations.

**Request:**
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "admin123"
  }'
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbiIsImp0aSI6IjEyMzQ1Njc4LTEyMzQtMTIzNC0xMjM0LTEyMzQ1Njc4OTBhYiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJhZG1pbiIsImV4cCI6MTcyOTEwMTYwMCwiaXNzIjoiTm90aWZpY2F0aW9uU2VydmljZSIsImF1ZCI6Ik5vdGlmaWNhdGlvblNlcnZpY2VDbGllbnQifQ.abcdef123456...",
  "username": "admin",
  "expiresAt": "2025-10-16T13:30:00Z"
}
```

**Save the token** for use in authenticated requests:
```bash
export TOKEN="your_token_here"
```

## Tenants

### Get All Tenants

**Request:**
```bash
curl http://localhost:5000/api/tenants
```

**Response:**
```json
[
  {
    "id": 1,
    "code": "default",
    "name": "Default Tenant",
    "isActive": true,
    "createdAt": "2025-01-01T00:00:00Z"
  }
]
```

### Get Tenant by ID

**Request:**
```bash
curl http://localhost:5000/api/tenants/1
```

**Response:**
```json
{
  "id": 1,
  "code": "default",
  "name": "Default Tenant",
  "isActive": true,
  "createdAt": "2025-01-01T00:00:00Z"
}
```

### Get Tenant by Code

**Request:**
```bash
curl http://localhost:5000/api/tenants/code/default
```

**Response:**
```json
{
  "id": 1,
  "code": "default",
  "name": "Default Tenant",
  "isActive": true,
  "createdAt": "2025-01-01T00:00:00Z"
}
```

### Create Tenant

**Request:**
```bash
curl -X POST http://localhost:5000/api/tenants \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "code": "acme-corp",
    "name": "ACME Corporation"
  }'
```

**Response:**
```json
{
  "id": 2,
  "code": "acme-corp",
  "name": "ACME Corporation",
  "isActive": true,
  "createdAt": "2025-10-16T12:30:00Z"
}
```

### Delete Tenant

**Request:**
```bash
curl -X DELETE http://localhost:5000/api/tenants/2 \
  -H "Authorization: Bearer $TOKEN"
```

**Response:** `204 No Content`

## Notifications

### Get All Notifications for a Tenant

**Request:**
```bash
curl http://localhost:5000/api/notifications/tenant/1
```

**Response:**
```json
[
  {
    "id": 1,
    "tenantId": 1,
    "title": "System Maintenance",
    "message": "Scheduled maintenance tonight from 10 PM to 2 AM",
    "type": "Warning",
    "isActive": true,
    "startDate": null,
    "endDate": null,
    "createdAt": "2025-10-16T12:00:00Z",
    "updatedAt": null,
    "createdBy": "admin"
  }
]
```

### Get Active Notifications for a Tenant

This endpoint returns only notifications that are:
- `isActive = true`
- `startDate` is null or in the past
- `endDate` is null or in the future

**Request:**
```bash
curl http://localhost:5000/api/notifications/tenant/1/active
```

**Response:**
```json
[
  {
    "id": 1,
    "tenantId": 1,
    "title": "System Maintenance",
    "message": "Scheduled maintenance tonight from 10 PM to 2 AM",
    "type": "Warning",
    "isActive": true,
    "startDate": null,
    "endDate": null,
    "createdAt": "2025-10-16T12:00:00Z",
    "updatedAt": null,
    "createdBy": "admin"
  }
]
```

### Get Notification by ID

**Request:**
```bash
curl http://localhost:5000/api/notifications/1
```

**Response:**
```json
{
  "id": 1,
  "tenantId": 1,
  "title": "System Maintenance",
  "message": "Scheduled maintenance tonight from 10 PM to 2 AM",
  "type": "Warning",
  "isActive": true,
  "startDate": null,
  "endDate": null,
  "createdAt": "2025-10-16T12:00:00Z",
  "updatedAt": null,
  "createdBy": "admin"
}
```

### Create Notification

**Info Notification:**
```bash
curl -X POST http://localhost:5000/api/notifications \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "tenantId": 1,
    "title": "New Feature Released",
    "message": "Check out our new dashboard feature!",
    "type": "Info",
    "isActive": true
  }'
```

**Warning Notification:**
```bash
curl -X POST http://localhost:5000/api/notifications \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "tenantId": 1,
    "title": "Scheduled Maintenance",
    "message": "System maintenance from 10 PM to 2 AM tonight",
    "type": "Warning",
    "isActive": true
  }'
```

**Error Notification:**
```bash
curl -X POST http://localhost:5000/api/notifications \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "tenantId": 1,
    "title": "Service Disruption",
    "message": "We are experiencing technical difficulties. Our team is working on a fix.",
    "type": "Error",
    "isActive": true
  }'
```

**Success Notification:**
```bash
curl -X POST http://localhost:5000/api/notifications \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "tenantId": 1,
    "title": "Maintenance Complete",
    "message": "All systems are back online and running smoothly",
    "type": "Success",
    "isActive": true
  }'
```

**Scheduled Notification (with dates):**
```bash
curl -X POST http://localhost:5000/api/notifications \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "tenantId": 1,
    "title": "Holiday Hours",
    "message": "We will be closed on December 25th",
    "type": "Info",
    "isActive": true,
    "startDate": "2025-12-20T00:00:00Z",
    "endDate": "2025-12-26T00:00:00Z"
  }'
```

**Response:**
```json
{
  "id": 2,
  "tenantId": 1,
  "title": "New Feature Released",
  "message": "Check out our new dashboard feature!",
  "type": "Info",
  "isActive": true,
  "startDate": null,
  "endDate": null,
  "createdAt": "2025-10-16T12:30:00Z",
  "updatedAt": null,
  "createdBy": "admin"
}
```

### Update Notification

**Request:**
```bash
curl -X PUT http://localhost:5000/api/notifications/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "title": "Updated Title",
    "message": "Updated message",
    "isActive": false
  }'
```

**Response:** `204 No Content`

**Partial Update (only change isActive):**
```bash
curl -X PUT http://localhost:5000/api/notifications/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "isActive": false
  }'
```

### Delete Notification

**Request:**
```bash
curl -X DELETE http://localhost:5000/api/notifications/1 \
  -H "Authorization: Bearer $TOKEN"
```

**Response:** `204 No Content`

## Notification Types

The API supports four notification types:

| Type | Use Case | Color in UI |
|------|----------|-------------|
| `Info` | General information, announcements | Blue |
| `Warning` | Important notices, upcoming changes | Yellow |
| `Error` | Critical issues, service disruptions | Red |
| `Success` | Positive updates, completed operations | Green |

## Error Responses

### 401 Unauthorized

**Scenario:** No token or invalid token

**Response:**
```json
{
  "message": "Unauthorized"
}
```

### 404 Not Found

**Scenario:** Resource doesn't exist

**Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

### 400 Bad Request

**Scenario:** Invalid input data

**Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Title": ["The Title field is required."]
  }
}
```

## Complete Example Workflow

Here's a complete workflow for creating and managing notifications:

```bash
# 1. Login
TOKEN=$(curl -s -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}' | jq -r '.token')

echo "Token: $TOKEN"

# 2. Get tenants
curl -s http://localhost:5000/api/tenants | jq

# 3. Create a warning notification
curl -s -X POST http://localhost:5000/api/notifications \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "tenantId": 1,
    "title": "Scheduled Maintenance",
    "message": "System will be down tonight",
    "type": "Warning",
    "isActive": true
  }' | jq

# 4. Get active notifications
curl -s http://localhost:5000/api/notifications/tenant/1/active | jq

# 5. Update notification to inactive
curl -X PUT http://localhost:5000/api/notifications/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"isActive": false}'

# 6. Verify it's inactive
curl -s http://localhost:5000/api/notifications/tenant/1/active | jq

# 7. Delete the notification
curl -X DELETE http://localhost:5000/api/notifications/1 \
  -H "Authorization: Bearer $TOKEN"
```

## Using with JavaScript/TypeScript

```typescript
// Login
const response = await fetch('http://localhost:5000/api/auth/login', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ username: 'admin', password: 'admin123' })
});
const { token } = await response.json();

// Get active notifications
const notifications = await fetch('http://localhost:5000/api/notifications/tenant/1/active')
  .then(res => res.json());

// Create notification
const newNotification = await fetch('http://localhost:5000/api/notifications', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  },
  body: JSON.stringify({
    tenantId: 1,
    title: 'New Notification',
    message: 'This is a test',
    type: 'Info',
    isActive: true
  })
}).then(res => res.json());
```

## Using with Python

```python
import requests

# Login
response = requests.post('http://localhost:5000/api/auth/login', json={
    'username': 'admin',
    'password': 'admin123'
})
token = response.json()['token']

# Get active notifications
headers = {'Authorization': f'Bearer {token}'}
notifications = requests.get(
    'http://localhost:5000/api/notifications/tenant/1/active'
).json()

# Create notification
new_notification = requests.post(
    'http://localhost:5000/api/notifications',
    headers=headers,
    json={
        'tenantId': 1,
        'title': 'New Notification',
        'message': 'This is a test',
        'type': 'Info',
        'isActive': True
    }
).json()
```

## Rate Limiting

Currently, there are no rate limits on the API. For production use, consider implementing rate limiting to prevent abuse.

## CORS

The API allows CORS requests from:
- `http://localhost:3000` (React dev server)
- `http://localhost:5173` (Vite dev server)

For production, update the CORS configuration in `Program.cs`.
