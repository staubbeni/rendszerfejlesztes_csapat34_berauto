// src/pages/EmployeePage.tsx
import React, { useEffect, useState } from "react";
import { getAllRentals, approveRental, rejectRental, recordPickup, recordReturn } from "../api/rentals";
import { RentalDto } from "../models";

const EmployeePage: React.FC = () => {
  const [rentals, setRentals] = useState<RentalDto[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchRentals = async () => {
      try {
        const data = await getAllRentals();
        setRentals(data);
      } catch (err: any) {
        setError(err.response?.data || "Hiba a bérlések betöltésekor");
      }
    };
    fetchRentals();
  }, []);

  const handleApprove = async (id: number) => {
    try {
      await approveRental(id);
      setRentals(rentals.map((r) => (r.id === id ? { ...r, status: "Approved" } : r)));
    } catch (err: any) {
      setError(err.response?.data || "Hiba a bérlés jóváhagyásakor");
    }
  };

  const handleReject = async (id: number) => {
    try {
      await rejectRental(id);
      setRentals(rentals.map((r) => (r.id === id ? { ...r, status: "Rejected" } : r)));
    } catch (err: any) {
      setError(err.response?.data || "Hiba a bérlés elutasításakor");
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2>Alkalmazotti Panel - Bérlések kezelése</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <ul>
        {rentals.map((rental) => (
          <li key={rental.id}>
            Autó ID: {rental.carId}, Státusz: {rental.status}
            <button onClick={() => handleApprove(rental.id)}>Jóváhagyás</button>
            <button onClick={() => handleReject(rental.id)}>Elutasítás</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default EmployeePage;