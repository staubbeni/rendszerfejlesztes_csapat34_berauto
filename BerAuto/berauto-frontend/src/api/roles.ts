// src/api/roles.ts
import axios from "axios";
import { RoleDto } from "../models";

const API_URL = "https://localhost:7029/api/Role";

export const getRoles = async (): Promise<RoleDto[]> => {
  const response = await axios.get(`${API_URL}/List`, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
  return response.data;
};