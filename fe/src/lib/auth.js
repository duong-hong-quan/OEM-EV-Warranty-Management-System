const USER_KEY = "authUser";

export function setCurrentUser(user) {
  try {
    if (user) {
      localStorage.setItem(USER_KEY, JSON.stringify(user));
    } else {
      localStorage.removeItem(USER_KEY);
    }
  } catch {}
}

export function getCurrentUser() {
  try {
    const raw = localStorage.getItem(USER_KEY);
    return raw ? JSON.parse(raw) : null;
  } catch {
    return null;
  }
}

export function getCurrentRole() {
  const user = getCurrentUser();
  return user?.role || user?.Role || null;
}

export function clearAuth() {
  localStorage.removeItem("authToken");
  localStorage.removeItem(USER_KEY);
} 