import React, { useState, useContext, useEffect } from "react";
import { AuthContext } from "../context/AuthContext";
import { requestRental } from "../api/rentals";
import { RentalRequestDto } from "../models";
import { useNavigate } from "react-router-dom";
import { getAllCars } from "../api/cars";
import { CarDto } from "../models";
import { getCurrentUserAddress } from "../api/user";

const RentalRequestPage: React.FC = () => {
  const { isAuthenticated, user } = useContext(AuthContext);
  // Determine if user is Admin or Employee
  const isAdminOrEmployee = isAuthenticated && user && user.roles && (user.roles.includes("Admin") || user.roles.includes("Employee"));
  const [cars, setCars] = useState<CarDto[]>([]);
  const [loadingCars, setLoadingCars] = useState<boolean>(true);
  const [userAddress, setUserAddress] = useState<string>("");

  useEffect(() => {
    const fetchCars = async () => {
      try {
        const allCars = await getAllCars();
        setCars(allCars.filter(car => car.isAvailable));
      } catch (e) {
        setCars([]);
      } finally {
        setLoadingCars(false);
      }
    };

    const fetchUserAddress = async () => {
      if (isAuthenticated) {
        try {
          const address = await getCurrentUserAddress();
          setUserAddress(`${address.city}, ${address.street}, ${address.zipCode}, ${address.state}`);
        } catch (e) {
          console.error("Hiba a cím lekérdezésekor:", e);
        }
      }
    };

    fetchCars();
    fetchUserAddress();
  }, [isAuthenticated]);

  const [formData, setFormData] = useState<RentalRequestDto>({
    carId: 0,
    from: "",
    to: "",
    guestName: isAuthenticated && user ? user.name : "",
    guestEmail: isAuthenticated && user ? user.email : "",
    guestPhone: isAuthenticated && user ? user.phoneNumber : "",
    guestAddress: isAuthenticated ? userAddress : "",
  });
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated && userAddress) {
      setFormData(prev => ({
        ...prev,
        guestName: user?.name || "",
        guestEmail: user?.email || "",
        guestPhone: user?.phoneNumber || "",
        guestAddress: userAddress,
      }));
    }
  }, [userAddress, user, isAuthenticated]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!formData.carId || !formData.from || !formData.to) {
      setError("Kérlek, válassz autót és add meg a kölcsönzés időszakát!");
      return;
    }
    const fromDate = new Date(formData.from);
    const toDate = new Date(formData.to);
    if (isNaN(fromDate.getTime()) || isNaN(toDate.getTime())) {
      setError("Hibás dátum formátum!");
      return;
    }
    if (toDate <= fromDate) {
      setError("A befejezés dátuma nem lehet korábbi, mint a kezdés dátuma!");
      return;
    }
    if (isAuthenticated && (!formData.guestEmail || !formData.guestAddress)) {
      setError("Email és cím megadása kötelező!");
      return;
    }
    try {
      const payload: RentalRequestDto = {
        carId: formData.carId,
        from: formData.from,
        to: formData.to,
        guestName: formData.guestName,
        guestEmail: formData.guestEmail,
        guestPhone: formData.guestPhone,
        guestAddress: formData.guestAddress,
      };
      console.log("Küldött kölcsönzési igény:", payload);
      await requestRental(payload);
      navigate(isAuthenticated ? "/rentals" : "/cars");
    } catch (err: any) {
      let backendMsg = err.response?.data || err.message || "";
      if (typeof backendMsg === "object" && backendMsg !== null) {
        backendMsg = backendMsg.title || backendMsg.message || JSON.stringify(backendMsg);
      }
      if (
        typeof backendMsg === "string" &&
        backendMsg.includes("Car is already rented for the specified period.")
      ) {
        setError("Ez az autó már foglalt a megadott időszakra.");
      } else {
        setError(backendMsg || "Hiba a kölcsönzési igény leadásakor");
      }
    }
  };

  if (isAdminOrEmployee) {
    return (
      <div style={{ minHeight: "100vh", display: "flex", alignItems: "center", justifyContent: "center", background: "#f4f6fb" }}>
        <div style={{ background: "#fff", borderRadius: 16, boxShadow: "0 4px 24px rgba(0,0,0,0.10)", padding: 32, minWidth: 340, maxWidth: 400, width: "100%", textAlign: "center" }}>
          <h2 style={{ marginBottom: 24 }}>Kölcsönzési igény</h2>
          <p style={{ color: "#f44336", fontWeight: 500, fontSize: 18 }}>
            Admin vagy dolgozó felhasználók nem adhatnak le kölcsönzési igényt.
          </p>
        </div>
      </div>
    );
  }

  return (
    <div style={{ minHeight: "100vh", display: "flex", alignItems: "center", justifyContent: "center", background: "#f4f6fb" }}>
      <div style={{ background: "#fff", borderRadius: 16, boxShadow: "0 4px 24px rgba(0,0,0,0.10)", padding: 32, minWidth: 340, maxWidth: 400, width: "100%" }}>
        <h2 style={{ textAlign: "center", marginBottom: 24 }}>Kölcsönzési igény</h2>
        <form onSubmit={handleSubmit}>
          {/* --- the original form code remains unchanged here --- */}
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Autó kiválasztása:</label>
            {loadingCars ? (
              <span>Autók betöltése...</span>
            ) : (
              <select
                value={formData.carId}
                onChange={(e: React.ChangeEvent<HTMLSelectElement>) =>
                  setFormData({ ...formData, carId: parseInt(e.target.value) })
                }
                style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
                required
              >
                <option value="">Válassz autót</option>
                {cars.map(car => (
                  <option key={car.id} value={car.id}>
                    {car.make} {car.model} (#{car.id})
                  </option>
                ))}
              </select>
            )}
          </div>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Kezdés dátuma:</label>
            <input
              type="date"
              value={formData.from}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setFormData({ ...formData, from: e.target.value })
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Vége dátuma:</label>
            <input
              type="date"
              value={formData.to}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setFormData({ ...formData, to: e.target.value })
              }
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          {!isAuthenticated && (
            <>
              <div style={{ marginBottom: 16 }}>
                <label style={{ fontWeight: 500 }}>Név:</label>
                <input
                  type="text"
                  value={formData.guestName}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                    setFormData({ ...formData, guestName: e.target.value })
                  }
                  style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
                  required
                />
              </div>
              <div style={{ marginBottom: 16 }}>
                <label style={{ fontWeight: 500 }}>Email:</label>
                <input
                  type="email"
                  value={formData.guestEmail}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                    setFormData({ ...formData, guestEmail: e.target.value })
                  }
                  style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
                  required
                />
              </div>
              <div style={{ marginBottom: 16 }}>
                <label style={{ fontWeight: 500 }}>Telefon:</label>
                <input
                  type="text"
                  value={formData.guestPhone}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                    setFormData({ ...formData, guestPhone: e.target.value })
                  }
                  style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
                  required
                />
              </div>
              <div style={{ marginBottom: 16 }}>
                <label style={{ fontWeight: 500 }}>Cím:</label>
                <input
                  type="text"
                  value={formData.guestAddress}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                    setFormData({ ...formData, guestAddress: e.target.value })
                  }
                  style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
                  required
                />
              </div>
            </>
          )}
          {isAuthenticated && (
            <>
              <div style={{ marginBottom: 16 }}>
                <label style={{ fontWeight: 500 }}>Név:</label>
                <input
                  type="text"
                  value={formData.guestName}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                    setFormData({ ...formData, guestName: e.target.value })
                  }
                  style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
                  required
                />
              </div>
              <div style={{ marginBottom: 16 }}>
                <label style={{ fontWeight: 500 }}>Email:</label>
                <input
                  type="email"
                  value={formData.guestEmail}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                    setFormData({ ...formData, guestEmail: e.target.value })
                  }
                  style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
                  required
                />
              </div>
              <div style={{ marginBottom: 16 }}>
                <label style={{ fontWeight: 500 }}>Telefon:</label>
                <input
                  type="text"
                  value={formData.guestPhone}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                    setFormData({ ...formData, guestPhone: e.target.value })
                  }
                  style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
                  required
                />
              </div>
              <div style={{ marginBottom: 16 }}>
                <label style={{ fontWeight: 500 }}>Cím:</label>
                <input
                  type="text"
                  value={formData.guestAddress}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                    setFormData({ ...formData, guestAddress: e.target.value })
                  }
                  style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
                  required
                />
              </div>
            </>
          )}
          {error && (
            <p style={{ color: "red", textAlign: "center", marginTop: 12, marginBottom: 0 }}>
              {typeof error === "string"
                ? error
                : "Hiba a kölcsönzési igény leadásakor"}
            </p>
          )}
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
              fontSize: 16,
              marginTop: 18,
              cursor: "pointer",
              boxShadow: "0 2px 8px rgba(25, 118, 210, 0.08)"
            }}
          >
            Kölcsönzési igény leadása
          </button>
        </form>
      </div>
    </div>
  );
};

export default RentalRequestPage;