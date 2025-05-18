import apiClient from "./apiClient";
import { UserRegisterDto, UserLoginDto } from "../models";

export const register = async (user: UserRegisterDto) => {
  const response = await apiClient.post("/User/register", user);
  return response.data;
};

export const login = async (credentials: UserLoginDto) => {
  const response = await apiClient.post("/User/login", credentials);
  const { token } = response.data;
  localStorage.setItem("token", token);
  return token;
};