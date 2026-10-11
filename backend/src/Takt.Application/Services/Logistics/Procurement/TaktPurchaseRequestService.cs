// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseRequestService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：采购申请应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Procurement;
using Takt.Domain.Entities.Logistics.Procurement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Procurement;

/// <summary>
/// 采购申请应用服务
/// </summary>
public class TaktPurchaseRequestService : TaktServiceBase, ITaktPurchaseRequestService
{
    private readonly ITaktApprovalRepository<TaktPurchaseRequest> _purchaseRequestRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseRequestItem> _purchaseRequestItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseRequestRepository">采购申请仓储</param>
    /// <param name="purchaseRequestItemRepository">PurchaseRequestItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseRequestService(
        ITaktApprovalRepository<TaktPurchaseRequest> purchaseRequestRepository,
        ITaktCompanyRepository<TaktPurchaseRequestItem> purchaseRequestItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseRequestRepository = purchaseRequestRepository;
        _purchaseRequestItemRepository = purchaseRequestItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购申请列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseRequestDto>> GetPurchaseRequestListAsync(TaktPurchaseRequestQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseRequestDto>.Create(
                new List<TaktPurchaseRequestDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseRequestRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseRequestDto>.Create(
            data.Adapt<List<TaktPurchaseRequestDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购申请
    /// </summary>
    /// <param name="id">采购申请ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestDto?> GetPurchaseRequestByIdAsync(long id)
    {
        var entity = await _purchaseRequestRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchaseRequestDto>();
        await FillPurchaseRequestDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购申请选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseRequestOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseRequestRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RequestStatus == 1,
            x => x.PlantCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseRequestCode,
            DictLabel = string.IsNullOrWhiteSpace(e.PlantCode) ? e.PurchaseRequestCode : $"{e.PurchaseRequestCode} {e.PlantCode}",
            ExtValue = e.Id,
        }).ToList();
    }

    /// <summary>
    /// 创建采购申请
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestDto> CreatePurchaseRequestAsync(TaktPurchaseRequestCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseRequest>();
        var isUnique_ix_takt_logistics_procurement_purchase_request_pr_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseRequestRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseRequestCode == entity.PurchaseRequestCode
                && x.RequestDate == entity.RequestDate);
        if (!isUnique_ix_takt_logistics_procurement_purchase_request_pr_unique)
        {
            throw new TaktBusinessException("采购申请的PlantCode、PurchaseRequestCode、RequestDate已存在");
        }
        entity = await _purchaseRequestRepository.CreateAsync(entity);
                await SavePurchaseRequestChildrenAsync(entity, dto);
        return await GetPurchaseRequestByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseRequestDto>();
    }

