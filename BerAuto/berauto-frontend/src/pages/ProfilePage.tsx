// src/pages/ProfilePage.tsx
import React, { useState, useContext, useEffect } from "react";
import { AuthContext } from "../context/AuthContext";
import { updateProfile, updateAddress, getCurrentUserAddress } from "../api/user";
import { UserUpdateDto, AddressDto } from "../models";

const ProfilePage: React.FC = () => {
  const { user } = useContext(AuthContext);
  const [profileData, setProfileData] = useState<UserUpdateDto>({
    name: "",
    email: "",
    phoneNumber: "",
    roleIds: [],
  });
  const [addressData, setAddressData] = useState<AddressDto>({
    id: 0, // Default ID for new addresses
    city: "",
    street: "",
    zipCode: "",
    state: "",
  });
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (user) {
      setProfileData({ name: user.name, email: "", phoneNumber: "", roleIds: [] });
      const fetchAddress = async () => {
        try {
          const address = await getCurrentUserAddress();
          setAddressData(address);
        } catch (err: any) {
          setError(err.response?.data || "Nincs tárolt cím, kérjük adja meg az adatokat.");
          // Keep default addressData if no address exists
        }
      };
      fetchAddress();
    }
  }, [user]);

  const handleProfileSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (user) {
        await updateProfile(user.id, profileData);
        alert("Profil frissítve!");
      }
    } catch (err: any) {
      setError("Hiba a profil frissítésekor");
    }
  };

  const handleAddressSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (user) {
        await updateAddress(user.id, addressData);
        alert("Cím frissítve!");
      }
    } catch (err: any) {
      setError("Hiba a cím frissítésekor");
    }
  };

  return (
    <div style={{ minHeight: "100vh", display: "flex", alignItems: "center", justifyContent: "center", background: "#f4f6fb" }}>
      <div style={{ background: "#fff", borderRadius: 16, boxShadow: "0 4px 24px rgba(0,0,0,0.10)", padding: 32, minWidth: 340, maxWidth: 420, width: "100%" }}>
        <h2 style={{ textAlign: "center", marginBottom: 24 }}>Profil</h2>
        {error && <p style={{ color: "#d32f2f", background: "#fff0f0", borderRadius: 8, padding: "8px 12px", margin: "12px 0", textAlign: "center" }}>{error}</p>}
        <h3 style={{ textAlign: "center", margin: "24px 0 12px 0", fontWeight: 600, color: "#2d3a4a" }}>Profil adatok</h3>
        <form onSubmit={handleProfileSubmit}>
          <div style={{ marginBottom: 18 }}>
            <label style={{ fontWeight: 500, display: "block", marginBottom: 4 }}>Felhasználónév:</label>
            <input
              type="text"
              value={profileData.name}
              onChange={e => setProfileData({ ...profileData, name: e.target.value })}
              style={{ marginBottom: 0, padding: "10px", borderRadius: 8, border: "1px solid #ccc", width: "100%", fontSize: 16 }}
              required
            />
          </div>
          <div style={{ marginBottom: 18 }}>
            <label style={{ fontWeight: 500, display: "block", marginBottom: 4 }}>Email:</label>
            <input
              type="email"
              value={profileData.email}
              onChange={e => setProfileData({ ...profileData, email: e.target.value })}
              style={{ marginBottom: 0, padding: "10px", borderRadius: 8, border: "1px solid #ccc", width: "100%", fontSize: 16 }}
              required
            />
          </div>
          <div style={{ marginBottom: 18 }}>
            <label style={{ fontWeight: 500, display: "block", marginBottom: 4 }}>Telefon:</label>
            <input
              type="text"
              value={profileData.phoneNumber}
              onChange={e => setProfileData({ ...profileData, phoneNumber: e.target.value })}
              style={{ marginBottom: 0, padding: "10px", borderRadius: 8, border: "1px solid #ccc", width: "100%", fontSize: 16 }}
            />
          </div>
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
            Profil frissítése
          </button>
        </form>
        <h3 style={{ textAlign: "center", margin: "32px 0 12px 0", fontWeight: 600, color: "#2d3a4a" }}>Cím adatok</h3>
        <form onSubmit={handleAddressSubmit}>
          <div style={{ marginBottom: 18 }}>
            <label style={{ fontWeight: 500, display: "block", marginBottom: 4 }}>Város:</label>
            <input
              type="text"
              value={addressData.city}
              onChange={e => setAddressData({ ...addressData, city: e.target.value })}
              style={{ marginBottom: 0, padding: "10px", borderRadius: 8, border: "1px solid #ccc", width: "100%", fontSize: 16 }}
              required
            />
          </div>
          <div style={{ marginBottom: 18 }}>
            <label style={{ fontWeight: 500, display: "block", marginBottom: 4 }}>Utca:</label>
            <input
              type="text"
              value={addressData.street}
              onChange={e => setAddressData({ ...addressData, street: e.target.value })}
              style={{ marginBottom: 0, padding: "10px", borderRadius: 8, border: "1px solid #ccc", width: "100%", fontSize: 16 }}
              required
            />
          </div>
          <div style={{ marginBottom: 18 }}>
            <label style={{ fontWeight: 500, display: "block", marginBottom: 4 }}>Irányítószám:</label>
            <input
              type="text"
              value={addressData.zipCode}
              onChange={e => setAddressData({ ...addressData, zipCode: e.target.value })}
              style={{ marginBottom: 0, padding: "10px", borderRadius: 8, border: "1px solid #ccc", width: "100%", fontSize: 16 }}
              required
            />
          </div>
          <div style={{ marginBottom: 18 }}>
            <label style={{ fontWeight: 500, display: "block", marginBottom: 4 }}>Állam:</label>
            <input
              type="text"
              value={addressData.state}
              onChange={e => setAddressData({ ...addressData, state: e.target.value })}
              style={{ marginBottom: 0, padding: "10px", borderRadius: 8, border: "1px solid #ccc", width: "100%", fontSize: 16 }}
              required
            />
          </div>
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
            Cím frissítése
          </button>
        </form>
      </div>
    </div>
  );
};

export default ProfilePage;