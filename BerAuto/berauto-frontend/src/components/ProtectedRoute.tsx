// src/components/ProtectedRoute.tsx
import React, { useContext } from "react";
import { Navigate, Outlet } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";

interface ProtectedRouteProps {
  allowedRoles: string[];
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ allowedRoles }) => {
  const { isAuthenticated, user } = useContext(AuthContext);

  if (!isAuthenticated || !user) {
    return <Navigate to="/login" replace />;
  }

  const hasRequiredRole = user.roles.some((role: string) =>
    allowedRoles.includes(role)
  );

  return hasRequiredRole ? <Outlet /> : <Navigate to="/unauthorized" replace />;
};

export {}; // **Ez teszi modullá a fájlt**
export default ProtectedRoute;
