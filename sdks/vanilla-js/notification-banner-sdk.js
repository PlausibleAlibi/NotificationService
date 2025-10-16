/**
 * Vanilla JavaScript SDK for Enterprise Notification Service
 * Provides an easy way to display notification banners in any web application
 * 
 * @version 1.0.0
 * @author Enterprise Notification Service Team
 */

(function(window) {
  'use strict';

  /**
   * NotificationBannerSDK Class
   * Manages fetching and displaying notification banners
   */
  class NotificationBannerSDK {
    /**
     * Initialize the SDK
     * @param {Object} config - Configuration options
     * @param {string} config.apiUrl - Base URL of the notification service API
     * @param {number} config.tenantId - Tenant ID to fetch notifications for
     * @param {number} config.pollInterval - How often to check for new notifications (ms), default 60000
     * @param {string} config.containerId - ID of the container element, default 'notification-banners'
     * @param {Object} config.theme - Theme configuration for banners
     */
    constructor(config) {
      this.apiUrl = config.apiUrl || 'http://localhost:5000';
      this.tenantId = config.tenantId;
      this.pollInterval = config.pollInterval || 60000;
      this.containerId = config.containerId || 'notification-banners';
      this.theme = config.theme || {};
      this.notifications = [];
      this.pollTimer = null;
      
      if (!this.tenantId) {
        throw new Error('tenantId is required');
      }

      this.initializeContainer();
    }

    /**
     * Initialize the notification container
     * @private
     */
    initializeContainer() {
      let container = document.getElementById(this.containerId);
      if (!container) {
        container = document.createElement('div');
        container.id = this.containerId;
        container.style.cssText = `
          position: fixed;
          top: 0;
          left: 0;
          right: 0;
          z-index: 9999;
          padding: 10px;
        `;
        document.body.insertBefore(container, document.body.firstChild);
      }
    }

    /**
     * Fetch active notifications from the API
     * @returns {Promise<Array>} Array of notification objects
     */
    async fetchNotifications() {
      try {
        const response = await fetch(
          `${this.apiUrl}/api/notifications/tenant/${this.tenantId}/active`
        );
        
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return await response.json();
      } catch (error) {
        console.error('Failed to fetch notifications:', error);
        return [];
      }
    }

    /**
     * Get banner color based on notification type
     * @param {string} type - Notification type (Info, Warning, Error, Success)
     * @returns {Object} Color configuration
     * @private
     */
    getTypeColors(type) {
      const defaultColors = {
        Info: { bg: '#2196F3', text: '#FFFFFF' },
        Warning: { bg: '#FF9800', text: '#000000' },
        Error: { bg: '#F44336', text: '#FFFFFF' },
        Success: { bg: '#4CAF50', text: '#FFFFFF' }
      };
      
      return this.theme[type] || defaultColors[type] || defaultColors.Info;
    }

    /**
     * Create a banner element for a notification
     * @param {Object} notification - Notification object
     * @returns {HTMLElement} Banner element
     * @private
     */
    createBanner(notification) {
      const colors = this.getTypeColors(notification.type);
      const banner = document.createElement('div');
      banner.id = `notification-${notification.id}`;
      banner.style.cssText = `
        background-color: ${colors.bg};
        color: ${colors.text};
        padding: 15px 20px;
        margin-bottom: 10px;
        border-radius: 4px;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
        display: flex;
        justify-content: space-between;
        align-items: center;
        animation: slideDown 0.3s ease-out;
      `;

      const content = document.createElement('div');
      content.style.flex = '1';
      
      const title = document.createElement('strong');
      title.textContent = notification.title;
      title.style.cssText = `
        display: block;
        font-size: 16px;
        margin-bottom: 5px;
      `;
      
      const message = document.createElement('div');
      message.textContent = notification.message;
      message.style.fontSize = '14px';
      
      content.appendChild(title);
      content.appendChild(message);
      
      const closeButton = document.createElement('button');
      closeButton.textContent = '×';
      closeButton.style.cssText = `
        background: none;
        border: none;
        color: ${colors.text};
        font-size: 24px;
        cursor: pointer;
        padding: 0;
        margin-left: 20px;
        line-height: 1;
      `;
      closeButton.addEventListener('click', () => {
        this.dismissBanner(notification.id);
      });
      
      banner.appendChild(content);
      banner.appendChild(closeButton);
      
      return banner;
    }

    /**
     * Render banners in the container
     * @private
     */
    renderBanners() {
      const container = document.getElementById(this.containerId);
      container.innerHTML = '';
      
      this.notifications.forEach(notification => {
        const banner = this.createBanner(notification);
        container.appendChild(banner);
      });
    }

    /**
     * Dismiss a banner
     * @param {number} notificationId - ID of the notification to dismiss
     */
    dismissBanner(notificationId) {
      this.notifications = this.notifications.filter(n => n.id !== notificationId);
      this.renderBanners();
      
      // Store dismissed notification in localStorage to not show again in this session
      const dismissed = this.getDismissedNotifications();
      dismissed.push(notificationId);
      localStorage.setItem('dismissed-notifications', JSON.stringify(dismissed));
    }

    /**
     * Get list of dismissed notifications
     * @returns {Array<number>} Array of dismissed notification IDs
     * @private
     */
    getDismissedNotifications() {
      try {
        return JSON.parse(localStorage.getItem('dismissed-notifications') || '[]');
      } catch {
        return [];
      }
    }

    /**
     * Update notifications
     * @private
     */
    async updateNotifications() {
      const notifications = await this.fetchNotifications();
      const dismissed = this.getDismissedNotifications();
      
      // Filter out dismissed notifications
      this.notifications = notifications.filter(n => !dismissed.includes(n.id));
      this.renderBanners();
    }

    /**
     * Start polling for notifications
     */
    start() {
      // Initial fetch
      this.updateNotifications();
      
      // Set up polling
      this.pollTimer = setInterval(() => {
        this.updateNotifications();
      }, this.pollInterval);
    }

    /**
     * Stop polling for notifications
     */
    stop() {
      if (this.pollTimer) {
        clearInterval(this.pollTimer);
        this.pollTimer = null;
      }
    }

    /**
     * Clear all dismissed notifications (useful for testing)
     */
    clearDismissed() {
      localStorage.removeItem('dismissed-notifications');
      this.updateNotifications();
    }
  }

  // Add CSS animation
  if (!document.getElementById('notification-sdk-styles')) {
    const style = document.createElement('style');
    style.id = 'notification-sdk-styles';
    style.textContent = `
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
    `;
    document.head.appendChild(style);
  }

  // Expose SDK to global scope
  window.NotificationBannerSDK = NotificationBannerSDK;

})(window);
