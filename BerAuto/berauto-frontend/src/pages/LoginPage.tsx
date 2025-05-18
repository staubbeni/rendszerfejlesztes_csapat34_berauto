import React, { useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { login } from "../api/auth";
import { UserLoginDto } from "../models";

const LoginPage: React.FC = () => {
  const { login: authLogin } = useContext(AuthContext);
  const [credentials, setCredentials] = useState<UserLoginDto>({ email: "", password: "" });
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const token = await login(credentials);
      authLogin(token);
    } catch (err: any) {
      setError(err.response?.data || "Hibás email vagy jelszó");
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <h2>Bejelentkezés</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Email:</label>
          <input
            type="email"
            value={credentials.email}
            onChange={(e) => setCredentials({ ...credentials, email: e.target.value })}
            style={{ margin: "10px", padding: "5px" }}
          />
        </div>
        <div>
          <label>Jelszó:</label>
          <input
            type="password"
            value={credentials.password}
            onChange={(e) => setCredentials({ ...credentials, password: e.target.value })}
            style={{ margin: "10px", padding: "5px" }}
          />
        </div>
        {error && <p style={{ color: "red" }}>{error}</p>}
        <button type="submit" style={{ padding: "10px" }}>
          Bejelentkezés
        </button>
      </form>
    </div>
  );
};

export default LoginPage;