USE master
Go
CREATE DATABASE InternetGameManagement;
GO
USE InternetGameManagement;
GO

-- Bảng Account (hợp nhất User và Manager)
CREATE TABLE Account (
    id INT PRIMARY KEY IDENTITY(1,1),
    username NVARCHAR(50) NOT NULL UNIQUE,
    password NVARCHAR(255) NOT NULL,
    role NVARCHAR(10) NOT NULL, -- "manager" hoặc "user"
    time INT DEFAULT 0 -- Thời gian còn lại để sử dụng máy tính (áp dụng cho "user")
);

-- Thêm dữ liệu mẫu cho Account
-- Thêm 1 tài khoản Manager và 3 tài khoản User
INSERT INTO Account (username, password, role, time) VALUES 
    (N'admin', N'123456', N'manager', NULL),  -- Tài khoản quản lý
    (N'user1', N'123', N'user', 120),          -- Tài khoản người chơi
    (N'user2', N'123', N'user', 90),           -- Tài khoản người chơi
    (N'user3', N'123', N'user', 60);           -- Tài khoản người chơi
GO

-- Bảng Product (sản phẩm hoặc dịch vụ có sẵn)
CREATE TABLE Product (
    id INT PRIMARY KEY IDENTITY(1,1),
    category NVARCHAR(50) NOT NULL, -- Danh mục sản phẩm như đồ ăn, nước uống, dịch vụ, v.v.
    name NVARCHAR(100) NOT NULL,
    price DECIMAL(10, 2) NOT NULL
);

-- Thêm dữ liệu mẫu cho Product
INSERT INTO Product (category, name, price) 
VALUES (N'food', N'Mì Tôm', 10000.00),
       (N'food', N'Sting', 12000.00),
       (N'drink', N'Coca', 8000.00);
GO

-- Bảng Bill (hóa đơn thanh toán)
CREATE TABLE Bill (
    id INT PRIMARY KEY IDENTITY(1,1),
    account_id INT NOT NULL,
    FOREIGN KEY (account_id) REFERENCES Account(id) ON DELETE CASCADE
);

-- Thêm dữ liệu mẫu cho Bill
INSERT INTO Bill (account_id) VALUES (2); -- Tạo hóa đơn cho user1
GO

-- Bảng BillDetails (chi tiết sản phẩm trong hóa đơn)
CREATE TABLE BillDetails (
    id INT PRIMARY KEY IDENTITY(1,1),
    bill_id INT NOT NULL,
    product_id INT NULL, -- Cho phép NULL để có thể sử dụng ON DELETE SET NULL
    quantity INT NOT NULL DEFAULT 1,
    price DECIMAL(10, 2) NOT NULL, -- Giá tại thời điểm mua
    total_price AS (quantity * price) PERSISTED, -- Tổng tiền của từng sản phẩm
    FOREIGN KEY (bill_id) REFERENCES Bill(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES Product(id) ON DELETE SET NULL
);

-- Thêm dữ liệu mẫu cho BillDetails
INSERT INTO BillDetails (bill_id, product_id, quantity, price)
VALUES (1, 1, 2, 10000.00), -- Mì Tôm, số lượng 2
       (1, 3, 1, 8000.00);  -- Coca, số lượng 1
GO
