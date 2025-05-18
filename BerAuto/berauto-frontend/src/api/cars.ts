// src/api/cars.ts
import axios from "axios";
import { CarDto, CarCreateDto, CarUpdateDto } from "../models";

const API_URL = "https://localhost:7029/api/Car";

export const getAllCars = async (): Promise<CarDto[]> => {
  const response = await axios.get(`${API_URL}/List`);
  return response.data;
};

export const getCarById = async (id: number): Promise<CarDto> => {
  const response = await axios.get(`${API_URL}/Details/${id}`);
  return response.data;
};

export const createCar = async (car: CarCreateDto): Promise<CarDto> => {
  const response = await axios.post(`${API_URL}/Create`, car, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
  return response.data;
};

export const updateCar = async (id: number, car: CarUpdateDto): Promise<CarDto> => {
  const response = await axios.put(`${API_URL}/Update/${id}`, car, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
  return response.data;
};

export const deleteCar = async (id: number): Promise<void> => {
  await axios.delete(`${API_URL}/Delete/${id}`, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
};

export const setAvailability = async (id: number, available: boolean): Promise<void> => {
  await axios.put(`${API_URL}/SetAvailability/${id}?available=${available}`, null, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
};

export const updateOdometer = async (id: number, newReading: number): Promise<void> => {
  await axios.put(`${API_URL}/UpdateOdometer/${id}?newReading=${newReading}`, null, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
};