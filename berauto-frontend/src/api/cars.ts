import apiClient from "./apiClient";
import { CarDto } from "../models";

export const getAllCars = async (): Promise<CarDto[]> => {
  const response = await apiClient.get("/Car/List");
  return response.data;
};

export const getCarById = async (id: number): Promise<CarDto> => {
  const response = await apiClient.get(`/Car/Details/${id}`);
  return response.data;
};