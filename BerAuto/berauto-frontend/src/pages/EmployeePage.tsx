import React, { useEffect, useState } from "react";
import {
  getAllRentals,
  approveRental,
  rejectRental,
  recordPickup,
  recordReturn,
  generateInvoice,
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
      if (status === 404) message = "A bérlés nem található";
      else if (status === 403) message = "Nincs jogosultságod a jóváhagyáshoz";
      else if (status === 401) message = "Kérlek, jelentkezz be újra";
      else if (status === 400) message = "Érvénytelen kérés, ellenőrizd az adatokat";
      else message = extractErrorMessage(err);
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
      if (status === 404) message = "A bérlés nem található";
      else if (status === 403) message = "Nincs jogosultságod az elutasításhoz";
      else if (status === 401) message = "Kérlek, jelentkezz be újra";
      else if (status === 400) message = "Érvénytelen kérés, ellenőrizd az adatokat";
      else message = extractErrorMessage(err);
      setError(message);
    }
  };

  const handlePickup = async (id: number | undefined) => {
    if (!id) {
      setError("Érvénytelen bérlés azonosító");
      return;
    }
    try {
      await recordPickup(id);
      setRentals(rentals.map((r) => (r.id === id ? { ...r, status: "PickedUp" } : r)));
      setError(null);
    } catch (err: any) {
      const status = err.response?.status;
      let message = "Hiba az autó átadásának rögzítésekor";
      if (status === 404) message = "A bérlés nem található";
      else if (status === 403) message = "Nincs jogosultságod az átadás rögzítéséhez";
      else if (status === 401) message = "Kérlek, jelentkezz be újra";
      else if (status === 400) message = "Érvénytelen kérés, ellenőrizd az adatokat";
      else message = extractErrorMessage(err);
      setError(message);
    }
  };

  const handleReturn = async (id: number | undefined) => {
    if (!id) {
      setError("Érvénytelen bérlés azonosító");
      return;
    }
    try {
      await recordReturn(id);
      setRentals(rentals.map((r) => (r.id === id ? { ...r, status: "Returned" } : r)));
      setError(null);
    } catch (err: any) {
      const status = err.response?.status;
      let message = "Hiba az autó visszavételének rögzítésekor";
      if (status === 404) message = "A bérlés nem található";
      else if (status === 403) message = "Nincs jogosultságod a visszavétel rögzítéséhez";
      else if (status === 401) message = "Kérlek, jelentkezz be újra";
      else if (status === 400) message = "Érvénytelen kérés, ellenőrizd az adatokat";
      else message = extractErrorMessage(err);
      setError(message);
    }
  };

  const handleGenerateInvoice = async (id: number | undefined) => {
    if (!id) {
      setError("Érvénytelen bérlés azonosító");
      return;
    }
    try {
      const blob = await generateInvoice(id);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement("a");
      link.href = url;
      link.setAttribute("download", `Invoice_${id}.pdf`);
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
      setError(null);
    } catch (err: any) {
      const status = err.response?.status;
      let message = "Hiba a számla generálásakor";
      if (status === 404) message = "A bérlés nem található";
      else if (status === 403) message = "Nincs jogosultságod számla generálásához";
      else if (status === 401) message = "Kérlek, jelentkezz be újra";
      else if (status === 400) message = "Érvénytelen kérés, ellenőrizd az adatokat";
      else message = extractErrorMessage(err);
      setError(message);
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
                        : rental.status === "PickedUp"
                        ? "blue"
                        : rental.status === "Returned"
                        ? "purple"
                        : "#333",
                  }}
                >
                  {rental.status ?? "N/A"}
                </span>
              </p>
              <p><strong>🔑 Bérlés ID:</strong> {rental.id ?? "N/A"}</p>
              <p><strong>👤 Vendég neve:</strong> {rental.guestName ?? "N/A"}</p>
              <p><strong>📅 Kezdés:</strong> {new Date(rental.from).toLocaleDateString()}</p>
              <p><strong>📅 Vége:</strong> {new Date(rental.to).toLocaleDateString()}</p>
              <div style={{ display: "flex", gap: "10px", marginTop: "10px", flexWrap: "wrap" }}>
                <button
                  onClick={() => handleApprove(rental.id)}
                  disabled={!rental.id || rental.status !== "Pending"}
                  style={{
                    backgroundColor: "#4CAF50",
                    color: "white",
                    border: "none",
                    padding: "8px 12px",
                    borderRadius: "4px",
                    cursor: rental.id && rental.status === "Pending" ? "pointer" : "not-allowed",
                    opacity: rental.id && rental.status === "Pending" ? 1 : 0.5,
                  }}
                >
                  Jóváhagyás
                </button>
                <button
                  onClick={() => handleReject(rental.id)}
                  disabled={!rental.id || rental.status !== "Pending"}
                  style={{
                    backgroundColor: "#f44336",
                    color: "white",
                    border: "none",
                    padding: "8px 12px",
                    borderRadius: "4px",
                    cursor: rental.id && rental.status === "Pending" ? "pointer" : "not-allowed",
                    opacity: rental.id && rental.status === "Pending" ? 1 : 0.5,
                  }}
                >
                  Elutasítás
                </button>
                <button
                  onClick={() => handlePickup(rental.id)}
                  disabled={!rental.id || rental.status !== "Approved"}
                  style={{
                    backgroundColor: "#2196F3",
                    color: "white",
                    border: "none",
                    padding: "8px 12px",
                    borderRadius: "4px",
                    cursor: rental.id && rental.status === "Approved" ? "pointer" : "not-allowed",
                    opacity: rental.id && rental.status === "Approved" ? 1 : 0.5,
                  }}
                >
                  Átadás rögzítése
                </button>
                <button
                  onClick={() => handleReturn(rental.id)}
                  disabled={!rental.id || rental.status !== "PickedUp"}
                  style={{
                    backgroundColor: "#9C27B0",
                    color: "white",
                    border: "none",
                    padding: "8px 12px",
                    borderRadius: "4px",
                    cursor: rental.id && rental.status === "PickedUp" ? "pointer" : "not-allowed",
                    opacity: rental.id && rental.status === "PickedUp" ? 1 : 0.5,
                  }}
                >
                  Visszavétel rögzítése
                </button>
                <button
                  onClick={() => handleGenerateInvoice(rental.id)}
                  disabled={!rental.id || !["PickedUp", "Returned"].includes(rental.status)}
                  style={{
                    backgroundColor: "#FF9800",
                    color: "white",
                    border: "none",
                    padding: "8px 12px",
                    borderRadius: "4px",
                    cursor:
                      rental.id && ["PickedUp", "Returned"].includes(rental.status)
                        ? "pointer"
                        : "not-allowed",
                    opacity: rental.id && ["PickedUp", "Returned"].includes(rental.status) ? 1 : 0.5,
                  }}
                >
                  Számla generálása
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