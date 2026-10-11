// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktPackagingMaterialService.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：包装物料应用服务实现
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
/// 包装物料应用服务
/// </summary>
public class TaktPackagingMaterialService : TaktServiceBase, ITaktPackagingMaterialService
{
    private readonly ITaktCompanyRepository<TaktPackagingMaterial> _packagingMaterialRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="packagingMaterialRepository">包装物料仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPackagingMaterialService(
        ITaktCompanyRepository<TaktPackagingMaterial> packagingMaterialRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _packagingMaterialRepository = packagingMaterialRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取包装物料列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPackagingMaterialDto>> GetPackagingMaterialListAsync(TaktPackagingMaterialQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPackagingMaterialDto>.Create(
                new List<TaktPackagingMaterialDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _packagingMaterialRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPackagingMaterialDto>.Create(
            data.Adapt<List<TaktPackagingMaterialDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取包装物料
    /// </summary>
    /// <param name="id">包装物料ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPackagingMaterialDto?> GetPackagingMaterialByIdAsync(long id)
    {
        var entity = await _packagingMaterialRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPackagingMaterialDto>();
    }

    /// <summary>
    /// 获取包装物料选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPackagingMaterialOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _packagingMaterialRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.HsName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.MaterialCode,
            DictLabel = e.HsName ?? e.MaterialCode,
        }).ToList();
    }

    /// <summary>
    /// 创建包装物料
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPackagingMaterialDto> CreatePackagingMaterialAsync(TaktPackagingMaterialCreateDto dto)
    {
        var entity = dto.Adapt<TaktPackagingMaterial>();
        var isUnique_ix_takt_logistics_materials_packaging_material_unique = await _uniqueValidator.IsUniqueAsync(
            _packagingMaterialRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PackagingMaterialCode == entity.PackagingMaterialCode);
        if (!isUnique_ix_takt_logistics_materials_packaging_material_unique)
        {
            throw new TaktBusinessException("包装物料的PlantCode、PackagingMaterialCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _packagingMaterialRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _packagingMaterialRepository.CreateAsync(entity);
        return await GetPackagingMaterialByIdAsync(entity.Id) ?? entity.Adapt<TaktPackagingMaterialDto>();
    }

    /// <summary>
    /// 更新包装物料
    /// </summary>
    /// <param name="id">包装物料ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPackagingMaterialDto> UpdatePackagingMaterialAsync(long id, TaktPackagingMaterialUpdateDto dto)
    {
        var entity = await _packagingMaterialRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("包装物料不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_materials_packaging_material_unique = await _uniqueValidator.IsUniqueAsync(
            _packagingMaterialRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PackagingMaterialCode == entity.PackagingMaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_packaging_material_unique)
        {
            throw new TaktBusinessException("包装物料的PlantCode、PackagingMaterialCode已存在");
        }
        await _packagingMaterialRepository.UpdateAsync(entity);
        return await GetPackagingMaterialByIdAsync(id) ?? throw new TaktBusinessException("包装物料不存在");
    }

    /// <summary>
    /// 删除包装物料
    /// </summary>
    /// <param name="id">包装物料ID</param>
    /// <returns>任务</returns>
    public async Task DeletePackagingMaterialByIdAsync(long id)
    {
        var deleted = await _packagingMaterialRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("包装物料不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除包装物料
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePackagingMaterialBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePackagingMaterialByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新包装物料排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPackagingMaterialDto> UpdatePackagingMaterialSortAsync(TaktPackagingMaterialSortDto dto)
    {
        var entity = await _packagingMaterialRepository.GetByIdAsync(dto.PackagingMaterialId);
        if (entity == null)
        {
            throw new TaktBusinessException("包装物料不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _packagingMaterialRepository.UpdateAsync(entity);
        return await GetPackagingMaterialByIdAsync(dto.PackagingMaterialId) ?? throw new TaktBusinessException("包装物料不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPackagingMaterialTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPackagingMaterialTemplateDto>(
            sheetName ?? "包装物料导入模板",
            fileName ?? "包装物料导入模板.xlsx");
    }

    /// <summary>
    /// 导入包装物料
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPackagingMaterialAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPackagingMaterialImportDto>(fileStream, sheetName ?? "包装物料导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _packagingMaterialRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktPackagingMaterial>();
                var importKey = $"{entity.PlantCode}|{entity.PackagingMaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PackagingMaterialCode）");
                }
                var isUnique_ix_takt_logistics_materials_packaging_material_unique = await _uniqueValidator.IsUniqueAsync(
                    _packagingMaterialRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PackagingMaterialCode == entity.PackagingMaterialCode);
                if (!isUnique_ix_takt_logistics_materials_packaging_material_unique)
                {
                    throw new TaktBusinessException("包装物料的PlantCode、PackagingMaterialCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _packagingMaterialRepository.CreateAsync(entity);
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
    /// 导出包装物料
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPackagingMaterialAsync(TaktPackagingMaterialQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPackagingMaterialQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPackagingMaterialExportDto>(),
                sheetName ?? "包装物料数据",
                fileName ?? "包装物料导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _packagingMaterialRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPackagingMaterialExportDto>(),
                sheetName ?? "包装物料数据",
                fileName ?? "包装物料导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPackagingMaterialExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "包装物料数据",
            fileName ?? "包装物料导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建包装物料查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPackagingMaterial, bool>> QueryExpression(TaktPackagingMaterialQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPackagingMaterial>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PackagingMaterialCode != null && x.PackagingMaterialCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.HsCode != null && x.HsCode.Contains(keywords))
                || (x.HsName != null && x.HsName.Contains(keywords))
                || (x.AdditionalCode != null && x.AdditionalCode.Contains(keywords))
                || (x.OriginCountryRegionCode != null && x.OriginCountryRegionCode.Contains(keywords))
                || (x.OriginCountryRegionName != null && x.OriginCountryRegionName.Contains(keywords))
                || (x.DestinationCountryRegionCode != null && x.DestinationCountryRegionCode.Contains(keywords))
                || (x.DestinationCountryRegionName != null && x.DestinationCountryRegionName.Contains(keywords))
                || (x.RegulatoryConditionCode != null && x.RegulatoryConditionCode.Contains(keywords))
                || (x.TariffRateType != null && x.TariffRateType.Contains(keywords))
                || (x.WeightUnit != null && x.WeightUnit.Contains(keywords))
                || (x.VolumeUnit != null && x.VolumeUnit.Contains(keywords))
                || (x.SizeDimension != null && x.SizeDimension.Contains(keywords))
                || (x.PackagingType != null && x.PackagingType.Contains(keywords))
                || (x.PackingUnit != null && x.PackingUnit.Contains(keywords))
                || (x.PackagingSpec != null && x.PackagingSpec.Contains(keywords))
                || (x.PackagingDescription != null && x.PackagingDescription.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CultureCode))
        {
            var cultureCode = queryDto.CultureCode;
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(cultureCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PackagingMaterialCode))
        {
            var packagingMaterialCode = queryDto.PackagingMaterialCode;
            exp = exp.And(x => x.PackagingMaterialCode != null && x.PackagingMaterialCode.Contains(packagingMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialDescription))
        {
            var materialDescription = queryDto.MaterialDescription;
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(materialDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HsCode))
        {
            var hsCode = queryDto.HsCode;
            exp = exp.And(x => x.HsCode != null && x.HsCode.Contains(hsCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HsName))
        {
            var hsName = queryDto.HsName;
            exp = exp.And(x => x.HsName != null && x.HsName.Contains(hsName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AdditionalCode))
        {
            var additionalCode = queryDto.AdditionalCode;
            exp = exp.And(x => x.AdditionalCode != null && x.AdditionalCode.Contains(additionalCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OriginCountryRegionCode))
        {
            var originCountryRegionCode = queryDto.OriginCountryRegionCode;
            exp = exp.And(x => x.OriginCountryRegionCode != null && x.OriginCountryRegionCode.Contains(originCountryRegionCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OriginCountryRegionName))
        {
            var originCountryRegionName = queryDto.OriginCountryRegionName;
            exp = exp.And(x => x.OriginCountryRegionName != null && x.OriginCountryRegionName.Contains(originCountryRegionName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DestinationCountryRegionCode))
        {
            var destinationCountryRegionCode = queryDto.DestinationCountryRegionCode;
            exp = exp.And(x => x.DestinationCountryRegionCode != null && x.DestinationCountryRegionCode.Contains(destinationCountryRegionCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DestinationCountryRegionName))
        {
            var destinationCountryRegionName = queryDto.DestinationCountryRegionName;
            exp = exp.And(x => x.DestinationCountryRegionName != null && x.DestinationCountryRegionName.Contains(destinationCountryRegionName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RegulatoryConditionCode))
        {
            var regulatoryConditionCode = queryDto.RegulatoryConditionCode;
            exp = exp.And(x => x.RegulatoryConditionCode != null && x.RegulatoryConditionCode.Contains(regulatoryConditionCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TariffRateType))
        {
            var tariffRateType = queryDto.TariffRateType;
            exp = exp.And(x => x.TariffRateType != null && x.TariffRateType.Contains(tariffRateType));
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

        if (queryDto?.BusinessVolume.HasValue == true)
        {
            var businessVolume = queryDto.BusinessVolume.Value;
            exp = exp.And(x => x.BusinessVolume == businessVolume);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.VolumeUnit))
        {
            var volumeUnit = queryDto.VolumeUnit;
            exp = exp.And(x => x.VolumeUnit != null && x.VolumeUnit.Contains(volumeUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SizeDimension))
        {
            var sizeDimension = queryDto.SizeDimension;
            exp = exp.And(x => x.SizeDimension != null && x.SizeDimension.Contains(sizeDimension));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PackagingType))
        {
            var packagingType = queryDto.PackagingType;
            exp = exp.And(x => x.PackagingType != null && x.PackagingType.Contains(packagingType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PackingUnit))
        {
            var packingUnit = queryDto.PackingUnit;
            exp = exp.And(x => x.PackingUnit != null && x.PackingUnit.Contains(packingUnit));
        }

        if (queryDto?.QuantityPerPacking.HasValue == true)
        {
            var quantityPerPacking = queryDto.QuantityPerPacking.Value;
            exp = exp.And(x => x.QuantityPerPacking == quantityPerPacking);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PackagingSpec))
        {
            var packagingSpec = queryDto.PackagingSpec;
            exp = exp.And(x => x.PackagingSpec != null && x.PackagingSpec.Contains(packagingSpec));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PackagingDescription))
        {
            var packagingDescription = queryDto.PackagingDescription;
            exp = exp.And(x => x.PackagingDescription != null && x.PackagingDescription.Contains(packagingDescription));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
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
    private static bool HasAnyListQueryFilter(TaktPackagingMaterialQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CultureCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PackagingMaterialCode))
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
        if (!string.IsNullOrWhiteSpace(queryDto.HsCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HsName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AdditionalCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OriginCountryRegionCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OriginCountryRegionName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DestinationCountryRegionCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DestinationCountryRegionName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RegulatoryConditionCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TariffRateType))
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
        if (queryDto.BusinessVolume.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VolumeUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SizeDimension))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PackagingType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PackingUnit))
        {
            return true;
        }
        if (queryDto.QuantityPerPacking.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PackagingSpec))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PackagingDescription))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
