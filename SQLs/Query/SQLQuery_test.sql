select * from AdminUsers
select * from AddressMaster
select * from Vendors
select * from users

select * from ReturnPolicyMaster
select * from Categories
select * from CategoryHSNMapping
select * from HSNMaster
select * from Products
select * from ProductSKUs
select * from GSTMaster
select * from VariantSetCategories
select stock,* from ProductSKUs
select * from ProductSKUImages
select * from ProductSKUValueSignature
select * from ProductSKUVariantValues

select * from VariantMaster --Varaint Like Color Size Memory Design Shape
select * from VariantSets   --Name of set which indicateds combination of variant and usage
select * from VariantInsideVariantSet   --In name of set which variant using
select * from VariantValues     --Value of Variant
select * from InventoryLogs where skuid=27 order by logid desc

select * from SystemSettingsMaster
select * from SystemSettingValues
select * from Orders
select * from OrderItems  where orderid=2
select * from OrderShipments
SELECT * FROM OrderShipmentDetails
select * from ReturnPolicyMaster

select * from AddressMaster
select * from CountryMaster
select * from StateMaster
select * from CityMaster
select * from LogisticsProvider

select * from orderitemreturns
select * from OrderReturnShipments
select * from OrderItemReturnImages
EXEC sp_ProductInventorySkuGrid_Get 1,1, 1002;
exec sp_VendorProducts_List 1, 'Men'
EXEC sp_ReturnImages_GetByReturn 1;
EXEC sp_AdminReturns_GetDetails 1
exec sp_LogisticsProviders_GetAll
exec sp_AdminReturnShipment_Create 1,1,'22423'
exec sp_Vendor_GetPassword 1002
EXEC sp_helptext 'sp_VendorInventoryAdd_Product_IN';
exec sp_VendorWarehouses_Lookup
exec sp_VendorWarehouse_GetById 1002

EXEC sp_AdminVariantSetCategories_Assign  @VariantSetID = 1,    @CategoryID = 2;
select * from fn_ProductSkuVariantDisplay() where skuid=    21
exec sp_VendorInventory_Logs  27, 0, '01/01/2026','01/31/2026'
exec sp_VendorInventory_Summary 27, 0, '01/01/2026','02/26/2026'
exec sp_VendorWarehouses_Lookup 2
exec sp_SkuFilter_SkuContext 27,1
EXEC sp_SkuFilter_SKUs @ProductID = 1, @Search = 'Blue';
exec sp_VendorSkuAddInventory_Summary 1,1,25,1002
exec sp_VendorSkuProduct_Autocomplete 1,'P1-7'
exec sp_VendorOrders_List @VendorID=1, @SkuOrProduct ='P1-28', @SearchType='SKU'
EXEC sp_VendorOrder_ViewDetails 1004
asdfas
update ProductSKUs set IsActive=0 where SKUId=25