// src/App.tsx
import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import CarsPage from "./pages/CarsPage";
import RentalsPage from "./pages/RentalsPage";
import AdminPage from "./pages/AdminPage";
import EmployeePage from "./pages/EmployeePage";
import RentalRequestPage from "./pages/RentalRequestPage";
import ProfilePage from "./pages/ProfilePage";
import UnauthorizedPage from "./pages/UnauthorizedPage";
import Navbar from "./components/Navbar";
import ProtectedRoute from "./components/ProtectedRoute";

const App: React.FC = () => {
  return (
    <AuthProvider>
      <Router>
        <Navbar />
        <Routes>
          {/* Nyilvános útvonalak */}
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/unauthorized" element={<UnauthorizedPage />} />
          <Route path="/cars" element={<CarsPage />} />
          <Route path="/rental-request" element={<RentalRequestPage />} />
          <Route path="/" element={<CarsPage />} />

          {/* Védett útvonalak */}
          <Route element={<ProtectedRoute allowedRoles={["Customer"]} />}>
            <Route path="/rentals" element={<RentalsPage />} />
            <Route path="/profile" element={<ProfilePage />} />
          </Route>
          <Route element={<ProtectedRoute allowedRoles={["Admin"]} />}>
            <Route path="/admin" element={<AdminPage />} />
          </Route>
          <Route element={<ProtectedRoute allowedRoles={["Employee", "Admin"]} />}>
            <Route path="/employee" element={<EmployeePage />} />
          </Route>
        </Routes>
      </Router>
    </AuthProvider>
  );
};

export default App;