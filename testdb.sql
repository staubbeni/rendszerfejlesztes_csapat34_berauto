-- Insert Roles (with explicit IDs, enable IDENTITY_INSERT)
SET IDENTITY_INSERT Roles ON;
INSERT INTO Roles (Id, Name) VALUES (1, 'Admin'), (2, 'Customer'), (3, 'Employee');
SET IDENTITY_INSERT Roles OFF;

-- Insert Users (let SQL assign IDs)
INSERT INTO Users (Name, PasswordHash, Email, PhoneNumber)
VALUES
('Alice Admin', 'hash1', 'alice.admin@example.com', '123456789'),
('Bob Customer', 'hash2', 'bob.customer@example.com', '987654321'),
('Eve Employee', 'hash3', 'eve.employee@example.com', '555555555');

-- Insert CarCategories (let SQL assign IDs)
INSERT INTO CarCategories (Name, Description)
VALUES
('Sedan', 'Comfortable 4-door cars'),
('SUV', 'Sport Utility Vehicles'),
('Convertible', 'Open-top cars');

-- Insert Cars (let SQL assign IDs, use valid CategoryId)
INSERT INTO Cars (Brand, Model, CategoryId, DailyRate, IsAvailable, Odometer)
VALUES
('Toyota', 'Corolla', 1, 10000, 1, 50000),
('Ford', 'Explorer', 2, 15000, 1, 30000),
('Mazda', 'MX-5', 3, 20000, 0, 20000);

-- Insert Addresses (let SQL assign IDs, use valid UserId)
INSERT INTO Addresses (Street, City, State, ZipCode, UserId)
VALUES
('Main St 1', 'Budapest', 'BP', '1000', 1),
('Second St 2', 'Debrecen', 'HB', '4026', 2),
('Third St 3', 'Szeged', 'CS', '6720', 3);

-- Insert UserRoles (use actual User and Role IDs)
-- Assuming Users got IDs 1, 2, 3 in the order inserted above
INSERT INTO UserRoles (UsersId, RolesId)
VALUES
(1, 1), -- Alice is Admin
(2, 2), -- Bob is Customer
(3, 3); -- Eve is Employee

-- Insert Rentals (use valid UserId, CarId, and required fields)
INSERT INTO Rentals (
    UserId, GuestName, GuestEmail, GuestPhone, GuestAddress, CarId,
    RequestDate, ApprovalDate, PickupDate, ReturnDate, [From], [To], Status, TotalCost
) VALUES
(2, 'Bob Customer', 'bob.customer@example.com', '987654321', 'Second St 2, Debrecen, HB, 4026', 1,
 '2025-05-01', '2025-05-01', '2025-05-02', '2025-05-05', '2025-05-02', '2025-05-05', 1, 30000.0000),
(3, 'Eve Employee', 'eve.employee@example.com', '555555555', 'Third St 3, Szeged, CS, 6720', 2,
 '2025-05-10', '2025-05-10', '2025-05-11', '2025-05-12', '2025-05-11', '2025-05-12', 3, 15000.0000);

-- Optionally, insert a rental for a guest (no user)
INSERT INTO Rentals (
    UserId, GuestName, GuestEmail, GuestPhone, GuestAddress, CarId,
    RequestDate, ApprovalDate, PickupDate, ReturnDate, [From], [To], Status, TotalCost
) VALUES
(NULL, 'Guest User', 'guest@example.com', '111222333', 'Guest St 4, Pécs, 7621', 3,
 '2025-05-15', NULL, NULL, NULL, '2025-05-16', '2025-05-18', 0, 40000.0000);