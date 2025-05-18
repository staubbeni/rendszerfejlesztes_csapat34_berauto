import apiClient from "./apiClient";
import { RentalRequestDto, RentalDto } from "../models";

export const requestRental = async (dto: RentalRequestDto): Promise<RentalDto> => {
  const response = await apiClient.post("/Rental/Request", dto);
  return response.data;
};

export const getUserRentals = async (): Promise<RentalDto[]> => {
  const response = await apiClient.get("/Rental/MyRentals");
  return response.data;
};

export const rejectRental = async (id: number): Promise<void> => {
  await apiClient.post(`/Rental/Reject/Reject/${id}`);
};

export const approveRental = async (id: number): Promise<void> => {
  await apiClient.post(`/Rental/Approve/${id}`);
};

export const pickupRental = async (id: number): Promise<void> => {
  await apiClient.post(`/Rental/Pickup/Pickup/${id}`);
};

export const returnRental = async (id: number): Promise<void> => {
  await apiClient.post(`/Rental/Return/Return/${id}`);
};

export const downloadInvoice = async (id: number): Promise<void> => {
  const response = await apiClient.get(`/Rental/Invoice/Invoice/${id}`, { responseType: "blob" });
  const url = window.URL.createObjectURL(new Blob([response.data]));
  const link = document.createElement("a");
  link.href = url;
  link.setAttribute("download", `Invoice_${id}.pdf`);
  document.body.appendChild(link);
  link.click();
  link.remove();
};