// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseInquiryService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：采购询价应用服务实现
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
/// 采购询价应用服务
/// </summary>
public class TaktPurchaseInquiryService : TaktServiceBase, ITaktPurchaseInquiryService
{
    private readonly ITaktCompanyRepository<TaktPurchaseInquiry> _purchaseInquiryRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInquiryItem> _purchaseInquiryItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInquiryRepository">采购询价仓储</param>
    /// <param name="purchaseInquiryItemRepository">PurchaseInquiryItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseInquiryService(
        ITaktCompanyRepository<TaktPurchaseInquiry> purchaseInquiryRepository,
        ITaktCompanyRepository<TaktPurchaseInquiryItem> purchaseInquiryItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInquiryRepository = purchaseInquiryRepository;
        _purchaseInquiryItemRepository = purchaseInquiryItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购询价列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseInquiryDto>> GetPurchaseInquiryListAsync(TaktPurchaseInquiryQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseInquiryDto>.Create(
                new List<TaktPurchaseInquiryDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseInquiryRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseInquiryDto>.Create(
            data.Adapt<List<TaktPurchaseInquiryDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购询价
    /// </summary>
    /// <param name="id">采购询价ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInquiryDto?> GetPurchaseInquiryByIdAsync(long id)
    {
        var entity = await _purchaseInquiryRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchaseInquiryDto>();
        await FillPurchaseInquiryDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购询价选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseInquiryOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var normalizedPlantCode = plantCode?.Trim();
        var normalizedKeyword = keyword?.Trim();
        var predicate = Expressionable.Create<TaktPurchaseInquiry>()
            .And(x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InquiryStatus == 1)
            .AndIF(!string.IsNullOrEmpty(normalizedPlantCode), x => x.PlantCode == normalizedPlantCode)
            .AndIF(!string.IsNullOrEmpty(normalizedKeyword), x =>
                (x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(normalizedKeyword!))
                || (x.InquiryEmployeeName != null && x.InquiryEmployeeName.Contains(normalizedKeyword!)))
            .ToExpression();
        var list = await _purchaseInquiryRepository.GetListAsync(
            predicate,
            x => x.InquiryEmployeeName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseInquiryCode,
            DictLabel = e.InquiryEmployeeName ?? e.PurchaseInquiryCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购询价
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInquiryDto> CreatePurchaseInquiryAsync(TaktPurchaseInquiryCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseInquiry>();
        var isUnique_ix_takt_logistics_procurement_purchase_inquiry_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInquiryRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseInquiryCode == entity.PurchaseInquiryCode
                && x.InquiryDate == entity.InquiryDate);
        if (!isUnique_ix_takt_logistics_procurement_purchase_inquiry_code_unique)
        {
            throw new TaktBusinessException("采购询价的PlantCode、PurchaseInquiryCode、InquiryDate已存在");
        }
        entity = await _purchaseInquiryRepository.CreateAsync(entity);
                await SavePurchaseInquiryChildrenAsync(entity, dto);
        return await GetPurchaseInquiryByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseInquiryDto>();
    }

    /// <summary>
    /// 更新采购询价
    /// </summary>
    /// <param name="id">采购询价ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInquiryDto> UpdatePurchaseInquiryAsync(long id, TaktPurchaseInquiryUpdateDto dto)
    {
        var entity = await _purchaseInquiryRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购询价不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_inquiry_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInquiryRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseInquiryCode == entity.PurchaseInquiryCode
                && x.InquiryDate == entity.InquiryDate,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_inquiry_code_unique)
        {
            throw new TaktBusinessException("采购询价的PlantCode、PurchaseInquiryCode、InquiryDate已存在");
        }
        await _purchaseInquiryRepository.UpdateAsync(entity);
                await SavePurchaseInquiryChildrenAsync(entity, dto);
        return await GetPurchaseInquiryByIdAsync(id) ?? throw new TaktBusinessException("采购询价不存在");
    }

    /// <summary>
    /// 删除采购询价
    /// </summary>
    /// <param name="id">采购询价ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInquiryByIdAsync(long id)
    {
        var entity = await _purchaseInquiryRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购询价不存在或已删除");
        }
        await _purchaseInquiryItemRepository.DeleteAsync(x => x.PurchaseInquiryId == entity.Id);
        var deleted = await _purchaseInquiryRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购询价不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购询价
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInquiryBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseInquiryByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购询价状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInquiryDto> UpdatePurchaseInquiryStatusAsync(TaktPurchaseInquiryStatusDto dto)
    {
        var entity = await _purchaseInquiryRepository.GetByIdAsync(dto.PurchaseInquiryId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购询价不存在");
        }
        entity.InquiryStatus = dto.InquiryStatus;
        await _purchaseInquiryRepository.UpdateAsync(entity);
        return await GetPurchaseInquiryByIdAsync(dto.PurchaseInquiryId) ?? throw new TaktBusinessException("采购询价不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseInquiryTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseInquiryTemplateDto>(
            sheetName ?? "采购询价导入模板",
            fileName ?? "采购询价导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购询价
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseInquiryAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseInquiryImportDto>(fileStream, sheetName ?? "采购询价导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseInquiry>();
                var importKey = $"{entity.PlantCode}|{entity.PurchaseInquiryCode}|{entity.InquiryDate}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchaseInquiryCode、InquiryDate）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_inquiry_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseInquiryRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchaseInquiryCode == entity.PurchaseInquiryCode
                        && x.InquiryDate == entity.InquiryDate);
                if (!isUnique_ix_takt_logistics_procurement_purchase_inquiry_code_unique)
                {
                    throw new TaktBusinessException("采购询价的PlantCode、PurchaseInquiryCode、InquiryDate已存在");
                }
                await _purchaseInquiryRepository.CreateAsync(entity);
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
    /// 导出采购询价
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseInquiryAsync(TaktPurchaseInquiryQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseInquiryQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInquiryExportDto>(),
                sheetName ?? "采购询价数据",
                fileName ?? "采购询价导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseInquiryRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInquiryExportDto>(),
                sheetName ?? "采购询价数据",
                fileName ?? "采购询价导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseInquiryExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购询价数据",
            fileName ?? "采购询价导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废采购询价明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchaseInquiryId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchaseInquiryItemsObsoleteAsync(long purchaseInquiryId)
    {
        if (purchaseInquiryId <= 0)
        {
            return;
        }
        var rows = await _purchaseInquiryItemRepository.GetListAsync(
            x => x.PurchaseInquiryId == purchaseInquiryId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchaseInquiryItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充采购询价详情（加载 OneToMany 子表：采购询价明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchaseInquiryDetailsAsync(TaktPurchaseInquiryDto dto, TaktPurchaseInquiry entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购询价明细 → dto.Items（含作废行）
        var items = await _purchaseInquiryItemRepository.GetListAsync(x => x.PurchaseInquiryId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchaseInquiryItemDto>>();
    }

    /// <summary>
    /// 保存采购询价子表级联（采购询价明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchaseInquiryChildrenAsync(TaktPurchaseInquiry entity, TaktPurchaseInquiryCreateDto dto)
    {
        // 采购询价明细（Items）
        List<TaktPurchaseInquiryItemUpdateDto>? itemsForSave;
        if (dto is TaktPurchaseInquiryUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktPurchaseInquiryItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkPurchaseInquiryItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _purchaseInquiryItemRepository.GetListAsync(x => x.PurchaseInquiryId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchaseInquiryItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.PurchaseInquiryId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.PurchaseInquiryCode = entity.PurchaseInquiryCode;
                childDto.TaxCode = entity.TaxCode ?? string.Empty;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("采购询价明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PurchaseInquiryId、LineNumber）");
                }
                if (childDto.PurchaseInquiryItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchaseInquiryItemId, out var target))
                    {
                        throw new TaktBusinessException("采购询价明细不存在（PurchaseInquiryItemId={childDto.PurchaseInquiryItemId}）");
                    }
                    if (target.PurchaseInquiryId != entity.Id)
                    {
                        throw new TaktBusinessException("采购询价明细不属于当前主表（PurchaseInquiryItemId={childDto.PurchaseInquiryItemId}）");
                    }
                    submittedIds.Add(childDto.PurchaseInquiryItemId);
                    var isUniqueUpdate_ix_takt_logistics_procurement_purchase_inquiry_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _purchaseInquiryItemRepository,
                        x => x.PurchaseInquiryId == x.PurchaseInquiryId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode,
                        childDto.PurchaseInquiryItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_procurement_purchase_inquiry_item_line_unique)
                    {
                        throw new TaktBusinessException("采购询价明细的PurchaseInquiryId、LineNumber、MaterialCode已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.PurchaseInquiryItemId;
                    target.PurchaseInquiryId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchaseInquiryItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_procurement_purchase_inquiry_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _purchaseInquiryItemRepository,
                        x => x.PurchaseInquiryId == x.PurchaseInquiryId
                && x.LineNumber == x.LineNumber
                && x.MaterialCode == x.MaterialCode);
                    if (!isUniqueCreate_ix_takt_logistics_procurement_purchase_inquiry_item_line_unique)
                    {
                        throw new TaktBusinessException("采购询价明细的PurchaseInquiryId、LineNumber、MaterialCode已存在");
                    }
                    var child = childDto.Adapt<TaktPurchaseInquiryItem>();
                    child.Id = 0;
                    child.PurchaseInquiryId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchaseInquiryItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseInquiryCode) ? entity.PurchaseInquiryCode : entity.Id.ToString();
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
                await _purchaseInquiryItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购询价查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseInquiry, bool>> QueryExpression(TaktPurchaseInquiryQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseInquiry>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(keywords))
                || (x.InquiryEmployeeName != null && x.InquiryEmployeeName.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.SupplierName1 != null && x.SupplierName1.Contains(keywords))
                || (x.PurchaseInquiryType != null && x.PurchaseInquiryType.Contains(keywords))
                || (x.PricingProcedure != null && x.PricingProcedure.Contains(keywords))
                || (x.PricingConditionCode != null && x.PricingConditionCode.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.PaymentMode != null && x.PaymentMode.Contains(keywords))
                || (x.InquiryReason != null && x.InquiryReason.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseInquiryCode))
        {
            var purchaseInquiryCode = queryDto.PurchaseInquiryCode;
            exp = exp.And(x => x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(purchaseInquiryCode));
        }

        if (queryDto?.InquiryEmployeeId.HasValue == true)
        {
            var inquiryEmployeeId = queryDto.InquiryEmployeeId.Value;
            exp = exp.And(x => x.InquiryEmployeeId == inquiryEmployeeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InquiryEmployeeName))
        {
            var inquiryEmployeeName = queryDto.InquiryEmployeeName;
            exp = exp.And(x => x.InquiryEmployeeName != null && x.InquiryEmployeeName.Contains(inquiryEmployeeName));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseInquiryType))
        {
            var purchaseInquiryType = queryDto.PurchaseInquiryType;
            exp = exp.And(x => x.PurchaseInquiryType != null && x.PurchaseInquiryType.Contains(purchaseInquiryType));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PaymentMode))
        {
            var paymentMode = queryDto.PaymentMode;
            exp = exp.And(x => x.PaymentMode != null && x.PaymentMode.Contains(paymentMode));
        }

        if (queryDto?.ChainScheme.HasValue == true)
        {
            var chainScheme = queryDto.ChainScheme.Value;
            exp = exp.And(x => x.ChainScheme == chainScheme);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.InquiryReason))
        {
            var inquiryReason = queryDto.InquiryReason;
            exp = exp.And(x => x.InquiryReason != null && x.InquiryReason.Contains(inquiryReason));
        }

        if (queryDto?.InquiryStatus.HasValue == true)
        {
            var inquiryStatus = queryDto.InquiryStatus.Value;
            exp = exp.And(x => x.InquiryStatus == inquiryStatus);
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

        if (queryDto?.InquiryDateStart.HasValue == true)
        {
            var inquiryDateStart = queryDto.InquiryDateStart.Value;
            exp = exp.And(x => x.InquiryDate >= inquiryDateStart);
        }

        if (queryDto?.InquiryDateEnd.HasValue == true)
        {
            var inquiryDateEnd = queryDto.InquiryDateEnd.Value;
            exp = exp.And(x => x.InquiryDate <= inquiryDateEnd);
        }

        if (queryDto?.QuoteDeadlineDateStart.HasValue == true)
        {
            var quoteDeadlineDateStart = queryDto.QuoteDeadlineDateStart.Value;
            exp = exp.And(x => x.QuoteDeadlineDate >= quoteDeadlineDateStart);
        }

        if (queryDto?.QuoteDeadlineDateEnd.HasValue == true)
        {
            var quoteDeadlineDateEnd = queryDto.QuoteDeadlineDateEnd.Value;
            exp = exp.And(x => x.QuoteDeadlineDate <= quoteDeadlineDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktPurchaseInquiryQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseInquiryCode))
        {
            return true;
        }
        if (queryDto.InquiryEmployeeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InquiryEmployeeName))
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseInquiryType))
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
        if (!string.IsNullOrWhiteSpace(queryDto.PaymentMode))
        {
            return true;
        }
        if (queryDto.ChainScheme.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.InquiryReason))
        {
            return true;
        }
        if (queryDto.InquiryStatus.HasValue)
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
        if (queryDto.InquiryDateStart.HasValue || queryDto.InquiryDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.QuoteDeadlineDateStart.HasValue || queryDto.QuoteDeadlineDateEnd.HasValue)
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
