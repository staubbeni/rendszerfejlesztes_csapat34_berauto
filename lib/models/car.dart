import 'package:json_annotation/json_annotation.dart';

part 'car.g.dart';

@JsonSerializable()
class Car {
  final int id;
  @JsonKey(name: 'make')
  final String? brand;
  final String? model;
  final int odometer;
  final bool isAvailable;
  @JsonKey(defaultValue: 0)
  final int categoryId;
  @JsonKey(defaultValue: 0.0)
  final double dailyRate;

  Car({
    required this.id,
    required this.brand,
    required this.model,
    required this.odometer,
    required this.isAvailable,
    required this.categoryId,
    required this.dailyRate,
  });

  factory Car.fromJson(Map<String, dynamic> json) => _$CarFromJson(json);
  Map<String, dynamic> toJson() => _$CarToJson(this);
}