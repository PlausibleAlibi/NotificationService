# Vanilla JavaScript SDK - Enterprise Notification Service

A lightweight, framework-agnostic JavaScript SDK for displaying notification banners in any web application.

## Features

- 🚀 Zero dependencies
- 📦 Small footprint (~8KB unminified)
- 🎨 Customizable themes
- 🔄 Automatic polling for new notifications
- 💾 Local storage for dismissed notifications
- 🎯 Easy integration

## Installation

### Via CDN

```html
<script src="path/to/notification-banner-sdk.js"></script>
```

### Via NPM (if packaged)

```bash
npm install @ens/notification-banner-sdk
```

## Quick Start

### Basic Usage

```html
<!DOCTYPE html>
<html>
<head>
    <title>My App</title>
</head>
<body>
    <!-- Your app content -->
    
    <script src="notification-banner-sdk.js"></script>
    <script>
        // Initialize the SDK
        const notificationSDK = new NotificationBannerSDK({
            apiUrl: 'http://localhost:5000',
            tenantId: 1,
            pollInterval: 30000 // Check every 30 seconds
        });
        
        // Start displaying notifications
        notificationSDK.start();
    </script>
</body>
</html>
```

## Configuration Options

```javascript
const sdk = new NotificationBannerSDK({
    // Required
    tenantId: 1,                          // Your tenant ID
    
    // Optional
    apiUrl: 'http://localhost:5000',      // API base URL (default: http://localhost:5000)
    pollInterval: 60000,                   // Polling interval in ms (default: 60000)
    containerId: 'notification-banners',   // Container element ID (default: notification-banners)
    
    // Theme customization
    theme: {
        Info: { bg: '#2196F3', text: '#FFFFFF' },
        Warning: { bg: '#FF9800', text: '#000000' },
        Error: { bg: '#F44336', text: '#FFFFFF' },
        Success: { bg: '#4CAF50', text: '#FFFFFF' }
    }
});
```

## API Methods

### `start()`
Start polling for notifications and display them.

```javascript
sdk.start();
```

### `stop()`
Stop polling for notifications.

```javascript
sdk.stop();
```

### `dismissBanner(notificationId)`
Manually dismiss a specific notification.

```javascript
sdk.dismissBanner(123);
```

### `clearDismissed()`
Clear all dismissed notifications (useful for testing).

```javascript
sdk.clearDismissed();
```

## Customization

### Custom Container

By default, the SDK creates a fixed-position container at the top of the page. You can provide your own container:

```html
<div id="my-notifications"></div>

<script>
const sdk = new NotificationBannerSDK({
    tenantId: 1,
    containerId: 'my-notifications'
});
sdk.start();
</script>
```

### Custom Theme

Customize the appearance of notification types:

```javascript
const sdk = new NotificationBannerSDK({
    tenantId: 1,
    theme: {
        Info: { bg: '#007bff', text: '#ffffff' },
        Warning: { bg: '#ffc107', text: '#000000' },
        Error: { bg: '#dc3545', text: '#ffffff' },
        Success: { bg: '#28a745', text: '#ffffff' }
    }
});
```

## Examples

### Single Page Application

```javascript
// Initialize once when app loads
const sdk = new NotificationBannerSDK({
    apiUrl: 'https://api.example.com',
    tenantId: 1,
    pollInterval: 30000
});

// Start when user logs in
function onUserLogin() {
    sdk.start();
}

// Stop when user logs out
function onUserLogout() {
    sdk.stop();
}
```

### React Integration (without React SDK)

```jsx
import { useEffect, useRef } from 'react';

function App() {
    const sdkRef = useRef(null);
    
    useEffect(() => {
        sdkRef.current = new NotificationBannerSDK({
            tenantId: 1,
            apiUrl: 'http://localhost:5000'
        });
        
        sdkRef.current.start();
        
        return () => {
            sdkRef.current.stop();
        };
    }, []);
    
    return <div>Your app content</div>;
}
```

### Multiple Tenants

```javascript
// Create separate instances for different tenants
const tenant1SDK = new NotificationBannerSDK({
    tenantId: 1,
    containerId: 'notifications-tenant-1'
});

const tenant2SDK = new NotificationBannerSDK({
    tenantId: 2,
    containerId: 'notifications-tenant-2'
});

tenant1SDK.start();
tenant2SDK.start();
```

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)
- IE 11+ (with polyfills for fetch and Promise)

## License

MIT License

## Support

For issues or questions:
- GitHub Issues: https://github.com/PlausibleAlibi/NotificationService/issues
- Documentation: See main README.md
