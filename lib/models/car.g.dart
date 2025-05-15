// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'car.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Car _$CarFromJson(Map<String, dynamic> json) => Car(
  id: (json['id'] as num).toInt(),
  brand: json['make'] as String?,
  model: json['model'] as String?,
  odometer: (json['odometer'] as num).toInt(),
  isAvailable: json['isAvailable'] as bool,
  categoryId: (json['categoryId'] as num?)?.toInt() ?? 0,
  dailyRate: (json['dailyRate'] as num?)?.toDouble() ?? 0.0,
);

Map<String, dynamic> _$CarToJson(Car instance) => <String, dynamic>{
  'id': instance.id,
  'make': instance.brand,
  'model': instance.model,
  'odometer': instance.odometer,
  'isAvailable': instance.isAvailable,
  'categoryId': instance.categoryId,
  'dailyRate': instance.dailyRate,
};
