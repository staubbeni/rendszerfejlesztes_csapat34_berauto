// src/pages/UnauthorizedPage.tsx
import React from "react";
import { Link } from "react-router-dom";

const UnauthorizedPage: React.FC = () => {
  return (
    <div style={{ padding: "20px" }}>
      <h2>Nem jogosult</h2>
      <p>Nincs jogosultságod ennek az oldalnak az eléréséhez.</p>
      <Link to="/cars">Vissza az autókhoz</Link>
    </div>
  );
};

export default UnauthorizedPage;