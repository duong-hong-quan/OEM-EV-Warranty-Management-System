# Frontend - Electric Vehicle Warranty Management System
## React + Vite + ShadCN/UI + TailwindCSS

---

## 🚀 Project Initialization / Khởi tạo Dự án

### 1. Create Vite React Project / Tạo dự án React với Vite

```bash
# Create new Vite project with React template
npm create vite@latest fe -- --template react
cd fe
npm install
```

### 2. Setup TailwindCSS / Cài đặt TailwindCSS

```bash
# Install TailwindCSS and dependencies
npm install -D tailwindcss@latest postcss autoprefixer
npm install @tailwindcss/vite @tailwindcss/postcss

# Generate Tailwind config files
npx tailwindcss init -p
```

**tailwind.config.js configuration:**
```javascript
/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
```

### 3. Setup ShadCN/UI / Cài đặt ShadCN/UI

```bash
# Initialize shadcn/ui
npx shadcn@latest init

# When prompted, choose:
# - TypeScript: No (using JavaScript)
# - Style: Default
# - Base color: Slate
# - Global CSS file: src/index.css
# - CSS variables: Yes
# - Tailwind config: tailwind.config.js
# - Components: src/components
# - Utils: src/lib/utils
```

**components.json configuration:**
```json
{
  "$schema": "https://ui.shadcn.com/schema.json",
  "style": "default",
  "rsc": false,
  "tsx": false,
  "tailwind": {
    "config": "tailwind.config.js",
    "css": "src/App.css",
    "baseColor": "slate",
    "cssVariables": true,
    "prefix": ""
  },
  "aliases": {
    "components": "@/components",
    "utils": "@/lib/utils"
  }
}
```

### 4. Install Additional Dependencies / Cài đặt thư viện bổ sung

```bash
# Router for navigation
npm install react-router-dom

# HTTP client for API calls
npm install axios

# Form handling
npm install react-hook-form @hookform/resolvers zod

# Icons
npm install lucide-react

# Additional utilities
npm install clsx tailwind-merge class-variance-authority
```

---

## 📁 Project Structure / Cấu trúc Dự án

```
fe/
├── 📁 public/                          # Static assets
│   ├── 📄 vite.svg                     # Vite logo
│   └── 📄 favicon.ico                  # Favicon
│
├── 📁 src/                             # Source code
│   ├── 📁 components/                  # React components
│   │   ├── 📁 ui/                      # ShadCN/UI components
│   │   │   ├── 📄 button.jsx           # Button component
│   │   │   ├── 📄 card.jsx             # Card component
│   │   │   ├── 📄 input.jsx            # Input component
│   │   │   ├── 📄 label.jsx            # Label component
│   │   │   ├── 📄 form.jsx             # Form component
│   │   │   ├── 📄 table.jsx            # Table component
│   │   │   ├── 📄 alert.jsx            # Alert component
│   │   │   ├── 📄 badge.jsx            # Badge component
│   │   │   ├── 📄 scroll-area.jsx      # Scroll area component
│   │   │   └── 📄 textarea.jsx         # Textarea component
│   │   ├── 📄 AdminLayout.jsx          # Admin layout wrapper
│   │   └── 📄 StatCard.jsx             # Statistics card
│   │
│   ├── 📁 pages/                       # Page components
│   │   ├── 📄 Dashboard.jsx            # Dashboard page
│   │   ├── 📄 Login.jsx                # Login page
│   │   ├── 📄 VehicleRegistration.jsx  # Vehicle management
│   │   ├── 📄 PartAttachment.jsx       # Parts management
│   │   ├── 📄 ServiceHistory.jsx       # Service history
│   │   └── 📄 WarrantyClaim.jsx        # Warranty claims
│   │
│   ├── 📁 lib/                         # Utility libraries
│   │   ├── 📄 utils.js                 # General utilities
│   │   ├── 📄 api.js                   # Axios configuration
│   │   ├── 📄 auth.js                  # Authentication utilities
│   │   └── 📄 apiServices.js           # API service functions
│   │
│   ├── 📁 hooks/                       # Custom React hooks
│   │   ├── 📄 useAuth.js               # Authentication hook
│   │   ├── 📄 useApi.js                # API hooks
│   │   ├── 📄 useLocalStorage.js       # Local storage hook
│   │   └── 📄 useForm.js               # Form validation hook
│   │
│   ├── 📁 assets/                      # Static assets
│   │   └── 📄 react.svg                # React logo
│   │
│   ├── 📄 App.jsx                      # Main App component
│   ├── 📄 App.css                      # Global styles
│   └── 📄 main.jsx                     # Application entry point
│
├── 📁 node_modules/                    # Dependencies
├── 📄 package.json                     # NPM dependencies
├── 📄 package-lock.json               # NPM lock file
├── 📄 vite.config.js                  # Vite configuration
├── 📄 tailwind.config.js              # TailwindCSS config
├── 📄 postcss.config.js               # PostCSS configuration
├── 📄 eslint.config.js                # ESLint configuration
├── 📄 jsconfig.json                   # JavaScript config
├── 📄 components.json                 # ShadCN/UI config
├── 📄 index.html                      # HTML template
├── 📄 vercel.json                     # Vercel deployment config
├── 📄 .env.example                    # Environment variables example
├── 📄 .gitignore                      # Git ignore rules
└── 📄 README.md                       # This file
```

