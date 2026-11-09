SET NOCOUNT ON;
DECLARE @progress_msg NVARCHAR(400);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'start' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  CAST(0 AS INT) AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  CAST(0 AS INT) AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'start', N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @tenant_code NVARCHAR(3) = N'{{TenantCode}}';
DECLARE @company_code NVARCHAR(4) = N'{{CompanyCode}}';
DECLARE @culture_code NVARCHAR(5) = N'{{CultureCode}}';
DECLARE @plant_code NVARCHAR(4) = N'{{PlantCode}}';
DECLARE @sync_user_id BIGINT = {{SyncUserId}};

DECLARE @batch_size INT = 0;
DECLARE @apply_chunk INT = 100000;
DECLARE @merge_from_rn INT;
DECLARE @merge_to_rn INT;
DECLARE @merge_max_rn INT;
DECLARE @dml_n INT;
DECLARE @now DATETIME = GETDATE();
DECLARE @base_id BIGINT = DATEDIFF_BIG(MICROSECOND, '1970-01-01', @now) * 1000;

-- 源表 / 目标表：列与实体 TaktGeneralMaterial / TaktMaterialDescription 一致
-- 源：{{SourceDatabase}}.dbo.takt_logistics_materials_general_material → 目标：当前租户库同名表（唯一键 Tenant+MaterialCode）
-- 说明：源库 general_material 可能含 culture_code，目标实体为 TaktTenantCoreEntityBase（无 culture_code），同步时忽略源 culture_code
-- 源：{{SourceDatabase}}.dbo.takt_logistics_materials_material_description → 当前租户库同名表（唯一键 Tenant+MaterialCode+CultureCode）
-- 工厂维见 sync_matplt.sql；本脚本不做 PP_SapMaterial / MaterialCode 折叠

-- ========== mat ==========
IF OBJECT_ID('tempdb..#mat_source') IS NOT NULL DROP TABLE #mat_source;
IF OBJECT_ID('tempdb..#mat_delta') IS NOT NULL DROP TABLE #mat_delta;
IF OBJECT_ID('tempdb..#mat_soft') IS NOT NULL DROP TABLE #mat_soft;

CREATE TABLE #mat_source (
  [rn] INT,
  [id] BIGINT,
  [material_code] NVARCHAR(20),
  [complete_maintenance_status] NVARCHAR(15),
  [maintenance_status] NVARCHAR(15),
  [client_deletion_flag] NVARCHAR(1),
  [material_type] NVARCHAR(4),
  [industry_sector] NVARCHAR(1),
  [material_group] NVARCHAR(9),
  [old_material_number] NVARCHAR(40),
  [base_unit] NVARCHAR(3),
  [order_unit] NVARCHAR(3),
  [document_number] NVARCHAR(22),
  [document_type] NVARCHAR(3),
  [document_version] NVARCHAR(2),
  [document_page_format] NVARCHAR(4),
  [document_change_number] NVARCHAR(6),
  [document_page_number] NVARCHAR(3),
  [document_sheet_count] NVARCHAR(3),
  [production_inspection_memo] NVARCHAR(18),
  [production_memo_page_format] NVARCHAR(4),
  [size_dimensions] NVARCHAR(32),
  [basic_material] NVARCHAR(48),
  [industry_standard_description] NVARCHAR(18),
  [laboratory_design_office] NVARCHAR(3),
  [purchasing_value_key] NVARCHAR(4),
  [gross_weight] DECIMAL(18,3),
  [net_weight] DECIMAL(18,3),
  [weight_unit] NVARCHAR(3),
  [volume] DECIMAL(18,3),
  [volume_unit] NVARCHAR(3),
  [container_requirements] NVARCHAR(2),
  [storage_conditions] NVARCHAR(2),
  [temperature_conditions] NVARCHAR(2),
  [low_level_code] NVARCHAR(3),
  [transportation_group] NVARCHAR(4),
  [hazardous_material_number] NVARCHAR(40),
  [division] NVARCHAR(2),
  [competitor] NVARCHAR(10),
  [european_article_number_obsolete] NVARCHAR(13),
  [gr_gi_slip_quantity] DECIMAL(18,3),
  [procurement_rule] NVARCHAR(1),
  [source_of_supply] NVARCHAR(1),
  [season_category] NVARCHAR(4),
  [label_type] NVARCHAR(2),
  [label_form] NVARCHAR(2),
  [deactivated_field] NVARCHAR(1),
  [international_article_number] NVARCHAR(18),
  [ean_category] NVARCHAR(2),
  [length] DECIMAL(18,3),
  [width] DECIMAL(18,3),
  [height] DECIMAL(18,3),
  [dimension_unit] NVARCHAR(3),
  [product_hierarchy] NVARCHAR(18),
  [stock_transfer_net_change_costing] NVARCHAR(1),
  [cad_indicator] NVARCHAR(1),
  [qm_in_procurement] NVARCHAR(1),
  [allowed_packaging_weight] DECIMAL(18,3),
  [allowed_packaging_weight_unit] NVARCHAR(3),
  [allowed_packaging_volume] DECIMAL(18,3),
  [allowed_packaging_volume_unit] NVARCHAR(3),
  [excess_weight_tolerance] DECIMAL(18,1),
  [excess_volume_tolerance] DECIMAL(18,1),
  [variable_purchase_order_unit] NVARCHAR(1),
  [revision_level_assigned] NVARCHAR(1),
  [configurable_material] NVARCHAR(1),
  [batch_management_required] NVARCHAR(1),
  [packaging_material_type] NVARCHAR(4),
  [maximum_level_by_volume] DECIMAL(18,0),
  [stacking_factor] INT,
  [packaging_material_group] NVARCHAR(4),
  [authorization_group] NVARCHAR(4),
  [valid_from_date] DATETIME,
  [valid_to_date] DATETIME,
  [season_year] NVARCHAR(4),
  [price_band_category] NVARCHAR(2),
  [empties_bill_of_material] NVARCHAR(1),
  [external_material_group] NVARCHAR(18),
  [cross_plant_configurable_material] NVARCHAR(40),
  [material_category] NVARCHAR(2),
  [co_product_indicator] NVARCHAR(1),
  [follow_up_material_indicator] NVARCHAR(1),
  [pricing_reference_material] NVARCHAR(40),
  [cross_plant_material_status] NVARCHAR(2),
  [cross_distribution_chain_status] NVARCHAR(2),
  [cross_plant_status_valid_from] DATETIME,
  [cross_distribution_status_valid_from] DATETIME,
  [tax_classification] NVARCHAR(1),
  [catalog_profile] NVARCHAR(9),
  [minimum_remaining_shelf_life] DECIMAL(18,0),
  [total_shelf_life] DECIMAL(18,0),
  [storage_percentage] DECIMAL(18,0),
  [content_unit] NVARCHAR(3),
  [net_contents] DECIMAL(18,3),
  [comparison_price_unit] DECIMAL(18,0),
  [labeling_material_grouping] NVARCHAR(18),
  [gross_contents] DECIMAL(18,3),
  [quantity_conversion_method] NVARCHAR(1),
  [internal_object_number] NVARCHAR(18),
  [environmentally_relevant] NVARCHAR(1),
  [product_allocation_procedure] NVARCHAR(18),
  [variant_pricing_profile] NVARCHAR(1),
  [discount_in_kind] NVARCHAR(1),
  [manufacturer_part_number] NVARCHAR(40),
  [manufacturer_number] NVARCHAR(10),
  [inventory_managed_material_number] NVARCHAR(40),
  [manufacturer_part_profile] NVARCHAR(4),
  [units_of_measure_usage] NVARCHAR(1),
  [season_rollout] NVARCHAR(2),
  [dangerous_goods_profile] NVARCHAR(3),
  [highly_viscous] NVARCHAR(1),
  [in_bulk_liquid] NVARCHAR(1),
  [serial_number_explicitness] NVARCHAR(1),
  [closed_packaging] NVARCHAR(1),
  [approved_batch_record_required] NVARCHAR(1),
  [effectivity_parameter_override] NVARCHAR(1),
  [material_completion_level] NVARCHAR(2),
  [shelf_life_period_indicator] NVARCHAR(1),
  [shelf_life_rounding_rule] NVARCHAR(1),
  [product_composition_on_packaging] NVARCHAR(1),
  [general_item_category_group] NVARCHAR(4),
  [logistical_variants] NVARCHAR(1),
  [material_locked] NVARCHAR(1),
  [configuration_management_relevant] NVARCHAR(1),
  [assortment_list_type] NVARCHAR(1),
  [expiration_date_type] NVARCHAR(1),
  [gtin_variant] NVARCHAR(2),
  [generic_material_number] NVARCHAR(40),
  [same_packing_reference_material] NVARCHAR(40),
  [global_data_sync_relevant] NVARCHAR(1),
  [acceptance_at_origin] NVARCHAR(1),
  [standard_hu_type] NVARCHAR(4),
  [pilferable] NVARCHAR(1),
  [warehouse_storage_condition] NVARCHAR(2),
  [warehouse_material_group] NVARCHAR(4),
  [handling_indicator] NVARCHAR(4),
  [hazardous_substances_relevant] NVARCHAR(1),
  [handling_unit_type] NVARCHAR(4),
  [variable_tare_weight] NVARCHAR(1),
  [maximum_allowed_capacity] DECIMAL(18,3),
  [overcapacity_tolerance] DECIMAL(18,1),
  [maximum_packing_length] DECIMAL(18,3),
  [maximum_packing_width] DECIMAL(18,3),
  [maximum_packing_height] DECIMAL(18,3),
  [maximum_packing_dimension_unit] NVARCHAR(3),
  [country_of_origin] NVARCHAR(3),
  [material_freight_group] NVARCHAR(8),
  [quarantine_period] DECIMAL(18,0),
  [quarantine_period_unit] NVARCHAR(3),
  [quality_inspection_group] NVARCHAR(4),
  [serial_number_profile] NVARCHAR(4),
  [form_name] NVARCHAR(30),
  [logistics_unit_of_measure] NVARCHAR(3),
  [catch_weight_material] NVARCHAR(1),
  [catch_weight_profile] NVARCHAR(2),
  [catch_weight_tolerance_group] NVARCHAR(9),
  [adjustment_profile] NVARCHAR(3),
  [intellectual_property_id] NVARCHAR(40),
  [variant_price_allowed] NVARCHAR(1),
  [medium] NVARCHAR(6),
  [physical_commodity] NVARCHAR(18),
  [animal_origin] NVARCHAR(1),
  [textile_composition_function] NVARCHAR(1),
  [segmentation_structure] NVARCHAR(4),
  [segmentation_strategy] NVARCHAR(8),
  [segmentation_status] NVARCHAR(1),
  [segmentation_scope] NVARCHAR(1),
  [segmentation_relevant] NVARCHAR(1),
  [anp_code] NVARCHAR(9),
  [fashion_attribute1] NVARCHAR(10),
  [fashion_attribute2] NVARCHAR(10),
  [fashion_attribute3] NVARCHAR(6),
  [season_usage_indicator] NVARCHAR(1),
  [season_active_in_inventory] NVARCHAR(1),
  [characteristic_conversion_id] NVARCHAR(2),
  [packaging_code] NVARCHAR(10),
  [dangerous_goods_packaging_status] NVARCHAR(10),
  [material_condition_management] NVARCHAR(1),
  [return_code] NVARCHAR(1),
  [return_to_logistics_level] NVARCHAR(1),
  [nato_item_identification_number] NVARCHAR(9),
  [fff_class] NVARCHAR(40),
  [supersession_chain_number] NVARCHAR(18),
  [seasonal_procurement_creation_status] NVARCHAR(2),
  [color_characteristic_internal_number] NVARCHAR(10),
  [main_size_characteristic_internal_number] NVARCHAR(10),
  [second_size_characteristic_internal_number] NVARCHAR(10),
  [color] NVARCHAR(18),
  [main_size] NVARCHAR(18),
  [second_size] NVARCHAR(18),
  [evaluation_characteristic_value] NVARCHAR(18),
  [care_code] NVARCHAR(16),
  [brand_id] NVARCHAR(4),
  [fiber_code1] NVARCHAR(3),
  [fiber_part1] NVARCHAR(3),
  [fiber_code2] NVARCHAR(3),
  [fiber_part2] NVARCHAR(3),
  [fiber_code3] NVARCHAR(3),
  [fiber_part3] NVARCHAR(3),
  [fiber_code4] NVARCHAR(3),
  [fiber_part4] NVARCHAR(3),
  [fiber_code5] NVARCHAR(3),
  [fiber_part5] NVARCHAR(3),
  [fashion_grade] NVARCHAR(4),
  [tenant_code] NVARCHAR(3),
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

