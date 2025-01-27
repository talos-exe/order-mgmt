    ALTER Table FreightOutbounds
    Drop column Order_Type;

    ALTER Table Billings
    Drop Column Amount;

    ALTER TABLE ParcelOutbounds
    Drop Column Order_Type;

    CREATE TABLE Customer (
    UserID NVARCHAR(25) PRIMARY KEY,
    Company_Name VARCHAR(50),
    Account_Status VARCHAR(25),
    Product_Need_Audit_Free VARCHAR(25),
    Warehouse_Availability INT,
    Billing_Account_ID VARCHAR(25),
    Date_Created DATE,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
    );

INSERT INTO Users (UserID, Username, Password, Email, DateCreated) VALUES
    ('U001', 'johndoe', 'password123', 'john.doe@gmail.com', '2024-01-01'),
    ('U002', 'janedoe', 'password456', 'jane.doe@gmail.com', '2024-01-02'),
    ('U003', 'alyssajones', 'password789', 'alicejones@hotmail.com', '2024-01-03'),
    ('U004', 'robertsmith', 'password321', 'rsmith@outlook.com', '2024-01-04'),
    ('U005', 'carlysimon', 'password654', 'carly.simon@gmail.com', '2024-01-05');

INSERT INTO Warehouse (Warehouse_ID, Warehouse, Country, City, Currency) VALUES
    ('W001', 'NYC Warehouse', 'United States', 'New York', 'USD'),
    ('W002', 'Berlin Supply', 'Germany', 'Berlin', 'EUR'),
    ('W003', 'TokyoStorage', 'Japan', 'Tokyo', 'JPY'),
    ('W004', 'IN Storage Center', 'United States', 'Evansville', 'USD'),
    ('W005', 'TopAus Warehouse', 'Australia', 'Sydney', 'AUD');
    
INSERT INTO Inventories (Product_ID, Warehouse_ID, SKU, Product_Name, Product_Description) VALUES
	('P001', 'W001', 'blb12345', 'Dish Soap', 'Dawn dish soap'),
    ('P002', 'W002', 'CS456321', 'Doormat', 'Welcome doormat'),
    ('P003', 'W003', 'gk001', 'Water Bottle', 'Multi-color water bottle'),
    ('P004', 'W004', '1123555', 'Phone Case', 'Otter mobile phone case'),
    ('P005', 'W005', '456787651', 'Sneakers', 'Black and white sneakers');
    
INSERT INTO BillingAccounts (Billing_Account_ID, User_ID, Account_Balance) VALUES
    ('BA001', 'U001', '500'),
    ('BA002', 'U002', '750'),
    ('BA003', 'U003', '400'),
    ('BA004', 'U004', '150'),
    ('BA005', 'U005', '600');
	
INSERT INTO Charges (Charge_ID, Amount, Charge_Type, Description) VALUES
    ('CC001', '50', 'Order-Based', 'Charge 1'),
    ('CC002', '20', 'Order-Based', 'Charge 2'),
    ('CC003', '15', 'Order-Based', 'Charge 3'),
    ('CC004', '30', 'Cost-Based','Charge 4'),
    ('CC005', '45', 'Cost-Based','Charge 5');

INSERT INTO Billings (Billing_ID, Billing_Account_ID, Charge_ID, Date_Created) VALUES
    ('B01','BA001', 'CC001', '2024-10-09'),
    ('B02','BA002', 'CC002', '2024-11-01'),
    ('B03','BA003', 'CC003', '2024-10-20'),
    ('B04','BA004', 'CC004', '2024-11-02'),
    ('B05','BA005', 'CC005', '2024-10-29');