---

## 🔗 Axios Integration / Tích hợp Axios

### 1. API Configuration / Cấu hình API

**File: `src/lib/api.js`**
```javascript
import axios from "axios";
import { clearAuth } from "@/lib/auth";

// Base URL from environment variables
const baseURL = import.meta.env.VITE_API_BASE_URL || "http://localhost:5165";

// Create axios instance
export const api = axios.create({
  baseURL,
  withCredentials: false,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor - Add auth token
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("authToken");
  if (token) {
    config.headers = config.headers || {};
    config.headers["Authorization"] = `Bearer ${token}`;
  }
  return config;
});

// Response interceptor - Handle errors
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Handle 401 Unauthorized
    if (error?.response?.status === 401) {
      console.warn("API 401 Unauthorized:", error?.config?.url);
      try {
        clearAuth();
        window.dispatchEvent(new CustomEvent("app:unauthorized", { 
          detail: { message: "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại." } 
        }));
        if (window.location.pathname !== "/login") {
          window.location.assign("/login");
        }
      } catch {}
    }
    
    // Handle 403 Forbidden
    if (error?.response?.status === 403) {
      console.warn("API 403 Forbidden:", error?.config?.url);
      window.dispatchEvent(new CustomEvent("app:forbidden", { 
        detail: { message: "Bạn không có quyền truy cập tài nguyên này." } 
      }));
    }
    
    return Promise.reject(error);
  }
);

// Helper function to set auth token
export function setAuthToken(token) {
  if (token) {
    localStorage.setItem("authToken", token);
  } else {
    localStorage.removeItem("authToken");
  }
}

// Helper function to get auth token
export function getAuthToken() {
  return localStorage.getItem("authToken");
}
```

### 2. API Service Functions / Các hàm service API

**File: `src/lib/apiServices.js`**
```javascript
import { api } from './api';

// Authentication APIs
export const authAPI = {
  login: (credentials) => api.post('/api/auth/login', credentials),
  register: (userData) => api.post('/api/auth/register', userData),
  refreshToken: (refreshToken) => api.post('/api/auth/refresh', { refreshToken }),
  logout: () => api.post('/api/auth/logout'),
  getProfile: () => api.get('/api/auth/profile'),
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
```

---

## 🪝 Custom Hooks / Hooks tùy chỉnh

### 1. Authentication Hook

