# React SDK - Enterprise Notification Service

A React SDK for displaying notification banners with hooks and components.

## Features

- ⚛️ Built for React 18+
- 🪝 Custom React hooks
- 🎨 Customizable themes
- 🔄 Automatic polling
- 💾 Local storage for dismissed notifications
- 📦 TypeScript support
- 🎯 Easy integration

## Installation

```bash
npm install @ens/notification-banner-react
```

Or copy the source files directly into your project.

## Quick Start

### Basic Usage

```tsx
import { useNotificationBanner, NotificationBannerContainer } from './sdks/react';

function App() {
  const { notifications, dismissNotification } = useNotificationBanner({
    apiUrl: 'http://localhost:5000',
    tenantId: 1,
    pollInterval: 30000 // Check every 30 seconds
  });

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
```

## API Reference

### `useNotificationBanner(config)`

React hook for managing notifications.

#### Config Options

```typescript
interface UseNotificationBannerConfig {
  apiUrl: string;           // API base URL (required)
  tenantId: number;         // Tenant ID (required)
  pollInterval?: number;    // Polling interval in ms (default: 60000)
  enabled?: boolean;        // Enable/disable polling (default: true)
  onError?: (error: Error) => void;  // Error callback
}
```

#### Returns

```typescript
{
  notifications: Notification[];     // Array of active notifications
  loading: boolean;                  // Loading state
  error: Error | null;               // Error state
  dismissNotification: (id: number) => void;  // Dismiss function
  clearDismissed: () => void;        // Clear all dismissed
  refresh: () => void;               // Manual refresh
}
```

### `NotificationBanner`

Component for displaying a single notification.

#### Props

```typescript
interface NotificationBannerProps {
  notification: Notification;      // Notification object (required)
  onDismiss?: () => void;          // Dismiss callback
  theme?: NotificationTheme;       // Custom theme
  style?: React.CSSProperties;     // Custom styles
}
```

### `NotificationBannerContainer`

Container component for displaying multiple notifications.

#### Props

```typescript
interface NotificationBannerContainerProps {
  notifications: Notification[];   // Array of notifications (required)
  onDismiss?: (id: number) => void; // Dismiss callback
  theme?: NotificationTheme;       // Custom theme
  position?: 'top' | 'bottom';     // Position (default: 'top')
  containerStyle?: React.CSSProperties; // Custom container styles
}
```

## Examples

### Custom Theme

```tsx
const theme = {
  Info: { bg: '#007bff', text: '#ffffff' },
  Warning: { bg: '#ffc107', text: '#000000' },
  Error: { bg: '#dc3545', text: '#ffffff' },
  Success: { bg: '#28a745', text: '#ffffff' }
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
      theme={theme}
    />
  );
}
```

### Manual Control

```tsx
function App() {
  const {
    notifications,
    dismissNotification,
    clearDismissed,
    refresh,
    loading,
    error
  } = useNotificationBanner({
    apiUrl: 'http://localhost:5000',
    tenantId: 1,
    enabled: false  // Disable automatic polling
  });

  return (
    <div>
      <button onClick={refresh}>Refresh Notifications</button>
      <button onClick={clearDismissed}>Clear Dismissed</button>
      
      {loading && <div>Loading...</div>}
      {error && <div>Error: {error.message}</div>}
      
      <NotificationBannerContainer
        notifications={notifications}
        onDismiss={dismissNotification}
      />
    </div>
  );
}
```

### Custom Banner Component

```tsx
function CustomBanner({ notification, onDismiss }: NotificationBannerProps) {
  return (
    <div className="my-custom-banner">
      <h3>{notification.title}</h3>
      <p>{notification.message}</p>
      {onDismiss && <button onClick={onDismiss}>Close</button>}
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
      {notifications.map(notification => (
        <CustomBanner
          key={notification.id}
          notification={notification}
          onDismiss={() => dismissNotification(notification.id)}
        />
      ))}
    </div>
  );
}
```

### Error Handling

```tsx
function App() {
  const handleError = (error: Error) => {
    console.error('Notification error:', error);
    // Send to error tracking service
  };

  const { notifications, dismissNotification } = useNotificationBanner({
    apiUrl: 'http://localhost:5000',
    tenantId: 1,
    onError: handleError
  });

  return (
    <NotificationBannerContainer
      notifications={notifications}
      onDismiss={dismissNotification}
    />
  );
}
```

### With React Router

```tsx
import { useParams } from 'react-router-dom';

function TenantApp() {
  const { tenantId } = useParams();
  
  const { notifications, dismissNotification } = useNotificationBanner({
    apiUrl: 'http://localhost:5000',
    tenantId: parseInt(tenantId || '1'),
    enabled: !!tenantId
  });

  return (
    <div>
      <NotificationBannerContainer
        notifications={notifications}
        onDismiss={dismissNotification}
      />
      {/* Rest of app */}
    </div>
  );
}
```

### Position at Bottom

```tsx
function App() {
  const { notifications, dismissNotification } = useNotificationBanner({
    apiUrl: 'http://localhost:5000',
    tenantId: 1
  });

  return (
    <NotificationBannerContainer
      notifications={notifications}
      onDismiss={dismissNotification}
      position="bottom"
    />
  );
}
```

## TypeScript Support

All components and hooks are fully typed. Import types as needed:

```typescript
import type {
  Notification,
  NotificationTheme,
  UseNotificationBannerConfig,
  NotificationBannerProps,
  NotificationBannerContainerProps
} from './sdks/react';
```

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

## License

MIT License

## Support

For issues or questions:
- GitHub Issues: https://github.com/PlausibleAlibi/NotificationService/issues
- Documentation: See main README.md
