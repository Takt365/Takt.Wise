// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktBudgetActualService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：预算实绩应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Globalization;
using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Accounting.Financial;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Accounting.Financial;

/// <summary>
/// 预算实绩应用服务
/// </summary>
public class TaktBudgetActualService : TaktServiceBase, ITaktBudgetActualService
{
    private readonly ITaktCompanyRepository<TaktBudgetActual> _budgetActualRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="budgetActualRepository">预算实绩仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBudgetActualService(
        ITaktCompanyRepository<TaktBudgetActual> budgetActualRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _budgetActualRepository = budgetActualRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取预算实绩列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBudgetActualDto>> GetBudgetActualListAsync(TaktBudgetActualQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _budgetActualRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBudgetActualDto>.Create(
            data.Adapt<List<TaktBudgetActualDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取预算实绩
    /// </summary>
    /// <param name="id">预算实绩ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBudgetActualDto?> GetBudgetActualByIdAsync(long id)
    {
        var entity = await _budgetActualRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBudgetActualDto>();
    }

    /// <summary>
    /// 获取预算实绩选项列表（DictValue=Id，DictLabel=预算项名称，ExtValue=本期预算金额）
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBudgetActualOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _budgetActualRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BudgetActualStatus == 1,
            x => x.CostCenterName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = string.IsNullOrWhiteSpace(e.BudgetItemName) ? e.BudgetItemCode : e.BudgetItemName,
            ExtLabel = e.BudgetItemCode,
            ExtValue = e.BudgetAmount.ToString(CultureInfo.InvariantCulture),
        }).ToList();
    }