**File: `src/hooks/useAuth.js`**
```javascript
import { useState, useEffect, useContext, createContext } from 'react';
import { authAPI, setAuthToken, getAuthToken } from '@/lib/api';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const token = getAuthToken();
    if (token) {
      // Verify token with backend
      authAPI.getProfile()
        .then(response => {
          setUser(response.data);
          setIsAuthenticated(true);
        })
        .catch(() => {
          // Token invalid, clear it
          setAuthToken(null);
          setUser(null);
          setIsAuthenticated(false);
        })
        .finally(() => {
          setLoading(false);
        });
    } else {
      setLoading(false);
    }
  }, []);

  const login = async (credentials) => {
    try {
      const response = await authAPI.login(credentials);
      const { token, user } = response.data;
      
      setAuthToken(token);
      setUser(user);
      setIsAuthenticated(true);
      
      return { success: true, user };
    } catch (error) {
      return { 
        success: false, 
        error: error.response?.data?.message || 'Login failed' 
      };
    }
  };

  const logout = () => {
    setAuthToken(null);
    setUser(null);
    setIsAuthenticated(false);
    // Optionally call logout API
    authAPI.logout().catch(() => {});
  };

  const value = {
    user,
    loading,
    isAuthenticated,
    login,
    logout,
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
};
```

### 2. API Data Fetching Hook

**File: `src/hooks/useApi.js`**
```javascript
import { useState, useEffect } from 'react';

export const useApi = (apiFunction, dependencies = []) => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        setError(null);
        const response = await apiFunction();
        setData(response.data);
      } catch (err) {
        setError(err.response?.data?.message || err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, dependencies);

  const refetch = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await apiFunction();
      setData(response.data);
      return response.data;
    } catch (err) {
      setError(err.response?.data?.message || err.message);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { data, loading, error, refetch };
};

// Hook for CRUD operations
export const useCrud = (apiService) => {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const fetchItems = async () => {
    try {
      setLoading(true);
      const response = await apiService.getAll();
      setItems(response.data);
    } catch (err) {
      setError(err.response?.data?.message || err.message);
    } finally {
      setLoading(false);
    }
  };

  const createItem = async (data) => {
    try {
      const response = await apiService.create(data);
      setItems(prev => [...prev, response.data]);
      return response.data;
    } catch (err) {
      throw err;
    }
  };

  const updateItem = async (id, data) => {
    try {
      const response = await apiService.update(id, data);
      setItems(prev => prev.map(item => 
        item.id === id ? response.data : item
      ));
      return response.data;
    } catch (err) {
      throw err;
    }
  };

  const deleteItem = async (id) => {
    try {
      await apiService.delete(id);
      setItems(prev => prev.filter(item => item.id !== id));
    } catch (err) {
      throw err;
    }
  };

  useEffect(() => {
    fetchItems();
  }, []);

  return {
    items,
    loading,
    error,
    createItem,
    updateItem,
    deleteItem,
    refetch: fetchItems,
  };
};
```

### 3. Local Storage Hook

**File: `src/hooks/useLocalStorage.js`**
```javascript
import { useState, useEffect } from 'react';

export const useLocalStorage = (key, initialValue) => {
  // Get value from localStorage or use initial value
  const [storedValue, setStoredValue] = useState(() => {
    try {
      const item = window.localStorage.getItem(key);
      return item ? JSON.parse(item) : initialValue;
    } catch (error) {
      console.error(`Error reading localStorage key "${key}":`, error);
      return initialValue;
    }
  });

  // Return a wrapped version of useState's setter function that persists the new value to localStorage
  const setValue = (value) => {
    try {
      // Allow value to be a function so we have the same API as useState
      const valueToStore = value instanceof Function ? value(storedValue) : value;
      setStoredValue(valueToStore);
      window.localStorage.setItem(key, JSON.stringify(valueToStore));
    } catch (error) {
      console.error(`Error setting localStorage key "${key}":`, error);
    }
  };

  // Remove item from localStorage
  const removeValue = () => {
    try {
      window.localStorage.removeItem(key);
      setStoredValue(initialValue);
    } catch (error) {
      console.error(`Error removing localStorage key "${key}":`, error);
    }
  };

  return [storedValue, setValue, removeValue];
};

// Hook for managing form data in localStorage
export const usePersistedForm = (key, initialValues = {}) => {
  const [formData, setFormData, removeFormData] = useLocalStorage(key, initialValues);

  const updateField = (fieldName, value) => {
    setFormData(prev => ({
      ...prev,
      [fieldName]: value
    }));
  };

  const resetForm = () => {
    removeFormData();
  };

  return {
    formData,
    updateField,
    resetForm,
    setFormData,
  };
};
```

