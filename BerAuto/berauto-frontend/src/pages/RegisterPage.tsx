// src/pages/RegisterPage.tsx
import React, { useState } from "react";
import { register } from "../api/user";
import { UserRegisterDto, AddressDto } from "../models";
import { useNavigate } from "react-router-dom";

const RegisterPage: React.FC = () => {
  const [formData, setFormData] = useState<UserRegisterDto>({
    name: "",
    email: "",
    password: "",
    phoneNumber: "",
    roleIds: [2], // Customer role ID, confirm with backend
    address: { id: 0, city: "", street: "", zipCode: "", state: "" }, // Added id: 0
  });
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await register(formData);
      navigate("/login");
    } catch (err: any) {
      setError(err.response?.data || "Hiba a regisztráció során");
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2>Regisztráció</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Név:</label>
          <input
            type="text"
            value={formData.name}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setFormData({ ...formData, name: e.target.value })
            }
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Email:</label>
          <input
            type="email"
            value={formData.email}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setFormData({ ...formData, email: e.target.value })
            }
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Jelszó:</label>
          <input
            type="password"
            value={formData.password}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setFormData({ ...formData, password: e.target.value })
            }
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Telefon:</label>
          <input
            type="text"
            value={formData.phoneNumber}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setFormData({ ...formData, phoneNumber: e.target.value })
            }
            style={{ margin: "10px", padding: "5px" }}
          />
        </div>
        <h3>Cím</h3>
        <div>
          <label>Város:</label>
          <input
            type="text"
            value={formData.address.city}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setFormData({
                ...formData,
                address: { ...formData.address, city: e.target.value },
              })
            }
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Utca:</label>
          <input
            type="text"
            value={formData.address.street}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setFormData({
                ...formData,
                address: { ...formData.address, street: e.target.value },
              })
            }
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Irányítószám:</label>
          <input
            type="text" // Corrected from "ProfilePage.tsx"
            value={formData.address.zipCode}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setFormData({
                ...formData,
                address: { ...formData.address, zipCode: e.target.value },
              })
            }
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Állam:</label>
          <input
            type="text"
            value={formData.address.state}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setFormData({
                ...formData,
                address: { ...formData.address, state: e.target.value },
              })
            }
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        {error && <p style={{ color: "red" }}>{error}</p>}
        <button type="submit" style={{ padding: "10px" }}>
          Regisztráció
        </button>
      </form>
    </div>
  );
};

export default RegisterPage;