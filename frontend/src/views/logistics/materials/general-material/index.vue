<!-- ======================================== -->
<!-- 项目名称：节拍智控 · TaktWise (TDF) -->
<!-- 命名空间：@/views/logistics/materials/general-material -->
<!-- 文件名称：index.vue -->
<!-- 功能描述：Takt全局物料实体管理页面，含查询、增删改，由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <div class="p-4">
    <!-- 查询栏 -->
    <TaktQueryBar
      v-model="queryKeyword"
      :placeholder="searchPlaceholder"
      :loading="loading"
      @search="handleSearch"
      @reset="handleReset"
    />

    <!-- 工具栏 -->
    <TaktToolsBar
      create-permission="logistics:materials:general:material:create"
      update-permission="logistics:materials:general:material:update"
      delete-permission="logistics:materials:general:material:delete"
      import-permission="logistics:materials:general:material:import"
      export-permission="logistics:materials:general:material:export"
      :show-create="true"
      :show-update="true"
      :show-delete="true"
      :show-import="true"
      :show-export="true"
      :show-expand="false"
      :show-advanced-query="true"
      :show-column-setting="true"
      :show-fullscreen="true"
      :show-refresh="true"
      :create-disabled="false"
      :create-loading="loading"
      :update-disabled="updateDisabled"
      :update-loading="loading"
      :delete-disabled="deleteDisabled"
      :delete-loading="loading"
      :refresh-loading="loading"
      @create="handleCreate"
      @update="handleUpdate"
      @delete="handleDelete"
      @import="handleImport"
      @export="handleExport"
      @advanced-query="handleAdvancedQuery"
      @column-setting="handleColumnSetting"
      @refresh="handleRefresh"
    />

    <!-- 表格 -->
    <TaktSingleTable
      entity-scope="tenant"
      :columns="columns"
      :visible-column-keys="visibleColumnKeys"
      :id-column-key="'generalMaterialId'"
      table-mode="single"
      :data-source="dataSource"
      :loading="loading"
      :stripe="true"
      :virtual="true"
      :row-key="getGeneralMaterialId"
      :row-selection="rowSelection"
      :custom-row="onClickRow"

      @change="handleTableChange"
      @resize-column="handleResizeColumn"
    >
      <!-- 字典/开关列渲染 -->
      <template #bodyCell="{ column, record }">
        <template v-if="column.key === 'clientDeletionFlag'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'clientDeletionFlag')"
            dict-type="logistics_client_deletion_flag"
          />
        </template>
        <template v-else-if="column.key === 'materialType'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'materialType')"
            dict-type="logistics_materials_material_type"
          />
        </template>
        <template v-else-if="column.key === 'industrySector'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'industrySector')"
            dict-type="logistics_materials_industry_sector"
          />
        </template>
        <template v-else-if="column.key === 'baseUnit'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'baseUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'orderUnit'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'orderUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'documentType'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'documentType')"
            dict-type="logistics_document_type"
          />
        </template>
        <template v-else-if="column.key === 'documentPageFormat'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'documentPageFormat')"
            dict-type="logistics_document_page_format"
          />
        </template>
        <template v-else-if="column.key === 'productionMemoPageFormat'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'productionMemoPageFormat')"
            dict-type="logistics_production_memo_page_format"
          />
        </template>
        <template v-else-if="column.key === 'laboratoryDesignOffice'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'laboratoryDesignOffice')"
            dict-type="logistics_laboratory_design_office"
          />
        </template>
        <template v-else-if="column.key === 'purchasingValueKey'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'purchasingValueKey')"
            dict-type="logistics_purchasing_value_key"
          />
        </template>
        <template v-else-if="column.key === 'weightUnit'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'weightUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'volumeUnit'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'volumeUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'containerRequirements'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'containerRequirements')"
            dict-type="logistics_container_requirements"
          />
        </template>
        <template v-else-if="column.key === 'storageConditions'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'storageConditions')"
            dict-type="logistics_storage_conditions"
          />
        </template>
        <template v-else-if="column.key === 'temperatureConditions'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'temperatureConditions')"
            dict-type="logistics_temperature_conditions"
          />
        </template>
        <template v-else-if="column.key === 'transportationGroup'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'transportationGroup')"
            dict-type="logistics_transportation_group"
          />
        </template>
        <template v-else-if="column.key === 'division'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'division')"
            dict-type="logistics_product_group"
          />
        </template>
        <template v-else-if="column.key === 'procurementRule'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'procurementRule')"
            dict-type="logistics_procurement_rule"
          />
        </template>
        <template v-else-if="column.key === 'sourceOfSupply'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'sourceOfSupply')"
            dict-type="logistics_source_of_supply_type"
          />
        </template>
        <template v-else-if="column.key === 'seasonCategory'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'seasonCategory')"
            dict-type="logistics_season_category"
          />
        </template>
        <template v-else-if="column.key === 'labelType'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'labelType')"
            dict-type="logistics_label_type"
          />
        </template>
        <template v-else-if="column.key === 'labelForm'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'labelForm')"
            dict-type="logistics_label_form"
          />
        </template>
        <template v-else-if="column.key === 'eanCategory'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'eanCategory')"
            dict-type="logistics_ean_category"
          />
        </template>
        <template v-else-if="column.key === 'dimensionUnit'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'dimensionUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'productHierarchy'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'productHierarchy')"
            dict-type="logistics_product_hierarchy"
          />
        </template>
        <template v-else-if="column.key === 'allowedPackagingWeightUnit'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'allowedPackagingWeightUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'allowedPackagingVolumeUnit'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'allowedPackagingVolumeUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'batchManagementRequired'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'batchManagementRequired')"
            dict-type="sys_yes_no"
          />
        </template>
        <template v-else-if="column.key === 'packagingMaterialType'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'packagingMaterialType')"
            dict-type="logistics_materials_material_type"
          />
        </template>
        <template v-else-if="column.key === 'packagingMaterialGroup'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'packagingMaterialGroup')"
            dict-type="logistics_packaging_material_group"
          />
        </template>
        <template v-else-if="column.key === 'authorizationGroup'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'authorizationGroup')"
            dict-type="logistics_authorization_group"
          />
        </template>
        <template v-else-if="column.key === 'seasonYear'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'seasonYear')"
            dict-type="logistics_season_year"
          />
        </template>
        <template v-else-if="column.key === 'priceBandCategory'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'priceBandCategory')"
            dict-type="logistics_price_band_category"
          />
        </template>
        <template v-else-if="column.key === 'externalMaterialGroup'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'externalMaterialGroup')"
            dict-type="logistics_external_material_group"
          />
        </template>
        <template v-else-if="column.key === 'materialCategory'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'materialCategory')"
            dict-type="logistics_material_category"
          />
        </template>
        <template v-else-if="column.key === 'crossPlantMaterialStatus'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'crossPlantMaterialStatus')"
            dict-type="logistics_cross_plant_material_status"
          />
        </template>
        <template v-else-if="column.key === 'crossDistributionChainStatus'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'crossDistributionChainStatus')"
            dict-type="logistics_cross_distribution_chain_status"
          />
        </template>
        <template v-else-if="column.key === 'taxClassification'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'taxClassification')"
            dict-type="logistics_material_tax_classification"
          />
        </template>
        <template v-else-if="column.key === 'catalogProfile'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'catalogProfile')"
            dict-type="logistics_catalog_profile"
          />
        </template>
        <template v-else-if="column.key === 'contentUnit'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'contentUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'labelingMaterialGrouping'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'labelingMaterialGrouping')"
            dict-type="logistics_labeling_material_grouping"
          />
        </template>
        <template v-else-if="column.key === 'quantityConversionMethod'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'quantityConversionMethod')"
            dict-type="logistics_quantity_conversion_method"
          />
        </template>
        <template v-else-if="column.key === 'productAllocationProcedure'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'productAllocationProcedure')"
            dict-type="logistics_product_allocation_procedure"
          />
        </template>
        <template v-else-if="column.key === 'variantPricingProfile'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'variantPricingProfile')"
            dict-type="logistics_variant_pricing_profile"
          />
        </template>
        <template v-else-if="column.key === 'manufacturerPartProfile'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'manufacturerPartProfile')"
            dict-type="logistics_manufacturer_part_profile"
          />
        </template>
        <template v-else-if="column.key === 'unitsOfMeasureUsage'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'unitsOfMeasureUsage')"
            dict-type="logistics_units_of_measure_usage"
          />
        </template>
        <template v-else-if="column.key === 'seasonRollout'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'seasonRollout')"
            dict-type="logistics_season_rollout"
          />
        </template>
        <template v-else-if="column.key === 'dangerousGoodsProfile'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'dangerousGoodsProfile')"
            dict-type="logistics_dangerous_goods_profile"
          />
        </template>
        <template v-else-if="column.key === 'serialNumberExplicitness'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'serialNumberExplicitness')"
            dict-type="logistics_serial_number_explicitness"
          />
        </template>
        <template v-else-if="column.key === 'materialCompletionLevel'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'materialCompletionLevel')"
            dict-type="logistics_material_completion_level"
          />
        </template>
        <template v-else-if="column.key === 'shelfLifePeriodIndicator'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'shelfLifePeriodIndicator')"
            dict-type="logistics_shelf_life_period_indicator"
          />
        </template>
        <template v-else-if="column.key === 'shelfLifeRoundingRule'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'shelfLifeRoundingRule')"
            dict-type="logistics_shelf_life_rounding_rule"
          />
        </template>
        <template v-else-if="column.key === 'generalItemCategoryGroup'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'generalItemCategoryGroup')"
            dict-type="logistics_general_item_category_group"
          />
        </template>
        <template v-else-if="column.key === 'standardHuType'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'standardHuType')"
            dict-type="logistics_standard_hu_type"
          />
        </template>
        <template v-else-if="column.key === 'warehouseStorageCondition'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'warehouseStorageCondition')"
            dict-type="logistics_warehouse_storage_condition"
          />
        </template>
        <template v-else-if="column.key === 'warehouseMaterialGroup'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'warehouseMaterialGroup')"
            dict-type="logistics_warehouse_material_group"
          />
        </template>
        <template v-else-if="column.key === 'handlingIndicator'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'handlingIndicator')"
            dict-type="logistics_handling_indicator"
          />
        </template>
        <template v-else-if="column.key === 'handlingUnitType'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'handlingUnitType')"
            dict-type="logistics_handling_unit_type"
          />
        </template>
        <template v-else-if="column.key === 'maximumPackingDimensionUnit'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'maximumPackingDimensionUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'countryOfOrigin'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'countryOfOrigin')"
            dict-type="sys_country_code"
          />
        </template>
        <template v-else-if="column.key === 'materialFreightGroup'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'materialFreightGroup')"
            dict-type="logistics_material_freight_group"
          />
        </template>
        <template v-else-if="column.key === 'quarantinePeriodUnit'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'quarantinePeriodUnit')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'qualityInspectionGroup'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'qualityInspectionGroup')"
            dict-type="logistics_quality_inspection_group"
          />
        </template>
        <template v-else-if="column.key === 'serialNumberProfile'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'serialNumberProfile')"
            dict-type="logistics_serial_number_profile"
          />
        </template>
        <template v-else-if="column.key === 'formName'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'formName')"
            dict-type="logistics_form_name"
          />
        </template>
        <template v-else-if="column.key === 'logisticsUnitOfMeasure'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'logisticsUnitOfMeasure')"
            dict-type="logistics_materials_unit_of_measure_code"
          />
        </template>
        <template v-else-if="column.key === 'catchWeightProfile'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'catchWeightProfile')"
            dict-type="logistics_catch_weight_profile"
          />
        </template>
        <template v-else-if="column.key === 'catchWeightToleranceGroup'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'catchWeightToleranceGroup')"
            dict-type="logistics_catch_weight_tolerance_group"
          />
        </template>
        <template v-else-if="column.key === 'adjustmentProfile'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'adjustmentProfile')"
            dict-type="logistics_adjustment_profile"
          />
        </template>
        <template v-else-if="column.key === 'medium'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'medium')"
            dict-type="logistics_medium"
          />
        </template>
        <template v-else-if="column.key === 'physicalCommodity'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'physicalCommodity')"
            dict-type="logistics_physical_commodity"
          />
        </template>
        <template v-else-if="column.key === 'segmentationStructure'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'segmentationStructure')"
            dict-type="logistics_segmentation_structure"
          />
        </template>
        <template v-else-if="column.key === 'segmentationStrategy'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'segmentationStrategy')"
            dict-type="logistics_segmentation_strategy"
          />
        </template>
        <template v-else-if="column.key === 'segmentationStatus'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'segmentationStatus')"
            dict-type="logistics_segmentation_status"
          />
        </template>
        <template v-else-if="column.key === 'segmentationScope'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'segmentationScope')"
            dict-type="logistics_segmentation_scope"
          />
        </template>
        <template v-else-if="column.key === 'anpCode'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'anpCode')"
            dict-type="logistics_anp_code"
          />
        </template>
        <template v-else-if="column.key === 'fashionAttribute1'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'fashionAttribute1')"
            dict-type="logistics_fashion_attribute"
          />
        </template>
        <template v-else-if="column.key === 'fashionAttribute2'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'fashionAttribute2')"
            dict-type="logistics_fashion_attribute"
          />
        </template>
        <template v-else-if="column.key === 'fashionAttribute3'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'fashionAttribute3')"
            dict-type="logistics_fashion_attribute"
          />
        </template>
        <template v-else-if="column.key === 'seasonUsageIndicator'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'seasonUsageIndicator')"
            dict-type="logistics_season_usage_indicator"
          />
        </template>
        <template v-else-if="column.key === 'packagingCode'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'packagingCode')"
            dict-type="logistics_packaging_code"
          />
        </template>
        <template v-else-if="column.key === 'dangerousGoodsPackagingStatus'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'dangerousGoodsPackagingStatus')"
            dict-type="logistics_dangerous_goods_packaging_status"
          />
        </template>
        <template v-else-if="column.key === 'returnCode'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'returnCode')"
            dict-type="logistics_return_code"
          />
        </template>
        <template v-else-if="column.key === 'returnToLogisticsLevel'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'returnToLogisticsLevel')"
            dict-type="logistics_return_to_logistics_level"
          />
        </template>
        <template v-else-if="column.key === 'fffClass'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'fffClass')"
            dict-type="logistics_fff_class"
          />
        </template>
        <template v-else-if="column.key === 'seasonalProcurementCreationStatus'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'seasonalProcurementCreationStatus')"
            dict-type="logistics_seasonal_procurement_creation_status"
          />
        </template>
        <template v-else-if="column.key === 'color'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'color')"
            dict-type="logistics_color"
          />
        </template>
        <template v-else-if="column.key === 'mainSize'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'mainSize')"
            dict-type="logistics_main_size"
          />
        </template>
        <template v-else-if="column.key === 'secondSize'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'secondSize')"
            dict-type="logistics_second_size"
          />
        </template>
        <template v-else-if="column.key === 'evaluationCharacteristicValue'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'evaluationCharacteristicValue')"
            dict-type="logistics_evaluation_characteristic_value"
          />
        </template>
        <template v-else-if="column.key === 'careCode'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'careCode')"
            dict-type="logistics_care_code"
          />
        </template>
        <template v-else-if="column.key === 'brandId'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'brandId')"
            dict-type="logistics_brand_id"
          />
        </template>
        <template v-else-if="column.key === 'brandName'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'brandName')"
            dict-type="logistics_brand_id"
          />
        </template>
        <template v-else-if="column.key === 'fiberCode1'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'fiberCode1')"
            dict-type="logistics_fiber_code"
          />
        </template>
        <template v-else-if="column.key === 'fiberCode2'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'fiberCode2')"
            dict-type="logistics_fiber_code"
          />
        </template>
        <template v-else-if="column.key === 'fiberCode3'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'fiberCode3')"
            dict-type="logistics_fiber_code"
          />
        </template>
        <template v-else-if="column.key === 'fiberCode4'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'fiberCode4')"
            dict-type="logistics_fiber_code"
          />
        </template>
        <template v-else-if="column.key === 'fiberCode5'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'fiberCode5')"
            dict-type="logistics_fiber_code"
          />
        </template>
        <template v-else-if="column.key === 'fashionGrade'">
          <TaktDictTag
            :value="getGeneralMaterialDictValue(record, 'fashionGrade')"
            dict-type="logistics_fashion_grade"
          />
        </template>
      </template>

    </TaktSingleTable>

    <!-- 分页（服务端分页，外置 TaktPagination） -->
    <TaktPagination
      v-model:current="currentPage"
      v-model:page-size="pageSize"
      :total="total"
      @change="handlePaginationChange"
      @show-size-change="handlePaginationSizeChange"
    />

    <!-- 新增/编辑对话框 -->
    <TaktModal
      v-model:open="formVisible"
      :title="formTitle"
      :width="formModalWidthPx"
      wrap-class-name="takt-form-modal-resizable"
      :confirm-loading="formLoading"
      @ok="handleFormSubmit"
      @cancel="handleFormCancel"
    >
      <GeneralMaterialForm
        :key="formData?.generalMaterialId ?? 'create'"
        ref="formRef"
        :form-data="formData"
        :loading="formLoading"
      />
    </TaktModal>
    <!-- 高级查询抽屉 -->
    <TaktQueryDrawer
      v-model:open="advancedQueryVisible"
      v-model:visible-field-keys="visibleQueryFieldKeys"
      :fields="queryFieldsMeta"
      :storage-key="'takt-query-fields-logistics-materials-general-material'"
      :form-model="advancedQueryForm"
      @submit="handleAdvancedQuerySubmit"
      @reset="handleAdvancedQueryReset"
    >
      <template #default="{ isFieldVisible }">
      <div v-show="isFieldVisible('materialCode')">
      <a-form-item :label="pi.queryLabel('materialCode')">
        <a-input
          v-model:value="advancedQueryForm.materialCode"
          :placeholder="pi.queryPh('materialCode', 'required')"
          show-count
          :maxlength="20"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('completeMaintenanceStatus')">
      <a-form-item :label="pi.queryLabel('completeMaintenanceStatus')">
        <a-input
          v-model:value="advancedQueryForm.completeMaintenanceStatus"
          :placeholder="pi.queryPh('completeMaintenanceStatus', 'required')"
          show-count
          :maxlength="15"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maintenanceStatus')">
      <a-form-item :label="pi.queryLabel('maintenanceStatus')">
        <a-input
          v-model:value="advancedQueryForm.maintenanceStatus"
          :placeholder="pi.queryPh('maintenanceStatus', 'required')"
          show-count
          :maxlength="15"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('clientDeletionFlag')">
      <a-form-item :label="pi.queryLabel('clientDeletionFlag')">
        <TaktSelect
          v-model:value="advancedQueryForm.clientDeletionFlag"
          dict-type="logistics_client_deletion_flag"
          :placeholder="pi.queryPh('clientDeletionFlag', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialType')">
      <a-form-item :label="pi.queryLabel('materialType')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialType"
          dict-type="logistics_materials_material_type"
          :placeholder="pi.queryPh('materialType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industrySector')">
      <a-form-item :label="pi.queryLabel('industrySector')">
        <TaktSelect
          v-model:value="advancedQueryForm.industrySector"
          dict-type="logistics_materials_industry_sector"
          :placeholder="pi.queryPh('industrySector', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialGroup')">
      <a-form-item :label="pi.queryLabel('materialGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialGroup"
          api-url="TaktMaterialGroups/options"
          :placeholder="pi.queryPh('materialGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('oldMaterialNumber')">
      <a-form-item :label="pi.queryLabel('oldMaterialNumber')">
        <a-input
          v-model:value="advancedQueryForm.oldMaterialNumber"
          :placeholder="pi.queryPh('oldMaterialNumber', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('baseUnit')">
      <a-form-item :label="pi.queryLabel('baseUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.baseUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('baseUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('orderUnit')">
      <a-form-item :label="pi.queryLabel('orderUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.orderUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('orderUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentNumber')">
      <a-form-item :label="pi.queryLabel('documentNumber')">
        <a-input
          v-model:value="advancedQueryForm.documentNumber"
          :placeholder="pi.queryPh('documentNumber', 'required')"
          show-count
          :maxlength="22"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentType')">
      <a-form-item :label="pi.queryLabel('documentType')">
        <TaktSelect
          v-model:value="advancedQueryForm.documentType"
          dict-type="logistics_document_type"
          :placeholder="pi.queryPh('documentType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentVersion')">
      <a-form-item :label="pi.queryLabel('documentVersion')">
        <a-input
          v-model:value="advancedQueryForm.documentVersion"
          :placeholder="pi.queryPh('documentVersion', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentPageFormat')">
      <a-form-item :label="pi.queryLabel('documentPageFormat')">
        <TaktSelect
          v-model:value="advancedQueryForm.documentPageFormat"
          dict-type="logistics_document_page_format"
          :placeholder="pi.queryPh('documentPageFormat', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentChangeNumber')">
      <a-form-item :label="pi.queryLabel('documentChangeNumber')">
        <a-input
          v-model:value="advancedQueryForm.documentChangeNumber"
          :placeholder="pi.queryPh('documentChangeNumber', 'required')"
          show-count
          :maxlength="6"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentPageNumber')">
      <a-form-item :label="pi.queryLabel('documentPageNumber')">
        <a-input
          v-model:value="advancedQueryForm.documentPageNumber"
          :placeholder="pi.queryPh('documentPageNumber', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('documentSheetCount')">
      <a-form-item :label="pi.queryLabel('documentSheetCount')">
        <a-input
          v-model:value="advancedQueryForm.documentSheetCount"
          :placeholder="pi.queryPh('documentSheetCount', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionInspectionMemo')">
      <a-form-item :label="pi.queryLabel('productionInspectionMemo')">
        <a-input
          v-model:value="advancedQueryForm.productionInspectionMemo"
          :placeholder="pi.queryPh('productionInspectionMemo', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productionMemoPageFormat')">
      <a-form-item :label="pi.queryLabel('productionMemoPageFormat')">
        <TaktSelect
          v-model:value="advancedQueryForm.productionMemoPageFormat"
          dict-type="logistics_production_memo_page_format"
          :placeholder="pi.queryPh('productionMemoPageFormat', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sizeDimensions')">
      <a-form-item :label="pi.queryLabel('sizeDimensions')">
        <a-input
          v-model:value="advancedQueryForm.sizeDimensions"
          :placeholder="pi.queryPh('sizeDimensions', 'required')"
          show-count
          :maxlength="32"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('basicMaterial')">
      <a-form-item :label="pi.queryLabel('basicMaterial')">
        <a-input
          v-model:value="advancedQueryForm.basicMaterial"
          :placeholder="pi.queryPh('basicMaterial', 'required')"
          show-count
          :maxlength="48"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('industryStandardDescription')">
      <a-form-item :label="pi.queryLabel('industryStandardDescription')">
        <a-textarea
          v-model:value="advancedQueryForm.industryStandardDescription"
          :placeholder="pi.queryPh('industryStandardDescription', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('laboratoryDesignOffice')">
      <a-form-item :label="pi.queryLabel('laboratoryDesignOffice')">
        <TaktSelect
          v-model:value="advancedQueryForm.laboratoryDesignOffice"
          dict-type="logistics_laboratory_design_office"
          :placeholder="pi.queryPh('laboratoryDesignOffice', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('purchasingValueKey')">
      <a-form-item :label="pi.queryLabel('purchasingValueKey')">
        <TaktSelect
          v-model:value="advancedQueryForm.purchasingValueKey"
          dict-type="logistics_purchasing_value_key"
          :placeholder="pi.queryPh('purchasingValueKey', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('grossWeight')">
      <a-form-item :label="pi.queryLabel('grossWeight')">
        <a-input-number
          v-model:value="advancedQueryForm.grossWeight"
          :placeholder="pi.queryPh('grossWeight', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('netWeight')">
      <a-form-item :label="pi.queryLabel('netWeight')">
        <a-input-number
          v-model:value="advancedQueryForm.netWeight"
          :placeholder="pi.queryPh('netWeight', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('weightUnit')">
      <a-form-item :label="pi.queryLabel('weightUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.weightUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('weightUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('volume')">
      <a-form-item :label="pi.queryLabel('volume')">
        <a-input-number
          v-model:value="advancedQueryForm.volume"
          :placeholder="pi.queryPh('volume', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('volumeUnit')">
      <a-form-item :label="pi.queryLabel('volumeUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.volumeUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('volumeUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('containerRequirements')">
      <a-form-item :label="pi.queryLabel('containerRequirements')">
        <TaktSelect
          v-model:value="advancedQueryForm.containerRequirements"
          dict-type="logistics_container_requirements"
          :placeholder="pi.queryPh('containerRequirements', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('storageConditions')">
      <a-form-item :label="pi.queryLabel('storageConditions')">
        <TaktSelect
          v-model:value="advancedQueryForm.storageConditions"
          dict-type="logistics_storage_conditions"
          :placeholder="pi.queryPh('storageConditions', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('temperatureConditions')">
      <a-form-item :label="pi.queryLabel('temperatureConditions')">
        <TaktSelect
          v-model:value="advancedQueryForm.temperatureConditions"
          dict-type="logistics_temperature_conditions"
          :placeholder="pi.queryPh('temperatureConditions', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('lowLevelCode')">
      <a-form-item :label="pi.queryLabel('lowLevelCode')">
        <a-input
          v-model:value="advancedQueryForm.lowLevelCode"
          :placeholder="pi.queryPh('lowLevelCode', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('transportationGroup')">
      <a-form-item :label="pi.queryLabel('transportationGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.transportationGroup"
          dict-type="logistics_transportation_group"
          :placeholder="pi.queryPh('transportationGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('hazardousMaterialNumber')">
      <a-form-item :label="pi.queryLabel('hazardousMaterialNumber')">
        <a-input
          v-model:value="advancedQueryForm.hazardousMaterialNumber"
          :placeholder="pi.queryPh('hazardousMaterialNumber', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('division')">
      <a-form-item :label="pi.queryLabel('division')">
        <TaktSelect
          v-model:value="advancedQueryForm.division"
          dict-type="logistics_product_group"
          :placeholder="pi.queryPh('division', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('competitor')">
      <a-form-item :label="pi.queryLabel('competitor')">
        <a-input
          v-model:value="advancedQueryForm.competitor"
          :placeholder="pi.queryPh('competitor', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('europeanArticleNumberObsolete')">
      <a-form-item :label="pi.queryLabel('europeanArticleNumberObsolete')">
        <a-input
          v-model:value="advancedQueryForm.europeanArticleNumberObsolete"
          :placeholder="pi.queryPh('europeanArticleNumberObsolete', 'required')"
          show-count
          :maxlength="13"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('grGiSlipQuantity')">
      <a-form-item :label="pi.queryLabel('grGiSlipQuantity')">
        <a-input-number
          v-model:value="advancedQueryForm.grGiSlipQuantity"
          :placeholder="pi.queryPh('grGiSlipQuantity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('procurementRule')">
      <a-form-item :label="pi.queryLabel('procurementRule')">
        <TaktSelect
          v-model:value="advancedQueryForm.procurementRule"
          dict-type="logistics_procurement_rule"
          :placeholder="pi.queryPh('procurementRule', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('sourceOfSupply')">
      <a-form-item :label="pi.queryLabel('sourceOfSupply')">
        <TaktSelect
          v-model:value="advancedQueryForm.sourceOfSupply"
          dict-type="logistics_source_of_supply_type"
          :placeholder="pi.queryPh('sourceOfSupply', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('seasonCategory')">
      <a-form-item :label="pi.queryLabel('seasonCategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.seasonCategory"
          dict-type="logistics_season_category"
          :placeholder="pi.queryPh('seasonCategory', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('labelType')">
      <a-form-item :label="pi.queryLabel('labelType')">
        <TaktSelect
          v-model:value="advancedQueryForm.labelType"
          dict-type="logistics_label_type"
          :placeholder="pi.queryPh('labelType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('labelForm')">
      <a-form-item :label="pi.queryLabel('labelForm')">
        <TaktSelect
          v-model:value="advancedQueryForm.labelForm"
          dict-type="logistics_label_form"
          :placeholder="pi.queryPh('labelForm', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('deactivatedField')">
      <a-form-item :label="pi.queryLabel('deactivatedField')">
        <a-input
          v-model:value="advancedQueryForm.deactivatedField"
          :placeholder="pi.queryPh('deactivatedField', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('internationalArticleNumber')">
      <a-form-item :label="pi.queryLabel('internationalArticleNumber')">
        <a-input
          v-model:value="advancedQueryForm.internationalArticleNumber"
          :placeholder="pi.queryPh('internationalArticleNumber', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('eanCategory')">
      <a-form-item :label="pi.queryLabel('eanCategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.eanCategory"
          dict-type="logistics_ean_category"
          :placeholder="pi.queryPh('eanCategory', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('length')">
      <a-form-item :label="pi.queryLabel('length')">
        <a-input-number
          v-model:value="advancedQueryForm.length"
          :placeholder="pi.queryPh('length', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('width')">
      <a-form-item :label="pi.queryLabel('width')">
        <a-input-number
          v-model:value="advancedQueryForm.width"
          :placeholder="pi.queryPh('width', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('height')">
      <a-form-item :label="pi.queryLabel('height')">
        <a-input-number
          v-model:value="advancedQueryForm.height"
          :placeholder="pi.queryPh('height', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dimensionUnit')">
      <a-form-item :label="pi.queryLabel('dimensionUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.dimensionUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('dimensionUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productHierarchy')">
      <a-form-item :label="pi.queryLabel('productHierarchy')">
        <TaktSelect
          v-model:value="advancedQueryForm.productHierarchy"
          dict-type="logistics_product_hierarchy"
          :placeholder="pi.queryPh('productHierarchy', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stockTransferNetChangeCosting')">
      <a-form-item :label="pi.queryLabel('stockTransferNetChangeCosting')">
        <a-input
          v-model:value="advancedQueryForm.stockTransferNetChangeCosting"
          :placeholder="pi.queryPh('stockTransferNetChangeCosting', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('cadIndicator')">
      <a-form-item :label="pi.queryLabel('cadIndicator')">
        <a-input
          v-model:value="advancedQueryForm.cadIndicator"
          :placeholder="pi.queryPh('cadIndicator', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qmInProcurement')">
      <a-form-item :label="pi.queryLabel('qmInProcurement')">
        <a-input
          v-model:value="advancedQueryForm.qmInProcurement"
          :placeholder="pi.queryPh('qmInProcurement', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('allowedPackagingWeight')">
      <a-form-item :label="pi.queryLabel('allowedPackagingWeight')">
        <a-input-number
          v-model:value="advancedQueryForm.allowedPackagingWeight"
          :placeholder="pi.queryPh('allowedPackagingWeight', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('allowedPackagingWeightUnit')">
      <a-form-item :label="pi.queryLabel('allowedPackagingWeightUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.allowedPackagingWeightUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('allowedPackagingWeightUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('allowedPackagingVolume')">
      <a-form-item :label="pi.queryLabel('allowedPackagingVolume')">
        <a-input-number
          v-model:value="advancedQueryForm.allowedPackagingVolume"
          :placeholder="pi.queryPh('allowedPackagingVolume', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('allowedPackagingVolumeUnit')">
      <a-form-item :label="pi.queryLabel('allowedPackagingVolumeUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.allowedPackagingVolumeUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('allowedPackagingVolumeUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('excessWeightTolerance')">
      <a-form-item :label="pi.queryLabel('excessWeightTolerance')">
        <a-input-number
          v-model:value="advancedQueryForm.excessWeightTolerance"
          :placeholder="pi.queryPh('excessWeightTolerance', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('excessVolumeTolerance')">
      <a-form-item :label="pi.queryLabel('excessVolumeTolerance')">
        <a-input-number
          v-model:value="advancedQueryForm.excessVolumeTolerance"
          :placeholder="pi.queryPh('excessVolumeTolerance', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('variablePurchaseOrderUnit')">
      <a-form-item :label="pi.queryLabel('variablePurchaseOrderUnit')">
        <a-input
          v-model:value="advancedQueryForm.variablePurchaseOrderUnit"
          :placeholder="pi.queryPh('variablePurchaseOrderUnit', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('revisionLevelAssigned')">
      <a-form-item :label="pi.queryLabel('revisionLevelAssigned')">
        <a-input
          v-model:value="advancedQueryForm.revisionLevelAssigned"
          :placeholder="pi.queryPh('revisionLevelAssigned', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('configurableMaterial')">
      <a-form-item :label="pi.queryLabel('configurableMaterial')">
        <a-input
          v-model:value="advancedQueryForm.configurableMaterial"
          :placeholder="pi.queryPh('configurableMaterial', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('batchManagementRequired')">
      <a-form-item :label="pi.queryLabel('batchManagementRequired')">
        <TaktSelect
          v-model:value="advancedQueryForm.batchManagementRequired"
          dict-type="sys_yes_no"
          :placeholder="pi.queryPh('batchManagementRequired', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('packagingMaterialType')">
      <a-form-item :label="pi.queryLabel('packagingMaterialType')">
        <TaktSelect
          v-model:value="advancedQueryForm.packagingMaterialType"
          dict-type="logistics_materials_material_type"
          :placeholder="pi.queryPh('packagingMaterialType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maximumLevelByVolume')">
      <a-form-item :label="pi.queryLabel('maximumLevelByVolume')">
        <a-input-number
          v-model:value="advancedQueryForm.maximumLevelByVolume"
          :placeholder="pi.queryPh('maximumLevelByVolume', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('stackingFactor')">
      <a-form-item :label="pi.queryLabel('stackingFactor')">
        <a-input-number
          v-model:value="advancedQueryForm.stackingFactor"
          :placeholder="pi.queryPh('stackingFactor', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('packagingMaterialGroup')">
      <a-form-item :label="pi.queryLabel('packagingMaterialGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.packagingMaterialGroup"
          dict-type="logistics_packaging_material_group"
          :placeholder="pi.queryPh('packagingMaterialGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('authorizationGroup')">
      <a-form-item :label="pi.queryLabel('authorizationGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.authorizationGroup"
          dict-type="logistics_authorization_group"
          :placeholder="pi.queryPh('authorizationGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromDateStart')">
      <a-form-item :label="pi.queryLabel('validFromDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromDateStart"
          :placeholder="pi.queryPh('validFromDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validFromDateEnd')">
      <a-form-item :label="pi.queryLabel('validFromDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.validFromDateEnd"
          :placeholder="pi.queryPh('validFromDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToDateStart')">
      <a-form-item :label="pi.queryLabel('validToDateStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToDateStart"
          :placeholder="pi.queryPh('validToDateStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('validToDateEnd')">
      <a-form-item :label="pi.queryLabel('validToDateEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.validToDateEnd"
          :placeholder="pi.queryPh('validToDateEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('seasonYear')">
      <a-form-item :label="pi.queryLabel('seasonYear')">
        <TaktSelect
          v-model:value="advancedQueryForm.seasonYear"
          dict-type="logistics_season_year"
          :placeholder="pi.queryPh('seasonYear', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('priceBandCategory')">
      <a-form-item :label="pi.queryLabel('priceBandCategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.priceBandCategory"
          dict-type="logistics_price_band_category"
          :placeholder="pi.queryPh('priceBandCategory', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('emptiesBillOfMaterial')">
      <a-form-item :label="pi.queryLabel('emptiesBillOfMaterial')">
        <a-input
          v-model:value="advancedQueryForm.emptiesBillOfMaterial"
          :placeholder="pi.queryPh('emptiesBillOfMaterial', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('externalMaterialGroup')">
      <a-form-item :label="pi.queryLabel('externalMaterialGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.externalMaterialGroup"
          dict-type="logistics_external_material_group"
          :placeholder="pi.queryPh('externalMaterialGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossPlantConfigurableMaterial')">
      <a-form-item :label="pi.queryLabel('crossPlantConfigurableMaterial')">
        <a-input
          v-model:value="advancedQueryForm.crossPlantConfigurableMaterial"
          :placeholder="pi.queryPh('crossPlantConfigurableMaterial', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCategory')">
      <a-form-item :label="pi.queryLabel('materialCategory')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialCategory"
          dict-type="logistics_material_category"
          :placeholder="pi.queryPh('materialCategory', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('coProductIndicator')">
      <a-form-item :label="pi.queryLabel('coProductIndicator')">
        <a-input
          v-model:value="advancedQueryForm.coProductIndicator"
          :placeholder="pi.queryPh('coProductIndicator', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('followUpMaterialIndicator')">
      <a-form-item :label="pi.queryLabel('followUpMaterialIndicator')">
        <a-input
          v-model:value="advancedQueryForm.followUpMaterialIndicator"
          :placeholder="pi.queryPh('followUpMaterialIndicator', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pricingReferenceMaterial')">
      <a-form-item :label="pi.queryLabel('pricingReferenceMaterial')">
        <a-input
          v-model:value="advancedQueryForm.pricingReferenceMaterial"
          :placeholder="pi.queryPh('pricingReferenceMaterial', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossPlantMaterialStatus')">
      <a-form-item :label="pi.queryLabel('crossPlantMaterialStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.crossPlantMaterialStatus"
          dict-type="logistics_cross_plant_material_status"
          :placeholder="pi.queryPh('crossPlantMaterialStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossDistributionChainStatus')">
      <a-form-item :label="pi.queryLabel('crossDistributionChainStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.crossDistributionChainStatus"
          dict-type="logistics_cross_distribution_chain_status"
          :placeholder="pi.queryPh('crossDistributionChainStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossPlantStatusValidFromStart')">
      <a-form-item :label="pi.queryLabel('crossPlantStatusValidFromStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.crossPlantStatusValidFromStart"
          :placeholder="pi.queryPh('crossPlantStatusValidFromStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossPlantStatusValidFromEnd')">
      <a-form-item :label="pi.queryLabel('crossPlantStatusValidFromEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.crossPlantStatusValidFromEnd"
          :placeholder="pi.queryPh('crossPlantStatusValidFromEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossDistributionStatusValidFromStart')">
      <a-form-item :label="pi.queryLabel('crossDistributionStatusValidFromStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.crossDistributionStatusValidFromStart"
          :placeholder="pi.queryPh('crossDistributionStatusValidFromStart', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('crossDistributionStatusValidFromEnd')">
      <a-form-item :label="pi.queryLabel('crossDistributionStatusValidFromEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.crossDistributionStatusValidFromEnd"
          :placeholder="pi.queryPh('crossDistributionStatusValidFromEnd', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('taxClassification')">
      <a-form-item :label="pi.queryLabel('taxClassification')">
        <TaktSelect
          v-model:value="advancedQueryForm.taxClassification"
          dict-type="logistics_material_tax_classification"
          :placeholder="pi.queryPh('taxClassification', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('catalogProfile')">
      <a-form-item :label="pi.queryLabel('catalogProfile')">
        <TaktSelect
          v-model:value="advancedQueryForm.catalogProfile"
          dict-type="logistics_catalog_profile"
          :placeholder="pi.queryPh('catalogProfile', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('minimumRemainingShelfLife')">
      <a-form-item :label="pi.queryLabel('minimumRemainingShelfLife')">
        <a-input-number
          v-model:value="advancedQueryForm.minimumRemainingShelfLife"
          :placeholder="pi.queryPh('minimumRemainingShelfLife', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('totalShelfLife')">
      <a-form-item :label="pi.queryLabel('totalShelfLife')">
        <a-input-number
          v-model:value="advancedQueryForm.totalShelfLife"
          :placeholder="pi.queryPh('totalShelfLife', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('storagePercentage')">
      <a-form-item :label="pi.queryLabel('storagePercentage')">
        <a-input-number
          v-model:value="advancedQueryForm.storagePercentage"
          :placeholder="pi.queryPh('storagePercentage', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('contentUnit')">
      <a-form-item :label="pi.queryLabel('contentUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.contentUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('contentUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('netContents')">
      <a-form-item :label="pi.queryLabel('netContents')">
        <a-textarea
          v-model:value="advancedQueryForm.netContents"
          :placeholder="pi.queryPh('netContents', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('comparisonPriceUnit')">
      <a-form-item :label="pi.queryLabel('comparisonPriceUnit')">
        <a-input-number
          v-model:value="advancedQueryForm.comparisonPriceUnit"
          :placeholder="pi.queryPh('comparisonPriceUnit', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('labelingMaterialGrouping')">
      <a-form-item :label="pi.queryLabel('labelingMaterialGrouping')">
        <TaktSelect
          v-model:value="advancedQueryForm.labelingMaterialGrouping"
          dict-type="logistics_labeling_material_grouping"
          :placeholder="pi.queryPh('labelingMaterialGrouping', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('grossContents')">
      <a-form-item :label="pi.queryLabel('grossContents')">
        <a-textarea
          v-model:value="advancedQueryForm.grossContents"
          :placeholder="pi.queryPh('grossContents', 'optional')"
          :rows="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quantityConversionMethod')">
      <a-form-item :label="pi.queryLabel('quantityConversionMethod')">
        <TaktSelect
          v-model:value="advancedQueryForm.quantityConversionMethod"
          dict-type="logistics_quantity_conversion_method"
          :placeholder="pi.queryPh('quantityConversionMethod', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('internalObjectNumber')">
      <a-form-item :label="pi.queryLabel('internalObjectNumber')">
        <a-input
          v-model:value="advancedQueryForm.internalObjectNumber"
          :placeholder="pi.queryPh('internalObjectNumber', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('environmentallyRelevant')">
      <a-form-item :label="pi.queryLabel('environmentallyRelevant')">
        <a-input
          v-model:value="advancedQueryForm.environmentallyRelevant"
          :placeholder="pi.queryPh('environmentallyRelevant', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productAllocationProcedure')">
      <a-form-item :label="pi.queryLabel('productAllocationProcedure')">
        <TaktSelect
          v-model:value="advancedQueryForm.productAllocationProcedure"
          dict-type="logistics_product_allocation_procedure"
          :placeholder="pi.queryPh('productAllocationProcedure', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('variantPricingProfile')">
      <a-form-item :label="pi.queryLabel('variantPricingProfile')">
        <TaktSelect
          v-model:value="advancedQueryForm.variantPricingProfile"
          dict-type="logistics_variant_pricing_profile"
          :placeholder="pi.queryPh('variantPricingProfile', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('discountInKind')">
      <a-form-item :label="pi.queryLabel('discountInKind')">
        <a-input
          v-model:value="advancedQueryForm.discountInKind"
          :placeholder="pi.queryPh('discountInKind', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerPartNumber')">
      <a-form-item :label="pi.queryLabel('manufacturerPartNumber')">
        <TaktSelect
          v-model:value="advancedQueryForm.manufacturerPartNumber"
          api-url="TaktManufacturerMaterials/options"
          :placeholder="pi.queryPh('manufacturerPartNumber', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerNumber')">
      <a-form-item :label="pi.queryLabel('manufacturerNumber')">
        <TaktSelect
          v-model:value="advancedQueryForm.manufacturerNumber"
          api-url="TaktSuppliers/options"
          :placeholder="pi.queryPh('manufacturerNumber', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inventoryManagedMaterialNumber')">
      <a-form-item :label="pi.queryLabel('inventoryManagedMaterialNumber')">
        <a-input
          v-model:value="advancedQueryForm.inventoryManagedMaterialNumber"
          :placeholder="pi.queryPh('inventoryManagedMaterialNumber', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('manufacturerPartProfile')">
      <a-form-item :label="pi.queryLabel('manufacturerPartProfile')">
        <TaktSelect
          v-model:value="advancedQueryForm.manufacturerPartProfile"
          dict-type="logistics_manufacturer_part_profile"
          :placeholder="pi.queryPh('manufacturerPartProfile', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('unitsOfMeasureUsage')">
      <a-form-item :label="pi.queryLabel('unitsOfMeasureUsage')">
        <TaktSelect
          v-model:value="advancedQueryForm.unitsOfMeasureUsage"
          dict-type="logistics_units_of_measure_usage"
          :placeholder="pi.queryPh('unitsOfMeasureUsage', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('seasonRollout')">
      <a-form-item :label="pi.queryLabel('seasonRollout')">
        <TaktSelect
          v-model:value="advancedQueryForm.seasonRollout"
          dict-type="logistics_season_rollout"
          :placeholder="pi.queryPh('seasonRollout', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dangerousGoodsProfile')">
      <a-form-item :label="pi.queryLabel('dangerousGoodsProfile')">
        <TaktSelect
          v-model:value="advancedQueryForm.dangerousGoodsProfile"
          dict-type="logistics_dangerous_goods_profile"
          :placeholder="pi.queryPh('dangerousGoodsProfile', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('highlyViscous')">
      <a-form-item :label="pi.queryLabel('highlyViscous')">
        <a-input
          v-model:value="advancedQueryForm.highlyViscous"
          :placeholder="pi.queryPh('highlyViscous', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('inBulkLiquid')">
      <a-form-item :label="pi.queryLabel('inBulkLiquid')">
        <a-input
          v-model:value="advancedQueryForm.inBulkLiquid"
          :placeholder="pi.queryPh('inBulkLiquid', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serialNumberExplicitness')">
      <a-form-item :label="pi.queryLabel('serialNumberExplicitness')">
        <TaktSelect
          v-model:value="advancedQueryForm.serialNumberExplicitness"
          dict-type="logistics_serial_number_explicitness"
          :placeholder="pi.queryPh('serialNumberExplicitness', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('closedPackaging')">
      <a-form-item :label="pi.queryLabel('closedPackaging')">
        <a-input
          v-model:value="advancedQueryForm.closedPackaging"
          :placeholder="pi.queryPh('closedPackaging', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('approvedBatchRecordRequired')">
      <a-form-item :label="pi.queryLabel('approvedBatchRecordRequired')">
        <a-input
          v-model:value="advancedQueryForm.approvedBatchRecordRequired"
          :placeholder="pi.queryPh('approvedBatchRecordRequired', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('effectivityParameterOverride')">
      <a-form-item :label="pi.queryLabel('effectivityParameterOverride')">
        <a-input
          v-model:value="advancedQueryForm.effectivityParameterOverride"
          :placeholder="pi.queryPh('effectivityParameterOverride', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialCompletionLevel')">
      <a-form-item :label="pi.queryLabel('materialCompletionLevel')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialCompletionLevel"
          dict-type="logistics_material_completion_level"
          :placeholder="pi.queryPh('materialCompletionLevel', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shelfLifePeriodIndicator')">
      <a-form-item :label="pi.queryLabel('shelfLifePeriodIndicator')">
        <TaktSelect
          v-model:value="advancedQueryForm.shelfLifePeriodIndicator"
          dict-type="logistics_shelf_life_period_indicator"
          :placeholder="pi.queryPh('shelfLifePeriodIndicator', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('shelfLifeRoundingRule')">
      <a-form-item :label="pi.queryLabel('shelfLifeRoundingRule')">
        <TaktSelect
          v-model:value="advancedQueryForm.shelfLifeRoundingRule"
          dict-type="logistics_shelf_life_rounding_rule"
          :placeholder="pi.queryPh('shelfLifeRoundingRule', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('productCompositionOnPackaging')">
      <a-form-item :label="pi.queryLabel('productCompositionOnPackaging')">
        <a-input
          v-model:value="advancedQueryForm.productCompositionOnPackaging"
          :placeholder="pi.queryPh('productCompositionOnPackaging', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('generalItemCategoryGroup')">
      <a-form-item :label="pi.queryLabel('generalItemCategoryGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.generalItemCategoryGroup"
          dict-type="logistics_general_item_category_group"
          :placeholder="pi.queryPh('generalItemCategoryGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('logisticalVariants')">
      <a-form-item :label="pi.queryLabel('logisticalVariants')">
        <a-input
          v-model:value="advancedQueryForm.logisticalVariants"
          :placeholder="pi.queryPh('logisticalVariants', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialLocked')">
      <a-form-item :label="pi.queryLabel('materialLocked')">
        <a-input
          v-model:value="advancedQueryForm.materialLocked"
          :placeholder="pi.queryPh('materialLocked', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('configurationManagementRelevant')">
      <a-form-item :label="pi.queryLabel('configurationManagementRelevant')">
        <a-input
          v-model:value="advancedQueryForm.configurationManagementRelevant"
          :placeholder="pi.queryPh('configurationManagementRelevant', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('assortmentListType')">
      <a-form-item :label="pi.queryLabel('assortmentListType')">
        <a-input
          v-model:value="advancedQueryForm.assortmentListType"
          :placeholder="pi.queryPh('assortmentListType', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('expirationDateType')">
      <a-form-item :label="pi.queryLabel('expirationDateType')">
        <a-date-picker
          v-model:value="advancedQueryForm.expirationDateType"
          :placeholder="pi.queryPh('expirationDateType', 'select')"
          value-format="YYYY-MM-DD"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('gtinVariant')">
      <a-form-item :label="pi.queryLabel('gtinVariant')">
        <a-input
          v-model:value="advancedQueryForm.gtinVariant"
          :placeholder="pi.queryPh('gtinVariant', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('genericMaterialNumber')">
      <a-form-item :label="pi.queryLabel('genericMaterialNumber')">
        <a-input
          v-model:value="advancedQueryForm.genericMaterialNumber"
          :placeholder="pi.queryPh('genericMaterialNumber', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('samePackingReferenceMaterial')">
      <a-form-item :label="pi.queryLabel('samePackingReferenceMaterial')">
        <a-input
          v-model:value="advancedQueryForm.samePackingReferenceMaterial"
          :placeholder="pi.queryPh('samePackingReferenceMaterial', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('globalDataSyncRelevant')">
      <a-form-item :label="pi.queryLabel('globalDataSyncRelevant')">
        <a-input
          v-model:value="advancedQueryForm.globalDataSyncRelevant"
          :placeholder="pi.queryPh('globalDataSyncRelevant', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('acceptanceAtOrigin')">
      <a-form-item :label="pi.queryLabel('acceptanceAtOrigin')">
        <a-input
          v-model:value="advancedQueryForm.acceptanceAtOrigin"
          :placeholder="pi.queryPh('acceptanceAtOrigin', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('standardHuType')">
      <a-form-item :label="pi.queryLabel('standardHuType')">
        <TaktSelect
          v-model:value="advancedQueryForm.standardHuType"
          dict-type="logistics_standard_hu_type"
          :placeholder="pi.queryPh('standardHuType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('pilferable')">
      <a-form-item :label="pi.queryLabel('pilferable')">
        <a-input
          v-model:value="advancedQueryForm.pilferable"
          :placeholder="pi.queryPh('pilferable', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warehouseStorageCondition')">
      <a-form-item :label="pi.queryLabel('warehouseStorageCondition')">
        <TaktSelect
          v-model:value="advancedQueryForm.warehouseStorageCondition"
          dict-type="logistics_warehouse_storage_condition"
          :placeholder="pi.queryPh('warehouseStorageCondition', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('warehouseMaterialGroup')">
      <a-form-item :label="pi.queryLabel('warehouseMaterialGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.warehouseMaterialGroup"
          dict-type="logistics_warehouse_material_group"
          :placeholder="pi.queryPh('warehouseMaterialGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingIndicator')">
      <a-form-item :label="pi.queryLabel('handlingIndicator')">
        <TaktSelect
          v-model:value="advancedQueryForm.handlingIndicator"
          dict-type="logistics_handling_indicator"
          :placeholder="pi.queryPh('handlingIndicator', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('hazardousSubstancesRelevant')">
      <a-form-item :label="pi.queryLabel('hazardousSubstancesRelevant')">
        <a-input
          v-model:value="advancedQueryForm.hazardousSubstancesRelevant"
          :placeholder="pi.queryPh('hazardousSubstancesRelevant', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('handlingUnitType')">
      <a-form-item :label="pi.queryLabel('handlingUnitType')">
        <TaktSelect
          v-model:value="advancedQueryForm.handlingUnitType"
          dict-type="logistics_handling_unit_type"
          :placeholder="pi.queryPh('handlingUnitType', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('variableTareWeight')">
      <a-form-item :label="pi.queryLabel('variableTareWeight')">
        <a-input
          v-model:value="advancedQueryForm.variableTareWeight"
          :placeholder="pi.queryPh('variableTareWeight', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maximumAllowedCapacity')">
      <a-form-item :label="pi.queryLabel('maximumAllowedCapacity')">
        <a-input-number
          v-model:value="advancedQueryForm.maximumAllowedCapacity"
          :placeholder="pi.queryPh('maximumAllowedCapacity', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('overcapacityTolerance')">
      <a-form-item :label="pi.queryLabel('overcapacityTolerance')">
        <a-input-number
          v-model:value="advancedQueryForm.overcapacityTolerance"
          :placeholder="pi.queryPh('overcapacityTolerance', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maximumPackingLength')">
      <a-form-item :label="pi.queryLabel('maximumPackingLength')">
        <a-input-number
          v-model:value="advancedQueryForm.maximumPackingLength"
          :placeholder="pi.queryPh('maximumPackingLength', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maximumPackingWidth')">
      <a-form-item :label="pi.queryLabel('maximumPackingWidth')">
        <a-input-number
          v-model:value="advancedQueryForm.maximumPackingWidth"
          :placeholder="pi.queryPh('maximumPackingWidth', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maximumPackingHeight')">
      <a-form-item :label="pi.queryLabel('maximumPackingHeight')">
        <a-input-number
          v-model:value="advancedQueryForm.maximumPackingHeight"
          :placeholder="pi.queryPh('maximumPackingHeight', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('maximumPackingDimensionUnit')">
      <a-form-item :label="pi.queryLabel('maximumPackingDimensionUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.maximumPackingDimensionUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('maximumPackingDimensionUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('countryOfOrigin')">
      <a-form-item :label="pi.queryLabel('countryOfOrigin')">
        <TaktSelect
          v-model:value="advancedQueryForm.countryOfOrigin"
          dict-type="sys_country_code"
          :placeholder="pi.queryPh('countryOfOrigin', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialFreightGroup')">
      <a-form-item :label="pi.queryLabel('materialFreightGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.materialFreightGroup"
          dict-type="logistics_material_freight_group"
          :placeholder="pi.queryPh('materialFreightGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quarantinePeriod')">
      <a-form-item :label="pi.queryLabel('quarantinePeriod')">
        <a-input-number
          v-model:value="advancedQueryForm.quarantinePeriod"
          :placeholder="pi.queryPh('quarantinePeriod', 'required')"
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('quarantinePeriodUnit')">
      <a-form-item :label="pi.queryLabel('quarantinePeriodUnit')">
        <TaktSelect
          v-model:value="advancedQueryForm.quarantinePeriodUnit"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('quarantinePeriodUnit', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('qualityInspectionGroup')">
      <a-form-item :label="pi.queryLabel('qualityInspectionGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.qualityInspectionGroup"
          dict-type="logistics_quality_inspection_group"
          :placeholder="pi.queryPh('qualityInspectionGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('serialNumberProfile')">
      <a-form-item :label="pi.queryLabel('serialNumberProfile')">
        <TaktSelect
          v-model:value="advancedQueryForm.serialNumberProfile"
          dict-type="logistics_serial_number_profile"
          :placeholder="pi.queryPh('serialNumberProfile', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('formName')">
      <a-form-item :label="pi.queryLabel('formName')">
        <TaktSelect
          v-model:value="advancedQueryForm.formName"
          dict-type="logistics_form_name"
          :placeholder="pi.queryPh('formName', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('logisticsUnitOfMeasure')">
      <a-form-item :label="pi.queryLabel('logisticsUnitOfMeasure')">
        <TaktSelect
          v-model:value="advancedQueryForm.logisticsUnitOfMeasure"
          dict-type="logistics_materials_unit_of_measure_code"
          :placeholder="pi.queryPh('logisticsUnitOfMeasure', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('catchWeightMaterial')">
      <a-form-item :label="pi.queryLabel('catchWeightMaterial')">
        <a-input
          v-model:value="advancedQueryForm.catchWeightMaterial"
          :placeholder="pi.queryPh('catchWeightMaterial', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('catchWeightProfile')">
      <a-form-item :label="pi.queryLabel('catchWeightProfile')">
        <TaktSelect
          v-model:value="advancedQueryForm.catchWeightProfile"
          dict-type="logistics_catch_weight_profile"
          :placeholder="pi.queryPh('catchWeightProfile', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('catchWeightToleranceGroup')">
      <a-form-item :label="pi.queryLabel('catchWeightToleranceGroup')">
        <TaktSelect
          v-model:value="advancedQueryForm.catchWeightToleranceGroup"
          dict-type="logistics_catch_weight_tolerance_group"
          :placeholder="pi.queryPh('catchWeightToleranceGroup', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('adjustmentProfile')">
      <a-form-item :label="pi.queryLabel('adjustmentProfile')">
        <TaktSelect
          v-model:value="advancedQueryForm.adjustmentProfile"
          dict-type="logistics_adjustment_profile"
          :placeholder="pi.queryPh('adjustmentProfile', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('intellectualPropertyId')">
      <a-form-item :label="pi.queryLabel('intellectualPropertyId')">
        <a-input
          v-model:value="advancedQueryForm.intellectualPropertyId"
          :placeholder="pi.queryPh('intellectualPropertyId', 'required')"
          show-count
          :maxlength="40"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('variantPriceAllowed')">
      <a-form-item :label="pi.queryLabel('variantPriceAllowed')">
        <a-input
          v-model:value="advancedQueryForm.variantPriceAllowed"
          :placeholder="pi.queryPh('variantPriceAllowed', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('medium')">
      <a-form-item :label="pi.queryLabel('medium')">
        <TaktSelect
          v-model:value="advancedQueryForm.medium"
          dict-type="logistics_medium"
          :placeholder="pi.queryPh('medium', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('physicalCommodity')">
      <a-form-item :label="pi.queryLabel('physicalCommodity')">
        <TaktSelect
          v-model:value="advancedQueryForm.physicalCommodity"
          dict-type="logistics_physical_commodity"
          :placeholder="pi.queryPh('physicalCommodity', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('animalOrigin')">
      <a-form-item :label="pi.queryLabel('animalOrigin')">
        <a-input
          v-model:value="advancedQueryForm.animalOrigin"
          :placeholder="pi.queryPh('animalOrigin', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('textileCompositionFunction')">
      <a-form-item :label="pi.queryLabel('textileCompositionFunction')">
        <a-input
          v-model:value="advancedQueryForm.textileCompositionFunction"
          :placeholder="pi.queryPh('textileCompositionFunction', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('segmentationStructure')">
      <a-form-item :label="pi.queryLabel('segmentationStructure')">
        <TaktSelect
          v-model:value="advancedQueryForm.segmentationStructure"
          dict-type="logistics_segmentation_structure"
          :placeholder="pi.queryPh('segmentationStructure', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('segmentationStrategy')">
      <a-form-item :label="pi.queryLabel('segmentationStrategy')">
        <TaktSelect
          v-model:value="advancedQueryForm.segmentationStrategy"
          dict-type="logistics_segmentation_strategy"
          :placeholder="pi.queryPh('segmentationStrategy', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('segmentationStatus')">
      <a-form-item :label="pi.queryLabel('segmentationStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.segmentationStatus"
          dict-type="logistics_segmentation_status"
          :placeholder="pi.queryPh('segmentationStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('segmentationScope')">
      <a-form-item :label="pi.queryLabel('segmentationScope')">
        <TaktSelect
          v-model:value="advancedQueryForm.segmentationScope"
          dict-type="logistics_segmentation_scope"
          :placeholder="pi.queryPh('segmentationScope', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('segmentationRelevant')">
      <a-form-item :label="pi.queryLabel('segmentationRelevant')">
        <a-input
          v-model:value="advancedQueryForm.segmentationRelevant"
          :placeholder="pi.queryPh('segmentationRelevant', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('anpCode')">
      <a-form-item :label="pi.queryLabel('anpCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.anpCode"
          dict-type="logistics_anp_code"
          :placeholder="pi.queryPh('anpCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fashionAttribute1')">
      <a-form-item :label="pi.queryLabel('fashionAttribute1')">
        <TaktSelect
          v-model:value="advancedQueryForm.fashionAttribute1"
          dict-type="logistics_fashion_attribute"
          :placeholder="pi.queryPh('fashionAttribute1', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fashionAttribute2')">
      <a-form-item :label="pi.queryLabel('fashionAttribute2')">
        <TaktSelect
          v-model:value="advancedQueryForm.fashionAttribute2"
          dict-type="logistics_fashion_attribute"
          :placeholder="pi.queryPh('fashionAttribute2', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fashionAttribute3')">
      <a-form-item :label="pi.queryLabel('fashionAttribute3')">
        <TaktSelect
          v-model:value="advancedQueryForm.fashionAttribute3"
          dict-type="logistics_fashion_attribute"
          :placeholder="pi.queryPh('fashionAttribute3', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('seasonUsageIndicator')">
      <a-form-item :label="pi.queryLabel('seasonUsageIndicator')">
        <TaktSelect
          v-model:value="advancedQueryForm.seasonUsageIndicator"
          dict-type="logistics_season_usage_indicator"
          :placeholder="pi.queryPh('seasonUsageIndicator', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('seasonActiveInInventory')">
      <a-form-item :label="pi.queryLabel('seasonActiveInInventory')">
        <a-input
          v-model:value="advancedQueryForm.seasonActiveInInventory"
          :placeholder="pi.queryPh('seasonActiveInInventory', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('characteristicConversionId')">
      <a-form-item :label="pi.queryLabel('characteristicConversionId')">
        <a-input
          v-model:value="advancedQueryForm.characteristicConversionId"
          :placeholder="pi.queryPh('characteristicConversionId', 'required')"
          show-count
          :maxlength="2"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('packagingCode')">
      <a-form-item :label="pi.queryLabel('packagingCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.packagingCode"
          dict-type="logistics_packaging_code"
          :placeholder="pi.queryPh('packagingCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('dangerousGoodsPackagingStatus')">
      <a-form-item :label="pi.queryLabel('dangerousGoodsPackagingStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.dangerousGoodsPackagingStatus"
          dict-type="logistics_dangerous_goods_packaging_status"
          :placeholder="pi.queryPh('dangerousGoodsPackagingStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('materialConditionManagement')">
      <a-form-item :label="pi.queryLabel('materialConditionManagement')">
        <a-input
          v-model:value="advancedQueryForm.materialConditionManagement"
          :placeholder="pi.queryPh('materialConditionManagement', 'required')"
          show-count
          :maxlength="1"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('returnCode')">
      <a-form-item :label="pi.queryLabel('returnCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.returnCode"
          dict-type="logistics_return_code"
          :placeholder="pi.queryPh('returnCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('returnToLogisticsLevel')">
      <a-form-item :label="pi.queryLabel('returnToLogisticsLevel')">
        <TaktSelect
          v-model:value="advancedQueryForm.returnToLogisticsLevel"
          dict-type="logistics_return_to_logistics_level"
          :placeholder="pi.queryPh('returnToLogisticsLevel', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('natoItemIdentificationNumber')">
      <a-form-item :label="pi.queryLabel('natoItemIdentificationNumber')">
        <a-input
          v-model:value="advancedQueryForm.natoItemIdentificationNumber"
          :placeholder="pi.queryPh('natoItemIdentificationNumber', 'required')"
          show-count
          :maxlength="9"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fffClass')">
      <a-form-item :label="pi.queryLabel('fffClass')">
        <TaktSelect
          v-model:value="advancedQueryForm.fffClass"
          dict-type="logistics_fff_class"
          :placeholder="pi.queryPh('fffClass', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('supersessionChainNumber')">
      <a-form-item :label="pi.queryLabel('supersessionChainNumber')">
        <a-input
          v-model:value="advancedQueryForm.supersessionChainNumber"
          :placeholder="pi.queryPh('supersessionChainNumber', 'required')"
          show-count
          :maxlength="18"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('seasonalProcurementCreationStatus')">
      <a-form-item :label="pi.queryLabel('seasonalProcurementCreationStatus')">
        <TaktSelect
          v-model:value="advancedQueryForm.seasonalProcurementCreationStatus"
          dict-type="logistics_seasonal_procurement_creation_status"
          :placeholder="pi.queryPh('seasonalProcurementCreationStatus', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('colorCharacteristicInternalNumber')">
      <a-form-item :label="pi.queryLabel('colorCharacteristicInternalNumber')">
        <a-input
          v-model:value="advancedQueryForm.colorCharacteristicInternalNumber"
          :placeholder="pi.queryPh('colorCharacteristicInternalNumber', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mainSizeCharacteristicInternalNumber')">
      <a-form-item :label="pi.queryLabel('mainSizeCharacteristicInternalNumber')">
        <a-input
          v-model:value="advancedQueryForm.mainSizeCharacteristicInternalNumber"
          :placeholder="pi.queryPh('mainSizeCharacteristicInternalNumber', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('secondSizeCharacteristicInternalNumber')">
      <a-form-item :label="pi.queryLabel('secondSizeCharacteristicInternalNumber')">
        <a-input
          v-model:value="advancedQueryForm.secondSizeCharacteristicInternalNumber"
          :placeholder="pi.queryPh('secondSizeCharacteristicInternalNumber', 'required')"
          show-count
          :maxlength="10"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('color')">
      <a-form-item :label="pi.queryLabel('color')">
        <TaktSelect
          v-model:value="advancedQueryForm.color"
          dict-type="logistics_color"
          :placeholder="pi.queryPh('color', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('mainSize')">
      <a-form-item :label="pi.queryLabel('mainSize')">
        <TaktSelect
          v-model:value="advancedQueryForm.mainSize"
          dict-type="logistics_main_size"
          :placeholder="pi.queryPh('mainSize', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('secondSize')">
      <a-form-item :label="pi.queryLabel('secondSize')">
        <TaktSelect
          v-model:value="advancedQueryForm.secondSize"
          dict-type="logistics_second_size"
          :placeholder="pi.queryPh('secondSize', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('evaluationCharacteristicValue')">
      <a-form-item :label="pi.queryLabel('evaluationCharacteristicValue')">
        <TaktSelect
          v-model:value="advancedQueryForm.evaluationCharacteristicValue"
          dict-type="logistics_evaluation_characteristic_value"
          :placeholder="pi.queryPh('evaluationCharacteristicValue', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('careCode')">
      <a-form-item :label="pi.queryLabel('careCode')">
        <TaktSelect
          v-model:value="advancedQueryForm.careCode"
          dict-type="logistics_care_code"
          :placeholder="pi.queryPh('careCode', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('brandId')">
      <a-form-item :label="pi.queryLabel('brandId')">
        <TaktSelect
          v-model:value="advancedQueryForm.brandId"
          dict-type="logistics_brand_id"
          :placeholder="pi.queryPh('brandId', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberCode1')">
      <a-form-item :label="pi.queryLabel('fiberCode1')">
        <TaktSelect
          v-model:value="advancedQueryForm.fiberCode1"
          dict-type="logistics_fiber_code"
          :placeholder="pi.queryPh('fiberCode1', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberPart1')">
      <a-form-item :label="pi.queryLabel('fiberPart1')">
        <a-input
          v-model:value="advancedQueryForm.fiberPart1"
          :placeholder="pi.queryPh('fiberPart1', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberCode2')">
      <a-form-item :label="pi.queryLabel('fiberCode2')">
        <TaktSelect
          v-model:value="advancedQueryForm.fiberCode2"
          dict-type="logistics_fiber_code"
          :placeholder="pi.queryPh('fiberCode2', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberPart2')">
      <a-form-item :label="pi.queryLabel('fiberPart2')">
        <a-input
          v-model:value="advancedQueryForm.fiberPart2"
          :placeholder="pi.queryPh('fiberPart2', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberCode3')">
      <a-form-item :label="pi.queryLabel('fiberCode3')">
        <TaktSelect
          v-model:value="advancedQueryForm.fiberCode3"
          dict-type="logistics_fiber_code"
          :placeholder="pi.queryPh('fiberCode3', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberPart3')">
      <a-form-item :label="pi.queryLabel('fiberPart3')">
        <a-input
          v-model:value="advancedQueryForm.fiberPart3"
          :placeholder="pi.queryPh('fiberPart3', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberCode4')">
      <a-form-item :label="pi.queryLabel('fiberCode4')">
        <TaktSelect
          v-model:value="advancedQueryForm.fiberCode4"
          dict-type="logistics_fiber_code"
          :placeholder="pi.queryPh('fiberCode4', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberPart4')">
      <a-form-item :label="pi.queryLabel('fiberPart4')">
        <a-input
          v-model:value="advancedQueryForm.fiberPart4"
          :placeholder="pi.queryPh('fiberPart4', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberCode5')">
      <a-form-item :label="pi.queryLabel('fiberCode5')">
        <TaktSelect
          v-model:value="advancedQueryForm.fiberCode5"
          dict-type="logistics_fiber_code"
          :placeholder="pi.queryPh('fiberCode5', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fiberPart5')">
      <a-form-item :label="pi.queryLabel('fiberPart5')">
        <a-input
          v-model:value="advancedQueryForm.fiberPart5"
          :placeholder="pi.queryPh('fiberPart5', 'required')"
          show-count
          :maxlength="3"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('fashionGrade')">
      <a-form-item :label="pi.queryLabel('fashionGrade')">
        <TaktSelect
          v-model:value="advancedQueryForm.fashionGrade"
          dict-type="logistics_fashion_grade"
          :placeholder="pi.queryPh('fashionGrade', 'select')"
          allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtStart')">
      <a-form-item :label="pi.queryLabel('createdAtStart')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtStart"
          :placeholder="pi.queryPh('createdAtStart', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('createdAtEnd')">
      <a-form-item :label="pi.queryLabel('createdAtEnd')">
        <a-date-picker
          v-model:value="advancedQueryForm.createdAtEnd"
          :placeholder="pi.queryPh('createdAtEnd', 'select')"
          value-format="YYYY-MM-DD HH:mm:ss"
            show-time
          style="width: 100%"
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('extField')">
      <a-form-item
        name="extField"
        class="takt-form-item-ext-field"
        :label-col="{ style: { width: 'auto', maxWidth: 'none', flex: '0 0 auto' } }"
        :wrapper-col="{ style: { flex: '1 1 0', minWidth: 0 } }"
      >
        <template #label>
          <span class="takt-form-ext-field-label">
            <a-tooltip
              :title="t('common.page.entity.extfieldhint')"
              placement="top"
            >
              <span class="takt-form-label-hint-icon"><RiQuestionLine class="takt-remix-icon" /></span>
            </a-tooltip>
            <span>{{ pi.queryLabel('extField') }}</span>
          </span>
        </template>
        <a-textarea
          v-model:value="advancedQueryForm.extField"
          :placeholder="t('common.page.form.placeholder.extfield')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      <div v-show="isFieldVisible('remark')">
      <a-form-item :label="pi.queryLabel('remark')">
        <a-textarea
          v-model:value="advancedQueryForm.remark"
          :placeholder="pi.queryPh('remark', 'optional')"
            :rows="4"
            show-count
            :maxlength="400"
            allow-clear
        />
      </a-form-item>
      </div>
      </template>
    </TaktQueryDrawer>

    <!-- 导入对话框 -->
    <TaktModal
      v-model:open="importVisible"
      :title="t('common.dialog.title.import', { entity: pi.self() })"
      :width="600"
      :footer="null"
      :cancel-text="t('common.page.button.close')"
      @cancel="handleImportCancel"
    >
      <TaktImportFile
        v-if="importVisible"
        :entity-i18n-key="GENERALMATERIAL_SELF_I18N_KEY"
        file-type="xlsx"
        :sheet-name="excelNames.sheet"
        :template-file-name="excelNames.fileBase"
        :download-template="handleDownloadTemplate"
        :import-file="handleImportFile"
        :max-size="10"
        :max-rows="1000"
        @success="handleImportSuccess"
      />
    </TaktModal>
    <!-- 列设置抽屉 -->
    <TaktColumnDrawer
      v-model:open="columnSettingVisible"
      :columns="columns"
      :checked-keys="visibleColumnKeys"
      :id-column-key="'generalMaterialId'"
      :action-column-key="'action'"
      entity-scope="tenant"
      table-mode="single"
      @update:checked-keys="handleColumnKeysChange"
      @reset="handleColumnSettingReset"
    />
  </div>
</template>

<script setup lang="ts">
/**
 * Takt全局物料实体管理页 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/general-material
 */
import { message, Modal } from 'ant-design-vue'
import type { TableColumnsType } from 'ant-design-vue'
import { CreateActionColumn } from '@/components/business/TaktActionColumn/index'
import { useI18n } from 'vue-i18n'
import { useTaktContentModalWidth } from '@/composables/useTaktContentModalWidth'
import { ensureTaktPaginationConfigAsync, getTaktDefaultPageIndex, getTaktDefaultPageSize } from '@/utils/taktPaged'
import GeneralMaterialForm from './components/GeneralMaterialForm.vue'
import { getGeneralMaterialList, getGeneralMaterialById, createGeneralMaterial, updateGeneralMaterial, deleteGeneralMaterialById, deleteGeneralMaterialBatch, getGeneralMaterialTemplate, importGeneralMaterial, exportGeneralMaterial, updateGeneralMaterialStatus } from '@/api/logistics/materials/generalMaterial.api'
import type { GeneralMaterial, GeneralMaterialQuery } from '@/types/logistics/materials/generalMaterial.types'
import { useDictDataStore } from '@/stores/foundation/useDictDataStore'
import { taktExcelEntityNames } from '@/utils/naming'
import { resolveExportDownloadFileName } from '@/utils/exportDownloadName'
import { normalizeImportResult, type TaktImportResult } from '@/utils/taktImportResult'
import { RiEditLine, RiDeleteBinLine, RiQuestionLine } from '@remixicon/vue'

import {
  useGeneralMaterialI18n,
  GENERALMATERIAL_LIST_FIELDS,
  GENERALMATERIAL_QUERY_STRING_FIELDS,
  GENERALMATERIAL_QUERY_FIELDS,
  GENERALMATERIAL_SELF_I18N_KEY,
} from './composables/useGeneralMaterialI18n'

/** 实体字段 i18n（标签/占位符统一入口） */
const pi = useGeneralMaterialI18n()
/** 表格行类型（TaktSingleTable slot record 与 dataSource 行兼容） */
type GeneralMaterialRowRecord = GeneralMaterial | Record<string, unknown>
/** i18n 翻译函数 */
const { t } = useI18n()
/** Excel 导入/导出默认 sheet 名与文件名前缀 */
const excelNames = taktExcelEntityNames('TaktGeneralMaterial')
/** 列表快捷查询占位文案 */
const searchPlaceholder = computed(
  () => t('common.page.form.placeholder.search', { keyword: pi.self() })
)

/** 快捷查询关键字 */
const queryKeyword = ref('')
/** 列表 loading */
const loading = ref(false)
/** 分页列表数据 */
const dataSource = ref<GeneralMaterial[]>([])
/** 当前页码 */
const currentPage = ref(getTaktDefaultPageIndex())
/** 每页条数 */
const pageSize = ref(getTaktDefaultPageSize())
/** 分页 total */
const total = ref(0)
/** 工具栏单选时当前行 */
const selectedRow = ref<GeneralMaterialRowRecord | null>(null)
/** 表格多选行 */
const selectedRows = ref<GeneralMaterialRowRecord[]>([])
/** 表格多选 row-key 集合 */
const selectedRowKeys = ref<(string | number)[]>([])

/** 新增/编辑弹窗是否打开 */
const formVisible = ref(false)
/** 弹窗标题（新增/编辑） */
const formTitle = ref('')
/** 传入内嵌表单的编辑数据 */
const formData = ref<Partial<GeneralMaterial> | null>(null)
/** 表单提交 loading */
const formLoading = ref(false)
/** 内嵌表单组件 ref（validate / getValues / resetFields） */
const formRef = ref()
/** 表单弹窗宽度：（视口 − 左侧菜单）× 80% */
const formModalWidthPx = useTaktContentModalWidth()

/** 高级查询抽屉是否打开 */
const advancedQueryVisible = ref(false)
/**
 * 是否存在任一业务查询条件（分页除外）；无参时不请求列表/导出
 * @returns {boolean}
 */
function hasAnyListQueryFilter(): boolean {
  const kw = (queryKeyword.value ?? '').trim()
  if (kw.length > 0) {
    return true
  }
  const form = advancedQueryForm.value
  for (const key of GENERALMATERIAL_QUERY_STRING_FIELDS) {
    if (String(form[key] ?? '').trim().length > 0) {
      return true
    }
  }
  if (form.grossWeight !== undefined && form.grossWeight !== null) {
    return true
  }
  if (form.netWeight !== undefined && form.netWeight !== null) {
    return true
  }
  if (form.volume !== undefined && form.volume !== null) {
    return true
  }
  if (form.grGiSlipQuantity !== undefined && form.grGiSlipQuantity !== null) {
    return true
  }
  if (form.length !== undefined && form.length !== null) {
    return true
  }
  if (form.width !== undefined && form.width !== null) {
    return true
  }
  if (form.height !== undefined && form.height !== null) {
    return true
  }
  if (form.allowedPackagingWeight !== undefined && form.allowedPackagingWeight !== null) {
    return true
  }
  if (form.allowedPackagingVolume !== undefined && form.allowedPackagingVolume !== null) {
    return true
  }
  if (form.excessWeightTolerance !== undefined && form.excessWeightTolerance !== null) {
    return true
  }
  if (form.excessVolumeTolerance !== undefined && form.excessVolumeTolerance !== null) {
    return true
  }
  if (form.maximumLevelByVolume !== undefined && form.maximumLevelByVolume !== null) {
    return true
  }
  if (form.stackingFactor !== undefined && form.stackingFactor !== null) {
    return true
  }
  if (form.minimumRemainingShelfLife !== undefined && form.minimumRemainingShelfLife !== null) {
    return true
  }
  if (form.totalShelfLife !== undefined && form.totalShelfLife !== null) {
    return true
  }
  if (form.storagePercentage !== undefined && form.storagePercentage !== null) {
    return true
  }
  if (form.netContents !== undefined && form.netContents !== null) {
    return true
  }
  if (form.comparisonPriceUnit !== undefined && form.comparisonPriceUnit !== null) {
    return true
  }
  if (form.grossContents !== undefined && form.grossContents !== null) {
    return true
  }
  if (form.maximumAllowedCapacity !== undefined && form.maximumAllowedCapacity !== null) {
    return true
  }
  if (form.overcapacityTolerance !== undefined && form.overcapacityTolerance !== null) {
    return true
  }
  if (form.maximumPackingLength !== undefined && form.maximumPackingLength !== null) {
    return true
  }
  if (form.maximumPackingWidth !== undefined && form.maximumPackingWidth !== null) {
    return true
  }
  if (form.maximumPackingHeight !== undefined && form.maximumPackingHeight !== null) {
    return true
  }
  if (form.quarantinePeriod !== undefined && form.quarantinePeriod !== null) {
    return true
  }
  return false
}

/**
 * 创建空的高级查询表单（无默认填充；无参时列表保持空）
 * @returns {Record<string, unknown>} 高级查询初始模型
 */
function createEmptyAdvancedQueryForm() {
  const form = Object.fromEntries(GENERALMATERIAL_QUERY_STRING_FIELDS.map((key) => [key, ''])) as Record<
    (typeof GENERALMATERIAL_QUERY_STRING_FIELDS)[number],
    string
  >
  return {
    ...form,
    grossWeight: undefined as number | undefined,
    netWeight: undefined as number | undefined,
    volume: undefined as number | undefined,
    grGiSlipQuantity: undefined as number | undefined,
    length: undefined as number | undefined,
    width: undefined as number | undefined,
    height: undefined as number | undefined,
    allowedPackagingWeight: undefined as number | undefined,
    allowedPackagingVolume: undefined as number | undefined,
    excessWeightTolerance: undefined as number | undefined,
    excessVolumeTolerance: undefined as number | undefined,
    maximumLevelByVolume: undefined as number | undefined,
    stackingFactor: undefined as number | undefined,
    minimumRemainingShelfLife: undefined as number | undefined,
    totalShelfLife: undefined as number | undefined,
    storagePercentage: undefined as number | undefined,
    netContents: undefined as number | undefined,
    comparisonPriceUnit: undefined as number | undefined,
    grossContents: undefined as number | undefined,
    maximumAllowedCapacity: undefined as number | undefined,
    overcapacityTolerance: undefined as number | undefined,
    maximumPackingLength: undefined as number | undefined,
    maximumPackingWidth: undefined as number | undefined,
    maximumPackingHeight: undefined as number | undefined,
    quarantinePeriod: undefined as number | undefined,  }
}
/** 高级查询表单模型 */
const advancedQueryForm = ref(createEmptyAdvancedQueryForm())
/** 高级查询字段元数据（列显隐配置） */
const queryFieldsMeta = computed(() =>
  GENERALMATERIAL_QUERY_FIELDS.map((key) => ({ key, label: pi.queryLabel(key) })),
)
/** 高级查询当前可见字段 key */
const visibleQueryFieldKeys = ref<string[]>([])
/** 列设置抽屉是否打开 */
const columnSettingVisible = ref(false)
/** 导入对话框是否打开 */
const importVisible = ref(false)
/** 表格当前可见列 key */
const visibleColumnKeys = ref<string[]>([])
/** 实体主键字段名（row-key、API 路径参数） */
const entityIdName = 'generalMaterialId'
/** 工具栏「编辑」是否禁用（须恰好选中一行） */
const updateDisabled = computed(() => selectedRows.value.length !== 1)
/** 工具栏「删除」是否禁用（未选中任何行） */
const deleteDisabled = computed(() => selectedRows.value.length === 0)

/** Pinia：字典缓存（列表/查询 dict-type 渲染前预热） */
const dictDataStore = useDictDataStore()

/**
 * 构建列表/导出查询参数（空字符串与未填数值/日期不下发，避免后端 DateTime? 模型绑定 400；无参不补默认）
 * @param overrides 覆盖分页或导出上限等字段
 * @returns {GeneralMaterialQuery} 查询 DTO
 */
function buildListQuery(overrides?: Partial<GeneralMaterialQuery>): GeneralMaterialQuery {
  const form = advancedQueryForm.value
  const kw = (queryKeyword.value ?? '').trim()
  const query: GeneralMaterialQuery = {
    pageIndex: currentPage.value,
    pageSize: pageSize.value,
    ...overrides,
  }
  if (kw.length > 0) {
    query.keyWords = kw
  }
  const assignTrimmed = (key: keyof GeneralMaterialQuery, value: string | undefined) => {
    const v = (value ?? '').trim()
    if (v.length > 0) {
      query[key] = v as never
    }
  }
  for (const key of GENERALMATERIAL_QUERY_STRING_FIELDS) {
    assignTrimmed(key, form[key])
  }
  if (form.grossWeight !== undefined && form.grossWeight !== null) {
    query.grossWeight = form.grossWeight
  }
  if (form.netWeight !== undefined && form.netWeight !== null) {
    query.netWeight = form.netWeight
  }
  if (form.volume !== undefined && form.volume !== null) {
    query.volume = form.volume
  }
  if (form.grGiSlipQuantity !== undefined && form.grGiSlipQuantity !== null) {
    query.grGiSlipQuantity = form.grGiSlipQuantity
  }
  if (form.length !== undefined && form.length !== null) {
    query.length = form.length
  }
  if (form.width !== undefined && form.width !== null) {
    query.width = form.width
  }
  if (form.height !== undefined && form.height !== null) {
    query.height = form.height
  }
  if (form.allowedPackagingWeight !== undefined && form.allowedPackagingWeight !== null) {
    query.allowedPackagingWeight = form.allowedPackagingWeight
  }
  if (form.allowedPackagingVolume !== undefined && form.allowedPackagingVolume !== null) {
    query.allowedPackagingVolume = form.allowedPackagingVolume
  }
  if (form.excessWeightTolerance !== undefined && form.excessWeightTolerance !== null) {
    query.excessWeightTolerance = form.excessWeightTolerance
  }
  if (form.excessVolumeTolerance !== undefined && form.excessVolumeTolerance !== null) {
    query.excessVolumeTolerance = form.excessVolumeTolerance
  }
  if (form.maximumLevelByVolume !== undefined && form.maximumLevelByVolume !== null) {
    query.maximumLevelByVolume = form.maximumLevelByVolume
  }
  if (form.stackingFactor !== undefined && form.stackingFactor !== null) {
    query.stackingFactor = form.stackingFactor
  }
  if (form.minimumRemainingShelfLife !== undefined && form.minimumRemainingShelfLife !== null) {
    query.minimumRemainingShelfLife = form.minimumRemainingShelfLife
  }
  if (form.totalShelfLife !== undefined && form.totalShelfLife !== null) {
    query.totalShelfLife = form.totalShelfLife
  }
  if (form.storagePercentage !== undefined && form.storagePercentage !== null) {
    query.storagePercentage = form.storagePercentage
  }
  if (form.netContents !== undefined && form.netContents !== null) {
    query.netContents = form.netContents
  }
  if (form.comparisonPriceUnit !== undefined && form.comparisonPriceUnit !== null) {
    query.comparisonPriceUnit = form.comparisonPriceUnit
  }
  if (form.grossContents !== undefined && form.grossContents !== null) {
    query.grossContents = form.grossContents
  }
  if (form.maximumAllowedCapacity !== undefined && form.maximumAllowedCapacity !== null) {
    query.maximumAllowedCapacity = form.maximumAllowedCapacity
  }
  if (form.overcapacityTolerance !== undefined && form.overcapacityTolerance !== null) {
    query.overcapacityTolerance = form.overcapacityTolerance
  }
  if (form.maximumPackingLength !== undefined && form.maximumPackingLength !== null) {
    query.maximumPackingLength = form.maximumPackingLength
  }
  if (form.maximumPackingWidth !== undefined && form.maximumPackingWidth !== null) {
    query.maximumPackingWidth = form.maximumPackingWidth
  }
  if (form.maximumPackingHeight !== undefined && form.maximumPackingHeight !== null) {
    query.maximumPackingHeight = form.maximumPackingHeight
  }
  if (form.quarantinePeriod !== undefined && form.quarantinePeriod !== null) {
    query.quarantinePeriod = form.quarantinePeriod
  }
  return query
}
/** 页面挂载：租户上下文就绪后加载分页配置；无查询条件时 loadData 保持空表 */
onMounted(async () => {
  await ensureTaktPaginationConfigAsync()
  void dictDataStore.loadAllDictDataAsync()
  loadData()
})

/**
 * 构建列表标准文本列
 * @param key 列 key / dataIndex
 * @param title 列标题
 * @param options 宽度与固定列
 */
function buildGeneralMaterialListColumn(
  key: string,
  title: string,
  options?: { width?: number; fixed?: 'left' },
) {
  return {
    title,
    dataIndex: key,
    key,
    width: options?.width ?? 120,
    resizable: true,
    ellipsis: true,
    ...(options?.fixed ? { fixed: options.fixed } : {}),
  }
}

/** 表格列定义（i18n 随 locale 变化） */
const columns = computed<TableColumnsType>(() => [
  buildGeneralMaterialListColumn('generalMaterialId', t('common.page.entity.id'), { width: 80, fixed: 'left' }),
  ...GENERALMATERIAL_LIST_FIELDS.map((key) => buildGeneralMaterialListColumn(key, pi.label(key))),
  CreateActionColumn({
    actions: [
      {
        key: 'update',
        label: t('common.page.button.edit'),
        shape: 'plain',
        icon: RiEditLine,
        permission: 'logistics:materials:general:material:update',
        onClick: (record: GeneralMaterialRowRecord) => handleEdit(record)
      },
      {
        key: 'delete',
        label: t('common.page.button.delete'),
        shape: 'plain',
        icon: RiDeleteBinLine,
        permission: 'logistics:materials:general:material:delete',
        onClick: (record: GeneralMaterialRowRecord) => handleDeleteOne(record)
      }
    ]
  })
])

/** 表格 row-key（优先实体主键字段） */
const getGeneralMaterialId = (record: GeneralMaterialRowRecord): string => {
  const id = (record as Record<string, unknown>)?.[entityIdName]
  return id != null ? String(id) : ''
}
/**
 * 供 TaktDictTag 等组件使用的标量字典值
 * @param record 行数据
 * @param field 字段名
 */
const getGeneralMaterialDictValue = (
  record: GeneralMaterialRowRecord,
  field: string,
): string | number | undefined => {
  const value = (record as Record<string, unknown>)?.[field]
  if (value === null || value === undefined) return undefined
  if (typeof value === 'string' || typeof value === 'number') return value
  return String(value)
}

/** 行选择配置 */
const rowSelection = computed(() => ({
  selectedRowKeys: selectedRowKeys.value,
  onChange: (keys: (string | number)[], rows: GeneralMaterialRowRecord[]) => {
    selectedRowKeys.value = keys
    selectedRows.value = rows
    selectedRow.value = rows.length === 1 ? (rows[0] ?? null) : null
  },
  onSelect: (record: GeneralMaterialRowRecord, selected: boolean) => {
    if (selected) {
      selectedRow.value = record
    } else if (selectedRow.value && getGeneralMaterialId(selectedRow.value) === getGeneralMaterialId(record)) {
      selectedRow.value = null
    }
  },
  onSelectAll: (selected: boolean, selectedRowsData: GeneralMaterialRowRecord[]) => {
    selectedRow.value = selected && selectedRowsData.length === 1 ? (selectedRowsData[0] ?? null) : null
  }
}))

/** 行点击切换选中（与 rowSelection 联动） */
const onClickRow = (record: GeneralMaterialRowRecord) => ({
  onClick: () => {
    const key = getGeneralMaterialId(record)
    const index = selectedRowKeys.value.indexOf(key)
    if (index > -1) {
      selectedRowKeys.value.splice(index, 1)
    } else {
      selectedRowKeys.value.push(key)
    }
    selectedRows.value = dataSource.value.filter((item) => selectedRowKeys.value.includes(getGeneralMaterialId(item)))
    selectedRow.value = selectedRowKeys.value.length === 1 ? (selectedRows.value[0] ?? null) : null
    if (rowSelection.value.onChange) {
      rowSelection.value.onChange(selectedRowKeys.value, selectedRows.value)
    }
  }
})

/** 加载分页列表 */
async function loadData() {
  loading.value = true
  try {
    if (!hasAnyListQueryFilter()) {
      dataSource.value = []
      total.value = 0
      return
    }
    const res = await getGeneralMaterialList(buildListQuery())
    dataSource.value = res.data ?? []
    total.value = res.total ?? 0
  } catch (error: any) {
    logger.error('[GeneralMaterial] 加载数据失败', { error })
    message.error(error?.message || t('common.feedback.load.data.failed'))
    dataSource.value = []
    total.value = 0
  } finally {
    loading.value = false
  }
}

/** 租户/公司切换时由 bootstrap 发出 table:refresh，自动重载列表 */
useTableRefresh(loadData)

/** 快捷查询 */
function handleSearch() {
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 重置查询条件并刷新列表 */
function handleReset() {
  queryKeyword.value = ''
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

/** 打开新增弹窗 */
function handleCreate() {
  formTitle.value = t('common.dialog.title.create', { entity: pi.self() })
  formData.value = null
  formVisible.value = true
  nextTick(() => formRef.value?.resetFields())
}
/** 打开编辑弹窗（拉取详情，避免列表列裁剪字段） */
async function handleEdit(record: GeneralMaterialRowRecord) {
  const id = getGeneralMaterialId(record)
  if (!id) {
    return
  }
  formTitle.value = t('common.dialog.title.edit', { entity: pi.self() })
  formLoading.value = true
  try {
    const detail = await getGeneralMaterialById(id)
    formData.value = detail ?? ({ ...record } as Partial<GeneralMaterial>)
    formVisible.value = true
  } catch (error: unknown) {
    message.error(t('common.feedback.load.data.failed'))
  } finally {
    formLoading.value = false
  }
}

/** 工具栏编辑：打开当前单选行 */
function handleUpdate() {
  if (selectedRow.value) {
    void handleEdit(selectedRow.value)
  } else {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.edit'), entity: pi.self() }))
  }
}
/** 提交新增/编辑表单 */
async function handleFormSubmit() {
  const refInst = formRef.value
  if (!refInst?.validate) return
  try {
    await refInst.validate()
  } catch {
    return
  }
  formLoading.value = true
  try {
    const payload = refInst.getValues?.() ?? { ...(formData.value as any) }
    const id = (formData.value as any)?.[entityIdName]
    if (id) {
      await updateGeneralMaterial(id, payload as any)
      message.success(t('common.feedback.updated', { target: pi.self() }))
    } else {
      await createGeneralMaterial(payload as any)
      message.success(t('common.feedback.created', { target: pi.self() }))
    }
    formVisible.value = false
    formData.value = null
  nextTick(() => formRef.value?.resetFields())
    loadData()
  } finally {
    formLoading.value = false
  }
}

/** 关闭新增/编辑弹窗（不提交） */
function handleFormCancel() {
  formVisible.value = false
  formData.value = null
  nextTick(() => formRef.value?.resetFields())
}
/** 打开导入对话框 */
function handleImport() {
  importVisible.value = true
}

/** 下载导入模板 Excel */
async function handleDownloadTemplate(sheetName?: string, fileName?: string): Promise<Blob> {
  const res = await getGeneralMaterialTemplate(sheetName, fileName)
  return (res as any)?.data ?? res
}

/** 上传并导入 Excel 文件（归一化后端 SuccessCount/successCount） */
async function handleImportFile(file: File, sheetName?: string): Promise<TaktImportResult> {
  const raw = await importGeneralMaterial(file, sheetName)
  return normalizeImportResult(raw)
}

/** 导入完成回调：刷新列表；全部成功时延迟关闭对话框 */
function handleImportSuccess(result: TaktImportResult) {
  loadData()
  if (result.fail === 0 && result.success > 0) {
    setTimeout(() => { importVisible.value = false }, 2000)
  }
}

/** 关闭导入对话框 */
function handleImportCancel() {
  importVisible.value = false
}
/** 导出当前查询条件下的 Excel */
async function handleExport() {
  try {
    loading.value = true
    if (!hasAnyListQueryFilter()) {
      return
    }
    const exportMeta = await exportGeneralMaterial(
      buildListQuery({ pageIndex: 1, pageSize: 100000 }),
      excelNames.sheet,
      excelNames.fileBase
    )
    const ts = new Date()
    const pad = (n: number, w = 2) => String(n).padStart(w, '0')
    const fallbackBase = `${excelNames.fileBase}_${ts.getFullYear()}${pad(ts.getMonth() + 1)}${pad(ts.getDate())}${pad(ts.getHours())}${pad(ts.getMinutes())}${pad(ts.getSeconds())}`
    const fileName = resolveExportDownloadFileName({
      contentDisposition: (exportMeta as any).contentDisposition ?? null,
      contentType: (exportMeta as any).contentType ?? null,
      fallbackBase
    })
    const blob = (exportMeta as any).blob ?? exportMeta
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = fileName
    link.style.display = 'none'
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
    setTimeout(() => window.URL.revokeObjectURL(url), 100)
    message.success(t('common.feedback.export.success', { target: pi.self() }))
  } catch (error: any) {
    logger.error('[GeneralMaterial] 导出失败', { error })
    message.error(error?.message || t('common.feedback.export.failed', { target: pi.self() }))
  } finally {
    loading.value = false
  }
}
/** 删除单行 */
async function handleDeleteOne(record: GeneralMaterialRowRecord) {
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.entity', { entity: pi.self(), name: t('common.tip.this.target', { target: pi.self() }) }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      await deleteGeneralMaterialById((record as any)[entityIdName])
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    }
  })
}
/** 批量删除选中行 */
async function handleDelete() {
  if (selectedRows.value.length === 0) {
    message.warning(t('common.tip.select.to.action', { action: t('common.page.button.delete'), entity: pi.self() }))
    return
  }
  Modal.confirm({
    title: t('common.tip.confirm.delete.title'),
    content: t('common.tip.confirm.delete.count', { entity: pi.self(), count: selectedRows.value.length }),
    okText: t('common.page.button.delete'),
    cancelText: t('common.page.button.cancel'),
    onOk: async () => {
      const ids = selectedRows.value.map((r: any) => r[entityIdName]).filter(Boolean)
      await deleteGeneralMaterialBatch(ids)
      message.success(t('common.feedback.deleted', { target: pi.self() }))
      loadData()
    }
  })
}
/** 打开高级查询抽屉 */
function handleAdvancedQuery() {
  advancedQueryVisible.value = true
}

