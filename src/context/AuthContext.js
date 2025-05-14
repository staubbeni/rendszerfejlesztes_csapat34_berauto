import React, { createContext, useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import axios from 'axios';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(null);
    const [token, setToken] = useState(localStorage.getItem('token') || '');

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
    }, [token]);

    const login = async (email, password) => {
        try {
            const response = await axios.post('http://localhost:7029/api/User/login', {
                email,
                password,
            });

            const newToken = response.data.token;
            localStorage.setItem('token', newToken);
            setToken(newToken);

            // Token dekódolása és felhasználói adatok beállítása
            const decoded = jwtDecode(newToken);
            setUser({ id: decoded.nameid, role: decoded.role, email: decoded.email });

            axios.defaults.headers.common['Authorization'] = `Bearer ${newToken}`;

            return true;
        } catch (error) {
            console.error('Login failed:', error);
            return false;
        }
    };

    const register = async (name, email, password, phoneNumber) => {
        try {
            await axios.post('http://localhost:7029/api/User/register', {
                name,
                email,
                password,
                phoneNumber,
                roleIds: [2], // Customer szerepkör
            });
            return true;
        } catch (error) {
            console.error('Registration failed:', error);
            return false;
        }
    };

    const logout = () => {
        setUser(null);
        setToken('');
        localStorage.removeItem('token');
        delete axios.defaults.headers.common['Authorization'];
    };

    return (
        <AuthContext.Provider value={{ user, token, login, register, logout, setUser, setToken }}>
            {children}
        </AuthContext.Provider>
    );
};
