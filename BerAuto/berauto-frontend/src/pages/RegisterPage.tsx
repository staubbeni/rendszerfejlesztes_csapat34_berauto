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
    address: { city: "", street: "", zipCode: "", state: "" }, // NINCS id property
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
    <div style={{ minHeight: "100vh", display: "flex", alignItems: "center", justifyContent: "center", background: "#f4f6fb" }}>
      <div style={{ background: "#fff", borderRadius: 16, boxShadow: "0 4px 24px rgba(0,0,0,0.10)", padding: 32, minWidth: 340, maxWidth: 420, width: "100%" }}>
        <h2 style={{ textAlign: "center", marginBottom: 24 }}>Regisztráció</h2>
        <form onSubmit={handleSubmit}>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Név:</label>
            <input
              type="text"
              value={formData.name}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setFormData({ ...formData, name: e.target.value })
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Email:</label>
            <input
              type="email"
              value={formData.email}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setFormData({ ...formData, email: e.target.value })
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Jelszó:</label>
            <input
              type="password"
              value={formData.password}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setFormData({ ...formData, password: e.target.value })
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Telefon:</label>
            <input
              type="text"
              value={formData.phoneNumber}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setFormData({ ...formData, phoneNumber: e.target.value })
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
            />
          </div>
          <h3 style={{ textAlign: "center", margin: "24px 0 8px 0" }}>Cím</h3>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Város:</label>
            <input
              type="text"
              value={formData.address.city}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setFormData({
                  ...formData,
                  address: { ...formData.address, city: e.target.value },
                })
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Utca:</label>
            <input
              type="text"
              value={formData.address.street}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setFormData({
                  ...formData,
                  address: { ...formData.address, street: e.target.value },
                })
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Irányítószám:</label>
            <input
              type="text"
              value={formData.address.zipCode}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setFormData({
                  ...formData,
                  address: { ...formData.address, zipCode: e.target.value },
                })
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          <div style={{ marginBottom: 18 }}>
            <label style={{ fontWeight: 500 }}>Állam:</label>
            <input
              type="text"
              value={formData.address.state}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setFormData({
                  ...formData,
                  address: { ...formData.address, state: e.target.value },
                })
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          {error && <p style={{ color: "#d32f2f", background: "#fff0f0", borderRadius: 8, padding: "8px 12px", margin: "12px 0", textAlign: "center" }}>{error}</p>}
          <button
            type="submit"
            style={{
              padding: "12px 0",
              width: "100%",
              background: "#1976d2",
              color: "#fff",
              border: "none",
              borderRadius: 8,
              fontWeight: 600,
              fontSize: 17,
              marginTop: 10,
              cursor: "pointer",
              transition: "background 0.2s"
            }}
            onMouseOver={e => (e.currentTarget.style.background = '#1565c0')}
            onMouseOut={e => (e.currentTarget.style.background = '#1976d2')}
          >
            Regisztráció
          </button>
        </form>
      </div>
    </div>
  );
};

export default RegisterPage;