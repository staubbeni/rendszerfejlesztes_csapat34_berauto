// src/pages/CarsPage.tsx
import React, { useState, useEffect } from "react";
import { getAllCars } from "../api/cars";
import { getAllCarCategories } from "../api/carCategories";
import { CarDto, CarCategoryDto } from "../models";

const CarsPage: React.FC = () => {
  const [cars, setCars] = useState<CarDto[]>([]);
  const [categories, setCategories] = useState<CarCategoryDto[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [carData, categoryData] = await Promise.all([
          getAllCars(),
          getAllCarCategories()
        ]);
        setCars(carData.filter(car => car.isAvailable));
        setCategories(categoryData);
        setLoading(false);
      } catch (err: any) {
        setError(err.response?.data || "Hiba az autók vagy kategóriák betöltésekor");
        setLoading(false);
      }
    };
    fetchData();
  }, []);

  if (loading) {
    return <div>Betöltés...</div>;
  }

  if (error) {
    return <div style={{ color: "red" }}>{error}</div>;
  }

  return (
    <div style={{ padding: "20px", maxWidth: 900, margin: "0 auto" }}>
      <h2 style={{ marginBottom: 24 }}>Bérelhető autók</h2>
      {cars.length === 0 ? (
        <p>Nincsenek bérelhető autók.</p>
      ) : (
        <div style={{ display: "flex", flexWrap: "wrap", gap: 24 }}>
          {cars.map((car) => {
            const category = categories.find(cat => cat.id === car.carCategoryId);
            return (
              <div
                key={car.id}
                style={{
                  background: "#fff",
                  borderRadius: 16,
                  boxShadow: "0 2px 12px rgba(0,0,0,0.08)",
                  padding: 24,
                  minWidth: 260,
                  flex: "1 1 260px",
                  display: "flex",
                  flexDirection: "column",
                  alignItems: "flex-start"
                }}
              >
                <div style={{ fontWeight: 600, fontSize: 20, marginBottom: 8 }}>
                  {car.make} {car.model} <span style={{ color: "#888", fontWeight: 400 }}>({car.year})</span>
                </div>
                <div style={{ marginBottom: 8, color: "#555" }}>
                  Kategória: {category ? category.name : "-"}
                </div>
                <div style={{ marginBottom: 8 }}>
                  Ár: <span style={{ fontWeight: 500 }}>{car.price.toLocaleString()} Ft/nap</span>
                </div>
                <div style={{ display: "flex", alignItems: "center", gap: 8, marginBottom: 8 }}>
                  <span
                    style={{
                      display: "inline-block",
                      width: 12,
                      height: 12,
                      borderRadius: "50%",
                      background: car.isAvailable ? "#4caf50" : "#f44336",
                      marginRight: 6,
                    }}
                  ></span>
                  <span style={{ color: car.isAvailable ? "#4caf50" : "#f44336", fontWeight: 500 }}>
                    {car.isAvailable ? "Elérhető" : "Nem elérhető"}
                  </span>
                </div>
                <div style={{ color: "#888", fontSize: 13 }}>Km óra: {car.odometer.toLocaleString()} km</div>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
};

export default CarsPage;