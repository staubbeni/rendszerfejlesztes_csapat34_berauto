import 'package:go_router/go_router.dart';
import 'package:berauto_client/screens/login_screen.dart';
import 'package:berauto_client/screens/car_list_screen.dart';
import 'package:berauto_client/screens/rental_screen.dart';

final GoRouter router = GoRouter(
  routes: [
    GoRoute(
      path: '/',
      builder: (context, state) => const LoginScreen(),
    ),
    GoRoute(
      path: '/cars',
      builder: (context, state) => const CarListScreen(),
    ),
    GoRoute(
      path: '/rentals',
      builder: (context, state) => const RentalScreen(),
    ),
  ],
);