CREATE TABLE #mat_delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  [material_code] NVARCHAR(40)
);

CREATE TABLE #mat_soft (
  [id] BIGINT,
  [material_code] NVARCHAR(40)
);

INSERT INTO #mat_source
SELECT
  S.rn,
  @base_id + 0 + S.rn,
  S.[material_code],
  S.[complete_maintenance_status],
  S.[maintenance_status],
  S.[client_deletion_flag],
  S.[material_type],
  S.[industry_sector],
  S.[material_group],
  S.[old_material_number],
  S.[base_unit],
  S.[order_unit],
  S.[document_number],
  S.[document_type],
  S.[document_version],
  S.[document_page_format],
  S.[document_change_number],
  S.[document_page_number],
  S.[document_sheet_count],
  S.[production_inspection_memo],
  S.[production_memo_page_format],
  S.[size_dimensions],
  S.[basic_material],
  S.[industry_standard_description],
  S.[laboratory_design_office],
  S.[purchasing_value_key],
  S.[gross_weight],
  S.[net_weight],
  S.[weight_unit],
  S.[volume],
  S.[volume_unit],
  S.[container_requirements],
  S.[storage_conditions],
  S.[temperature_conditions],
  S.[low_level_code],
  S.[transportation_group],
  S.[hazardous_material_number],
  S.[division],
  S.[competitor],
  S.[european_article_number_obsolete],
  S.[gr_gi_slip_quantity],
  S.[procurement_rule],
  S.[source_of_supply],
  S.[season_category],
  S.[label_type],
  S.[label_form],
  S.[deactivated_field],
  S.[international_article_number],
  S.[ean_category],
  S.[length],
  S.[width],
  S.[height],
  S.[dimension_unit],
  S.[product_hierarchy],
  S.[stock_transfer_net_change_costing],
  S.[cad_indicator],
  S.[qm_in_procurement],
  S.[allowed_packaging_weight],
  S.[allowed_packaging_weight_unit],
  S.[allowed_packaging_volume],
  S.[allowed_packaging_volume_unit],
  S.[excess_weight_tolerance],
  S.[excess_volume_tolerance],
  S.[variable_purchase_order_unit],
  S.[revision_level_assigned],
  S.[configurable_material],
  S.[batch_management_required],
  S.[packaging_material_type],
  S.[maximum_level_by_volume],
  S.[stacking_factor],
  S.[packaging_material_group],
  S.[authorization_group],
  S.[valid_from_date],
  S.[valid_to_date],
  S.[season_year],
  S.[price_band_category],
  S.[empties_bill_of_material],
  S.[external_material_group],
  S.[cross_plant_configurable_material],
  S.[material_category],
  S.[co_product_indicator],
  S.[follow_up_material_indicator],
  S.[pricing_reference_material],
  S.[cross_plant_material_status],
  S.[cross_distribution_chain_status],
  S.[cross_plant_status_valid_from],
  S.[cross_distribution_status_valid_from],
  S.[tax_classification],
  S.[catalog_profile],
  S.[minimum_remaining_shelf_life],
  S.[total_shelf_life],
  S.[storage_percentage],
  S.[content_unit],
  S.[net_contents],
  S.[comparison_price_unit],
  S.[labeling_material_grouping],
  S.[gross_contents],
  S.[quantity_conversion_method],
  S.[internal_object_number],
  S.[environmentally_relevant],
  S.[product_allocation_procedure],
  S.[variant_pricing_profile],
  S.[discount_in_kind],
  S.[manufacturer_part_number],
  S.[manufacturer_number],
  S.[inventory_managed_material_number],
  S.[manufacturer_part_profile],
  S.[units_of_measure_usage],
  S.[season_rollout],
  S.[dangerous_goods_profile],
  S.[highly_viscous],
  S.[in_bulk_liquid],
  S.[serial_number_explicitness],
  S.[closed_packaging],
  S.[approved_batch_record_required],
  S.[effectivity_parameter_override],
  S.[material_completion_level],
  S.[shelf_life_period_indicator],
  S.[shelf_life_rounding_rule],
  S.[product_composition_on_packaging],
  S.[general_item_category_group],
  S.[logistical_variants],
  S.[material_locked],
  S.[configuration_management_relevant],
  S.[assortment_list_type],
  S.[expiration_date_type],
  S.[gtin_variant],
  S.[generic_material_number],
  S.[same_packing_reference_material],
  S.[global_data_sync_relevant],
  S.[acceptance_at_origin],
  S.[standard_hu_type],
  S.[pilferable],
  S.[warehouse_storage_condition],
  S.[warehouse_material_group],
  S.[handling_indicator],
  S.[hazardous_substances_relevant],
  S.[handling_unit_type],
  S.[variable_tare_weight],
  S.[maximum_allowed_capacity],
  S.[overcapacity_tolerance],
  S.[maximum_packing_length],
  S.[maximum_packing_width],
  S.[maximum_packing_height],
  S.[maximum_packing_dimension_unit],
  S.[country_of_origin],
  S.[material_freight_group],
  S.[quarantine_period],
  S.[quarantine_period_unit],
  S.[quality_inspection_group],
  S.[serial_number_profile],
  S.[form_name],
  S.[logistics_unit_of_measure],
  S.[catch_weight_material],
  S.[catch_weight_profile],
  S.[catch_weight_tolerance_group],
  S.[adjustment_profile],
  S.[intellectual_property_id],
  S.[variant_price_allowed],
  S.[medium],
  S.[physical_commodity],
  S.[animal_origin],
  S.[textile_composition_function],
  S.[segmentation_structure],
  S.[segmentation_strategy],
  S.[segmentation_status],
  S.[segmentation_scope],
  S.[segmentation_relevant],
  S.[anp_code],
  S.[fashion_attribute1],
  S.[fashion_attribute2],
  S.[fashion_attribute3],
  S.[season_usage_indicator],
  S.[season_active_in_inventory],
  S.[characteristic_conversion_id],
  S.[packaging_code],
  S.[dangerous_goods_packaging_status],
  S.[material_condition_management],
  S.[return_code],
  S.[return_to_logistics_level],
  S.[nato_item_identification_number],
  S.[fff_class],
  S.[supersession_chain_number],
  S.[seasonal_procurement_creation_status],
  S.[color_characteristic_internal_number],
  S.[main_size_characteristic_internal_number],
  S.[second_size_characteristic_internal_number],
  S.[color],
  S.[main_size],
  S.[second_size],
  S.[evaluation_characteristic_value],
  S.[care_code],
  S.[brand_id],
  S.[fiber_code1],
  S.[fiber_part1],
  S.[fiber_code2],
  S.[fiber_part2],
  S.[fiber_code3],
  S.[fiber_part3],
  S.[fiber_code4],
  S.[fiber_part4],
  S.[fiber_code5],
  S.[fiber_part5],
  S.[fashion_grade],
  S.[tenant_code],
  S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[material_code]) AS rn
  FROM (
    SELECT
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_code])), 20), N''), N'') AS [material_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[complete_maintenance_status])), 15), N'') AS [complete_maintenance_status],
      NULLIF(LEFT(LTRIM(RTRIM(R.[maintenance_status])), 15), N'') AS [maintenance_status],
      NULLIF(LEFT(LTRIM(RTRIM(R.[client_deletion_flag])), 1), N'') AS [client_deletion_flag],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_type])), 4), N''), N'') AS [material_type],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[industry_sector])), 1), N''), N'') AS [industry_sector],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_group])), 9), N''), N'') AS [material_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[old_material_number])), 40), N'') AS [old_material_number],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[base_unit])), 3), N''), N'') AS [base_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[order_unit])), 3), N'') AS [order_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_number])), 22), N'') AS [document_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_type])), 3), N'') AS [document_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_version])), 2), N'') AS [document_version],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_page_format])), 4), N'') AS [document_page_format],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_change_number])), 6), N'') AS [document_change_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_page_number])), 3), N'') AS [document_page_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[document_sheet_count])), 3), N'') AS [document_sheet_count],
      NULLIF(LEFT(LTRIM(RTRIM(R.[production_inspection_memo])), 18), N'') AS [production_inspection_memo],
      NULLIF(LEFT(LTRIM(RTRIM(R.[production_memo_page_format])), 4), N'') AS [production_memo_page_format],
      NULLIF(LEFT(LTRIM(RTRIM(R.[size_dimensions])), 32), N'') AS [size_dimensions],
      NULLIF(LEFT(LTRIM(RTRIM(R.[basic_material])), 48), N'') AS [basic_material],
      NULLIF(LEFT(LTRIM(RTRIM(R.[industry_standard_description])), 18), N'') AS [industry_standard_description],
      NULLIF(LEFT(LTRIM(RTRIM(R.[laboratory_design_office])), 3), N'') AS [laboratory_design_office],
      NULLIF(LEFT(LTRIM(RTRIM(R.[purchasing_value_key])), 4), N'') AS [purchasing_value_key],
      ROUND(TRY_CAST(R.[gross_weight] AS DECIMAL(18,3)), 3) AS [gross_weight],
      ROUND(TRY_CAST(R.[net_weight] AS DECIMAL(18,3)), 3) AS [net_weight],
      NULLIF(LEFT(LTRIM(RTRIM(R.[weight_unit])), 3), N'') AS [weight_unit],
      ROUND(TRY_CAST(R.[volume] AS DECIMAL(18,3)), 3) AS [volume],
      NULLIF(LEFT(LTRIM(RTRIM(R.[volume_unit])), 3), N'') AS [volume_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[container_requirements])), 2), N'') AS [container_requirements],
      NULLIF(LEFT(LTRIM(RTRIM(R.[storage_conditions])), 2), N'') AS [storage_conditions],
      NULLIF(LEFT(LTRIM(RTRIM(R.[temperature_conditions])), 2), N'') AS [temperature_conditions],
      NULLIF(LEFT(LTRIM(RTRIM(R.[low_level_code])), 3), N'') AS [low_level_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[transportation_group])), 4), N'') AS [transportation_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[hazardous_material_number])), 40), N'') AS [hazardous_material_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[division])), 2), N'') AS [division],
      NULLIF(LEFT(LTRIM(RTRIM(R.[competitor])), 10), N'') AS [competitor],
      NULLIF(LEFT(LTRIM(RTRIM(R.[european_article_number_obsolete])), 13), N'') AS [european_article_number_obsolete],
      ROUND(TRY_CAST(R.[gr_gi_slip_quantity] AS DECIMAL(18,3)), 3) AS [gr_gi_slip_quantity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[procurement_rule])), 1), N'') AS [procurement_rule],
      NULLIF(LEFT(LTRIM(RTRIM(R.[source_of_supply])), 1), N'') AS [source_of_supply],
      NULLIF(LEFT(LTRIM(RTRIM(R.[season_category])), 4), N'') AS [season_category],
      NULLIF(LEFT(LTRIM(RTRIM(R.[label_type])), 2), N'') AS [label_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[label_form])), 2), N'') AS [label_form],
      NULLIF(LEFT(LTRIM(RTRIM(R.[deactivated_field])), 1), N'') AS [deactivated_field],
      NULLIF(LEFT(LTRIM(RTRIM(R.[international_article_number])), 18), N'') AS [international_article_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[ean_category])), 2), N'') AS [ean_category],
      ROUND(TRY_CAST(R.[length] AS DECIMAL(18,3)), 3) AS [length],
      ROUND(TRY_CAST(R.[width] AS DECIMAL(18,3)), 3) AS [width],
      ROUND(TRY_CAST(R.[height] AS DECIMAL(18,3)), 3) AS [height],
      NULLIF(LEFT(LTRIM(RTRIM(R.[dimension_unit])), 3), N'') AS [dimension_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[product_hierarchy])), 18), N'') AS [product_hierarchy],
      NULLIF(LEFT(LTRIM(RTRIM(R.[stock_transfer_net_change_costing])), 1), N'') AS [stock_transfer_net_change_costing],
      NULLIF(LEFT(LTRIM(RTRIM(R.[cad_indicator])), 1), N'') AS [cad_indicator],
      NULLIF(LEFT(LTRIM(RTRIM(R.[qm_in_procurement])), 1), N'') AS [qm_in_procurement],
      ROUND(TRY_CAST(R.[allowed_packaging_weight] AS DECIMAL(18,3)), 3) AS [allowed_packaging_weight],
      NULLIF(LEFT(LTRIM(RTRIM(R.[allowed_packaging_weight_unit])), 3), N'') AS [allowed_packaging_weight_unit],
      ROUND(TRY_CAST(R.[allowed_packaging_volume] AS DECIMAL(18,3)), 3) AS [allowed_packaging_volume],
      NULLIF(LEFT(LTRIM(RTRIM(R.[allowed_packaging_volume_unit])), 3), N'') AS [allowed_packaging_volume_unit],
      ROUND(TRY_CAST(R.[excess_weight_tolerance] AS DECIMAL(18,1)), 1) AS [excess_weight_tolerance],
      ROUND(TRY_CAST(R.[excess_volume_tolerance] AS DECIMAL(18,1)), 1) AS [excess_volume_tolerance],
      NULLIF(LEFT(LTRIM(RTRIM(R.[variable_purchase_order_unit])), 1), N'') AS [variable_purchase_order_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[revision_level_assigned])), 1), N'') AS [revision_level_assigned],
      NULLIF(LEFT(LTRIM(RTRIM(R.[configurable_material])), 1), N'') AS [configurable_material],
      NULLIF(LEFT(LTRIM(RTRIM(R.[batch_management_required])), 1), N'') AS [batch_management_required],
      NULLIF(LEFT(LTRIM(RTRIM(R.[packaging_material_type])), 4), N'') AS [packaging_material_type],
      TRY_CAST(R.[maximum_level_by_volume] AS DECIMAL(18,0)) AS [maximum_level_by_volume],
      TRY_CAST(R.[stacking_factor] AS INT) AS [stacking_factor],
      NULLIF(LEFT(LTRIM(RTRIM(R.[packaging_material_group])), 4), N'') AS [packaging_material_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[authorization_group])), 4), N'') AS [authorization_group],
      TRY_CAST(R.[valid_from_date] AS DATETIME) AS [valid_from_date],
      TRY_CAST(R.[valid_to_date] AS DATETIME) AS [valid_to_date],
      NULLIF(LEFT(LTRIM(RTRIM(R.[season_year])), 4), N'') AS [season_year],
      NULLIF(LEFT(LTRIM(RTRIM(R.[price_band_category])), 2), N'') AS [price_band_category],
      NULLIF(LEFT(LTRIM(RTRIM(R.[empties_bill_of_material])), 1), N'') AS [empties_bill_of_material],
      NULLIF(LEFT(LTRIM(RTRIM(R.[external_material_group])), 18), N'') AS [external_material_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[cross_plant_configurable_material])), 40), N'') AS [cross_plant_configurable_material],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_category])), 2), N'') AS [material_category],
      NULLIF(LEFT(LTRIM(RTRIM(R.[co_product_indicator])), 1), N'') AS [co_product_indicator],
      NULLIF(LEFT(LTRIM(RTRIM(R.[follow_up_material_indicator])), 1), N'') AS [follow_up_material_indicator],
      NULLIF(LEFT(LTRIM(RTRIM(R.[pricing_reference_material])), 40), N'') AS [pricing_reference_material],
      NULLIF(LEFT(LTRIM(RTRIM(R.[cross_plant_material_status])), 2), N'') AS [cross_plant_material_status],
      NULLIF(LEFT(LTRIM(RTRIM(R.[cross_distribution_chain_status])), 2), N'') AS [cross_distribution_chain_status],
      TRY_CAST(R.[cross_plant_status_valid_from] AS DATETIME) AS [cross_plant_status_valid_from],
      TRY_CAST(R.[cross_distribution_status_valid_from] AS DATETIME) AS [cross_distribution_status_valid_from],
      NULLIF(LEFT(LTRIM(RTRIM(R.[tax_classification])), 1), N'') AS [tax_classification],
      NULLIF(LEFT(LTRIM(RTRIM(R.[catalog_profile])), 9), N'') AS [catalog_profile],
      TRY_CAST(R.[minimum_remaining_shelf_life] AS DECIMAL(18,0)) AS [minimum_remaining_shelf_life],
      TRY_CAST(R.[total_shelf_life] AS DECIMAL(18,0)) AS [total_shelf_life],
      TRY_CAST(R.[storage_percentage] AS DECIMAL(18,0)) AS [storage_percentage],
      NULLIF(LEFT(LTRIM(RTRIM(R.[content_unit])), 3), N'') AS [content_unit],
      ROUND(TRY_CAST(R.[net_contents] AS DECIMAL(18,3)), 3) AS [net_contents],
      TRY_CAST(R.[comparison_price_unit] AS DECIMAL(18,0)) AS [comparison_price_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[labeling_material_grouping])), 18), N'') AS [labeling_material_grouping],
      ROUND(TRY_CAST(R.[gross_contents] AS DECIMAL(18,3)), 3) AS [gross_contents],
      NULLIF(LEFT(LTRIM(RTRIM(R.[quantity_conversion_method])), 1), N'') AS [quantity_conversion_method],
      NULLIF(LEFT(LTRIM(RTRIM(R.[internal_object_number])), 18), N'') AS [internal_object_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[environmentally_relevant])), 1), N'') AS [environmentally_relevant],
      NULLIF(LEFT(LTRIM(RTRIM(R.[product_allocation_procedure])), 18), N'') AS [product_allocation_procedure],
      NULLIF(LEFT(LTRIM(RTRIM(R.[variant_pricing_profile])), 1), N'') AS [variant_pricing_profile],
      NULLIF(LEFT(LTRIM(RTRIM(R.[discount_in_kind])), 1), N'') AS [discount_in_kind],
      NULLIF(LEFT(LTRIM(RTRIM(R.[manufacturer_part_number])), 40), N'') AS [manufacturer_part_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[manufacturer_number])), 10), N'') AS [manufacturer_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[inventory_managed_material_number])), 40), N'') AS [inventory_managed_material_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[manufacturer_part_profile])), 4), N'') AS [manufacturer_part_profile],
      NULLIF(LEFT(LTRIM(RTRIM(R.[units_of_measure_usage])), 1), N'') AS [units_of_measure_usage],
      NULLIF(LEFT(LTRIM(RTRIM(R.[season_rollout])), 2), N'') AS [season_rollout],
      NULLIF(LEFT(LTRIM(RTRIM(R.[dangerous_goods_profile])), 3), N'') AS [dangerous_goods_profile],
      NULLIF(LEFT(LTRIM(RTRIM(R.[highly_viscous])), 1), N'') AS [highly_viscous],
      NULLIF(LEFT(LTRIM(RTRIM(R.[in_bulk_liquid])), 1), N'') AS [in_bulk_liquid],
      NULLIF(LEFT(LTRIM(RTRIM(R.[serial_number_explicitness])), 1), N'') AS [serial_number_explicitness],
      NULLIF(LEFT(LTRIM(RTRIM(R.[closed_packaging])), 1), N'') AS [closed_packaging],
      NULLIF(LEFT(LTRIM(RTRIM(R.[approved_batch_record_required])), 1), N'') AS [approved_batch_record_required],
      NULLIF(LEFT(LTRIM(RTRIM(R.[effectivity_parameter_override])), 1), N'') AS [effectivity_parameter_override],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_completion_level])), 2), N'') AS [material_completion_level],
      NULLIF(LEFT(LTRIM(RTRIM(R.[shelf_life_period_indicator])), 1), N'') AS [shelf_life_period_indicator],
      NULLIF(LEFT(LTRIM(RTRIM(R.[shelf_life_rounding_rule])), 1), N'') AS [shelf_life_rounding_rule],
      NULLIF(LEFT(LTRIM(RTRIM(R.[product_composition_on_packaging])), 1), N'') AS [product_composition_on_packaging],
      NULLIF(LEFT(LTRIM(RTRIM(R.[general_item_category_group])), 4), N'') AS [general_item_category_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[logistical_variants])), 1), N'') AS [logistical_variants],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_locked])), 1), N'') AS [material_locked],
      NULLIF(LEFT(LTRIM(RTRIM(R.[configuration_management_relevant])), 1), N'') AS [configuration_management_relevant],
      NULLIF(LEFT(LTRIM(RTRIM(R.[assortment_list_type])), 1), N'') AS [assortment_list_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[expiration_date_type])), 1), N'') AS [expiration_date_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[gtin_variant])), 2), N'') AS [gtin_variant],
      NULLIF(LEFT(LTRIM(RTRIM(R.[generic_material_number])), 40), N'') AS [generic_material_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[same_packing_reference_material])), 40), N'') AS [same_packing_reference_material],
      NULLIF(LEFT(LTRIM(RTRIM(R.[global_data_sync_relevant])), 1), N'') AS [global_data_sync_relevant],
      NULLIF(LEFT(LTRIM(RTRIM(R.[acceptance_at_origin])), 1), N'') AS [acceptance_at_origin],
      NULLIF(LEFT(LTRIM(RTRIM(R.[standard_hu_type])), 4), N'') AS [standard_hu_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[pilferable])), 1), N'') AS [pilferable],
      NULLIF(LEFT(LTRIM(RTRIM(R.[warehouse_storage_condition])), 2), N'') AS [warehouse_storage_condition],
      NULLIF(LEFT(LTRIM(RTRIM(R.[warehouse_material_group])), 4), N'') AS [warehouse_material_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[handling_indicator])), 4), N'') AS [handling_indicator],
      NULLIF(LEFT(LTRIM(RTRIM(R.[hazardous_substances_relevant])), 1), N'') AS [hazardous_substances_relevant],
      NULLIF(LEFT(LTRIM(RTRIM(R.[handling_unit_type])), 4), N'') AS [handling_unit_type],
      NULLIF(LEFT(LTRIM(RTRIM(R.[variable_tare_weight])), 1), N'') AS [variable_tare_weight],
      ROUND(TRY_CAST(R.[maximum_allowed_capacity] AS DECIMAL(18,3)), 3) AS [maximum_allowed_capacity],
      ROUND(TRY_CAST(R.[overcapacity_tolerance] AS DECIMAL(18,1)), 1) AS [overcapacity_tolerance],
      ROUND(TRY_CAST(R.[maximum_packing_length] AS DECIMAL(18,3)), 3) AS [maximum_packing_length],
      ROUND(TRY_CAST(R.[maximum_packing_width] AS DECIMAL(18,3)), 3) AS [maximum_packing_width],
      ROUND(TRY_CAST(R.[maximum_packing_height] AS DECIMAL(18,3)), 3) AS [maximum_packing_height],
      NULLIF(LEFT(LTRIM(RTRIM(R.[maximum_packing_dimension_unit])), 3), N'') AS [maximum_packing_dimension_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[country_of_origin])), 3), N'') AS [country_of_origin],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_freight_group])), 8), N'') AS [material_freight_group],
      TRY_CAST(R.[quarantine_period] AS DECIMAL(18,0)) AS [quarantine_period],
      NULLIF(LEFT(LTRIM(RTRIM(R.[quarantine_period_unit])), 3), N'') AS [quarantine_period_unit],
      NULLIF(LEFT(LTRIM(RTRIM(R.[quality_inspection_group])), 4), N'') AS [quality_inspection_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[serial_number_profile])), 4), N'') AS [serial_number_profile],
      NULLIF(LEFT(LTRIM(RTRIM(R.[form_name])), 30), N'') AS [form_name],
      NULLIF(LEFT(LTRIM(RTRIM(R.[logistics_unit_of_measure])), 3), N'') AS [logistics_unit_of_measure],
      NULLIF(LEFT(LTRIM(RTRIM(R.[catch_weight_material])), 1), N'') AS [catch_weight_material],
      NULLIF(LEFT(LTRIM(RTRIM(R.[catch_weight_profile])), 2), N'') AS [catch_weight_profile],
      NULLIF(LEFT(LTRIM(RTRIM(R.[catch_weight_tolerance_group])), 9), N'') AS [catch_weight_tolerance_group],
      NULLIF(LEFT(LTRIM(RTRIM(R.[adjustment_profile])), 3), N'') AS [adjustment_profile],
      NULLIF(LEFT(LTRIM(RTRIM(R.[intellectual_property_id])), 40), N'') AS [intellectual_property_id],
      NULLIF(LEFT(LTRIM(RTRIM(R.[variant_price_allowed])), 1), N'') AS [variant_price_allowed],
      NULLIF(LEFT(LTRIM(RTRIM(R.[medium])), 6), N'') AS [medium],
      NULLIF(LEFT(LTRIM(RTRIM(R.[physical_commodity])), 18), N'') AS [physical_commodity],
      NULLIF(LEFT(LTRIM(RTRIM(R.[animal_origin])), 1), N'') AS [animal_origin],
      NULLIF(LEFT(LTRIM(RTRIM(R.[textile_composition_function])), 1), N'') AS [textile_composition_function],
      NULLIF(LEFT(LTRIM(RTRIM(R.[segmentation_structure])), 4), N'') AS [segmentation_structure],
      NULLIF(LEFT(LTRIM(RTRIM(R.[segmentation_strategy])), 8), N'') AS [segmentation_strategy],
      NULLIF(LEFT(LTRIM(RTRIM(R.[segmentation_status])), 1), N'') AS [segmentation_status],
      NULLIF(LEFT(LTRIM(RTRIM(R.[segmentation_scope])), 1), N'') AS [segmentation_scope],
      NULLIF(LEFT(LTRIM(RTRIM(R.[segmentation_relevant])), 1), N'') AS [segmentation_relevant],
      NULLIF(LEFT(LTRIM(RTRIM(R.[anp_code])), 9), N'') AS [anp_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fashion_attribute1])), 10), N'') AS [fashion_attribute1],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fashion_attribute2])), 10), N'') AS [fashion_attribute2],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fashion_attribute3])), 6), N'') AS [fashion_attribute3],
      NULLIF(LEFT(LTRIM(RTRIM(R.[season_usage_indicator])), 1), N'') AS [season_usage_indicator],
      NULLIF(LEFT(LTRIM(RTRIM(R.[season_active_in_inventory])), 1), N'') AS [season_active_in_inventory],
      NULLIF(LEFT(LTRIM(RTRIM(R.[characteristic_conversion_id])), 2), N'') AS [characteristic_conversion_id],
      NULLIF(LEFT(LTRIM(RTRIM(R.[packaging_code])), 10), N'') AS [packaging_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[dangerous_goods_packaging_status])), 10), N'') AS [dangerous_goods_packaging_status],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_condition_management])), 1), N'') AS [material_condition_management],
      NULLIF(LEFT(LTRIM(RTRIM(R.[return_code])), 1), N'') AS [return_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[return_to_logistics_level])), 1), N'') AS [return_to_logistics_level],
      NULLIF(LEFT(LTRIM(RTRIM(R.[nato_item_identification_number])), 9), N'') AS [nato_item_identification_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fff_class])), 40), N'') AS [fff_class],
      NULLIF(LEFT(LTRIM(RTRIM(R.[supersession_chain_number])), 18), N'') AS [supersession_chain_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[seasonal_procurement_creation_status])), 2), N'') AS [seasonal_procurement_creation_status],
      NULLIF(LEFT(LTRIM(RTRIM(R.[color_characteristic_internal_number])), 10), N'') AS [color_characteristic_internal_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[main_size_characteristic_internal_number])), 10), N'') AS [main_size_characteristic_internal_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[second_size_characteristic_internal_number])), 10), N'') AS [second_size_characteristic_internal_number],
      NULLIF(LEFT(LTRIM(RTRIM(R.[color])), 18), N'') AS [color],
      NULLIF(LEFT(LTRIM(RTRIM(R.[main_size])), 18), N'') AS [main_size],
      NULLIF(LEFT(LTRIM(RTRIM(R.[second_size])), 18), N'') AS [second_size],
      NULLIF(LEFT(LTRIM(RTRIM(R.[evaluation_characteristic_value])), 18), N'') AS [evaluation_characteristic_value],
      NULLIF(LEFT(LTRIM(RTRIM(R.[care_code])), 16), N'') AS [care_code],
      NULLIF(LEFT(LTRIM(RTRIM(R.[brand_id])), 4), N'') AS [brand_id],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fiber_code1])), 3), N'') AS [fiber_code1],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fiber_part1])), 3), N'') AS [fiber_part1],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fiber_code2])), 3), N'') AS [fiber_code2],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fiber_part2])), 3), N'') AS [fiber_part2],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fiber_code3])), 3), N'') AS [fiber_code3],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fiber_part3])), 3), N'') AS [fiber_part3],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fiber_code4])), 3), N'') AS [fiber_code4],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fiber_part4])), 3), N'') AS [fiber_part4],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fiber_code5])), 3), N'') AS [fiber_code5],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fiber_part5])), 3), N'') AS [fiber_part5],
      NULLIF(LEFT(LTRIM(RTRIM(R.[fashion_grade])), 4), N'') AS [fashion_grade],
      ISNULL(R.[ext_field], N'{}') AS [ext_field],
      ISNULL(R.[remark], N'') AS [remark],
      COALESCE(TRY_CAST(R.[created_by] AS BIGINT), 0) AS [created_by],
      R.[created_at] AS [created_at],
      TRY_CAST(R.[updated_by] AS BIGINT) AS [updated_by],
      R.[updated_at] AS [updated_at],
      TRY_CAST(R.[deleted_by] AS BIGINT) AS [deleted_by],
      R.[deleted_at] AS [deleted_at],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
            ROW_NUMBER() OVER (
        PARTITION BY LTRIM(RTRIM(R.[material_code]))
        ORDER BY LTRIM(RTRIM(R.[material_code]))
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_general_material] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @mat_source_count INT = (SELECT COUNT(*) FROM #mat_source);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @mat_source_count AS [to_rn],
  @mat_source_count AS [max_rn],
  @mat_source_count AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@mat_source_count) AS NVARCHAR(20)), N'|',
  CAST((@mat_source_count) AS NVARCHAR(20)), N'|',
  CAST((@mat_source_count) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @mat_sap_raw INT = (
  SELECT COUNT(*)
  FROM (
    SELECT LTRIM(RTRIM(R.[material_code])) AS [material_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_general_material] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
    GROUP BY LTRIM(RTRIM(R.[material_code]))
  ) K
);

IF @mat_source_count <> @mat_sap_raw
BEGIN
  DECLARE @mat_src_msg NVARCHAR(200) = CONCAT(
    N'mat 源键与装入不一致: keys=', @mat_sap_raw, N', loaded=', @mat_source_count);
  THROW 50003, @mat_src_msg, 1;
END;

IF EXISTS (
  SELECT 1 FROM #mat_source
  GROUP BY [material_code]
  HAVING COUNT(*) > 1
)
BEGIN
  THROW 50001, N'mat 装入后业务键重复', 1;
END;

UPDATE S
SET S.[id] = COALESCE(T.[id], S.[id])
FROM #mat_source S
LEFT JOIN [takt_logistics_materials_general_material] T
  ON T.[tenant_code] = @tenant_code
 AND LTRIM(RTRIM(T.[material_code])) = S.[material_code];

DECLARE @mat_target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_general_material]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #mat_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_materials_general_material] AS T
USING (SELECT * FROM #mat_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = S.[tenant_code]
AND LTRIM(RTRIM(T.[material_code])) = S.[material_code]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR EXISTS (
    SELECT
      S.[complete_maintenance_status],
      S.[maintenance_status],
      S.[client_deletion_flag],
      S.[material_type],
      S.[industry_sector],
      S.[material_group],
      S.[old_material_number],
      S.[base_unit],
      S.[order_unit],
      S.[document_number],
      S.[document_type],
      S.[document_version],
      S.[document_page_format],
      S.[document_change_number],
      S.[document_page_number],
      S.[document_sheet_count],
      S.[production_inspection_memo],
      S.[production_memo_page_format],
      S.[size_dimensions],
      S.[basic_material],
      S.[industry_standard_description],
      S.[laboratory_design_office],
      S.[purchasing_value_key],
      S.[gross_weight],
      S.[net_weight],
      S.[weight_unit],
      S.[volume],
      S.[volume_unit],
      S.[container_requirements],
      S.[storage_conditions],
      S.[temperature_conditions],
      S.[low_level_code],
      S.[transportation_group],
      S.[hazardous_material_number],
      S.[division],
      S.[competitor],
      S.[european_article_number_obsolete],
      S.[gr_gi_slip_quantity],
      S.[procurement_rule],
      S.[source_of_supply],
      S.[season_category],
      S.[label_type],
      S.[label_form],
      S.[deactivated_field],
      S.[international_article_number],
      S.[ean_category],
      S.[length],
      S.[width],
      S.[height],
      S.[dimension_unit],
      S.[product_hierarchy],
      S.[stock_transfer_net_change_costing],
      S.[cad_indicator],
      S.[qm_in_procurement],
      S.[allowed_packaging_weight],
      S.[allowed_packaging_weight_unit],
      S.[allowed_packaging_volume],
      S.[allowed_packaging_volume_unit],
      S.[excess_weight_tolerance],
      S.[excess_volume_tolerance],
      S.[variable_purchase_order_unit],
      S.[revision_level_assigned],
      S.[configurable_material],
      S.[batch_management_required],
      S.[packaging_material_type],
      S.[maximum_level_by_volume],
      S.[stacking_factor],
      S.[packaging_material_group],
      S.[authorization_group],
      S.[valid_from_date],
      S.[valid_to_date],
      S.[season_year],
      S.[price_band_category],
      S.[empties_bill_of_material],
      S.[external_material_group],
      S.[cross_plant_configurable_material],
      S.[material_category],
      S.[co_product_indicator],
      S.[follow_up_material_indicator],
      S.[pricing_reference_material],
      S.[cross_plant_material_status],
      S.[cross_distribution_chain_status],
      S.[cross_plant_status_valid_from],
      S.[cross_distribution_status_valid_from],
      S.[tax_classification],
      S.[catalog_profile],
      S.[minimum_remaining_shelf_life],
      S.[total_shelf_life],
      S.[storage_percentage],
      S.[content_unit],
      S.[net_contents],
      S.[comparison_price_unit],
      S.[labeling_material_grouping],
      S.[gross_contents],
      S.[quantity_conversion_method],
      S.[internal_object_number],
      S.[environmentally_relevant],
      S.[product_allocation_procedure],
      S.[variant_pricing_profile],
      S.[discount_in_kind],
      S.[manufacturer_part_number],
      S.[manufacturer_number],
      S.[inventory_managed_material_number],
      S.[manufacturer_part_profile],
      S.[units_of_measure_usage],
      S.[season_rollout],
      S.[dangerous_goods_profile],
      S.[highly_viscous],
      S.[in_bulk_liquid],
      S.[serial_number_explicitness],
      S.[closed_packaging],
      S.[approved_batch_record_required],
      S.[effectivity_parameter_override],
      S.[material_completion_level],
      S.[shelf_life_period_indicator],
      S.[shelf_life_rounding_rule],
      S.[product_composition_on_packaging],
      S.[general_item_category_group],
      S.[logistical_variants],
      S.[material_locked],
      S.[configuration_management_relevant],
      S.[assortment_list_type],
      S.[expiration_date_type],
      S.[gtin_variant],
      S.[generic_material_number],
      S.[same_packing_reference_material],
      S.[global_data_sync_relevant],
      S.[acceptance_at_origin],
      S.[standard_hu_type],
      S.[pilferable],
      S.[warehouse_storage_condition],
      S.[warehouse_material_group],
      S.[handling_indicator],
      S.[hazardous_substances_relevant],
      S.[handling_unit_type],
      S.[variable_tare_weight],
      S.[maximum_allowed_capacity],
      S.[overcapacity_tolerance],
      S.[maximum_packing_length],
      S.[maximum_packing_width],
      S.[maximum_packing_height],
      S.[maximum_packing_dimension_unit],
      S.[country_of_origin],
      S.[material_freight_group],
      S.[quarantine_period],
      S.[quarantine_period_unit],
      S.[quality_inspection_group],
      S.[serial_number_profile],
      S.[form_name],
      S.[logistics_unit_of_measure],
      S.[catch_weight_material],
      S.[catch_weight_profile],
      S.[catch_weight_tolerance_group],
      S.[adjustment_profile],
      S.[intellectual_property_id],
      S.[variant_price_allowed],
      S.[medium],
      S.[physical_commodity],
      S.[animal_origin],
      S.[textile_composition_function],
      S.[segmentation_structure],
      S.[segmentation_strategy],
      S.[segmentation_status],
      S.[segmentation_scope],
      S.[segmentation_relevant],
      S.[anp_code],
      S.[fashion_attribute1],
      S.[fashion_attribute2],
      S.[fashion_attribute3],
      S.[season_usage_indicator],
      S.[season_active_in_inventory],
      S.[characteristic_conversion_id],
      S.[packaging_code],
      S.[dangerous_goods_packaging_status],
      S.[material_condition_management],
      S.[return_code],
      S.[return_to_logistics_level],
      S.[nato_item_identification_number],
      S.[fff_class],
      S.[supersession_chain_number],
      S.[seasonal_procurement_creation_status],
      S.[color_characteristic_internal_number],
      S.[main_size_characteristic_internal_number],
      S.[second_size_characteristic_internal_number],
      S.[color],
      S.[main_size],
      S.[second_size],
      S.[evaluation_characteristic_value],
      S.[care_code],
      S.[brand_id],
      S.[fiber_code1],
      S.[fiber_part1],
      S.[fiber_code2],
      S.[fiber_part2],
      S.[fiber_code3],
      S.[fiber_part3],
      S.[fiber_code4],
      S.[fiber_part4],
      S.[fiber_code5],
      S.[fiber_part5],
      S.[fashion_grade]
    EXCEPT
    SELECT
      T.[complete_maintenance_status],
      T.[maintenance_status],
      T.[client_deletion_flag],
      T.[material_type],
      T.[industry_sector],
      T.[material_group],
      T.[old_material_number],
      T.[base_unit],
      T.[order_unit],
      T.[document_number],
      T.[document_type],
      T.[document_version],
      T.[document_page_format],
      T.[document_change_number],
      T.[document_page_number],
      T.[document_sheet_count],
      T.[production_inspection_memo],
      T.[production_memo_page_format],
      T.[size_dimensions],
      T.[basic_material],
      T.[industry_standard_description],
      T.[laboratory_design_office],
      T.[purchasing_value_key],
      T.[gross_weight],
      T.[net_weight],
      T.[weight_unit],
      T.[volume],
      T.[volume_unit],
      T.[container_requirements],
      T.[storage_conditions],
      T.[temperature_conditions],
      T.[low_level_code],
      T.[transportation_group],
      T.[hazardous_material_number],
      T.[division],
      T.[competitor],
      T.[european_article_number_obsolete],
      T.[gr_gi_slip_quantity],
      T.[procurement_rule],
      T.[source_of_supply],
      T.[season_category],
      T.[label_type],
      T.[label_form],
      T.[deactivated_field],
      T.[international_article_number],
      T.[ean_category],
      T.[length],
      T.[width],
      T.[height],
      T.[dimension_unit],
      T.[product_hierarchy],
      T.[stock_transfer_net_change_costing],
      T.[cad_indicator],
      T.[qm_in_procurement],
      T.[allowed_packaging_weight],
      T.[allowed_packaging_weight_unit],
      T.[allowed_packaging_volume],
      T.[allowed_packaging_volume_unit],
      T.[excess_weight_tolerance],
      T.[excess_volume_tolerance],
      T.[variable_purchase_order_unit],
      T.[revision_level_assigned],
      T.[configurable_material],
      T.[batch_management_required],
      T.[packaging_material_type],
      T.[maximum_level_by_volume],
      T.[stacking_factor],
      T.[packaging_material_group],
      T.[authorization_group],
      T.[valid_from_date],
      T.[valid_to_date],
      T.[season_year],
      T.[price_band_category],
      T.[empties_bill_of_material],
      T.[external_material_group],
      T.[cross_plant_configurable_material],
      T.[material_category],
      T.[co_product_indicator],
      T.[follow_up_material_indicator],
      T.[pricing_reference_material],
      T.[cross_plant_material_status],
      T.[cross_distribution_chain_status],
      T.[cross_plant_status_valid_from],
      T.[cross_distribution_status_valid_from],
      T.[tax_classification],
      T.[catalog_profile],
      T.[minimum_remaining_shelf_life],
      T.[total_shelf_life],
      T.[storage_percentage],
      T.[content_unit],
      T.[net_contents],
      T.[comparison_price_unit],
      T.[labeling_material_grouping],
      T.[gross_contents],
      T.[quantity_conversion_method],
      T.[internal_object_number],
      T.[environmentally_relevant],
      T.[product_allocation_procedure],
      T.[variant_pricing_profile],
      T.[discount_in_kind],
      T.[manufacturer_part_number],
      T.[manufacturer_number],
      T.[inventory_managed_material_number],
      T.[manufacturer_part_profile],
      T.[units_of_measure_usage],
      T.[season_rollout],
      T.[dangerous_goods_profile],
      T.[highly_viscous],
      T.[in_bulk_liquid],
      T.[serial_number_explicitness],
      T.[closed_packaging],
      T.[approved_batch_record_required],
      T.[effectivity_parameter_override],
      T.[material_completion_level],
      T.[shelf_life_period_indicator],
      T.[shelf_life_rounding_rule],
      T.[product_composition_on_packaging],
      T.[general_item_category_group],
      T.[logistical_variants],
      T.[material_locked],
      T.[configuration_management_relevant],
      T.[assortment_list_type],
      T.[expiration_date_type],
      T.[gtin_variant],
      T.[generic_material_number],
      T.[same_packing_reference_material],
      T.[global_data_sync_relevant],
      T.[acceptance_at_origin],
      T.[standard_hu_type],
      T.[pilferable],
      T.[warehouse_storage_condition],
      T.[warehouse_material_group],
      T.[handling_indicator],
      T.[hazardous_substances_relevant],
      T.[handling_unit_type],
      T.[variable_tare_weight],
      T.[maximum_allowed_capacity],
      T.[overcapacity_tolerance],
      T.[maximum_packing_length],
      T.[maximum_packing_width],
      T.[maximum_packing_height],
      T.[maximum_packing_dimension_unit],
      T.[country_of_origin],
      T.[material_freight_group],
      T.[quarantine_period],
      T.[quarantine_period_unit],
      T.[quality_inspection_group],
      T.[serial_number_profile],
      T.[form_name],
      T.[logistics_unit_of_measure],
      T.[catch_weight_material],
      T.[catch_weight_profile],
      T.[catch_weight_tolerance_group],
      T.[adjustment_profile],
      T.[intellectual_property_id],
      T.[variant_price_allowed],
      T.[medium],
      T.[physical_commodity],
      T.[animal_origin],
      T.[textile_composition_function],
      T.[segmentation_structure],
      T.[segmentation_strategy],
      T.[segmentation_status],
      T.[segmentation_scope],
      T.[segmentation_relevant],
      T.[anp_code],
      T.[fashion_attribute1],
      T.[fashion_attribute2],
      T.[fashion_attribute3],
      T.[season_usage_indicator],
      T.[season_active_in_inventory],
      T.[characteristic_conversion_id],
      T.[packaging_code],
      T.[dangerous_goods_packaging_status],
      T.[material_condition_management],
      T.[return_code],
      T.[return_to_logistics_level],
      T.[nato_item_identification_number],
      T.[fff_class],
      T.[supersession_chain_number],
      T.[seasonal_procurement_creation_status],
      T.[color_characteristic_internal_number],
      T.[main_size_characteristic_internal_number],
      T.[second_size_characteristic_internal_number],
      T.[color],
      T.[main_size],
      T.[second_size],
      T.[evaluation_characteristic_value],
      T.[care_code],
      T.[brand_id],
      T.[fiber_code1],
      T.[fiber_part1],
      T.[fiber_code2],
      T.[fiber_part2],
      T.[fiber_code3],
      T.[fiber_part3],
      T.[fiber_code4],
      T.[fiber_part4],
      T.[fiber_code5],
      T.[fiber_part5],
      T.[fashion_grade]
  )

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN
  UPDATE SET
  T.[complete_maintenance_status]=S.[complete_maintenance_status],
  T.[maintenance_status]=S.[maintenance_status],
  T.[client_deletion_flag]=S.[client_deletion_flag],
  T.[material_type]=S.[material_type],
  T.[industry_sector]=S.[industry_sector],
  T.[material_group]=S.[material_group],
  T.[old_material_number]=S.[old_material_number],
  T.[base_unit]=S.[base_unit],
  T.[order_unit]=S.[order_unit],
  T.[document_number]=S.[document_number],
  T.[document_type]=S.[document_type],
  T.[document_version]=S.[document_version],
  T.[document_page_format]=S.[document_page_format],
  T.[document_change_number]=S.[document_change_number],
  T.[document_page_number]=S.[document_page_number],
  T.[document_sheet_count]=S.[document_sheet_count],
  T.[production_inspection_memo]=S.[production_inspection_memo],
  T.[production_memo_page_format]=S.[production_memo_page_format],
  T.[size_dimensions]=S.[size_dimensions],
  T.[basic_material]=S.[basic_material],
  T.[industry_standard_description]=S.[industry_standard_description],
  T.[laboratory_design_office]=S.[laboratory_design_office],
  T.[purchasing_value_key]=S.[purchasing_value_key],
  T.[gross_weight]=S.[gross_weight],
  T.[net_weight]=S.[net_weight],
  T.[weight_unit]=S.[weight_unit],
  T.[volume]=S.[volume],
  T.[volume_unit]=S.[volume_unit],
  T.[container_requirements]=S.[container_requirements],
  T.[storage_conditions]=S.[storage_conditions],
  T.[temperature_conditions]=S.[temperature_conditions],
  T.[low_level_code]=S.[low_level_code],
  T.[transportation_group]=S.[transportation_group],
  T.[hazardous_material_number]=S.[hazardous_material_number],
  T.[division]=S.[division],
  T.[competitor]=S.[competitor],
  T.[european_article_number_obsolete]=S.[european_article_number_obsolete],
  T.[gr_gi_slip_quantity]=S.[gr_gi_slip_quantity],
  T.[procurement_rule]=S.[procurement_rule],
  T.[source_of_supply]=S.[source_of_supply],
  T.[season_category]=S.[season_category],
  T.[label_type]=S.[label_type],
  T.[label_form]=S.[label_form],
  T.[deactivated_field]=S.[deactivated_field],
  T.[international_article_number]=S.[international_article_number],
  T.[ean_category]=S.[ean_category],
  T.[length]=S.[length],
  T.[width]=S.[width],
  T.[height]=S.[height],
  T.[dimension_unit]=S.[dimension_unit],
  T.[product_hierarchy]=S.[product_hierarchy],
  T.[stock_transfer_net_change_costing]=S.[stock_transfer_net_change_costing],
  T.[cad_indicator]=S.[cad_indicator],
  T.[qm_in_procurement]=S.[qm_in_procurement],
  T.[allowed_packaging_weight]=S.[allowed_packaging_weight],
  T.[allowed_packaging_weight_unit]=S.[allowed_packaging_weight_unit],
  T.[allowed_packaging_volume]=S.[allowed_packaging_volume],
  T.[allowed_packaging_volume_unit]=S.[allowed_packaging_volume_unit],
  T.[excess_weight_tolerance]=S.[excess_weight_tolerance],
  T.[excess_volume_tolerance]=S.[excess_volume_tolerance],
  T.[variable_purchase_order_unit]=S.[variable_purchase_order_unit],
  T.[revision_level_assigned]=S.[revision_level_assigned],
  T.[configurable_material]=S.[configurable_material],
  T.[batch_management_required]=S.[batch_management_required],
  T.[packaging_material_type]=S.[packaging_material_type],
  T.[maximum_level_by_volume]=S.[maximum_level_by_volume],
  T.[stacking_factor]=S.[stacking_factor],
  T.[packaging_material_group]=S.[packaging_material_group],
  T.[authorization_group]=S.[authorization_group],
  T.[valid_from_date]=S.[valid_from_date],
  T.[valid_to_date]=S.[valid_to_date],
  T.[season_year]=S.[season_year],
  T.[price_band_category]=S.[price_band_category],
  T.[empties_bill_of_material]=S.[empties_bill_of_material],
  T.[external_material_group]=S.[external_material_group],
  T.[cross_plant_configurable_material]=S.[cross_plant_configurable_material],
  T.[material_category]=S.[material_category],
  T.[co_product_indicator]=S.[co_product_indicator],
  T.[follow_up_material_indicator]=S.[follow_up_material_indicator],
  T.[pricing_reference_material]=S.[pricing_reference_material],
  T.[cross_plant_material_status]=S.[cross_plant_material_status],
  T.[cross_distribution_chain_status]=S.[cross_distribution_chain_status],
  T.[cross_plant_status_valid_from]=S.[cross_plant_status_valid_from],
  T.[cross_distribution_status_valid_from]=S.[cross_distribution_status_valid_from],
  T.[tax_classification]=S.[tax_classification],
  T.[catalog_profile]=S.[catalog_profile],
  T.[minimum_remaining_shelf_life]=S.[minimum_remaining_shelf_life],
  T.[total_shelf_life]=S.[total_shelf_life],
  T.[storage_percentage]=S.[storage_percentage],
  T.[content_unit]=S.[content_unit],
  T.[net_contents]=S.[net_contents],
  T.[comparison_price_unit]=S.[comparison_price_unit],
  T.[labeling_material_grouping]=S.[labeling_material_grouping],
  T.[gross_contents]=S.[gross_contents],
  T.[quantity_conversion_method]=S.[quantity_conversion_method],
  T.[internal_object_number]=S.[internal_object_number],
  T.[environmentally_relevant]=S.[environmentally_relevant],
  T.[product_allocation_procedure]=S.[product_allocation_procedure],
  T.[variant_pricing_profile]=S.[variant_pricing_profile],
  T.[discount_in_kind]=S.[discount_in_kind],
  T.[manufacturer_part_number]=S.[manufacturer_part_number],
  T.[manufacturer_number]=S.[manufacturer_number],
  T.[inventory_managed_material_number]=S.[inventory_managed_material_number],
  T.[manufacturer_part_profile]=S.[manufacturer_part_profile],
  T.[units_of_measure_usage]=S.[units_of_measure_usage],
  T.[season_rollout]=S.[season_rollout],
  T.[dangerous_goods_profile]=S.[dangerous_goods_profile],
  T.[highly_viscous]=S.[highly_viscous],
  T.[in_bulk_liquid]=S.[in_bulk_liquid],
  T.[serial_number_explicitness]=S.[serial_number_explicitness],
  T.[closed_packaging]=S.[closed_packaging],
  T.[approved_batch_record_required]=S.[approved_batch_record_required],
  T.[effectivity_parameter_override]=S.[effectivity_parameter_override],
  T.[material_completion_level]=S.[material_completion_level],
  T.[shelf_life_period_indicator]=S.[shelf_life_period_indicator],
  T.[shelf_life_rounding_rule]=S.[shelf_life_rounding_rule],
  T.[product_composition_on_packaging]=S.[product_composition_on_packaging],
  T.[general_item_category_group]=S.[general_item_category_group],
  T.[logistical_variants]=S.[logistical_variants],
  T.[material_locked]=S.[material_locked],
  T.[configuration_management_relevant]=S.[configuration_management_relevant],
  T.[assortment_list_type]=S.[assortment_list_type],
  T.[expiration_date_type]=S.[expiration_date_type],
  T.[gtin_variant]=S.[gtin_variant],
  T.[generic_material_number]=S.[generic_material_number],
  T.[same_packing_reference_material]=S.[same_packing_reference_material],
  T.[global_data_sync_relevant]=S.[global_data_sync_relevant],
  T.[acceptance_at_origin]=S.[acceptance_at_origin],
  T.[standard_hu_type]=S.[standard_hu_type],
  T.[pilferable]=S.[pilferable],
  T.[warehouse_storage_condition]=S.[warehouse_storage_condition],
  T.[warehouse_material_group]=S.[warehouse_material_group],
  T.[handling_indicator]=S.[handling_indicator],
  T.[hazardous_substances_relevant]=S.[hazardous_substances_relevant],
  T.[handling_unit_type]=S.[handling_unit_type],
  T.[variable_tare_weight]=S.[variable_tare_weight],
  T.[maximum_allowed_capacity]=S.[maximum_allowed_capacity],
  T.[overcapacity_tolerance]=S.[overcapacity_tolerance],
  T.[maximum_packing_length]=S.[maximum_packing_length],
  T.[maximum_packing_width]=S.[maximum_packing_width],
  T.[maximum_packing_height]=S.[maximum_packing_height],
  T.[maximum_packing_dimension_unit]=S.[maximum_packing_dimension_unit],
  T.[country_of_origin]=S.[country_of_origin],
  T.[material_freight_group]=S.[material_freight_group],
  T.[quarantine_period]=S.[quarantine_period],
  T.[quarantine_period_unit]=S.[quarantine_period_unit],
  T.[quality_inspection_group]=S.[quality_inspection_group],
  T.[serial_number_profile]=S.[serial_number_profile],
  T.[form_name]=S.[form_name],
  T.[logistics_unit_of_measure]=S.[logistics_unit_of_measure],
  T.[catch_weight_material]=S.[catch_weight_material],
  T.[catch_weight_profile]=S.[catch_weight_profile],
  T.[catch_weight_tolerance_group]=S.[catch_weight_tolerance_group],
  T.[adjustment_profile]=S.[adjustment_profile],
  T.[intellectual_property_id]=S.[intellectual_property_id],
  T.[variant_price_allowed]=S.[variant_price_allowed],
  T.[medium]=S.[medium],
  T.[physical_commodity]=S.[physical_commodity],
  T.[animal_origin]=S.[animal_origin],
  T.[textile_composition_function]=S.[textile_composition_function],
  T.[segmentation_structure]=S.[segmentation_structure],
  T.[segmentation_strategy]=S.[segmentation_strategy],
  T.[segmentation_status]=S.[segmentation_status],
  T.[segmentation_scope]=S.[segmentation_scope],
  T.[segmentation_relevant]=S.[segmentation_relevant],
  T.[anp_code]=S.[anp_code],
  T.[fashion_attribute1]=S.[fashion_attribute1],
  T.[fashion_attribute2]=S.[fashion_attribute2],
  T.[fashion_attribute3]=S.[fashion_attribute3],
  T.[season_usage_indicator]=S.[season_usage_indicator],
  T.[season_active_in_inventory]=S.[season_active_in_inventory],
  T.[characteristic_conversion_id]=S.[characteristic_conversion_id],
  T.[packaging_code]=S.[packaging_code],
  T.[dangerous_goods_packaging_status]=S.[dangerous_goods_packaging_status],
  T.[material_condition_management]=S.[material_condition_management],
  T.[return_code]=S.[return_code],
  T.[return_to_logistics_level]=S.[return_to_logistics_level],
  T.[nato_item_identification_number]=S.[nato_item_identification_number],
  T.[fff_class]=S.[fff_class],
  T.[supersession_chain_number]=S.[supersession_chain_number],
  T.[seasonal_procurement_creation_status]=S.[seasonal_procurement_creation_status],
  T.[color_characteristic_internal_number]=S.[color_characteristic_internal_number],
  T.[main_size_characteristic_internal_number]=S.[main_size_characteristic_internal_number],
  T.[second_size_characteristic_internal_number]=S.[second_size_characteristic_internal_number],
  T.[color]=S.[color],
  T.[main_size]=S.[main_size],
  T.[second_size]=S.[second_size],
  T.[evaluation_characteristic_value]=S.[evaluation_characteristic_value],
  T.[care_code]=S.[care_code],
  T.[brand_id]=S.[brand_id],
  T.[fiber_code1]=S.[fiber_code1],
  T.[fiber_part1]=S.[fiber_part1],
  T.[fiber_code2]=S.[fiber_code2],
  T.[fiber_part2]=S.[fiber_part2],
  T.[fiber_code3]=S.[fiber_code3],
  T.[fiber_part3]=S.[fiber_part3],
  T.[fiber_code4]=S.[fiber_code4],
  T.[fiber_part4]=S.[fiber_part4],
  T.[fiber_code5]=S.[fiber_code5],
  T.[fiber_part5]=S.[fiber_part5],
  T.[fashion_grade]=S.[fashion_grade],
  T.[created_by]=S.[created_by],
  T.[created_at]=S.[created_at],
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=S.[updated_at],
  T.[is_deleted]=S.[is_deleted],
  T.[deleted_by]=S.[deleted_by],
  T.[deleted_at]=S.[deleted_at]
WHEN NOT MATCHED THEN
  INSERT ([id],[material_code],[complete_maintenance_status],[maintenance_status],[client_deletion_flag],[material_type],[industry_sector],[material_group],[old_material_number],[base_unit],[order_unit],[document_number],[document_type],[document_version],[document_page_format],[document_change_number],[document_page_number],[document_sheet_count],[production_inspection_memo],[production_memo_page_format],[size_dimensions],[basic_material],[industry_standard_description],[laboratory_design_office],[purchasing_value_key],[gross_weight],[net_weight],[weight_unit],[volume],[volume_unit],[container_requirements],[storage_conditions],[temperature_conditions],[low_level_code],[transportation_group],[hazardous_material_number],[division],[competitor],[european_article_number_obsolete],[gr_gi_slip_quantity],[procurement_rule],[source_of_supply],[season_category],[label_type],[label_form],[deactivated_field],[international_article_number],[ean_category],[length],[width],[height],[dimension_unit],[product_hierarchy],[stock_transfer_net_change_costing],[cad_indicator],[qm_in_procurement],[allowed_packaging_weight],[allowed_packaging_weight_unit],[allowed_packaging_volume],[allowed_packaging_volume_unit],[excess_weight_tolerance],[excess_volume_tolerance],[variable_purchase_order_unit],[revision_level_assigned],[configurable_material],[batch_management_required],[packaging_material_type],[maximum_level_by_volume],[stacking_factor],[packaging_material_group],[authorization_group],[valid_from_date],[valid_to_date],[season_year],[price_band_category],[empties_bill_of_material],[external_material_group],[cross_plant_configurable_material],[material_category],[co_product_indicator],[follow_up_material_indicator],[pricing_reference_material],[cross_plant_material_status],[cross_distribution_chain_status],[cross_plant_status_valid_from],[cross_distribution_status_valid_from],[tax_classification],[catalog_profile],[minimum_remaining_shelf_life],[total_shelf_life],[storage_percentage],[content_unit],[net_contents],[comparison_price_unit],[labeling_material_grouping],[gross_contents],[quantity_conversion_method],[internal_object_number],[environmentally_relevant],[product_allocation_procedure],[variant_pricing_profile],[discount_in_kind],[manufacturer_part_number],[manufacturer_number],[inventory_managed_material_number],[manufacturer_part_profile],[units_of_measure_usage],[season_rollout],[dangerous_goods_profile],[highly_viscous],[in_bulk_liquid],[serial_number_explicitness],[closed_packaging],[approved_batch_record_required],[effectivity_parameter_override],[material_completion_level],[shelf_life_period_indicator],[shelf_life_rounding_rule],[product_composition_on_packaging],[general_item_category_group],[logistical_variants],[material_locked],[configuration_management_relevant],[assortment_list_type],[expiration_date_type],[gtin_variant],[generic_material_number],[same_packing_reference_material],[global_data_sync_relevant],[acceptance_at_origin],[standard_hu_type],[pilferable],[warehouse_storage_condition],[warehouse_material_group],[handling_indicator],[hazardous_substances_relevant],[handling_unit_type],[variable_tare_weight],[maximum_allowed_capacity],[overcapacity_tolerance],[maximum_packing_length],[maximum_packing_width],[maximum_packing_height],[maximum_packing_dimension_unit],[country_of_origin],[material_freight_group],[quarantine_period],[quarantine_period_unit],[quality_inspection_group],[serial_number_profile],[form_name],[logistics_unit_of_measure],[catch_weight_material],[catch_weight_profile],[catch_weight_tolerance_group],[adjustment_profile],[intellectual_property_id],[variant_price_allowed],[medium],[physical_commodity],[animal_origin],[textile_composition_function],[segmentation_structure],[segmentation_strategy],[segmentation_status],[segmentation_scope],[segmentation_relevant],[anp_code],[fashion_attribute1],[fashion_attribute2],[fashion_attribute3],[season_usage_indicator],[season_active_in_inventory],[characteristic_conversion_id],[packaging_code],[dangerous_goods_packaging_status],[material_condition_management],[return_code],[return_to_logistics_level],[nato_item_identification_number],[fff_class],[supersession_chain_number],[seasonal_procurement_creation_status],[color_characteristic_internal_number],[main_size_characteristic_internal_number],[second_size_characteristic_internal_number],[color],[main_size],[second_size],[evaluation_characteristic_value],[care_code],[brand_id],[fiber_code1],[fiber_part1],[fiber_code2],[fiber_part2],[fiber_code3],[fiber_part3],[fiber_code4],[fiber_part4],[fiber_code5],[fiber_part5],[fashion_grade],[tenant_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[material_code],S.[complete_maintenance_status],S.[maintenance_status],S.[client_deletion_flag],S.[material_type],S.[industry_sector],S.[material_group],S.[old_material_number],S.[base_unit],S.[order_unit],S.[document_number],S.[document_type],S.[document_version],S.[document_page_format],S.[document_change_number],S.[document_page_number],S.[document_sheet_count],S.[production_inspection_memo],S.[production_memo_page_format],S.[size_dimensions],S.[basic_material],S.[industry_standard_description],S.[laboratory_design_office],S.[purchasing_value_key],S.[gross_weight],S.[net_weight],S.[weight_unit],S.[volume],S.[volume_unit],S.[container_requirements],S.[storage_conditions],S.[temperature_conditions],S.[low_level_code],S.[transportation_group],S.[hazardous_material_number],S.[division],S.[competitor],S.[european_article_number_obsolete],S.[gr_gi_slip_quantity],S.[procurement_rule],S.[source_of_supply],S.[season_category],S.[label_type],S.[label_form],S.[deactivated_field],S.[international_article_number],S.[ean_category],S.[length],S.[width],S.[height],S.[dimension_unit],S.[product_hierarchy],S.[stock_transfer_net_change_costing],S.[cad_indicator],S.[qm_in_procurement],S.[allowed_packaging_weight],S.[allowed_packaging_weight_unit],S.[allowed_packaging_volume],S.[allowed_packaging_volume_unit],S.[excess_weight_tolerance],S.[excess_volume_tolerance],S.[variable_purchase_order_unit],S.[revision_level_assigned],S.[configurable_material],S.[batch_management_required],S.[packaging_material_type],S.[maximum_level_by_volume],S.[stacking_factor],S.[packaging_material_group],S.[authorization_group],S.[valid_from_date],S.[valid_to_date],S.[season_year],S.[price_band_category],S.[empties_bill_of_material],S.[external_material_group],S.[cross_plant_configurable_material],S.[material_category],S.[co_product_indicator],S.[follow_up_material_indicator],S.[pricing_reference_material],S.[cross_plant_material_status],S.[cross_distribution_chain_status],S.[cross_plant_status_valid_from],S.[cross_distribution_status_valid_from],S.[tax_classification],S.[catalog_profile],S.[minimum_remaining_shelf_life],S.[total_shelf_life],S.[storage_percentage],S.[content_unit],S.[net_contents],S.[comparison_price_unit],S.[labeling_material_grouping],S.[gross_contents],S.[quantity_conversion_method],S.[internal_object_number],S.[environmentally_relevant],S.[product_allocation_procedure],S.[variant_pricing_profile],S.[discount_in_kind],S.[manufacturer_part_number],S.[manufacturer_number],S.[inventory_managed_material_number],S.[manufacturer_part_profile],S.[units_of_measure_usage],S.[season_rollout],S.[dangerous_goods_profile],S.[highly_viscous],S.[in_bulk_liquid],S.[serial_number_explicitness],S.[closed_packaging],S.[approved_batch_record_required],S.[effectivity_parameter_override],S.[material_completion_level],S.[shelf_life_period_indicator],S.[shelf_life_rounding_rule],S.[product_composition_on_packaging],S.[general_item_category_group],S.[logistical_variants],S.[material_locked],S.[configuration_management_relevant],S.[assortment_list_type],S.[expiration_date_type],S.[gtin_variant],S.[generic_material_number],S.[same_packing_reference_material],S.[global_data_sync_relevant],S.[acceptance_at_origin],S.[standard_hu_type],S.[pilferable],S.[warehouse_storage_condition],S.[warehouse_material_group],S.[handling_indicator],S.[hazardous_substances_relevant],S.[handling_unit_type],S.[variable_tare_weight],S.[maximum_allowed_capacity],S.[overcapacity_tolerance],S.[maximum_packing_length],S.[maximum_packing_width],S.[maximum_packing_height],S.[maximum_packing_dimension_unit],S.[country_of_origin],S.[material_freight_group],S.[quarantine_period],S.[quarantine_period_unit],S.[quality_inspection_group],S.[serial_number_profile],S.[form_name],S.[logistics_unit_of_measure],S.[catch_weight_material],S.[catch_weight_profile],S.[catch_weight_tolerance_group],S.[adjustment_profile],S.[intellectual_property_id],S.[variant_price_allowed],S.[medium],S.[physical_commodity],S.[animal_origin],S.[textile_composition_function],S.[segmentation_structure],S.[segmentation_strategy],S.[segmentation_status],S.[segmentation_scope],S.[segmentation_relevant],S.[anp_code],S.[fashion_attribute1],S.[fashion_attribute2],S.[fashion_attribute3],S.[season_usage_indicator],S.[season_active_in_inventory],S.[characteristic_conversion_id],S.[packaging_code],S.[dangerous_goods_packaging_status],S.[material_condition_management],S.[return_code],S.[return_to_logistics_level],S.[nato_item_identification_number],S.[fff_class],S.[supersession_chain_number],S.[seasonal_procurement_creation_status],S.[color_characteristic_internal_number],S.[main_size_characteristic_internal_number],S.[second_size_characteristic_internal_number],S.[color],S.[main_size],S.[second_size],S.[evaluation_characteristic_value],S.[care_code],S.[brand_id],S.[fiber_code1],S.[fiber_part1],S.[fiber_code2],S.[fiber_part2],S.[fiber_code3],S.[fiber_part3],S.[fiber_code4],S.[fiber_part4],S.[fiber_code5],S.[fiber_part5],S.[fashion_grade],S.[tenant_code],S.[ext_field],S.[remark],COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at])
OUTPUT
  S.rn, $action, INSERTED.[id], INSERTED.[material_code]
INTO #mat_delta (rn, oper_type, id, [material_code]);
  SET @dml_n = @@ROWCOUNT;
  SELECT
    N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
    N'merge' AS [phase],
    @merge_from_rn AS [from_rn],
    CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END AS [to_rn],
    @merge_max_rn AS [max_rn],
    @dml_n AS [batch_rows];
  SET @progress_msg = CONCAT(
    N'QUARTZ_SYNC_PROGRESS|',
    N'merge', N'|',
    CAST((@merge_from_rn) AS NVARCHAR(20)), N'|',
    CAST((CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END) AS NVARCHAR(20)), N'|',
    CAST((@merge_max_rn) AS NVARCHAR(20)), N'|',
    CAST((@dml_n) AS NVARCHAR(20)), N'|',
    N'');
  RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
  SET @merge_from_rn = @merge_to_rn + 1;
END


DECLARE @mat_delete_count INT = 0;
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
UPDATE TOP (@apply_chunk) T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT INSERTED.[id], INSERTED.[material_code]
INTO #mat_soft ([id], [material_code])
FROM [takt_logistics_materials_general_material] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1 FROM #mat_source S
    WHERE S.[material_code] = LTRIM(RTRIM(T.[material_code]))
  );
  SET @dml_n = @@ROWCOUNT;
  SET @mat_delete_count = @mat_delete_count + @dml_n;
