import React, { useState } from "react";
import { register } from "../api/auth";
import { UserRegisterDto } from "../models";
import { useNavigate } from "react-router-dom";

// Segítő típus az address mezők kezelésére
interface AddressForm {
  city: string;
  street: string;
  zipCode: string;
  state: string;
}

const RegisterPage: React.FC = () => {
  const [formData, setFormData] = useState<UserRegisterDto>({
    name: "",
    email: "",
    password: "",
    phoneNumber: "",
    roleIds: [2], // Customer role ID, confirm with backend
    address: { city: "", street: "", zipCode: "", state: "" }, // Teljes inicializálás
  });
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  // Segítő függvény az address mezők frissítéséhez
  const updateAddressField = (field: keyof AddressForm, value: string) => {
    setFormData({
      ...formData,
      address: {
        ...(formData.address || { city: "", street: "", zipCode: "", state: "" }),
        [field]: value
      } as AddressForm
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      // Validáció: minden address mező kitöltött
      if (!formData.address?.city || !formData.address?.street || !formData.address?.zipCode || !formData.address?.state) {
        setError("Minden címmező kitöltése kötelező");
        return;
      }
      await register(formData);
      navigate("/login");
    } catch (err: any) {
      setError(err.response?.data?.message || "Hiba a regisztráció során");
    }
  };

  // Biztosítjuk, hogy az address létezik, de az inicializálás miatt ez mindig igaz
  const address = formData.address || { city: "", street: "", zipCode: "", state: "" };

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
              value={address.city}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                updateAddressField("city", e.target.value)
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Utca:</label>
            <input
              type="text"
              value={address.street}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                updateAddressField("street", e.target.value)
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Irányítószám:</label>
            <input
              type="text"
              value={address.zipCode}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                updateAddressField("zipCode", e.target.value)
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          <div style={{ marginBottom: 18 }}>
            <label style={{ fontWeight: 500 }}>Megye:</label>
            <input
              type="text"
              value={address.state}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                updateAddressField("state", e.target.value)
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