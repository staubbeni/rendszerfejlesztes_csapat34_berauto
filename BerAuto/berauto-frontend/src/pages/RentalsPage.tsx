import React, { useEffect, useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { getUserRentals } from "../api/rentals";
import { RentalDto } from "../models";

const RentalsPage: React.FC = () => {
  const { isAuthenticated } = useContext(AuthContext);
  const [rentals, setRentals] = useState<RentalDto[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (isAuthenticated) {
      const fetchRentals = async () => {
        try {
          const data = await getUserRentals();
          setRentals(data);
        } catch (err: any) {
          setError(err.response?.data || "Hiba a bérlések betöltésekor");
        }
      };
      fetchRentals();
    }
  }, [isAuthenticated]);

  if (!isAuthenticated) {
    return <p style={{ padding: "20px" }}>Bejelentkezés szükséges!</p>;
  }

  const formatDate = (date: string) =>
  new Date(date).toLocaleDateString("hu-HU", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit"
  });

  return (
    <div style={{ padding: "20px" }}>
      <h2>Bérléseim</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <ul>
        {rentals.map((rental) => (
          <div
      style={{
        background: "#fff",
        border: "1px solid #ddd",
        borderRadius: "12px",
        padding: "16px",
        marginBottom: "16px",
        boxShadow: "0 2px 8px rgba(0, 0, 0, 0.05)"
      }}
    >
      <h4 style={{ margin: "0 0 8px", color: "#1976d2" }}>
        Autó ID: {rental.carId}
      </h4>
      <p style={{ margin: "4px 0" }}>
        <strong>Időtartam:</strong> {formatDate(rental.from)} – {formatDate(rental.to)}
      </p>
      <p style={{ margin: "4px 0" }}>
        <strong>Státusz:</strong> {rental.status}
      </p>
    </div>
        ))}
      </ul>
    </div>
  );
};

export default RentalsPage;