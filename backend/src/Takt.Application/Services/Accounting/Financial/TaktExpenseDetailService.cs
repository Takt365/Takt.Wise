// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktExpenseDetailService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：费用单明细应用服务实现
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
/// 费用单明细应用服务
/// </summary>
public class TaktExpenseDetailService : TaktServiceBase, ITaktExpenseDetailService
{
    private readonly ITaktCompanyRepository<TaktExpenseDetail> _expenseDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="expenseDetailRepository">费用单明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktExpenseDetailService(
        ITaktCompanyRepository<TaktExpenseDetail> expenseDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _expenseDetailRepository = expenseDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取费用单明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktExpenseDetailDto>> GetExpenseDetailListAsync(TaktExpenseDetailQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktExpenseDetailDto>.Create(
                new List<TaktExpenseDetailDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _expenseDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktExpenseDetailDto>.Create(
            data.Adapt<List<TaktExpenseDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取费用单明细
    /// </summary>
    /// <param name="id">费用单明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktExpenseDetailDto?> GetExpenseDetailByIdAsync(long id)
    {
        var entity = await _expenseDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktExpenseDetailDto>();
    }

    /// <summary>
    /// 获取费用单明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetExpenseDetailOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _expenseDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.ItemName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ExpenseCode,
            DictLabel = e.ItemName ?? e.ExpenseCode,
        }).ToList();
    }

    /// <summary>
    /// 创建费用单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktExpenseDetailDto> CreateExpenseDetailAsync(TaktExpenseDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktExpenseDetail>();
        entity.IsObsolete = 0;
        var isUnique_ix_expense_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _expenseDetailRepository,
            x => x.ExpenseId == entity.ExpenseId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_expense_detail_line_unique)
        {
            throw new TaktBusinessException("费用单明细的ExpenseId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _expenseDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ExpenseId == entity.ExpenseId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.ExpenseCode) ? entity.ExpenseCode : entity.ExpenseId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _expenseDetailRepository.CreateAsync(entity);
        return await GetExpenseDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktExpenseDetailDto>();
    }

    /// <summary>
    /// 更新费用单明细
    /// </summary>
    /// <param name="id">费用单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktExpenseDetailDto> UpdateExpenseDetailAsync(long id, TaktExpenseDetailUpdateDto dto)
    {
        var entity = await _expenseDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("费用单明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_expense_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _expenseDetailRepository,
            x => x.ExpenseId == entity.ExpenseId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_expense_detail_line_unique)
        {
            throw new TaktBusinessException("费用单明细的ExpenseId、LineNumber已存在");
        }
        await _expenseDetailRepository.UpdateAsync(entity);
        return await GetExpenseDetailByIdAsync(id) ?? throw new TaktBusinessException("费用单明细不存在");
    }

    /// <summary>
    /// 删除费用单明细
    /// </summary>
    /// <param name="id">费用单明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteExpenseDetailByIdAsync(long id)
    {
        var entity = await _expenseDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("费用单明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("费用单明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("费用单明细已作废");
        }
        entity.IsObsolete = 1;
        await _expenseDetailRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除费用单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteExpenseDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteExpenseDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新费用单明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktExpenseDetailDto> UpdateExpenseDetailObsoleteAsync(TaktExpenseDetailObsoleteDto dto)
    {
        var entity = await _expenseDetailRepository.GetByIdAsync(dto.ExpenseDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("费用单明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("费用单明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _expenseDetailRepository.UpdateAsync(entity);
        return await GetExpenseDetailByIdAsync(dto.ExpenseDetailId) ?? throw new TaktBusinessException("费用单明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetExpenseDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktExpenseDetailTemplateDto>(
            sheetName ?? "费用单明细导入模板",
            fileName ?? "费用单明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入费用单明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportExpenseDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktExpenseDetailImportDto>(fileStream, sheetName ?? "费用单明细导入模板");
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
                var entity = rows[i].Adapt<TaktExpenseDetail>();
                var importKey = $"{entity.ExpenseId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ExpenseId、LineNumber）");
                }
                var isUnique_ix_expense_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _expenseDetailRepository,
                    x => x.ExpenseId == entity.ExpenseId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_expense_detail_line_unique)
                {
                    throw new TaktBusinessException("费用单明细的ExpenseId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _expenseDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ExpenseId == entity.ExpenseId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.ExpenseCode) ? entity.ExpenseCode : entity.ExpenseId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _expenseDetailRepository.CreateAsync(entity);
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
    /// 导出费用单明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportExpenseDetailAsync(TaktExpenseDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktExpenseDetailQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktExpenseDetailExportDto>(),
                sheetName ?? "费用单明细数据",
                fileName ?? "费用单明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _expenseDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktExpenseDetailExportDto>(),
                sheetName ?? "费用单明细数据",
                fileName ?? "费用单明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktExpenseDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "费用单明细数据",
            fileName ?? "费用单明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建费用单明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktExpenseDetail, bool>> QueryExpression(TaktExpenseDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktExpenseDetail>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ExpenseCode != null && x.ExpenseCode.Contains(keywords))
                || (x.AllocationCategory != null && x.AllocationCategory.Contains(keywords))
                || (x.ItemName != null && x.ItemName.Contains(keywords))
                || (x.ItemDescription != null && x.ItemDescription.Contains(keywords))
                || (x.AccountTitle != null && x.AccountTitle.Contains(keywords))
                || (x.InvoiceCode != null && x.InvoiceCode.Contains(keywords))
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

        if (queryDto?.ExpenseId.HasValue == true)
        {
            var expenseId = queryDto.ExpenseId.Value;
            exp = exp.And(x => x.ExpenseId == expenseId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExpenseCode))
        {
            var expenseCode = queryDto.ExpenseCode;
            exp = exp.And(x => x.ExpenseCode != null && x.ExpenseCode.Contains(expenseCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AllocationCategory))
        {
            var allocationCategory = queryDto.AllocationCategory;
            exp = exp.And(x => x.AllocationCategory != null && x.AllocationCategory.Contains(allocationCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ItemName))
        {
            var itemName = queryDto.ItemName;
            exp = exp.And(x => x.ItemName != null && x.ItemName.Contains(itemName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ItemDescription))
        {
            var itemDescription = queryDto.ItemDescription;
            exp = exp.And(x => x.ItemDescription != null && x.ItemDescription.Contains(itemDescription));
        }

        if (queryDto?.ItemQuantity.HasValue == true)
        {
            var itemQuantity = queryDto.ItemQuantity.Value;
            exp = exp.And(x => x.ItemQuantity == itemQuantity);
        }

        if (queryDto?.ItemAmount.HasValue == true)
        {
            var itemAmount = queryDto.ItemAmount.Value;
            exp = exp.And(x => x.ItemAmount == itemAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountTitle))
        {
            var accountTitle = queryDto.AccountTitle;
            exp = exp.And(x => x.AccountTitle != null && x.AccountTitle.Contains(accountTitle));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InvoiceCode))
        {
            var invoiceCode = queryDto.InvoiceCode;
            exp = exp.And(x => x.InvoiceCode != null && x.InvoiceCode.Contains(invoiceCode));
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

        if (queryDto?.ExpenseDetailDateStart.HasValue == true)
        {
            var expenseDetailDateStart = queryDto.ExpenseDetailDateStart.Value;
            exp = exp.And(x => x.ExpenseDetailDate >= expenseDetailDateStart);
        }

        if (queryDto?.ExpenseDetailDateEnd.HasValue == true)
        {
            var expenseDetailDateEnd = queryDto.ExpenseDetailDateEnd.Value;
            exp = exp.And(x => x.ExpenseDetailDate <= expenseDetailDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktExpenseDetailQueryDto? queryDto)
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
        if (queryDto.ExpenseId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExpenseCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AllocationCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ItemName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ItemDescription))
        {
            return true;
        }
        if (queryDto.ItemQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.ItemAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccountTitle))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InvoiceCode))
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
        if (queryDto.IsObsolete.HasValue)
        {
            return true;
        }
        if (queryDto.ExpenseDetailDateStart.HasValue || queryDto.ExpenseDetailDateEnd.HasValue)
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
