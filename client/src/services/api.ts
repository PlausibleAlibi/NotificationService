import axios from 'axios';
import type { Notification, Tenant, CreateNotificationDto, LoginRequest, LoginResponse } from '../types';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add token to requests if available
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const authApi = {
  login: async (data: LoginRequest): Promise<LoginResponse> => {
    const response = await api.post<LoginResponse>('/api/auth/login', data);
    return response.data;
  },
};

export const tenantsApi = {
  getAll: async (): Promise<Tenant[]> => {
    const response = await api.get<Tenant[]>('/api/tenants');
    return response.data;
  },
  getById: async (id: number): Promise<Tenant> => {
    const response = await api.get<Tenant>(`/api/tenants/${id}`);
    return response.data;
  },
};

export const notificationsApi = {
  getByTenant: async (tenantId: number): Promise<Notification[]> => {
    const response = await api.get<Notification[]>(`/api/notifications/tenant/${tenantId}`);
    return response.data;
  },
  getActiveByTenant: async (tenantId: number): Promise<Notification[]> => {
    const response = await api.get<Notification[]>(`/api/notifications/tenant/${tenantId}/active`);
    return response.data;
  },
  create: async (data: CreateNotificationDto): Promise<Notification> => {
    const response = await api.post<Notification>('/api/notifications', data);
    return response.data;
  },
  update: async (id: number, data: Partial<CreateNotificationDto>): Promise<void> => {
    await api.put(`/api/notifications/${id}`, data);
  },
  delete: async (id: number): Promise<void> => {
    await api.delete(`/api/notifications/${id}`);
  },
};
