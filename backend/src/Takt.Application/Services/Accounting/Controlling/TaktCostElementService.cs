// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Controlling
// 文件名称：TaktCostElementService.cs
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：成本要素应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Controlling;
using Takt.Domain.Entities.Accounting.Controlling;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Controlling;

/// <summary>
/// 成本要素应用服务
/// </summary>
public class TaktCostElementService : TaktServiceBase, ITaktCostElementService
{
    private readonly ITaktCompanyRepository<TaktCostElement> _costElementRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="costElementRepository">成本要素仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCostElementService(
        ITaktCompanyRepository<TaktCostElement> costElementRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _costElementRepository = costElementRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取成本要素列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCostElementDto>> GetCostElementListAsync(TaktCostElementQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCostElementDto>.Create(
                new List<TaktCostElementDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _costElementRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCostElementDto>.Create(
            data.Adapt<List<TaktCostElementDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取成本要素
    /// </summary>
    /// <param name="id">成本要素ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostElementDto?> GetCostElementByIdAsync(long id)
    {
        var entity = await _costElementRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCostElementDto>();
    }

    /// <summary>
    /// 获取成本要素树形选项列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    public async Task<List<TaktTreeSelectOption>> GetCostElementTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _costElementRepository.GetListAsync(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId && x.CostElementStatus == 1);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var isLeaf = false;
                return new TaktTreeSelectOption
                {
                    DictValue = item.Id.ToString(),
                    DictLabel = item.CostElementName,
                    SortOrder = item.SortOrder,
                    IsLeaf = isLeaf,
                    Children = null,
                };
            })
            .ToList();
    }

    /// <summary>
    /// 获取成本要素树形列表（懒加载：仅 parentId 直接子级一层；不整表加载、不递归构树）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    public async Task<List<TaktCostElementTreeDto>> GetCostElementTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        EnsureThreeLayerContext();
        Expression<Func<TaktCostElement, bool>> predicate = includeDisabled
            ? (x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId)
            : (x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId && x.CostElementStatus == 1);
        var list = await _costElementRepository.GetListAsync(predicate);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var treeDto = item.Adapt<TaktCostElementTreeDto>();
                treeDto.Children = new List<TaktCostElementTreeDto>();
                return treeDto;
            })
            .ToList();
    }

    /// <summary>
    /// 创建成本要素
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostElementDto> CreateCostElementAsync(TaktCostElementCreateDto dto)
    {
        var entity = dto.Adapt<TaktCostElement>();
        var isUnique_ix_cost_element_code_unique = await _uniqueValidator.IsUniqueAsync(
            _costElementRepository,
            x => x.CostElementCode == entity.CostElementCode);
        if (!isUnique_ix_cost_element_code_unique)
        {
            throw new TaktBusinessException("成本要素的CostElementCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _costElementRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
        }
        entity = await _costElementRepository.CreateAsync(entity);
        return await GetCostElementByIdAsync(entity.Id) ?? entity.Adapt<TaktCostElementDto>();
    }

    /// <summary>
    /// 更新成本要素
    /// </summary>
    /// <param name="id">成本要素ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostElementDto> UpdateCostElementAsync(long id, TaktCostElementUpdateDto dto)
    {
        var entity = await _costElementRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("成本要素不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_cost_element_code_unique = await _uniqueValidator.IsUniqueAsync(
            _costElementRepository,
            x => x.CostElementCode == entity.CostElementCode,
            id);
        if (!isUnique_ix_cost_element_code_unique)
        {
            throw new TaktBusinessException("成本要素的CostElementCode已存在");
        }
        await _costElementRepository.UpdateAsync(entity);
        return await GetCostElementByIdAsync(id) ?? throw new TaktBusinessException("成本要素不存在");
    }

    /// <summary>
    /// 删除成本要素
    /// </summary>
    /// <param name="id">成本要素ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCostElementByIdAsync(long id)
    {

        var hasChildren = await _costElementRepository.ExistsAsync(x => x.ParentId == id);
        if (hasChildren)
        {
            throw new TaktBusinessException("存在子节点，无法删除");
        }
        var deleted = await _costElementRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("成本要素不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除成本要素
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCostElementBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCostElementByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新成本要素状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostElementDto> UpdateCostElementStatusAsync(TaktCostElementStatusDto dto)
    {
        var entity = await _costElementRepository.GetByIdAsync(dto.CostElementId);
        if (entity == null)
        {
            throw new TaktBusinessException("成本要素不存在");
        }
        entity.CostElementStatus = dto.CostElementStatus;
        await _costElementRepository.UpdateAsync(entity);
        return await GetCostElementByIdAsync(dto.CostElementId) ?? throw new TaktBusinessException("成本要素不存在");
    }

    /// <summary>
    /// 更新成本要素排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostElementDto> UpdateCostElementSortAsync(TaktCostElementSortDto dto)
    {
        var entity = await _costElementRepository.GetByIdAsync(dto.CostElementId);
        if (entity == null)
        {
            throw new TaktBusinessException("成本要素不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _costElementRepository.UpdateAsync(entity);
        return await GetCostElementByIdAsync(dto.CostElementId) ?? throw new TaktBusinessException("成本要素不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCostElementTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCostElementTemplateDto>(
            sheetName ?? "成本要素导入模板",
            fileName ?? "成本要素导入模板.xlsx");
    }

    /// <summary>
    /// 导入成本要素
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCostElementAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCostElementImportDto>(fileStream, sheetName ?? "成本要素导入模板");
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
                var entity = rows[i].Adapt<TaktCostElement>();
                var importKey = $"{entity.CostElementCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CostElementCode）");
                }
                var isUnique_ix_cost_element_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _costElementRepository,
                    x => x.CostElementCode == entity.CostElementCode);
                if (!isUnique_ix_cost_element_code_unique)
                {
                    throw new TaktBusinessException("成本要素的CostElementCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _costElementRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
                }
                await _costElementRepository.CreateAsync(entity);
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
    /// 导出成本要素
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCostElementAsync(TaktCostElementQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCostElementQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCostElementExportDto>(),
                sheetName ?? "成本要素数据",
                fileName ?? "成本要素导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _costElementRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCostElementExportDto>(),
                sheetName ?? "成本要素数据",
                fileName ?? "成本要素导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCostElementExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "成本要素数据",
            fileName ?? "成本要素导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建成本要素查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCostElement, bool>> QueryExpression(TaktCostElementQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCostElement>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CostElementCode != null && x.CostElementCode.Contains(keywords))
                || (x.CostElementName != null && x.CostElementName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.CostElementCode))
        {
            var costElementCode = queryDto.CostElementCode;
            exp = exp.And(x => x.CostElementCode != null && x.CostElementCode.Contains(costElementCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CostElementName))
        {
            var costElementName = queryDto.CostElementName;
            exp = exp.And(x => x.CostElementName != null && x.CostElementName.Contains(costElementName));
        }

        if (queryDto?.CostElementType.HasValue == true)
        {
            var costElementType = queryDto.CostElementType.Value;
            exp = exp.And(x => x.CostElementType == costElementType);
        }

        if (queryDto?.CostElementCategory.HasValue == true)
        {
            var costElementCategory = queryDto.CostElementCategory.Value;
            exp = exp.And(x => x.CostElementCategory == costElementCategory);
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            var parentId = queryDto.ParentId.Value;
            exp = exp.And(x => x.ParentId == parentId);
        }

        if (queryDto?.CostElementLevel.HasValue == true)
        {
            var costElementLevel = queryDto.CostElementLevel.Value;
            exp = exp.And(x => x.CostElementLevel == costElementLevel);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.CostElementStatus.HasValue == true)
        {
            var costElementStatus = queryDto.CostElementStatus.Value;
            exp = exp.And(x => x.CostElementStatus == costElementStatus);
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

        if (queryDto?.ValidFromStart.HasValue == true)
        {
            var validFromStart = queryDto.ValidFromStart.Value;
            exp = exp.And(x => x.ValidFrom >= validFromStart);
        }

        if (queryDto?.ValidFromEnd.HasValue == true)
        {
            var validFromEnd = queryDto.ValidFromEnd.Value;
            exp = exp.And(x => x.ValidFrom <= validFromEnd);
        }

        if (queryDto?.ValidToStart.HasValue == true)
        {
            var validToStart = queryDto.ValidToStart.Value;
            exp = exp.And(x => x.ValidTo >= validToStart);
        }

        if (queryDto?.ValidToEnd.HasValue == true)
        {
            var validToEnd = queryDto.ValidToEnd.Value;
            exp = exp.And(x => x.ValidTo <= validToEnd);
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
    private static bool HasAnyListQueryFilter(TaktCostElementQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.CostElementCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CostElementName))
        {
            return true;
        }
        if (queryDto.CostElementType.HasValue)
        {
            return true;
        }
        if (queryDto.CostElementCategory.HasValue)
        {
            return true;
        }
        if (queryDto.ParentId.HasValue)
        {
            return true;
        }
        if (queryDto.CostElementLevel.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.CostElementStatus.HasValue)
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
        if (queryDto.ValidFromStart.HasValue || queryDto.ValidFromEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ValidToStart.HasValue || queryDto.ValidToEnd.HasValue)
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
