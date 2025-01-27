-- MAKE TABLES SINGULAR -- 

BEGIN TRANSACTION

USE OMS;
GO


EXEC sp_rename 'dbo.BillingAccounts', 'BillingAccount';
EXEC sp_rename 'dbo.Billings', 'Billing';
EXEC sp_rename 'dbo.Charges', 'Charge';
EXEC sp_rename 'dbo.Customers', 'Customer';
EXEC sp_rename 'dbo.FreightOutbounds', 'FreightOutbound';
EXEC sp_rename 'dbo.FreightProductLists', 'FreightProductList';
EXEC sp_rename 'dbo.InboundOrders', 'InboundOrder';
EXEC sp_rename 'dbo.InboundProductLists', 'InboundProductList';
EXEC sp_rename 'dbo.Inventories', 'Inventory';
EXEC sp_rename 'dbo.OrderBasedBillings', 'OrderBasedBilling';
EXEC sp_rename 'dbo.OrderBasedCharges', 'OrderBasedCharge';
EXEC sp_rename 'dbo.OrderItems', 'OrderItem';
EXEC sp_rename 'dbo.Orders', 'Order';
EXEC sp_rename 'dbo.ParcelOutbounds', 'ParcelOutbound';
EXEC sp_rename 'dbo.ParcelProductLists', 'ParcelProductList';
EXEC sp_rename 'dbo.PlatformOrders', 'PlatformOrder';
EXEC sp_rename 'dbo.PlatformProductLists', 'PlatformProductList';
EXEC sp_rename 'dbo.Roles', 'Role';
EXEC sp_rename 'dbo.UserRoles', 'UserRole';

COMMIT TRANSACTION

BEGIN TRANSACTION

USE [OMS];
GO


EXEC sp_rename 'dbo.BillingAccounts', 'BillingAccount';
EXEC sp_rename 'dbo.Billings', 'Billing';
EXEC sp_rename 'dbo.Charges', 'Charge';
EXEC sp_rename 'dbo.Customers', 'Customer';
EXEC sp_rename 'dbo.FreightOutbounds', 'FreightOutbound';
EXEC sp_rename 'dbo.FreightProductLists', 'FreightProductList';
EXEC sp_rename 'dbo.InboundOrders', 'InboundOrder';
EXEC sp_rename 'dbo.InboundProductLists', 'InboundProductList';
EXEC sp_rename 'dbo.Inventories', 'Inventory';
EXEC sp_rename 'dbo.OrderBasedBillings', 'OrderBasedBilling';
EXEC sp_rename 'dbo.OrderBasedCharges', 'OrderBasedCharge';
EXEC sp_rename 'dbo.OrderItems', 'OrderItem';
EXEC sp_rename 'dbo.Orders', 'Order';
EXEC sp_rename 'dbo.ParcelOutbounds', 'ParcelOutbound';
EXEC sp_rename 'dbo.ParcelProductLists', 'ParcelProductList';
EXEC sp_rename 'dbo.PlatformOrders', 'PlatformOrder';
EXEC sp_rename 'dbo.PlatformProductLists', 'PlatformProductList';
EXEC sp_rename 'dbo.Roles', 'Role';
EXEC sp_rename 'dbo.UserRoles', 'UserRole';

COMMIT TRANSACTION