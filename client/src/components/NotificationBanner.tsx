import { useState } from 'react';
import type { Notification } from '../types';

interface NotificationBannerProps {
  notification: Notification;
  onDismiss?: (id: number) => void;
}

/**
 * NotificationBanner component displays alerts/notifications as UI banners
 */
export default function NotificationBanner({ notification, onDismiss }: NotificationBannerProps) {
  const [isVisible, setIsVisible] = useState(true);

  if (!isVisible) return null;

  const handleDismiss = () => {
    setIsVisible(false);
    onDismiss?.(notification.id);
  };

  const getTypeStyle = () => {
    switch (notification.type) {
      case 'Error':
        return {
          backgroundColor: '#fee',
          borderColor: '#c33',
          color: '#800',
        };
      case 'Warning':
        return {
          backgroundColor: '#fff3cd',
          borderColor: '#ffc107',
          color: '#856404',
        };
      case 'Success':
        return {
          backgroundColor: '#d4edda',
          borderColor: '#28a745',
          color: '#155724',
        };
      case 'Info':
      default:
        return {
          backgroundColor: '#d1ecf1',
          borderColor: '#17a2b8',
          color: '#0c5460',
        };
    }
  };

  const style = getTypeStyle();

  return (
    <div
      style={{
        ...style,
        border: `2px solid ${style.borderColor}`,
        borderRadius: '4px',
        padding: '12px 16px',
        margin: '8px 0',
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
      }}
    >
      <div style={{ flex: 1 }}>
        <strong>{notification.title}</strong>
        <div style={{ marginTop: '4px' }}>{notification.message}</div>
        <small style={{ opacity: 0.8, marginTop: '4px', display: 'block' }}>
          {notification.type} • {new Date(notification.createdAt).toLocaleString()}
        </small>
      </div>
      <button
        onClick={handleDismiss}
        style={{
          background: 'none',
          border: 'none',
          fontSize: '20px',
          cursor: 'pointer',
          padding: '0 8px',
          color: 'inherit',
          opacity: 0.7,
        }}
        title="Dismiss"
      >
        ×
      </button>
    </div>
  );
}
