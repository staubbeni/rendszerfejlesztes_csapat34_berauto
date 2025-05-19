// src/api/carCategories.ts
import axios from "axios";
import { CarCategoryDto } from "../models";

const API_URL = "https://localhost:7029/api/CarCategory";

export const getAllCarCategories = async (): Promise<CarCategoryDto[]> => {
  const response = await axios.get(`${API_URL}/List`);
  return response.data;
};
