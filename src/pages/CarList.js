import { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import { getCars, requestRental } from '../services/api';
import { toast } from 'react-toastify';
import {
    Container,
    Grid,
    Card,
    CardContent,
    CardMedia,
    Typography,
    Button,
    Box,
} from '@mui/material';
import { LocalizationProvider, DatePicker } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs from 'dayjs';

const CarList = () => {
    const [cars, setCars] = useState([]);
    const [startDate, setStartDate] = useState(null);
    const [endDate, setEndDate] = useState(null);
    const { user } = useContext(AuthContext);
    const navigate = useNavigate();

    // Autók lekérése az API-ról
    useEffect(() => {
        const fetchCars = async () => {
            try {
                const response = await getCars();
                setCars(response.data);
            } catch (error) {
                console.error('Error fetching cars:', error);
                toast.error('Failed to load cars');
            }
        };
        fetchCars();
    }, []);

    // Bérlés kezelése
    const handleRent = async (carId) => {
        if (!user) {
            // Vendég bérlés: navigálás a vendég űrlapra
            navigate(`/guest-rental/${carId}`, { state: { startDate, endDate } });
            return;
        }

        if (!startDate || !endDate) {
            toast.error('Please select start and end dates');
            return;
        }

        if (endDate.isBefore(startDate)) {
            toast.error('End date must be after start date');
            return;
        }

        try {
            await requestRental({
                carId,
                startDate: startDate.format('YYYY-MM-DD'),
                endDate: endDate.format('YYYY-MM-DD'),
            });
            toast.success('Rental request submitted!');
            setStartDate(null);
            setEndDate(null);
        } catch (error) {
            console.error('Rental request failed:', error);
            toast.error(error.response?.data?.message || 'Failed to submit rental request');
        }
    };

    return (
        <LocalizationProvider dateAdapter={AdapterDayjs}>
            <Container sx={{ py: 4 }}>
                <Typography variant="h4" gutterBottom align="center">
                    Available Cars
                </Typography>
                <Box sx={{ mb: 4, display: 'flex', gap: 2, justifyContent: 'center' }}>
                    <DatePicker
                        label="Start Date"
                        value={startDate}
                        onChange={(newValue) => setStartDate(newValue)}
                        minDate={dayjs()}
                        slotProps={{ textField: { size: 'small' } }}
                    />
                    <DatePicker
                        label="End Date"
                        value={endDate}
                        onChange={(newValue) => setEndDate(newValue)}
                        minDate={startDate || dayjs()}
                        slotProps={{ textField: { size: 'small' } }}
                    />
                </Box>
                <Grid container spacing={3}>
                    {cars.map((car) => (
                        <Grid item xs={12} sm={6} md={4} key={car.id}>
                            <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
                                <CardMedia
                                    component="img"
                                    height="140"
                                    image={car.imageUrl || 'https://via.placeholder.com/300x140?text=Car'}
                                    alt={`${car.make} ${car.model}`}
                                />
                                <CardContent sx={{ flexGrow: 1 }}>
                                    <Typography variant="h6" gutterBottom>
                                        {car.make} {car.model}
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary">
                                        Category: {car.category?.name || 'N/A'}
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary">
                                        Daily Rate: ${car.dailyRate}/day
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary">
                                        Available: {car.isAvailable ? 'Yes' : 'No'}
                                    </Typography>
                                </CardContent>
                                <Box sx={{ p: 2 }}>
                                    <Button
                                        variant="contained"
                                        color="primary"
                                        fullWidth
                                        onClick={() => handleRent(car.id)}
                                        disabled={!car.isAvailable}
                                    >
                                        Rent
                                    </Button>
                                </Box>
                            </Card>
                        </Grid>
                    ))}
                </Grid>
            </Container>
        </LocalizationProvider>
    );
};

export default CarList;