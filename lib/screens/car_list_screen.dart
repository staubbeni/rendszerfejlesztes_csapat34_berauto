import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:berauto_client/services/api_service.dart';
import 'package:berauto_client/models/car.dart';
import 'package:berauto_client/models/rental_request_dto.dart';
import 'package:provider/provider.dart';
import 'package:berauto_client/providers/auth_provider.dart';

class CarListScreen extends StatefulWidget {
  const CarListScreen({super.key});

  @override
  _CarListScreenState createState() => _CarListScreenState();
}

class _CarListScreenState extends State<CarListScreen> {
  final ApiService _apiService = ApiService();
  late Future<List<Car>> _carsFuture;

  @override
  void initState() {
    super.initState();
    _carsFuture = _apiService.getCars();
  }

  void _showRentalForm(BuildContext context, int carId) {
    final guestNameController = TextEditingController();
    final guestEmailController = TextEditingController();
    final guestPhoneController = TextEditingController();
    final guestAddressController = TextEditingController();
    DateTime? fromDate;
    DateTime? toDate;

    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        title: const Text('Bérlési kérelem'),
        content: StatefulBuilder(
          builder: (context, setDialogState) => SingleChildScrollView(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                TextField(
                  controller: guestNameController,
                  decoration: const InputDecoration(labelText: 'Név'),
                ),
                TextField(
                  controller: guestEmailController,
                  decoration: const InputDecoration(labelText: 'Email'),
                ),
                TextField(
                  controller: guestPhoneController,
                  decoration: const InputDecoration(labelText: 'Telefonszám'),
                ),
                TextField(
                  controller: guestAddressController,
                  decoration: const InputDecoration(labelText: 'Cím'),
                ),
                ListTile(
                  title: Text(
                    fromDate == null
                        ? 'Kezdés dátuma'
                        : 'Kezdés: ${fromDate!.toLocal()}',
                  ),
                  trailing: const Icon(Icons.calendar_today),
                  onTap: () async {
                    final selectedDate = await showDatePicker(
                      context: context,
                      initialDate: DateTime.now(),
                      firstDate: DateTime.now(),
                      lastDate: DateTime.now().add(const Duration(days: 365)),
                    );
                    if (selectedDate != null) {
                      final selectedTime = await showTimePicker(
                        context: context,
                        initialTime: TimeOfDay.now(),
                      );
                      if (selectedTime != null) {
                        setDialogState(() {
                          fromDate = DateTime(
                            selectedDate.year,
                            selectedDate.month,
                            selectedDate.day,
                            selectedTime.hour,
                            selectedTime.minute,
                          );
                        });
                      }
                    }
                  },
                ),
                ListTile(
                  title: Text(
                    toDate == null
                        ? 'Befejezés dátuma'
                        : 'Befejezés: ${toDate!.toLocal()}',
                  ),
                  trailing: const Icon(Icons.calendar_today),
                  onTap: () async {
                    final selectedDate = await showDatePicker(
                      context: context,
                      initialDate: DateTime.now().add(const Duration(days: 1)),
                      firstDate: DateTime.now(),
                      lastDate: DateTime.now().add(const Duration(days: 365)),
                    );
                    if (selectedDate != null) {
                      final selectedTime = await showTimePicker(
                        context: context,
                        initialTime: TimeOfDay.now(),
                      );
                      if (selectedTime != null) {
                        setDialogState(() {
                          toDate = DateTime(
                            selectedDate.year,
                            selectedDate.month,
                            selectedDate.day,
                            selectedTime.hour,
                            selectedTime.minute,
                          );
                        });
                      }
                    }
                  },
                ),
              ],
            ),
          ),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('Mégse'),
          ),
          ElevatedButton(
            onPressed: () async {
              if (fromDate == null || toDate == null) {
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(content: Text('Kérem adja meg a kezdő és befejező dátumot')),
                );
                return;
              }
              if (toDate!.isBefore(fromDate!)) {
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(content: Text('A befejezés dátuma nem lehet korábbi, mint a kezdés dátuma')),
                );
                return;
              }

              try {
                final authProvider = Provider.of<AuthProvider>(context, listen: false);
                final dto = RentalRequestDto(
                  carId: carId,
                  from: fromDate!,
                  to: toDate!,
                  guestName: authProvider.role != 'Customer' ? null : guestNameController.text.isEmpty ? null : guestNameController.text,
                  guestEmail: authProvider.role != 'Customer' ? null : guestEmailController.text.isEmpty ? null : guestEmailController.text,
                  guestPhone: authProvider.role != 'Customer' ? null : guestPhoneController.text.isEmpty ? null : guestPhoneController.text,
                  guestAddress: authProvider.role != 'Customer' ? null : guestAddressController.text.isEmpty ? null : guestAddressController.text,
                );
                await _apiService.requestRental(dto);
                Navigator.pop(context);
                ScaffoldMessenger.of(context).showSnackBar(
                  const SnackBar(content: Text('Bérlési kérelem elküldve')),
                );
                context.go('/rentals');
              } catch (e) {
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text('Hiba: $e')),
                );
              }
            },
            child: const Text('Küldés'),
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final authProvider = Provider.of<AuthProvider>(context);
    return Scaffold(
      appBar: AppBar(
        title: const Text('Autók'),
        actions: [
          if (authProvider.role == 'Employee' || authProvider.role == 'Admin')
            IconButton(
              icon: const Icon(Icons.admin_panel_settings),
              onPressed: () => context.go('/admin-rentals'),
              tooltip: 'Összes bérlés',
            ),
          IconButton(
            icon: const Icon(Icons.logout),
            onPressed: () async {
              await authProvider.logout();
              context.go('/');
            },
            tooltip: 'Kijelentkezés',
          ),
        ],
      ),
      body: FutureBuilder<List<Car>>(
        future: _carsFuture,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          } else if (snapshot.hasError) {
            return Center(child: Text('Hiba: ${snapshot.error}'));
          } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
            return const Center(child: Text('Nincsenek autók'));
          }

          final cars = snapshot.data!;
          return ListView.builder(
            itemCount: cars.length,
            itemBuilder: (context, index) {
              final car = cars[index];
              return ListTile(
                title: Text('${car.brand ?? 'Ismeretlen'} ${car.model ?? 'Ismeretlen'}'),
                subtitle: Text('Kilométeróra: ${car.odometer} km | Napi díj: ${car.dailyRate.toStringAsFixed(2)} Ft'),
                trailing: car.isAvailable
                    ? ElevatedButton(
                        onPressed: () => _showRentalForm(context, car.id),
                        child: const Text('Bérlés'),
                      )
                    : const Text('Nem elérhető'),
              );
            },
          );
        },
      ),
    );
  }
}