END
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'soft' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  @mat_delete_count AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  @mat_delete_count AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'soft', N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@mat_delete_count) AS NVARCHAR(20)), N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@mat_delete_count) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;

DECLARE @mat_target_after INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_general_material]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);
DECLARE @mat_source_active INT = (SELECT COUNT(*) FROM #mat_source WHERE [is_deleted] = 0);

IF @mat_target_after <> @mat_source_active
BEGIN
  DECLARE @mat_cnt_msg NVARCHAR(200) = CONCAT(
    N'mat 有效行数不一致: source=', @mat_source_active, N', active=', @mat_target_after);
  THROW 50002, @mat_cnt_msg, 1;
END;

DECLARE @mat_insert INT = (SELECT COUNT(*) FROM #mat_delta WHERE oper_type = N'INSERT');
DECLARE @mat_update INT = (SELECT COUNT(*) FROM #mat_delta WHERE oper_type = N'UPDATE');
DECLARE @mat_unchanged INT = @mat_source_count - @mat_insert - @mat_update;

DECLARE @mat_soft_keys NVARCHAR(MAX) = N'';
SELECT @mat_soft_keys = STRING_AGG(
  CAST(
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([material_code], N''))
  AS NVARCHAR(MAX)),
  N'; '
)
FROM (
  SELECT TOP (100) [id], [material_code]
  FROM #mat_soft
  ORDER BY [id]
) SoftSample;
SET @mat_soft_keys = ISNULL(@mat_soft_keys, N'');
IF @mat_delete_count > 100
  SET @mat_soft_keys = CONCAT(@mat_soft_keys, N'; ...(+', CAST(@mat_delete_count - 100 AS NVARCHAR(20)), N')');

-- ========== desc ==========
IF OBJECT_ID('tempdb..#desc_source') IS NOT NULL DROP TABLE #desc_source;
IF OBJECT_ID('tempdb..#desc_delta') IS NOT NULL DROP TABLE #desc_delta;
IF OBJECT_ID('tempdb..#desc_soft') IS NOT NULL DROP TABLE #desc_soft;

CREATE TABLE #desc_source (
  [rn] INT,
  [id] BIGINT,
  [material_code] NVARCHAR(20),
  [material_description] NVARCHAR(40),
  [material_specification] NVARCHAR(70),
  [material_model] NVARCHAR(70),
  [material_long_description] NVARCHAR(255),
  [culture_code] NVARCHAR(5),
  [tenant_code] NVARCHAR(3),
  [ext_field] NVARCHAR(MAX), [remark] NVARCHAR(MAX),
  [created_by] BIGINT, [created_at] DATETIME, [updated_by] BIGINT, [updated_at] DATETIME, [deleted_by] BIGINT, [deleted_at] DATETIME,
  [is_deleted] INT);

CREATE TABLE #desc_delta (
  rn INT,
  oper_type NVARCHAR(10),
  id BIGINT,
  [material_code] NVARCHAR(40),
  [culture_code] NVARCHAR(5)
);

