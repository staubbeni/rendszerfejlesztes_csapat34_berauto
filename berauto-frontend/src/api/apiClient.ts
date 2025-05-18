import axios, { AxiosInstance, InternalAxiosRequestConfig } from "axios";

const apiClient: AxiosInstance = axios.create({
  baseURL: "https://localhost:7029/api", // A backend API URL-je
  headers: {
    "Content-Type": "application/json",
  },
});

// Interceptor a JWT token hozzáadásához
apiClient.interceptors.request.use((config: InternalAxiosRequestConfig) => {
  const token = localStorage.getItem("token");
  if (token && config.headers) {
    config.headers.set("Authorization", `Bearer ${token}`);
  }
  return config;
});

export default apiClient;