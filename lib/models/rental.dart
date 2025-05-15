import 'package:json_annotation/json_annotation.dart';

part 'rental.g.dart';

enum RentalStatus { pending, approved, rejected, pickedUp, returned }

@JsonSerializable()
class Rental {
  final int id;
  final int? userId;
  final String? guestName;
  final String? guestEmail;
  final String? guestPhone;
  final String? guestAddress;
  final int carId;
  final DateTime requestDate;
  final DateTime? approvalDate;
  final DateTime? pickupDate;
  final DateTime? returnDate;
  final RentalStatus status;
  final double totalCost;

  Rental({
    required this.id,
    this.userId,
    this.guestName,
    this.guestEmail,
    this.guestPhone,
    this.guestAddress,
    required this.carId,
    required this.requestDate,
    this.approvalDate,
    this.pickupDate,
    this.returnDate,
    required this.status,
    required this.totalCost,
  });

  factory Rental.fromJson(Map<String, dynamic> json) => _$RentalFromJson(json);
  Map<String, dynamic> toJson() => _$RentalToJson(this);
}