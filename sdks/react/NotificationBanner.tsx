import React from 'react';
import type { Notification, NotificationTheme } from './useNotificationBanner';
import { getTypeColors } from './useNotificationBanner';

/**
 * Props for NotificationBanner component
 */
export interface NotificationBannerProps {
  notification: Notification;
  onDismiss?: () => void;
  theme?: NotificationTheme;
  style?: React.CSSProperties;
}

/**
 * NotificationBanner Component
 * Displays a single notification banner with optional dismiss button
 * 
 * @example
 * ```tsx
 * <NotificationBanner
 *   notification={notification}
 *   onDismiss={() => handleDismiss(notification.id)}
 *   theme={{
 *     Info: { bg: '#007bff', text: '#ffffff' }
 *   }}
 * />
 * ```
 */
export const NotificationBanner: React.FC<NotificationBannerProps> = ({
  notification,
  onDismiss,
  theme,
  style
}) => {
  const colors = getTypeColors(notification.type, theme);

  const bannerStyle: React.CSSProperties = {
    backgroundColor: colors.bg,
    color: colors.text,
    padding: '15px 20px',
    marginBottom: '10px',
    borderRadius: '4px',
    boxShadow: '0 2px 8px rgba(0, 0, 0, 0.15)',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    animation: 'slideDown 0.3s ease-out',
    ...style
  };

  const contentStyle: React.CSSProperties = {
    flex: 1
  };

  const titleStyle: React.CSSProperties = {
    display: 'block',
    fontSize: '16px',
    fontWeight: 'bold',
    marginBottom: '5px'
  };

  const messageStyle: React.CSSProperties = {
    fontSize: '14px'
  };

  const closeButtonStyle: React.CSSProperties = {
    background: 'none',
    border: 'none',
    color: colors.text,
    fontSize: '24px',
    cursor: 'pointer',
    padding: 0,
    marginLeft: '20px',
    lineHeight: 1
  };

  return (
    <div style={bannerStyle}>
      <div style={contentStyle}>
        <strong style={titleStyle}>{notification.title}</strong>
        <div style={messageStyle}>{notification.message}</div>
      </div>
      {onDismiss && (
        <button
          style={closeButtonStyle}
          onClick={onDismiss}
          aria-label="Dismiss notification"
        >
          ×
        </button>
      )}
    </div>
  );
};

/**
 * Container for multiple notification banners
 */
export interface NotificationBannerContainerProps {
  notifications: Notification[];
  onDismiss?: (notificationId: number) => void;
  theme?: NotificationTheme;
  position?: 'top' | 'bottom';
  containerStyle?: React.CSSProperties;
}

/**
 * NotificationBannerContainer Component
 * Container for displaying multiple notification banners
 * 
 * @example
 * ```tsx
 * const { notifications, dismissNotification } = useNotificationBanner({
 *   apiUrl: 'http://localhost:5000',
 *   tenantId: 1
 * });
 * 
 * <NotificationBannerContainer
 *   notifications={notifications}
 *   onDismiss={dismissNotification}
 *   position="top"
 * />
 * ```
 */
export const NotificationBannerContainer: React.FC<NotificationBannerContainerProps> = ({
  notifications,
  onDismiss,
  theme,
  position = 'top',
  containerStyle
}) => {
  const defaultContainerStyle: React.CSSProperties = {
    position: 'fixed',
    [position]: 0,
    left: 0,
    right: 0,
    zIndex: 9999,
    padding: '10px',
    ...containerStyle
  };

  if (notifications.length === 0) {
    return null;
  }

  return (
    <div style={defaultContainerStyle}>
      {notifications.map(notification => (
        <NotificationBanner
          key={notification.id}
          notification={notification}
          onDismiss={onDismiss ? () => onDismiss(notification.id) : undefined}
          theme={theme}
        />
      ))}
      <style>
        {`
          @keyframes slideDown {
            from {
              transform: translateY(-100%);
              opacity: 0;
            }
            to {
              transform: translateY(0);
              opacity: 1;
            }
          }
        `}
      </style>
    </div>
  );
};
