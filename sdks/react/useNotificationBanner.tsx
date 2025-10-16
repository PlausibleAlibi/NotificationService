import { useState, useEffect, useCallback } from 'react';

/**
 * Notification interface
 */
export interface Notification {
  id: number;
  tenantId: number;
  title: string;
  message: string;
  type: 'Info' | 'Warning' | 'Error' | 'Success';
  isActive: boolean;
  startDate?: string;
  endDate?: string;
  createdAt: string;
  updatedAt?: string;
  createdBy: string;
}

/**
 * Theme configuration for notification types
 */
export interface NotificationTheme {
  Info?: { bg: string; text: string };
  Warning?: { bg: string; text: string };
  Error?: { bg: string; text: string };
  Success?: { bg: string; text: string };
}

/**
 * Hook configuration
 */
export interface UseNotificationBannerConfig {
  apiUrl: string;
  tenantId: number;
  pollInterval?: number;
  enabled?: boolean;
  onError?: (error: Error) => void;
}

/**
 * React Hook for Enterprise Notification Service
 * 
 * @param config Configuration options
 * @returns Object with notifications and control functions
 * 
 * @example
 * ```tsx
 * function App() {
 *   const { notifications, dismissNotification } = useNotificationBanner({
 *     apiUrl: 'http://localhost:5000',
 *     tenantId: 1,
 *     pollInterval: 30000
 *   });
 *   
 *   return (
 *     <div>
 *       {notifications.map(notification => (
 *         <NotificationBanner
 *           key={notification.id}
 *           notification={notification}
 *           onDismiss={() => dismissNotification(notification.id)}
 *         />
 *       ))}
 *     </div>
 *   );
 * }
 * ```
 */
export function useNotificationBanner(config: UseNotificationBannerConfig) {
  const {
    apiUrl,
    tenantId,
    pollInterval = 60000,
    enabled = true,
    onError
  } = config;

  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);

  /**
   * Fetch active notifications from the API
   */
  const fetchNotifications = useCallback(async () => {
    setLoading(true);
    setError(null);

    try {
      const response = await fetch(
        `${apiUrl}/api/notifications/tenant/${tenantId}/active`
      );

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const data = await response.json();
      
      // Filter out dismissed notifications
      const dismissed = getDismissedNotifications();
      const activeNotifications = data.filter(
        (n: Notification) => !dismissed.includes(n.id)
      );
      
      setNotifications(activeNotifications);
    } catch (err) {
      const error = err instanceof Error ? err : new Error('Failed to fetch notifications');
      setError(error);
      if (onError) {
        onError(error);
      }
    } finally {
      setLoading(false);
    }
  }, [apiUrl, tenantId, onError]);

  /**
   * Get dismissed notifications from localStorage
   */
  const getDismissedNotifications = (): number[] => {
    try {
      const dismissed = localStorage.getItem('dismissed-notifications');
      return dismissed ? JSON.parse(dismissed) : [];
    } catch {
      return [];
    }
  };

  /**
   * Store dismissed notification ID
   */
  const storeDismissedNotification = (notificationId: number) => {
    const dismissed = getDismissedNotifications();
    if (!dismissed.includes(notificationId)) {
      dismissed.push(notificationId);
      localStorage.setItem('dismissed-notifications', JSON.stringify(dismissed));
    }
  };

  /**
   * Dismiss a notification
   */
  const dismissNotification = useCallback((notificationId: number) => {
    storeDismissedNotification(notificationId);
    setNotifications(prev => prev.filter(n => n.id !== notificationId));
  }, []);

  /**
   * Clear all dismissed notifications
   */
  const clearDismissed = useCallback(() => {
    localStorage.removeItem('dismissed-notifications');
    fetchNotifications();
  }, [fetchNotifications]);

  /**
   * Manually refresh notifications
   */
  const refresh = useCallback(() => {
    fetchNotifications();
  }, [fetchNotifications]);

  // Initial fetch and polling
  useEffect(() => {
    if (!enabled) {
      return;
    }

    fetchNotifications();

    if (pollInterval > 0) {
      const interval = setInterval(fetchNotifications, pollInterval);
      return () => clearInterval(interval);
    }
  }, [enabled, pollInterval, fetchNotifications]);

  return {
    notifications,
    loading,
    error,
    dismissNotification,
    clearDismissed,
    refresh
  };
}

/**
 * Get default theme colors for notification types
 */
export const getDefaultTheme = (): Required<NotificationTheme> => ({
  Info: { bg: '#2196F3', text: '#FFFFFF' },
  Warning: { bg: '#FF9800', text: '#000000' },
  Error: { bg: '#F44336', text: '#FFFFFF' },
  Success: { bg: '#4CAF50', text: '#FFFFFF' }
});

/**
 * Get colors for a notification type
 */
export const getTypeColors = (
  type: Notification['type'],
  theme?: NotificationTheme
): { bg: string; text: string } => {
  const defaultTheme = getDefaultTheme();
  return (theme && theme[type]) || defaultTheme[type];
};
