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

export interface Tenant {
  id: number;
  code: string;
  name: string;
  isActive: boolean;
  createdAt: string;
}

export interface CreateNotificationDto {
  tenantId: number;
  title: string;
  message: string;
  type: 'Info' | 'Warning' | 'Error' | 'Success';
  isActive: boolean;
  startDate?: string;
  endDate?: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  username: string;
  expiresAt: string;
}
