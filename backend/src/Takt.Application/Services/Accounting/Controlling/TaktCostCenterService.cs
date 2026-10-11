// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Controlling
// 文件名称：TaktCostCenterService.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：成本中心应用服务实现
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
/// 成本中心应用服务
/// </summary>
public class TaktCostCenterService : TaktServiceBase, ITaktCostCenterService
{
    private readonly ITaktCompanyRepository<TaktCostCenter> _costCenterRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="costCenterRepository">成本中心仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCostCenterService(
        ITaktCompanyRepository<TaktCostCenter> costCenterRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _costCenterRepository = costCenterRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取成本中心列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCostCenterDto>> GetCostCenterListAsync(TaktCostCenterQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCostCenterDto>.Create(
                new List<TaktCostCenterDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _costCenterRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCostCenterDto>.Create(
            data.Adapt<List<TaktCostCenterDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取成本中心
    /// </summary>
    /// <param name="id">成本中心ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterDto?> GetCostCenterByIdAsync(long id)
    {
        var entity = await _costCenterRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktCostCenterDto>();
    }

    /// <summary>
    /// 获取成本中心树形选项列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    public async Task<List<TaktTreeSelectOption>> GetCostCenterTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _costCenterRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.ParentId == parentId
            && x.CostCenterStatus == 1);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item => new TaktTreeSelectOption
            {
                DictValue = item.Id.ToString(),
                DictLabel = item.CostCenterName ?? item.CostCenterCode,
                SortOrder = item.SortOrder,
                IsLeaf = false,
                Children = null,
            })
            .ToList();
    }

    /// <summary>
    /// 获取成本中心树形列表（懒加载：仅 parentId 直接子级一层；不整表加载、不递归构树）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    public async Task<List<TaktCostCenterTreeDto>> GetCostCenterTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        EnsureThreeLayerContext();
        Expression<Func<TaktCostCenter, bool>> predicate = includeDisabled
            ? (x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId)
            : (x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId && x.CostCenterStatus == 1);
        var list = await _costCenterRepository.GetListAsync(predicate);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var treeDto = item.Adapt<TaktCostCenterTreeDto>();
                treeDto.Children = null;
                return treeDto;
            })
            .ToList();
    }

    /// <summary>
    /// 创建成本中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterDto> CreateCostCenterAsync(TaktCostCenterCreateDto dto)
    {
        var entity = dto.Adapt<TaktCostCenter>();
        var isUnique_ix_cost_center_code_unique = await _uniqueValidator.IsUniqueAsync(
            _costCenterRepository,
            x => x.CostCenterCode == entity.CostCenterCode);
        if (!isUnique_ix_cost_center_code_unique)
        {
            throw new TaktBusinessException("成本中心的CostCenterCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _costCenterRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
        }
        entity = await _costCenterRepository.CreateAsync(entity);
        return await GetCostCenterByIdAsync(entity.Id) ?? entity.Adapt<TaktCostCenterDto>();
    }

    /// <summary>
    /// 更新成本中心
    /// </summary>
    /// <param name="id">成本中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterDto> UpdateCostCenterAsync(long id, TaktCostCenterUpdateDto dto)
    {
        var entity = await _costCenterRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("成本中心不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_cost_center_code_unique = await _uniqueValidator.IsUniqueAsync(
            _costCenterRepository,
            x => x.CostCenterCode == entity.CostCenterCode,
            id);
        if (!isUnique_ix_cost_center_code_unique)
        {
            throw new TaktBusinessException("成本中心的CostCenterCode已存在");
        }
        await _costCenterRepository.UpdateAsync(entity);
        return await GetCostCenterByIdAsync(id) ?? throw new TaktBusinessException("成本中心不存在");
    }

    /// <summary>
    /// 删除成本中心
    /// </summary>
    /// <param name="id">成本中心ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCostCenterByIdAsync(long id)
    {

        var hasChildren = await _costCenterRepository.ExistsAsync(x => x.ParentId == id);
        if (hasChildren)
        {
            throw new TaktBusinessException("存在子节点，无法删除");
        }
        var deleted = await _costCenterRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("成本中心不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除成本中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCostCenterBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCostCenterByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新成本中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterDto> UpdateCostCenterStatusAsync(TaktCostCenterStatusDto dto)
    {
        var entity = await _costCenterRepository.GetByIdAsync(dto.CostCenterId);
        if (entity == null)
        {
            throw new TaktBusinessException("成本中心不存在");
        }
        entity.CostCenterStatus = dto.CostCenterStatus;
        await _costCenterRepository.UpdateAsync(entity);
        return await GetCostCenterByIdAsync(dto.CostCenterId) ?? throw new TaktBusinessException("成本中心不存在");
    }

    /// <summary>
    /// 更新成本中心排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCostCenterDto> UpdateCostCenterSortAsync(TaktCostCenterSortDto dto)
    {
        var entity = await _costCenterRepository.GetByIdAsync(dto.CostCenterId);
        if (entity == null)
        {
            throw new TaktBusinessException("成本中心不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _costCenterRepository.UpdateAsync(entity);
        return await GetCostCenterByIdAsync(dto.CostCenterId) ?? throw new TaktBusinessException("成本中心不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCostCenterTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCostCenterTemplateDto>(
            sheetName ?? "成本中心导入模板",
            fileName ?? "成本中心导入模板.xlsx");
    }

    /// <summary>
    /// 导入成本中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCostCenterAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCostCenterImportDto>(fileStream, sheetName ?? "成本中心导入模板");
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
                var entity = rows[i].Adapt<TaktCostCenter>();
                var importKey = $"{entity.CostCenterCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CostCenterCode）");
                }
                var isUnique_ix_cost_center_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _costCenterRepository,
                    x => x.CostCenterCode == entity.CostCenterCode);
                if (!isUnique_ix_cost_center_code_unique)
                {
                    throw new TaktBusinessException("成本中心的CostCenterCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _costCenterRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
                }
                await _costCenterRepository.CreateAsync(entity);
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
    /// 导出成本中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCostCenterAsync(TaktCostCenterQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCostCenterQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCostCenterExportDto>(),
                sheetName ?? "成本中心数据",
                fileName ?? "成本中心导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _costCenterRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCostCenterExportDto>(),
                sheetName ?? "成本中心数据",
                fileName ?? "成本中心导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCostCenterExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "成本中心数据",
            fileName ?? "成本中心导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建成本中心查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCostCenter, bool>> QueryExpression(TaktCostCenterQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCostCenter>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CostCenterCode != null && x.CostCenterCode.Contains(keywords))
                || (x.CostCenterName != null && x.CostCenterName.Contains(keywords))
                || (x.CostCenterType != null && x.CostCenterType.Contains(keywords))
                || (x.ManagerName != null && x.ManagerName.Contains(keywords))
                || (x.DeptName != null && x.DeptName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.CostCenterCode))
        {
            var costCenterCode = queryDto.CostCenterCode;
            exp = exp.And(x => x.CostCenterCode != null && x.CostCenterCode.Contains(costCenterCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CostCenterName))
        {
            var costCenterName = queryDto.CostCenterName;
            exp = exp.And(x => x.CostCenterName != null && x.CostCenterName.Contains(costCenterName));
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            var parentId = queryDto.ParentId.Value;
            exp = exp.And(x => x.ParentId == parentId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CostCenterType))
        {
            var costCenterType = queryDto.CostCenterType;
            exp = exp.And(x => x.CostCenterType != null && x.CostCenterType.Contains(costCenterType));
        }

        if (queryDto?.ManagerId.HasValue == true)
        {
            var managerId = queryDto.ManagerId.Value;
            exp = exp.And(x => x.ManagerId == managerId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ManagerName))
        {
            var managerName = queryDto.ManagerName;
            exp = exp.And(x => x.ManagerName != null && x.ManagerName.Contains(managerName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            var deptId = queryDto.DeptId.Value;
            exp = exp.And(x => x.DeptId == deptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptName))
        {
            var deptName = queryDto.DeptName;
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(deptName));
        }

        if (queryDto?.CostCenterLevel.HasValue == true)
        {
            var costCenterLevel = queryDto.CostCenterLevel.Value;
            exp = exp.And(x => x.CostCenterLevel == costCenterLevel);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.CostCenterStatus.HasValue == true)
        {
            var costCenterStatus = queryDto.CostCenterStatus.Value;
            exp = exp.And(x => x.CostCenterStatus == costCenterStatus);
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
    private static bool HasAnyListQueryFilter(TaktCostCenterQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.CostCenterCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CostCenterName))
        {
            return true;
        }
        if (queryDto.ParentId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CostCenterType))
        {
            return true;
        }
        if (queryDto.ManagerId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ManagerName))
        {
            return true;
        }
        if (queryDto.DeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptName))
        {
            return true;
        }
        if (queryDto.CostCenterLevel.HasValue)
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.CostCenterStatus.HasValue)
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