CREATE TABLE #desc_soft (
  [id] BIGINT,
  [material_code] NVARCHAR(40),
  [culture_code] NVARCHAR(5)
);

INSERT INTO #desc_source
SELECT
  S.rn,
  @base_id + 500000000 + S.rn,
  S.[material_code],
  S.[material_description],
  S.[material_specification],
  S.[material_model],
  S.[material_long_description],
  S.[culture_code],
  S.[tenant_code],
  S.[ext_field], S.[remark],
  S.[created_by], S.[created_at], S.[updated_by], S.[updated_at], S.[deleted_by], S.[deleted_at], S.[is_deleted]
FROM (
  SELECT
    N.*,
    ROW_NUMBER() OVER (ORDER BY N.[material_code], N.[culture_code]) AS rn
  FROM (
    SELECT
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_code])), 20), N''), N'') AS [material_code],
      LEFT(LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))), 3) AS [tenant_code],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[material_description])), 40), N''), N'') AS [material_description],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_specification])), 70), N'') AS [material_specification],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_model])), 70), N'') AS [material_model],
      NULLIF(LEFT(LTRIM(RTRIM(R.[material_long_description])), 255), N'') AS [material_long_description],
      ISNULL(NULLIF(LEFT(LTRIM(RTRIM(R.[culture_code])), 5), N''), N'zh-CN') AS [culture_code],
      ISNULL(R.[ext_field], N'{}') AS [ext_field],
      ISNULL(R.[remark], N'') AS [remark],
      COALESCE(TRY_CAST(R.[created_by] AS BIGINT), 0) AS [created_by],
      R.[created_at] AS [created_at],
      TRY_CAST(R.[updated_by] AS BIGINT) AS [updated_by],
      R.[updated_at] AS [updated_at],
      TRY_CAST(R.[deleted_by] AS BIGINT) AS [deleted_by],
      R.[deleted_at] AS [deleted_at],
      CASE WHEN ISNULL(R.[is_deleted], 0) = 0 THEN 0 ELSE 1 END AS [is_deleted],
            ROW_NUMBER() OVER (
        PARTITION BY LTRIM(RTRIM(R.[material_code])), LTRIM(RTRIM(R.[culture_code]))
        ORDER BY LTRIM(RTRIM(R.[material_code])), LTRIM(RTRIM(R.[culture_code]))
      ) AS dup_rn
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_description] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
  ) N
  WHERE N.dup_rn = 1
) S
WHERE @batch_size = 0 OR S.rn <= @batch_size;

