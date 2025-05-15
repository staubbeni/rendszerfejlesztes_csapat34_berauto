import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:berauto_client/models/car.dart';
import 'package:berauto_client/models/rental.dart';
import 'package:berauto_client/models/rental_request_dto.dart';

class ApiService {
  static const String baseUrl = 'https://localhost:7029/api';
  final storage = const FlutterSecureStorage();

  Future<void> saveToken(String token) async {
    await storage.write(key: 'jwt_token', value: token);
  }

  Future<String?> getToken() async {
    return await storage.read(key: 'jwt_token');
  }

  Future<String> login(String email, String password) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/User/login'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({'email': email, 'password': password}),
      );

      if (response.statusCode == 200) {
        final token = jsonDecode(response.body)['token'];
        await saveToken(token);
        return token;
      } else {
        throw Exception('Bejelentkezés sikertelen: ${response.statusCode} - ${response.body}');
      }
    } catch (e) {
      throw Exception('Bejelentkezési hiba: $e');
    }
  }

  Future<List<Car>> getCars() async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/Car/List'),
        headers: {'Authorization': 'Bearer ${await getToken()}'},
      );

      if (response.statusCode == 200) {
        List<dynamic> data = jsonDecode(response.body);
        return data.map((json) => Car.fromJson(json)).toList();
      } else {
        throw Exception('Autók betöltése sikertelen: ${response.statusCode} - ${response.body}');
      }
    } catch (e) {
      throw Exception('Hiba az autók lekérésekor: $e');
    }
  }

  Future<Rental> requestRental(RentalRequestDto dto) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/Rental/Request'),
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer ${await getToken()}',
        },
        body: jsonEncode(dto.toJson()),
      );

      if (response.statusCode == 200) {
        return Rental.fromJson(jsonDecode(response.body));
      } else {
        throw Exception('Bérlési kérelem sikertelen: ${response.statusCode} - ${response.body}');
      }
    } catch (e) {
      throw Exception('Hiba a bérlési kérelem során: $e');
    }
  }

  Future<List<Rental>> getMyRentals() async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/Rental/MyRentals'),
        headers: {'Authorization': 'Bearer ${await getToken()}'},
      );

      if (response.statusCode == 200) {
        List<dynamic> data = jsonDecode(response.body);
        return data.map((json) => Rental.fromJson(json)).toList();
      } else {
        throw Exception('Bérlések betöltése sikertelen: ${response.statusCode} - ${response.body}');
      }
    } catch (e) {
      throw Exception('Hiba a bérlések lekérésekor: $e');
    }
  }

  Future<List<Rental>> getAllRentals() async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/Rental/List'),
        headers: {'Authorization': 'Bearer ${await getToken()}'},
      );

      if (response.statusCode == 200) {
        List<dynamic> data = jsonDecode(response.body);
        return data.map((json) => Rental.fromJson(json)).toList();
      } else {
        throw Exception('Összes bérlés betöltése sikertelen: ${response.statusCode} - ${response.body}');
      }
    } catch (e) {
      throw Exception('Hiba az összes bérlés lekérésekor: $e');
    }
  }

  Future<String> approveRental(int rentalId) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/Rental/Approve/$rentalId'),
        headers: {
          'Authorization': 'Bearer ${await getToken()}',
        },
      );

      if (response.statusCode == 200) {
        return jsonDecode(response.body)['message'] ?? 'Bérlés jóváhagyva';
      } else {
        throw Exception('Bérlés jóváhagyása sikertelen: ${response.statusCode} - ${response.body}');
      }
    } catch (e) {
      throw Exception('Hiba a bérlés jóváhagyása során: $e');
    }
  }

  Future<String> rejectRental(int rentalId) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/Rental/Reject/$rentalId'),
        headers: {
          'Authorization': 'Bearer ${await getToken()}',
        },
      );

      if (response.statusCode == 200) {
        return jsonDecode(response.body)['message'] ?? 'Bérlés elutasítva';
      } else {
        throw Exception('Bérlés elutasítása sikertelen: ${response.statusCode} - ${response.body}');
      }
    } catch (e) {
      throw Exception('Hiba a bérlés elutasítása során: $e');
    }
  }
}