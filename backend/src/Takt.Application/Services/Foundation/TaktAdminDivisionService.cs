// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktAdminDivisionService.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：行政区划应用服务实现
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
/// 行政区划应用服务
/// </summary>
public class TaktAdminDivisionService : TaktServiceBase, ITaktAdminDivisionService
{
    private readonly ITaktTenantRepository<TaktAdminDivision> _adminDivisionRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="adminDivisionRepository">行政区划仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAdminDivisionService(
        ITaktTenantRepository<TaktAdminDivision> adminDivisionRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _adminDivisionRepository = adminDivisionRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取行政区划列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAdminDivisionDto>> GetAdminDivisionListAsync(TaktAdminDivisionQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktAdminDivisionDto>.Create(
                new List<TaktAdminDivisionDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _adminDivisionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAdminDivisionDto>.Create(
            data.Adapt<List<TaktAdminDivisionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取行政区划
    /// </summary>
    /// <param name="id">行政区划ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAdminDivisionDto?> GetAdminDivisionByIdAsync(long id)
    {
        var entity = await _adminDivisionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode)
        {
            return null;
        }
        return entity.Adapt<TaktAdminDivisionDto>();
    }

    /// <summary>
    /// 获取行政区划平铺选项（DictValue=DivisionCode；供省/市等业务字段下拉，非 parentId）。
    /// </summary>
    /// <param name="plantCode">工厂代码（可选；本实体无工厂列，忽略）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配编码/名称）</param>
    /// <param name="level">层级（可选；1～6）</param>
    /// <returns>下拉选项（有上限，禁止全表）</returns>
    public async Task<List<TaktSelectOption>> GetAdminDivisionOptionsAsync(string? plantCode = null, string? keyword = null, int? level = null)
    {
        _ = plantCode;
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktAdminDivision>()
            .And(x => x.TenantCode == CurrentTenantCode && x.DivisionStatus == 1)
            .AndIF(level is >= 1 and <= 6, x => x.Level == level)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.DivisionCode != null && x.DivisionCode.Contains(normalizedKeyword!))
                || (x.DivisionName != null && x.DivisionName.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _adminDivisionRepository.GetListForExportAsync(predicate, 500);
        return list
            .OrderBy(x => x.Level)
            .ThenBy(x => x.SortOrder)
            .ThenBy(x => x.DivisionCode)
            .Select(e => new TaktSelectOption
            {
                DictValue = e.DivisionCode,
                DictLabel = string.IsNullOrWhiteSpace(e.DivisionName) ? e.DivisionCode : e.DivisionName,
                ExtLabel = e.DivisionCode,
                SortOrder = e.SortOrder,
            })
            .ToList();
    }

    /// <summary>
    /// 获取行政区划树形选项（懒加载：仅 parentId 直接子级一层；DictValue=Id 字符串，供表单 parentId）。
    /// 根层 DictLabel=CountryCode，子级 DictLabel=DivisionName；不整表、不递归。
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    public async Task<List<TaktTreeSelectOption>> GetAdminDivisionTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null)
    {
        var list = await _adminDivisionRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.ParentId == parentId
            && x.DivisionStatus == 1);
        return list
            .OrderBy(x => x.CountryCode)
            .ThenBy(x => x.SortOrder)
            .Select(item =>
            {
                var isLeaf = TaktLazyTreeHelper.ToAntIsLeaf(item.IsLeaf);
                var isCountryRoot = item.ParentId == 0 || item.Level == 1;
                var dictLabel = isCountryRoot
                    ? (string.IsNullOrWhiteSpace(item.CountryCode) ? item.DivisionName : item.CountryCode)
                    : (string.IsNullOrWhiteSpace(item.DivisionName) ? item.DivisionCode : item.DivisionName);
                return new TaktTreeSelectOption
                {
                    DictValue = item.Id.ToString(),
                    DictLabel = dictLabel,
                    ExtLabel = item.DivisionCode,
                    ExtValue = item.Id.ToString(),
                    SortOrder = item.SortOrder,
                    IsLeaf = isLeaf,
                    Children = null,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 获取行政区划树形列表（懒加载：仅 parentId 直接子级一层；不整表加载、不递归构树）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    public async Task<List<TaktAdminDivisionTreeDto>> GetAdminDivisionTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        Expression<Func<TaktAdminDivision, bool>> predicate = includeDisabled
            ? (x => x.TenantCode == CurrentTenantCode && x.ParentId == parentId)
            : (x => x.TenantCode == CurrentTenantCode && x.ParentId == parentId && x.DivisionStatus == 1);
        var list = await _adminDivisionRepository.GetListAsync(predicate);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var treeDto = item.Adapt<TaktAdminDivisionTreeDto>();
                treeDto.Children = null;
                return treeDto;
            })
            .ToList();
    }

    /// <summary>
    /// 创建行政区划
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAdminDivisionDto> CreateAdminDivisionAsync(TaktAdminDivisionCreateDto dto)
    {
        var entity = dto.Adapt<TaktAdminDivision>();
        entity.IsBuiltIn = 0;
        var isUnique_ix_admin_division_code_unique = await _uniqueValidator.IsUniqueAsync(
            _adminDivisionRepository,
            x => x.DivisionCode == entity.DivisionCode);
        if (!isUnique_ix_admin_division_code_unique)
        {
            throw new TaktBusinessException("行政区划的DivisionCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _adminDivisionRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.ParentId == entity.ParentId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
        }
        entity = await _adminDivisionRepository.CreateAsync(entity);
        return await GetAdminDivisionByIdAsync(entity.Id) ?? entity.Adapt<TaktAdminDivisionDto>();
    }

    /// <summary>
    /// 更新行政区划
    /// </summary>
    /// <param name="id">行政区划ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAdminDivisionDto> UpdateAdminDivisionAsync(long id, TaktAdminDivisionUpdateDto dto)
    {
        var entity = await _adminDivisionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("行政区划不存在");
        }
        var originalIsBuiltIn = entity.IsBuiltIn;
        dto.Adapt(entity);
        entity.IsBuiltIn = originalIsBuiltIn;
        var isUnique_ix_admin_division_code_unique = await _uniqueValidator.IsUniqueAsync(
            _adminDivisionRepository,
            x => x.DivisionCode == entity.DivisionCode,
            id);
        if (!isUnique_ix_admin_division_code_unique)
        {
            throw new TaktBusinessException("行政区划的DivisionCode已存在");
        }
        await _adminDivisionRepository.UpdateAsync(entity);
        return await GetAdminDivisionByIdAsync(id) ?? throw new TaktBusinessException("行政区划不存在");
    }

    /// <summary>
    /// 删除行政区划
    /// </summary>
    /// <param name="id">行政区划ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAdminDivisionByIdAsync(long id)
    {
        var entity = await _adminDivisionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("行政区划不存在或已删除");
        }
        if (entity.IsBuiltIn == 1)
        {
            throw new TaktBusinessException("内置行政区划不允许删除");
        }

        var hasChildren = await _adminDivisionRepository.ExistsAsync(x => x.ParentId == id);
        if (hasChildren)
        {
            throw new TaktBusinessException("存在子节点，无法删除");
        }
        var deleted = await _adminDivisionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("行政区划不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除行政区划
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAdminDivisionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        if (await _adminDivisionRepository.ExistsAsync(x => idList.Contains(x.Id) && x.IsBuiltIn == 1))
        {
            throw new TaktBusinessException("内置行政区划不允许删除");
        }
        foreach (var id in idList)
        {
            await DeleteAdminDivisionByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新行政区划状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAdminDivisionDto> UpdateAdminDivisionStatusAsync(TaktAdminDivisionStatusDto dto)
    {
        var entity = await _adminDivisionRepository.GetByIdAsync(dto.AdminDivisionId);
        if (entity == null)
        {
            throw new TaktBusinessException("行政区划不存在");
        }
        if (entity.IsBuiltIn == 1 && dto.DivisionStatus != 1)
        {
            throw new TaktBusinessException("不允许禁用内置行政区划");
        }
        entity.DivisionStatus = dto.DivisionStatus;
        await _adminDivisionRepository.UpdateAsync(entity);
        return await GetAdminDivisionByIdAsync(dto.AdminDivisionId) ?? throw new TaktBusinessException("行政区划不存在");
    }

    /// <summary>
    /// 更新行政区划排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAdminDivisionDto> UpdateAdminDivisionSortAsync(TaktAdminDivisionSortDto dto)
    {
        var entity = await _adminDivisionRepository.GetByIdAsync(dto.AdminDivisionId);
        if (entity == null)
        {
            throw new TaktBusinessException("行政区划不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _adminDivisionRepository.UpdateAsync(entity);
        return await GetAdminDivisionByIdAsync(dto.AdminDivisionId) ?? throw new TaktBusinessException("行政区划不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAdminDivisionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAdminDivisionTemplateDto>(
            sheetName ?? "行政区划导入模板",
            fileName ?? "行政区划导入模板.xlsx");
    }

    /// <summary>
    /// 导入行政区划
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAdminDivisionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAdminDivisionImportDto>(fileStream, sheetName ?? "行政区划导入模板");
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
                var entity = rows[i].Adapt<TaktAdminDivision>();
                entity.IsBuiltIn = 0;
                var importKey = $"{entity.DivisionCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DivisionCode）");
                }
                var isUnique_ix_admin_division_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _adminDivisionRepository,
                    x => x.DivisionCode == entity.DivisionCode);
                if (!isUnique_ix_admin_division_code_unique)
                {
                    throw new TaktBusinessException("行政区划的DivisionCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _adminDivisionRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.ParentId == entity.ParentId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
                }
                await _adminDivisionRepository.CreateAsync(entity);
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
    /// 导出行政区划
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAdminDivisionAsync(TaktAdminDivisionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktAdminDivisionQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAdminDivisionExportDto>(),
                sheetName ?? "行政区划数据",
                fileName ?? "行政区划导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _adminDivisionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAdminDivisionExportDto>(),
                sheetName ?? "行政区划数据",
                fileName ?? "行政区划导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAdminDivisionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "行政区划数据",
            fileName ?? "行政区划导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建行政区划查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAdminDivision, bool>> QueryExpression(TaktAdminDivisionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAdminDivision>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CountryCode != null && x.CountryCode.Contains(keywords))
                || (x.DivisionCode != null && x.DivisionCode.Contains(keywords))
                || (x.DivisionName != null && x.DivisionName.Contains(keywords))
                || (x.DivisionPath != null && x.DivisionPath.Contains(keywords))
                || (x.PostalCode != null && x.PostalCode.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.PhoneCode != null && x.PhoneCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
            );
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CountryCode))
        {
            var countryCode = queryDto.CountryCode;
            exp = exp.And(x => x.CountryCode != null && x.CountryCode.Contains(countryCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DivisionCode))
        {
            var divisionCode = queryDto.DivisionCode;
            exp = exp.And(x => x.DivisionCode != null && x.DivisionCode.Contains(divisionCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DivisionName))
        {
            var divisionName = queryDto.DivisionName;
            exp = exp.And(x => x.DivisionName != null && x.DivisionName.Contains(divisionName));
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            var parentId = queryDto.ParentId.Value;
            exp = exp.And(x => x.ParentId == parentId);
        }

        if (queryDto?.Level.HasValue == true)
        {
            var level = queryDto.Level.Value;
            exp = exp.And(x => x.Level == level);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DivisionPath))
        {
            var divisionPath = queryDto.DivisionPath;
            exp = exp.And(x => x.DivisionPath != null && x.DivisionPath.Contains(divisionPath));
        }

        if (queryDto?.IsLeaf.HasValue == true)
        {
            var isLeaf = queryDto.IsLeaf.Value;
            exp = exp.And(x => x.IsLeaf == isLeaf);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostalCode))
        {
            var postalCode = queryDto.PostalCode;
            exp = exp.And(x => x.PostalCode != null && x.PostalCode.Contains(postalCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PhoneCode))
        {
            var phoneCode = queryDto.PhoneCode;
            exp = exp.And(x => x.PhoneCode != null && x.PhoneCode.Contains(phoneCode));
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

        if (queryDto?.DivisionStatus.HasValue == true)
        {
            var divisionStatus = queryDto.DivisionStatus.Value;
            exp = exp.And(x => x.DivisionStatus == divisionStatus);
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
    private static bool HasAnyListQueryFilter(TaktAdminDivisionQueryDto? queryDto)
    {
        if (queryDto == null)
        {
            return false;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.KeyWords))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CountryCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DivisionCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DivisionName))
        {
            return true;
        }
        if (queryDto.ParentId.HasValue)
        {
            return true;
        }
        if (queryDto.Level.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DivisionPath))
        {
            return true;
        }
        if (queryDto.IsLeaf.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostalCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PhoneCode))
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
        if (queryDto.DivisionStatus.HasValue)
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
