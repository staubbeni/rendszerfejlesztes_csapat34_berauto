import React, { useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { login, UserLoginDto, LoginResponse } from "../api/auth";
import { useNavigate } from "react-router-dom";

const LoginPage: React.FC = () => {
  const { login: authLogin } = useContext(AuthContext);
  const [credentials, setCredentials] = useState<UserLoginDto>({ email: "", password: "" });
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response: LoginResponse = await login(credentials);
      authLogin(response.token, response.user);
      navigate("/cars");
    } catch (err: any) {
      setError(err.response?.data?.message || "Hibás email vagy jelszó");
    }
  };

  return (
    <div style={{ minHeight: "100vh", display: "flex", alignItems: "center", justifyContent: "center", background: "#f4f6fb" }}>
      <div style={{ background: "#fff", borderRadius: 16, boxShadow: "0 4px 24px rgba(0,0,0,0.10)", padding: 32, minWidth: 340, maxWidth: 400, width: "100%" }}>
        <h2 style={{ textAlign: "center", marginBottom: 24 }}>Bejelentkezés</h2>
        <form onSubmit={handleSubmit}>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Email:</label>
            <input
              type="email"
              value={credentials.email}
              onChange={(e) => setCredentials({ ...credentials, email: e.target.value })}
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          <div style={{ marginBottom: 16 }}>
            <label style={{ fontWeight: 500 }}>Jelszó:</label>
            <input
              type="password"
              value={credentials.password}
              onChange={(e) => setCredentials({ ...credentials, password: e.target.value })}
              style={{ margin: "10px 0", padding: "8px", borderRadius: 8, border: "1px solid #ccc", width: "100%" }}
              required
            />
          </div>
          {error && (
            <p style={{ color: "red", textAlign: "center", marginTop: 12, marginBottom: 0 }}>{error}</p>
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
            Bejelentkezés
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;