INSERT INTO FreightOutbounds (Outbound_Order_ID, Order_Status, User_ID, Warehouse_ID, Product_Quantity, Creation_Date, Estimated_Delivery_Date, Order_Ship_Date, Cost, Currency, Recipient, Recipient_Post_Code, Destination_Type, Platform, Shipping_Company, Transport_Days, Related_Adjustment_Order, Tracking_Number, Reference_Order_Number, FBA_Shipment_ID, FBA_Tracking_Number, Outbound_Method) VALUES
    ('FO001', 'Awaiting', 'U001', 'W001', 100, '2024-10-01', '2024-10-15', '2024-10-11', 150.00, 'USD', 'Michael Turner', '10001', 'Others', 'Amazon', 'FedEx', 5, 'RAO001', '123456789', 'REF001', 'FBA001', '2564', 'By product'),
    ('FO002', 'Completed', 'U002', 'W002', 200, '2024-09-01', '2024-09-10', '2024-09-05', 300.00, 'EUR', 'Emily Davis', '10117', 'FBA', 'eBay', 'DHL', 7, 'RAO002', '234567891', 'REF002', 'FBA002', '4579', 'By carton'),
    ('FO003', 'Void', 'U003', 'W003', 150, '2024-08-05', '2024-08-12', '2024-08-07', 250.00, 'JPY', 'David Lee', '160-0022', 'Others', 'Walmart', 'UPS', 4, 'RAO003', '345678912', 'REF003', 'FBA003', '3689', 'By product'),
    ('FO004', 'Exceptions', 'U004', 'W004', 75, '2024-07-10', '2024-07-17', '2024-07-11', 100.00, 'USD', 'Sophia Martin', '47710', 'FBA', 'Target', 'USPS', 3, 'RAO004', '456789123', 'REF004', 'FBA004', '4785', 'By carton'),
    ('FO005', 'Completed', 'U005', 'W005', 120, '2024-06-02', '2024-06-08', '2024-06-04', 180.00, 'AUD', 'Oliver Brown', '2000', 'Others', 'Etsy', 'Australia Post', 6, 'RAO005', '567891234', 'REF005', 'FBA005', '5698', 'By product');


INSERT INTO FreightProductLists (Order_ID, Product_ID, Quantity) VALUES
    ('FO001', 'P001', 50),
    ('FO002', 'P002', 75),
    ('FO003', 'P003', 100),
    ('FO004', 'P004', 25),
    ('FO005', 'P005', 60);

INSERT INTO InboundOrders (Inbound_Order_ID, Order_Status, User_ID, Warehouse_ID, Estimated_Arrival, Product_Quantity, Creation_Date, Cost, Currency, Boxes, Inbound_Type, Tracking_Number, Reference_Order_Number, Arrival_Method) VALUES
    ('IO001', 'Awaiting', 'U001', 'W001', '2024-10-20', 150, '2024-10-01', 500.00, 'USD', 10, 'For Freight', 'TRACK001', 'REFIN001', 'Parcel'),
    ('IO002', 'Receiving', 'U002', 'W002', '2024-09-25', 100, '2024-09-01', 300.00, 'EUR', 5, 'For Parcel', 'TRACK002', 'REFIN002', 'Carton'),
    ('IO003', 'Received', 'U003', 'W003', '2024-08-18', 200, '2024-08-01', 750.00, 'JPY', 8, 'For Freight', 'TRACK003', 'REFIN003', '20FT Container'),
    ('IO004', 'Shelved', 'U004', 'W004', '2024-07-30', 50, '2024-07-10', 150.00, 'USD', 3, 'For Parcel', 'TRACK004', 'REFIN004', 'Self-initiated first mile'),
    ('IO005', 'Void', 'U005', 'W005', '2024-06-15', 250, '2024-06-01', 900.00, 'AUD', 15, 'For Freight', 'TRACK005', 'REFIN005', '40 FT HC Container');

INSERT INTO InboundProductLists (OrderID, ProductID, Quantity) VALUES
    ('IO001', 'P001', 50),
    ('IO001', 'P002', 20),
    ('IO002', 'P002', 75),
    ('IO003', 'P003', 100),
    ('IO004', 'P004', 25),
    ('IO005', 'P005', 60);
    
