USE master;
GO

IF DB_ID(N'AutoServiceDB') IS NOT NULL
BEGIN
    ALTER DATABASE AutoServiceDB
    SET SINGLE_USER
    WITH ROLLBACK IMMEDIATE;

    DROP DATABASE AutoServiceDB;
END
GO

CREATE DATABASE AutoServiceDB;
GO

USE AutoServiceDB;
GO


-- =============================================
-- Таблица пользователей
-- =============================================

CREATE TABLE users
(
    user_id INT IDENTITY(1,1) NOT NULL,
    login NVARCHAR(50) NOT NULL,
    password_hash NVARCHAR(255) NOT NULL,
    first_name NVARCHAR(100) NOT NULL,
    last_name NVARCHAR(100) NOT NULL,
    role NVARCHAR(20) NOT NULL,
    created_at DATETIME NOT NULL
        CONSTRAINT DF_users_created_at DEFAULT GETDATE(),

    CONSTRAINT PK_users
        PRIMARY KEY (user_id),

    CONSTRAINT UQ_users_login
        UNIQUE (login),

    CONSTRAINT CK_users_role
        CHECK (
            role = N'Администратор'
            OR role = N'Пользователь'
        )
);
GO


-- =============================================
-- Таблица клиентов
-- =============================================

CREATE TABLE clients
(
    client_id INT IDENTITY(1,1) NOT NULL,
    first_name NVARCHAR(100) NOT NULL,
    last_name NVARCHAR(100) NOT NULL,
    phone NVARCHAR(20) NOT NULL,
    email NVARCHAR(100) NULL,
    created_at DATETIME NOT NULL
        CONSTRAINT DF_clients_created_at DEFAULT GETDATE(),

    CONSTRAINT PK_clients
        PRIMARY KEY (client_id)
);
GO


-- =============================================
-- Таблица автомобилей
-- =============================================

CREATE TABLE vehicles
(
    vehicle_id INT IDENTITY(1,1) NOT NULL,
    client_id INT NOT NULL,
    vin NVARCHAR(17) NOT NULL,
    make NVARCHAR(50) NOT NULL,
    model NVARCHAR(50) NOT NULL,
    year INT NOT NULL,
    license_plate NVARCHAR(20) NOT NULL,

    CONSTRAINT PK_vehicles
        PRIMARY KEY (vehicle_id),

    CONSTRAINT UQ_vehicles_vin
        UNIQUE (vin),

    CONSTRAINT UQ_vehicles_license_plate
        UNIQUE (license_plate),

    CONSTRAINT CK_vehicles_year
        CHECK (
            year >= 1900
            AND year <= 2100
        ),

    CONSTRAINT FK_vehicles_clients
        FOREIGN KEY (client_id)
        REFERENCES clients(client_id)
        ON DELETE CASCADE
);
GO


-- =============================================
-- Таблица заказов
-- =============================================

CREATE TABLE work_orders
(
    order_id INT IDENTITY(1,1) NOT NULL,
    vehicle_id INT NOT NULL,
    status NVARCHAR(30) NOT NULL,
    start_date DATETIME NOT NULL,
    completion_date DATETIME NULL,
    total_amount DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_work_orders_total_amount DEFAULT 0,

    CONSTRAINT PK_work_orders
        PRIMARY KEY (order_id),

    CONSTRAINT CK_work_orders_amount
        CHECK (total_amount >= 0),

    CONSTRAINT CK_work_orders_status
        CHECK (
            status = N'Новый'
            OR status = N'В работе'
            OR status = N'Ожидает запчасти'
            OR status = N'Завершён'
            OR status = N'Отменён'
        ),

    CONSTRAINT FK_work_orders_vehicles
        FOREIGN KEY (vehicle_id)
        REFERENCES vehicles(vehicle_id)
        ON DELETE CASCADE
);
GO


-- =============================================
-- Тестовые данные
-- =============================================

INSERT INTO users
(
    login,
    password_hash,
    first_name,
    last_name,
    role
)
VALUES
(
    N'admin',
    N'ADMIN_PASSWORD_HASH',
    N'Иван',
    N'Иванов',
    N'Администратор'
),
(
    N'user',
    N'USER_PASSWORD_HASH',
    N'Пётр',
    N'Петров',
    N'Пользователь'
);
GO


INSERT INTO clients
(
    first_name,
    last_name,
    phone,
    email
)
VALUES
(
    N'Алексей',
    N'Смирнов',
    N'+79990000001',
    N'smirnov@mail.ru'
),
(
    N'Дмитрий',
    N'Иванов',
    N'+79990000002',
    N'ivanov@mail.ru'
),
(
    N'Сергей',
    N'Кузнецов',
    N'+79990000003',
    N'kuznetsov@mail.ru'
),
(
    N'Андрей',
    N'Попов',
    N'+79990000004',
    N'popov@mail.ru'
),
(
    N'Максим',
    N'Соколов',
    N'+79990000005',
    N'sokolov@mail.ru'
);
GO


INSERT INTO vehicles
(
    client_id,
    vin,
    make,
    model,
    year,
    license_plate
)
VALUES
(
    1,
    N'JTDBR32E720000001',
    N'Toyota',
    N'Camry',
    2020,
    N'А111АА'
),
(
    2,
    N'XTA21099070000002',
    N'Lada',
    N'Vesta',
    2021,
    N'В222ВВ'
),
(
    3,
    N'WBA00000000000003',
    N'BMW',
    N'X5',
    2019,
    N'С333СС'
),
(
    4,
    N'ZFA00000000000004',
    N'Kia',
    N'Rio',
    2022,
    N'Е444ЕЕ'
),
(
    5,
    N'JH000000000000005',
    N'Honda',
    N'Civic',
    2018,
    N'К555КК'
);
GO


INSERT INTO work_orders
(
    vehicle_id,
    status,
    start_date,
    completion_date,
    total_amount
)
VALUES
(
    1,
    N'Новый',
    GETDATE(),
    NULL,
    15000.00
),
(
    2,
    N'В работе',
    DATEADD(DAY, -2, GETDATE()),
    NULL,
    25000.00
),
(
    3,
    N'Ожидает запчасти',
    DATEADD(DAY, -3, GETDATE()),
    NULL,
    30000.00
),
(
    4,
    N'Завершён',
    DATEADD(DAY, -7, GETDATE()),
    DATEADD(DAY, -5, GETDATE()),
    18000.00
),
(
    5,
    N'Отменён',
    DATEADD(DAY, -10, GETDATE()),
    DATEADD(DAY, -9, GETDATE()),
    0.00
);
GO


-- =============================================
-- Проверка созданных данных
-- =============================================

SELECT * FROM users;
SELECT * FROM clients;
SELECT * FROM vehicles;
SELECT * FROM work_orders;
GO