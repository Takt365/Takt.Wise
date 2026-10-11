// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktBalanceSheetService.cs
// 创建时间：2026-07-18
// 创建人：Takt365(Cursor AI)
// 功能描述：资产负债应用服务实现
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
/// 资产负债应用服务
/// </summary>
public class TaktBalanceSheetService : TaktServiceBase, ITaktBalanceSheetService
{
    private readonly ITaktCompanyRepository<TaktBalanceSheet> _balanceSheetRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="balanceSheetRepository">资产负债仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktBalanceSheetService(
        ITaktCompanyRepository<TaktBalanceSheet> balanceSheetRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _balanceSheetRepository = balanceSheetRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取资产负债列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktBalanceSheetDto>> GetBalanceSheetListAsync(TaktBalanceSheetQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _balanceSheetRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktBalanceSheetDto>.Create(
            data.Adapt<List<TaktBalanceSheetDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取资产负债
    /// </summary>
    /// <param name="id">资产负债ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktBalanceSheetDto?> GetBalanceSheetByIdAsync(long id)
    {
        var entity = await _balanceSheetRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktBalanceSheetDto>();
    }

    /// <summary>
    /// 获取资产负债选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetBalanceSheetOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _balanceSheetRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.BalanceSheetStatus == 1,
            x => x.AccountTitleName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.AccountTitleName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建资产负债
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBalanceSheetDto> CreateBalanceSheetAsync(TaktBalanceSheetCreateDto dto)
    {
        var entity = dto.Adapt<TaktBalanceSheet>();
        var isUnique_ix_takt_accounting_financial_balance_sheet_unique = await _uniqueValidator.IsUniqueAsync(
            _balanceSheetRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PeriodCode == entity.PeriodCode
                && x.StatementLineCode == entity.StatementLineCode);
        if (!isUnique_ix_takt_accounting_financial_balance_sheet_unique)
        {
            throw new TaktBusinessException("资产负债的PlantCode、PeriodCode、StatementLineCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _balanceSheetRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNext(maxSort);
        }
        ApplyBalanceSheetCasMeasurement(entity);
        entity = await _balanceSheetRepository.CreateAsync(entity);
        return await GetBalanceSheetByIdAsync(entity.Id) ?? entity.Adapt<TaktBalanceSheetDto>();
    }

    /// <summary>
    /// 更新资产负债
    /// </summary>
    /// <param name="id">资产负债ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBalanceSheetDto> UpdateBalanceSheetAsync(long id, TaktBalanceSheetUpdateDto dto)
    {
        var entity = await _balanceSheetRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("资产负债不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_accounting_financial_balance_sheet_unique = await _uniqueValidator.IsUniqueAsync(
            _balanceSheetRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PeriodCode == entity.PeriodCode
                && x.StatementLineCode == entity.StatementLineCode,
            id);
        if (!isUnique_ix_takt_accounting_financial_balance_sheet_unique)
        {
            throw new TaktBusinessException("资产负债的PlantCode、PeriodCode、StatementLineCode已存在");
        }
        ApplyBalanceSheetCasMeasurement(entity);
        await _balanceSheetRepository.UpdateAsync(entity);
        return await GetBalanceSheetByIdAsync(id) ?? throw new TaktBusinessException("资产负债不存在");
    }

    /// <summary>
    /// 删除资产负债
    /// </summary>
    /// <param name="id">资产负债ID</param>
    /// <returns>任务</returns>
    public async Task DeleteBalanceSheetByIdAsync(long id)
    {
        var deleted = await _balanceSheetRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("资产负债不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除资产负债
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteBalanceSheetBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteBalanceSheetByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新资产负债状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBalanceSheetDto> UpdateBalanceSheetStatusAsync(TaktBalanceSheetStatusDto dto)
    {
        var entity = await _balanceSheetRepository.GetByIdAsync(dto.BalanceSheetId);
        if (entity == null)
        {
            throw new TaktBusinessException("资产负债不存在");
        }
        entity.BalanceSheetStatus = dto.BalanceSheetStatus;
        await _balanceSheetRepository.UpdateAsync(entity);
        return await GetBalanceSheetByIdAsync(dto.BalanceSheetId) ?? throw new TaktBusinessException("资产负债不存在");
    }

    /// <summary>
    /// 更新资产负债排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktBalanceSheetDto> UpdateBalanceSheetSortAsync(TaktBalanceSheetSortDto dto)
    {
        var entity = await _balanceSheetRepository.GetByIdAsync(dto.BalanceSheetId);
        if (entity == null)
        {
            throw new TaktBusinessException("资产负债不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _balanceSheetRepository.UpdateAsync(entity);
        return await GetBalanceSheetByIdAsync(dto.BalanceSheetId) ?? throw new TaktBusinessException("资产负债不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetBalanceSheetTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktBalanceSheetTemplateDto>(
            sheetName ?? "资产负债导入模板",
            fileName ?? "资产负债导入模板.xlsx");
    }

    /// <summary>
    /// 导入资产负债
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportBalanceSheetAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktBalanceSheetImportDto>(fileStream, sheetName ?? "资产负债导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<string>(StringComparer.Ordinal);
        var importSortMax = await _balanceSheetRepository.GetMaxIntAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SortOrder);
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktBalanceSheet>();
                var importKey = $"{entity.PlantCode}|{entity.PeriodCode}|{entity.StatementLineCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PeriodCode、StatementLineCode）");
                }
                var isUnique_ix_takt_accounting_financial_balance_sheet_unique = await _uniqueValidator.IsUniqueAsync(
                    _balanceSheetRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PeriodCode == entity.PeriodCode
                        && x.StatementLineCode == entity.StatementLineCode);
                if (!isUnique_ix_takt_accounting_financial_balance_sheet_unique)
                {
                    throw new TaktBusinessException("资产负债的PlantCode、PeriodCode、StatementLineCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    entity.SortOrder = _sortOrderGenerator.GenerateNext(importSortMax);
                    importSortMax = entity.SortOrder;
                }
                ApplyBalanceSheetCasMeasurement(entity);
                await _balanceSheetRepository.CreateAsync(entity);
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
    /// 导出资产负债
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportBalanceSheetAsync(TaktBalanceSheetQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktBalanceSheetQueryDto());
        var list = await _balanceSheetRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktBalanceSheetExportDto>(),
                sheetName ?? "资产负债数据",
                fileName ?? "资产负债导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktBalanceSheetExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "资产负债数据",
            fileName ?? "资产负债导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建资产负债查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktBalanceSheet, bool>> QueryExpression(TaktBalanceSheetQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktBalanceSheet>();

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
                || SqlFunc.ToString(x.BalanceDirection).Contains(keywords)
                || SqlFunc.ToString(x.IsTotalLine).Contains(keywords)
                || SqlFunc.ToString(x.OpeningBalance).Contains(keywords)
                || SqlFunc.ToString(x.DebitAmount).Contains(keywords)
                || SqlFunc.ToString(x.CreditAmount).Contains(keywords)
                || SqlFunc.ToString(x.ClosingBalance).Contains(keywords)
                || SqlFunc.ToString(x.PresentationAmount).Contains(keywords)
                || SqlFunc.ToString(x.PriorPeriodAmount).Contains(keywords)
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || SqlFunc.ToString(x.BalanceSheetStatus).Contains(keywords)
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

        if (queryDto?.BalanceDirection.HasValue == true)
        {
            exp = exp.And(x => x.BalanceDirection == queryDto.BalanceDirection);
        }

        if (queryDto?.IsTotalLine.HasValue == true)
        {
            exp = exp.And(x => x.IsTotalLine == queryDto.IsTotalLine);
        }

        if (queryDto?.OpeningBalance.HasValue == true)
        {
            exp = exp.And(x => x.OpeningBalance == queryDto.OpeningBalance);
        }

        if (queryDto?.DebitAmount.HasValue == true)
        {
            exp = exp.And(x => x.DebitAmount == queryDto.DebitAmount);
        }

        if (queryDto?.CreditAmount.HasValue == true)
        {
            exp = exp.And(x => x.CreditAmount == queryDto.CreditAmount);
        }

        if (queryDto?.ClosingBalance.HasValue == true)
        {
            exp = exp.And(x => x.ClosingBalance == queryDto.ClosingBalance);
        }

        if (queryDto?.PresentationAmount.HasValue == true)
        {
            exp = exp.And(x => x.PresentationAmount == queryDto.PresentationAmount);
        }

        if (queryDto?.PriorPeriodAmount.HasValue == true)
        {
            exp = exp.And(x => x.PriorPeriodAmount == queryDto.PriorPeriodAmount);
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrencyCode))
        {
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(queryDto.CurrencyCode));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
        }

        if (queryDto?.BalanceSheetStatus.HasValue == true)
        {
            exp = exp.And(x => x.BalanceSheetStatus == queryDto.BalanceSheetStatus);
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
    /// 按 CAS/IAS 1 勾稽：依余额方向计算期末余额，并回填期末列报金额。
    /// 借方余额科目：期末=期初+借方−贷方；贷方余额科目：期末=期初+贷方−借方。
    /// </summary>
    /// <param name="entity">资产负债表行</param>
    private static void ApplyBalanceSheetCasMeasurement(TaktBalanceSheet entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var computedClosing = entity.BalanceDirection == 1
            ? entity.OpeningBalance + entity.CreditAmount - entity.DebitAmount
            : entity.OpeningBalance + entity.DebitAmount - entity.CreditAmount;
        computedClosing = Math.Round(computedClosing, 2, MidpointRounding.AwayFromZero);
        if (entity.ClosingBalance == 0 && (entity.OpeningBalance != 0 || entity.DebitAmount != 0 || entity.CreditAmount != 0))
        {
            entity.ClosingBalance = computedClosing;
        }
        else if (Math.Abs(entity.ClosingBalance - computedClosing) > 0.01m
            && (entity.DebitAmount != 0 || entity.CreditAmount != 0 || entity.OpeningBalance != 0))
        {
            throw new TaktBusinessException(
                $"期末余额与借贷发生额勾稽不符（应计 {computedClosing}，实际 {entity.ClosingBalance}）");
        }
        if (entity.PresentationAmount == 0)
        {
            entity.PresentationAmount = entity.ClosingBalance;
        }
    }
}
