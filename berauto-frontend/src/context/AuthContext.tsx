import React, { createContext, useState, useEffect } from "react";
import { jwtDecode } from "jwt-decode";

// Explicit típus a jwtDecode függvényhez
interface JwtPayload {
  nameid: string;
  unique_name: string;
  role?: string | string[];
  [key: string]: any;
}

// A jwt-decode valódi függvényaláírásához igazított típus
interface JwtDecodeFn {
  <T = JwtPayload>(token: string, options?: { header?: boolean }): T;
}

// Biztosítjuk, hogy a jwtDecode hívható függvényként legyen kezelve
const decodeToken: JwtDecodeFn = jwtDecode as JwtDecodeFn;

interface AuthContextType {
  isAuthenticated: boolean;
  user: { id: number; name: string; roles: string[] } | null;
  login: (token: string) => void;
  logout: () => void;
}

export const AuthContext = createContext<AuthContextType>({
  isAuthenticated: false,
  user: null,
  login: () => {},
  logout: () => {},
});

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState<{ id: number; name: string; roles: string[] } | null>(null);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      try {
        const decoded = decodeToken<JwtPayload>(token);
        setUser({
          id: parseInt(decoded["nameid"]), // String -> Number konverzió
          name: decoded["unique_name"],
          roles: decoded["role"] ? (Array.isArray(decoded["role"]) ? decoded["role"] : [decoded["role"]]) : [],
        });
        setIsAuthenticated(true);
      } catch (error) {
        localStorage.removeItem("token");
      }
    }
  }, []);

  const login = (token: string) => {
    localStorage.setItem("token", token);
    const decoded = decodeToken<JwtPayload>(token);
    setUser({
      id: parseInt(decoded["nameid"]), // String -> Number konverzió
      name: decoded["unique_name"],
      roles: decoded["role"] ? (Array.isArray(decoded["role"]) ? decoded["role"] : [decoded["role"]]) : [],
    });
    setIsAuthenticated(true);
  };

  const logout = () => {
    localStorage.removeItem("token");
    setUser(null);
    setIsAuthenticated(false);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};