// src/pages/AdminPage.tsx
import React, { useEffect, useState } from "react";
import { getAllCars, createCar, updateCar, deleteCar, setAvailability, updateOdometer } from "../api/cars";
import { getRoles } from "../api/roles";
import { CarDto, CarCreateDto, CarUpdateDto, RoleDto } from "../models";

const AdminPage: React.FC = () => {
  const [cars, setCars] = useState<CarDto[]>([]);
  const [roles, setRoles] = useState<RoleDto[]>([]);
  const [newCar, setNewCar] = useState<CarCreateDto>({ make: "", model: "", price: 0, carCategoryId: 0 });
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [carsData, rolesData] = await Promise.all([getAllCars(), getRoles()]);
        setCars(carsData);
        setRoles(rolesData);
      } catch (err: any) {
        setError(err.response?.data || "Hiba az adatok betöltésekor");
      }
    };
    fetchData();
  }, []);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const createdCar = await createCar(newCar);
      setCars([...cars, createdCar]);
      setNewCar({ make: "", model: "", price: 0, carCategoryId: 0 });
    } catch (err: any) {
      setError(err.response?.data || "Hiba az autó létrehozásakor");
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await deleteCar(id);
      setCars(cars.filter((car) => car.id !== id));
    } catch (err: any) {
      setError(err.response?.data || "Hiba az autó törlésekor");
    }
  };

  const handleSetAvailability = async (id: number, available: boolean) => {
    try {
      await setAvailability(id, available);
      setCars(cars.map((car) => (car.id === id ? { ...car, isAvailable: available } : car)));
    } catch (err: any) {
      setError(err.response?.data || "Hiba az elérhetőség beállításakor");
    }
  };

  const handleUpdateOdometer = async (id: number, newReading: number) => {
    try {
      await updateOdometer(id, newReading);
      setCars(cars.map((car) => (car.id === id ? { ...car, odometer: newReading } : car)));
    } catch (err: any) {
      setError(err.response?.data || "Hiba a kilométeróra frissítésekor");
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2>Admin Panel</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <h3>Új autó hozzáadása</h3>
      <form onSubmit={handleCreate}>
        <div>
          <label>Márka:</label>
          <input
            type="text"
            value={newCar.make}
            onChange={(e) => setNewCar({ ...newCar, make: e.target.value })}
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Modell:</label>
          <input
            type="text"
            value={newCar.model}
            onChange={(e) => setNewCar({ ...newCar, model: e.target.value })}
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Ár (Ft/nap):</label>
          <input
            type="number"
            value={newCar.price}
            onChange={(e) => setNewCar({ ...newCar, price: parseInt(e.target.value) })}
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Kategória ID:</label>
          <input
            type="number"
            value={newCar.carCategoryId}
            onChange={(e) => setNewCar({ ...newCar, carCategoryId: parseInt(e.target.value) })}
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <button type="submit" style={{ padding: "10px" }}>
          Autó hozzáadása
        </button>
      </form>
      <h3>Autók listája</h3>
      <ul>
        {cars.map((car) => (
          <li key={car.id}>
            {car.make} {car.model} - {car.price} Ft/nap, Kilométeróra: {car.odometer},{" "}
            {car.isAvailable ? "Elérhető" : "Nem elérhető"}
            <button onClick={() => handleDelete(car.id)} style={{ marginLeft: "10px" }}>
              Törlés
            </button>
            <button
              onClick={() => handleSetAvailability(car.id, !car.isAvailable)}
              style={{ marginLeft: "10px" }}
            >
              {car.isAvailable ? "Nem elérhetővé tenni" : "Elérhetővé tenni"}
            </button>
            <input
              type="number"
              placeholder="Új kilométeróra"
              onChange={(e) => handleUpdateOdometer(car.id, parseInt(e.target.value))}
              style={{ marginLeft: "10px", padding: "5px" }}
            />
          </li>
        ))}
      </ul>
      <h3>Szerepek listája</h3>
      <ul>
        {roles.map((role) => (
          <li key={role.id}>{role.name}</li>
        ))}
      </ul>
    </div>
  );
};

export default AdminPage;

