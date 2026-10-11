// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktStorageLocationService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：库位主数据应用服务实现
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
/// 库位主数据应用服务
/// </summary>
public class TaktStorageLocationService : TaktServiceBase, ITaktStorageLocationService
{
    private readonly ITaktCompanyRepository<TaktStorageLocation> _storageLocationRepository;
    private readonly ITaktCompanyRepository<TaktWarehouse> _warehouseRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="storageLocationRepository">库位主数据仓储</param>
    /// <param name="warehouseRepository">仓库主数据仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktStorageLocationService(
        ITaktCompanyRepository<TaktStorageLocation> storageLocationRepository,
        ITaktCompanyRepository<TaktWarehouse> warehouseRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _storageLocationRepository = storageLocationRepository;
        _warehouseRepository = warehouseRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取库位主数据列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktStorageLocationDto>> GetStorageLocationListAsync(TaktStorageLocationQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktStorageLocationDto>.Create(
                new List<TaktStorageLocationDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _storageLocationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktStorageLocationDto>.Create(
            data.Adapt<List<TaktStorageLocationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取库位主数据
    /// </summary>
    /// <param name="id">库位主数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktStorageLocationDto?> GetStorageLocationByIdAsync(long id)
    {
        var entity = await _storageLocationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktStorageLocationDto>();
    }

    /// <summary>
    /// 获取库位主数据选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetStorageLocationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _storageLocationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.LocationStatus == 1,
            x => x.LocationName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.LocationName,
            DictLabel = e.LocationName,
        }).ToList();
    }

