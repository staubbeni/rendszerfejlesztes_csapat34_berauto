import apiClient from "./apiClient";
import { UserRegisterDto, UserLoginDto } from "../models";

// Re-exportáljuk az UserLoginDto-t az ../models-ből export type használatával
export type { UserLoginDto } from "../models";

export interface LoginResponse {
  token: string;
  user: {
    id: number;
    name: string;
    email: string;
    phoneNumber: string;
    roles: string[];
  };
}

export const register = async (user: UserRegisterDto): Promise<UserRegisterDto> => {
  const response = await apiClient.post("/User/register", user);
  return response.data;
};

export const login = async (credentials: UserLoginDto): Promise<LoginResponse> => {
  const response = await apiClient.post("/User/login", credentials);
  const { token, user } = response.data;
  localStorage.setItem("token", token);
  return { token, user };
};