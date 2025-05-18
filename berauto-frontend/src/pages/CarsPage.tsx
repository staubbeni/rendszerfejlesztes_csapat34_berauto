import React, { useEffect, useState } from "react";
import { getAllCars } from "../api/cars";
import { CarDto } from "../models";

const CarsPage: React.FC = () => {
  const [cars, setCars] = useState<CarDto[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchCars = async () => {
      try {
        const data = await getAllCars();
        setCars(data);
      } catch (err: any) {
        setError(err.response?.data || "Hiba az autók betöltésekor");
      }
    };
    fetchCars();
  }, []);

  return (
    <div style={{ padding: "20px" }}>
      <h2>Autók</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <ul>
        {cars.map((car) => (
          <li key={car.id}>
            {car.make} {car.model} - {car.price} Ft/nap ({car.isAvailable ? "Elérhető" : "Nem elérhető"})
          </li>
        ))}
      </ul>
    </div>
  );
};

export default CarsPage;