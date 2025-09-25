# Authentication and Authorization Demo

This API implements JWT-based authentication with role-based authorization.

## Demo Users

The system automatically creates demo users on startup:

| Email | Password | Role |
|-------|----------|------|
| admin@warranty.com | admin123 | Admin |
| manager@warranty.com | manager123 | Manager |
| tech@warranty.com | tech123 | Technician |
| customer@warranty.com | customer123 | Customer |

## Authentication Endpoints

### POST /api/auth/login
Login with email and password to get a JWT token.

**Request:**
```json
{
  "email": "admin@warranty.com",
  "password": "admin123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64-encoded-refresh-token",
  "expiresAt": "2025-09-25T15:00:00Z",
  "user": {
    "id": "guid",
    "email": "admin@warranty.com",
    "name": "System Admin",
    "role": "Admin"
  }
}
```

### POST /api/auth/register
Register a new user.

### GET /api/auth/me
Get current user information (requires authentication).

## Demo Endpoints for 401/403 Responses

### GET /api/auth/demo/unauthorized
Always returns 401 - Demonstrates unauthorized access.

### GET /api/auth/demo/forbidden
Returns 403 if user is not Admin - Demonstrates forbidden access.

### GET /api/auth/demo/customer-only
Only accessible to users with "Customer" role.

### GET /api/auth/demo/manager-only
Only accessible to users with "Manager" or "Admin" roles.

## Protected Endpoints

### Customers Controller
- `GET /api/customers` - Requires Admin or Manager role
- `POST /api/customers` - Requires Admin role
- All endpoints require authentication

### Parts Controller
- `GET /api/parts` - Requires authentication
- `POST /api/parts` - Requires Admin, Manager, or Technician role
- `DELETE /api/parts/{id}` - Requires Admin role

### Warranty Claims Controller
- `GET /api/warrantyclaims` - Requires Admin or Manager role
- Other endpoints require authentication

## Testing Authorization

1. **No Token (401):**
   ```bash
   curl -X GET "https://localhost:7000/api/customers"
   # Returns: 401 Unauthorized
   ```

2. **With Valid Token:**
   ```bash
   curl -X GET "https://localhost:7000/api/customers" \
     -H "Authorization: Bearer your-jwt-token-here"
   ```

3. **Insufficient Role (403):**
   Login as "customer@warranty.com" and try to access:
   ```bash
   curl -X GET "https://localhost:7000/api/customers" \
     -H "Authorization: Bearer customer-jwt-token"
   # Returns: 403 Forbidden (customer cannot access admin/manager endpoints)
   ```

## HTTP Status Codes

- **200 OK**: Request successful
- **401 Unauthorized**: No valid authentication token provided
- **403 Forbidden**: Valid token but insufficient permissions (wrong role)
- **404 Not Found**: Resource not found

## JWT Token Usage

Include the JWT token in the Authorization header:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

Tokens expire in 1 hour. Use the refresh token to get a new JWT token without re-authenticating.
