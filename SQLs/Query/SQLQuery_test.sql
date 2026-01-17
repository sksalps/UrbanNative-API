select * from Products
select * from Vendors
select * from InventoryLogs
select * from VariantSetCategories
select * from ProductSKUs
select * from SystemSettingsMaster
select * from SystemSettingValues
select * from Orders
select * from OrderItems 
select * from OrderShipments
SELECT * FROM OrderShipmentDetails

select * from AddressMaster
select * from CountryMaster
select * from StateMaster
select * from CityMaster
select * from LogisticsProvider

select * from orderitemreturns
select * from OrderReturnShipments
select * from OrderItemReturnImages



EXEC sp_ReturnImages_GetByReturn 1;
EXEC sp_AdminReturns_GetDetails 1
exec sp_LogisticsProviders_GetAll
exec sp_AdminReturnShipment_Create 1,1,'22423'


EXEC sp_AdminVariantSetCategories_Assign
    @VariantSetID = 1,
    @CategoryID = 2;
	
