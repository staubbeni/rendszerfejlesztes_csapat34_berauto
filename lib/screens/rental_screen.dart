import 'package:flutter/material.dart';
import 'package:berauto_client/services/api_service.dart';
import 'package:berauto_client/models/rental.dart';

class RentalScreen extends StatefulWidget {
  const RentalScreen({super.key});

  @override
  _RentalScreenState createState() => _RentalScreenState();
}

class _RentalScreenState extends State<RentalScreen> {
  final ApiService _apiService = ApiService();
  late Future<List<Rental>> _rentalsFuture;

  @override
  void initState() {
    super.initState();
    _rentalsFuture = _apiService.getMyRentals();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Saját Bérlések')),
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
                    if (rental.approvalDate != null)
                      Text('Jóváhagyás dátuma: ${rental.approvalDate!.toLocal()}'),
                  ],
                ),
              );
            },
          );
        },
      ),
    );
  }
}