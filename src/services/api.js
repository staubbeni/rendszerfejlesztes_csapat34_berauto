import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:7029',
});

export const getCars = () => api.get('/Car/List');
export const getCarDetails = (id) => api.get(`/Car/Details/${id}`);
export const requestRental = (rentalData) =>
    api.post('/Rental/Request', rentalData);
// További függvények a többi végponthoz...
export default api;