    /// <summary>
    /// 创建预算实绩
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBudgetActualDto> CreateBudgetActualAsync(TaktBudgetActualCreateDto dto)
    {
        var entity = dto.Adapt<TaktBudgetActual>();
        var isUnique_ix_takt_accounting_financial_budget_actual_unique = await _uniqueValidator.IsUniqueAsync(
            _budgetActualRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PeriodCode == entity.PeriodCode
                && x.CostCenterCode == entity.CostCenterCode
                && x.BudgetItemCode == entity.BudgetItemCode);
        if (!isUnique_ix_takt_accounting_financial_budget_actual_unique)
        {
            throw new TaktBusinessException("预算实绩的PlantCode、PeriodCode、CostCenterCode、BudgetItemCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _budgetActualRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        ApplyBudgetActualCasMeasurement(entity);
        entity = await _budgetActualRepository.CreateAsync(entity);
        return await GetBudgetActualByIdAsync(entity.Id) ?? entity.Adapt<TaktBudgetActualDto>();
    }

    /// <summary>
    /// 更新预算实绩
    /// </summary>
    /// <param name="id">预算实绩ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBudgetActualDto> UpdateBudgetActualAsync(long id, TaktBudgetActualUpdateDto dto)
    {
        var entity = await _budgetActualRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("预算实绩不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_accounting_financial_budget_actual_unique = await _uniqueValidator.IsUniqueAsync(
            _budgetActualRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PeriodCode == entity.PeriodCode
                && x.CostCenterCode == entity.CostCenterCode
                && x.BudgetItemCode == entity.BudgetItemCode,
            id);
        if (!isUnique_ix_takt_accounting_financial_budget_actual_unique)
        {
            throw new TaktBusinessException("预算实绩的PlantCode、PeriodCode、CostCenterCode、BudgetItemCode已存在");
        }
        ApplyBudgetActualCasMeasurement(entity);
        await _budgetActualRepository.UpdateAsync(entity);
        return await GetBudgetActualByIdAsync(id) ?? throw new TaktBusinessException("预算实绩不存在");
    }

    /// <summary>
    /// 删除预算实绩
    /// </summary>
    /// <param name="id">预算实绩ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBudgetActualByIdAsync(long id)
    {
        var deleted = await _budgetActualRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("预算实绩不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除预算实绩
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBudgetActualBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBudgetActualByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新预算实绩状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBudgetActualDto> UpdateBudgetActualStatusAsync(TaktBudgetActualStatusDto dto)
    {
        var entity = await _budgetActualRepository.GetByIdAsync(dto.BudgetActualId);
        if (entity == null)
        {
            throw new TaktBusinessException("预算实绩不存在");
        }
        entity.BudgetActualStatus = dto.BudgetActualStatus;
        await _budgetActualRepository.UpdateAsync(entity);
        return await GetBudgetActualByIdAsync(dto.BudgetActualId) ?? throw new TaktBusinessException("预算实绩不存在");
    }

    /// <summary>
    /// 更新预算实绩排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBudgetActualDto> UpdateBudgetActualSortAsync(TaktBudgetActualSortDto dto)
    {
        var entity = await _budgetActualRepository.GetByIdAsync(dto.BudgetActualId);
        if (entity == null)
        {
            throw new TaktBusinessException("预算实绩不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _budgetActualRepository.UpdateAsync(entity);
        return await GetBudgetActualByIdAsync(dto.BudgetActualId) ?? throw new TaktBusinessException("预算实绩不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBudgetActualTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBudgetActualTemplateDto>(
            sheetName ?? "预算实绩导入模板",
            fileName ?? "预算实绩导入模板.xlsx");
    }

    /// <summary>
    /// 导入预算实绩
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBudgetActualAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBudgetActualImportDto>(fileStream, sheetName ?? "预算实绩导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _budgetActualRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktBudgetActual>();
                var importKey = $"{entity.PlantCode}|{entity.PeriodCode}|{entity.CostCenterCode}|{entity.BudgetItemCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PeriodCode、CostCenterCode、BudgetItemCode）");
                }
                var isUnique_ix_takt_accounting_financial_budget_actual_unique = await _uniqueValidator.IsUniqueAsync(
                    _budgetActualRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PeriodCode == entity.PeriodCode
                        && x.CostCenterCode == entity.CostCenterCode
                        && x.BudgetItemCode == entity.BudgetItemCode);
                if (!isUnique_ix_takt_accounting_financial_budget_actual_unique)
                {
                    throw new TaktBusinessException("预算实绩的PlantCode、PeriodCode、CostCenterCode、BudgetItemCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                ApplyBudgetActualCasMeasurement(entity);
                await _budgetActualRepository.CreateAsync(entity);
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
    /// 导出预算实绩
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBudgetActualAsync(TaktBudgetActualQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktBudgetActualQueryDto());
        var list = await _budgetActualRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBudgetActualExportDto>(),
                sheetName ?? "预算实绩数据",
                fileName ?? "预算实绩导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBudgetActualExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "预算实绩数据",
            fileName ?? "预算实绩导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建预算实绩查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBudgetActual, bool>> QueryExpression(TaktBudgetActualQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBudgetActual>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PeriodCode != null && x.PeriodCode.Contains(keywords))
                || (x.CostCenterCode != null && x.CostCenterCode.Contains(keywords))
                || (x.CostCenterName != null && x.CostCenterName.Contains(keywords))
                || (x.BudgetItemCode != null && x.BudgetItemCode.Contains(keywords))
                || (x.BudgetItemName != null && x.BudgetItemName.Contains(keywords))
                || (x.AccountTitleCode != null && x.AccountTitleCode.Contains(keywords))
                || SqlFunc.ToString(x.BudgetType).Contains(keywords)
                || SqlFunc.ToString(x.MeasureType).Contains(keywords)
                || SqlFunc.ToString(x.BudgetAmount).Contains(keywords)
                || SqlFunc.ToString(x.ActualAmount).Contains(keywords)
                || SqlFunc.ToString(x.VarianceAmount).Contains(keywords)
                || SqlFunc.ToString(x.VariancePercent).Contains(keywords)
                || SqlFunc.ToString(x.PriorPeriodActual).Contains(keywords)
                || SqlFunc.ToString(x.YtdBudgetAmount).Contains(keywords)
                || SqlFunc.ToString(x.YtdActualAmount).Contains(keywords)
                || SqlFunc.ToString(x.YtdVarianceAmount).Contains(keywords)
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.BudgetActualStatus).Contains(keywords)
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

        if (!string.IsNullOrEmpty(queryDto?.PeriodCode))
        {
            exp = exp.And(x => x.PeriodCode != null && x.PeriodCode.Contains(queryDto.PeriodCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CostCenterCode))
        {
            exp = exp.And(x => x.CostCenterCode != null && x.CostCenterCode.Contains(queryDto.CostCenterCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.CostCenterName))
        {
            exp = exp.And(x => x.CostCenterName != null && x.CostCenterName.Contains(queryDto.CostCenterName));
        }

        if (!string.IsNullOrEmpty(queryDto?.BudgetItemCode))
        {
            exp = exp.And(x => x.BudgetItemCode != null && x.BudgetItemCode.Contains(queryDto.BudgetItemCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.BudgetItemName))
        {
            exp = exp.And(x => x.BudgetItemName != null && x.BudgetItemName.Contains(queryDto.BudgetItemName));
        }

        if (!string.IsNullOrEmpty(queryDto?.AccountTitleCode))
        {
            exp = exp.And(x => x.AccountTitleCode != null && x.AccountTitleCode.Contains(queryDto.AccountTitleCode));
        }

        if (queryDto?.BudgetType.HasValue == true)
        {
            exp = exp.And(x => x.BudgetType == queryDto.BudgetType);
        }

        if (queryDto?.MeasureType.HasValue == true)
        {
            exp = exp.And(x => x.MeasureType == queryDto.MeasureType);
        }

        if (queryDto?.BudgetAmount.HasValue == true)
        {
            exp = exp.And(x => x.BudgetAmount == queryDto.BudgetAmount);
        }

        if (queryDto?.ActualAmount.HasValue == true)
        {
            exp = exp.And(x => x.ActualAmount == queryDto.ActualAmount);
        }

        if (queryDto?.VarianceAmount.HasValue == true)
        {
            exp = exp.And(x => x.VarianceAmount == queryDto.VarianceAmount);
        }

        if (queryDto?.VariancePercent.HasValue == true)
        {
            exp = exp.And(x => x.VariancePercent == queryDto.VariancePercent);
        }

        if (queryDto?.PriorPeriodActual.HasValue == true)
        {
            exp = exp.And(x => x.PriorPeriodActual == queryDto.PriorPeriodActual);
        }

        if (queryDto?.YtdBudgetAmount.HasValue == true)
        {
            exp = exp.And(x => x.YtdBudgetAmount == queryDto.YtdBudgetAmount);
        }

        if (queryDto?.YtdActualAmount.HasValue == true)
        {
            exp = exp.And(x => x.YtdActualAmount == queryDto.YtdActualAmount);
        }

        if (queryDto?.YtdVarianceAmount.HasValue == true)
        {
            exp = exp.And(x => x.YtdVarianceAmount == queryDto.YtdVarianceAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.BudgetActualStatus.HasValue == true)
        {
            exp = exp.And(x => x.BudgetActualStatus == queryDto.BudgetActualStatus);
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

    /// <summary>
    /// 按管理会计惯例勾稽：本期/本年累计差异 = 实绩 − 预算；差异率 = 差异 / |预算|。
    /// </summary>
    /// <param name="entity">预算实绩行</param>
    private static void ApplyBudgetActualCasMeasurement(TaktBudgetActual entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        entity.VarianceAmount = Math.Round(entity.ActualAmount - entity.BudgetAmount, 2, MidpointRounding.AwayFromZero);
        entity.VariancePercent = entity.BudgetAmount == 0
            ? 0
            : Math.Round(entity.VarianceAmount / Math.Abs(entity.BudgetAmount), 6, MidpointRounding.AwayFromZero);
        entity.YtdVarianceAmount = Math.Round(
            entity.YtdActualAmount - entity.YtdBudgetAmount,
            2,
            MidpointRounding.AwayFromZero);
    }
}