/** 高级查询提交：关闭抽屉并重置分页 */
function handleAdvancedQuerySubmit() {
  advancedQueryVisible.value = false
  currentPage.value = getTaktDefaultPageIndex()
  loadData()
}

function handleAdvancedQueryReset() {
  advancedQueryForm.value = createEmptyAdvancedQueryForm()
}

/** 打开列设置抽屉 */
function handleColumnSetting() {
  columnSettingVisible.value = true
}

/** 列设置：更新可见列 key */
function handleColumnKeysChange(keys: string[]) {
  visibleColumnKeys.value = keys
}

/** 列设置：恢复默认可见列 */
function handleColumnSettingReset() {
  visibleColumnKeys.value = []
}

/** 刷新列表 */
function handleRefresh() {
  loadData()
}

/** 表格 change 占位 */
function handleTableChange() {}
/** 列宽拖拽回调占位 */
function handleResizeColumn() {}
/** 分页页码变更 */
function handlePaginationChange(page: number, size: number) {
  currentPage.value = page
  pageSize.value = size
  loadData()
}

/** 分页每页条数变更（重置到第 1 页） */
function handlePaginationSizeChange(_current: number, size: number) {
  currentPage.value = getTaktDefaultPageIndex()
  pageSize.value = size
  loadData()
}
</script>
