import { api } from './api';

// Authentication APIs
export const authAPI = {
  login: (credentials) => api.post('/api/auth/login', credentials),
  register: (userData) => api.post('/api/auth/register', userData),
  refreshToken: (refreshToken) => api.post('/api/auth/refresh', { refreshToken }),
  logout: () => api.post('/api/auth/logout'),
  getProfile: () => api.get('/api/auth/profile'),
  
  // Demo endpoints
  demo401: () => api.get('/api/auth/demo/401'),
  demo403: () => api.get('/api/auth/demo/403'),
};

// Vehicle APIs
export const vehicleAPI = {
  getAll: () => api.get('/api/vehicles'),
  getById: (id) => api.get(`/api/vehicles/${id}`),
  create: (data) => api.post('/api/vehicles', data),
  update: (id, data) => api.put(`/api/vehicles/${id}`, data),
  delete: (id) => api.delete(`/api/vehicles/${id}`),
  getParts: (vehicleId) => api.get(`/api/vehicles/${vehicleId}/parts`),
  addPart: (vehicleId, partData) => api.post(`/api/vehicles/${vehicleId}/parts`, partData),
  updatePart: (partId, partData) => api.put(`/api/vehicles/parts/${partId}`, partData),
  removePart: (partId) => api.delete(`/api/vehicles/parts/${partId}`),
};

// Customer APIs
export const customerAPI = {
  getAll: (params) => api.get('/api/customers', { params }),
  getById: (id) => api.get(`/api/customers/${id}`),
  create: (data) => api.post('/api/customers', data),
  update: (id, data) => api.put(`/api/customers/${id}`, data),
  delete: (id) => api.delete(`/api/customers/${id}`),
};

// Parts APIs
export const partAPI = {
  getAll: () => api.get('/api/parts'),
  getById: (id) => api.get(`/api/parts/${id}`),
  create: (data) => api.post('/api/parts', data),
  update: (id, data) => api.put(`/api/parts/${id}`, data),
  delete: (id) => api.delete(`/api/parts/${id}`),
};

// Service History APIs
export const serviceAPI = {
  getAll: () => api.get('/api/servicehistory'),
  getById: (id) => api.get(`/api/servicehistory/${id}`),
  create: (data) => api.post('/api/servicehistory', data),
  update: (id, data) => api.put(`/api/servicehistory/${id}`, data),
  delete: (id) => api.delete(`/api/servicehistory/${id}`),
};

// Warranty Claims APIs
export const warrantyAPI = {
  getAll: () => api.get('/api/warrantyclaims'),
  getById: (id) => api.get(`/api/warrantyclaims/${id}`),
  create: (data) => api.post('/api/warrantyclaims', data),
  update: (id, data) => api.put(`/api/warrantyclaims/${id}`, data),
  delete: (id) => api.delete(`/api/warrantyclaims/${id}`),
};

// Generic API helper for common operations
export const createAPIService = (endpoint) => ({
  getAll: (params) => api.get(endpoint, { params }),
  getById: (id) => api.get(`${endpoint}/${id}`),
  create: (data) => api.post(endpoint, data),
  update: (id, data) => api.put(`${endpoint}/${id}`, data),
  delete: (id) => api.delete(`${endpoint}/${id}`),
});