    /// <summary>
    /// 更新采购申请
    /// </summary>
    /// <param name="id">采购申请ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestDto> UpdatePurchaseRequestAsync(long id, TaktPurchaseRequestUpdateDto dto)
    {
        var entity = await _purchaseRequestRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购申请不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_request_pr_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseRequestRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseRequestCode == entity.PurchaseRequestCode
                && x.RequestDate == entity.RequestDate,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_request_pr_unique)
        {
            throw new TaktBusinessException("采购申请的PlantCode、PurchaseRequestCode、RequestDate已存在");
        }
        await _purchaseRequestRepository.UpdateAsync(entity);
                await SavePurchaseRequestChildrenAsync(entity, dto);
        return await GetPurchaseRequestByIdAsync(id) ?? throw new TaktBusinessException("采购申请不存在");
    }

    /// <summary>
    /// 删除采购申请
    /// </summary>
    /// <param name="id">采购申请ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseRequestByIdAsync(long id)
    {
        var entity = await _purchaseRequestRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购申请不存在或已删除");
        }
        await _purchaseRequestItemRepository.DeleteAsync(x => x.PurchaseRequestId == entity.Id);
        var deleted = await _purchaseRequestRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购申请不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购申请
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseRequestBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseRequestByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购申请状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestDto> UpdatePurchaseRequestStatusAsync(TaktPurchaseRequestStatusDto dto)
    {
        var entity = await _purchaseRequestRepository.GetByIdAsync(dto.PurchaseRequestId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购申请不存在");
        }
        entity.RequestStatus = dto.RequestStatus;
        await _purchaseRequestRepository.UpdateAsync(entity);
        return await GetPurchaseRequestByIdAsync(dto.PurchaseRequestId) ?? throw new TaktBusinessException("采购申请不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseRequestTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseRequestTemplateDto>(
            sheetName ?? "采购申请导入模板",
            fileName ?? "采购申请导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购申请
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseRequestAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseRequestImportDto>(fileStream, sheetName ?? "采购申请导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseRequest>();
                var importKey = $"{entity.PlantCode}|{entity.PurchaseRequestCode}|{entity.RequestDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchaseRequestCode、RequestDate）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_request_pr_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseRequestRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchaseRequestCode == entity.PurchaseRequestCode
                        && x.RequestDate == entity.RequestDate);
                if (!isUnique_ix_takt_logistics_procurement_purchase_request_pr_unique)
                {
                    throw new TaktBusinessException("采购申请的PlantCode、PurchaseRequestCode、RequestDate已存在");
                }
                await _purchaseRequestRepository.CreateAsync(entity);
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
    /// 导出采购申请
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseRequestAsync(TaktPurchaseRequestQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseRequestQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseRequestExportDto>(),
                sheetName ?? "采购申请数据",
                fileName ?? "采购申请导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseRequestRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseRequestExportDto>(),
                sheetName ?? "采购申请数据",
                fileName ?? "采购申请导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseRequestExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购申请数据",
            fileName ?? "采购申请导出.xlsx");
    }

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取采购申请统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>采购申请统计</returns>
    public async Task<TaktPurchaseRequestStatDto> GetPurchaseRequestStatAsync(TaktProcurementStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.RequestDateStart ?? queryDto.OrderDateStart,
            queryDto.RequestDateEnd ?? queryDto.OrderDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktPurchaseRequest, bool>> requestPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.RequestDate >= start
            && x.RequestDate <= end;
        var monthRequestCount = await _purchaseRequestRepository.CountAsync(requestPredicate);
        Expression<Func<TaktPurchaseRequestItem, bool>> itemPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IsObsolete == 0
            && SqlFunc.Subqueryable<TaktPurchaseRequest>()
                .Where(h =>
                    h.Id == x.PurchaseRequestId
                    && h.TenantCode == tenantCode
                    && h.CompanyCode == companyCode
                    && h.RequestDate >= start
                    && h.RequestDate <= end
                    && h.IsDeleted == 0)
                .Any();
        var monthTotalAmount = await _purchaseRequestItemRepository.SumAsync(x => x.RequestAmount, itemPredicate);
        if (monthTotalAmount == 0 && monthRequestCount > 0)
        {
            monthTotalAmount = await _purchaseRequestRepository.SumAsync(x => x.TotalAmount, requestPredicate);
        }
        return new TaktPurchaseRequestStatDto
        {
            StatMonth = statMonth,
            MonthRequestCount = monthRequestCount,
            MonthTotalAmount = monthTotalAmount,
        };
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废采购申请明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchaseRequestId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchaseRequestItemsObsoleteAsync(long purchaseRequestId)
    {
        if (purchaseRequestId <= 0)
        {
            return;
        }
        var rows = await _purchaseRequestItemRepository.GetListAsync(
            x => x.PurchaseRequestId == purchaseRequestId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchaseRequestItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充采购申请详情（加载 OneToMany 子表：采购申请明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchaseRequestDetailsAsync(TaktPurchaseRequestDto dto, TaktPurchaseRequest entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购申请明细 → dto.Items（含作废行）
        var items = await _purchaseRequestItemRepository.GetListAsync(x => x.PurchaseRequestId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchaseRequestItemDto>>();
    }

    /// <summary>
    /// 保存采购申请子表级联（采购申请明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchaseRequestChildrenAsync(TaktPurchaseRequest entity, TaktPurchaseRequestCreateDto dto)
    {
        // 采购申请明细（Items）
        List<TaktPurchaseRequestItemUpdateDto>? itemsForSave;
        if (dto is TaktPurchaseRequestUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktPurchaseRequestItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkPurchaseRequestItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _purchaseRequestItemRepository.GetListAsync(x => x.PurchaseRequestId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchaseRequestItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.PurchaseRequestId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.PurchaseRequestCode = entity.PurchaseRequestCode;
                childDto.TaxCode = entity.TaxCode ?? string.Empty;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("采购申请明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PurchaseRequestId、LineNumber）");
                }
                if (childDto.PurchaseRequestItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchaseRequestItemId, out var target))
                    {
                        throw new TaktBusinessException("采购申请明细不存在（PurchaseRequestItemId={childDto.PurchaseRequestItemId}）");
                    }
                    if (target.PurchaseRequestId != entity.Id)
                    {
                        throw new TaktBusinessException("采购申请明细不属于当前主表（PurchaseRequestItemId={childDto.PurchaseRequestItemId}）");
                    }
                    submittedIds.Add(childDto.PurchaseRequestItemId);
                    var isUniqueUpdate_ix_takt_logistics_procurement_purchase_request_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _purchaseRequestItemRepository,
                        x => x.PurchaseRequestId == x.PurchaseRequestId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode,
                        childDto.PurchaseRequestItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_procurement_purchase_request_item_request_line_unique)
                    {
                        throw new TaktBusinessException("采购申请明细的PurchaseRequestId、LineNumber、MaterialCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.PurchaseRequestItemId;
                    target.PurchaseRequestId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchaseRequestItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_procurement_purchase_request_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _purchaseRequestItemRepository,
                        x => x.PurchaseRequestId == x.PurchaseRequestId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode);
                    if (!isUniqueCreate_ix_takt_logistics_procurement_purchase_request_item_request_line_unique)
                    {
                        throw new TaktBusinessException("采购申请明细的PurchaseRequestId、LineNumber、MaterialCode已存在");
                    }
                    var child = childDto.Adapt<TaktPurchaseRequestItem>();
                    child.Id = 0;
                    child.PurchaseRequestId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchaseRequestItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseRequestCode) ? entity.PurchaseRequestCode : entity.Id.ToString();
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
                await _purchaseRequestItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购申请查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseRequest, bool>> QueryExpression(TaktPurchaseRequestQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseRequest>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(keywords))
                || (x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(keywords))
                || (x.PurchasePlanCode != null && x.PurchasePlanCode.Contains(keywords))
                || (x.CountersignCode != null && x.CountersignCode.Contains(keywords))
                || (x.RequestEmployeeName != null && x.RequestEmployeeName.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.SupplierName1 != null && x.SupplierName1.Contains(keywords))
                || (x.PurchaseRequestType != null && x.PurchaseRequestType.Contains(keywords))
                || (x.PricingProcedure != null && x.PricingProcedure.Contains(keywords))
                || (x.PricingConditionCode != null && x.PricingConditionCode.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.RequestReason != null && x.RequestReason.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseRequestCode))
        {
            var purchaseRequestCode = queryDto.PurchaseRequestCode;
            exp = exp.And(x => x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(purchaseRequestCode));
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

        if (queryDto?.PurchasePlanId.HasValue == true)
        {
            var purchasePlanId = queryDto.PurchasePlanId.Value;
            exp = exp.And(x => x.PurchasePlanId == purchasePlanId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchasePlanCode))
        {
            var purchasePlanCode = queryDto.PurchasePlanCode;
            exp = exp.And(x => x.PurchasePlanCode != null && x.PurchasePlanCode.Contains(purchasePlanCode));
        }

        if (queryDto?.ChainScheme.HasValue == true)
        {
            var chainScheme = queryDto.ChainScheme.Value;
            exp = exp.And(x => x.ChainScheme == chainScheme);
        }

        if (queryDto?.PoDecision.HasValue == true)
        {
            var poDecision = queryDto.PoDecision.Value;
            exp = exp.And(x => x.PoDecision == poDecision);
        }

        if (queryDto?.CountersignId.HasValue == true)
        {
            var countersignId = queryDto.CountersignId.Value;
            exp = exp.And(x => x.CountersignId == countersignId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CountersignCode))
        {
            var countersignCode = queryDto.CountersignCode;
            exp = exp.And(x => x.CountersignCode != null && x.CountersignCode.Contains(countersignCode));
        }

        if (queryDto?.RequestEmployeeId.HasValue == true)
        {
            var requestEmployeeId = queryDto.RequestEmployeeId.Value;
            exp = exp.And(x => x.RequestEmployeeId == requestEmployeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RequestEmployeeName))
        {
            var requestEmployeeName = queryDto.RequestEmployeeName;
            exp = exp.And(x => x.RequestEmployeeName != null && x.RequestEmployeeName.Contains(requestEmployeeName));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseRequestType))
        {
            var purchaseRequestType = queryDto.PurchaseRequestType;
            exp = exp.And(x => x.PurchaseRequestType != null && x.PurchaseRequestType.Contains(purchaseRequestType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PricingProcedure))
        {
            var pricingProcedure = queryDto.PricingProcedure;
            exp = exp.And(x => x.PricingProcedure != null && x.PricingProcedure.Contains(pricingProcedure));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PricingConditionCode))
        {
            var pricingConditionCode = queryDto.PricingConditionCode;
            exp = exp.And(x => x.PricingConditionCode != null && x.PricingConditionCode.Contains(pricingConditionCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxCode))
        {
            var taxCode = queryDto.TaxCode;
            exp = exp.And(x => x.TaxCode != null && x.TaxCode.Contains(taxCode));
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

        if (queryDto?.TotalQuantity.HasValue == true)
        {
            var totalQuantity = queryDto.TotalQuantity.Value;
            exp = exp.And(x => x.TotalQuantity == totalQuantity);
        }

        if (queryDto?.TotalAmount.HasValue == true)
        {
            var totalAmount = queryDto.TotalAmount.Value;
            exp = exp.And(x => x.TotalAmount == totalAmount);
        }

        if (queryDto?.ConvertedQuantity.HasValue == true)
        {
            var convertedQuantity = queryDto.ConvertedQuantity.Value;
            exp = exp.And(x => x.ConvertedQuantity == convertedQuantity);
        }

        if (queryDto?.ConvertedAmount.HasValue == true)
        {
            var convertedAmount = queryDto.ConvertedAmount.Value;
            exp = exp.And(x => x.ConvertedAmount == convertedAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RequestReason))
        {
            var requestReason = queryDto.RequestReason;
            exp = exp.And(x => x.RequestReason != null && x.RequestReason.Contains(requestReason));
        }

        if (queryDto?.RequestStatus.HasValue == true)
        {
            var requestStatus = queryDto.RequestStatus.Value;
            exp = exp.And(x => x.RequestStatus == requestStatus);
        }

        if (queryDto?.ConvertedStatus.HasValue == true)
        {
            var convertedStatus = queryDto.ConvertedStatus.Value;
            exp = exp.And(x => x.ConvertedStatus == convertedStatus);
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

        if (queryDto?.RequestDateStart.HasValue == true)
        {
            var requestDateStart = queryDto.RequestDateStart.Value;
            exp = exp.And(x => x.RequestDate >= requestDateStart);
        }

        if (queryDto?.RequestDateEnd.HasValue == true)
        {
            var requestDateEnd = queryDto.RequestDateEnd.Value;
            exp = exp.And(x => x.RequestDate <= requestDateEnd);
        }

        if (queryDto?.RequiredArrivalDateStart.HasValue == true)
        {
            var requiredArrivalDateStart = queryDto.RequiredArrivalDateStart.Value;
            exp = exp.And(x => x.RequiredArrivalDate >= requiredArrivalDateStart);
        }

        if (queryDto?.RequiredArrivalDateEnd.HasValue == true)
        {
            var requiredArrivalDateEnd = queryDto.RequiredArrivalDateEnd.Value;
            exp = exp.And(x => x.RequiredArrivalDate <= requiredArrivalDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktPurchaseRequestQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseRequestCode))
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
        if (queryDto.PurchasePlanId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchasePlanCode))
        {
            return true;
        }
        if (queryDto.ChainScheme.HasValue)
        {
            return true;
        }
        if (queryDto.PoDecision.HasValue)
        {
            return true;
        }
        if (queryDto.CountersignId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CountersignCode))
        {
            return true;
        }
        if (queryDto.RequestEmployeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RequestEmployeeName))
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseRequestType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PricingProcedure))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PricingConditionCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxCode))
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
        if (queryDto.TotalQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.TotalAmount.HasValue)
        {
            return true;
        }
        if (queryDto.ConvertedQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.ConvertedAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RequestReason))
        {
            return true;
        }
        if (queryDto.RequestStatus.HasValue)
        {
            return true;
        }
        if (queryDto.ConvertedStatus.HasValue)
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
        if (queryDto.RequestDateStart.HasValue || queryDto.RequestDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.RequiredArrivalDateStart.HasValue || queryDto.RequiredArrivalDateEnd.HasValue)
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
