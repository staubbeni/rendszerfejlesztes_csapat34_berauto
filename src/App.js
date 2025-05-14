import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext'; // korábban ../context volt
import Navbar from './components/PrivateRoute';              // korábban ../../ volt // korábban ../../ volt
import Login from './pages/Login';
import Register from './pages/Register';
import CarList from './pages/CarList';
import PrivateRoute from './components/PrivateRoute';  // korábban ./components volt, ez helyes így is

function App() {
  return (
      <AuthProvider>
        <Router>
          <Navbar />
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/cars" element={<CarList />} />
          </Routes>
        </Router>
      </AuthProvider>
  );
}

export default App;