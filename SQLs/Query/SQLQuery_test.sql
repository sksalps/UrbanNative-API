select * from AdminUsers
select * from AddressMaster
select * from Vendors


select * from Categories
select * from CategoryHSNMapping
select * from Products
select * from VariantSetCategories
select * from ProductSKUs
select * from ProductSKUImages
select * from ProductSKUValueSignature
select * from ProductSKUVariantValues
select * from VariantInsideVariantSet
select * from VariantMaster
select * from VariantSets
select * from VariantValues
select * from InventoryLogs


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

EXEC sp_VendorWarehouses_List 1 ;
exec sp_VendorProducts_List 1, 'Men'
EXEC sp_ReturnImages_GetByReturn 1;
EXEC sp_AdminReturns_GetDetails 1
exec sp_LogisticsProviders_GetAll
exec sp_AdminReturnShipment_Create 1,1,'22423'
exec sp_Vendor_GetPassword 1002

EXEC sp_AdminVariantSetCategories_Assign
    @VariantSetID = 1,
    @CategoryID = 2;
	
exec sp_Vendor_ChangePassword  1002


update categories set SharedMargin=2 