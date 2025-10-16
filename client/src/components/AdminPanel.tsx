import { useState, useEffect } from 'react';
import { notificationsApi, tenantsApi } from '../services/api';
import type { Notification, Tenant, CreateNotificationDto } from '../types';
import NotificationBanner from './NotificationBanner';

interface AdminPanelProps {
  username: string;
  onLogout: () => void;
}

/**
 * Admin panel for managing notifications
 */
export default function AdminPanel({ username, onLogout }: AdminPanelProps) {
  const [tenants, setTenants] = useState<Tenant[]>([]);
  const [selectedTenant, setSelectedTenant] = useState<number | null>(null);
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  const [formData, setFormData] = useState<CreateNotificationDto>({
    tenantId: 0,
    title: '',
    message: '',
    type: 'Info',
    isActive: true,
  });

  useEffect(() => {
    loadTenants();
  }, []);

  useEffect(() => {
    if (selectedTenant) {
      loadNotifications(selectedTenant);
    }
  }, [selectedTenant]);

  const loadTenants = async () => {
    try {
      const data = await tenantsApi.getAll();
      setTenants(data);
      if (data.length > 0) {
        setSelectedTenant(data[0].id);
        setFormData((prev) => ({ ...prev, tenantId: data[0].id }));
      }
    } catch (err) {
      setError('Failed to load tenants');
    }
  };

  const loadNotifications = async (tenantId: number) => {
    setLoading(true);
    try {
      const data = await notificationsApi.getByTenant(tenantId);
      setNotifications(data);
      setError('');
    } catch (err) {
      setError('Failed to load notifications');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      await notificationsApi.create(formData);
      setShowCreateForm(false);
      setFormData({
        tenantId: selectedTenant || 0,
        title: '',
        message: '',
        type: 'Info',
        isActive: true,
      });
      if (selectedTenant) {
        await loadNotifications(selectedTenant);
      }
    } catch (err) {
      setError('Failed to create notification');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Are you sure you want to delete this notification?')) return;

    try {
      await notificationsApi.delete(id);
      if (selectedTenant) {
        await loadNotifications(selectedTenant);
      }
    } catch (err) {
      setError('Failed to delete notification');
    }
  };

  const handleToggleActive = async (notification: Notification) => {
    try {
      await notificationsApi.update(notification.id, {
        isActive: !notification.isActive,
      });
      if (selectedTenant) {
        await loadNotifications(selectedTenant);
      }
    } catch (err) {
      setError('Failed to update notification');
    }
  };

  return (
    <div style={{ maxWidth: '1200px', margin: '0 auto', padding: '20px' }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '20px' }}>
        <h1>Enterprise Notification Service</h1>
        <div>
          <span style={{ marginRight: '15px' }}>Welcome, {username}</span>
          <button
            onClick={onLogout}
            style={{
              padding: '8px 16px',
              backgroundColor: '#6c757d',
              color: 'white',
              border: 'none',
              borderRadius: '4px',
              cursor: 'pointer',
            }}
          >
            Logout
          </button>
        </div>
      </div>

      {error && (
        <div
          style={{
            backgroundColor: '#fee',
            color: '#c33',
            padding: '12px',
            borderRadius: '4px',
            marginBottom: '20px',
          }}
        >
          {error}
        </div>
      )}

      <div style={{ marginBottom: '20px' }}>
        <label style={{ display: 'block', marginBottom: '8px', fontWeight: 'bold' }}>
          Select Tenant:
        </label>
        <select
          value={selectedTenant || ''}
          onChange={(e) => setSelectedTenant(Number(e.target.value))}
          style={{
            padding: '8px',
            fontSize: '16px',
            border: '1px solid #ccc',
            borderRadius: '4px',
            minWidth: '200px',
          }}
        >
          {tenants.map((tenant) => (
            <option key={tenant.id} value={tenant.id}>
              {tenant.name} ({tenant.code})
            </option>
          ))}
        </select>
      </div>

      <div style={{ marginBottom: '20px' }}>
        <button
          onClick={() => setShowCreateForm(!showCreateForm)}
          style={{
            padding: '10px 20px',
            backgroundColor: '#28a745',
            color: 'white',
            border: 'none',
            borderRadius: '4px',
            cursor: 'pointer',
            fontSize: '16px',
          }}
        >
          {showCreateForm ? 'Cancel' : '+ Create Notification'}
        </button>
      </div>

      {showCreateForm && (
        <form
          onSubmit={handleSubmit}
          style={{
            backgroundColor: '#f8f9fa',
            padding: '20px',
            borderRadius: '4px',
            marginBottom: '20px',
          }}
        >
          <h3>Create New Notification</h3>
          <div style={{ marginBottom: '15px' }}>
            <label style={{ display: 'block', marginBottom: '5px' }}>Title</label>
            <input
              type="text"
              value={formData.title}
              onChange={(e) => setFormData({ ...formData, title: e.target.value })}
              style={{
                width: '100%',
                padding: '8px',
                fontSize: '16px',
                border: '1px solid #ccc',
                borderRadius: '4px',
              }}
              required
            />
          </div>
          <div style={{ marginBottom: '15px' }}>
            <label style={{ display: 'block', marginBottom: '5px' }}>Message</label>
            <textarea
              value={formData.message}
              onChange={(e) => setFormData({ ...formData, message: e.target.value })}
              style={{
                width: '100%',
                padding: '8px',
                fontSize: '16px',
                border: '1px solid #ccc',
                borderRadius: '4px',
                minHeight: '100px',
              }}
              required
            />
          </div>
          <div style={{ marginBottom: '15px' }}>
            <label style={{ display: 'block', marginBottom: '5px' }}>Type</label>
            <select
              value={formData.type}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  type: e.target.value as CreateNotificationDto['type'],
                })
              }
              style={{
                padding: '8px',
                fontSize: '16px',
                border: '1px solid #ccc',
                borderRadius: '4px',
              }}
            >
              <option value="Info">Info</option>
              <option value="Warning">Warning</option>
              <option value="Error">Error</option>
              <option value="Success">Success</option>
            </select>
          </div>
          <div style={{ marginBottom: '15px' }}>
            <label style={{ display: 'flex', alignItems: 'center' }}>
              <input
                type="checkbox"
                checked={formData.isActive}
                onChange={(e) => setFormData({ ...formData, isActive: e.target.checked })}
                style={{ marginRight: '8px' }}
              />
              Active
            </label>
          </div>
          <button
            type="submit"
            disabled={loading}
            style={{
              padding: '10px 20px',
              backgroundColor: '#007bff',
              color: 'white',
              border: 'none',
              borderRadius: '4px',
              cursor: loading ? 'not-allowed' : 'pointer',
              fontSize: '16px',
              opacity: loading ? 0.6 : 1,
            }}
          >
            {loading ? 'Creating...' : 'Create Notification'}
          </button>
        </form>
      )}

      <h2>Notifications</h2>
      {loading && <p>Loading notifications...</p>}
      {!loading && notifications.length === 0 && <p>No notifications found.</p>}
      
      <div>
        {notifications.map((notification) => (
          <div
            key={notification.id}
            style={{
              marginBottom: '10px',
              opacity: notification.isActive ? 1 : 0.5,
            }}
          >
            <NotificationBanner notification={notification} />
            <div style={{ marginTop: '5px', display: 'flex', gap: '10px' }}>
              <button
                onClick={() => handleToggleActive(notification)}
                style={{
                  padding: '5px 10px',
                  backgroundColor: notification.isActive ? '#ffc107' : '#28a745',
                  color: 'white',
                  border: 'none',
                  borderRadius: '4px',
                  cursor: 'pointer',
                  fontSize: '14px',
                }}
              >
                {notification.isActive ? 'Deactivate' : 'Activate'}
              </button>
              <button
                onClick={() => handleDelete(notification.id)}
                style={{
                  padding: '5px 10px',
                  backgroundColor: '#dc3545',
                  color: 'white',
                  border: 'none',
                  borderRadius: '4px',
                  cursor: 'pointer',
                  fontSize: '14px',
                }}
              >
                Delete
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