INSERT INTO ParcelOutbounds (Order_ID, Order_Status, Warehouse_ID, User_ID, Platform, Estimated_Delivery_Date, Ship_Date, Transport_Days, Cost, Currency, Recipient, Country, Postcode, Tracking_Number, Reference_Order_Number, Creation_Date, Boxes, Shipping_Company, Latest_Information, Tracking_Update_Time, Internet_Posting_Time, Delivery_Time, Related_Adjustment_Order) VALUES
    ('PO001', 'Awaiting', 'W001', 'U001', 'Amazon', '2024-10-25', '2024-10-20', 7, 120.00, 'USD', 'Michael Anderson', 'United States', '10001', '123456789', 'REFPO001', '2024-10-01', 5, 'FedEx', 'Delivered to sorting facility', '2024-10-02 14:30:00', '2024-10-02 10:00:00', '2024-10-10 16:00:00', 'RAO002'),
	('PO002', 'Completed', 'W002', 'U002', 'eBay', '2024-09-30', '2024-09-25', 5, 100.00, 'EUR', 'Emily Johnson', 'Germany', '10115', '223344556', 'REFPO002', '2024-09-15', 3, 'DHL', 'Delivered to recipient', '2024-09-28 12:45:00', '2024-09-28 08:30:00', '2024-09-29 14:00:00', 'RAO003'),
    ('PO003', 'Void', 'W003', 'U003', 'Walmart', '2024-08-10', '2024-08-05', 10, 200.00, 'JPY', 'David Lee', 'Japan', '100-0001', '987654321', 'REFPO003', '2024-07-30', 8, 'Japan Post', 'Order canceled', '2024-08-06 16:00:00', '2024-08-06 10:00:00', '2024-08-07 18:00:00', 'RAO004'),
    ('PO004', 'Exceptions', 'W004', 'U004', 'Shopify', '2024-07-20', '2024-07-15', 4, 80.00, 'USD', 'Sarah Williams', 'United States', '90210', '112233445', 'REFPO004', '2024-07-01', 6, 'UPS', 'Delayed due to customs', '2024-07-18 09:00:00', '2024-07-18 08:00:00', '2024-07-25 16:00:00', 'RAO005'),
    ('PO005', 'Awaiting', 'W005', 'U005', 'AliExpress', '2024-06-05', '2024-06-01', 9, 150.00, 'AUD', 'Jessica Brown', 'Australia', '2000', '334455667', 'REFPO005', '2024-05-20', 7, 'DHL', 'Shipped to regional hub', '2024-06-03 10:15:00', '2024-06-02 14:00:00', '2024-06-07 12:00:00', 'RAO006');

INSERT INTO ParcelProductLists (Order_ID, Product_ID, Quantity) VALUES
    ('PO001', 'P001', '5'),
    ('PO002', 'P002', '10'),
    ('PO003', 'P003', '15'),
    ('PO004', 'P004', '20'),
    ('PO005', 'P005', '25');
    
INSERT INTO PlatformOrders (Order_ID, Platform, Warehouse_ID, Product_Quantity, User_ID, Buyer, Recipient_Postcode, Recipient_Country, Store, Site, Shipping_Service, Tracking_Number, Carrier, Order_Time, Payment_Time, Created_Time, Order_Source) VALUES
    ('PFO001', 'Amazon', 'W001', 2, 'U001', 'Buyer1', '12345', 'USA', 'Store1', 'Site1', 'FedEx', 'TN001', 'iMile', '2024-10-01 10:00:00', '2024-10-01 10:05:00', '2024-10-01 09:55:00', 'Online'),
    ('PFO002', 'eBay', 'W001', 1, 'U002', 'Buyer2', '67890', 'USA', 'Store2', 'Site2', 'DHL', 'TN002', 'iMile', '2024-10-02 11:00:00', '2024-10-02 11:05:00', '2024-10-02 10:55:00', 'Online'),
    ('PFO003', 'TikTok', 'W001', 3, 'U003', 'Buyer3', '54321', 'USA', 'Store3', 'Site3', 'UPS', 'TN003', 'UPS', '2024-10-03 12:00:00', '2024-10-03 12:05:00', '2024-10-03 11:55:00', 'Online'),
    ('PFO004', 'Etsy', 'W001', 1, 'U004', 'Buyer4', '98765', 'USA', 'Store4', 'Site4', 'USPS', 'TN004', 'iMile', '2024-10-04 13:00:00', '2024-10-04 13:05:00', '2024-10-04 12:55:00', 'Online'),
    ('PFO005', 'Target', 'W001', 4, 'U005', 'Buyer5', '45678', 'USA', 'Store5', 'Site5', 'FedEx', 'TN005', 'iMile', '2024-10-05 14:00:00', '2024-10-05 14:05:00', '2024-10-05 13:55:00', 'Online');

    
INSERT INTO Roles (Role_ID, Role, Role_Description) VALUES
    ('R001', 'Seller', 'Grants seller access'),
    ('R002', 'Administrator', 'Grants administrator access');

INSERT INTO UserRoles (User_ID, Role_ID) VALUES
    ('U001', 'R001'),
    ('U002', 'R001'),
    ('U003', 'R001'),
    ('U004', 'R002'),
    ('U005', 'R002');

INSERT INTO Customer (UserID, Account_Status, Company_Name, Product_Need_Audit_Free, Date_Created) 
VALUES 
    ('U001', 'Enabled', 'Company A', 'Yes', '2024-04-10'), 
    ('U002','Enabled', 'Company B', 'No', '2024-02-20');


