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
      setLoading(true);
      const response = await authAPI.login(credentials);
      const token = response.data?.token || response.data?.Token;
      const userData = response.data?.user || response.data?.User || response.data;
      
      if (!token) {
        throw new Error('No token received from server');
      }
      
      setAuthToken(token);
      setUser(userData);
      setIsAuthenticated(true);
      
      return { success: true, user: userData };
    } catch (error) {
      console.error('Login error:', error);
      return { 
        success: false, 
        error: error.response?.data?.message || error.message || 'Login failed' 
      };
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    setAuthToken(null);
    setUser(null);
    setIsAuthenticated(false);
    
    // Optionally call logout API
    authAPI.logout().catch(() => {
      // Ignore logout API errors
    });
    
    // Redirect to login page
    window.location.href = '/login';
  };

  const updateUser = (userData) => {
    setUser(userData);
  };

  const value = {
    user,
    loading,
    isAuthenticated,
    login,
    logout,
    updateUser,
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

// Higher-order component for protected routes
export const withAuth = (Component) => {
  return (props) => {
    const { isAuthenticated, loading } = useAuth();

    if (loading) {
      return (
        <div className="flex items-center justify-center min-h-screen">
          <div className="text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-gray-900 mx-auto"></div>
            <p className="mt-2 text-gray-600">Loading...</p>
          </div>
        </div>
      );
    }

    if (!isAuthenticated) {
      window.location.href = '/login';
      return null;
    }

    return <Component {...props} />;
  };
};

// Hook for role-based access control
export const useRole = () => {
  const { user } = useAuth();
  
  const hasRole = (requiredRole) => {
    if (!user || !user.role) return false;
    
    // Admin has access to everything
    if (user.role === 'Admin') return true;
    
    // Check specific role
    return user.role === requiredRole;
  };

  const hasAnyRole = (roles) => {
    return roles.some(role => hasRole(role));
  };

  return {
    userRole: user?.role,
    hasRole,
    hasAnyRole,
    isAdmin: hasRole('Admin'),
    isManager: hasRole('Manager'),
    isTechnician: hasRole('Technician'),
    isCustomer: hasRole('Customer'),
  };
};
