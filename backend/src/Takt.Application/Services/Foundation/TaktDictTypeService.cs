// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktDictTypeService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：字典类型应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 字典类型应用服务
/// </summary>
public class TaktDictTypeService : TaktServiceBase, ITaktDictTypeService
{
    private readonly ITaktTenantRepository<TaktDictType> _dictTypeRepository;
    private readonly ITaktTenantRepository<TaktDictData> _dictDataRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="dictTypeRepository">字典类型仓储</param>
    /// <param name="dictDataRepository">DictData仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDictTypeService(
        ITaktTenantRepository<TaktDictType> dictTypeRepository,
        ITaktTenantRepository<TaktDictData> dictDataRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _dictTypeRepository = dictTypeRepository;
        _dictDataRepository = dictDataRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取字典类型列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDictTypeDto>> GetDictTypeListAsync(TaktDictTypeQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktDictTypeDto>.Create(
                new List<TaktDictTypeDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _dictTypeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDictTypeDto>.Create(
            data.Adapt<List<TaktDictTypeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取字典类型
    /// </summary>
    /// <param name="id">字典类型ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictTypeDto?> GetDictTypeByIdAsync(long id)
    {
        var entity = await _dictTypeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktDictTypeDto>();
        await FillDictTypeDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取字典类型选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetDictTypeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        var list = await _dictTypeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.DictStatus == 1,
            x => x.DictTypeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.DictTypeCode,
            DictLabel = e.DictTypeName ?? e.DictTypeCode,
        }).ToList();
    }

    /// <summary>
    /// 创建字典类型
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictTypeDto> CreateDictTypeAsync(TaktDictTypeCreateDto dto)
    {
        var entity = dto.Adapt<TaktDictType>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_dict_type_code_unique = await _uniqueValidator.IsUniqueAsync(
            _dictTypeRepository,
            x => x.DictTypeCode == entity.DictTypeCode);
        if (!isUnique_ix_dict_type_code_unique)
        {
            throw new TaktBusinessException("字典类型的DictTypeCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _dictTypeRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        entity = await _dictTypeRepository.CreateAsync(entity);
                await SaveDictTypeChildrenAsync(entity, dto);
        return await GetDictTypeByIdAsync(entity.Id) ?? entity.Adapt<TaktDictTypeDto>();
    }

    /// <summary>
    /// 更新字典类型
    /// </summary>
    /// <param name="id">字典类型ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictTypeDto> UpdateDictTypeAsync(long id, TaktDictTypeUpdateDto dto)
    {
        var entity = await _dictTypeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("字典类型不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_dict_type_code_unique = await _uniqueValidator.IsUniqueAsync(
            _dictTypeRepository,
            x => x.DictTypeCode == entity.DictTypeCode,
            id);
        if (!isUnique_ix_dict_type_code_unique)
        {
            throw new TaktBusinessException("字典类型的DictTypeCode已存在");
        }
        await _dictTypeRepository.UpdateAsync(entity);
                await SaveDictTypeChildrenAsync(entity, dto);
        return await GetDictTypeByIdAsync(id) ?? throw new TaktBusinessException("字典类型不存在");
    }

    /// <summary>
    /// 删除字典类型
    /// </summary>
    /// <param name="id">字典类型ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDictTypeByIdAsync(long id)
    {
        var entity = await _dictTypeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("字典类型不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置字典类型不允许删除");
        }
        await _dictDataRepository.DeleteAsync(x => x.DictTypeId == entity.Id);
        var deleted = await _dictTypeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("字典类型不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除字典类型
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDictTypeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _dictTypeRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置字典类型不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteDictTypeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新字典类型状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictTypeDto> UpdateDictTypeStatusAsync(TaktDictTypeStatusDto dto)
    {
        var entity = await _dictTypeRepository.GetByIdAsync(dto.DictTypeId);
        if (entity == null)
        {
            throw new TaktBusinessException("字典类型不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.DictStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置字典类型");
        }
        entity.DictStatus = dto.DictStatus;
        await _dictTypeRepository.UpdateAsync(entity);
        return await GetDictTypeByIdAsync(dto.DictTypeId) ?? throw new TaktBusinessException("字典类型不存在");
    }

    /// <summary>
    /// 更新字典类型排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictTypeDto> UpdateDictTypeSortAsync(TaktDictTypeSortDto dto)
    {
        var entity = await _dictTypeRepository.GetByIdAsync(dto.DictTypeId);
        if (entity == null)
        {
            throw new TaktBusinessException("字典类型不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _dictTypeRepository.UpdateAsync(entity);
        return await GetDictTypeByIdAsync(dto.DictTypeId) ?? throw new TaktBusinessException("字典类型不存在");
    }

    /// <summary>
    /// 更新字典类型内置
    /// </summary>
    /// <param name="dto">内置 DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDictTypeDto> UpdateDictTypeBuiltInAsync(TaktDictTypeBuiltInDto dto)
    {
        var entity = await _dictTypeRepository.GetByIdAsync(dto.DictTypeId);
        if (entity == null)
        {
            throw new TaktBusinessException("字典类型不存在");
        }
        if (dto.IsBuiltIn is not 0 and not 1)
        {
            throw new TaktBusinessException("内置必须为字典 sys_yes_no 合法值（0=否，1=是）");
        }
        if (entity.IsBuiltIn == 1 && dto.IsBuiltIn != 1)
        {
            throw new TaktBusinessException("不允许取消内置字典类型标识");
        }
        entity.IsBuiltIn = dto.IsBuiltIn;
        await _dictTypeRepository.UpdateAsync(entity);
        return await GetDictTypeByIdAsync(dto.DictTypeId) ?? throw new TaktBusinessException("字典类型不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetDictTypeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktDictTypeTemplateDto>(
            sheetName ?? "字典类型导入模板",
            fileName ?? "字典类型导入模板.xlsx");
    }

    /// <summary>
    /// 导入字典类型
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportDictTypeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktDictTypeImportDto>(fileStream, sheetName ?? "字典类型导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _dictTypeRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktDictType>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.DictTypeCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DictTypeCode）");
                }
                var isUnique_ix_dict_type_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _dictTypeRepository,
                    x => x.DictTypeCode == entity.DictTypeCode);
                if (!isUnique_ix_dict_type_code_unique)
                {
                    throw new TaktBusinessException("字典类型的DictTypeCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                await _dictTypeRepository.CreateAsync(entity);
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
    /// 导出字典类型
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDictTypeAsync(TaktDictTypeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktDictTypeQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDictTypeExportDto>(),
                sheetName ?? "字典类型数据",
                fileName ?? "字典类型导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _dictTypeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDictTypeExportDto>(),
                sheetName ?? "字典类型数据",
                fileName ?? "字典类型导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDictTypeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "字典类型数据",
            fileName ?? "字典类型导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充字典类型详情（加载 OneToMany 子表：字典数据）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillDictTypeDetailsAsync(TaktDictTypeDto dto, TaktDictType entity)
    {
        if (dto == null)
        {
            return;
        }
        // 字典数据 → dto.DictDataList
        var dictdatalist = await _dictDataRepository.GetListAsync(x => x.DictTypeId == entity.Id);
        dto.DictDataList = dictdatalist.Adapt<List<TaktDictDataDto>>();
    }

    /// <summary>
    /// 保存字典类型子表级联（字典数据；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveDictTypeChildrenAsync(TaktDictType entity, TaktDictTypeCreateDto dto)
    {
        // 字典数据（DictDataList）
        List<TaktDictDataUpdateDto>? dictDataListForSave;
        if (dto is TaktDictTypeUpdateDto updateDtoForDictDataList && updateDtoForDictDataList.DictDataList != null)
        {
            dictDataListForSave = updateDtoForDictDataList.DictDataList;
        }
        else if (dto.DictDataList != null)
        {
            dictDataListForSave = dto.DictDataList.Adapt<List<TaktDictDataUpdateDto>>();
        }
        else
        {
            dictDataListForSave = null;
        }
        if (dictDataListForSave is not { Count: > 0 })
        {
            await _dictDataRepository.DeleteAsync(x => x.DictTypeId == entity.Id);
        }
        else
        {
            var existingList = await _dictDataRepository.GetListAsync(x => x.DictTypeId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktDictData>();
            for (var i = 0; i < dictDataListForSave.Count; i++)
            {
                var childDto = dictDataListForSave[i];
                childDto.DictTypeId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                if (childDto.DictDataId > 0)
                {
                    if (!existingById.TryGetValue(childDto.DictDataId, out var target))
                    {
                        throw new TaktBusinessException("字典数据不存在（DictDataId={childDto.DictDataId}）");
                    }
                    if (target.DictTypeId != entity.Id)
                    {
                        throw new TaktBusinessException("字典数据不属于当前主表（DictDataId={childDto.DictDataId}）");
                    }
                    submittedIds.Add(childDto.DictDataId);
                    childDto.Adapt(target);
                    target.Id = childDto.DictDataId;
                    target.DictTypeId = entity.Id;
                    await _dictDataRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktDictData>();
                    child.Id = 0;
                    child.DictTypeId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _dictDataRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _dictDataRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建字典类型查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDictType, bool>> QueryExpression(TaktDictTypeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDictType>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.DictTypeCode != null && x.DictTypeCode.Contains(keywords))
                || (x.DictTypeName != null && x.DictTypeName.Contains(keywords))
                || (x.DictScript != null && x.DictScript.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DictTypeCode))
        {
            var dictTypeCode = queryDto.DictTypeCode;
            exp = exp.And(x => x.DictTypeCode != null && x.DictTypeCode.Contains(dictTypeCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DictTypeName))
        {
            var dictTypeName = queryDto.DictTypeName;
            exp = exp.And(x => x.DictTypeName != null && x.DictTypeName.Contains(dictTypeName));
        }

        if (queryDto?.DataSource.HasValue == true)
        {
            var dataSource = queryDto.DataSource.Value;
            exp = exp.And(x => x.DataSource == dataSource);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DictScript))
        {
            var dictScript = queryDto.DictScript;
            exp = exp.And(x => x.DictScript != null && x.DictScript.Contains(dictScript));
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

        if (queryDto?.DictStatus.HasValue == true)
        {
            var dictStatus = queryDto.DictStatus.Value;
            exp = exp.And(x => x.DictStatus == dictStatus);
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
    private static bool HasAnyListQueryFilter(TaktDictTypeQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DictTypeCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DictTypeName))
        {
            return true;
        }
        if (queryDto.DataSource.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DictScript))
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
        if (queryDto.DictStatus.HasValue)
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
