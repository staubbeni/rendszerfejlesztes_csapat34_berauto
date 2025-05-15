// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'rental.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Rental _$RentalFromJson(Map<String, dynamic> json) => Rental(
  id: (json['id'] as num).toInt(),
  userId: (json['userId'] as num?)?.toInt(),
  guestName: json['guestName'] as String?,
  guestEmail: json['guestEmail'] as String?,
  guestPhone: json['guestPhone'] as String?,
  guestAddress: json['guestAddress'] as String?,
  carId: (json['carId'] as num).toInt(),
  requestDate: DateTime.parse(json['requestDate'] as String),
  approvalDate:
      json['approvalDate'] == null
          ? null
          : DateTime.parse(json['approvalDate'] as String),
  pickupDate:
      json['pickupDate'] == null
          ? null
          : DateTime.parse(json['pickupDate'] as String),
  returnDate:
      json['returnDate'] == null
          ? null
          : DateTime.parse(json['returnDate'] as String),
  status: $enumDecode(_$RentalStatusEnumMap, json['status']),
  totalCost: (json['totalCost'] as num).toDouble(),
);

Map<String, dynamic> _$RentalToJson(Rental instance) => <String, dynamic>{
  'id': instance.id,
  'userId': instance.userId,
  'guestName': instance.guestName,
  'guestEmail': instance.guestEmail,
  'guestPhone': instance.guestPhone,
  'guestAddress': instance.guestAddress,
  'carId': instance.carId,
  'requestDate': instance.requestDate.toIso8601String(),
  'approvalDate': instance.approvalDate?.toIso8601String(),
  'pickupDate': instance.pickupDate?.toIso8601String(),
  'returnDate': instance.returnDate?.toIso8601String(),
  'status': _$RentalStatusEnumMap[instance.status]!,
  'totalCost': instance.totalCost,
};

const _$RentalStatusEnumMap = {
  RentalStatus.pending: 'pending',
  RentalStatus.approved: 'approved',
  RentalStatus.rejected: 'rejected',
  RentalStatus.pickedUp: 'pickedUp',
  RentalStatus.returned: 'returned',
};
