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

  return (
    <div style={{ padding: "20px" }}>
      <h2>Bérléseim</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <ul>
        {rentals.map((rental: RentalDto) => (
          <li key={rental.id}>
            Autó ID: {rental.carId}, Időtartam: {new Date(rental.from).toLocaleDateString()} -{" "}
            {new Date(rental.to).toLocaleDateString()}, Státusz: {rental.status}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default RentalsPage;