// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktMaterialPlantService.cs
// 创建时间：2026-08-05
// 创建人：Takt365(Cursor AI)
// 功能描述：工厂物料应用服务实现
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
/// 工厂物料应用服务
/// </summary>
public class TaktMaterialPlantService : TaktServiceBase, ITaktMaterialPlantService
{
    private readonly ITaktCompanyRepository<TaktMaterialPlant> _materialPlantRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="materialPlantRepository">工厂物料仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMaterialPlantService(
        ITaktCompanyRepository<TaktMaterialPlant> materialPlantRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _materialPlantRepository = materialPlantRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工厂物料列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMaterialPlantDto>> GetMaterialPlantListAsync(TaktMaterialPlantQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _materialPlantRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMaterialPlantDto>.Create(
            data.Adapt<List<TaktMaterialPlantDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工厂物料
    /// </summary>
    /// <param name="id">工厂物料ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialPlantDto?> GetMaterialPlantByIdAsync(long id)
    {
        var entity = await _materialPlantRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMaterialPlantDto>();
    }

    /// <summary>
    /// 获取工厂物料选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMaterialPlantOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _materialPlantRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MaterialStatus == 1,
            x => x.MaterialCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MaterialCode,
            DictLabel = e.MaterialCode,
        }).ToList();
    }

    /// <summary>
    /// 创建工厂物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialPlantDto> CreateMaterialPlantAsync(TaktMaterialPlantCreateDto dto)
    {
        var entity = dto.Adapt<TaktMaterialPlant>();
        var isUnique_ix_takt_logistics_materials_material_unique = await _uniqueValidator.IsUniqueAsync(
            _materialPlantRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_materials_material_unique)
        {
            throw new TaktBusinessException("工厂物料的PlantCode、MaterialCode已存在");
        }
        entity = await _materialPlantRepository.CreateAsync(entity);
        return await GetMaterialPlantByIdAsync(entity.Id) ?? entity.Adapt<TaktMaterialPlantDto>();
    }

    /// <summary>
    /// 更新工厂物料
    /// </summary>
    /// <param name="id">工厂物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialPlantDto> UpdateMaterialPlantAsync(long id, TaktMaterialPlantUpdateDto dto)
    {
        var entity = await _materialPlantRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂物料不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_material_unique = await _uniqueValidator.IsUniqueAsync(
            _materialPlantRepository,
            x => x.PlantCode == entity.PlantCode
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_material_unique)
        {
            throw new TaktBusinessException("工厂物料的PlantCode、MaterialCode已存在");
        }
        await _materialPlantRepository.UpdateAsync(entity);
        return await GetMaterialPlantByIdAsync(id) ?? throw new TaktBusinessException("工厂物料不存在");
    }

