// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'rental_request_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

RentalRequestDto _$RentalRequestDtoFromJson(Map<String, dynamic> json) =>
    RentalRequestDto(
      userId: (json['userId'] as num?)?.toInt(),
      guestName: json['guestName'] as String?,
      guestEmail: json['guestEmail'] as String?,
      guestPhone: json['guestPhone'] as String?,
      guestAddress: json['guestAddress'] as String?,
      carId: (json['carId'] as num).toInt(),
      from: DateTime.parse(json['from'] as String),
      to: DateTime.parse(json['to'] as String),
    );

Map<String, dynamic> _$RentalRequestDtoToJson(RentalRequestDto instance) =>
    <String, dynamic>{
      'userId': instance.userId,
      'guestName': instance.guestName,
      'guestEmail': instance.guestEmail,
      'guestPhone': instance.guestPhone,
      'guestAddress': instance.guestAddress,
      'carId': instance.carId,
      'from': instance.from.toIso8601String(),
      'to': instance.to.toIso8601String(),
    };
