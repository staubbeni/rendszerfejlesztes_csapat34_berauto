import 'package:flutter/material.dart';
import 'package:berauto_client/services/api_service.dart';
import 'package:berauto_client/models/rental.dart';

class AdminRentalScreen extends StatefulWidget {
  const AdminRentalScreen({super.key});

  @override
  _AdminRentalScreenState createState() => _AdminRentalScreenState();
}

class _AdminRentalScreenState extends State<AdminRentalScreen> {
  final ApiService _apiService = ApiService();
  late Future<List<Rental>> _rentalsFuture;

  @override
  void initState() {
    super.initState();
    _rentalsFuture = _apiService.getAllRentals();
  }

  void _refreshRentals() {
    setState(() {
      _rentalsFuture = _apiService.getAllRentals();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Összes Bérlés')),
      body: FutureBuilder<List<Rental>>(
        future: _rentalsFuture,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          } else if (snapshot.hasError) {
            return Center(child: Text('Hiba: ${snapshot.error}'));
          } else if (!snapshot.hasData || snapshot.data!.isEmpty) {
            return const Center(child: Text('Nincsenek bérlések'));
          }

          final rentals = snapshot.data!;
          return ListView.builder(
            itemCount: rentals.length,
            itemBuilder: (context, index) {
              final rental = rentals[index];
              return ListTile(
                title: Text('Bérlés ID: ${rental.id}'),
                subtitle: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text('Autó ID: ${rental.carId}'),
                    Text('Státusz: ${rental.status.toString().split('.').last}'),
                    Text('Név: ${rental.guestName ?? 'N/A'}'),
                    Text('Email: ${rental.guestEmail ?? 'N/A'}'),
                    Text('Költség: ${rental.totalCost}'),
                    Text('Kezdés: ${rental.requestDate.toLocal()}'),
                    Text('Befejezés: ${rental.returnDate?.toLocal() ?? 'N/A'}'),
                  ],
                ),
                trailing: rental.status == RentalStatus.pending
                    ? Row(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          IconButton(
                            icon: const Icon(Icons.check, color: Colors.green),
                            onPressed: () async {
                              try {
                                final message = await _apiService.approveRental(rental.id);
                                ScaffoldMessenger.of(context).showSnackBar(
                                  SnackBar(content: Text(message)),
                                );
                                _refreshRentals();
                              } catch (e) {
                                ScaffoldMessenger.of(context).showSnackBar(
                                  SnackBar(content: Text('Hiba: $e')),
                                );
                              }
                            },
                          ),
                          IconButton(
                            icon: const Icon(Icons.close, color: Colors.red),
                            onPressed: () async {
                              try {
                                final message = await _apiService.rejectRental(rental.id);
                                ScaffoldMessenger.of(context).showSnackBar(
                                  SnackBar(content: Text(message)),
                                );
                                _refreshRentals();
                              } catch (e) {
                                ScaffoldMessenger.of(context).showSnackBar(
                                  SnackBar(content: Text('Hiba: $e')),
                                );
                              }
                            },
                          ),
                        ],
                      )
                    : null,
              );
            },
          );
        },
      ),
    );
  }
}