### 4. Form Validation Hook

**File: `src/hooks/useForm.js`**
```javascript
import { useState } from 'react';

export const useForm = (initialValues = {}, validationRules = {}) => {
  const [values, setValues] = useState(initialValues);
  const [errors, setErrors] = useState({});
  const [touched, setTouched] = useState({});

  const validate = (fieldName, value) => {
    const rules = validationRules[fieldName];
    if (!rules) return '';

    for (const rule of rules) {
      const error = rule(value);
      if (error) return error;
    }
    return '';
  };

  const handleChange = (fieldName, value) => {
    setValues(prev => ({
      ...prev,
      [fieldName]: value
    }));

    // Clear error when user starts typing
    if (errors[fieldName]) {
      setErrors(prev => ({
        ...prev,
        [fieldName]: ''
      }));
    }
  };

  const handleBlur = (fieldName) => {
    setTouched(prev => ({
      ...prev,
      [fieldName]: true
    }));

    const error = validate(fieldName, values[fieldName]);
    setErrors(prev => ({
      ...prev,
      [fieldName]: error
    }));
  };

  const validateAll = () => {
    const newErrors = {};
    let isValid = true;

    Object.keys(validationRules).forEach(fieldName => {
      const error = validate(fieldName, values[fieldName]);
      if (error) {
        newErrors[fieldName] = error;
        isValid = false;
      }
    });

    setErrors(newErrors);
    setTouched(Object.keys(validationRules).reduce((acc, key) => {
      acc[key] = true;
      return acc;
    }, {}));

    return isValid;
  };

  const reset = () => {
    setValues(initialValues);
    setErrors({});
    setTouched({});
  };

  return {
    values,
    errors,
    touched,
    handleChange,
    handleBlur,
    validateAll,
    reset,
    isValid: Object.keys(errors).length === 0,
  };
};

// Common validation rules
export const validationRules = {
  required: (value) => {
    if (!value || (typeof value === 'string' && value.trim() === '')) {
      return 'This field is required';
    }
    return '';
  },
  
  email: (value) => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (value && !emailRegex.test(value)) {
      return 'Please enter a valid email address';
    }
    return '';
  },
  
  minLength: (min) => (value) => {
    if (value && value.length < min) {
      return `Must be at least ${min} characters long`;
    }
    return '';
  },
  
  maxLength: (max) => (value) => {
    if (value && value.length > max) {
      return `Must be no more than ${max} characters long`;
    }
    return '';
  },
};
```

---

## 🔧 Development Scripts / Scripts phát triển

### package.json scripts:
```json
{
  "scripts": {
    "dev": "vite",
    "build": "vite build",
    "lint": "eslint .",
    "preview": "vite preview",
    "test": "vitest",
    "test:ui": "vitest --ui"
  }
}
```

### Development commands:
```bash
# Start development server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview

# Run linting
npm run lint

# Run tests
npm run test
```

---

## 🌐 Environment Configuration / Cấu hình môi trường

### Environment Variables:
```bash
# .env.local (for development)
VITE_API_BASE_URL=http://localhost:5165
VITE_APP_NAME="Electric Vehicle Warranty System"
VITE_APP_VERSION="1.0.0"

# .env.production (for production)
VITE_API_BASE_URL=https://your-api-domain.com
VITE_APP_NAME="Electric Vehicle Warranty System"
VITE_APP_VERSION="1.0.0"
```

---

## 📱 Component Usage Examples / Ví dụ sử dụng Component

