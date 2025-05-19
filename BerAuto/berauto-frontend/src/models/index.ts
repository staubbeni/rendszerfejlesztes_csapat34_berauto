// src/models/index.ts
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

export interface CarCreateDto {
  make: string;
  model: string;
  price: number;
  carCategoryId: number;
}

export interface CarUpdateDto {
  make: string;
  model: string;
  price?: number;
  carCategoryId: number;
}

export interface AddressDto {
  id?: number;
  city: string;
  street: string;
  zipCode: string;
  state: string;
}

export interface RoleDto {
  id: number;
  name: string;
}

export interface RentalRequestDto {
  carId: number;
  from: string;
  to: string;
  guestName?: string;
  guestEmail?: string;
  guestPhone?: string;
  guestAddress?: string;
}

export interface RentalDto {
  id: number;
  carId: number;
  from: string;
  to: string;
  guestName: string;
  guestEmail: string;
  guestPhone: string;
  guestAddress: string;
  status: string;
}

export interface UserDto {
  id: number;
  name: string;
  email: string;
  phoneNumber: string;
  rentals: RentalDto[];
  address: AddressDto[];
}

export interface UserRegisterDto {
  name: string;
  email: string;
  password: string;
  phoneNumber: string;
  roleIds: number[];
  address: AddressDto;
}

export interface UserLoginDto {
  email: string;
  password: string;
}

export interface UserUpdateDto {
  username: string;
  email: string;
  phoneNumber: string;
  roleIds: number[];
}

export interface CarCategoryDto {
  id: number;
  name: string;
}