export interface AddressDto {
  id: number;
  street: string;
  city: string;
  state: string;
  zipCode: string;
}

export interface CarDto {
  id: number;
  make: string;
  model: string;
  year: number;
  odometer: number;
  isAvailable: boolean;
  price: number;
  carCategoryId: number;
}

export interface RentalRequestDto {
  carId: number;
  from: string; // ISO dátum string, pl. "2025-05-19T15:35:44.188Z"
  to: string;
  guestName?: string;
  guestEmail?: string;
  guestPhone?: string;
  guestAddress?: string;
}

export interface RentalDto {
  id: number;
  carId: number;
  userId?: number;
  guestName?: string;
  guestEmail?: string;
  guestPhone?: string;
  guestAddress?: string;
  requestDate: string;
  approvalDate?: string;
  pickupDate?: string;
  returnDate?: string;
  from: string;
  to: string;
  status: RentalStatus;
  totalCost: number;
}

export enum RentalStatus {
  Pending = "Pending",
  Approved = "Approved",
  Rejected = "Rejected",
  PickedUp = "PickedUp",
  Returned = "Returned",
}

export interface UserRegisterDto {
  name: string;
  email: string;
  password: string;
  phoneNumber: string;
  address?: AddressDto;
  roleIds: number[];
}

export interface UserLoginDto {
  email: string;
  password: string;
}