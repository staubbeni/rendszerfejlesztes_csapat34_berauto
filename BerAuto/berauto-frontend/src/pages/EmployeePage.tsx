// src/pages/EmployeePage.tsx
import React, { useEffect, useState } from "react";
import {
  getAllRentals,
  approveRental,
  rejectRental,
} from "../api/rentals";
import { RentalDto } from "../models";

const extractErrorMessage = (error: any): string => {
  if (!error.response || !error.response.data) {
    return error.message || "Ismeretlen hiba történt";
  }

  const data = error.response.data;
  if (typeof data === "string") return data;
  if (data.message) return data.message;
  if (data.title) return data.title;
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
        setRentals(data);
      } catch (err: any) {
        setError(extractErrorMessage(err));
      }
    };
    fetchRentals();
  }, []);

  const handleApprove = async (id: number | undefined) => {
    if (!id) return;
    try {
      await approveRental(id);
      setRentals(rentals.map((r) => (r.id === id ? { ...r, status: "Approved" } : r)));
      setError(null);
    } catch (err: any) {
      setError(extractErrorMessage(err));
    }
  };

  const handleReject = async (id: number | undefined) => {
    if (!id) return;
    try {
      await rejectRental(id);
      setRentals(rentals.map((r) => (r.id === id ? { ...r, status: "Rejected" } : r)));
      setError(null);
    } catch (err: any) {
      setError(extractErrorMessage(err));
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2 style={{ marginBottom: "20px" }}>🎯 Alkalmazotti panel – Bérlések kezelése</h2>
      {error && <div style={{ color: "red", marginBottom: "16px" }}>{error}</div>}

      {rentals.length === 0 ? (
        <p>Nincs elérhető bérlés.</p>
      ) : (
        <div style={{ display: "grid", gap: "16px" }}>
          {rentals.map((rental) => (
            <div
              key={rental.id}
              style={{
                border: "1px solid #ccc",
                borderRadius: "8px",
                padding: "16px",
                boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
              }}
            >
              <p><strong>🚗 Autó ID:</strong> {rental.carId ?? "N/A"}</p>
              <p>
                <strong>📄 Státusz:</strong>{" "}
                <span
                  style={{
                    color:
                      rental.status === "Approved"
                        ? "green"
                        : rental.status === "Rejected"
                        ? "red"
                        : "#333",
                  }}
                >
                  {rental.status ?? "N/A"}
                </span>
              </p>
              <p><strong>🔑 Bérlés ID:</strong> {rental.id ?? "N/A"}</p>
              <div style={{ display: "flex", gap: "10px", marginTop: "10px" }}>
                <button
                  onClick={() => handleApprove(rental.id)}
                  disabled={!rental.id}
                  style={{
                    backgroundColor: "#4CAF50",
                    color: "white",
                    border: "none",
                    padding: "8px 12px",
                    borderRadius: "4px",
                    cursor: "pointer",
                  }}
                >
                  Jóváhagyás
                </button>
                <button
                  onClick={() => handleReject(rental.id)}
                  disabled={!rental.id}
                  style={{
                    backgroundColor: "#f44336",
                    color: "white",
                    border: "none",
                    padding: "8px 12px",
                    borderRadius: "4px",
                    cursor: "pointer",
                  }}
                >
                  Elutasítás
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default EmployeePage;
