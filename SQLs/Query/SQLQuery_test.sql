select * from AdminUsers
select * from AddressMaster
select * from Vendors

select * from ReturnPolicyMaster
select * from Categories
select * from CategoryHSNMapping
select * from HSNMaster
select * from Products
select * from GSTMaster
select * from VariantSetCategories
select * from ProductSKUs
select * from ProductSKUImages
select * from ProductSKUValueSignature
select * from ProductSKUVariantValues

select * from VariantMaster --Varaint Like Color Size Memory Design Shape
select * from VariantSets   --Name of set which indicateds combination of variant and usage
select * from VariantInsideVariantSet   --In name of set which variant using
select * from VariantValues     --Value of Variant
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

exec sp_VendorWarehouse_GetById 1002

EXEC sp_AdminVariantSetCategories_Assign
    @VariantSetID = 1,
    @CategoryID = 2;
	
exec SP_Vendor_ProductSKU_Header  1
exec sp_VendorInventory_Summary 27, 0, '01/01/2026','01/26/2026'
exec sp_VendorWarehouses_Lookup 2
exec sp_SkuFilter_SkuContext 27,1

update categories set SharedMargin=2 