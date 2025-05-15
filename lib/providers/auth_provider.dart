import 'package:flutter/material.dart';
import 'package:berauto_client/services/api_service.dart';
import 'dart:convert';

class AuthProvider with ChangeNotifier {
  final ApiService _apiService = ApiService();
  String? _token;
  String? _role;

  String? get token => _token;
  String? get role => _role;

  Future<void> login(String email, String password) async {
    _token = await _apiService.login(email, password);
    if (_token != null) {
      // JWT token dekódolása
      final parts = _token!.split('.');
      if (parts.length == 3) {
        final payload = jsonDecode(
          utf8.decode(base64Url.decode(base64Url.normalize(parts[1]))),
        );
        _role = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? 'Customer';
      } else {
        _role = 'Customer';
      }
    }
    notifyListeners();
  }

  Future<void> logout() async {
    _token = null;
    _role = null;
    await _apiService.storage.delete(key: 'jwt_token');
    notifyListeners();
  }
}