GO
CREATE VIEW Inbound AS 
SELECT 
  Inbound_Order_ID AS "Order ID", 
  Order_Status AS "Status", 
  USERS.Username AS "Creator", 
  Warehouse.Warehouse, 
  Cost, 
  InboundOrders.Currency, 
  Tracking_Number AS "Tracking Number", 
  Reference_Order_Number AS "Reference Order Number", 
  Estimated_Arrival AS "Expected Arrival", 
  InboundProductLists.Quantity, 
  Boxes, 
  Creation_Date AS "Creation Date", 
  Inbound_Type AS "Inbound Type", 
  Arrival_Method AS "Arrival Method"
FROM 
  InboundOrders 
INNER JOIN 
  Warehouse ON InboundOrders.Warehouse_ID = Warehouse.Warehouse_ID 
INNER JOIN 
  USERS ON InboundOrders.User_ID = USERS.UserID 
INNER JOIN 
  InboundProductLists ON InboundOrders.Inbound_Order_ID = InboundProductLists.OrderID;
GO
CREATE VIEW Parcel AS 
SELECT
	Order_ID AS "Order ID",
    USERS.Username AS "Creator",
    Warehouse.Warehouse,
    Order_Status AS "Status",
    Platform,
    Estimated_Delivery_Date AS "Expected Delivery",
    Ship_Date AS "Ship Date",
    Cost,
    ParcelOutbounds.Currency,
    Recipient,
    ParcelOutbounds.Country,
    Postcode,
    Tracking_Number AS "Tracking Number",
    Reference_Order_Number AS "Reference Order Number",
    Related_Adjustment_Order AS "Related Adjustment Order",
    Creation_Date AS "Creation Date",
    Boxes,
    Shipping_Company AS "Shipping Company",
    Latest_Information AS "Latest Information",
    Tracking_Update_Time AS "Tracking Update Time",
    Internet_Posting_Time AS "Internet Posting Time",
    Delivery_Time AS "Delivery Time"
FROM
	ParcelOutbounds
INNER JOIN
	Warehouse ON ParcelOutbounds.Warehouse_ID = Warehouse.Warehouse_ID
INNER JOIN
	USERS ON ParcelOutbounds.User_ID = USERS.UserID;
GO

CREATE VIEW Freight AS 
SELECT
	Outbound_Order_ID AS "Order ID",
    USERS.Username AS "Creator",
    Warehouse.Warehouse,
    Order_Status AS "Status",
    Cost,
    FreightOutbounds.Currency,
    Outbound_Method AS "Outbound Method",
    Related_Adjustment_Order AS "Related Adjustment Order",
    Tracking_Number AS "Tracking Number",
    Reference_Order_Number AS "Reference Order Number",
    Creation_Date AS "Creation Date",
    Estimated_Delivery_Date AS "Expected Delivery",
    Order_Ship_Date AS "Ship Date",
    Recipient,
    Recipient_Post_Code AS "Post Code",
    Destination_Type AS "Destination Type",
    Platform,
    Shipping_Company AS "Shipping Company",
    Transport_Days AS "Transport Days",
    FBA_Shipment_ID AS "FBA Shipment ID",
    FBA_Tracking_Number AS "FBA Tracking Number"
FROM
	FreightOutbounds
INNER JOIN
	Warehouse ON FReightOutbounds.Warehouse_ID = Warehouse.Warehouse_ID
INNER JOIN
	USERS ON FreightOutbounds.User_ID = USERS.UserID;
GO

CREATE VIEW Platform_Orders AS 
SELECT 
    Order_ID AS "Order ID", 
    Platform,
    Warehouse.Warehouse,
    Product_Quantity AS "Product Quantity",
    USERS.Username AS "Creator",
    Buyer,
    Recipient_Postcode AS "Recipient Postcode",
    Recipient_Country AS "Recipient Country",
    Store,
    Site,
    Shipping_Service AS "Shipping Service",
    Tracking_Number AS "Tracking Number",
    Carrier,
    Order_Time AS "Order Time",
    Payment_Time AS "Payment Time",
    Created_Time AS "Created Time",
    Order_Source AS "Order Source"
FROM 
    PlatformOrders 
INNER JOIN 
    Warehouse ON PlatformOrders.Warehouse_ID = Warehouse.Warehouse_ID 
INNER JOIN 
    USERS ON PlatformOrders.User_ID = USERS.UserID;
GO

CREATE VIEW User_List AS 
SELECT
	USERS.UserID AS "User ID",
    Username,
    Roles.Role AS "Role",
    Email,
    Password,
    DateCreated AS "Date Created"
FROM
	USERS
INNER JOIN
	UserRoles ON USERS.USerID = UserRoles.User_ID
