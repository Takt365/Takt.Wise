// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Controlling
// 文件名称：TaktProfitCenterService.cs
// 创建时间：2026-07-02
// 创建人：Takt365(Cursor AI)
// 功能描述：利润中心应用服务实现
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
/// 利润中心应用服务
/// </summary>
public class TaktProfitCenterService : TaktServiceBase, ITaktProfitCenterService
{
    private readonly ITaktCompanyRepository<TaktProfitCenter> _profitCenterRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="profitCenterRepository">利润中心仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProfitCenterService(
        ITaktCompanyRepository<TaktProfitCenter> profitCenterRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _profitCenterRepository = profitCenterRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取利润中心列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProfitCenterDto>> GetProfitCenterListAsync(TaktProfitCenterQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _profitCenterRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProfitCenterDto>.Create(
            data.Adapt<List<TaktProfitCenterDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取利润中心
    /// </summary>
    /// <param name="id">利润中心ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitCenterDto?> GetProfitCenterByIdAsync(long id)
    {
        var entity = await _profitCenterRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktProfitCenterDto>();
        await FillProfitCenterDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取利润中心树形选项列表（懒加载：仅 parentId 直接子级一层）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>树形选项（一层）</returns>
    public async Task<List<TaktTreeSelectOption>> GetProfitCenterTreeOptionsAsync(long parentId = 0, string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _profitCenterRepository.GetListAsync(x =>
            x.TenantCode == CurrentTenantCode
            && x.CompanyCode == CurrentCompanyCode
            && x.ParentId == parentId
            && x.ProfitCenterStatus == 1);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item => new TaktTreeSelectOption
            {
                DictValue = item.Id.ToString(),
                DictLabel = item.ProfitCenterName ?? item.ProfitCenterCode,
                SortOrder = item.SortOrder,
                IsLeaf = false,
                Children = null,
            })
            .ToList();
    }

    /// <summary>
    /// 获取利润中心树形列表（懒加载：仅 parentId 直接子级一层；不整表加载、不递归构树）
    /// </summary>
    /// <param name="parentId">父级ID（0=根）</param>
    /// <param name="includeDisabled">是否包含禁用项</param>
    /// <returns>树形列表（一层）</returns>
    public async Task<List<TaktProfitCenterTreeDto>> GetProfitCenterTreeAsync(long parentId = 0, bool includeDisabled = false)
    {
        EnsureThreeLayerContext();
        Expression<Func<TaktProfitCenter, bool>> predicate = includeDisabled
            ? (x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId)
            : (x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == parentId && x.ProfitCenterStatus == 1);
        var list = await _profitCenterRepository.GetListAsync(predicate);
        return list
            .OrderBy(x => x.SortOrder)
            .Select(item =>
            {
                var treeDto = item.Adapt<TaktProfitCenterTreeDto>();
                treeDto.Children = null;
                return treeDto;
            })
            .ToList();
    }

    /// <summary>
    /// 创建利润中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitCenterDto> CreateProfitCenterAsync(TaktProfitCenterCreateDto dto)
    {
        var entity = dto.Adapt<TaktProfitCenter>();
        var isUnique_ix_profit_center_code_unique = await _uniqueValidator.IsUniqueAsync(
            _profitCenterRepository,
            x => x.ProfitCenterCode == entity.ProfitCenterCode);
        if (!isUnique_ix_profit_center_code_unique)
        {
            throw new TaktBusinessException("利润中心的ProfitCenterCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _profitCenterRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
        }
        entity = await _profitCenterRepository.CreateAsync(entity);
                await SaveProfitCenterChildrenAsync(entity, dto);
        return await GetProfitCenterByIdAsync(entity.Id) ?? entity.Adapt<TaktProfitCenterDto>();
    }

    /// <summary>
    /// 更新利润中心
    /// </summary>
    /// <param name="id">利润中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitCenterDto> UpdateProfitCenterAsync(long id, TaktProfitCenterUpdateDto dto)
    {
        var entity = await _profitCenterRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("利润中心不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_profit_center_code_unique = await _uniqueValidator.IsUniqueAsync(
            _profitCenterRepository,
            x => x.ProfitCenterCode == entity.ProfitCenterCode,
            id);
        if (!isUnique_ix_profit_center_code_unique)
        {
            throw new TaktBusinessException("利润中心的ProfitCenterCode已存在");
        }
        await _profitCenterRepository.UpdateAsync(entity);
                await SaveProfitCenterChildrenAsync(entity, dto);
        return await GetProfitCenterByIdAsync(id) ?? throw new TaktBusinessException("利润中心不存在");
    }

    /// <summary>
    /// 删除利润中心
    /// </summary>
    /// <param name="id">利润中心ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProfitCenterByIdAsync(long id)
    {
        var entity = await _profitCenterRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("利润中心不存在或已删除");
        }        var deleted = await _profitCenterRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("利润中心不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除利润中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProfitCenterBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProfitCenterByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新利润中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitCenterDto> UpdateProfitCenterStatusAsync(TaktProfitCenterStatusDto dto)
    {
        var entity = await _profitCenterRepository.GetByIdAsync(dto.ProfitCenterId);
        if (entity == null)
        {
            throw new TaktBusinessException("利润中心不存在");
        }
        entity.ProfitCenterStatus = dto.ProfitCenterStatus;
        await _profitCenterRepository.UpdateAsync(entity);
        return await GetProfitCenterByIdAsync(dto.ProfitCenterId) ?? throw new TaktBusinessException("利润中心不存在");
    }

    /// <summary>
    /// 更新利润中心排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitCenterDto> UpdateProfitCenterSortAsync(TaktProfitCenterSortDto dto)
    {
        var entity = await _profitCenterRepository.GetByIdAsync(dto.ProfitCenterId);
        if (entity == null)
        {
            throw new TaktBusinessException("利润中心不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _profitCenterRepository.UpdateAsync(entity);
        return await GetProfitCenterByIdAsync(dto.ProfitCenterId) ?? throw new TaktBusinessException("利润中心不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProfitCenterTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProfitCenterTemplateDto>(
            sheetName ?? "利润中心导入模板",
            fileName ?? "利润中心导入模板.xlsx");
    }

    /// <summary>
    /// 导入利润中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProfitCenterAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProfitCenterImportDto>(fileStream, sheetName ?? "利润中心导入模板");
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
                var entity = rows[i].Adapt<TaktProfitCenter>();
                var importKey = $"{entity.ProfitCenterCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ProfitCenterCode）");
                }
                var isUnique_ix_profit_center_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _profitCenterRepository,
                    x => x.ProfitCenterCode == entity.ProfitCenterCode);
                if (!isUnique_ix_profit_center_code_unique)
                {
                    throw new TaktBusinessException("利润中心的ProfitCenterCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _profitCenterRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ParentId == entity.ParentId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(entity.ParentId, maxSort);
                }
                await _profitCenterRepository.CreateAsync(entity);
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
    /// 导出利润中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProfitCenterAsync(TaktProfitCenterQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProfitCenterQueryDto());
        var list = await _profitCenterRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProfitCenterExportDto>(),
                sheetName ?? "利润中心数据",
                fileName ?? "利润中心导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProfitCenterExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "利润中心数据",
            fileName ?? "利润中心导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充利润中心详情（加载 OneToMany 子表：利润中心变更记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillProfitCenterDetailsAsync(TaktProfitCenterDto dto, TaktProfitCenter entity)
    {
        if (dto == null)
        {
            return;
        }
    }

    /// <summary>
    /// 保存利润中心子表级联（利润中心变更记录；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveProfitCenterChildrenAsync(TaktProfitCenter entity, TaktProfitCenterCreateDto dto)
    {
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建利润中心查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProfitCenter, bool>> QueryExpression(TaktProfitCenterQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProfitCenter>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(keywords))
                || (x.ProfitCenterName != null && x.ProfitCenterName.Contains(keywords))
                || SqlFunc.ToString(x.ParentId).Contains(keywords)
                || SqlFunc.ToString(x.ManagerId).Contains(keywords)
                || (x.ManagerName != null && x.ManagerName.Contains(keywords))
                || SqlFunc.ToString(x.DeptId).Contains(keywords)
                || (x.DeptName != null && x.DeptName.Contains(keywords))
                || SqlFunc.ToString(x.ProfitCenterLevel).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.ProfitCenterStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ValidFrom).Contains(keywords)
                || SqlFunc.ToString(x.ValidTo).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ProfitCenterCode))
        {
            exp = exp.And(x => x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(queryDto.ProfitCenterCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProfitCenterName))
        {
            exp = exp.And(x => x.ProfitCenterName != null && x.ProfitCenterName.Contains(queryDto.ProfitCenterName));
        }

        if (queryDto?.ParentId.HasValue == true)
        {
            exp = exp.And(x => x.ParentId == queryDto.ParentId);
        }

        if (queryDto?.ManagerId.HasValue == true)
        {
            exp = exp.And(x => x.ManagerId == queryDto.ManagerId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ManagerName))
        {
            exp = exp.And(x => x.ManagerName != null && x.ManagerName.Contains(queryDto.ManagerName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            exp = exp.And(x => x.DeptId == queryDto.DeptId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeptName))
        {
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(queryDto.DeptName));
        }

        if (queryDto?.ProfitCenterLevel.HasValue == true)
        {
            exp = exp.And(x => x.ProfitCenterLevel == queryDto.ProfitCenterLevel);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.ProfitCenterStatus.HasValue == true)
        {
            exp = exp.And(x => x.ProfitCenterStatus == queryDto.ProfitCenterStatus);
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

        if (queryDto?.ValidFromStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidFrom >= queryDto.ValidFromStart);
        }

        if (queryDto?.ValidFromEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidFrom <= queryDto.ValidFromEnd);
        }

        if (queryDto?.ValidToStart.HasValue == true)
        {
            exp = exp.And(x => x.ValidTo >= queryDto.ValidToStart);
        }

        if (queryDto?.ValidToEnd.HasValue == true)
        {
            exp = exp.And(x => x.ValidTo <= queryDto.ValidToEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
