// src/pages/CarsPage.tsx
import React, { useState, useEffect } from "react";
import { getAllCars } from "../api/cars";
import { CarDto } from "../models";

const CarsPage: React.FC = () => {
  const [cars, setCars] = useState<CarDto[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchCars = async () => {
      try {
        const data = await getAllCars();
        setCars(data.filter(car => car.isAvailable)); // Csak bérelhető autók
        setLoading(false);
      } catch (err: any) {
        setError(err.response?.data || "Hiba az autók betöltésekor");
        setLoading(false);
      }
    };
    fetchCars();
  }, []);

  if (loading) {
    return <div>Betöltés...</div>;
  }

  if (error) {
    return <div style={{ color: "red" }}>{error}</div>;
  }

  return (
    <div style={{ padding: "20px" }}>
      <h2>Bérelhető autók</h2>
      {cars.length === 0 ? (
        <p>Nincsenek bérelhető autók.</p>
      ) : (
        <ul>
          {cars.map((car) => (
            <li key={car.id}>
              {car.make} {car.model} ({car.year}) - Ár: {car.price} Ft/nap
              {car.isAvailable ? " (Elérhető)" : " (Nem elérhető)"}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default CarsPage;