    /// <summary>
    /// 删除工厂物料
    /// </summary>
    /// <param name="id">工厂物料ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialPlantByIdAsync(long id)
    {
        var deleted = await _materialPlantRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工厂物料不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工厂物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMaterialPlantBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMaterialPlantByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工厂物料状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMaterialPlantDto> UpdateMaterialPlantStatusAsync(TaktMaterialPlantStatusDto dto)
    {
        var entity = await _materialPlantRepository.GetByIdAsync(dto.MaterialPlantId);
        if (entity == null)
        {
            throw new TaktBusinessException("工厂物料不存在");
        }
        entity.MaterialStatus = dto.MaterialStatus;
        await _materialPlantRepository.UpdateAsync(entity);
        return await GetMaterialPlantByIdAsync(dto.MaterialPlantId) ?? throw new TaktBusinessException("工厂物料不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMaterialPlantTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMaterialPlantTemplateDto>(
            sheetName ?? "工厂物料导入模板",
            fileName ?? "工厂物料导入模板.xlsx");
    }

    /// <summary>
    /// 导入工厂物料
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMaterialPlantAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMaterialPlantImportDto>(fileStream, sheetName ?? "工厂物料导入模板");
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
                var entity = rows[i].Adapt<TaktMaterialPlant>();
                var importKey = $"{entity.PlantCode}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_materials_material_unique = await _uniqueValidator.IsUniqueAsync(
                    _materialPlantRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_materials_material_unique)
                {
                    throw new TaktBusinessException("工厂物料的PlantCode、MaterialCode已存在");
                }
                await _materialPlantRepository.CreateAsync(entity);
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
    /// 导出工厂物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMaterialPlantAsync(TaktMaterialPlantQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMaterialPlantQueryDto());
        var list = await _materialPlantRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMaterialPlantExportDto>(),
                sheetName ?? "工厂物料数据",
                fileName ?? "工厂物料导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMaterialPlantExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工厂物料数据",
            fileName ?? "工厂物料导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工厂物料查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMaterialPlant, bool>> QueryExpression(TaktMaterialPlantQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMaterialPlant>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.IndustrySector != null && x.IndustrySector.Contains(keywords))
                || (x.MaterialHierarchy != null && x.MaterialHierarchy.Contains(keywords))
                || (x.MaterialGroup != null && x.MaterialGroup.Contains(keywords))
                || (x.MaterialType != null && x.MaterialType.Contains(keywords))
                || (x.BaseUnit != null && x.BaseUnit.Contains(keywords))
                || (x.PurchaseGroup != null && x.PurchaseGroup.Contains(keywords))
                || (x.PurchaseType != null && x.PurchaseType.Contains(keywords))
                || SqlFunc.ToString(x.SpecialProcurement).Contains(keywords)
                || SqlFunc.ToString(x.IsBulk).Contains(keywords)
                || SqlFunc.ToString(x.MinOrderQuantity).Contains(keywords)
                || SqlFunc.ToString(x.RoundingValue).Contains(keywords)
                || SqlFunc.ToString(x.PlannedDeliveryTimeDays).Contains(keywords)
                || SqlFunc.ToString(x.InHouseProductionDays).Contains(keywords)
                || (x.Manufacturer != null && x.Manufacturer.Contains(keywords))
                || (x.ManufacturerMaterialCode != null && x.ManufacturerMaterialCode.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.PriceControl != null && x.PriceControl.Contains(keywords))
                || SqlFunc.ToString(x.PriceUnit).Contains(keywords)
                || (x.Valuation != null && x.Valuation.Contains(keywords))
                || SqlFunc.ToString(x.MovingPrice).Contains(keywords)
                || (x.DifferenceCode != null && x.DifferenceCode.Contains(keywords))
                || (x.ProfitCenter != null && x.ProfitCenter.Contains(keywords))
                || SqlFunc.ToString(x.CurrentStock).Contains(keywords)
                || (x.ProductionLocation != null && x.ProductionLocation.Contains(keywords))
                || (x.PurchasingLocation != null && x.PurchasingLocation.Contains(keywords))
                || (x.StorageLocation != null && x.StorageLocation.Contains(keywords))
                || SqlFunc.ToString(x.RequiresInspection).Contains(keywords)
                || SqlFunc.ToString(x.IsBatch).Contains(keywords)
                || (x.DiscontinuedStatus != null && x.DiscontinuedStatus.Contains(keywords))
                || SqlFunc.ToString(x.MaterialStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialCode))
        {
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(queryDto.MaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialDescription))
        {
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(queryDto.MaterialDescription));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialSpecification))
        {
            exp = exp.And(x => x.MaterialSpecification != null && x.MaterialSpecification.Contains(queryDto.MaterialSpecification));
        }

        if (!string.IsNullOrEmpty(queryDto?.IndustrySector))
        {
            exp = exp.And(x => x.IndustrySector != null && x.IndustrySector.Contains(queryDto.IndustrySector));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialHierarchy))
        {
            exp = exp.And(x => x.MaterialHierarchy != null && x.MaterialHierarchy.Contains(queryDto.MaterialHierarchy));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialGroup))
        {
            exp = exp.And(x => x.MaterialGroup != null && x.MaterialGroup.Contains(queryDto.MaterialGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.MaterialType))
        {
            exp = exp.And(x => x.MaterialType != null && x.MaterialType.Contains(queryDto.MaterialType));
        }

        if (!string.IsNullOrEmpty(queryDto?.BaseUnit))
        {
            exp = exp.And(x => x.BaseUnit != null && x.BaseUnit.Contains(queryDto.BaseUnit));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseGroup))
        {
            exp = exp.And(x => x.PurchaseGroup != null && x.PurchaseGroup.Contains(queryDto.PurchaseGroup));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchaseType))
        {
            exp = exp.And(x => x.PurchaseType != null && x.PurchaseType.Contains(queryDto.PurchaseType));
        }

        if (queryDto?.SpecialProcurement.HasValue == true)
        {
            exp = exp.And(x => x.SpecialProcurement == queryDto.SpecialProcurement);
        }

        if (queryDto?.IsBulk.HasValue == true)
        {
            exp = exp.And(x => x.IsBulk == queryDto.IsBulk);
        }

        if (queryDto?.MinOrderQuantity.HasValue == true)
        {
            exp = exp.And(x => x.MinOrderQuantity == queryDto.MinOrderQuantity);
        }

        if (queryDto?.RoundingValue.HasValue == true)
        {
            exp = exp.And(x => x.RoundingValue == queryDto.RoundingValue);
        }

        if (queryDto?.PlannedDeliveryTimeDays.HasValue == true)
        {
            exp = exp.And(x => x.PlannedDeliveryTimeDays == queryDto.PlannedDeliveryTimeDays);
        }

        if (queryDto?.InHouseProductionDays.HasValue == true)
        {
            exp = exp.And(x => x.InHouseProductionDays == queryDto.InHouseProductionDays);
        }

        if (!string.IsNullOrEmpty(queryDto?.Manufacturer))
        {
            exp = exp.And(x => x.Manufacturer != null && x.Manufacturer.Contains(queryDto.Manufacturer));
        }

        if (!string.IsNullOrEmpty(queryDto?.ManufacturerMaterialCode))
        {
            exp = exp.And(x => x.ManufacturerMaterialCode != null && x.ManufacturerMaterialCode.Contains(queryDto.ManufacturerMaterialCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.PriceControl))
        {
            exp = exp.And(x => x.PriceControl != null && x.PriceControl.Contains(queryDto.PriceControl));
        }

        if (queryDto?.PriceUnit.HasValue == true)
        {
            exp = exp.And(x => x.PriceUnit == queryDto.PriceUnit);
        }

        if (!string.IsNullOrEmpty(queryDto?.Valuation))
        {
            exp = exp.And(x => x.Valuation != null && x.Valuation.Contains(queryDto.Valuation));
        }

        if (queryDto?.MovingPrice.HasValue == true)
        {
            exp = exp.And(x => x.MovingPrice == queryDto.MovingPrice);
        }

        if (!string.IsNullOrEmpty(queryDto?.DifferenceCode))
        {
            exp = exp.And(x => x.DifferenceCode != null && x.DifferenceCode.Contains(queryDto.DifferenceCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProfitCenter))
        {
            exp = exp.And(x => x.ProfitCenter != null && x.ProfitCenter.Contains(queryDto.ProfitCenter));
        }

        if (queryDto?.CurrentStock.HasValue == true)
        {
            exp = exp.And(x => x.CurrentStock == queryDto.CurrentStock);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProductionLocation))
        {
            exp = exp.And(x => x.ProductionLocation != null && x.ProductionLocation.Contains(queryDto.ProductionLocation));
        }

        if (!string.IsNullOrEmpty(queryDto?.PurchasingLocation))
        {
            exp = exp.And(x => x.PurchasingLocation != null && x.PurchasingLocation.Contains(queryDto.PurchasingLocation));
        }

        if (!string.IsNullOrEmpty(queryDto?.StorageLocation))
        {
            exp = exp.And(x => x.StorageLocation != null && x.StorageLocation.Contains(queryDto.StorageLocation));
        }

        if (queryDto?.RequiresInspection.HasValue == true)
        {
            exp = exp.And(x => x.RequiresInspection == queryDto.RequiresInspection);
        }

        if (queryDto?.IsBatch.HasValue == true)
        {
            exp = exp.And(x => x.IsBatch == queryDto.IsBatch);
        }

        if (!string.IsNullOrEmpty(queryDto?.DiscontinuedStatus))
        {
            exp = exp.And(x => x.DiscontinuedStatus != null && x.DiscontinuedStatus.Contains(queryDto.DiscontinuedStatus));
        }

        if (queryDto?.MaterialStatus.HasValue == true)
        {
            exp = exp.And(x => x.MaterialStatus == queryDto.MaterialStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        var rangeStart = queryDto?.CreatedAtStart;
        var rangeEnd = queryDto?.CreatedAtEnd;
        if (!rangeStart.HasValue && !rangeEnd.HasValue && !HasFiltersBesidesDefaultListScope(queryDto))
        {
            var monthBounds = GetCurrentMonthRangeBounds();
            rangeStart = monthBounds.Start;
            rangeEnd = monthBounds.End;
        }

        if (rangeStart.HasValue)
        {
            exp = exp.And(x => x.CreatedAt >= rangeStart.Value);
        }

        if (rangeEnd.HasValue)
        {
            exp = exp.And(x => x.CreatedAt <= rangeEnd.Value);
        }

        return exp.ToExpression();
    }

    /// <summary>
    /// 当前自然月起止（含月末最后一刻），用于列表无参默认过滤、避免全表扫描
    /// </summary>
    /// <returns>起、止</returns>
    private static (DateTime Start, DateTime End) GetCurrentMonthRangeBounds()
    {
        var today = DateTime.Today;
        var start = new DateTime(today.Year, today.Month, 1);
        var end = start.AddMonths(1).AddTicks(-1);
        return (start, end);
    }
    /// <summary>
    /// 是否存在除默认当前月/当前期间外的查询条件（有参则不强制默认范围）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>有其它条件为 true</returns>
    private static bool HasFiltersBesidesDefaultListScope(TaktMaterialPlantQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialSpecification))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.IndustrySector))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialHierarchy))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BaseUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseType))
        {
            return true;
        }
        if (queryDto.SpecialProcurement.HasValue)
        {
            return true;
        }
        if (queryDto.IsBulk.HasValue)
        {
            return true;
        }
        if (queryDto.MinOrderQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.RoundingValue.HasValue)
        {
            return true;
        }
        if (queryDto.PlannedDeliveryTimeDays.HasValue)
        {
            return true;
        }
        if (queryDto.InHouseProductionDays.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Manufacturer))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ManufacturerMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PriceControl))
        {
            return true;
        }
        if (queryDto.PriceUnit.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Valuation))
        {
            return true;
        }
        if (queryDto.MovingPrice.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DifferenceCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProfitCenter))
        {
            return true;
        }
        if (queryDto.CurrentStock.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductionLocation))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchasingLocation))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StorageLocation))
        {
            return true;
        }
        if (queryDto.RequiresInspection.HasValue)
        {
            return true;
        }
        if (queryDto.IsBatch.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DiscontinuedStatus))
        {
            return true;
        }
        if (queryDto.MaterialStatus.HasValue)
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
        return false;
    }
}