DECLARE @desc_source_count INT = (SELECT COUNT(*) FROM #desc_source);
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'load' AS [phase],
  CAST(1 AS INT) AS [from_rn],
  @desc_source_count AS [to_rn],
  @desc_source_count AS [max_rn],
  @desc_source_count AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'load', N'|',
  CAST((CAST(1 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@desc_source_count) AS NVARCHAR(20)), N'|',
  CAST((@desc_source_count) AS NVARCHAR(20)), N'|',
  CAST((@desc_source_count) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
DECLARE @desc_sap_raw INT = (
  SELECT COUNT(*)
  FROM (
    SELECT LTRIM(RTRIM(R.[material_code])) AS [material_code], LTRIM(RTRIM(R.[culture_code])) AS [culture_code]
    FROM [{{SourceDatabase}}].[dbo].[takt_logistics_materials_material_description] R
    WHERE LTRIM(RTRIM(ISNULL(R.[material_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[tenant_code], N''))) <> N''
      AND LTRIM(RTRIM(ISNULL(R.[culture_code], N''))) <> N''
    GROUP BY LTRIM(RTRIM(R.[material_code])), LTRIM(RTRIM(R.[culture_code]))
  ) K
);

IF @desc_source_count <> @desc_sap_raw
BEGIN
  DECLARE @desc_src_msg NVARCHAR(200) = CONCAT(
    N'desc 源键与装入不一致: keys=', @desc_sap_raw, N', loaded=', @desc_source_count);
  THROW 50003, @desc_src_msg, 1;
END;

IF EXISTS (
  SELECT 1 FROM #desc_source
  GROUP BY [material_code], [culture_code] HAVING COUNT(*) > 1
)
BEGIN
  THROW 50001, N'desc 装入后业务键重复', 1;
END;

UPDATE S
SET S.[id] = COALESCE(T.[id], S.[id])
FROM #desc_source S
LEFT JOIN [takt_logistics_materials_material_description] T
  ON T.[tenant_code] = @tenant_code
 AND LTRIM(RTRIM(T.[material_code])) = S.[material_code]
 AND T.[culture_code] = S.[culture_code];

DECLARE @desc_target_before INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_description]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);

SET @merge_from_rn = 1;
SET @merge_max_rn = ISNULL((SELECT MAX([rn]) FROM #desc_source), 0);
WHILE @merge_from_rn <= @merge_max_rn
BEGIN
  SET @merge_to_rn = @merge_from_rn + @apply_chunk - 1;
MERGE INTO [takt_logistics_materials_material_description] AS T
USING (SELECT * FROM #desc_source WHERE [rn] >= @merge_from_rn AND [rn] <= @merge_to_rn) AS S
ON T.[tenant_code] = S.[tenant_code]
AND LTRIM(RTRIM(T.[material_code])) = S.[material_code]
AND T.[culture_code] = S.[culture_code]
WHEN MATCHED AND (
  ISNULL(T.[is_deleted], 0) <> S.[is_deleted]
  OR EXISTS (
    SELECT
      S.[material_description],
      S.[material_specification],
      S.[material_model],
      S.[material_long_description]
    EXCEPT
    SELECT
      T.[material_description],
      T.[material_specification],
      T.[material_model],
      T.[material_long_description]
  )

  OR ISNULL(T.[created_by], 0) <> ISNULL(S.[created_by], 0)
  OR ISNULL(T.[updated_by], 0) <> ISNULL(S.[updated_by], 0)
  OR ISNULL(T.[updated_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[updated_at], CAST('1900-01-01' AS DATETIME))
  OR ISNULL(T.[deleted_by], 0) <> ISNULL(S.[deleted_by], 0)
  OR ISNULL(T.[deleted_at], CAST('1900-01-01' AS DATETIME)) <> ISNULL(S.[deleted_at], CAST('1900-01-01' AS DATETIME))
) THEN
  UPDATE SET
  T.[material_description]=S.[material_description],
  T.[material_specification]=S.[material_specification],
  T.[material_model]=S.[material_model],
  T.[material_long_description]=S.[material_long_description],
  T.[created_by]=S.[created_by],
  T.[created_at]=S.[created_at],
  T.[updated_by]=S.[updated_by],
  T.[updated_at]=S.[updated_at],
  T.[is_deleted]=S.[is_deleted],
  T.[deleted_by]=S.[deleted_by],
  T.[deleted_at]=S.[deleted_at]
WHEN NOT MATCHED THEN
  INSERT ([id],[material_code],[material_description],[material_specification],[material_model],[material_long_description],[culture_code],[tenant_code],[ext_field],[remark],[created_by],[created_at],[updated_by],[updated_at],[is_deleted],[deleted_by],[deleted_at])
  VALUES (S.[id],S.[material_code],S.[material_description],S.[material_specification],S.[material_model],S.[material_long_description],S.[culture_code],S.[tenant_code],S.[ext_field],S.[remark],COALESCE(S.[created_by],@sync_user_id),COALESCE(S.[created_at],@now),S.[updated_by],S.[updated_at],S.[is_deleted],S.[deleted_by],S.[deleted_at])
OUTPUT
  S.rn, $action, INSERTED.[id], INSERTED.[material_code], INSERTED.[culture_code]
INTO #desc_delta (rn, oper_type, id, [material_code], [culture_code]);
  SET @dml_n = @@ROWCOUNT;
  SELECT
    N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
    N'merge' AS [phase],
    @merge_from_rn AS [from_rn],
    CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END AS [to_rn],
    @merge_max_rn AS [max_rn],
    @dml_n AS [batch_rows];
  SET @progress_msg = CONCAT(
    N'QUARTZ_SYNC_PROGRESS|',
    N'merge', N'|',
    CAST((@merge_from_rn) AS NVARCHAR(20)), N'|',
    CAST((CASE WHEN @merge_to_rn > @merge_max_rn THEN @merge_max_rn ELSE @merge_to_rn END) AS NVARCHAR(20)), N'|',
    CAST((@merge_max_rn) AS NVARCHAR(20)), N'|',
    CAST((@dml_n) AS NVARCHAR(20)), N'|',
    N'');
  RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;
  SET @merge_from_rn = @merge_to_rn + 1;
END


DECLARE @desc_delete_count INT = 0;
SET @dml_n = 1;
WHILE @dml_n > 0
BEGIN
UPDATE TOP (@apply_chunk) T
SET
  T.[is_deleted] = 1,
  T.[deleted_by] = @sync_user_id,
  T.[deleted_at] = @now,
  T.[updated_by] = @sync_user_id,
  T.[updated_at] = @now
OUTPUT INSERTED.[id], INSERTED.[material_code], INSERTED.[culture_code]
INTO #desc_soft ([id], [material_code], [culture_code])
FROM [takt_logistics_materials_material_description] T
WHERE T.[tenant_code] = @tenant_code
  AND T.[is_deleted] = 0
  AND NOT EXISTS (
    SELECT 1 FROM #desc_source S
    WHERE S.[material_code] = LTRIM(RTRIM(T.[material_code]))
      AND S.[culture_code] = T.[culture_code]
  );
  SET @dml_n = @@ROWCOUNT;
  SET @desc_delete_count = @desc_delete_count + @dml_n;
END
SELECT
  N'QUARTZ_SYNC_PROGRESS' AS [summary_tag],
  N'soft' AS [phase],
  CAST(0 AS INT) AS [from_rn],
  @desc_delete_count AS [to_rn],
  CAST(0 AS INT) AS [max_rn],
  @desc_delete_count AS [batch_rows];
SET @progress_msg = CONCAT(
  N'QUARTZ_SYNC_PROGRESS|',
  N'soft', N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@desc_delete_count) AS NVARCHAR(20)), N'|',
  CAST((CAST(0 AS INT)) AS NVARCHAR(20)), N'|',
  CAST((@desc_delete_count) AS NVARCHAR(20)), N'|',
  N'');
RAISERROR(@progress_msg, 10, 1) WITH NOWAIT;

DECLARE @desc_target_after INT = (
  SELECT COUNT(*)
  FROM [takt_logistics_materials_material_description]
  WHERE [tenant_code] = @tenant_code
    AND [is_deleted] = 0
);
DECLARE @desc_source_active INT = (SELECT COUNT(*) FROM #desc_source WHERE [is_deleted] = 0);

IF @desc_target_after <> @desc_source_active
BEGIN
  DECLARE @desc_cnt_msg NVARCHAR(200) = CONCAT(
    N'desc 有效行数不一致: source=', @desc_source_active, N', active=', @desc_target_after);
  THROW 50002, @desc_cnt_msg, 1;
END;

DECLARE @desc_insert INT = (SELECT COUNT(*) FROM #desc_delta WHERE oper_type = N'INSERT');
DECLARE @desc_update INT = (SELECT COUNT(*) FROM #desc_delta WHERE oper_type = N'UPDATE');
DECLARE @desc_unchanged INT = @desc_source_count - @desc_insert - @desc_update;

DECLARE @desc_soft_keys NVARCHAR(MAX) = N'';
SELECT @desc_soft_keys = STRING_AGG(
  CAST(
    CONCAT(CAST([id] AS NVARCHAR(30)), N'|', ISNULL([material_code], N''), N'|', ISNULL([culture_code], N''))
  AS NVARCHAR(MAX)),
  N'; '
)
FROM (
  SELECT TOP (100) [id], [material_code], [culture_code]
  FROM #desc_soft
  ORDER BY [id]
) SoftSample;
SET @desc_soft_keys = ISNULL(@desc_soft_keys, N'');
IF @desc_delete_count > 100
  SET @desc_soft_keys = CONCAT(@desc_soft_keys, N'; ...(+', CAST(@desc_delete_count - 100 AS NVARCHAR(20)), N')');

DECLARE @json_result NVARCHAR(MAX) =
  N'{"mat_sap_keys":' + CAST(@mat_sap_raw AS NVARCHAR)
  + N',"mat_source":' + CAST(@mat_source_count AS NVARCHAR)
  + N',"mat_before":' + CAST(@mat_target_before AS NVARCHAR)
  + N',"mat_after":' + CAST(@mat_target_after AS NVARCHAR)
  + N',"mat_insert":' + CAST(@mat_insert AS NVARCHAR)
  + N',"mat_update":' + CAST(@mat_update AS NVARCHAR)
  + N',"mat_unchanged":' + CAST(@mat_unchanged AS NVARCHAR)
  + N',"mat_soft_delete":' + CAST(@mat_delete_count AS NVARCHAR)
  + N',"desc_sap_keys":' + CAST(@desc_sap_raw AS NVARCHAR)
  + N',"desc_source":' + CAST(@desc_source_count AS NVARCHAR)
  + N',"desc_before":' + CAST(@desc_target_before AS NVARCHAR)
  + N',"desc_after":' + CAST(@desc_target_after AS NVARCHAR)
  + N',"desc_insert":' + CAST(@desc_insert AS NVARCHAR)
  + N',"desc_update":' + CAST(@desc_update AS NVARCHAR)
  + N',"desc_unchanged":' + CAST(@desc_unchanged AS NVARCHAR)
  + N',"desc_soft_delete":' + CAST(@desc_delete_count AS NVARCHAR)
  + N'}';



INSERT INTO [takt_statistics_logging_oper_log] (
  [id],[user_name],[oper_type],[oper_module],[oper_method],
  [request_method],[oper_url],[request_param],[json_result],
  [oper_ip],[oper_location],[user_agent],[browser],[os],[device_type],
  [oper_time],[elapsed_time],[oper_status],[error_msg],
  [tenant_code],[company_code],[plant_code],[culture_code],[created_by],[created_at]
)
VALUES (
  @base_id + 1,
  N'SYSTEM_SYNC',
  N'SYNC',
  N'全局物料主数据',
  N'exec_sql_merge',
  'SQL',
  N'/sync/material',
  CONCAT('batch_size=', @batch_size),
  @json_result,
  '127.0.0.1','Server','SQLCMD','Server','Windows','Server',
  @now,DATEDIFF(MILLISECOND,@now,GETDATE()),1,'',
  @tenant_code,@company_code,@plant_code,@culture_code,@sync_user_id,@now
);

SELECT
  N'QUARTZ_SYNC_SUMMARY' AS [summary_tag],
  CAST(N'material' AS NVARCHAR(40)) AS [scope],
  @mat_sap_raw AS [source_raw_count],
  @mat_source_count AS [source_count],
  @mat_target_before AS [target_before],
  @mat_target_after AS [target_after],
  @mat_target_after AS [target_physical],
  @mat_delete_count AS [soft_deleted],
  @mat_insert AS [insert_count],
  @mat_update AS [update_count],
  @mat_unchanged AS [unchanged_count],
  @mat_delete_count AS [delete_count],
  @mat_soft_keys AS [soft_deleted_keys]
UNION ALL
SELECT
  N'QUARTZ_SYNC_SUMMARY',
  CAST(N'material_description' AS NVARCHAR(40)),
  @desc_sap_raw,
  @desc_source_count,
  @desc_target_before,
  @desc_target_after,
  @desc_target_after,
  @desc_delete_count,
  @desc_insert,
  @desc_update,
  @desc_unchanged,
  @desc_delete_count,
  @desc_soft_keys;
