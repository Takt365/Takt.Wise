// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Materials
// 文件名称：TaktWarehouseService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：仓库主数据应用服务实现
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
/// 仓库主数据应用服务
/// </summary>
public class TaktWarehouseService : TaktServiceBase, ITaktWarehouseService
{
    private readonly ITaktCompanyRepository<TaktWarehouse> _warehouseRepository;
    private readonly ITaktCompanyRepository<TaktStorageLocation> _storageLocationRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="warehouseRepository">仓库主数据仓储</param>
    /// <param name="storageLocationRepository">StorageLocation仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktWarehouseService(
        ITaktCompanyRepository<TaktWarehouse> warehouseRepository,
        ITaktCompanyRepository<TaktStorageLocation> storageLocationRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _warehouseRepository = warehouseRepository;
        _storageLocationRepository = storageLocationRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取仓库主数据列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktWarehouseDto>> GetWarehouseListAsync(TaktWarehouseQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktWarehouseDto>.Create(
                new List<TaktWarehouseDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _warehouseRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktWarehouseDto>.Create(
            data.Adapt<List<TaktWarehouseDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取仓库主数据
    /// </summary>
    /// <param name="id">仓库主数据ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktWarehouseDto?> GetWarehouseByIdAsync(long id)
    {
        var entity = await _warehouseRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktWarehouseDto>();
        await FillWarehouseDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取仓库主数据选项列表（DictValue 为 WarehouseCode，DictLabel 为仓库名称）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetWarehouseOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _warehouseRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.WarehouseStatus == 1,
            x => x.SortOrder,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.WarehouseCode,
            DictLabel = string.IsNullOrWhiteSpace(e.WarehouseName) ? e.WarehouseCode : e.WarehouseName,
            ExtLabel = e.WarehouseCode,
            SortOrder = e.SortOrder,
        }).ToList();
    }

    /// <summary>
    /// 创建仓库主数据
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWarehouseDto> CreateWarehouseAsync(TaktWarehouseCreateDto dto)
    {
        var entity = dto.Adapt<TaktWarehouse>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_takt_logistics_materials_warehouse_unique = await _uniqueValidator.IsUniqueAsync(
            _warehouseRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WarehouseCode == entity.WarehouseCode);
        if (!isUnique_ix_takt_logistics_materials_warehouse_unique)
        {
            throw new TaktBusinessException("仓库主数据的PlantCode、WarehouseCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _warehouseRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _warehouseRepository.CreateAsync(entity);
                await SaveWarehouseChildrenAsync(entity, dto);
        return await GetWarehouseByIdAsync(entity.Id) ?? entity.Adapt<TaktWarehouseDto>();
    }

    /// <summary>
    /// 更新品仓库主数据
    /// </summary>
    /// <param name="id">仓库主数据ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWarehouseDto> UpdateWarehouseAsync(long id, TaktWarehouseUpdateDto dto)
    {
        var entity = await _warehouseRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("仓库主数据不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_takt_logistics_materials_warehouse_unique = await _uniqueValidator.IsUniqueAsync(
            _warehouseRepository,
            x => x.PlantCode == entity.PlantCode
                && x.WarehouseCode == entity.WarehouseCode,
            id);
        if (!isUnique_ix_takt_logistics_materials_warehouse_unique)
        {
            throw new TaktBusinessException("仓库主数据的PlantCode、WarehouseCode已存在");
        }
        await _warehouseRepository.UpdateAsync(entity);
                await SaveWarehouseChildrenAsync(entity, dto);
        return await GetWarehouseByIdAsync(id) ?? throw new TaktBusinessException("仓库主数据不存在");
    }

    /// <summary>
    /// 删除仓库主数据
    /// </summary>
    /// <param name="id">仓库主数据ID</param>
    /// <returns>任务</returns>
    public async Task DeleteWarehouseByIdAsync(long id)
    {
        var entity = await _warehouseRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("仓库主数据不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置仓库主数据不允许删除");
        }
        await _storageLocationRepository.DeleteAsync(x => x.WarehouseId == entity.Id);
        var deleted = await _warehouseRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("仓库主数据不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除仓库主数据
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteWarehouseBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _warehouseRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置仓库主数据不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteWarehouseByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新品仓库主数据状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWarehouseDto> UpdateWarehouseStatusAsync(TaktWarehouseStatusDto dto)
    {
        var entity = await _warehouseRepository.GetByIdAsync(dto.WarehouseId);
        if (entity == null)
        {
            throw new TaktBusinessException("仓库主数据不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.WarehouseStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置仓库主数据");
        }
        entity.WarehouseStatus = dto.WarehouseStatus;
        await _warehouseRepository.UpdateAsync(entity);
        return await GetWarehouseByIdAsync(dto.WarehouseId) ?? throw new TaktBusinessException("仓库主数据不存在");
    }

    /// <summary>
    /// 更新品仓库主数据排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktWarehouseDto> UpdateWarehouseSortAsync(TaktWarehouseSortDto dto)
    {
        var entity = await _warehouseRepository.GetByIdAsync(dto.WarehouseId);
        if (entity == null)
        {
            throw new TaktBusinessException("仓库主数据不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _warehouseRepository.UpdateAsync(entity);
        return await GetWarehouseByIdAsync(dto.WarehouseId) ?? throw new TaktBusinessException("仓库主数据不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetWarehouseTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktWarehouseTemplateDto>(
            sheetName ?? "仓库主数据导入模板",
            fileName ?? "仓库主数据导入模板.xlsx");
    }

    /// <summary>
    /// 导入仓库主数据
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportWarehouseAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktWarehouseImportDto>(fileStream, sheetName ?? "仓库主数据导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _warehouseRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktWarehouse>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.PlantCode}|{entity.WarehouseCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、WarehouseCode）");
                }
                var isUnique_ix_takt_logistics_materials_warehouse_unique = await _uniqueValidator.IsUniqueAsync(
                    _warehouseRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.WarehouseCode == entity.WarehouseCode);
                if (!isUnique_ix_takt_logistics_materials_warehouse_unique)
                {
                    throw new TaktBusinessException("仓库主数据的PlantCode、WarehouseCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _warehouseRepository.CreateAsync(entity);
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
    /// 导出仓库主数据
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportWarehouseAsync(TaktWarehouseQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktWarehouseQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktWarehouseExportDto>(),
                sheetName ?? "仓库主数据数据",
                fileName ?? "仓库主数据导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _warehouseRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktWarehouseExportDto>(),
                sheetName ?? "仓库主数据数据",
                fileName ?? "仓库主数据导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktWarehouseExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "仓库主数据数据",
            fileName ?? "仓库主数据导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充仓库主数据详情（加载 OneToMany 子表：库位主数据）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillWarehouseDetailsAsync(TaktWarehouseDto dto, TaktWarehouse entity)
    {
        if (dto == null)
        {
            return;
        }
        // 库位主数据 → dto.StorageLocations
        var storagelocations = await _storageLocationRepository.GetListAsync(x => x.WarehouseId == entity.Id);
        dto.StorageLocations = storagelocations.Adapt<List<TaktStorageLocationDto>>();
    }

    /// <summary>
    /// 保存仓库主数据子表级联（库位主数据；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveWarehouseChildrenAsync(TaktWarehouse entity, TaktWarehouseCreateDto dto)
    {
        // 库位主数据（StorageLocations）
        List<TaktStorageLocationUpdateDto>? storageLocationsForSave;
        if (dto is TaktWarehouseUpdateDto updateDtoForStorageLocations && updateDtoForStorageLocations.StorageLocations != null)
        {
            storageLocationsForSave = updateDtoForStorageLocations.StorageLocations;
        }
        else if (dto.StorageLocations != null)
        {
            storageLocationsForSave = dto.StorageLocations.Adapt<List<TaktStorageLocationUpdateDto>>();
        }
        else
        {
            storageLocationsForSave = null;
        }
        if (storageLocationsForSave is not { Count: > 0 })
        {
            await _storageLocationRepository.DeleteAsync(x => x.WarehouseId == entity.Id);
        }
        else
        {
            var existingList = await _storageLocationRepository.GetListAsync(x => x.WarehouseId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktStorageLocation>();
            for (var i = 0; i < storageLocationsForSave.Count; i++)
            {
                var childDto = storageLocationsForSave[i];
                childDto.WarehouseId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.WarehouseCode = entity.WarehouseCode;
                if (childDto.StorageLocationId > 0)
                {
                    if (!existingById.TryGetValue(childDto.StorageLocationId, out var target))
                    {
                        throw new TaktBusinessException("库位主数据不存在（StorageLocationId={childDto.StorageLocationId}）");
                    }
                    if (target.WarehouseId != entity.Id)
                    {
                        throw new TaktBusinessException("库位主数据不属于当前主表（StorageLocationId={childDto.StorageLocationId}）");
                    }
                    submittedIds.Add(childDto.StorageLocationId);
                    childDto.Adapt(target);
                    target.Id = childDto.StorageLocationId;
                    target.WarehouseId = entity.Id;
                    await _storageLocationRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktStorageLocation>();
                    child.Id = 0;
                    child.WarehouseId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _storageLocationRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _storageLocationRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建仓库主数据查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktWarehouse, bool>> QueryExpression(TaktWarehouseQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktWarehouse>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.WarehouseCode != null && x.WarehouseCode.Contains(keywords))
                || (x.WarehouseName != null && x.WarehouseName.Contains(keywords))
                || (x.WarehouseShortName != null && x.WarehouseShortName.Contains(keywords))
                || (x.Address != null && x.Address.Contains(keywords))
                || (x.ContactPerson != null && x.ContactPerson.Contains(keywords))
                || (x.ContactPhone != null && x.ContactPhone.Contains(keywords))
                || (x.ManagerUserName != null && x.ManagerUserName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.WarehouseCode))
        {
            var warehouseCode = queryDto.WarehouseCode;
            exp = exp.And(x => x.WarehouseCode != null && x.WarehouseCode.Contains(warehouseCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WarehouseName))
        {
            var warehouseName = queryDto.WarehouseName;
            exp = exp.And(x => x.WarehouseName != null && x.WarehouseName.Contains(warehouseName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WarehouseShortName))
        {
            var warehouseShortName = queryDto.WarehouseShortName;
            exp = exp.And(x => x.WarehouseShortName != null && x.WarehouseShortName.Contains(warehouseShortName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Address))
        {
            var address = queryDto.Address;
            exp = exp.And(x => x.Address != null && x.Address.Contains(address));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContactPerson))
        {
            var contactPerson = queryDto.ContactPerson;
            exp = exp.And(x => x.ContactPerson != null && x.ContactPerson.Contains(contactPerson));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ContactPhone))
        {
            var contactPhone = queryDto.ContactPhone;
            exp = exp.And(x => x.ContactPhone != null && x.ContactPhone.Contains(contactPhone));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ManagerUserName))
        {
            var managerUserCode = queryDto.ManagerUserName;
            exp = exp.And(x => x.ManagerUserName != null && x.ManagerUserName.Contains(managerUserCode));
        }

        if (queryDto?.IsVirtual.HasValue == true)
        {
            var isVirtual = queryDto.IsVirtual.Value;
            exp = exp.And(x => x.IsVirtual == isVirtual);
        }

        if (queryDto?.WarehouseType.HasValue == true)
        {
            var warehouseType = queryDto.WarehouseType.Value;
            exp = exp.And(x => x.WarehouseType == warehouseType);
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

        if (queryDto?.WarehouseStatus.HasValue == true)
        {
            var warehouseStatus = queryDto.WarehouseStatus.Value;
            exp = exp.And(x => x.WarehouseStatus == warehouseStatus);
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
    private static bool HasAnyListQueryFilter(TaktWarehouseQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.WarehouseCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WarehouseName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WarehouseShortName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Address))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContactPerson))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ContactPhone))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ManagerUserName))
        {
            return true;
        }
        if (queryDto.IsVirtual.HasValue)
        {
            return true;
        }
        if (queryDto.WarehouseType.HasValue)
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
        if (queryDto.WarehouseStatus.HasValue)
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
