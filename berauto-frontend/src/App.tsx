import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import LoginPage from "./pages/LoginPage";
import CarsPage from "./pages/CarsPage";
import RentalsPage from "./pages/RentalsPage";
import Navbar from "./components/Navbar";

const App: React.FC = () => {
  return (
    <AuthProvider>
      <Router>
        <Navbar />
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/cars" element={<CarsPage />} />
          <Route path="/rentals" element={<RentalsPage />} />
          <Route path="/" element={<CarsPage />} />
        </Routes>
      </Router>
    </AuthProvider>
  );
};

export default App;