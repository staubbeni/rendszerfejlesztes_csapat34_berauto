import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:berauto_client/screens/login_screen.dart';
import 'package:berauto_client/screens/car_list_screen.dart';
import 'package:berauto_client/screens/rental_screen.dart';
import 'package:berauto_client/screens/admin_rental_screen.dart';
import 'package:provider/provider.dart';
import 'package:berauto_client/providers/auth_provider.dart';

final GoRouter router = GoRouter(
  routes: [
    GoRoute(
      path: '/',
      builder: (context, state) => const LoginScreen(),
    ),
    GoRoute(
      path: '/cars',
      builder: (context, state) => const CarListScreen(),
      redirect: (context, state) {
        final authProvider = Provider.of<AuthProvider>(context, listen: false);
        if (authProvider.token == null) return '/';
        return null;
      },
    ),
    GoRoute(
      path: '/rentals',
      builder: (context, state) => const RentalScreen(),
      redirect: (context, state) {
        final authProvider = Provider.of<AuthProvider>(context, listen: false);
        if (authProvider.token == null) return '/';
        return null;
      },
    ),
    GoRoute(
      path: '/admin-rentals',
      builder: (context, state) => const AdminRentalScreen(),
      redirect: (context, state) {
        final authProvider = Provider.of<AuthProvider>(context, listen: false);
        if (authProvider.token == null) return '/';
        if (authProvider.role != 'Employee' && authProvider.role != 'Admin') {
          return '/cars';
        }
        return null;
      },
    ),
  ],
);