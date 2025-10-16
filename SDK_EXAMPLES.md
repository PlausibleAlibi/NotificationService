# SDK Usage Examples

Complete examples for integrating the Enterprise Notification Service SDKs into your applications.

## Table of Contents
- [Vanilla JavaScript SDK](#vanilla-javascript-sdk)
- [React SDK](#react-sdk)
- [Integration Patterns](#integration-patterns)

## Vanilla JavaScript SDK

### Basic Integration

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>My Application</title>
</head>
<body>
    <h1>My Application</h1>
    <div id="content">
        <!-- Your application content -->
    </div>

    <!-- Include the SDK -->
    <script src="./sdks/vanilla-js/notification-banner-sdk.js"></script>
    
    <script>
        // Initialize and start the SDK
        const notificationSDK = new NotificationBannerSDK({
            apiUrl: 'http://localhost:5000',
            tenantId: 1,
            pollInterval: 30000 // Check every 30 seconds
        });
        
        notificationSDK.start();
    </script>
</body>
</html>
```

### Custom Theme Example

```javascript
const notificationSDK = new NotificationBannerSDK({
    apiUrl: 'https://notifications.mycompany.com',
    tenantId: 42,
    pollInterval: 60000,
    theme: {
        Info: { bg: '#007bff', text: '#ffffff' },
        Warning: { bg: '#ffc107', text: '#212529' },
        Error: { bg: '#dc3545', text: '#ffffff' },
        Success: { bg: '#28a745', text: '#ffffff' }
    }
});

notificationSDK.start();
```

### Conditional Loading

```javascript
// Only load notifications for authenticated users
if (userIsAuthenticated()) {
    const sdk = new NotificationBannerSDK({
        apiUrl: 'http://localhost:5000',
        tenantId: getCurrentTenantId(),
        pollInterval: 45000
    });
    
    sdk.start();
    
    // Stop when user logs out
    window.addEventListener('logout', () => {
        sdk.stop();
    });
}
```

### Manual Control

```javascript
const sdk = new NotificationBannerSDK({
    apiUrl: 'http://localhost:5000',
    tenantId: 1
});

// Start polling
document.getElementById('start-btn').addEventListener('click', () => {
    sdk.start();
});

// Stop polling
document.getElementById('stop-btn').addEventListener('click', () => {
    sdk.stop();
});

// Clear dismissed notifications
document.getElementById('reset-btn').addEventListener('click', () => {
    sdk.clearDismissed();
});
```

## React SDK

### Basic Integration

```tsx
// App.tsx
import { useNotificationBanner, NotificationBannerContainer } from './sdks/react';

function App() {
  const { notifications, dismissNotification } = useNotificationBanner({
    apiUrl: process.env.REACT_APP_API_URL || 'http://localhost:5000',
    tenantId: 1,
    pollInterval: 30000
  });

  return (
    <div>
      <NotificationBannerContainer
        notifications={notifications}
        onDismiss={dismissNotification}
      />
      
      <header>
        <h1>My Application</h1>
      </header>
      
      <main>
        {/* Your app content */}
      </main>
    </div>
  );
}

export default App;
```

### With Custom Theme

```tsx
import { useNotificationBanner, NotificationBannerContainer } from './sdks/react';

const customTheme = {
  Info: { bg: '#3b82f6', text: '#ffffff' },
  Warning: { bg: '#f59e0b', text: '#ffffff' },
  Error: { bg: '#ef4444', text: '#ffffff' },
  Success: { bg: '#10b981', text: '#ffffff' }
};

function App() {
  const { notifications, dismissNotification } = useNotificationBanner({
    apiUrl: 'http://localhost:5000',
    tenantId: 1
  });

  return (
    <NotificationBannerContainer
      notifications={notifications}
      onDismiss={dismissNotification}
      theme={customTheme}
      position="top"
    />
  );
}
```

### Error Handling

```tsx
import { useState } from 'react';
import { useNotificationBanner, NotificationBannerContainer } from './sdks/react';

function App() {
  const [apiError, setApiError] = useState<string | null>(null);

  const { notifications, dismissNotification, error } = useNotificationBanner({
    apiUrl: 'http://localhost:5000',
    tenantId: 1,
    onError: (err) => {
      console.error('Notification error:', err);
      setApiError(err.message);
    }
  });

  return (
    <div>
      {apiError && (
        <div style={{ padding: '10px', background: '#fee', color: '#c00' }}>
          Error loading notifications: {apiError}
        </div>
      )}
      
      <NotificationBannerContainer
        notifications={notifications}
        onDismiss={dismissNotification}
      />
      
      {/* Rest of app */}
    </div>
  );
}
```

### With Loading State

```tsx
import { useNotificationBanner, NotificationBannerContainer } from './sdks/react';

function App() {
  const {
    notifications,
    dismissNotification,
    loading,
    error
  } = useNotificationBanner({
    apiUrl: 'http://localhost:5000',
    tenantId: 1
  });

  if (loading && notifications.length === 0) {
    return <div>Loading notifications...</div>;
  }

  if (error) {
    return <div>Error: {error.message}</div>;
  }

  return (
    <div>
      <NotificationBannerContainer
        notifications={notifications}
        onDismiss={dismissNotification}
      />
      
      <main>
        {/* Your app content */}
      </main>
    </div>
  );
}
```

### Manual Refresh

```tsx
import { useNotificationBanner, NotificationBannerContainer } from './sdks/react';

function App() {
  const {
    notifications,
    dismissNotification,
    refresh,
    clearDismissed
  } = useNotificationBanner({
    apiUrl: 'http://localhost:5000',
    tenantId: 1,
    enabled: false // Disable automatic polling
  });

  return (
    <div>
      <div style={{ padding: '10px', background: '#f0f0f0' }}>
        <button onClick={refresh}>Refresh Notifications</button>
        <button onClick={clearDismissed}>Clear Dismissed</button>
      </div>
      
      <NotificationBannerContainer
        notifications={notifications}
        onDismiss={dismissNotification}
      />
      
      {/* Your app content */}
    </div>
  );
}
```

### Custom Banner Component

```tsx
import { useNotificationBanner } from './sdks/react';
import type { Notification } from './sdks/react/useNotificationBanner';

function CustomNotificationBanner({ 
  notification, 
  onDismiss 
}: { 
  notification: Notification; 
  onDismiss: () => void;
}) {
  return (
    <div className={`custom-banner ${notification.type.toLowerCase()}`}>
      <div className="banner-icon">
        {notification.type === 'Error' && '⚠️'}
        {notification.type === 'Success' && '✓'}
        {notification.type === 'Warning' && '⚡'}
        {notification.type === 'Info' && 'ℹ️'}
      </div>
      <div className="banner-content">
        <h4>{notification.title}</h4>
        <p>{notification.message}</p>
      </div>
      <button onClick={onDismiss} className="banner-close">
        ×
      </button>
    </div>
  );
}

function App() {
  const { notifications, dismissNotification } = useNotificationBanner({
    apiUrl: 'http://localhost:5000',
    tenantId: 1
  });

  return (
    <div>
      <div className="custom-notification-container">
        {notifications.map(notification => (
          <CustomNotificationBanner
            key={notification.id}
            notification={notification}
            onDismiss={() => dismissNotification(notification.id)}
          />
        ))}
      </div>
      
      {/* Your app content */}
    </div>
  );
}
```

### With React Router

```tsx
import { useParams } from 'react-router-dom';
import { useNotificationBanner, NotificationBannerContainer } from './sdks/react';

function TenantDashboard() {
  const { tenantId } = useParams<{ tenantId: string }>();
  
  const { notifications, dismissNotification } = useNotificationBanner({
    apiUrl: 'http://localhost:5000',
    tenantId: parseInt(tenantId || '1', 10),
    enabled: !!tenantId
  });

  return (
    <div>
      <NotificationBannerContainer
        notifications={notifications}
        onDismiss={dismissNotification}
      />
      
      <h1>Tenant Dashboard</h1>
      {/* Dashboard content */}
    </div>
  );
}
```

### With Context Provider

```tsx
// NotificationContext.tsx
import { createContext, useContext, ReactNode } from 'react';
import { useNotificationBanner } from './sdks/react';
import type { Notification } from './sdks/react/useNotificationBanner';

interface NotificationContextType {
  notifications: Notification[];
  dismissNotification: (id: number) => void;
  refresh: () => void;
}

const NotificationContext = createContext<NotificationContextType | null>(null);

export function NotificationProvider({ 
  children,
  tenantId 
}: { 
  children: ReactNode;
  tenantId: number;
}) {
  const notificationData = useNotificationBanner({
    apiUrl: 'http://localhost:5000',
    tenantId
  });

  return (
    <NotificationContext.Provider value={notificationData}>
      {children}
    </NotificationContext.Provider>
  );
}

export function useNotifications() {
  const context = useContext(NotificationContext);
  if (!context) {
    throw new Error('useNotifications must be used within NotificationProvider');
  }
  return context;
}

// App.tsx
import { NotificationProvider, useNotifications } from './NotificationContext';
import { NotificationBannerContainer } from './sdks/react';

function AppContent() {
  const { notifications, dismissNotification } = useNotifications();
  
  return (
    <div>
      <NotificationBannerContainer
        notifications={notifications}
        onDismiss={dismissNotification}
      />
      
      {/* Your app content */}
    </div>
  );
}

function App() {
  return (
    <NotificationProvider tenantId={1}>
      <AppContent />
    </NotificationProvider>
  );
}
```

## Integration Patterns

### Pattern 1: Always On

Notifications are always active and polling in the background.

```javascript
// Best for: Internal tools, dashboards, admin panels
const sdk = new NotificationBannerSDK({
    apiUrl: 'http://localhost:5000',
    tenantId: 1,
    pollInterval: 60000 // 1 minute
});

sdk.start();
```

### Pattern 2: Session-Based

Start notifications after user authentication.

```javascript
// Best for: Multi-tenant SaaS applications
function onUserLogin(tenantId) {
    const sdk = new NotificationBannerSDK({
        apiUrl: 'http://localhost:5000',
        tenantId: tenantId,
        pollInterval: 30000
    });
    
    sdk.start();
    
    // Store reference for logout
    window.notificationSDK = sdk;
}

function onUserLogout() {
    if (window.notificationSDK) {
        window.notificationSDK.stop();
        window.notificationSDK = null;
    }
}
```

### Pattern 3: On-Demand

Load notifications only when requested by user.

```javascript
// Best for: High-performance applications, mobile web apps
const sdk = new NotificationBannerSDK({
    apiUrl: 'http://localhost:5000',
    tenantId: 1
});

// Don't auto-start
document.getElementById('check-notifications-btn').addEventListener('click', async () => {
    await sdk.updateNotifications();
});
```

### Pattern 4: Environment-Specific

Different configurations for different environments.

```javascript
const config = {
    development: {
        apiUrl: 'http://localhost:5000',
        pollInterval: 10000 // More frequent in dev
    },
    production: {
        apiUrl: 'https://notifications.mycompany.com',
        pollInterval: 60000
    }
};

const env = process.env.NODE_ENV || 'development';

const sdk = new NotificationBannerSDK({
    ...config[env],
    tenantId: 1
});

sdk.start();
```

## Best Practices

1. **Error Handling**: Always implement error callbacks to handle API failures gracefully
2. **Poll Intervals**: Choose appropriate intervals (30-60 seconds recommended)
3. **Cleanup**: Stop polling when component unmounts or user logs out
4. **Theming**: Match notification styles to your application's design system
5. **Testing**: Use `clearDismissed()` for testing without persistent state
6. **Performance**: Consider disabling auto-polling on low-bandwidth connections
7. **Accessibility**: Ensure banners are keyboard-navigable and screen-reader friendly

## Troubleshooting

### Notifications Not Appearing

1. Check API URL is correct
2. Verify tenant ID exists
3. Check browser console for errors
4. Ensure notifications are marked as `isActive: true`
5. Verify date ranges (startDate/endDate)

### CORS Issues

Add appropriate CORS configuration in the API:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://yourfrontend.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

### TypeScript Errors

Ensure you have the correct type definitions imported:
```typescript
import type { Notification, NotificationTheme } from './sdks/react/useNotificationBanner';
```

## Additional Resources

- [API Documentation](API_EXAMPLES.md)
- [Vanilla JS SDK Documentation](sdks/vanilla-js/README.md)
- [React SDK Documentation](sdks/react/README.md)
- [Architecture Overview](ARCHITECTURE.md)
