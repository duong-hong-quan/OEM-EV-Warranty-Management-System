import axios from "axios";
import { clearAuth } from "@/lib/auth";

const baseURL = import.meta.env.VITE_API_BASE_URL || "http://localhost:5165";

export const api = axios.create({
  baseURL,
  withCredentials: false,
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("authToken");
  if (token) {
    config.headers = config.headers || {};
    config.headers["Authorization"] = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (res) => res,
  (err) => {
    if (err?.response?.status === 401) {
      console.warn("API 401 Unauthorized:", err?.config?.url);
      try {
        clearAuth();
      } catch {}
      try {
        window.dispatchEvent(new CustomEvent("app:unauthorized", { detail: { message: "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại." } }));
      } catch {}
      try {
        if (window.location.pathname !== "/login") {
          window.location.assign("/login");
        }
      } catch {}
    }
    return Promise.reject(err);
  }
);

export function setAuthToken(token) {
  if (token) {
    localStorage.setItem("authToken", token);
  } else {
    localStorage.removeItem("authToken");
  }
} 