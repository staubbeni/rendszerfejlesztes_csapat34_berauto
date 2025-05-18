// src/pages/ProfilePage.tsx
import React, { useState, useContext, useEffect } from "react";
import { AuthContext } from "../context/AuthContext";
import { updateProfile, updateAddress, getCurrentUserAddress } from "../api/user";
import { UserUpdateDto, AddressDto } from "../models";

const ProfilePage: React.FC = () => {
  const { user } = useContext(AuthContext);
  const [profileData, setProfileData] = useState<UserUpdateDto>({
    username: "",
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
      setProfileData({ username: user.name, email: "", phoneNumber: "", roleIds: [] });
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
      setError(err.response?.data || "Hiba a profil frissítésekor");
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
      setError(err.response?.data || "Hiba a cím frissítésekor");
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2>Profil</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <h3>Profil adatok</h3>
      <form onSubmit={handleProfileSubmit}>
        <div>
          <label>Felhasználónév:</label>
          <input
            type="text"
            value={profileData.username}
            onChange={(e) => setProfileData({ ...profileData, username: e.target.value })}
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Email:</label>
          <input
            type="email"
            value={profileData.email}
            onChange={(e) => setProfileData({ ...profileData, email: e.target.value })}
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Telefon:</label>
          <input
            type="text"
            value={profileData.phoneNumber}
            onChange={(e) => setProfileData({ ...profileData, phoneNumber: e.target.value })}
            style={{ margin: "10px", padding: "5px" }}
          />
        </div>
        <button type="submit" style={{ padding: "10px" }}>
          Profil frissítése
        </button>
      </form>
      <h3>Cím adatok</h3>
      <form onSubmit={handleAddressSubmit}>
        <div>
          <label>Város:</label>
          <input
            type="text"
            value={addressData.city}
            onChange={(e) => setAddressData({ ...addressData, city: e.target.value })}
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Utca:</label>
          <input
            type="text"
            value={addressData.street}
            onChange={(e) => setAddressData({ ...addressData, street: e.target.value })}
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Irányítószám:</label>
          <input
            type="text"
            value={addressData.zipCode}
            onChange={(e) => setAddressData({ ...addressData, zipCode: e.target.value })}
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <div>
          <label>Állam:</label>
          <input
            type="text"
            value={addressData.state}
            onChange={(e) => setAddressData({ ...addressData, state: e.target.value })}
            style={{ margin: "10px", padding: "5px" }}
            required
          />
        </div>
        <button type="submit" style={{ padding: "10px" }}>
          Cím frissítése
        </button>
      </form>
    </div>
  );
};

export default ProfilePage;