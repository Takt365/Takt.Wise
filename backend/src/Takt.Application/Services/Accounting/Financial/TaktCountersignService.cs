// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Accounting.Financial
// 文件名称：TaktCountersignService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：会签单应用服务实现
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
/// 会签单应用服务
/// </summary>
public class TaktCountersignService : TaktServiceBase, ITaktCountersignService
{
    private readonly ITaktApprovalRepository<TaktCountersign> _countersignRepository;
    private readonly ITaktCompanyRepository<TaktCountersignDetail> _countersignDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="countersignRepository">会签单仓储</param>
    /// <param name="countersignDetailRepository">CountersignDetail仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktCountersignService(
        ITaktApprovalRepository<TaktCountersign> countersignRepository,
        ITaktCompanyRepository<TaktCountersignDetail> countersignDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _countersignRepository = countersignRepository;
        _countersignDetailRepository = countersignDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取会签单列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktCountersignDto>> GetCountersignListAsync(TaktCountersignQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktCountersignDto>.Create(
                new List<TaktCountersignDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _countersignRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktCountersignDto>.Create(
            data.Adapt<List<TaktCountersignDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会签单
    /// </summary>
    /// <param name="id">会签单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktCountersignDto?> GetCountersignByIdAsync(long id)
    {
        var entity = await _countersignRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktCountersignDto>();
        await FillCountersignDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取会签单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetCountersignOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _countersignRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CountersignStatus == 1,
            x => x.CountersignCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CountersignCode,
            DictLabel = e.CountersignCode,
        }).ToList();
    }

    /// <summary>
    /// 创建会签单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCountersignDto> CreateCountersignAsync(TaktCountersignCreateDto dto)
    {
        var entity = dto.Adapt<TaktCountersign>();
        var isUnique_ix_countersign_code_unique = await _uniqueValidator.IsUniqueAsync(
            _countersignRepository,
            x => x.CountersignCode == entity.CountersignCode);
        if (!isUnique_ix_countersign_code_unique)
        {
            throw new TaktBusinessException("会签单的CountersignCode已存在");
        }
        entity = await _countersignRepository.CreateAsync(entity);
                await SaveCountersignChildrenAsync(entity, dto);
        return await GetCountersignByIdAsync(entity.Id) ?? entity.Adapt<TaktCountersignDto>();
    }

    /// <summary>
    /// 更新会签单
    /// </summary>
    /// <param name="id">会签单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCountersignDto> UpdateCountersignAsync(long id, TaktCountersignUpdateDto dto)
    {
        var entity = await _countersignRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会签单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_countersign_code_unique = await _uniqueValidator.IsUniqueAsync(
            _countersignRepository,
            x => x.CountersignCode == entity.CountersignCode,
            id);
        if (!isUnique_ix_countersign_code_unique)
        {
            throw new TaktBusinessException("会签单的CountersignCode已存在");
        }
        await _countersignRepository.UpdateAsync(entity);
                await SaveCountersignChildrenAsync(entity, dto);
        return await GetCountersignByIdAsync(id) ?? throw new TaktBusinessException("会签单不存在");
    }

    /// <summary>
    /// 删除会签单
    /// </summary>
    /// <param name="id">会签单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteCountersignByIdAsync(long id)
    {
        var entity = await _countersignRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会签单不存在或已删除");
        }
        await _countersignDetailRepository.DeleteAsync(x => x.CountersignId == entity.Id);
        var deleted = await _countersignRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("会签单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除会签单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteCountersignBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteCountersignByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新会签单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktCountersignDto> UpdateCountersignStatusAsync(TaktCountersignStatusDto dto)
    {
        var entity = await _countersignRepository.GetByIdAsync(dto.CountersignId);
        if (entity == null)
        {
            throw new TaktBusinessException("会签单不存在");
        }
        entity.CountersignStatus = dto.CountersignStatus;
        await _countersignRepository.UpdateAsync(entity);
        return await GetCountersignByIdAsync(dto.CountersignId) ?? throw new TaktBusinessException("会签单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetCountersignTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktCountersignTemplateDto>(
            sheetName ?? "会签单导入模板",
            fileName ?? "会签单导入模板.xlsx");
    }

    /// <summary>
    /// 导入会签单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportCountersignAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktCountersignImportDto>(fileStream, sheetName ?? "会签单导入模板");
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
                var entity = rows[i].Adapt<TaktCountersign>();
                var importKey = $"{entity.CountersignCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CountersignCode）");
                }
                var isUnique_ix_countersign_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _countersignRepository,
                    x => x.CountersignCode == entity.CountersignCode);
                if (!isUnique_ix_countersign_code_unique)
                {
                    throw new TaktBusinessException("会签单的CountersignCode已存在");
                }
                await _countersignRepository.CreateAsync(entity);
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
    /// 导出会签单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportCountersignAsync(TaktCountersignQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktCountersignQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCountersignExportDto>(),
                sheetName ?? "会签单数据",
                fileName ?? "会签单导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _countersignRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktCountersignExportDto>(),
                sheetName ?? "会签单数据",
                fileName ?? "会签单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktCountersignExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会签单数据",
            fileName ?? "会签单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废会签单明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="countersignId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkCountersignDetailsObsoleteAsync(long countersignId)
    {
        if (countersignId <= 0)
        {
            return;
        }
        var rows = await _countersignDetailRepository.GetListAsync(
            x => x.CountersignId == countersignId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _countersignDetailRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充会签单详情（加载 OneToMany 子表：会签单明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillCountersignDetailsAsync(TaktCountersignDto dto, TaktCountersign entity)
    {
        if (dto == null)
        {
            return;
        }
        // 会签单明细 → dto.CountersignDetails（含作废行）
        var countersigndetails = await _countersignDetailRepository.GetListAsync(x => x.CountersignId == entity.Id);
        dto.CountersignDetails = countersigndetails.Adapt<List<TaktCountersignDetailDto>>();
    }

    /// <summary>
    /// 保存会签单子表级联（会签单明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveCountersignChildrenAsync(TaktCountersign entity, TaktCountersignCreateDto dto)
    {
        // 会签单明细（CountersignDetails）
        List<TaktCountersignDetailUpdateDto>? countersignDetailsForSave;
        if (dto is TaktCountersignUpdateDto updateDtoForCountersignDetails && updateDtoForCountersignDetails.CountersignDetails != null)
        {
            countersignDetailsForSave = updateDtoForCountersignDetails.CountersignDetails;
        }
        else if (dto.CountersignDetails != null)
        {
            countersignDetailsForSave = dto.CountersignDetails.Adapt<List<TaktCountersignDetailUpdateDto>>();
        }
        else
        {
            countersignDetailsForSave = null;
        }
        if (countersignDetailsForSave is not { Count: > 0 })
        {
            await MarkCountersignDetailsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _countersignDetailRepository.GetListAsync(x => x.CountersignId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktCountersignDetail>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < countersignDetailsForSave.Count; i++)
            {
                var childDto = countersignDetailsForSave[i];
                childDto.CountersignId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.CountersignCode = entity.CountersignCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("会签单明细第{i + 1}项与本次提交的其他项重复（CompanyCode、CountersignId、LineNumber）");
                }
                if (childDto.CountersignDetailId > 0)
                {
                    if (!existingById.TryGetValue(childDto.CountersignDetailId, out var target))
                    {
                        throw new TaktBusinessException("会签单明细不存在（CountersignDetailId={childDto.CountersignDetailId}）");
                    }
                    if (target.CountersignId != entity.Id)
                    {
                        throw new TaktBusinessException("会签单明细不属于当前主表（CountersignDetailId={childDto.CountersignDetailId}）");
                    }
                    submittedIds.Add(childDto.CountersignDetailId);
                    var isUniqueUpdate_ix_countersign_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _countersignDetailRepository,
                        x => x.CountersignId == x.CountersignId
                && x.LineNumber == x.LineNumber,
                        childDto.CountersignDetailId);
                    if (!isUniqueUpdate_ix_countersign_detail_line_unique)
                    {
                        throw new TaktBusinessException("会签单明细的CountersignId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.CountersignDetailId;
                    target.CountersignId = entity.Id;
                    target.IsObsolete = 0;
                    await _countersignDetailRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_countersign_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _countersignDetailRepository,
                        x => x.CountersignId == x.CountersignId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_countersign_detail_line_unique)
                    {
                        throw new TaktBusinessException("会签单明细的CountersignId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktCountersignDetail>();
                    child.Id = 0;
                    child.CountersignId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _countersignDetailRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.CountersignCode) ? entity.CountersignCode : entity.Id.ToString();
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
                await _countersignDetailRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会签单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktCountersign, bool>> QueryExpression(TaktCountersignQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktCountersign>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.CountersignCode != null && x.CountersignCode.Contains(keywords))
                || (x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(keywords))
                || (x.BusinessType != null && x.BusinessType.Contains(keywords))
                || (x.BusinessKey != null && x.BusinessKey.Contains(keywords))
                || (x.CountersignDepts != null && x.CountersignDepts.Contains(keywords))
                || (x.FinanceDept != null && x.FinanceDept.Contains(keywords))
                || (x.BudgetReviewComment != null && x.BudgetReviewComment.Contains(keywords))
                || (x.ExecutiveOffice != null && x.ExecutiveOffice.Contains(keywords))
                || (x.ApplicantName != null && x.ApplicantName.Contains(keywords))
                || (x.ApplicationDeptName != null && x.ApplicationDeptName.Contains(keywords))
                || (x.CostBearerDeptName != null && x.CostBearerDeptName.Contains(keywords))
                || (x.BudgetItem != null && x.BudgetItem.Contains(keywords))
                || (x.CountersignTitle != null && x.CountersignTitle.Contains(keywords))
                || (x.ApplicationReason != null && x.ApplicationReason.Contains(keywords))
                || (x.BudgetUsageDescription != null && x.BudgetUsageDescription.Contains(keywords))
                || (x.TargetAndExpectedBenefit != null && x.TargetAndExpectedBenefit.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.CountersignCode))
        {
            var countersignCode = queryDto.CountersignCode;
            exp = exp.And(x => x.CountersignCode != null && x.CountersignCode.Contains(countersignCode));
        }

        if (queryDto?.PurchaseInquiryId.HasValue == true)
        {
            var purchaseInquiryId = queryDto.PurchaseInquiryId.Value;
            exp = exp.And(x => x.PurchaseInquiryId == purchaseInquiryId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseInquiryCode))
        {
            var purchaseInquiryCode = queryDto.PurchaseInquiryCode;
            exp = exp.And(x => x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(purchaseInquiryCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BusinessType))
        {
            var businessType = queryDto.BusinessType;
            exp = exp.And(x => x.BusinessType != null && x.BusinessType.Contains(businessType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BusinessKey))
        {
            var businessKey = queryDto.BusinessKey;
            exp = exp.And(x => x.BusinessKey != null && x.BusinessKey.Contains(businessKey));
        }

        if (queryDto?.StepNo.HasValue == true)
        {
            var stepNo = queryDto.StepNo.Value;
            exp = exp.And(x => x.StepNo == stepNo);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CountersignDepts))
        {
            var countersignDepts = queryDto.CountersignDepts;
            exp = exp.And(x => x.CountersignDepts != null && x.CountersignDepts.Contains(countersignDepts));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FinanceDept))
        {
            var financeDept = queryDto.FinanceDept;
            exp = exp.And(x => x.FinanceDept != null && x.FinanceDept.Contains(financeDept));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BudgetReviewComment))
        {
            var budgetReviewComment = queryDto.BudgetReviewComment;
            exp = exp.And(x => x.BudgetReviewComment != null && x.BudgetReviewComment.Contains(budgetReviewComment));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExecutiveOffice))
        {
            var executiveOffice = queryDto.ExecutiveOffice;
            exp = exp.And(x => x.ExecutiveOffice != null && x.ExecutiveOffice.Contains(executiveOffice));
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

        if (queryDto?.IsBudget.HasValue == true)
        {
            var isBudget = queryDto.IsBudget.Value;
            exp = exp.And(x => x.IsBudget == isBudget);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BudgetItem))
        {
            var budgetItem = queryDto.BudgetItem;
            exp = exp.And(x => x.BudgetItem != null && x.BudgetItem.Contains(budgetItem));
        }

        if (queryDto?.BudgetItemId.HasValue == true)
        {
            var budgetItemId = queryDto.BudgetItemId.Value;
            exp = exp.And(x => x.BudgetItemId == budgetItemId);
        }

        if (queryDto?.BudgetAmount.HasValue == true)
        {
            var budgetAmount = queryDto.BudgetAmount.Value;
            exp = exp.And(x => x.BudgetAmount == budgetAmount);
        }

        if (queryDto?.ApplicationAmount.HasValue == true)
        {
            var applicationAmount = queryDto.ApplicationAmount.Value;
            exp = exp.And(x => x.ApplicationAmount == applicationAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CountersignTitle))
        {
            var countersignTitle = queryDto.CountersignTitle;
            exp = exp.And(x => x.CountersignTitle != null && x.CountersignTitle.Contains(countersignTitle));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApplicationReason))
        {
            var applicationReason = queryDto.ApplicationReason;
            exp = exp.And(x => x.ApplicationReason != null && x.ApplicationReason.Contains(applicationReason));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BudgetUsageDescription))
        {
            var budgetUsageDescription = queryDto.BudgetUsageDescription;
            exp = exp.And(x => x.BudgetUsageDescription != null && x.BudgetUsageDescription.Contains(budgetUsageDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TargetAndExpectedBenefit))
        {
            var targetAndExpectedBenefit = queryDto.TargetAndExpectedBenefit;
            exp = exp.And(x => x.TargetAndExpectedBenefit != null && x.TargetAndExpectedBenefit.Contains(targetAndExpectedBenefit));
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

        if (queryDto?.CountersignStatus.HasValue == true)
        {
            var countersignStatus = queryDto.CountersignStatus.Value;
            exp = exp.And(x => x.CountersignStatus == countersignStatus);
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
    private static bool HasAnyListQueryFilter(TaktCountersignQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.CountersignCode))
        {
            return true;
        }
        if (queryDto.PurchaseInquiryId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseInquiryCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BusinessType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BusinessKey))
        {
            return true;
        }
        if (queryDto.StepNo.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CountersignDepts))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FinanceDept))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BudgetReviewComment))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExecutiveOffice))
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
        if (queryDto.IsBudget.HasValue)
        {
            return true;
        }
        if (queryDto.BudgetItemId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BudgetItem))
        {
            return true;
        }
        if (queryDto.BudgetAmount.HasValue)
        {
            return true;
        }
        if (queryDto.ApplicationAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CountersignTitle))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApplicationReason))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BudgetUsageDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TargetAndExpectedBenefit))
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
        if (queryDto.CountersignStatus.HasValue)
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
