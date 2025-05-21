
import React, { useEffect, useState } from "react";
import { getAllCars, createCar, updateCar, deleteCar, setAvailability, updateOdometer } from "../api/cars";
import { getAllCarCategories } from "../api/carCategories";
import { getRoles } from "../api/roles";
import { CarDto, CarCreateDto, CarUpdateDto, RoleDto, CarCategoryDto } from "../models";

const AdminPage: React.FC = () => {
  const [cars, setCars] = useState<CarDto[]>([]);
  const [roles, setRoles] = useState<RoleDto[]>([]);
  const [categories, setCategories] = useState<CarCategoryDto[]>([]);
  const [newCar, setNewCar] = useState<CarCreateDto>({ make: "", model: "", price: 0, carCategoryId: 0 });
  const [error, setError] = useState<string | null>(null);
  const [editStates, setEditStates] = useState<{ [id: number]: Partial<CarDto> }>({});

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [carsData, rolesData, categoryData] = await Promise.all([
          getAllCars(),
          getRoles(),
          getAllCarCategories()
        ]);
        setCars(carsData);
        setRoles(rolesData);
        setCategories(categoryData);
      } catch (err: any) {
        setError(err.response?.data || "Hiba az adatok betöltésekor");
      }
    };
    fetchData();
  }, []);

  // Szerkesztés kezelése (mező változás)
  const handleEditChange = (id: number, field: keyof CarDto, value: any) => {
    setEditStates((prev: { [id: number]: Partial<CarDto> }) => ({
      ...prev,
      [id]: {
        ...prev[id],
        [field]: value,
      },
    }));
  };

  // Szerkesztett autó mentése
  const handleSaveEdit = async (id: number) => {
    const edit = editStates[id];
    if (!edit) return;
    try {

      const updated = await updateCar(id, {
        make: edit.make ?? cars.find((c: CarDto) => c.id === id)?.make ?? "",
        model: edit.model ?? cars.find((c: CarDto) => c.id === id)?.model ?? "",
        price: edit.price ?? cars.find((c: CarDto) => c.id === id)?.price ?? 0,
        carCategoryId: edit.carCategoryId ?? cars.find((c: CarDto) => c.id === id)?.carCategoryId ?? 0,
      });
      setCars(cars.map((car: CarDto) => (car.id === id ? updated : car)));
      setEditStates((prev: { [id: number]: Partial<CarDto> }) => {
        const copy = { ...prev };
        delete copy[id];
        return copy;
      });
      setError(null);
    } catch (err: any) {
      setError(err.response?.data || "Hiba az autó frissítésekor");
    }
  };

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
      setCars(cars.filter((car: CarDto) => car.id !== id));
    } catch (err: any) {
      setError(err.response?.data || "Hiba az autó törlésekor");
    }
  };

  const handleSetAvailability = async (id: number, available: boolean) => {
    try {
      await setAvailability(id, available);
      setCars(cars.map((car: CarDto) => (car.id === id ? { ...car, isAvailable: available } : car)));
    } catch (err: any) {
      setError(err.response?.data || "Hiba az elérhetőség beállításakor");
    }
  };

  const handleUpdateOdometer = async (id: number, newReading: number) => {
    try {
      await updateOdometer(id, newReading);
      setCars(cars.map((car: CarDto) => (car.id === id ? { ...car, odometer: newReading } : car)));
    } catch (err: any) {
      setError(err.response?.data || "Hiba a kilométeróra frissítésekor");
    }
  };

  return (
    <div style={{ padding: 24, maxWidth: 1100, margin: "0 auto" }}>
      <h2 style={{ marginBottom: 24 }}>🛠️ Admin Panel</h2>
      {error && <div style={{ color: "red", marginBottom: 16 }}>{error}</div>}


      <div style={{ background: "#fff", borderRadius: 16, boxShadow: "0 2px 12px rgba(0,0,0,0.08)", padding: 24, marginBottom: 32 }}>
        <h3 style={{ marginTop: 0 }}>Új autó hozzáadása</h3>
        <form onSubmit={handleCreate} style={{ display: "flex", gap: 16, flexWrap: "wrap", alignItems: "center" }}>
          <input
            type="text"
            placeholder="Márka"
            value={newCar.make}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setNewCar({ ...newCar, make: e.target.value })}
            style={{ padding: "8px 12px", borderRadius: 6, border: "1px solid #ccc", minWidth: 120 }}
            required
          />
          <input
            type="text"
            placeholder="Modell"
            value={newCar.model}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setNewCar({ ...newCar, model: e.target.value })}
            style={{ padding: "8px 12px", borderRadius: 6, border: "1px solid #ccc", minWidth: 120 }}
            required
          />
          <input
            type="number"
            placeholder="Ár (Ft/nap)"
            value={newCar.price}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setNewCar({ ...newCar, price: parseInt(e.target.value) })}
            style={{ padding: "8px 12px", borderRadius: 6, border: "1px solid #ccc", minWidth: 120 }}
            required
          />
          <select
            value={newCar.carCategoryId}
            onChange={(e: React.ChangeEvent<HTMLSelectElement>) => setNewCar({ ...newCar, carCategoryId: parseInt(e.target.value) })}
            style={{ padding: "8px 12px", borderRadius: 6, border: "1px solid #ccc", minWidth: 120 }}
            required
          >
            <option value={0} disabled>Kategória</option>
            {categories.map((cat: CarCategoryDto) => (
              <option key={cat.id} value={cat.id}>{cat.name}</option>
            ))}
          </select>
          <button type="submit" style={{ padding: "10px 18px", borderRadius: 6, background: "#4CAF50", color: "#fff", border: "none", fontWeight: 600 }}>
            Autó hozzáadása
          </button>
        </form>
      </div>

      <h3 style={{ marginBottom: 16 }}>Autók listája</h3>
      <div style={{ display: "flex", flexWrap: "wrap", gap: 24 }}>
        {cars.map((car: CarDto) => {
          const edit = editStates[car.id] || {};
          const category = categories.find((cat: CarCategoryDto) => cat.id === (edit.carCategoryId ?? car.carCategoryId));
          return (
            <div key={car.id} style={{ background: "#fff", borderRadius: 16, boxShadow: "0 2px 12px rgba(0,0,0,0.08)", padding: 24, minWidth: 300, flex: "1 1 300px", display: "flex", flexDirection: "column", gap: 10 }}>
              <div style={{ fontWeight: 600, fontSize: 20, marginBottom: 8 }}>
                <input
                  type="text"
                  value={edit.make ?? car.make}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) => handleEditChange(car.id, "make", e.target.value)}
                  style={{ fontWeight: 600, fontSize: 20, border: "none", background: "#f7f7f7", borderRadius: 6, padding: "4px 8px", marginRight: 8 }}
                />
                <input
                  type="text"
                  value={edit.model ?? car.model}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) => handleEditChange(car.id, "model", e.target.value)}
                  style={{ fontWeight: 600, fontSize: 20, border: "none", background: "#f7f7f7", borderRadius: 6, padding: "4px 8px" }}
                />

              </div>
              <div style={{ marginBottom: 8, color: "#555" }}>
                Kategória: <select
                  value={edit.carCategoryId ?? car.carCategoryId}
                  onChange={(e: React.ChangeEvent<HTMLSelectElement>) => handleEditChange(car.id, "carCategoryId", parseInt(e.target.value))}
                  style={{ padding: "4px 8px", borderRadius: 6, border: "1px solid #ccc" }}
                >
                  {categories.map((cat: CarCategoryDto) => (
                    <option key={cat.id} value={cat.id}>{cat.name}</option>
                  ))}
                </select>
              </div>
              <div style={{ marginBottom: 8 }}>
                Ár: <input
                  type="number"
                  value={edit.price ?? car.price}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) => handleEditChange(car.id, "price", parseInt(e.target.value))}
                  style={{ fontWeight: 500, border: "none", background: "#f7f7f7", borderRadius: 6, padding: "4px 8px", width: 90 }}
                /> Ft/nap
              </div>
              <div style={{ marginBottom: 8 }}>
                Km óra: <input
                  type="number"
                  value={edit.odometer ?? car.odometer}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) => handleEditChange(car.id, "odometer", parseInt(e.target.value))}
                  style={{ fontWeight: 500, border: "none", background: "#f7f7f7", borderRadius: 6, padding: "4px 8px", width: 110 }}
                /> km
                <button
                  style={{ marginLeft: 8, padding: "4px 10px", borderRadius: 6, background: "#2196F3", color: "#fff", border: "none", fontWeight: 600, fontSize: 13 }}
                  onClick={async () => {
                    const newValue = edit.odometer ?? car.odometer;
                    if (typeof newValue === "number" && newValue !== car.odometer && !isNaN(newValue)) {
                      try {
                        await handleUpdateOdometer(car.id, newValue);
                        // Sikeres mentés után töröljük az editStates-ből az adott id-t
                        setEditStates(prev => {
                          const copy = { ...prev };
                          delete copy[car.id];
                          return copy;
                        });
                        setError(null);
                      } catch (err: any) {
                        setError("Hiba a kilométeróra frissítésekor");
                      }
                    } else if (isNaN(newValue)) {
                      setError("A kilométeróra értéke nem lehet üres vagy hibás!");
                    }
                  }}
                  disabled={edit.odometer === undefined || edit.odometer === car.odometer || isNaN(edit.odometer as number)}
                  title="Kilométeróra mentése"
                >
                  Km mentés
                </button>
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
                <button
                  onClick={() => handleSetAvailability(car.id, !car.isAvailable)}
                  style={{ marginLeft: 12, padding: "6px 12px", borderRadius: 6, background: car.isAvailable ? "#f44336" : "#4caf50", color: "#fff", border: "none", fontWeight: 600 }}
                >
                  {car.isAvailable ? "Nem elérhetővé tenni" : "Elérhetővé tenni"}
                </button>
              </div>
              <div style={{ display: "flex", gap: 10, marginTop: 8 }}>
                <button
                  onClick={() => handleSaveEdit(car.id)}
                  style={{ padding: "8px 16px", borderRadius: 6, background: "#2196F3", color: "#fff", border: "none", fontWeight: 600 }}
                >
                  Mentés
                </button>
                <button
                  onClick={() => handleDelete(car.id)}
                  style={{ padding: "8px 16px", borderRadius: 6, background: "#f44336", color: "#fff", border: "none", fontWeight: 600 }}
                >
                  Törlés
                </button>
              </div>
            </div>
          );
        })}
      </div>

      <h3 style={{ marginTop: 40 }}>Szerepek listája</h3>
      <ul>
        {roles.map((role: RoleDto) => (
          <li key={role.id}>{role.name}</li>
        ))}
      </ul>
    </div>
  );
};

export default AdminPage;

