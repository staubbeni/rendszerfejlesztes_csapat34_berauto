import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:berauto_client/providers/auth_provider.dart';
import 'package:berauto_client/routes/app_router.dart';

void main() {
  runApp(
    ChangeNotifierProvider(
      create: (context) => AuthProvider(),
      child: const MyApp(),
    ),
  );
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp.router(
      title: 'BerAuto',
      theme: ThemeData(primarySwatch: Colors.blue),
      routerConfig: router,
    );
  }
}