// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktExpenseService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：费用单应用服务实现
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
/// 费用单应用服务
/// </summary>
public class TaktExpenseService : TaktServiceBase, ITaktExpenseService
{
    private readonly ITaktApprovalRepository<TaktExpense> _expenseRepository;
    private readonly ITaktCompanyRepository<TaktExpenseDetail> _expenseDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="expenseRepository">费用单仓储</param>
    /// <param name="expenseDetailRepository">ExpenseDetail仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktExpenseService(
        ITaktApprovalRepository<TaktExpense> expenseRepository,
        ITaktCompanyRepository<TaktExpenseDetail> expenseDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _expenseRepository = expenseRepository;
        _expenseDetailRepository = expenseDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取费用单列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktExpenseDto>> GetExpenseListAsync(TaktExpenseQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktExpenseDto>.Create(
                new List<TaktExpenseDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _expenseRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktExpenseDto>.Create(
            data.Adapt<List<TaktExpenseDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取费用单
    /// </summary>
    /// <param name="id">费用单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktExpenseDto?> GetExpenseByIdAsync(long id)
    {
        var entity = await _expenseRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktExpenseDto>();
        await FillExpenseDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取费用单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetExpenseOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _expenseRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ExpenseStatus == 1,
            x => x.ExpenseCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ExpenseCode,
            DictLabel = e.ExpenseCode,
        }).ToList();
    }

    /// <summary>
    /// 创建费用单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktExpenseDto> CreateExpenseAsync(TaktExpenseCreateDto dto)
    {
        var entity = dto.Adapt<TaktExpense>();
        var isUnique_ix_expense_code_unique = await _uniqueValidator.IsUniqueAsync(
            _expenseRepository,
            x => x.ExpenseCode == entity.ExpenseCode);
        if (!isUnique_ix_expense_code_unique)
        {
            throw new TaktBusinessException("费用单的ExpenseCode已存在");
        }
        entity = await _expenseRepository.CreateAsync(entity);
                await SaveExpenseChildrenAsync(entity, dto);
        return await GetExpenseByIdAsync(entity.Id) ?? entity.Adapt<TaktExpenseDto>();
    }

    /// <summary>
    /// 更新费用单
    /// </summary>
    /// <param name="id">费用单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktExpenseDto> UpdateExpenseAsync(long id, TaktExpenseUpdateDto dto)
    {
        var entity = await _expenseRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("费用单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_expense_code_unique = await _uniqueValidator.IsUniqueAsync(
            _expenseRepository,
            x => x.ExpenseCode == entity.ExpenseCode,
            id);
        if (!isUnique_ix_expense_code_unique)
        {
            throw new TaktBusinessException("费用单的ExpenseCode已存在");
        }
        await _expenseRepository.UpdateAsync(entity);
                await SaveExpenseChildrenAsync(entity, dto);
        return await GetExpenseByIdAsync(id) ?? throw new TaktBusinessException("费用单不存在");
    }

    /// <summary>
    /// 删除费用单
    /// </summary>
    /// <param name="id">费用单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteExpenseByIdAsync(long id)
    {
        var entity = await _expenseRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("费用单不存在或已删除");
        }
        await _expenseDetailRepository.DeleteAsync(x => x.ExpenseId == entity.Id);
        var deleted = await _expenseRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("费用单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除费用单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteExpenseBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteExpenseByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新费用单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktExpenseDto> UpdateExpenseStatusAsync(TaktExpenseStatusDto dto)
    {
        var entity = await _expenseRepository.GetByIdAsync(dto.ExpenseId);
        if (entity == null)
        {
            throw new TaktBusinessException("费用单不存在");
        }
        entity.ExpenseStatus = dto.ExpenseStatus;
        await _expenseRepository.UpdateAsync(entity);
        return await GetExpenseByIdAsync(dto.ExpenseId) ?? throw new TaktBusinessException("费用单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetExpenseTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktExpenseTemplateDto>(
            sheetName ?? "费用单导入模板",
            fileName ?? "费用单导入模板.xlsx");
    }

    /// <summary>
    /// 导入费用单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportExpenseAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktExpenseImportDto>(fileStream, sheetName ?? "费用单导入模板");
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
                var entity = rows[i].Adapt<TaktExpense>();
                var importKey = $"{entity.ExpenseCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ExpenseCode）");
                }
                var isUnique_ix_expense_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _expenseRepository,
                    x => x.ExpenseCode == entity.ExpenseCode);
                if (!isUnique_ix_expense_code_unique)
                {
                    throw new TaktBusinessException("费用单的ExpenseCode已存在");
                }
                await _expenseRepository.CreateAsync(entity);
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
    /// 导出费用单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportExpenseAsync(TaktExpenseQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktExpenseQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktExpenseExportDto>(),
                sheetName ?? "费用单数据",
                fileName ?? "费用单导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _expenseRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktExpenseExportDto>(),
                sheetName ?? "费用单数据",
                fileName ?? "费用单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktExpenseExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "费用单数据",
            fileName ?? "费用单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废费用单明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="expenseId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkExpenseDetailsObsoleteAsync(long expenseId)
    {
        if (expenseId <= 0)
        {
            return;
        }
        var rows = await _expenseDetailRepository.GetListAsync(
            x => x.ExpenseId == expenseId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _expenseDetailRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充费用单详情（加载 OneToMany 子表：费用单明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillExpenseDetailsAsync(TaktExpenseDto dto, TaktExpense entity)
    {
        if (dto == null)
        {
            return;
        }
        // 费用单明细 → dto.ExpenseDetails（含作废行）
        var expensedetails = await _expenseDetailRepository.GetListAsync(x => x.ExpenseId == entity.Id);
        dto.ExpenseDetails = expensedetails.Adapt<List<TaktExpenseDetailDto>>();
    }

    /// <summary>
    /// 保存费用单子表级联（费用单明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveExpenseChildrenAsync(TaktExpense entity, TaktExpenseCreateDto dto)
    {
        // 费用单明细（ExpenseDetails）
        List<TaktExpenseDetailUpdateDto>? expenseDetailsForSave;
        if (dto is TaktExpenseUpdateDto updateDtoForExpenseDetails && updateDtoForExpenseDetails.ExpenseDetails != null)
        {
            expenseDetailsForSave = updateDtoForExpenseDetails.ExpenseDetails;
        }
        else if (dto.ExpenseDetails != null)
        {
            expenseDetailsForSave = dto.ExpenseDetails.Adapt<List<TaktExpenseDetailUpdateDto>>();
        }
        else
        {
            expenseDetailsForSave = null;
        }
        if (expenseDetailsForSave is not { Count: > 0 })
        {
            await MarkExpenseDetailsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _expenseDetailRepository.GetListAsync(x => x.ExpenseId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktExpenseDetail>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < expenseDetailsForSave.Count; i++)
            {
                var childDto = expenseDetailsForSave[i];
                childDto.ExpenseId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.ExpenseCode = entity.ExpenseCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("费用单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、ExpenseId、LineNumber）");
                }
                if (childDto.ExpenseDetailId > 0)
                {
                    if (!existingById.TryGetValue(childDto.ExpenseDetailId, out var target))
                    {
                        throw new TaktBusinessException("费用单明细不存在（ExpenseDetailId={childDto.ExpenseDetailId}）");
                    }
                    if (target.ExpenseId != entity.Id)
                    {
                        throw new TaktBusinessException("费用单明细不属于当前主表（ExpenseDetailId={childDto.ExpenseDetailId}）");
                    }
                    submittedIds.Add(childDto.ExpenseDetailId);
                    var isUniqueUpdate_ix_expense_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _expenseDetailRepository,
                        x => x.ExpenseId == x.ExpenseId
                && x.LineNumber == x.LineNumber,
                        childDto.ExpenseDetailId);
                    if (!isUniqueUpdate_ix_expense_detail_line_unique)
                    {
                        throw new TaktBusinessException("费用单明细的ExpenseId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.ExpenseDetailId;
                    target.ExpenseId = entity.Id;
                    target.IsObsolete = 0;
                    await _expenseDetailRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_expense_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _expenseDetailRepository,
                        x => x.ExpenseId == x.ExpenseId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_expense_detail_line_unique)
                    {
                        throw new TaktBusinessException("费用单明细的ExpenseId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktExpenseDetail>();
                    child.Id = 0;
                    child.ExpenseId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _expenseDetailRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.ExpenseCode) ? entity.ExpenseCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _expenseDetailRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建费用单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktExpense, bool>> QueryExpression(TaktExpenseQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktExpense>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ExpenseCode != null && x.ExpenseCode.Contains(keywords))
                || (x.ExpenseTitle != null && x.ExpenseTitle.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.SupplierName1 != null && x.SupplierName1.Contains(keywords))
                || (x.ApplicantName != null && x.ApplicantName.Contains(keywords))
                || (x.ApplicationDeptName != null && x.ApplicationDeptName.Contains(keywords))
                || (x.CostBearerDeptName != null && x.CostBearerDeptName.Contains(keywords))
                || (x.CostCenter != null && x.CostCenter.Contains(keywords))
                || (x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(keywords))
                || (x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(keywords))
                || (x.ApplicationReason != null && x.ApplicationReason.Contains(keywords))
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.AccessUrl != null && x.AccessUrl.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ExpenseCode))
        {
            var expenseCode = queryDto.ExpenseCode;
            exp = exp.And(x => x.ExpenseCode != null && x.ExpenseCode.Contains(expenseCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExpenseTitle))
        {
            var expenseTitle = queryDto.ExpenseTitle;
            exp = exp.And(x => x.ExpenseTitle != null && x.ExpenseTitle.Contains(expenseTitle));
        }

        if (queryDto?.ExpenseType.HasValue == true)
        {
            var expenseType = queryDto.ExpenseType.Value;
            exp = exp.And(x => x.ExpenseType == expenseType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierCode))
        {
            var supplierCode = queryDto.SupplierCode;
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(supplierCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierName1))
        {
            var supplierName1 = queryDto.SupplierName1;
            exp = exp.And(x => x.SupplierName1 != null && x.SupplierName1.Contains(supplierName1));
        }

        if (queryDto?.ApplicantBy.HasValue == true)
        {
            var applicantBy = queryDto.ApplicantBy.Value;
            exp = exp.And(x => x.ApplicantBy == applicantBy);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApplicantName))
        {
            var applicantName = queryDto.ApplicantName;
            exp = exp.And(x => x.ApplicantName != null && x.ApplicantName.Contains(applicantName));
        }

        if (queryDto?.ApplicationDeptId.HasValue == true)
        {
            var applicationDeptId = queryDto.ApplicationDeptId.Value;
            exp = exp.And(x => x.ApplicationDeptId == applicationDeptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApplicationDeptName))
        {
            var applicationDeptName = queryDto.ApplicationDeptName;
            exp = exp.And(x => x.ApplicationDeptName != null && x.ApplicationDeptName.Contains(applicationDeptName));
        }

        if (queryDto?.CostBearerDeptId.HasValue == true)
        {
            var costBearerDeptId = queryDto.CostBearerDeptId.Value;
            exp = exp.And(x => x.CostBearerDeptId == costBearerDeptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CostBearerDeptName))
        {
            var costBearerDeptName = queryDto.CostBearerDeptName;
            exp = exp.And(x => x.CostBearerDeptName != null && x.CostBearerDeptName.Contains(costBearerDeptName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CostCenter))
        {
            var costCenter = queryDto.CostCenter;
            exp = exp.And(x => x.CostCenter != null && x.CostCenter.Contains(costCenter));
        }

        if (queryDto?.CountersignId.HasValue == true)
        {
            var countersignId = queryDto.CountersignId.Value;
            exp = exp.And(x => x.CountersignId == countersignId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseOrderCode))
        {
            var purchaseOrderCode = queryDto.PurchaseOrderCode;
            exp = exp.And(x => x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(purchaseOrderCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseRequestCode))
        {
            var purchaseRequestCode = queryDto.PurchaseRequestCode;
            exp = exp.And(x => x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(purchaseRequestCode));
        }

        if (queryDto?.ExpenseAmount.HasValue == true)
        {
            var expenseAmount = queryDto.ExpenseAmount.Value;
            exp = exp.And(x => x.ExpenseAmount == expenseAmount);
        }

        if (queryDto?.TaxRate.HasValue == true)
        {
            var taxRate = queryDto.TaxRate.Value;
            exp = exp.And(x => x.TaxRate == taxRate);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            var taxAmount = queryDto.TaxAmount.Value;
            exp = exp.And(x => x.TaxAmount == taxAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApplicationReason))
        {
            var applicationReason = queryDto.ApplicationReason;
            exp = exp.And(x => x.ApplicationReason != null && x.ApplicationReason.Contains(applicationReason));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FileName))
        {
            var fileName = queryDto.FileName;
            exp = exp.And(x => x.FileName != null && x.FileName.Contains(fileName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccessUrl))
        {
            var accessUrl = queryDto.AccessUrl;
            exp = exp.And(x => x.AccessUrl != null && x.AccessUrl.Contains(accessUrl));
        }

        if (queryDto?.ExpenseStatus.HasValue == true)
        {
            var expenseStatus = queryDto.ExpenseStatus.Value;
            exp = exp.And(x => x.ExpenseStatus == expenseStatus);
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

        if (queryDto?.ExpenseDateStart.HasValue == true)
        {
            var expenseDateStart = queryDto.ExpenseDateStart.Value;
            exp = exp.And(x => x.ExpenseDate >= expenseDateStart);
        }

        if (queryDto?.ExpenseDateEnd.HasValue == true)
        {
            var expenseDateEnd = queryDto.ExpenseDateEnd.Value;
            exp = exp.And(x => x.ExpenseDate <= expenseDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktExpenseQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ExpenseCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExpenseTitle))
        {
            return true;
        }
        if (queryDto.ExpenseType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierName1))
        {
            return true;
        }
        if (queryDto.ApplicantBy.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApplicantName))
        {
            return true;
        }
        if (queryDto.ApplicationDeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApplicationDeptName))
        {
            return true;
        }
        if (queryDto.CostBearerDeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CostBearerDeptName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CostCenter))
        {
            return true;
        }
        if (queryDto.CountersignId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseOrderCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseRequestCode))
        {
            return true;
        }
        if (queryDto.ExpenseAmount.HasValue)
        {
            return true;
        }
        if (queryDto.TaxRate.HasValue)
        {
            return true;
        }
        if (queryDto.TaxAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApplicationReason))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FileName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccessUrl))
        {
            return true;
        }
        if (queryDto.ExpenseStatus.HasValue)
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
        if (queryDto.ExpenseDateStart.HasValue || queryDto.ExpenseDateEnd.HasValue)
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
