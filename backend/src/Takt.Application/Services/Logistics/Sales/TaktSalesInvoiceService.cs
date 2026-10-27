// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesInvoiceService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：销售发票应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Sales;
using Takt.Domain.Entities.Logistics.Sales;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Sales;

/// <summary>
/// 销售发票应用服务
/// </summary>
public class TaktSalesInvoiceService : TaktServiceBase, ITaktSalesInvoiceService
{
    private readonly ITaktCompanyRepository<TaktSalesInvoice> _salesInvoiceRepository;
    private readonly ITaktCompanyRepository<TaktSalesInvoiceItem> _salesInvoiceItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesInvoiceRepository">销售发票仓储</param>
    /// <param name="salesInvoiceItemRepository">SalesInvoiceItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesInvoiceService(
        ITaktCompanyRepository<TaktSalesInvoice> salesInvoiceRepository,
        ITaktCompanyRepository<TaktSalesInvoiceItem> salesInvoiceItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesInvoiceRepository = salesInvoiceRepository;
        _salesInvoiceItemRepository = salesInvoiceItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取销售发票列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesInvoiceDto>> GetSalesInvoiceListAsync(TaktSalesInvoiceQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesInvoiceDto>.Create(
                new List<TaktSalesInvoiceDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesInvoiceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesInvoiceDto>.Create(
            data.Adapt<List<TaktSalesInvoiceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售发票
    /// </summary>
    /// <param name="id">销售发票ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceDto?> GetSalesInvoiceByIdAsync(long id)
    {
        var entity = await _salesInvoiceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesInvoiceDto>();
        await FillSalesInvoiceDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售发票选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesInvoiceOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesInvoiceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.CustomerCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.CustomerCode,
            DictLabel = e.CustomerCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售发票
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceDto> CreateSalesInvoiceAsync(TaktSalesInvoiceCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesInvoice>();
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var existingRows = await _salesInvoiceRepository.GetListAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode);
            var generated = await _numberingGenerator.GenerateNextAsync(
                dto.NumberingRuleCode.Trim(),
                existingRows.Select(x => (string?)x.BillingDocumentCode).ToList());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.BillingDocumentCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.BillingDocumentCode))
        {
            throw new TaktBusinessException("开票凭证不能为空");
        }
        var isUnique_ix_takt_logistics_sales_invoice_billing_doc_unique = await _uniqueValidator.IsUniqueAsync(
            _salesInvoiceRepository,
            x => x.BillingDocumentCode == entity.BillingDocumentCode);
        if (!isUnique_ix_takt_logistics_sales_invoice_billing_doc_unique)
        {
            throw new TaktBusinessException("销售发票的BillingDocumentCode已存在");
        }
        entity = await _salesInvoiceRepository.CreateAsync(entity);
                await SaveSalesInvoiceChildrenAsync(entity, dto);
        return await GetSalesInvoiceByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesInvoiceDto>();
    }

    /// <summary>
    /// 更新销售发票
    /// </summary>
    /// <param name="id">销售发票ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceDto> UpdateSalesInvoiceAsync(long id, TaktSalesInvoiceUpdateDto dto)
    {
        var entity = await _salesInvoiceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售发票不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_invoice_billing_doc_unique = await _uniqueValidator.IsUniqueAsync(
            _salesInvoiceRepository,
            x => x.BillingDocumentCode == entity.BillingDocumentCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_invoice_billing_doc_unique)
        {
            throw new TaktBusinessException("销售发票的BillingDocumentCode已存在");
        }
        await _salesInvoiceRepository.UpdateAsync(entity);
                await SaveSalesInvoiceChildrenAsync(entity, dto);
        return await GetSalesInvoiceByIdAsync(id) ?? throw new TaktBusinessException("销售发票不存在");
    }

    /// <summary>
    /// 删除销售发票
    /// </summary>
    /// <param name="id">销售发票ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesInvoiceByIdAsync(long id)
    {
        var entity = await _salesInvoiceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售发票不存在或已删除");
        }
        await _salesInvoiceItemRepository.DeleteAsync(x => x.SalesInvoiceId == entity.Id);
        var deleted = await _salesInvoiceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售发票不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售发票
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesInvoiceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesInvoiceByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售发票状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceDto> UpdateSalesInvoiceStatusAsync(TaktSalesInvoiceStatusDto dto)
    {
        var entity = await _salesInvoiceRepository.GetByIdAsync(dto.SalesInvoiceId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售发票不存在");
        }
        entity.PostingStatus = dto.PostingStatus;
        await _salesInvoiceRepository.UpdateAsync(entity);
        return await GetSalesInvoiceByIdAsync(dto.SalesInvoiceId) ?? throw new TaktBusinessException("销售发票不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesInvoiceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesInvoiceTemplateDto>(
            sheetName ?? "销售发票导入模板",
            fileName ?? "销售发票导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售发票
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesInvoiceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesInvoiceImportDto>(fileStream, sheetName ?? "销售发票导入模板");
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
                var entity = rows[i].Adapt<TaktSalesInvoice>();
                var importKey = $"{entity.BillingDocumentCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（BillingDocumentCode）");
                }
                var isUnique_ix_takt_logistics_sales_invoice_billing_doc_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesInvoiceRepository,
                    x => x.BillingDocumentCode == entity.BillingDocumentCode);
                if (!isUnique_ix_takt_logistics_sales_invoice_billing_doc_unique)
                {
                    throw new TaktBusinessException("销售发票的BillingDocumentCode已存在");
                }
                await _salesInvoiceRepository.CreateAsync(entity);
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
    /// 导出销售发票
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesInvoiceAsync(TaktSalesInvoiceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesInvoiceQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesInvoiceExportDto>(),
                sheetName ?? "销售发票数据",
                fileName ?? "销售发票导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesInvoiceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesInvoiceExportDto>(),
                sheetName ?? "销售发票数据",
                fileName ?? "销售发票导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesInvoiceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售发票数据",
            fileName ?? "销售发票导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废销售发票明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesInvoiceId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesInvoiceItemsObsoleteAsync(long salesInvoiceId)
    {
        if (salesInvoiceId <= 0)
        {
            return;
        }
        var rows = await _salesInvoiceItemRepository.GetListAsync(
            x => x.SalesInvoiceId == salesInvoiceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesInvoiceItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充销售发票详情（加载 OneToMany 子表：销售发票明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesInvoiceDetailsAsync(TaktSalesInvoiceDto dto, TaktSalesInvoice entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售发票明细 → dto.Items（含作废行）
        var items = await _salesInvoiceItemRepository.GetListAsync(x => x.SalesInvoiceId == entity.Id);
        dto.Items = items.Adapt<List<TaktSalesInvoiceItemDto>>();
    }

    /// <summary>
    /// 保存销售发票子表级联（销售发票明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesInvoiceChildrenAsync(TaktSalesInvoice entity, TaktSalesInvoiceCreateDto dto)
    {
        // 销售发票明细（Items）
        List<TaktSalesInvoiceItemUpdateDto>? itemsForSave;
        if (dto is TaktSalesInvoiceUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktSalesInvoiceItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkSalesInvoiceItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _salesInvoiceItemRepository.GetListAsync(x => x.SalesInvoiceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesInvoiceItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.SalesInvoiceId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.BillingDocumentCode = entity.BillingDocumentCode;
                childDto.Division = entity.Division ?? string.Empty;
                childDto.DocumentCategory = entity.DocumentCategory ?? string.Empty;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("销售发票明细第{i + 1}项与本次提交的其他项重复（CompanyCode、SalesInvoiceId、LineNumber）");
                }
                if (childDto.SalesInvoiceItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesInvoiceItemId, out var target))
                    {
                        throw new TaktBusinessException("销售发票明细不存在（SalesInvoiceItemId={childDto.SalesInvoiceItemId}）");
                    }
                    if (target.SalesInvoiceId != entity.Id)
                    {
                        throw new TaktBusinessException("销售发票明细不属于当前主表（SalesInvoiceItemId={childDto.SalesInvoiceItemId}）");
                    }
                    submittedIds.Add(childDto.SalesInvoiceItemId);
                    var isUniqueUpdate_ix_takt_logistics_sales_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _salesInvoiceItemRepository,
                        x => x.SalesInvoiceId == x.SalesInvoiceId
                && x.LineNumber == x.LineNumber,
                        childDto.SalesInvoiceItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_sales_invoice_item_invoice_line_unique)
                    {
                        throw new TaktBusinessException("销售发票明细的SalesInvoiceId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.SalesInvoiceItemId;
                    target.SalesInvoiceId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesInvoiceItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_sales_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _salesInvoiceItemRepository,
                        x => x.SalesInvoiceId == x.SalesInvoiceId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_sales_invoice_item_invoice_line_unique)
                    {
                        throw new TaktBusinessException("销售发票明细的SalesInvoiceId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktSalesInvoiceItem>();
                    child.Id = 0;
                    child.SalesInvoiceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesInvoiceItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.BillingDocumentCode) ? entity.BillingDocumentCode : entity.Id.ToString();
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
                await _salesInvoiceItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售发票查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesInvoice, bool>> QueryExpression(TaktSalesInvoiceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesInvoice>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.BillingDocumentCode != null && x.BillingDocumentCode.Contains(keywords))
                || (x.BillingType != null && x.BillingType.Contains(keywords))
                || (x.BillingCategory != null && x.BillingCategory.Contains(keywords))
                || (x.DocumentCategory != null && x.DocumentCategory.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.SalesOrganization != null && x.SalesOrganization.Contains(keywords))
                || (x.DistributionChannel != null && x.DistributionChannel.Contains(keywords))
                || (x.PricingProcedure != null && x.PricingProcedure.Contains(keywords))
                || (x.ConditionCode != null && x.ConditionCode.Contains(keywords))
                || (x.ShippingConditions != null && x.ShippingConditions.Contains(keywords))
                || (x.CustomerGroup != null && x.CustomerGroup.Contains(keywords))
                || (x.Incoterms1 != null && x.Incoterms1.Contains(keywords))
                || (x.Incoterms2 != null && x.Incoterms2.Contains(keywords))
                || (x.PostingStatus != null && x.PostingStatus.Contains(keywords))
                || (x.PaymentTerms != null && x.PaymentTerms.Contains(keywords))
                || (x.AccountAssignmentGroup != null && x.AccountAssignmentGroup.Contains(keywords))
                || (x.CountryCode != null && x.CountryCode.Contains(keywords))
                || (x.PayerCode != null && x.PayerCode.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.StatisticsCurrencyCode != null && x.StatisticsCurrencyCode.Contains(keywords))
                || (x.ForeignTradeCode != null && x.ForeignTradeCode.Contains(keywords))
                || (x.CancelledBillingDocument != null && x.CancelledBillingDocument.Contains(keywords))
                || (x.InvoiceListType != null && x.InvoiceListType.Contains(keywords))
                || (x.Division != null && x.Division.Contains(keywords))
                || (x.HierarchyTypePricing != null && x.HierarchyTypePricing.Contains(keywords))
                || (x.TradingPartner != null && x.TradingPartner.Contains(keywords))
                || (x.TaxDepartureCountry != null && x.TaxDepartureCountry.Contains(keywords))
                || (x.OrganizationSalesTaxNumber != null && x.OrganizationSalesTaxNumber.Contains(keywords))
                || (x.CountrySalesTaxNumber != null && x.CountrySalesTaxNumber.Contains(keywords))
                || (x.ReferenceCode != null && x.ReferenceCode.Contains(keywords))
                || (x.CancelledFlag != null && x.CancelledFlag.Contains(keywords))
                || (x.PaymentReference != null && x.PaymentReference.Contains(keywords))
                || (x.ReversalReason != null && x.ReversalReason.Contains(keywords))
                || (x.PostedBy != null && x.PostedBy.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.BillingDocumentCode))
        {
            var billingDocumentCode = queryDto.BillingDocumentCode;
            exp = exp.And(x => x.BillingDocumentCode != null && x.BillingDocumentCode.Contains(billingDocumentCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BillingType))
        {
            var billingType = queryDto.BillingType;
            exp = exp.And(x => x.BillingType != null && x.BillingType.Contains(billingType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BillingCategory))
        {
            var billingCategory = queryDto.BillingCategory;
            exp = exp.And(x => x.BillingCategory != null && x.BillingCategory.Contains(billingCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentCategory))
        {
            var documentCategory = queryDto.DocumentCategory;
            exp = exp.And(x => x.DocumentCategory != null && x.DocumentCategory.Contains(documentCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesOrganization))
        {
            var salesOrganization = queryDto.SalesOrganization;
            exp = exp.And(x => x.SalesOrganization != null && x.SalesOrganization.Contains(salesOrganization));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DistributionChannel))
        {
            var distributionChannel = queryDto.DistributionChannel;
            exp = exp.And(x => x.DistributionChannel != null && x.DistributionChannel.Contains(distributionChannel));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PricingProcedure))
        {
            var pricingProcedure = queryDto.PricingProcedure;
            exp = exp.And(x => x.PricingProcedure != null && x.PricingProcedure.Contains(pricingProcedure));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConditionCode))
        {
            var conditionCode = queryDto.ConditionCode;
            exp = exp.And(x => x.ConditionCode != null && x.ConditionCode.Contains(conditionCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ShippingConditions))
        {
            var shippingConditions = queryDto.ShippingConditions;
            exp = exp.And(x => x.ShippingConditions != null && x.ShippingConditions.Contains(shippingConditions));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerGroup))
        {
            var customerGroup = queryDto.CustomerGroup;
            exp = exp.And(x => x.CustomerGroup != null && x.CustomerGroup.Contains(customerGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Incoterms1))
        {
            var incoterms1 = queryDto.Incoterms1;
            exp = exp.And(x => x.Incoterms1 != null && x.Incoterms1.Contains(incoterms1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Incoterms2))
        {
            var incoterms2 = queryDto.Incoterms2;
            exp = exp.And(x => x.Incoterms2 != null && x.Incoterms2.Contains(incoterms2));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostingStatus))
        {
            var postingStatus = queryDto.PostingStatus;
            exp = exp.And(x => x.PostingStatus != null && x.PostingStatus.Contains(postingStatus));
        }

        if (queryDto?.AccountingExchangeRate.HasValue == true)
        {
            var accountingExchangeRate = queryDto.AccountingExchangeRate.Value;
            exp = exp.And(x => x.AccountingExchangeRate == accountingExchangeRate);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PaymentTerms))
        {
            var paymentTerms = queryDto.PaymentTerms;
            exp = exp.And(x => x.PaymentTerms != null && x.PaymentTerms.Contains(paymentTerms));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountAssignmentGroup))
        {
            var accountAssignmentGroup = queryDto.AccountAssignmentGroup;
            exp = exp.And(x => x.AccountAssignmentGroup != null && x.AccountAssignmentGroup.Contains(accountAssignmentGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CountryCode))
        {
            var countryCode = queryDto.CountryCode;
            exp = exp.And(x => x.CountryCode != null && x.CountryCode.Contains(countryCode));
        }

        if (queryDto?.NetAmount.HasValue == true)
        {
            var netAmount = queryDto.NetAmount.Value;
            exp = exp.And(x => x.NetAmount == netAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PayerCode))
        {
            var payerCode = queryDto.PayerCode;
            exp = exp.And(x => x.PayerCode != null && x.PayerCode.Contains(payerCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerCode))
        {
            var customerCode = queryDto.CustomerCode;
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(customerCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StatisticsCurrencyCode))
        {
            var statisticsCurrencyCode = queryDto.StatisticsCurrencyCode;
            exp = exp.And(x => x.StatisticsCurrencyCode != null && x.StatisticsCurrencyCode.Contains(statisticsCurrencyCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ForeignTradeCode))
        {
            var foreignTradeCode = queryDto.ForeignTradeCode;
            exp = exp.And(x => x.ForeignTradeCode != null && x.ForeignTradeCode.Contains(foreignTradeCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CancelledBillingDocument))
        {
            var cancelledBillingDocument = queryDto.CancelledBillingDocument;
            exp = exp.And(x => x.CancelledBillingDocument != null && x.CancelledBillingDocument.Contains(cancelledBillingDocument));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InvoiceListType))
        {
            var invoiceListType = queryDto.InvoiceListType;
            exp = exp.And(x => x.InvoiceListType != null && x.InvoiceListType.Contains(invoiceListType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Division))
        {
            var division = queryDto.Division;
            exp = exp.And(x => x.Division != null && x.Division.Contains(division));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HierarchyTypePricing))
        {
            var hierarchyTypePricing = queryDto.HierarchyTypePricing;
            exp = exp.And(x => x.HierarchyTypePricing != null && x.HierarchyTypePricing.Contains(hierarchyTypePricing));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TradingPartner))
        {
            var tradingPartner = queryDto.TradingPartner;
            exp = exp.And(x => x.TradingPartner != null && x.TradingPartner.Contains(tradingPartner));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxDepartureCountry))
        {
            var taxDepartureCountry = queryDto.TaxDepartureCountry;
            exp = exp.And(x => x.TaxDepartureCountry != null && x.TaxDepartureCountry.Contains(taxDepartureCountry));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OrganizationSalesTaxNumber))
        {
            var organizationSalesTaxNumber = queryDto.OrganizationSalesTaxNumber;
            exp = exp.And(x => x.OrganizationSalesTaxNumber != null && x.OrganizationSalesTaxNumber.Contains(organizationSalesTaxNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CountrySalesTaxNumber))
        {
            var countrySalesTaxNumber = queryDto.CountrySalesTaxNumber;
            exp = exp.And(x => x.CountrySalesTaxNumber != null && x.CountrySalesTaxNumber.Contains(countrySalesTaxNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReferenceCode))
        {
            var referenceCode = queryDto.ReferenceCode;
            exp = exp.And(x => x.ReferenceCode != null && x.ReferenceCode.Contains(referenceCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CancelledFlag))
        {
            var cancelledFlag = queryDto.CancelledFlag;
            exp = exp.And(x => x.CancelledFlag != null && x.CancelledFlag.Contains(cancelledFlag));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PaymentReference))
        {
            var paymentReference = queryDto.PaymentReference;
            exp = exp.And(x => x.PaymentReference != null && x.PaymentReference.Contains(paymentReference));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReversalReason))
        {
            var reversalReason = queryDto.ReversalReason;
            exp = exp.And(x => x.ReversalReason != null && x.ReversalReason.Contains(reversalReason));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PostedBy))
        {
            var postedBy = queryDto.PostedBy;
            exp = exp.And(x => x.PostedBy != null && x.PostedBy.Contains(postedBy));
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

        if (queryDto?.BillingDateStart.HasValue == true)
        {
            var billingDateStart = queryDto.BillingDateStart.Value;
            exp = exp.And(x => x.BillingDate >= billingDateStart);
        }

        if (queryDto?.BillingDateEnd.HasValue == true)
        {
            var billingDateEnd = queryDto.BillingDateEnd.Value;
            exp = exp.And(x => x.BillingDate <= billingDateEnd);
        }

        if (queryDto?.ExchangeRateDateStart.HasValue == true)
        {
            var exchangeRateDateStart = queryDto.ExchangeRateDateStart.Value;
            exp = exp.And(x => x.ExchangeRateDate >= exchangeRateDateStart);
        }

        if (queryDto?.ExchangeRateDateEnd.HasValue == true)
        {
            var exchangeRateDateEnd = queryDto.ExchangeRateDateEnd.Value;
            exp = exp.And(x => x.ExchangeRateDate <= exchangeRateDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktSalesInvoiceQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.BillingDocumentCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BillingType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BillingCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesOrganization))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DistributionChannel))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PricingProcedure))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConditionCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ShippingConditions))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Incoterms1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Incoterms2))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostingStatus))
        {
            return true;
        }
        if (queryDto.AccountingExchangeRate.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PaymentTerms))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccountAssignmentGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CountryCode))
        {
            return true;
        }
        if (queryDto.NetAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PayerCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StatisticsCurrencyCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ForeignTradeCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CancelledBillingDocument))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InvoiceListType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Division))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HierarchyTypePricing))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TradingPartner))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxDepartureCountry))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OrganizationSalesTaxNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CountrySalesTaxNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReferenceCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CancelledFlag))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PaymentReference))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReversalReason))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PostedBy))
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
        if (queryDto.BillingDateStart.HasValue || queryDto.BillingDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ExchangeRateDateStart.HasValue || queryDto.ExchangeRateDateEnd.HasValue)
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
