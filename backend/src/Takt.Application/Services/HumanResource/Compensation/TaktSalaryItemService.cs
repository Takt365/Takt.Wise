// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.HumanResource.Compensation
// 文件名称：TaktSalaryItemService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：薪资项目应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.HumanResource.Compensation;
using Takt.Domain.Entities.HumanResource.Compensation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.HumanResource.Compensation;

/// <summary>
/// 薪资项目应用服务
/// </summary>
public class TaktSalaryItemService : TaktServiceBase, ITaktSalaryItemService
{
    private readonly ITaktCompanyRepository<TaktSalaryItem> _salaryItemRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salaryItemRepository">薪资项目仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalaryItemService(
        ITaktCompanyRepository<TaktSalaryItem> salaryItemRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salaryItemRepository = salaryItemRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取薪资项目列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalaryItemDto>> GetSalaryItemListAsync(TaktSalaryItemQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salaryItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalaryItemDto>.Create(
            data.Adapt<List<TaktSalaryItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取薪资项目
    /// </summary>
    /// <param name="id">薪资项目ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryItemDto?> GetSalaryItemByIdAsync(long id)
    {
        var entity = await _salaryItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalaryItemDto>();
    }

    /// <summary>
    /// 获取薪资项目选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalaryItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salaryItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ItemStatus == 1,
            x => x.ItemName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ItemName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建薪资项目
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryItemDto> CreateSalaryItemAsync(TaktSalaryItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalaryItem>();
        var isUnique_ix_salary_item_code_unique = await _uniqueValidator.IsUniqueAsync(
            _salaryItemRepository,
            x => x.ItemCode == entity.ItemCode);
        if (!isUnique_ix_salary_item_code_unique)
        {
            throw new TaktBusinessException("薪资项目的ItemCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _salaryItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalaryFormulaId == entity.SalaryFormulaId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.SalaryFormulaId.GetValueOrDefault(), maxSort);
        }
        entity = await _salaryItemRepository.CreateAsync(entity);
        return await GetSalaryItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSalaryItemDto>();
    }

    /// <summary>
    /// 更新薪资项目
    /// </summary>
    /// <param name="id">薪资项目ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryItemDto> UpdateSalaryItemAsync(long id, TaktSalaryItemUpdateDto dto)
    {
        var entity = await _salaryItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("薪资项目不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_salary_item_code_unique = await _uniqueValidator.IsUniqueAsync(
            _salaryItemRepository,
            x => x.ItemCode == entity.ItemCode,
            id);
        if (!isUnique_ix_salary_item_code_unique)
        {
            throw new TaktBusinessException("薪资项目的ItemCode已存在");
        }
        await _salaryItemRepository.UpdateAsync(entity);
        return await GetSalaryItemByIdAsync(id) ?? throw new TaktBusinessException("薪资项目不存在");
    }

    /// <summary>
    /// 删除薪资项目
    /// </summary>
    /// <param name="id">薪资项目ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalaryItemByIdAsync(long id)
    {
        var deleted = await _salaryItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("薪资项目不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除薪资项目
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalaryItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalaryItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新薪资项目状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryItemDto> UpdateSalaryItemStatusAsync(TaktSalaryItemStatusDto dto)
    {
        var entity = await _salaryItemRepository.GetByIdAsync(dto.SalaryItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("薪资项目不存在");
        }
        entity.ItemStatus = dto.ItemStatus;
        await _salaryItemRepository.UpdateAsync(entity);
        return await GetSalaryItemByIdAsync(dto.SalaryItemId) ?? throw new TaktBusinessException("薪资项目不存在");
    }

    /// <summary>
    /// 更新薪资项目排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalaryItemDto> UpdateSalaryItemSortAsync(TaktSalaryItemSortDto dto)
    {
        var entity = await _salaryItemRepository.GetByIdAsync(dto.SalaryItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("薪资项目不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _salaryItemRepository.UpdateAsync(entity);
        return await GetSalaryItemByIdAsync(dto.SalaryItemId) ?? throw new TaktBusinessException("薪资项目不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalaryItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalaryItemTemplateDto>(
            sheetName ?? "薪资项目导入模板",
            fileName ?? "薪资项目导入模板.xlsx");
    }

    /// <summary>
    /// 导入薪资项目
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalaryItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalaryItemImportDto>(fileStream, sheetName ?? "薪资项目导入模板");
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
                var entity = rows[i].Adapt<TaktSalaryItem>();
                var importKey = $"{entity.ItemCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ItemCode）");
                }
                var isUnique_ix_salary_item_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _salaryItemRepository,
                    x => x.ItemCode == entity.ItemCode);
                if (!isUnique_ix_salary_item_code_unique)
                {
                    throw new TaktBusinessException("薪资项目的ItemCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _salaryItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalaryFormulaId == entity.SalaryFormulaId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.SalaryFormulaId.GetValueOrDefault(), maxSort);
                }
                await _salaryItemRepository.CreateAsync(entity);
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
    /// 导出薪资项目
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalaryItemAsync(TaktSalaryItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSalaryItemQueryDto());
        var list = await _salaryItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalaryItemExportDto>(),
                sheetName ?? "薪资项目数据",
                fileName ?? "薪资项目导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalaryItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "薪资项目数据",
            fileName ?? "薪资项目导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建薪资项目查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalaryItem, bool>> QueryExpression(TaktSalaryItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalaryItem>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ItemCode != null && x.ItemCode.Contains(keywords))
                || (x.ItemName != null && x.ItemName.Contains(keywords))
                || (x.ShortName != null && x.ShortName.Contains(keywords))
                || SqlFunc.ToString(x.ItemType).Contains(keywords)
                || SqlFunc.ToString(x.CalcMethod).Contains(keywords)
                || SqlFunc.ToString(x.SalaryFormulaId).Contains(keywords)
                || SqlFunc.ToString(x.DefaultAmount).Contains(keywords)
                || SqlFunc.ToString(x.DefaultRate).Contains(keywords)
                || SqlFunc.ToString(x.StrikePrice).Contains(keywords)
                || SqlFunc.ToString(x.VestingYears).Contains(keywords)
                || SqlFunc.ToString(x.IsDeduction).Contains(keywords)
                || SqlFunc.ToString(x.IsTaxable).Contains(keywords)
                || SqlFunc.ToString(x.IncludeSocialSecurityBase).Contains(keywords)
                || SqlFunc.ToString(x.IncludeHousingFundBase).Contains(keywords)
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.ItemStatus).Contains(keywords)
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.ItemCode))
        {
            exp = exp.And(x => x.ItemCode != null && x.ItemCode.Contains(queryDto.ItemCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ItemName))
        {
            exp = exp.And(x => x.ItemName != null && x.ItemName.Contains(queryDto.ItemName));
        }

        if (!string.IsNullOrEmpty(queryDto?.ShortName))
        {
            exp = exp.And(x => x.ShortName != null && x.ShortName.Contains(queryDto.ShortName));
        }

        if (queryDto?.ItemType.HasValue == true)
        {
            exp = exp.And(x => x.ItemType == queryDto.ItemType);
        }

        if (queryDto?.CalcMethod.HasValue == true)
        {
            exp = exp.And(x => x.CalcMethod == queryDto.CalcMethod);
        }

        if (queryDto?.SalaryFormulaId.HasValue == true)
        {
            exp = exp.And(x => x.SalaryFormulaId == queryDto.SalaryFormulaId);
        }

        if (queryDto?.DefaultAmount.HasValue == true)
        {
            exp = exp.And(x => x.DefaultAmount == queryDto.DefaultAmount);
        }

        if (queryDto?.DefaultRate.HasValue == true)
        {
            exp = exp.And(x => x.DefaultRate == queryDto.DefaultRate);
        }

        if (queryDto?.StrikePrice.HasValue == true)
        {
            exp = exp.And(x => x.StrikePrice == queryDto.StrikePrice);
        }

        if (queryDto?.VestingYears.HasValue == true)
        {
            exp = exp.And(x => x.VestingYears == queryDto.VestingYears);
        }

        if (queryDto?.IsDeduction.HasValue == true)
        {
            exp = exp.And(x => x.IsDeduction == queryDto.IsDeduction);
        }

        if (queryDto?.IsTaxable.HasValue == true)
        {
            exp = exp.And(x => x.IsTaxable == queryDto.IsTaxable);
        }

        if (queryDto?.IncludeSocialSecurityBase.HasValue == true)
        {
            exp = exp.And(x => x.IncludeSocialSecurityBase == queryDto.IncludeSocialSecurityBase);
        }

        if (queryDto?.IncludeHousingFundBase.HasValue == true)
        {
            exp = exp.And(x => x.IncludeHousingFundBase == queryDto.IncludeHousingFundBase);
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.ItemStatus.HasValue == true)
        {
            exp = exp.And(x => x.ItemStatus == queryDto.ItemStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
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
