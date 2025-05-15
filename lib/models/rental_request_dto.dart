import 'package:json_annotation/json_annotation.dart';

part 'rental_request_dto.g.dart';

@JsonSerializable()
class RentalRequestDto {
  final int? userId;
  final String? guestName;
  final String? guestEmail;
  final String? guestPhone;
  final String? guestAddress;
  final int carId;
  final DateTime from;
  final DateTime to;

  RentalRequestDto({
    this.userId,
    this.guestName,
    this.guestEmail,
    this.guestPhone,
    this.guestAddress,
    required this.carId,
    required this.from,
    required this.to,
  });

  factory RentalRequestDto.fromJson(Map<String, dynamic> json) => _$RentalRequestDtoFromJson(json);
  Map<String, dynamic> toJson() => _$RentalRequestDtoToJson(this);
}