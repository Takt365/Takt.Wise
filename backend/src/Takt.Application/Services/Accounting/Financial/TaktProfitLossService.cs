// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktProfitLossService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：利润应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

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
/// 利润应用服务
/// </summary>
public class TaktProfitLossService : TaktServiceBase, ITaktProfitLossService
{
    private readonly ITaktCompanyRepository<TaktProfitLoss> _profitLossRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="profitLossRepository">利润仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktProfitLossService(
        ITaktCompanyRepository<TaktProfitLoss> profitLossRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _profitLossRepository = profitLossRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取利润列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktProfitLossDto>> GetProfitLossListAsync(TaktProfitLossQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _profitLossRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktProfitLossDto>.Create(
            data.Adapt<List<TaktProfitLossDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取利润
    /// </summary>
    /// <param name="id">利润ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitLossDto?> GetProfitLossByIdAsync(long id)
    {
        var entity = await _profitLossRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktProfitLossDto>();
    }

    /// <summary>
    /// 获取利润选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetProfitLossOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _profitLossRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ProfitLossStatus == 1,
            x => x.AccountTitleName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.AccountTitleName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建利润
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitLossDto> CreateProfitLossAsync(TaktProfitLossCreateDto dto)
    {
        var entity = dto.Adapt<TaktProfitLoss>();
        var isUnique_ix_takt_accounting_financial_profit_loss_unique = await _uniqueValidator.IsUniqueAsync(
            _profitLossRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PeriodCode == entity.PeriodCode
                && x.StatementLineCode == entity.StatementLineCode);
        if (!isUnique_ix_takt_accounting_financial_profit_loss_unique)
        {
            throw new TaktBusinessException("利润的PlantCode、PeriodCode、StatementLineCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _profitLossRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        ApplyProfitLossCasMeasurement(entity);
        entity = await _profitLossRepository.CreateAsync(entity);
        return await GetProfitLossByIdAsync(entity.Id) ?? entity.Adapt<TaktProfitLossDto>();
    }

    /// <summary>
    /// 更新利润
    /// </summary>
    /// <param name="id">利润ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitLossDto> UpdateProfitLossAsync(long id, TaktProfitLossUpdateDto dto)
    {
        var entity = await _profitLossRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("利润不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_accounting_financial_profit_loss_unique = await _uniqueValidator.IsUniqueAsync(
            _profitLossRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PeriodCode == entity.PeriodCode
                && x.StatementLineCode == entity.StatementLineCode,
            id);
        if (!isUnique_ix_takt_accounting_financial_profit_loss_unique)
        {
            throw new TaktBusinessException("利润的PlantCode、PeriodCode、StatementLineCode已存在");
        }
        ApplyProfitLossCasMeasurement(entity);
        await _profitLossRepository.UpdateAsync(entity);
        return await GetProfitLossByIdAsync(id) ?? throw new TaktBusinessException("利润不存在");
    }

    /// <summary>
    /// 删除利润
    /// </summary>
    /// <param name="id">利润ID</param>
    /// <returns>任务</returns>
    public async Task DeleteProfitLossByIdAsync(long id)
    {
        var deleted = await _profitLossRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("利润不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除利润
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteProfitLossBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteProfitLossByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新利润状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitLossDto> UpdateProfitLossStatusAsync(TaktProfitLossStatusDto dto)
    {
        var entity = await _profitLossRepository.GetByIdAsync(dto.ProfitLossId);
        if (entity == null)
        {
            throw new TaktBusinessException("利润不存在");
        }
        entity.ProfitLossStatus = dto.ProfitLossStatus;
        await _profitLossRepository.UpdateAsync(entity);
        return await GetProfitLossByIdAsync(dto.ProfitLossId) ?? throw new TaktBusinessException("利润不存在");
    }

    /// <summary>
    /// 更新利润排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktProfitLossDto> UpdateProfitLossSortAsync(TaktProfitLossSortDto dto)
    {
        var entity = await _profitLossRepository.GetByIdAsync(dto.ProfitLossId);
        if (entity == null)
        {
            throw new TaktBusinessException("利润不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _profitLossRepository.UpdateAsync(entity);
        return await GetProfitLossByIdAsync(dto.ProfitLossId) ?? throw new TaktBusinessException("利润不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetProfitLossTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktProfitLossTemplateDto>(
            sheetName ?? "利润导入模板",
            fileName ?? "利润导入模板.xlsx");
    }

    /// <summary>
    /// 导入利润
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportProfitLossAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktProfitLossImportDto>(fileStream, sheetName ?? "利润导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _profitLossRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktProfitLoss>();
                var importKey = $"{entity.PlantCode}|{entity.PeriodCode}|{entity.StatementLineCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PeriodCode、StatementLineCode）");
                }
                var isUnique_ix_takt_accounting_financial_profit_loss_unique = await _uniqueValidator.IsUniqueAsync(
                    _profitLossRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PeriodCode == entity.PeriodCode
                        && x.StatementLineCode == entity.StatementLineCode);
                if (!isUnique_ix_takt_accounting_financial_profit_loss_unique)
                {
                    throw new TaktBusinessException("利润的PlantCode、PeriodCode、StatementLineCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                ApplyProfitLossCasMeasurement(entity);
                await _profitLossRepository.CreateAsync(entity);
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
    /// 导出利润
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportProfitLossAsync(TaktProfitLossQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktProfitLossQueryDto());
        var list = await _profitLossRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktProfitLossExportDto>(),
                sheetName ?? "利润数据",
                fileName ?? "利润导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktProfitLossExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "利润数据",
            fileName ?? "利润导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建利润查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktProfitLoss, bool>> QueryExpression(TaktProfitLossQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktProfitLoss>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PeriodCode != null && x.PeriodCode.Contains(keywords))
                || (x.StatementLineCode != null && x.StatementLineCode.Contains(keywords))
                || (x.StatementLineName != null && x.StatementLineName.Contains(keywords))
                || (x.AccountTitleCode != null && x.AccountTitleCode.Contains(keywords))
                || (x.AccountTitleName != null && x.AccountTitleName.Contains(keywords))
                || SqlFunc.ToString(x.LineCategory).Contains(keywords)
                || SqlFunc.ToString(x.IsTotalLine).Contains(keywords)
                || SqlFunc.ToString(x.PeriodAmount).Contains(keywords)
                || SqlFunc.ToString(x.PriorPeriodAmount).Contains(keywords)
                || SqlFunc.ToString(x.YearToDateAmount).Contains(keywords)
                || SqlFunc.ToString(x.IsExpense).Contains(keywords)
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.ProfitLossStatus).Contains(keywords)
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

        if (!string.IsNullOrEmpty(queryDto?.StatementLineCode))
        {
            exp = exp.And(x => x.StatementLineCode != null && x.StatementLineCode.Contains(queryDto.StatementLineCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.StatementLineName))
        {
            exp = exp.And(x => x.StatementLineName != null && x.StatementLineName.Contains(queryDto.StatementLineName));
        }

        if (!string.IsNullOrEmpty(queryDto?.AccountTitleCode))
        {
            exp = exp.And(x => x.AccountTitleCode != null && x.AccountTitleCode.Contains(queryDto.AccountTitleCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.AccountTitleName))
        {
            exp = exp.And(x => x.AccountTitleName != null && x.AccountTitleName.Contains(queryDto.AccountTitleName));
        }

        if (queryDto?.LineCategory.HasValue == true)
        {
            exp = exp.And(x => x.LineCategory == queryDto.LineCategory);
        }

        if (queryDto?.IsTotalLine.HasValue == true)
        {
            exp = exp.And(x => x.IsTotalLine == queryDto.IsTotalLine);
        }

        if (queryDto?.PeriodAmount.HasValue == true)
        {
            exp = exp.And(x => x.PeriodAmount == queryDto.PeriodAmount);
        }

        if (queryDto?.PriorPeriodAmount.HasValue == true)
        {
            exp = exp.And(x => x.PriorPeriodAmount == queryDto.PriorPeriodAmount);
        }

        if (queryDto?.YearToDateAmount.HasValue == true)
        {
            exp = exp.And(x => x.YearToDateAmount == queryDto.YearToDateAmount);
        }

        if (queryDto?.IsExpense.HasValue == true)
        {
            exp = exp.And(x => x.IsExpense == queryDto.IsExpense);
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.ProfitLossStatus.HasValue == true)
        {
            exp = exp.And(x => x.ProfitLossStatus == queryDto.ProfitLossStatus);
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
    /// 按 CAS 利润表 / IAS 1（含 OCI）惯例：依行类别默认费用性质与合计行标记；本年累计为空时默认等于本期。
    /// </summary>
    /// <param name="entity">利润表行</param>
    private static void ApplyProfitLossCasMeasurement(TaktProfitLoss entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        // 2营业成本 3税金及附加 4期间费用 9所得税 → 费用减项
        if (entity.LineCategory is 2 or 3 or 4 or 9)
        {
            entity.IsExpense = 1;
        }
        // 6营业利润 8利润总额 10净利润 12综合收益总额 → 合计行
        if (entity.LineCategory is 6 or 8 or 10 or 12)
        {
            entity.IsTotalLine = 1;
        }
        if (entity.YearToDateAmount == 0 && entity.PeriodAmount != 0)
        {
            entity.YearToDateAmount = entity.PeriodAmount;
        }
    }
}