### Example: Vehicle Registration Page với Hooks
```jsx
import React from 'react';
import { useForm, validationRules } from '@/hooks/useForm';
import { useCrud } from '@/hooks/useApi';
import { vehicleAPI } from '@/lib/apiServices';
import { Button } from '@/components/ui/button';
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Alert, AlertDescription } from '@/components/ui/alert';

export default function VehicleRegistration() {
  const { items: vehicles, createItem, loading } = useCrud(vehicleAPI);
  const { 
    values, 
    handleChange, 
    handleBlur, 
    errors, 
    validateAll,
    reset 
  } = useForm(
    { make: '', model: '', year: '', vin: '' },
    {
      make: [validationRules.required],
      model: [validationRules.required],
      year: [validationRules.required],
      vin: [validationRules.required, validationRules.minLength(17)],
    }
  );

  const [submitError, setSubmitError] = React.useState('');
  const [success, setSuccess] = React.useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSubmitError('');
    setSuccess(false);

    if (validateAll()) {
      try {
        await createItem(values);
        setSuccess(true);
        reset();
      } catch (error) {
        setSubmitError(error.response?.data?.message || 'Failed to create vehicle');
      }
    }
  };

  return (
    <div className="max-w-2xl mx-auto p-6">
      <Card>
        <CardHeader>
          <CardTitle>Vehicle Registration</CardTitle>
        </CardHeader>
        <CardContent>
          {submitError && (
            <Alert variant="destructive" className="mb-4">
              <AlertDescription>{submitError}</AlertDescription>
            </Alert>
          )}
          
          {success && (
            <Alert className="mb-4">
              <AlertDescription>Vehicle registered successfully!</AlertDescription>
            </Alert>
          )}

          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <div>
                <Label htmlFor="make">Make *</Label>
                <Input
                  id="make"
                  value={values.make}
                  onChange={(e) => handleChange('make', e.target.value)}
                  onBlur={() => handleBlur('make')}
                  className={errors.make ? 'border-red-500' : ''}
                  placeholder="Enter vehicle make"
                />
                {errors.make && <p className="text-red-500 text-sm mt-1">{errors.make}</p>}
              </div>

              <div>
                <Label htmlFor="model">Model *</Label>
                <Input
                  id="model"
                  value={values.model}
                  onChange={(e) => handleChange('model', e.target.value)}
                  onBlur={() => handleBlur('model')}
                  className={errors.model ? 'border-red-500' : ''}
                  placeholder="Enter vehicle model"
                />
                {errors.model && <p className="text-red-500 text-sm mt-1">{errors.model}</p>}
              </div>

              <div>
                <Label htmlFor="year">Year *</Label>
                <Input
                  id="year"
                  type="number"
                  value={values.year}
                  onChange={(e) => handleChange('year', e.target.value)}
                  onBlur={() => handleBlur('year')}
                  className={errors.year ? 'border-red-500' : ''}
                  placeholder="Enter year"
                />
                {errors.year && <p className="text-red-500 text-sm mt-1">{errors.year}</p>}
              </div>

              <div>
                <Label htmlFor="vin">VIN *</Label>
                <Input
                  id="vin"
                  value={values.vin}
                  onChange={(e) => handleChange('vin', e.target.value.toUpperCase())}
                  onBlur={() => handleBlur('vin')}
                  className={errors.vin ? 'border-red-500' : ''}
                  placeholder="Enter VIN (17 characters)"
                  maxLength={17}
                />
                {errors.vin && <p className="text-red-500 text-sm mt-1">{errors.vin}</p>}
              </div>
            </div>

            <Button type="submit" disabled={loading} className="w-full">
              {loading ? 'Registering...' : 'Register Vehicle'}
            </Button>
          </form>
        </CardContent>
      </Card>

      {/* Display existing vehicles */}
      <Card className="mt-6">
        <CardHeader>
          <CardTitle>Registered Vehicles ({vehicles.length})</CardTitle>
        </CardHeader>
        <CardContent>
          {vehicles.length === 0 ? (
            <p className="text-gray-500">No vehicles registered yet.</p>
          ) : (
            <div className="space-y-2">
              {vehicles.map((vehicle) => (
                <div key={vehicle.id} className="p-3 border rounded-lg">
                  <div className="flex justify-between items-center">
                    <span className="font-medium">
                      {vehicle.year} {vehicle.make} {vehicle.model}
                    </span>
                    <span className="text-sm text-gray-500">
                      VIN: {vehicle.vin}
                    </span>
                  </div>
                </div>
              ))}
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
```

