import React, { useEffect, useState } from "react";
import { getAllRentals, approveRental, rejectRental, recordPickup, recordReturn } from "../api/rentals";
import { RentalDto } from "../models";

// Segédfüggvény a hibaüzenet kinyerésére
const extractErrorMessage = (error: any): string => {
  if (!error.response || !error.response.data) {
    return error.message || "Ismeretlen hiba történt";
  }

  const data = error.response.data;
  if (typeof data === "string") {
    return data;
  }
  if (data.message) {
    return data.message;
  }
  if (data.title) {
    return data.title;
  }
  if (data.errors) {
    const errorMessages = Object.values(data.errors).flat() as string[];
    return errorMessages.length > 0 ? errorMessages[0] : "Hiba történt";
  }
  return "Ismeretlen hiba történt";
};

const EmployeePage: React.FC = () => {
  const [rentals, setRentals] = useState<RentalDto[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchRentals = async () => {
      try {
        const data = await getAllRentals();
        console.log("Backendtől kapott bérlések:", data);
        setRentals(data);
      } catch (err: any) {
        setError(extractErrorMessage(err));
      }
    };
    fetchRentals();
  }, []);

  const handleApprove = async (id: number | undefined) => {
    if (!id) {
      setError("Érvénytelen bérlés azonosító");
      return;
    }
    try {
      await approveRental(id);
      setRentals(rentals.map((r) => (r.id === id ? { ...r, status: "Approved" } : r)));
      setError(null);
    } catch (err: any) {
      const status = err.response?.status;
      let message = "Hiba a bérlés jóváhagyásakor";
      if (status === 404) {
        message = "A bérlés nem található";
      } else if (status === 403) {
        message = "Nincs jogosultságod a jóváhagyáshoz";
      } else if (status === 401) {
        message = "Kérlek, jelentkezz be újra";
      } else if (status === 400) {
        message = "Érvénytelen kérés, ellenőrizd az adatokat";
      } else {
        message = extractErrorMessage(err);
      }
      setError(message);
    }
  };

  const handleReject = async (id: number | undefined) => {
    if (!id) {
      setError("Érvénytelen bérlés azonosító");
      return;
    }
    try {
      await rejectRental(id);
      setRentals(rentals.map((r) => (r.id === id ? { ...r, status: "Rejected" } : r)));
      setError(null);
    } catch (err: any) {
      const status = err.response?.status;
      let message = "Hiba a bérlés elutasításakor";
      if (status === 404) {
        message = "A bérlés nem található";
      } else if (status === 403) {
        message = "Nincs jogosultságod az elutasításhoz";
      } else if (status === 401) {
        message = "Kérlek, jelentkezz be újra";
      } else if (status === 400) {
        message = "Érvénytelen kérés, ellenőrizd az adatokat";
      } else {
        message = extractErrorMessage(err);
      }
      setError(message);
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2>Alkalmazotti Panel - Bérlések kezelése</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <ul>
        {rentals.map((rental) => (
          <li key={rental.id || Math.random()}>
            Autó ID: {rental.carId || "N/A"}, Státusz: {rental.status || "N/A"}, Bérlés ID: {rental.id || "Nincs ID"}
            <button onClick={() => handleApprove(rental.id)} disabled={!rental.id}>Jóváhagyás</button>
            <button onClick={() => handleReject(rental.id)} disabled={!rental.id}>Elutasítás</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default EmployeePage;