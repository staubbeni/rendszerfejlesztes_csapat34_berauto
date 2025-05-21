import axios from "axios";
import { RentalDto, RentalRequestDto } from "../models";

const API_URL = "https://localhost:7029/api/Rental";

export const requestRental = async (dto: RentalRequestDto): Promise<RentalDto> => {
  const response = await axios.post(`${API_URL}/RequestRental`, dto, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
  return response.data;
};

export const getUserRentals = async (): Promise<RentalDto[]> => {
  const response = await axios.get(`${API_URL}/MyRentals`, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
  return response.data;
};

export const getAllRentals = async (): Promise<RentalDto[]> => {
  const response = await axios.get(`${API_URL}/List`, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
  return response.data;
};

export const approveRental = async (id: number): Promise<void> => {
  await axios.post(`${API_URL}/Approve/${id}`, null, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
};

export const rejectRental = async (id: number): Promise<void> => {
  await axios.post(`${API_URL}/Reject/${id}`, null, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
};

export const recordPickup = async (id: number): Promise<void> => {
  await axios.post(`${API_URL}/Pickup/${id}`, null, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
};

export const recordReturn = async (id: number): Promise<void> => {
  await axios.post(`${API_URL}/Return/${id}`, null, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
  });
};

export const generateInvoice = async (id: number): Promise<Blob> => {
  const response = await axios.get(`${API_URL}/Invoice/${id}`, {
    headers: { Authorization: `Bearer ${localStorage.getItem("token")}` },
    responseType: "blob",
  });
  return response.data;
};