### Example: Authentication với useAuth Hook
```jsx
import React from 'react';
import { useAuth } from '@/hooks/useAuth';
import { Navigate } from 'react-router-dom';

// Protected Route Component
export const ProtectedRoute = ({ children }) => {
  const { isAuthenticated, loading } = useAuth();

  if (loading) {
    return <div>Loading...</div>;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return children;
};

// Login Component
export const LoginForm = () => {
  const { login } = useAuth();
  const [credentials, setCredentials] = React.useState({
    email: '',
    password: ''
  });
  const [error, setError] = React.useState('');
  const [loading, setLoading] = React.useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    const result = await login(credentials);
    
    if (!result.success) {
      setError(result.error);
    }
    
    setLoading(false);
  };

  return (
    <form onSubmit={handleSubmit}>
      {error && <Alert variant="destructive">{error}</Alert>}
      <Input
        type="email"
        placeholder="Email"
        value={credentials.email}
        onChange={(e) => setCredentials(prev => ({
          ...prev,
          email: e.target.value
        }))}
      />
      <Input
        type="password"
        placeholder="Password"
        value={credentials.password}
        onChange={(e) => setCredentials(prev => ({
          ...prev,
          password: e.target.value
        }))}
      />
      <Button type="submit" disabled={loading}>
        {loading ? 'Logging in...' : 'Login'}
      </Button>
    </form>
  );
};
```

---

## 🚀 Deployment / Triển khai

### Build for Production:
```bash
npm run build
```

### Deploy to Vercel:
1. Connect GitHub repository
2. Set root directory to `fe/`
3. Configure environment variables
4. Deploy

### Environment Variables for Vercel:
- `VITE_API_BASE_URL`: Your backend API URL

### Testing Build Locally:
```bash
# Build project
npm run build

# Test production build
npm run preview

# Open http://localhost:4173 to test
```

---

## 📋 Project Features / Tính năng Dự án

### ✅ Implemented Features:
- 🔐 **JWT Authentication** với login/logout
- 🚗 **Vehicle Management** - CRUD operations
- 👥 **Customer Management** - với pagination
- 🔧 **Parts Management** - attach parts to vehicles
- 📋 **Service History** - track maintenance
- 🛡️ **Warranty Claims** - create and track claims
- 🎨 **Responsive UI** với ShadCN/UI + TailwindCSS
- 📱 **Mobile-friendly** design
- 🔄 **Real-time API** integration với error handling

### 🎯 Component Architecture:
- **Atomic Design** pattern với ShadCN/UI
- **Custom Hooks** for state management
- **Protected Routes** với authentication
- **Error Boundaries** cho error handling
- **Loading States** và user feedback
- **Form Validation** với custom hook

---

## 📚 Additional Resources / Tài liệu tham khảo

- [Vite Documentation](https://vitejs.dev/)
- [React Documentation](https://react.dev/)
- [ShadCN/UI Components](https://ui.shadcn.com/)
- [TailwindCSS Documentation](https://tailwindcss.com/)
- [React Router Documentation](https://reactrouter.com/)
- [Axios Documentation](https://axios-http.com/)
- [React Hook Form](https://react-hook-form.com/)

---

## 🤝 Contributing / Đóng góp

### Development Workflow:
1. Clone repository
2. Install dependencies: `npm install`
3. Start dev server: `npm run dev`
4. Make changes
5. Test: `npm run build && npm run preview`
6. Create pull request

### Code Style:
- Use ESLint configuration
- Follow React best practices
- Use ShadCN/UI components when possible
- Write custom hooks for reusable logic

---

*This frontend is built with modern React patterns and best practices for scalability and maintainability. The project demonstrates professional-grade React development with proper state management, API integration, and UI component architecture.*
