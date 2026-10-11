// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktGeneralMaterialService.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：全局物料应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Materials;
using Takt.Domain.Entities.Logistics.Materials;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Materials;

/// <summary>
/// 全局物料应用服务
/// </summary>
public class TaktGeneralMaterialService : TaktServiceBase, ITaktGeneralMaterialService
{
    private readonly ITaktTenantRepository<TaktGeneralMaterial> _generalMaterialRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="generalMaterialRepository">全局物料仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktGeneralMaterialService(
        ITaktTenantRepository<TaktGeneralMaterial> generalMaterialRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _generalMaterialRepository = generalMaterialRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取全局物料列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktGeneralMaterialDto>> GetGeneralMaterialListAsync(TaktGeneralMaterialQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktGeneralMaterialDto>.Create(
                new List<TaktGeneralMaterialDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _generalMaterialRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktGeneralMaterialDto>.Create(
            data.Adapt<List<TaktGeneralMaterialDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取全局物料
    /// </summary>
    /// <param name="id">全局物料ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktGeneralMaterialDto?> GetGeneralMaterialByIdAsync(long id)
    {
        var entity = await _generalMaterialRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktGeneralMaterialDto>();
    }

    /// <summary>
    /// 获取全局物料选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetGeneralMaterialOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _generalMaterialRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.MaterialCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MaterialCode,
            DictLabel = e.MaterialCode,
        }).ToList();
    }

    /// <summary>
    /// 创建全局物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGeneralMaterialDto> CreateGeneralMaterialAsync(TaktGeneralMaterialCreateDto dto)
    {
        var entity = dto.Adapt<TaktGeneralMaterial>();
        var isUnique_ix_takt_logistics_materials_general_material_unique = await _uniqueValidator.IsUniqueAsync(
            _generalMaterialRepository,
            x => x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_materials_general_material_unique)
        {
            throw new TaktBusinessException("全局物料的MaterialCode已存在");
        }
        entity = await _generalMaterialRepository.CreateAsync(entity);
        return await GetGeneralMaterialByIdAsync(entity.Id) ?? entity.Adapt<TaktGeneralMaterialDto>();
    }

    /// <summary>
    /// 更新全局物料
    /// </summary>
    /// <param name="id">全局物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGeneralMaterialDto> UpdateGeneralMaterialAsync(long id, TaktGeneralMaterialUpdateDto dto)
    {
        var entity = await _generalMaterialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("全局物料不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_general_material_unique = await _uniqueValidator.IsUniqueAsync(
            _generalMaterialRepository,
            x => x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_general_material_unique)
        {
            throw new TaktBusinessException("全局物料的MaterialCode已存在");
        }
        await _generalMaterialRepository.UpdateAsync(entity);
        return await GetGeneralMaterialByIdAsync(id) ?? throw new TaktBusinessException("全局物料不存在");
    }

    /// <summary>
    /// 删除全局物料
    /// </summary>
    /// <param name="id">全局物料ID</param>
    /// <returns>任务</returns>
    public async Task DeleteGeneralMaterialByIdAsync(long id)
    {
        var deleted = await _generalMaterialRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("全局物料不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除全局物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteGeneralMaterialBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteGeneralMaterialByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新全局物料状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktGeneralMaterialDto> UpdateGeneralMaterialStatusAsync(TaktGeneralMaterialStatusDto dto)
    {
        var entity = await _generalMaterialRepository.GetByIdAsync(dto.GeneralMaterialId);
        if (entity == null)
        {
            throw new TaktBusinessException("全局物料不存在");
        }
        entity.CompleteMaintenanceStatus = dto.CompleteMaintenanceStatus;
        await _generalMaterialRepository.UpdateAsync(entity);
        return await GetGeneralMaterialByIdAsync(dto.GeneralMaterialId) ?? throw new TaktBusinessException("全局物料不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetGeneralMaterialTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktGeneralMaterialTemplateDto>(
            sheetName ?? "全局物料导入模板",
            fileName ?? "全局物料导入模板.xlsx");
    }

    /// <summary>
    /// 导入全局物料
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportGeneralMaterialAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktGeneralMaterialImportDto>(fileStream, sheetName ?? "全局物料导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktGeneralMaterial>();
                var importKey = $"{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（MaterialCode）");
                }
                var isUnique_ix_takt_logistics_materials_general_material_unique = await _uniqueValidator.IsUniqueAsync(
                    _generalMaterialRepository,
                    x => x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_materials_general_material_unique)
                {
                    throw new TaktBusinessException("全局物料的MaterialCode已存在");
                }
                await _generalMaterialRepository.CreateAsync(entity);
                success += 1;
            }
            catch (Exception ex)
            {
                fail += 1;
                errors.Add($"第{i + 2}行: {ex.Message}");
            }
        }
        return (success, fail, errors);
    }

    /// <summary>
    /// 导出全局物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportGeneralMaterialAsync(TaktGeneralMaterialQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktGeneralMaterialQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktGeneralMaterialExportDto>(),
                sheetName ?? "全局物料数据",
                fileName ?? "全局物料导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _generalMaterialRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktGeneralMaterialExportDto>(),
                sheetName ?? "全局物料数据",
                fileName ?? "全局物料导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktGeneralMaterialExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "全局物料数据",
            fileName ?? "全局物料导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建全局物料查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktGeneralMaterial, bool>> QueryExpression(TaktGeneralMaterialQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktGeneralMaterial>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.CompleteMaintenanceStatus != null && x.CompleteMaintenanceStatus.Contains(keywords))
                || (x.MaintenanceStatus != null && x.MaintenanceStatus.Contains(keywords))
                || (x.ClientDeletionFlag != null && x.ClientDeletionFlag.Contains(keywords))
                || (x.MaterialType != null && x.MaterialType.Contains(keywords))
                || (x.IndustrySector != null && x.IndustrySector.Contains(keywords))
                || (x.MaterialGroup != null && x.MaterialGroup.Contains(keywords))
                || (x.OldMaterialNumber != null && x.OldMaterialNumber.Contains(keywords))
                || (x.BaseUnit != null && x.BaseUnit.Contains(keywords))
                || (x.OrderUnit != null && x.OrderUnit.Contains(keywords))
                || (x.DocumentNumber != null && x.DocumentNumber.Contains(keywords))
                || (x.DocumentType != null && x.DocumentType.Contains(keywords))
                || (x.DocumentVersion != null && x.DocumentVersion.Contains(keywords))
                || (x.DocumentPageFormat != null && x.DocumentPageFormat.Contains(keywords))
                || (x.DocumentChangeNumber != null && x.DocumentChangeNumber.Contains(keywords))
                || (x.DocumentPageNumber != null && x.DocumentPageNumber.Contains(keywords))
                || (x.DocumentSheetCount != null && x.DocumentSheetCount.Contains(keywords))
                || (x.ProductionInspectionMemo != null && x.ProductionInspectionMemo.Contains(keywords))
                || (x.ProductionMemoPageFormat != null && x.ProductionMemoPageFormat.Contains(keywords))
                || (x.SizeDimensions != null && x.SizeDimensions.Contains(keywords))
                || (x.BasicMaterial != null && x.BasicMaterial.Contains(keywords))
                || (x.IndustryStandardDescription != null && x.IndustryStandardDescription.Contains(keywords))
                || (x.LaboratoryDesignOffice != null && x.LaboratoryDesignOffice.Contains(keywords))
                || (x.PurchasingValueKey != null && x.PurchasingValueKey.Contains(keywords))
                || (x.WeightUnit != null && x.WeightUnit.Contains(keywords))
                || (x.VolumeUnit != null && x.VolumeUnit.Contains(keywords))
                || (x.ContainerRequirements != null && x.ContainerRequirements.Contains(keywords))
                || (x.StorageConditions != null && x.StorageConditions.Contains(keywords))
                || (x.TemperatureConditions != null && x.TemperatureConditions.Contains(keywords))
                || (x.LowLevelCode != null && x.LowLevelCode.Contains(keywords))
                || (x.TransportationGroup != null && x.TransportationGroup.Contains(keywords))
                || (x.HazardousMaterialNumber != null && x.HazardousMaterialNumber.Contains(keywords))
                || (x.Division != null && x.Division.Contains(keywords))
                || (x.Competitor != null && x.Competitor.Contains(keywords))
                || (x.EuropeanArticleNumberObsolete != null && x.EuropeanArticleNumberObsolete.Contains(keywords))
                || (x.ProcurementRule != null && x.ProcurementRule.Contains(keywords))
                || (x.SourceOfSupply != null && x.SourceOfSupply.Contains(keywords))
                || (x.SeasonCategory != null && x.SeasonCategory.Contains(keywords))
                || (x.LabelType != null && x.LabelType.Contains(keywords))
                || (x.LabelForm != null && x.LabelForm.Contains(keywords))
                || (x.DeactivatedField != null && x.DeactivatedField.Contains(keywords))
                || (x.InternationalArticleNumber != null && x.InternationalArticleNumber.Contains(keywords))
                || (x.EanCategory != null && x.EanCategory.Contains(keywords))
                || (x.DimensionUnit != null && x.DimensionUnit.Contains(keywords))
                || (x.ProductHierarchy != null && x.ProductHierarchy.Contains(keywords))
                || (x.StockTransferNetChangeCosting != null && x.StockTransferNetChangeCosting.Contains(keywords))
                || (x.CadIndicator != null && x.CadIndicator.Contains(keywords))
                || (x.QmInProcurement != null && x.QmInProcurement.Contains(keywords))
                || (x.AllowedPackagingWeightUnit != null && x.AllowedPackagingWeightUnit.Contains(keywords))
                || (x.AllowedPackagingVolumeUnit != null && x.AllowedPackagingVolumeUnit.Contains(keywords))
                || (x.VariablePurchaseOrderUnit != null && x.VariablePurchaseOrderUnit.Contains(keywords))
                || (x.RevisionLevelAssigned != null && x.RevisionLevelAssigned.Contains(keywords))
                || (x.ConfigurableMaterial != null && x.ConfigurableMaterial.Contains(keywords))
                || (x.BatchManagementRequired != null && x.BatchManagementRequired.Contains(keywords))
                || (x.PackagingMaterialType != null && x.PackagingMaterialType.Contains(keywords))
                || (x.PackagingMaterialGroup != null && x.PackagingMaterialGroup.Contains(keywords))
                || (x.AuthorizationGroup != null && x.AuthorizationGroup.Contains(keywords))
                || (x.SeasonYear != null && x.SeasonYear.Contains(keywords))
                || (x.PriceBandCategory != null && x.PriceBandCategory.Contains(keywords))
                || (x.EmptiesBillOfMaterial != null && x.EmptiesBillOfMaterial.Contains(keywords))
                || (x.ExternalMaterialGroup != null && x.ExternalMaterialGroup.Contains(keywords))
                || (x.CrossPlantConfigurableMaterial != null && x.CrossPlantConfigurableMaterial.Contains(keywords))
                || (x.MaterialCategory != null && x.MaterialCategory.Contains(keywords))
                || (x.CoProductIndicator != null && x.CoProductIndicator.Contains(keywords))
                || (x.FollowUpMaterialIndicator != null && x.FollowUpMaterialIndicator.Contains(keywords))
                || (x.PricingReferenceMaterial != null && x.PricingReferenceMaterial.Contains(keywords))
                || (x.CrossPlantMaterialStatus != null && x.CrossPlantMaterialStatus.Contains(keywords))
                || (x.CrossDistributionChainStatus != null && x.CrossDistributionChainStatus.Contains(keywords))
                || (x.TaxClassification != null && x.TaxClassification.Contains(keywords))
                || (x.CatalogProfile != null && x.CatalogProfile.Contains(keywords))
                || (x.ContentUnit != null && x.ContentUnit.Contains(keywords))
                || (x.LabelingMaterialGrouping != null && x.LabelingMaterialGrouping.Contains(keywords))
                || (x.QuantityConversionMethod != null && x.QuantityConversionMethod.Contains(keywords))
                || (x.InternalObjectNumber != null && x.InternalObjectNumber.Contains(keywords))
                || (x.EnvironmentallyRelevant != null && x.EnvironmentallyRelevant.Contains(keywords))
                || (x.ProductAllocationProcedure != null && x.ProductAllocationProcedure.Contains(keywords))
                || (x.VariantPricingProfile != null && x.VariantPricingProfile.Contains(keywords))
                || (x.DiscountInKind != null && x.DiscountInKind.Contains(keywords))
                || (x.ManufacturerPartNumber != null && x.ManufacturerPartNumber.Contains(keywords))
                || (x.ManufacturerNumber != null && x.ManufacturerNumber.Contains(keywords))
                || (x.InventoryManagedMaterialNumber != null && x.InventoryManagedMaterialNumber.Contains(keywords))
                || (x.ManufacturerPartProfile != null && x.ManufacturerPartProfile.Contains(keywords))
                || (x.UnitsOfMeasureUsage != null && x.UnitsOfMeasureUsage.Contains(keywords))
                || (x.SeasonRollout != null && x.SeasonRollout.Contains(keywords))
                || (x.DangerousGoodsProfile != null && x.DangerousGoodsProfile.Contains(keywords))
                || (x.HighlyViscous != null && x.HighlyViscous.Contains(keywords))
                || (x.InBulkLiquid != null && x.InBulkLiquid.Contains(keywords))
                || (x.SerialNumberExplicitness != null && x.SerialNumberExplicitness.Contains(keywords))
                || (x.ClosedPackaging != null && x.ClosedPackaging.Contains(keywords))
                || (x.ApprovedBatchRecordRequired != null && x.ApprovedBatchRecordRequired.Contains(keywords))
                || (x.EffectivityParameterOverride != null && x.EffectivityParameterOverride.Contains(keywords))
                || (x.MaterialCompletionLevel != null && x.MaterialCompletionLevel.Contains(keywords))
                || (x.ShelfLifePeriodIndicator != null && x.ShelfLifePeriodIndicator.Contains(keywords))
                || (x.ShelfLifeRoundingRule != null && x.ShelfLifeRoundingRule.Contains(keywords))
                || (x.ProductCompositionOnPackaging != null && x.ProductCompositionOnPackaging.Contains(keywords))
                || (x.GeneralItemCategoryGroup != null && x.GeneralItemCategoryGroup.Contains(keywords))
                || (x.LogisticalVariants != null && x.LogisticalVariants.Contains(keywords))
                || (x.MaterialLocked != null && x.MaterialLocked.Contains(keywords))
                || (x.ConfigurationManagementRelevant != null && x.ConfigurationManagementRelevant.Contains(keywords))
                || (x.AssortmentListType != null && x.AssortmentListType.Contains(keywords))
                || (x.ExpirationDateType != null && x.ExpirationDateType.Contains(keywords))
                || (x.GtinVariant != null && x.GtinVariant.Contains(keywords))
                || (x.GenericMaterialNumber != null && x.GenericMaterialNumber.Contains(keywords))
                || (x.SamePackingReferenceMaterial != null && x.SamePackingReferenceMaterial.Contains(keywords))
                || (x.GlobalDataSyncRelevant != null && x.GlobalDataSyncRelevant.Contains(keywords))
                || (x.AcceptanceAtOrigin != null && x.AcceptanceAtOrigin.Contains(keywords))
                || (x.StandardHuType != null && x.StandardHuType.Contains(keywords))
                || (x.Pilferable != null && x.Pilferable.Contains(keywords))
                || (x.WarehouseStorageCondition != null && x.WarehouseStorageCondition.Contains(keywords))
                || (x.WarehouseMaterialGroup != null && x.WarehouseMaterialGroup.Contains(keywords))
                || (x.HandlingIndicator != null && x.HandlingIndicator.Contains(keywords))
                || (x.HazardousSubstancesRelevant != null && x.HazardousSubstancesRelevant.Contains(keywords))
                || (x.HandlingUnitType != null && x.HandlingUnitType.Contains(keywords))
                || (x.VariableTareWeight != null && x.VariableTareWeight.Contains(keywords))
                || (x.MaximumPackingDimensionUnit != null && x.MaximumPackingDimensionUnit.Contains(keywords))
                || (x.CountryOfOrigin != null && x.CountryOfOrigin.Contains(keywords))
                || (x.MaterialFreightGroup != null && x.MaterialFreightGroup.Contains(keywords))
                || (x.QuarantinePeriodUnit != null && x.QuarantinePeriodUnit.Contains(keywords))
                || (x.QualityInspectionGroup != null && x.QualityInspectionGroup.Contains(keywords))
                || (x.SerialNumberProfile != null && x.SerialNumberProfile.Contains(keywords))
                || (x.FormName != null && x.FormName.Contains(keywords))
                || (x.LogisticsUnitOfMeasure != null && x.LogisticsUnitOfMeasure.Contains(keywords))
                || (x.CatchWeightMaterial != null && x.CatchWeightMaterial.Contains(keywords))
                || (x.CatchWeightProfile != null && x.CatchWeightProfile.Contains(keywords))
                || (x.CatchWeightToleranceGroup != null && x.CatchWeightToleranceGroup.Contains(keywords))
                || (x.AdjustmentProfile != null && x.AdjustmentProfile.Contains(keywords))
                || (x.IntellectualPropertyId != null && x.IntellectualPropertyId.Contains(keywords))
                || (x.VariantPriceAllowed != null && x.VariantPriceAllowed.Contains(keywords))
                || (x.Medium != null && x.Medium.Contains(keywords))
                || (x.PhysicalCommodity != null && x.PhysicalCommodity.Contains(keywords))
                || (x.AnimalOrigin != null && x.AnimalOrigin.Contains(keywords))
                || (x.TextileCompositionFunction != null && x.TextileCompositionFunction.Contains(keywords))
                || (x.SegmentationStructure != null && x.SegmentationStructure.Contains(keywords))
                || (x.SegmentationStrategy != null && x.SegmentationStrategy.Contains(keywords))
                || (x.SegmentationStatus != null && x.SegmentationStatus.Contains(keywords))
                || (x.SegmentationScope != null && x.SegmentationScope.Contains(keywords))
                || (x.SegmentationRelevant != null && x.SegmentationRelevant.Contains(keywords))
                || (x.AnpCode != null && x.AnpCode.Contains(keywords))
                || (x.FashionAttribute1 != null && x.FashionAttribute1.Contains(keywords))
                || (x.FashionAttribute2 != null && x.FashionAttribute2.Contains(keywords))
                || (x.FashionAttribute3 != null && x.FashionAttribute3.Contains(keywords))
                || (x.SeasonUsageIndicator != null && x.SeasonUsageIndicator.Contains(keywords))
                || (x.SeasonActiveInInventory != null && x.SeasonActiveInInventory.Contains(keywords))
                || (x.CharacteristicConversionId != null && x.CharacteristicConversionId.Contains(keywords))
                || (x.PackagingCode != null && x.PackagingCode.Contains(keywords))
                || (x.DangerousGoodsPackagingStatus != null && x.DangerousGoodsPackagingStatus.Contains(keywords))
                || (x.MaterialConditionManagement != null && x.MaterialConditionManagement.Contains(keywords))
                || (x.ReturnCode != null && x.ReturnCode.Contains(keywords))
                || (x.ReturnToLogisticsLevel != null && x.ReturnToLogisticsLevel.Contains(keywords))
                || (x.NatoItemIdentificationNumber != null && x.NatoItemIdentificationNumber.Contains(keywords))
                || (x.FffClass != null && x.FffClass.Contains(keywords))
                || (x.SupersessionChainNumber != null && x.SupersessionChainNumber.Contains(keywords))
                || (x.SeasonalProcurementCreationStatus != null && x.SeasonalProcurementCreationStatus.Contains(keywords))
                || (x.ColorCharacteristicInternalNumber != null && x.ColorCharacteristicInternalNumber.Contains(keywords))
                || (x.MainSizeCharacteristicInternalNumber != null && x.MainSizeCharacteristicInternalNumber.Contains(keywords))
                || (x.SecondSizeCharacteristicInternalNumber != null && x.SecondSizeCharacteristicInternalNumber.Contains(keywords))
                || (x.Color != null && x.Color.Contains(keywords))
                || (x.MainSize != null && x.MainSize.Contains(keywords))
                || (x.SecondSize != null && x.SecondSize.Contains(keywords))
                || (x.EvaluationCharacteristicValue != null && x.EvaluationCharacteristicValue.Contains(keywords))
                || (x.CareCode != null && x.CareCode.Contains(keywords))
                || (x.BrandId != null && x.BrandId.Contains(keywords))
                || (x.FiberCode1 != null && x.FiberCode1.Contains(keywords))
                || (x.FiberPart1 != null && x.FiberPart1.Contains(keywords))
                || (x.FiberCode2 != null && x.FiberCode2.Contains(keywords))
                || (x.FiberPart2 != null && x.FiberPart2.Contains(keywords))
                || (x.FiberCode3 != null && x.FiberCode3.Contains(keywords))
                || (x.FiberPart3 != null && x.FiberPart3.Contains(keywords))
                || (x.FiberCode4 != null && x.FiberCode4.Contains(keywords))
                || (x.FiberPart4 != null && x.FiberPart4.Contains(keywords))
                || (x.FiberCode5 != null && x.FiberCode5.Contains(keywords))
                || (x.FiberPart5 != null && x.FiberPart5.Contains(keywords))
                || (x.FashionGrade != null && x.FashionGrade.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CompleteMaintenanceStatus))
        {
            var completeMaintenanceStatus = queryDto.CompleteMaintenanceStatus;
            exp = exp.And(x => x.CompleteMaintenanceStatus != null && x.CompleteMaintenanceStatus.Contains(completeMaintenanceStatus));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaintenanceStatus))
        {
            var maintenanceStatus = queryDto.MaintenanceStatus;
            exp = exp.And(x => x.MaintenanceStatus != null && x.MaintenanceStatus.Contains(maintenanceStatus));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClientDeletionFlag))
        {
            var clientDeletionFlag = queryDto.ClientDeletionFlag;
            exp = exp.And(x => x.ClientDeletionFlag != null && x.ClientDeletionFlag.Contains(clientDeletionFlag));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialType))
        {
            var materialType = queryDto.MaterialType;
            exp = exp.And(x => x.MaterialType != null && x.MaterialType.Contains(materialType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.IndustrySector))
        {
            var industrySector = queryDto.IndustrySector;
            exp = exp.And(x => x.IndustrySector != null && x.IndustrySector.Contains(industrySector));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialGroup))
        {
            var materialGroup = queryDto.MaterialGroup;
            exp = exp.And(x => x.MaterialGroup != null && x.MaterialGroup.Contains(materialGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OldMaterialNumber))
        {
            var oldMaterialNumber = queryDto.OldMaterialNumber;
            exp = exp.And(x => x.OldMaterialNumber != null && x.OldMaterialNumber.Contains(oldMaterialNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BaseUnit))
        {
            var baseUnit = queryDto.BaseUnit;
            exp = exp.And(x => x.BaseUnit != null && x.BaseUnit.Contains(baseUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OrderUnit))
        {
            var orderUnit = queryDto.OrderUnit;
            exp = exp.And(x => x.OrderUnit != null && x.OrderUnit.Contains(orderUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentNumber))
        {
            var documentNumber = queryDto.DocumentNumber;
            exp = exp.And(x => x.DocumentNumber != null && x.DocumentNumber.Contains(documentNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentType))
        {
            var documentType = queryDto.DocumentType;
            exp = exp.And(x => x.DocumentType != null && x.DocumentType.Contains(documentType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentVersion))
        {
            var documentVersion = queryDto.DocumentVersion;
            exp = exp.And(x => x.DocumentVersion != null && x.DocumentVersion.Contains(documentVersion));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentPageFormat))
        {
            var documentPageFormat = queryDto.DocumentPageFormat;
            exp = exp.And(x => x.DocumentPageFormat != null && x.DocumentPageFormat.Contains(documentPageFormat));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentChangeNumber))
        {
            var documentChangeNumber = queryDto.DocumentChangeNumber;
            exp = exp.And(x => x.DocumentChangeNumber != null && x.DocumentChangeNumber.Contains(documentChangeNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentPageNumber))
        {
            var documentPageNumber = queryDto.DocumentPageNumber;
            exp = exp.And(x => x.DocumentPageNumber != null && x.DocumentPageNumber.Contains(documentPageNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentSheetCount))
        {
            var documentSheetCount = queryDto.DocumentSheetCount;
            exp = exp.And(x => x.DocumentSheetCount != null && x.DocumentSheetCount.Contains(documentSheetCount));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductionInspectionMemo))
        {
            var productionInspectionMemo = queryDto.ProductionInspectionMemo;
            exp = exp.And(x => x.ProductionInspectionMemo != null && x.ProductionInspectionMemo.Contains(productionInspectionMemo));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductionMemoPageFormat))
        {
            var productionMemoPageFormat = queryDto.ProductionMemoPageFormat;
            exp = exp.And(x => x.ProductionMemoPageFormat != null && x.ProductionMemoPageFormat.Contains(productionMemoPageFormat));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SizeDimensions))
        {
            var sizeDimensions = queryDto.SizeDimensions;
            exp = exp.And(x => x.SizeDimensions != null && x.SizeDimensions.Contains(sizeDimensions));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BasicMaterial))
        {
            var basicMaterial = queryDto.BasicMaterial;
            exp = exp.And(x => x.BasicMaterial != null && x.BasicMaterial.Contains(basicMaterial));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.IndustryStandardDescription))
        {
            var industryStandardDescription = queryDto.IndustryStandardDescription;
            exp = exp.And(x => x.IndustryStandardDescription != null && x.IndustryStandardDescription.Contains(industryStandardDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LaboratoryDesignOffice))
        {
            var laboratoryDesignOffice = queryDto.LaboratoryDesignOffice;
            exp = exp.And(x => x.LaboratoryDesignOffice != null && x.LaboratoryDesignOffice.Contains(laboratoryDesignOffice));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchasingValueKey))
        {
            var purchasingValueKey = queryDto.PurchasingValueKey;
            exp = exp.And(x => x.PurchasingValueKey != null && x.PurchasingValueKey.Contains(purchasingValueKey));
        }

        if (queryDto?.GrossWeight.HasValue == true)
        {
            var grossWeight = queryDto.GrossWeight.Value;
            exp = exp.And(x => x.GrossWeight == grossWeight);
        }

        if (queryDto?.NetWeight.HasValue == true)
        {
            var netWeight = queryDto.NetWeight.Value;
            exp = exp.And(x => x.NetWeight == netWeight);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WeightUnit))
        {
            var weightUnit = queryDto.WeightUnit;
            exp = exp.And(x => x.WeightUnit != null && x.WeightUnit.Contains(weightUnit));
        }

        if (queryDto?.Volume.HasValue == true)
        {
            var volume = queryDto.Volume.Value;
            exp = exp.And(x => x.Volume == volume);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.VolumeUnit))
        {
            var volumeUnit = queryDto.VolumeUnit;
            exp = exp.And(x => x.VolumeUnit != null && x.VolumeUnit.Contains(volumeUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContainerRequirements))
        {
            var containerRequirements = queryDto.ContainerRequirements;
            exp = exp.And(x => x.ContainerRequirements != null && x.ContainerRequirements.Contains(containerRequirements));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StorageConditions))
        {
            var storageConditions = queryDto.StorageConditions;
            exp = exp.And(x => x.StorageConditions != null && x.StorageConditions.Contains(storageConditions));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TemperatureConditions))
        {
            var temperatureConditions = queryDto.TemperatureConditions;
            exp = exp.And(x => x.TemperatureConditions != null && x.TemperatureConditions.Contains(temperatureConditions));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LowLevelCode))
        {
            var lowLevelCode = queryDto.LowLevelCode;
            exp = exp.And(x => x.LowLevelCode != null && x.LowLevelCode.Contains(lowLevelCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TransportationGroup))
        {
            var transportationGroup = queryDto.TransportationGroup;
            exp = exp.And(x => x.TransportationGroup != null && x.TransportationGroup.Contains(transportationGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HazardousMaterialNumber))
        {
            var hazardousMaterialNumber = queryDto.HazardousMaterialNumber;
            exp = exp.And(x => x.HazardousMaterialNumber != null && x.HazardousMaterialNumber.Contains(hazardousMaterialNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Division))
        {
            var division = queryDto.Division;
            exp = exp.And(x => x.Division != null && x.Division.Contains(division));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Competitor))
        {
            var competitor = queryDto.Competitor;
            exp = exp.And(x => x.Competitor != null && x.Competitor.Contains(competitor));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EuropeanArticleNumberObsolete))
        {
            var europeanArticleNumberObsolete = queryDto.EuropeanArticleNumberObsolete;
            exp = exp.And(x => x.EuropeanArticleNumberObsolete != null && x.EuropeanArticleNumberObsolete.Contains(europeanArticleNumberObsolete));
        }

        if (queryDto?.GrGiSlipQuantity.HasValue == true)
        {
            var grGiSlipQuantity = queryDto.GrGiSlipQuantity.Value;
            exp = exp.And(x => x.GrGiSlipQuantity == grGiSlipQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProcurementRule))
        {
            var procurementRule = queryDto.ProcurementRule;
            exp = exp.And(x => x.ProcurementRule != null && x.ProcurementRule.Contains(procurementRule));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceOfSupply))
        {
            var sourceOfSupply = queryDto.SourceOfSupply;
            exp = exp.And(x => x.SourceOfSupply != null && x.SourceOfSupply.Contains(sourceOfSupply));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SeasonCategory))
        {
            var seasonCategory = queryDto.SeasonCategory;
            exp = exp.And(x => x.SeasonCategory != null && x.SeasonCategory.Contains(seasonCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LabelType))
        {
            var labelType = queryDto.LabelType;
            exp = exp.And(x => x.LabelType != null && x.LabelType.Contains(labelType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LabelForm))
        {
            var labelForm = queryDto.LabelForm;
            exp = exp.And(x => x.LabelForm != null && x.LabelForm.Contains(labelForm));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeactivatedField))
        {
            var deactivatedField = queryDto.DeactivatedField;
            exp = exp.And(x => x.DeactivatedField != null && x.DeactivatedField.Contains(deactivatedField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InternationalArticleNumber))
        {
            var internationalArticleNumber = queryDto.InternationalArticleNumber;
            exp = exp.And(x => x.InternationalArticleNumber != null && x.InternationalArticleNumber.Contains(internationalArticleNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EanCategory))
        {
            var eanCategory = queryDto.EanCategory;
            exp = exp.And(x => x.EanCategory != null && x.EanCategory.Contains(eanCategory));
        }

        if (queryDto?.Length.HasValue == true)
        {
            var length = queryDto.Length.Value;
            exp = exp.And(x => x.Length == length);
        }

        if (queryDto?.Width.HasValue == true)
        {
            var width = queryDto.Width.Value;
            exp = exp.And(x => x.Width == width);
        }

        if (queryDto?.Height.HasValue == true)
        {
            var height = queryDto.Height.Value;
            exp = exp.And(x => x.Height == height);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DimensionUnit))
        {
            var dimensionUnit = queryDto.DimensionUnit;
            exp = exp.And(x => x.DimensionUnit != null && x.DimensionUnit.Contains(dimensionUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductHierarchy))
        {
            var productHierarchy = queryDto.ProductHierarchy;
            exp = exp.And(x => x.ProductHierarchy != null && x.ProductHierarchy.Contains(productHierarchy));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StockTransferNetChangeCosting))
        {
            var stockTransferNetChangeCosting = queryDto.StockTransferNetChangeCosting;
            exp = exp.And(x => x.StockTransferNetChangeCosting != null && x.StockTransferNetChangeCosting.Contains(stockTransferNetChangeCosting));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CadIndicator))
        {
            var cadIndicator = queryDto.CadIndicator;
            exp = exp.And(x => x.CadIndicator != null && x.CadIndicator.Contains(cadIndicator));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.QmInProcurement))
        {
            var qmInProcurement = queryDto.QmInProcurement;
            exp = exp.And(x => x.QmInProcurement != null && x.QmInProcurement.Contains(qmInProcurement));
        }

        if (queryDto?.AllowedPackagingWeight.HasValue == true)
        {
            var allowedPackagingWeight = queryDto.AllowedPackagingWeight.Value;
            exp = exp.And(x => x.AllowedPackagingWeight == allowedPackagingWeight);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AllowedPackagingWeightUnit))
        {
            var allowedPackagingWeightUnit = queryDto.AllowedPackagingWeightUnit;
            exp = exp.And(x => x.AllowedPackagingWeightUnit != null && x.AllowedPackagingWeightUnit.Contains(allowedPackagingWeightUnit));
        }

        if (queryDto?.AllowedPackagingVolume.HasValue == true)
        {
            var allowedPackagingVolume = queryDto.AllowedPackagingVolume.Value;
            exp = exp.And(x => x.AllowedPackagingVolume == allowedPackagingVolume);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AllowedPackagingVolumeUnit))
        {
            var allowedPackagingVolumeUnit = queryDto.AllowedPackagingVolumeUnit;
            exp = exp.And(x => x.AllowedPackagingVolumeUnit != null && x.AllowedPackagingVolumeUnit.Contains(allowedPackagingVolumeUnit));
        }

        if (queryDto?.ExcessWeightTolerance.HasValue == true)
        {
            var excessWeightTolerance = queryDto.ExcessWeightTolerance.Value;
            exp = exp.And(x => x.ExcessWeightTolerance == excessWeightTolerance);
        }

        if (queryDto?.ExcessVolumeTolerance.HasValue == true)
        {
            var excessVolumeTolerance = queryDto.ExcessVolumeTolerance.Value;
            exp = exp.And(x => x.ExcessVolumeTolerance == excessVolumeTolerance);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.VariablePurchaseOrderUnit))
        {
            var variablePurchaseOrderUnit = queryDto.VariablePurchaseOrderUnit;
            exp = exp.And(x => x.VariablePurchaseOrderUnit != null && x.VariablePurchaseOrderUnit.Contains(variablePurchaseOrderUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RevisionLevelAssigned))
        {
            var revisionLevelAssigned = queryDto.RevisionLevelAssigned;
            exp = exp.And(x => x.RevisionLevelAssigned != null && x.RevisionLevelAssigned.Contains(revisionLevelAssigned));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConfigurableMaterial))
        {
            var configurableMaterial = queryDto.ConfigurableMaterial;
            exp = exp.And(x => x.ConfigurableMaterial != null && x.ConfigurableMaterial.Contains(configurableMaterial));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BatchManagementRequired))
        {
            var batchManagementRequired = queryDto.BatchManagementRequired;
            exp = exp.And(x => x.BatchManagementRequired != null && x.BatchManagementRequired.Contains(batchManagementRequired));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PackagingMaterialType))
        {
            var packagingMaterialType = queryDto.PackagingMaterialType;
            exp = exp.And(x => x.PackagingMaterialType != null && x.PackagingMaterialType.Contains(packagingMaterialType));
        }

        if (queryDto?.MaximumLevelByVolume.HasValue == true)
        {
            var maximumLevelByVolume = queryDto.MaximumLevelByVolume.Value;
            exp = exp.And(x => x.MaximumLevelByVolume == maximumLevelByVolume);
        }

        if (queryDto?.StackingFactor.HasValue == true)
        {
            var stackingFactor = queryDto.StackingFactor.Value;
            exp = exp.And(x => x.StackingFactor == stackingFactor);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PackagingMaterialGroup))
        {
            var packagingMaterialGroup = queryDto.PackagingMaterialGroup;
            exp = exp.And(x => x.PackagingMaterialGroup != null && x.PackagingMaterialGroup.Contains(packagingMaterialGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AuthorizationGroup))
        {
            var authorizationGroup = queryDto.AuthorizationGroup;
            exp = exp.And(x => x.AuthorizationGroup != null && x.AuthorizationGroup.Contains(authorizationGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SeasonYear))
        {
            var seasonYear = queryDto.SeasonYear;
            exp = exp.And(x => x.SeasonYear != null && x.SeasonYear.Contains(seasonYear));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PriceBandCategory))
        {
            var priceBandCategory = queryDto.PriceBandCategory;
            exp = exp.And(x => x.PriceBandCategory != null && x.PriceBandCategory.Contains(priceBandCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EmptiesBillOfMaterial))
        {
            var emptiesBillOfMaterial = queryDto.EmptiesBillOfMaterial;
            exp = exp.And(x => x.EmptiesBillOfMaterial != null && x.EmptiesBillOfMaterial.Contains(emptiesBillOfMaterial));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExternalMaterialGroup))
        {
            var externalMaterialGroup = queryDto.ExternalMaterialGroup;
            exp = exp.And(x => x.ExternalMaterialGroup != null && x.ExternalMaterialGroup.Contains(externalMaterialGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CrossPlantConfigurableMaterial))
        {
            var crossPlantConfigurableMaterial = queryDto.CrossPlantConfigurableMaterial;
            exp = exp.And(x => x.CrossPlantConfigurableMaterial != null && x.CrossPlantConfigurableMaterial.Contains(crossPlantConfigurableMaterial));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCategory))
        {
            var materialCategory = queryDto.MaterialCategory;
            exp = exp.And(x => x.MaterialCategory != null && x.MaterialCategory.Contains(materialCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CoProductIndicator))
        {
            var coProductIndicator = queryDto.CoProductIndicator;
            exp = exp.And(x => x.CoProductIndicator != null && x.CoProductIndicator.Contains(coProductIndicator));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FollowUpMaterialIndicator))
        {
            var followUpMaterialIndicator = queryDto.FollowUpMaterialIndicator;
            exp = exp.And(x => x.FollowUpMaterialIndicator != null && x.FollowUpMaterialIndicator.Contains(followUpMaterialIndicator));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PricingReferenceMaterial))
        {
            var pricingReferenceMaterial = queryDto.PricingReferenceMaterial;
            exp = exp.And(x => x.PricingReferenceMaterial != null && x.PricingReferenceMaterial.Contains(pricingReferenceMaterial));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CrossPlantMaterialStatus))
        {
            var crossPlantMaterialStatus = queryDto.CrossPlantMaterialStatus;
            exp = exp.And(x => x.CrossPlantMaterialStatus != null && x.CrossPlantMaterialStatus.Contains(crossPlantMaterialStatus));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CrossDistributionChainStatus))
        {
            var crossDistributionChainStatus = queryDto.CrossDistributionChainStatus;
            exp = exp.And(x => x.CrossDistributionChainStatus != null && x.CrossDistributionChainStatus.Contains(crossDistributionChainStatus));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxClassification))
        {
            var taxClassification = queryDto.TaxClassification;
            exp = exp.And(x => x.TaxClassification != null && x.TaxClassification.Contains(taxClassification));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CatalogProfile))
        {
            var catalogProfile = queryDto.CatalogProfile;
            exp = exp.And(x => x.CatalogProfile != null && x.CatalogProfile.Contains(catalogProfile));
        }

        if (queryDto?.MinimumRemainingShelfLife.HasValue == true)
        {
            var minimumRemainingShelfLife = queryDto.MinimumRemainingShelfLife.Value;
            exp = exp.And(x => x.MinimumRemainingShelfLife == minimumRemainingShelfLife);
        }

        if (queryDto?.TotalShelfLife.HasValue == true)
        {
            var totalShelfLife = queryDto.TotalShelfLife.Value;
            exp = exp.And(x => x.TotalShelfLife == totalShelfLife);
        }

        if (queryDto?.StoragePercentage.HasValue == true)
        {
            var storagePercentage = queryDto.StoragePercentage.Value;
            exp = exp.And(x => x.StoragePercentage == storagePercentage);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContentUnit))
        {
            var contentUnit = queryDto.ContentUnit;
            exp = exp.And(x => x.ContentUnit != null && x.ContentUnit.Contains(contentUnit));
        }

        if (queryDto?.NetContents.HasValue == true)
        {
            var netContents = queryDto.NetContents.Value;
            exp = exp.And(x => x.NetContents == netContents);
        }

        if (queryDto?.ComparisonPriceUnit.HasValue == true)
        {
            var comparisonPriceUnit = queryDto.ComparisonPriceUnit.Value;
            exp = exp.And(x => x.ComparisonPriceUnit == comparisonPriceUnit);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LabelingMaterialGrouping))
        {
            var labelingMaterialGrouping = queryDto.LabelingMaterialGrouping;
            exp = exp.And(x => x.LabelingMaterialGrouping != null && x.LabelingMaterialGrouping.Contains(labelingMaterialGrouping));
        }

        if (queryDto?.GrossContents.HasValue == true)
        {
            var grossContents = queryDto.GrossContents.Value;
            exp = exp.And(x => x.GrossContents == grossContents);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.QuantityConversionMethod))
        {
            var quantityConversionMethod = queryDto.QuantityConversionMethod;
            exp = exp.And(x => x.QuantityConversionMethod != null && x.QuantityConversionMethod.Contains(quantityConversionMethod));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InternalObjectNumber))
        {
            var internalObjectNumber = queryDto.InternalObjectNumber;
            exp = exp.And(x => x.InternalObjectNumber != null && x.InternalObjectNumber.Contains(internalObjectNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EnvironmentallyRelevant))
        {
            var environmentallyRelevant = queryDto.EnvironmentallyRelevant;
            exp = exp.And(x => x.EnvironmentallyRelevant != null && x.EnvironmentallyRelevant.Contains(environmentallyRelevant));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductAllocationProcedure))
        {
            var productAllocationProcedure = queryDto.ProductAllocationProcedure;
            exp = exp.And(x => x.ProductAllocationProcedure != null && x.ProductAllocationProcedure.Contains(productAllocationProcedure));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.VariantPricingProfile))
        {
            var variantPricingProfile = queryDto.VariantPricingProfile;
            exp = exp.And(x => x.VariantPricingProfile != null && x.VariantPricingProfile.Contains(variantPricingProfile));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DiscountInKind))
        {
            var discountInKind = queryDto.DiscountInKind;
            exp = exp.And(x => x.DiscountInKind != null && x.DiscountInKind.Contains(discountInKind));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ManufacturerPartNumber))
        {
            var manufacturerPartNumber = queryDto.ManufacturerPartNumber;
            exp = exp.And(x => x.ManufacturerPartNumber != null && x.ManufacturerPartNumber.Contains(manufacturerPartNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ManufacturerNumber))
        {
            var manufacturerNumber = queryDto.ManufacturerNumber;
            exp = exp.And(x => x.ManufacturerNumber != null && x.ManufacturerNumber.Contains(manufacturerNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InventoryManagedMaterialNumber))
        {
            var inventoryManagedMaterialNumber = queryDto.InventoryManagedMaterialNumber;
            exp = exp.And(x => x.InventoryManagedMaterialNumber != null && x.InventoryManagedMaterialNumber.Contains(inventoryManagedMaterialNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ManufacturerPartProfile))
        {
            var manufacturerPartProfile = queryDto.ManufacturerPartProfile;
            exp = exp.And(x => x.ManufacturerPartProfile != null && x.ManufacturerPartProfile.Contains(manufacturerPartProfile));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UnitsOfMeasureUsage))
        {
            var unitsOfMeasureUsage = queryDto.UnitsOfMeasureUsage;
            exp = exp.And(x => x.UnitsOfMeasureUsage != null && x.UnitsOfMeasureUsage.Contains(unitsOfMeasureUsage));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SeasonRollout))
        {
            var seasonRollout = queryDto.SeasonRollout;
            exp = exp.And(x => x.SeasonRollout != null && x.SeasonRollout.Contains(seasonRollout));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DangerousGoodsProfile))
        {
            var dangerousGoodsProfile = queryDto.DangerousGoodsProfile;
            exp = exp.And(x => x.DangerousGoodsProfile != null && x.DangerousGoodsProfile.Contains(dangerousGoodsProfile));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HighlyViscous))
        {
            var highlyViscous = queryDto.HighlyViscous;
            exp = exp.And(x => x.HighlyViscous != null && x.HighlyViscous.Contains(highlyViscous));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InBulkLiquid))
        {
            var inBulkLiquid = queryDto.InBulkLiquid;
            exp = exp.And(x => x.InBulkLiquid != null && x.InBulkLiquid.Contains(inBulkLiquid));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SerialNumberExplicitness))
        {
            var serialNumberExplicitness = queryDto.SerialNumberExplicitness;
            exp = exp.And(x => x.SerialNumberExplicitness != null && x.SerialNumberExplicitness.Contains(serialNumberExplicitness));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClosedPackaging))
        {
            var closedPackaging = queryDto.ClosedPackaging;
            exp = exp.And(x => x.ClosedPackaging != null && x.ClosedPackaging.Contains(closedPackaging));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApprovedBatchRecordRequired))
        {
            var approvedBatchRecordRequired = queryDto.ApprovedBatchRecordRequired;
            exp = exp.And(x => x.ApprovedBatchRecordRequired != null && x.ApprovedBatchRecordRequired.Contains(approvedBatchRecordRequired));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EffectivityParameterOverride))
        {
            var effectivityParameterOverride = queryDto.EffectivityParameterOverride;
            exp = exp.And(x => x.EffectivityParameterOverride != null && x.EffectivityParameterOverride.Contains(effectivityParameterOverride));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCompletionLevel))
        {
            var materialCompletionLevel = queryDto.MaterialCompletionLevel;
            exp = exp.And(x => x.MaterialCompletionLevel != null && x.MaterialCompletionLevel.Contains(materialCompletionLevel));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ShelfLifePeriodIndicator))
        {
            var shelfLifePeriodIndicator = queryDto.ShelfLifePeriodIndicator;
            exp = exp.And(x => x.ShelfLifePeriodIndicator != null && x.ShelfLifePeriodIndicator.Contains(shelfLifePeriodIndicator));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ShelfLifeRoundingRule))
        {
            var shelfLifeRoundingRule = queryDto.ShelfLifeRoundingRule;
            exp = exp.And(x => x.ShelfLifeRoundingRule != null && x.ShelfLifeRoundingRule.Contains(shelfLifeRoundingRule));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductCompositionOnPackaging))
        {
            var productCompositionOnPackaging = queryDto.ProductCompositionOnPackaging;
            exp = exp.And(x => x.ProductCompositionOnPackaging != null && x.ProductCompositionOnPackaging.Contains(productCompositionOnPackaging));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GeneralItemCategoryGroup))
        {
            var generalItemCategoryGroup = queryDto.GeneralItemCategoryGroup;
            exp = exp.And(x => x.GeneralItemCategoryGroup != null && x.GeneralItemCategoryGroup.Contains(generalItemCategoryGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LogisticalVariants))
        {
            var logisticalVariants = queryDto.LogisticalVariants;
            exp = exp.And(x => x.LogisticalVariants != null && x.LogisticalVariants.Contains(logisticalVariants));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialLocked))
        {
            var materialLocked = queryDto.MaterialLocked;
            exp = exp.And(x => x.MaterialLocked != null && x.MaterialLocked.Contains(materialLocked));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConfigurationManagementRelevant))
        {
            var configurationManagementRelevant = queryDto.ConfigurationManagementRelevant;
            exp = exp.And(x => x.ConfigurationManagementRelevant != null && x.ConfigurationManagementRelevant.Contains(configurationManagementRelevant));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssortmentListType))
        {
            var assortmentListType = queryDto.AssortmentListType;
            exp = exp.And(x => x.AssortmentListType != null && x.AssortmentListType.Contains(assortmentListType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExpirationDateType))
        {
            var expirationDateType = queryDto.ExpirationDateType;
            exp = exp.And(x => x.ExpirationDateType != null && x.ExpirationDateType.Contains(expirationDateType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GtinVariant))
        {
            var gtinVariant = queryDto.GtinVariant;
            exp = exp.And(x => x.GtinVariant != null && x.GtinVariant.Contains(gtinVariant));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GenericMaterialNumber))
        {
            var genericMaterialNumber = queryDto.GenericMaterialNumber;
            exp = exp.And(x => x.GenericMaterialNumber != null && x.GenericMaterialNumber.Contains(genericMaterialNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SamePackingReferenceMaterial))
        {
            var samePackingReferenceMaterial = queryDto.SamePackingReferenceMaterial;
            exp = exp.And(x => x.SamePackingReferenceMaterial != null && x.SamePackingReferenceMaterial.Contains(samePackingReferenceMaterial));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.GlobalDataSyncRelevant))
        {
            var globalDataSyncRelevant = queryDto.GlobalDataSyncRelevant;
            exp = exp.And(x => x.GlobalDataSyncRelevant != null && x.GlobalDataSyncRelevant.Contains(globalDataSyncRelevant));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AcceptanceAtOrigin))
        {
            var acceptanceAtOrigin = queryDto.AcceptanceAtOrigin;
            exp = exp.And(x => x.AcceptanceAtOrigin != null && x.AcceptanceAtOrigin.Contains(acceptanceAtOrigin));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StandardHuType))
        {
            var standardHuType = queryDto.StandardHuType;
            exp = exp.And(x => x.StandardHuType != null && x.StandardHuType.Contains(standardHuType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Pilferable))
        {
            var pilferable = queryDto.Pilferable;
            exp = exp.And(x => x.Pilferable != null && x.Pilferable.Contains(pilferable));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WarehouseStorageCondition))
        {
            var warehouseStorageCondition = queryDto.WarehouseStorageCondition;
            exp = exp.And(x => x.WarehouseStorageCondition != null && x.WarehouseStorageCondition.Contains(warehouseStorageCondition));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WarehouseMaterialGroup))
        {
            var warehouseMaterialGroup = queryDto.WarehouseMaterialGroup;
            exp = exp.And(x => x.WarehouseMaterialGroup != null && x.WarehouseMaterialGroup.Contains(warehouseMaterialGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HandlingIndicator))
        {
            var handlingIndicator = queryDto.HandlingIndicator;
            exp = exp.And(x => x.HandlingIndicator != null && x.HandlingIndicator.Contains(handlingIndicator));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HazardousSubstancesRelevant))
        {
            var hazardousSubstancesRelevant = queryDto.HazardousSubstancesRelevant;
            exp = exp.And(x => x.HazardousSubstancesRelevant != null && x.HazardousSubstancesRelevant.Contains(hazardousSubstancesRelevant));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HandlingUnitType))
        {
            var handlingUnitType = queryDto.HandlingUnitType;
            exp = exp.And(x => x.HandlingUnitType != null && x.HandlingUnitType.Contains(handlingUnitType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.VariableTareWeight))
        {
            var variableTareWeight = queryDto.VariableTareWeight;
            exp = exp.And(x => x.VariableTareWeight != null && x.VariableTareWeight.Contains(variableTareWeight));
        }

        if (queryDto?.MaximumAllowedCapacity.HasValue == true)
        {
            var maximumAllowedCapacity = queryDto.MaximumAllowedCapacity.Value;
            exp = exp.And(x => x.MaximumAllowedCapacity == maximumAllowedCapacity);
        }

        if (queryDto?.OvercapacityTolerance.HasValue == true)
        {
            var overcapacityTolerance = queryDto.OvercapacityTolerance.Value;
            exp = exp.And(x => x.OvercapacityTolerance == overcapacityTolerance);
        }

        if (queryDto?.MaximumPackingLength.HasValue == true)
        {
            var maximumPackingLength = queryDto.MaximumPackingLength.Value;
            exp = exp.And(x => x.MaximumPackingLength == maximumPackingLength);
        }

        if (queryDto?.MaximumPackingWidth.HasValue == true)
        {
            var maximumPackingWidth = queryDto.MaximumPackingWidth.Value;
            exp = exp.And(x => x.MaximumPackingWidth == maximumPackingWidth);
        }

        if (queryDto?.MaximumPackingHeight.HasValue == true)
        {
            var maximumPackingHeight = queryDto.MaximumPackingHeight.Value;
            exp = exp.And(x => x.MaximumPackingHeight == maximumPackingHeight);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaximumPackingDimensionUnit))
        {
            var maximumPackingDimensionUnit = queryDto.MaximumPackingDimensionUnit;
            exp = exp.And(x => x.MaximumPackingDimensionUnit != null && x.MaximumPackingDimensionUnit.Contains(maximumPackingDimensionUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CountryOfOrigin))
        {
            var countryOfOrigin = queryDto.CountryOfOrigin;
            exp = exp.And(x => x.CountryOfOrigin != null && x.CountryOfOrigin.Contains(countryOfOrigin));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialFreightGroup))
        {
            var materialFreightGroup = queryDto.MaterialFreightGroup;
            exp = exp.And(x => x.MaterialFreightGroup != null && x.MaterialFreightGroup.Contains(materialFreightGroup));
        }

        if (queryDto?.QuarantinePeriod.HasValue == true)
        {
            var quarantinePeriod = queryDto.QuarantinePeriod.Value;
            exp = exp.And(x => x.QuarantinePeriod == quarantinePeriod);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.QuarantinePeriodUnit))
        {
            var quarantinePeriodUnit = queryDto.QuarantinePeriodUnit;
            exp = exp.And(x => x.QuarantinePeriodUnit != null && x.QuarantinePeriodUnit.Contains(quarantinePeriodUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.QualityInspectionGroup))
        {
            var qualityInspectionGroup = queryDto.QualityInspectionGroup;
            exp = exp.And(x => x.QualityInspectionGroup != null && x.QualityInspectionGroup.Contains(qualityInspectionGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SerialNumberProfile))
        {
            var serialNumberProfile = queryDto.SerialNumberProfile;
            exp = exp.And(x => x.SerialNumberProfile != null && x.SerialNumberProfile.Contains(serialNumberProfile));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FormName))
        {
            var formName = queryDto.FormName;
            exp = exp.And(x => x.FormName != null && x.FormName.Contains(formName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LogisticsUnitOfMeasure))
        {
            var logisticsUnitOfMeasure = queryDto.LogisticsUnitOfMeasure;
            exp = exp.And(x => x.LogisticsUnitOfMeasure != null && x.LogisticsUnitOfMeasure.Contains(logisticsUnitOfMeasure));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CatchWeightMaterial))
        {
            var catchWeightMaterial = queryDto.CatchWeightMaterial;
            exp = exp.And(x => x.CatchWeightMaterial != null && x.CatchWeightMaterial.Contains(catchWeightMaterial));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CatchWeightProfile))
        {
            var catchWeightProfile = queryDto.CatchWeightProfile;
            exp = exp.And(x => x.CatchWeightProfile != null && x.CatchWeightProfile.Contains(catchWeightProfile));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CatchWeightToleranceGroup))
        {
            var catchWeightToleranceGroup = queryDto.CatchWeightToleranceGroup;
            exp = exp.And(x => x.CatchWeightToleranceGroup != null && x.CatchWeightToleranceGroup.Contains(catchWeightToleranceGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AdjustmentProfile))
        {
            var adjustmentProfile = queryDto.AdjustmentProfile;
            exp = exp.And(x => x.AdjustmentProfile != null && x.AdjustmentProfile.Contains(adjustmentProfile));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.IntellectualPropertyId))
        {
            var intellectualPropertyId = queryDto.IntellectualPropertyId;
            exp = exp.And(x => x.IntellectualPropertyId != null && x.IntellectualPropertyId.Contains(intellectualPropertyId));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.VariantPriceAllowed))
        {
            var variantPriceAllowed = queryDto.VariantPriceAllowed;
            exp = exp.And(x => x.VariantPriceAllowed != null && x.VariantPriceAllowed.Contains(variantPriceAllowed));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Medium))
        {
            var medium = queryDto.Medium;
            exp = exp.And(x => x.Medium != null && x.Medium.Contains(medium));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PhysicalCommodity))
        {
            var physicalCommodity = queryDto.PhysicalCommodity;
            exp = exp.And(x => x.PhysicalCommodity != null && x.PhysicalCommodity.Contains(physicalCommodity));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AnimalOrigin))
        {
            var animalOrigin = queryDto.AnimalOrigin;
            exp = exp.And(x => x.AnimalOrigin != null && x.AnimalOrigin.Contains(animalOrigin));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TextileCompositionFunction))
        {
            var textileCompositionFunction = queryDto.TextileCompositionFunction;
            exp = exp.And(x => x.TextileCompositionFunction != null && x.TextileCompositionFunction.Contains(textileCompositionFunction));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SegmentationStructure))
        {
            var segmentationStructure = queryDto.SegmentationStructure;
            exp = exp.And(x => x.SegmentationStructure != null && x.SegmentationStructure.Contains(segmentationStructure));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SegmentationStrategy))
        {
            var segmentationStrategy = queryDto.SegmentationStrategy;
            exp = exp.And(x => x.SegmentationStrategy != null && x.SegmentationStrategy.Contains(segmentationStrategy));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SegmentationStatus))
        {
            var segmentationStatus = queryDto.SegmentationStatus;
            exp = exp.And(x => x.SegmentationStatus != null && x.SegmentationStatus.Contains(segmentationStatus));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SegmentationScope))
        {
            var segmentationScope = queryDto.SegmentationScope;
            exp = exp.And(x => x.SegmentationScope != null && x.SegmentationScope.Contains(segmentationScope));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SegmentationRelevant))
        {
            var segmentationRelevant = queryDto.SegmentationRelevant;
            exp = exp.And(x => x.SegmentationRelevant != null && x.SegmentationRelevant.Contains(segmentationRelevant));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AnpCode))
        {
            var anpCode = queryDto.AnpCode;
            exp = exp.And(x => x.AnpCode != null && x.AnpCode.Contains(anpCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FashionAttribute1))
        {
            var fashionAttribute1 = queryDto.FashionAttribute1;
            exp = exp.And(x => x.FashionAttribute1 != null && x.FashionAttribute1.Contains(fashionAttribute1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FashionAttribute2))
        {
            var fashionAttribute2 = queryDto.FashionAttribute2;
            exp = exp.And(x => x.FashionAttribute2 != null && x.FashionAttribute2.Contains(fashionAttribute2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FashionAttribute3))
        {
            var fashionAttribute3 = queryDto.FashionAttribute3;
            exp = exp.And(x => x.FashionAttribute3 != null && x.FashionAttribute3.Contains(fashionAttribute3));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SeasonUsageIndicator))
        {
            var seasonUsageIndicator = queryDto.SeasonUsageIndicator;
            exp = exp.And(x => x.SeasonUsageIndicator != null && x.SeasonUsageIndicator.Contains(seasonUsageIndicator));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SeasonActiveInInventory))
        {
            var seasonActiveInInventory = queryDto.SeasonActiveInInventory;
            exp = exp.And(x => x.SeasonActiveInInventory != null && x.SeasonActiveInInventory.Contains(seasonActiveInInventory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CharacteristicConversionId))
        {
            var characteristicConversionId = queryDto.CharacteristicConversionId;
            exp = exp.And(x => x.CharacteristicConversionId != null && x.CharacteristicConversionId.Contains(characteristicConversionId));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PackagingCode))
        {
            var packagingCode = queryDto.PackagingCode;
            exp = exp.And(x => x.PackagingCode != null && x.PackagingCode.Contains(packagingCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DangerousGoodsPackagingStatus))
        {
            var dangerousGoodsPackagingStatus = queryDto.DangerousGoodsPackagingStatus;
            exp = exp.And(x => x.DangerousGoodsPackagingStatus != null && x.DangerousGoodsPackagingStatus.Contains(dangerousGoodsPackagingStatus));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialConditionManagement))
        {
            var materialConditionManagement = queryDto.MaterialConditionManagement;
            exp = exp.And(x => x.MaterialConditionManagement != null && x.MaterialConditionManagement.Contains(materialConditionManagement));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReturnCode))
        {
            var returnCode = queryDto.ReturnCode;
            exp = exp.And(x => x.ReturnCode != null && x.ReturnCode.Contains(returnCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReturnToLogisticsLevel))
        {
            var returnToLogisticsLevel = queryDto.ReturnToLogisticsLevel;
            exp = exp.And(x => x.ReturnToLogisticsLevel != null && x.ReturnToLogisticsLevel.Contains(returnToLogisticsLevel));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.NatoItemIdentificationNumber))
        {
            var natoItemIdentificationNumber = queryDto.NatoItemIdentificationNumber;
            exp = exp.And(x => x.NatoItemIdentificationNumber != null && x.NatoItemIdentificationNumber.Contains(natoItemIdentificationNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FffClass))
        {
            var fffClass = queryDto.FffClass;
            exp = exp.And(x => x.FffClass != null && x.FffClass.Contains(fffClass));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupersessionChainNumber))
        {
            var supersessionChainNumber = queryDto.SupersessionChainNumber;
            exp = exp.And(x => x.SupersessionChainNumber != null && x.SupersessionChainNumber.Contains(supersessionChainNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SeasonalProcurementCreationStatus))
        {
            var seasonalProcurementCreationStatus = queryDto.SeasonalProcurementCreationStatus;
            exp = exp.And(x => x.SeasonalProcurementCreationStatus != null && x.SeasonalProcurementCreationStatus.Contains(seasonalProcurementCreationStatus));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ColorCharacteristicInternalNumber))
        {
            var colorCharacteristicInternalNumber = queryDto.ColorCharacteristicInternalNumber;
            exp = exp.And(x => x.ColorCharacteristicInternalNumber != null && x.ColorCharacteristicInternalNumber.Contains(colorCharacteristicInternalNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MainSizeCharacteristicInternalNumber))
        {
            var mainSizeCharacteristicInternalNumber = queryDto.MainSizeCharacteristicInternalNumber;
            exp = exp.And(x => x.MainSizeCharacteristicInternalNumber != null && x.MainSizeCharacteristicInternalNumber.Contains(mainSizeCharacteristicInternalNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SecondSizeCharacteristicInternalNumber))
        {
            var secondSizeCharacteristicInternalNumber = queryDto.SecondSizeCharacteristicInternalNumber;
            exp = exp.And(x => x.SecondSizeCharacteristicInternalNumber != null && x.SecondSizeCharacteristicInternalNumber.Contains(secondSizeCharacteristicInternalNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Color))
        {
            var color = queryDto.Color;
            exp = exp.And(x => x.Color != null && x.Color.Contains(color));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MainSize))
        {
            var mainSize = queryDto.MainSize;
            exp = exp.And(x => x.MainSize != null && x.MainSize.Contains(mainSize));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SecondSize))
        {
            var secondSize = queryDto.SecondSize;
            exp = exp.And(x => x.SecondSize != null && x.SecondSize.Contains(secondSize));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EvaluationCharacteristicValue))
        {
            var evaluationCharacteristicValue = queryDto.EvaluationCharacteristicValue;
            exp = exp.And(x => x.EvaluationCharacteristicValue != null && x.EvaluationCharacteristicValue.Contains(evaluationCharacteristicValue));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CareCode))
        {
            var careCode = queryDto.CareCode;
            exp = exp.And(x => x.CareCode != null && x.CareCode.Contains(careCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BrandId))
        {
            var brandId = queryDto.BrandId;
            exp = exp.And(x => x.BrandId != null && x.BrandId.Contains(brandId));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiberCode1))
        {
            var fiberCode1 = queryDto.FiberCode1;
            exp = exp.And(x => x.FiberCode1 != null && x.FiberCode1.Contains(fiberCode1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiberPart1))
        {
            var fiberPart1 = queryDto.FiberPart1;
            exp = exp.And(x => x.FiberPart1 != null && x.FiberPart1.Contains(fiberPart1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiberCode2))
        {
            var fiberCode2 = queryDto.FiberCode2;
            exp = exp.And(x => x.FiberCode2 != null && x.FiberCode2.Contains(fiberCode2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiberPart2))
        {
            var fiberPart2 = queryDto.FiberPart2;
            exp = exp.And(x => x.FiberPart2 != null && x.FiberPart2.Contains(fiberPart2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiberCode3))
        {
            var fiberCode3 = queryDto.FiberCode3;
            exp = exp.And(x => x.FiberCode3 != null && x.FiberCode3.Contains(fiberCode3));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiberPart3))
        {
            var fiberPart3 = queryDto.FiberPart3;
            exp = exp.And(x => x.FiberPart3 != null && x.FiberPart3.Contains(fiberPart3));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiberCode4))
        {
            var fiberCode4 = queryDto.FiberCode4;
            exp = exp.And(x => x.FiberCode4 != null && x.FiberCode4.Contains(fiberCode4));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiberPart4))
        {
            var fiberPart4 = queryDto.FiberPart4;
            exp = exp.And(x => x.FiberPart4 != null && x.FiberPart4.Contains(fiberPart4));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiberCode5))
        {
            var fiberCode5 = queryDto.FiberCode5;
            exp = exp.And(x => x.FiberCode5 != null && x.FiberCode5.Contains(fiberCode5));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiberPart5))
        {
            var fiberPart5 = queryDto.FiberPart5;
            exp = exp.And(x => x.FiberPart5 != null && x.FiberPart5.Contains(fiberPart5));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FashionGrade))
        {
            var fashionGrade = queryDto.FashionGrade;
            exp = exp.And(x => x.FashionGrade != null && x.FashionGrade.Contains(fashionGrade));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExtField))
        {
            var extField = queryDto.ExtField;
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(extField));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Remark))
        {
            var remark = queryDto.Remark;
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(remark));
        }

        if (queryDto?.ValidFromDateStart.HasValue == true)
        {
            var validFromDateStart = queryDto.ValidFromDateStart.Value;
            exp = exp.And(x => x.ValidFromDate >= validFromDateStart);
        }

        if (queryDto?.ValidFromDateEnd.HasValue == true)
        {
            var validFromDateEnd = queryDto.ValidFromDateEnd.Value;
            exp = exp.And(x => x.ValidFromDate <= validFromDateEnd);
        }

        if (queryDto?.ValidToDateStart.HasValue == true)
        {
            var validToDateStart = queryDto.ValidToDateStart.Value;
            exp = exp.And(x => x.ValidToDate >= validToDateStart);
        }

        if (queryDto?.ValidToDateEnd.HasValue == true)
        {
            var validToDateEnd = queryDto.ValidToDateEnd.Value;
            exp = exp.And(x => x.ValidToDate <= validToDateEnd);
        }

        if (queryDto?.CrossPlantStatusValidFromStart.HasValue == true)
        {
            var crossPlantStatusValidFromStart = queryDto.CrossPlantStatusValidFromStart.Value;
            exp = exp.And(x => x.CrossPlantStatusValidFrom >= crossPlantStatusValidFromStart);
        }

        if (queryDto?.CrossPlantStatusValidFromEnd.HasValue == true)
        {
            var crossPlantStatusValidFromEnd = queryDto.CrossPlantStatusValidFromEnd.Value;
            exp = exp.And(x => x.CrossPlantStatusValidFrom <= crossPlantStatusValidFromEnd);
        }

        if (queryDto?.CrossDistributionStatusValidFromStart.HasValue == true)
        {
            var crossDistributionStatusValidFromStart = queryDto.CrossDistributionStatusValidFromStart.Value;
            exp = exp.And(x => x.CrossDistributionStatusValidFrom >= crossDistributionStatusValidFromStart);
        }

        if (queryDto?.CrossDistributionStatusValidFromEnd.HasValue == true)
        {
            var crossDistributionStatusValidFromEnd = queryDto.CrossDistributionStatusValidFromEnd.Value;
            exp = exp.And(x => x.CrossDistributionStatusValidFrom <= crossDistributionStatusValidFromEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            var createdAtStart = queryDto.CreatedAtStart.Value;
            exp = exp.And(x => x.CreatedAt >= createdAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            var createdAtEnd = queryDto.CreatedAtEnd.Value;
            exp = exp.And(x => x.CreatedAt <= createdAtEnd);
        }

        return exp.ToExpression();
    }

    /// <summary>
    /// 是否存在任一业务查询条件（KeyWords / 字段 / 日期范围）；无参时列表与导出返回空，避免全表扫描
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有条件为 true</returns>
    private static bool HasAnyListQueryFilter(TaktGeneralMaterialQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CompleteMaintenanceStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaintenanceStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClientDeletionFlag))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IndustrySector))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OldMaterialNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BaseUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OrderUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentVersion))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentPageFormat))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentChangeNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentPageNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentSheetCount))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionInspectionMemo))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionMemoPageFormat))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SizeDimensions))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BasicMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IndustryStandardDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LaboratoryDesignOffice))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchasingValueKey))
        {
            return true;
        }
        if (queryDto.GrossWeight.HasValue)
        {
            return true;
        }
        if (queryDto.NetWeight.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WeightUnit))
        {
            return true;
        }
        if (queryDto.Volume.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VolumeUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContainerRequirements))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StorageConditions))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TemperatureConditions))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LowLevelCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TransportationGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HazardousMaterialNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Division))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Competitor))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EuropeanArticleNumberObsolete))
        {
            return true;
        }
        if (queryDto.GrGiSlipQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProcurementRule))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceOfSupply))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SeasonCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LabelType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LabelForm))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeactivatedField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InternationalArticleNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EanCategory))
        {
            return true;
        }
        if (queryDto.Length.HasValue)
        {
            return true;
        }
        if (queryDto.Width.HasValue)
        {
            return true;
        }
        if (queryDto.Height.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DimensionUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductHierarchy))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StockTransferNetChangeCosting))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CadIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QmInProcurement))
        {
            return true;
        }
        if (queryDto.AllowedPackagingWeight.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AllowedPackagingWeightUnit))
        {
            return true;
        }
        if (queryDto.AllowedPackagingVolume.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AllowedPackagingVolumeUnit))
        {
            return true;
        }
        if (queryDto.ExcessWeightTolerance.HasValue)
        {
            return true;
        }
        if (queryDto.ExcessVolumeTolerance.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VariablePurchaseOrderUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RevisionLevelAssigned))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConfigurableMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BatchManagementRequired))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PackagingMaterialType))
        {
            return true;
        }
        if (queryDto.MaximumLevelByVolume.HasValue)
        {
            return true;
        }
        if (queryDto.StackingFactor.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PackagingMaterialGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AuthorizationGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SeasonYear))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PriceBandCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EmptiesBillOfMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExternalMaterialGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CrossPlantConfigurableMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CoProductIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FollowUpMaterialIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PricingReferenceMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CrossPlantMaterialStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CrossDistributionChainStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxClassification))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CatalogProfile))
        {
            return true;
        }
        if (queryDto.MinimumRemainingShelfLife.HasValue)
        {
            return true;
        }
        if (queryDto.TotalShelfLife.HasValue)
        {
            return true;
        }
        if (queryDto.StoragePercentage.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContentUnit))
        {
            return true;
        }
        if (queryDto.NetContents.HasValue)
        {
            return true;
        }
        if (queryDto.ComparisonPriceUnit.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LabelingMaterialGrouping))
        {
            return true;
        }
        if (queryDto.GrossContents.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QuantityConversionMethod))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InternalObjectNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EnvironmentallyRelevant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductAllocationProcedure))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VariantPricingProfile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DiscountInKind))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ManufacturerPartNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ManufacturerNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InventoryManagedMaterialNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ManufacturerPartProfile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UnitsOfMeasureUsage))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SeasonRollout))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DangerousGoodsProfile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HighlyViscous))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InBulkLiquid))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SerialNumberExplicitness))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClosedPackaging))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApprovedBatchRecordRequired))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EffectivityParameterOverride))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCompletionLevel))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ShelfLifePeriodIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ShelfLifeRoundingRule))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCompositionOnPackaging))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GeneralItemCategoryGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LogisticalVariants))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialLocked))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConfigurationManagementRelevant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssortmentListType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExpirationDateType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GtinVariant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GenericMaterialNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SamePackingReferenceMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.GlobalDataSyncRelevant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AcceptanceAtOrigin))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StandardHuType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Pilferable))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WarehouseStorageCondition))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WarehouseMaterialGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HandlingIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HazardousSubstancesRelevant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HandlingUnitType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VariableTareWeight))
        {
            return true;
        }
        if (queryDto.MaximumAllowedCapacity.HasValue)
        {
            return true;
        }
        if (queryDto.OvercapacityTolerance.HasValue)
        {
            return true;
        }
        if (queryDto.MaximumPackingLength.HasValue)
        {
            return true;
        }
        if (queryDto.MaximumPackingWidth.HasValue)
        {
            return true;
        }
        if (queryDto.MaximumPackingHeight.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaximumPackingDimensionUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CountryOfOrigin))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialFreightGroup))
        {
            return true;
        }
        if (queryDto.QuarantinePeriod.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QuarantinePeriodUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.QualityInspectionGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SerialNumberProfile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FormName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LogisticsUnitOfMeasure))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CatchWeightMaterial))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CatchWeightProfile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CatchWeightToleranceGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AdjustmentProfile))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IntellectualPropertyId))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VariantPriceAllowed))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Medium))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PhysicalCommodity))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AnimalOrigin))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TextileCompositionFunction))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SegmentationStructure))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SegmentationStrategy))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SegmentationStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SegmentationScope))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SegmentationRelevant))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AnpCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FashionAttribute1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FashionAttribute2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FashionAttribute3))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SeasonUsageIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SeasonActiveInInventory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CharacteristicConversionId))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PackagingCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DangerousGoodsPackagingStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialConditionManagement))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReturnCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReturnToLogisticsLevel))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.NatoItemIdentificationNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FffClass))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupersessionChainNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SeasonalProcurementCreationStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ColorCharacteristicInternalNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MainSizeCharacteristicInternalNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SecondSizeCharacteristicInternalNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Color))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MainSize))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SecondSize))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EvaluationCharacteristicValue))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CareCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BrandId))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberCode1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberPart1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberCode2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberPart2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberCode3))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberPart3))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberCode4))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberPart4))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberCode5))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiberPart5))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FashionGrade))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExtField))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Remark))
        {
            return true;
        }
        if (queryDto.ValidFromDateStart.HasValue || queryDto.ValidFromDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ValidToDateStart.HasValue || queryDto.ValidToDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CrossPlantStatusValidFromStart.HasValue || queryDto.CrossPlantStatusValidFromEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CrossDistributionStatusValidFromStart.HasValue || queryDto.CrossDistributionStatusValidFromEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
