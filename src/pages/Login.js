import React, { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const { login, logout, setUser, setToken, token } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post('https://localhost:7029/api/User/login', {
                email,
                password,
            });

            if (response.status === 200 && response.data.token) {
                const newToken = response.data.token;
                setToken(newToken);
                localStorage.setItem('token', newToken);

                // Token dekódolása és felhasználói adatok beállítása
                const decoded = jwtDecode(newToken);
                setUser({ id: decoded.nameid, role: decoded.role, email: decoded.email });

                axios.defaults.headers.common['Authorization'] = `Bearer ${newToken}`;

                navigate('/'); // Sikeres bejelentkezés után navigálás
            } else {
                alert('Login failed');
            }
        } catch (error) {
            console.log('Login failed:', error);
            alert('Login failed');
        }
    };

    useEffect(() => {
        if (token) {
            try {
                const decoded = jwtDecode(token);
                setUser({ id: decoded.nameid, role: decoded.role, email: decoded.email });
                axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
            } catch (error) {
                setUser(null);
                setToken('');
                localStorage.removeItem('token');
            }
        }
    }, [token, setUser, setToken]); // Az effect csak akkor fut, ha a token változik

    return (
        <div>
            <h2>Login</h2>
            <form onSubmit={handleSubmit}>
                <input
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    placeholder="Email"
                    required
                />
                <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Password"
                    required
                />
                <button type="submit">Login</button>
            </form>
        </div>
    );
};

export default Login;