    /// <summary>
    /// 创建库位主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStorageLocationDto> CreateStorageLocationAsync(TaktStorageLocationCreateDto dto)
    {
        var entity = dto.Adapt<TaktStorageLocation>();
        entity.IsBuiltIn = 0;
        await StampStorageLocationWarehouseAsync(entity, dto);
        var isUnique_ix_takt_logistics_materials_storage_location_unique = await _uniqueValidator.IsUniqueAsync(
            _storageLocationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WarehouseCode == entity.WarehouseCode
                && x.LocationCode == entity.LocationCode);
        if (!isUnique_ix_takt_logistics_materials_storage_location_unique)
        {
            throw new TaktBusinessException("库位主数据的PlantCode、WarehouseCode、LocationCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _storageLocationRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.WarehouseId == entity.WarehouseId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.WarehouseId, maxSort);
        }
        entity = await _storageLocationRepository.CreateAsync(entity);
        return await GetStorageLocationByIdAsync(entity.Id) ?? entity.Adapt<TaktStorageLocationDto>();
    }

    /// <summary>
    /// 更新库位主数据
    /// </summary>
    /// <param name="id">库位主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStorageLocationDto> UpdateStorageLocationAsync(long id, TaktStorageLocationUpdateDto dto)
    {
        var entity = await _storageLocationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("库位主数据不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        await StampStorageLocationWarehouseAsync(entity, dto);
        var isUnique_ix_takt_logistics_materials_storage_location_unique = await _uniqueValidator.IsUniqueAsync(
            _storageLocationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WarehouseCode == entity.WarehouseCode
                && x.LocationCode == entity.LocationCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_storage_location_unique)
        {
            throw new TaktBusinessException("库位主数据的PlantCode、WarehouseCode、LocationCode已存在");
        }
        await _storageLocationRepository.UpdateAsync(entity);
        return await GetStorageLocationByIdAsync(id) ?? throw new TaktBusinessException("库位主数据不存在");
    }

    /// <summary>
    /// 删除库位主数据
    /// </summary>
    /// <param name="id">库位主数据ID</param>
    /// <returns>任务</returns>
    public async Task DeleteStorageLocationByIdAsync(long id)
    {
        var entity = await _storageLocationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("库位主数据不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置库位主数据不允许删除");
        }
        var deleted = await _storageLocationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("库位主数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除库位主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteStorageLocationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _storageLocationRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置库位主数据不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteStorageLocationByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新库位主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStorageLocationDto> UpdateStorageLocationStatusAsync(TaktStorageLocationStatusDto dto)
    {
        var entity = await _storageLocationRepository.GetByIdAsync(dto.StorageLocationId);
        if (entity == null)
        {
            throw new TaktBusinessException("库位主数据不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.LocationStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置库位主数据");
        }
        entity.LocationStatus = dto.LocationStatus;
        await _storageLocationRepository.UpdateAsync(entity);
        return await GetStorageLocationByIdAsync(dto.StorageLocationId) ?? throw new TaktBusinessException("库位主数据不存在");
    }

    /// <summary>
    /// 更新库位主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktStorageLocationDto> UpdateStorageLocationSortAsync(TaktStorageLocationSortDto dto)
    {
        var entity = await _storageLocationRepository.GetByIdAsync(dto.StorageLocationId);
        if (entity == null)
        {
            throw new TaktBusinessException("库位主数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _storageLocationRepository.UpdateAsync(entity);
        return await GetStorageLocationByIdAsync(dto.StorageLocationId) ?? throw new TaktBusinessException("库位主数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetStorageLocationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktStorageLocationTemplateDto>(
            sheetName ?? "库位主数据导入模板",
            fileName ?? "库位主数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入库位主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportStorageLocationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktStorageLocationImportDto>(fileStream, sheetName ?? "库位主数据导入模板");
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
                var entity = rows[i].Adapt<TaktStorageLocation>();
                entity.IsBuiltIn = 0;
                var importDto = rows[i].Adapt<TaktStorageLocationCreateDto>();
                await StampStorageLocationWarehouseAsync(entity, importDto);
                var importKey = $"{entity.PlantCode}|{entity.WarehouseCode}|{entity.LocationCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、WarehouseCode、LocationCode）");
                }
                var isUnique_ix_takt_logistics_materials_storage_location_unique = await _uniqueValidator.IsUniqueAsync(
                    _storageLocationRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.WarehouseCode == entity.WarehouseCode
                        && x.LocationCode == entity.LocationCode);
                if (!isUnique_ix_takt_logistics_materials_storage_location_unique)
                {
                    throw new TaktBusinessException("库位主数据的PlantCode、WarehouseCode、LocationCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _storageLocationRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.WarehouseId == entity.WarehouseId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.WarehouseId, maxSort);
                }
                await _storageLocationRepository.CreateAsync(entity);
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
    /// 导出库位主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportStorageLocationAsync(TaktStorageLocationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktStorageLocationQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktStorageLocationExportDto>(),
                sheetName ?? "库位主数据数据",
                fileName ?? "库位主数据导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _storageLocationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktStorageLocationExportDto>(),
                sheetName ?? "库位主数据数据",
                fileName ?? "库位主数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktStorageLocationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "库位主数据数据",
            fileName ?? "库位主数据导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步库位主数据主表外键（ManyToOne → 仓库主数据）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampStorageLocationWarehouseAsync(TaktStorageLocation entity, TaktStorageLocationCreateDto dto)
    {
        if (dto.WarehouseId <= 0)
        {
            return;
        }
        var master = await _warehouseRepository.GetByIdAsync(dto.WarehouseId);
        if (master == null)
        {
            throw new TaktBusinessException("仓库主数据不存在");
        }
        entity.WarehouseId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
        if (string.IsNullOrEmpty(entity.WarehouseCode))
        {
            entity.WarehouseCode = master.WarehouseCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建库位主数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktStorageLocation, bool>> QueryExpression(TaktStorageLocationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktStorageLocation>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.WarehouseCode != null && x.WarehouseCode.Contains(keywords))
                || (x.LocationCode != null && x.LocationCode.Contains(keywords))
                || (x.LocationName != null && x.LocationName.Contains(keywords))
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

        if (queryDto?.WarehouseId.HasValue == true)
        {
            var warehouseId = queryDto.WarehouseId.Value;
            exp = exp.And(x => x.WarehouseId == warehouseId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WarehouseCode))
        {
            var warehouseCode = queryDto.WarehouseCode;
            exp = exp.And(x => x.WarehouseCode != null && x.WarehouseCode.Contains(warehouseCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LocationCode))
        {
            var locationCode = queryDto.LocationCode;
            exp = exp.And(x => x.LocationCode != null && x.LocationCode.Contains(locationCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.LocationName))
        {
            var locationName = queryDto.LocationName;
            exp = exp.And(x => x.LocationName != null && x.LocationName.Contains(locationName));
        }

        if (queryDto?.LocationType.HasValue == true)
        {
            var locationType = queryDto.LocationType.Value;
            exp = exp.And(x => x.LocationType == locationType);
        }

        if (queryDto?.IsBuiltIn.HasValue == true)
        {
            var isBuiltIn = queryDto.IsBuiltIn.Value;
            exp = exp.And(x => x.IsBuiltIn == isBuiltIn);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.LocationStatus.HasValue == true)
        {
            var locationStatus = queryDto.LocationStatus.Value;
            exp = exp.And(x => x.LocationStatus == locationStatus);
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
    private static bool HasAnyListQueryFilter(TaktStorageLocationQueryDto? queryDto)
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
        if (queryDto.WarehouseId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WarehouseCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LocationCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.LocationName))
        {
            return true;
        }
        if (queryDto.LocationType.HasValue)
        {
            return true;
        }
        if (queryDto.IsBuiltIn.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.LocationStatus.HasValue)
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
