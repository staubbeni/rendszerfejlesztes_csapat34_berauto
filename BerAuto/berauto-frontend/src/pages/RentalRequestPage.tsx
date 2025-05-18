// src/pages/RentalRequestPage.tsx
import React, { useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { requestRental } from "../api/rentals";
import { RentalRequestDto } from "../models";
import { useNavigate } from "react-router-dom";

const RentalRequestPage: React.FC = () => {
  const { isAuthenticated, user } = useContext(AuthContext);
  const [formData, setFormData] = useState<RentalRequestDto>({
    carId: 0,
    from: "",
    to: "",
    guestName: "",
    guestEmail: "",
    guestPhone: "",
    guestAddress: "",
  });
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await requestRental(formData);
      navigate(isAuthenticated ? "/rentals" : "/cars");
    } catch (err: any) {
      setError(err.response?.data || "Hiba a kölcsönzési igény leadásakor");
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2>Kölcsönzési igény</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Autó ID:</label>
          <input
            type="number"
            value={formData.carId}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setFormData({ ...formData, carId: parseInt(e.target.value) })
            }
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Kezdés dátuma:</label>
          <input
            type="date"
            value={formData.from}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setFormData({ ...formData, from: e.target.value })
            }
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Vége dátuma:</label>
          <input
            type="date"
            value={formData.to}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setFormData({ ...formData, to: e.target.value })
            }
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        {!isAuthenticated && (
          <>
            <div>
              <label>Név:</label>
              <input
                type="text"
                value={formData.guestName}
                onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                  setFormData({ ...formData, guestName: e.target.value })
                }
                style={{ margin: "10px", padding: "5px" }}
                required
              />
            </div>
            <div>
              <label>Email:</label>
              <input
                type="email"
                value={formData.guestEmail}
                onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                  setFormData({ ...formData, guestEmail: e.target.value })
                }
                style={{ margin: "10px", padding: "5px" }}
                required
              />
            </div>
            <div>
              <label>Telefon:</label>
              <input
                type="text"
                value={formData.guestPhone}
                onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                  setFormData({ ...formData, guestPhone: e.target.value })
                }
                style={{ margin: "10px", padding: "5px" }}
                required
              />
            </div>
            <div>
              <label>Cím:</label>
              <input
                type="text"
                value={formData.guestAddress}
                onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                  setFormData({ ...formData, guestAddress: e.target.value })
                }
                style={{ margin: "10px", padding: "5px" }}
                required
              />
            </div>
          </>
        )}
        {error && <p style={{ color: "red" }}>{error}</p>}
        <button type="submit" style={{ padding: "10px" }}>
          Kölcsönzési igény leadása
        </button>
      </form>
    </div>
  );
};

export {}; // **Ez teszi modullá a fájlt**
export default RentalRequestPage;