INNER JOIN
	Roles ON UserRoles.Role_ID = Roles.Role_ID;
GO

CREATE VIEW Warehouses AS 
SELECT
	Warehouse_ID AS "Warehouse ID",
    Warehouse,
    Country,
    City,
    Currency
FROM
	Warehouse;
GO

CREATE VIEW Inventory_List AS 
SELECT
	Product_ID AS "Product ID",
    Product_Name AS "Product",
    SKU,
    Warehouse,
    Product_Description AS "Description"
FROM
	Inventories
INNER JOIN
	Warehouse ON Inventories.Warehouse_ID = Warehouse.Warehouse_ID;
GO
CREATE VIEW Customer_List AS
SELECT  
    Customer.UserID AS "Customer ID",
    Users.Username AS "Customer",
    Customer.Company_Name AS "Company",
    Customer.Account_Status AS "Account Status",
    Customer.Product_Need_Audit_Free AS "Product Need Audit Free",
   Customer.Date_Created AS "Date Created"
FROM  
    Customer
JOIN  
    USERS ON Customer.UserID = Users.UserID;
GO
    
CREATE VIEW Seller_List AS
SELECT
	USERS.UserID AS "User ID",
    Username,
    Email,
    Password,
    DateCreated AS "Date Created"
FROM
	USERS
INNER JOIN
	UserRoles ON USERS.UserID = UserRoles.User_ID
INNER JOIN
	Roles ON UserRoles.Role_ID = Roles.Role_ID
GO


CREATE VIEW Charge_List AS
SELECT 
    BillingAccounts.Billing_Account_ID AS "Billing Account ID",
    USERS.Username,
    USERS.Email,
    BillingAccounts.Account_Balance AS "Billing Account Balance",
    Charges.Charge_ID AS "Charge ID",
    Charges.Charge_Type AS "Charge Type",
    Charges.Description,
    Charges.Amount
FROM 
    BillingAccounts
JOIN 
    Billings ON BillingAccounts.Billing_Account_ID = Billings.Billing_Account_ID
JOIN 
    Charges ON Billings.Charge_ID = Charges.Charge_ID
JOIN 
    USERS ON BillingAccounts.User_ID = USERS.UserID;
GO

CREATE VIEW Billing_List AS
SELECT 
    USERS.Username AS "User",
    BillingAccounts.Billing_Account_ID AS "Billing Account ID",
    BillingAccounts.Account_Balance AS Account_Balance
FROM 
    BillingAccounts
JOIN 
    USERS ON BillingAccounts.User_ID = USERS.UserID
LEFT JOIN 
    Billings ON BillingAccounts.Billing_Account_ID = Billings.Billing_Account_ID
LEFT JOIN 
    Charges ON Billings.Charge_ID = Charges.Charge_ID;
    
GO
CREATE VIEW Inbound_Products AS
SELECT
	InboundOrders.Inbound_Order_ID,
    Inventories.Product_Name AS "Product" ,
    InboundProductLists.Quantity
FROM 
	InboundOrders
JOIN
	InboundProductLists ON InboundProductLists.OrderID = InboundOrders.Inbound_Order_ID
JOIN 
	Inventories ON Inventories.Product_ID=InboundProductLists.ProductID;
GO

CREATE VIEW Parcel_Products AS
SELECT
  ParcelOutbounds.Order_ID,
  Inventories.Product_Name AS "Product",
  ParcelProductLists.Quantity
FROM 
  ParcelOutbounds
JOIN 
  ParcelProductLists ON ParcelProductLists.Order_ID = ParcelOutbounds.Order_ID
JOIN 
  Inventories ON Inventories.Product_ID = ParcelProductLists.Product_ID;
GO

CREATE VIEW Freight_Products AS
SELECT
 FreightOutbounds.Outbound_Order_ID AS "Order ID", 
  Inventories.Product_Name AS "Product",
  FreightProductLists.Quantity
FROM 
  FreightOutbounds
JOIN 
  FreightProductLists ON FreightProductLists.Order_ID = FreightOutbounds.Outbound_Order_ID
JOIN 
  Inventories ON Inventories.Product_ID = FreightProductLists.Product_ID;

GO
CREATE VIEW Platform_Products AS
SELECT
  PlatformOrders.Order_ID,
  Inventories.Product_Name AS "Product",
  PlatformProductLists.Quantity
FROM 
  PlatformOrders
JOIN
  PlatformProductLists ON PlatformProductLists.Order_ID = PlatformOrders.Order_ID
JOIN 
  Inventories ON Inventories.Product_ID = PlatformProductLists.Product_ID;