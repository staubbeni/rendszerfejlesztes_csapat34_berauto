import React, { useContext } from "react";
import { Link } from "react-router-dom";
import { AuthContext } from "../context/AuthContext";

const Navbar: React.FC = () => {
  const { isAuthenticated, user, logout } = useContext(AuthContext);

  return (
    <nav style={{ padding: "10px", background: "#f0f0f0" }}>
      <ul style={{ listStyle: "none", display: "flex", gap: "20px" }}>
        <li>
          <Link to="/cars">Autók</Link>
        </li>
        {isAuthenticated && (
          <li>
            <Link to="/rentals">Bérléseim</Link>
          </li>
        )}
        <li>
          {isAuthenticated ? (
            <>
              <span>Üdv, {user?.name}!</span>
              <button onClick={logout} style={{ marginLeft: "10px" }}>
                Kijelentkezés
              </button>
            </>
          ) : (
            <Link to="/login">Bejelentkezés</Link>
          )}
        </li>
      </ul>
    </nav>
  );
};

export default Navbar;