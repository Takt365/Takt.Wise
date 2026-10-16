// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：采购发票应用服务实现
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
/// 采购发票应用服务
/// </summary>
public class TaktPurchaseInvoiceService : TaktServiceBase, ITaktPurchaseInvoiceService
{
    private readonly ITaktCompanyRepository<TaktPurchaseInvoice> _purchaseInvoiceRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInvoiceItem> _purchaseInvoiceItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInvoiceRepository">采购发票仓储</param>
    /// <param name="purchaseInvoiceItemRepository">PurchaseInvoiceItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseInvoiceService(
        ITaktCompanyRepository<TaktPurchaseInvoice> purchaseInvoiceRepository,
        ITaktCompanyRepository<TaktPurchaseInvoiceItem> purchaseInvoiceItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInvoiceRepository = purchaseInvoiceRepository;
        _purchaseInvoiceItemRepository = purchaseInvoiceItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取采购发票列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseInvoiceDto>> GetPurchaseInvoiceListAsync(TaktPurchaseInvoiceQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseInvoiceDto>.Create(
                new List<TaktPurchaseInvoiceDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseInvoiceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseInvoiceDto>.Create(
            data.Adapt<List<TaktPurchaseInvoiceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购发票
    /// </summary>
    /// <param name="id">采购发票ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInvoiceDto?> GetPurchaseInvoiceByIdAsync(long id)
    {
        var entity = await _purchaseInvoiceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchaseInvoiceDto>();
        await FillPurchaseInvoiceDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购发票选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseInvoiceOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseInvoiceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.PurchaseInvoiceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseInvoiceCode,
            DictLabel = e.PurchaseInvoiceCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购发票
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInvoiceDto> CreatePurchaseInvoiceAsync(TaktPurchaseInvoiceCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseInvoice>();
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var generated = await _numberingGenerator.GenerateNextAsync(dto.NumberingRuleCode.Trim());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.PurchaseInvoiceCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.PurchaseInvoiceCode))
        {
            throw new TaktBusinessException("发票凭证编号不能为空");
        }
        var isUnique_ix_takt_logistics_procurement_purchase_invoice_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInvoiceRepository,
            x => x.FiscalYear == entity.FiscalYear
                && x.PurchaseInvoiceCode == entity.PurchaseInvoiceCode);
        if (!isUnique_ix_takt_logistics_procurement_purchase_invoice_code_unique)
        {
            throw new TaktBusinessException("采购发票的FiscalYear、PurchaseInvoiceCode已存在");
        }
        entity = await _purchaseInvoiceRepository.CreateAsync(entity);
                await SavePurchaseInvoiceChildrenAsync(entity, dto);
        return await GetPurchaseInvoiceByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseInvoiceDto>();
    }

    /// <summary>
    /// 更新采购发票
    /// </summary>
    /// <param name="id">采购发票ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInvoiceDto> UpdatePurchaseInvoiceAsync(long id, TaktPurchaseInvoiceUpdateDto dto)
    {
        var entity = await _purchaseInvoiceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购发票不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_invoice_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInvoiceRepository,
            x => x.FiscalYear == entity.FiscalYear
                && x.PurchaseInvoiceCode == entity.PurchaseInvoiceCode,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_invoice_code_unique)
        {
            throw new TaktBusinessException("采购发票的FiscalYear、PurchaseInvoiceCode已存在");
        }
        await _purchaseInvoiceRepository.UpdateAsync(entity);
                await SavePurchaseInvoiceChildrenAsync(entity, dto);
        return await GetPurchaseInvoiceByIdAsync(id) ?? throw new TaktBusinessException("采购发票不存在");
    }

    /// <summary>
    /// 删除采购发票
    /// </summary>
    /// <param name="id">采购发票ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInvoiceByIdAsync(long id)
    {
        var entity = await _purchaseInvoiceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购发票不存在或已删除");
        }
        await _purchaseInvoiceItemRepository.DeleteAsync(x => x.PurchaseInvoiceId == entity.Id);
        var deleted = await _purchaseInvoiceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购发票不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购发票
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInvoiceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseInvoiceByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseInvoiceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseInvoiceTemplateDto>(
            sheetName ?? "采购发票导入模板",
            fileName ?? "采购发票导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购发票
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseInvoiceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseInvoiceImportDto>(fileStream, sheetName ?? "采购发票导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseInvoice>();
                var importKey = $"{entity.FiscalYear}|{entity.PurchaseInvoiceCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（FiscalYear、PurchaseInvoiceCode）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_invoice_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseInvoiceRepository,
                    x => x.FiscalYear == entity.FiscalYear
                        && x.PurchaseInvoiceCode == entity.PurchaseInvoiceCode);
                if (!isUnique_ix_takt_logistics_procurement_purchase_invoice_code_unique)
                {
                    throw new TaktBusinessException("采购发票的FiscalYear、PurchaseInvoiceCode已存在");
                }
                await _purchaseInvoiceRepository.CreateAsync(entity);
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
    /// 导出采购发票
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseInvoiceAsync(TaktPurchaseInvoiceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseInvoiceQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInvoiceExportDto>(),
                sheetName ?? "采购发票数据",
                fileName ?? "采购发票导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseInvoiceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInvoiceExportDto>(),
                sheetName ?? "采购发票数据",
                fileName ?? "采购发票导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseInvoiceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购发票数据",
            fileName ?? "采购发票导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废采购发票明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchaseInvoiceId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchaseInvoiceItemsObsoleteAsync(long purchaseInvoiceId)
    {
        if (purchaseInvoiceId <= 0)
        {
            return;
        }
        var rows = await _purchaseInvoiceItemRepository.GetListAsync(
            x => x.PurchaseInvoiceId == purchaseInvoiceId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchaseInvoiceItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充采购发票详情（加载 OneToMany 子表：采购发票明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchaseInvoiceDetailsAsync(TaktPurchaseInvoiceDto dto, TaktPurchaseInvoice entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购发票明细 → dto.Items（含作废行）
        var items = await _purchaseInvoiceItemRepository.GetListAsync(x => x.PurchaseInvoiceId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchaseInvoiceItemDto>>();
    }

    /// <summary>
    /// 保存采购发票子表级联（采购发票明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchaseInvoiceChildrenAsync(TaktPurchaseInvoice entity, TaktPurchaseInvoiceCreateDto dto)
    {
        // 采购发票明细（Items）
        List<TaktPurchaseInvoiceItemUpdateDto>? itemsForSave;
        if (dto is TaktPurchaseInvoiceUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktPurchaseInvoiceItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkPurchaseInvoiceItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _purchaseInvoiceItemRepository.GetListAsync(x => x.PurchaseInvoiceId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchaseInvoiceItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.PurchaseInvoiceId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.PurchaseInvoiceCode = entity.PurchaseInvoiceCode;
                childDto.TaxCode = entity.TaxCode ?? string.Empty;
                childDto.ReferenceCode = entity.ReferenceCode ?? string.Empty;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("采购发票明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PurchaseInvoiceId、LineNumber）");
                }
                if (childDto.PurchaseInvoiceItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchaseInvoiceItemId, out var target))
                    {
                        throw new TaktBusinessException("采购发票明细不存在（PurchaseInvoiceItemId={childDto.PurchaseInvoiceItemId}）");
                    }
                    if (target.PurchaseInvoiceId != entity.Id)
                    {
                        throw new TaktBusinessException("采购发票明细不属于当前主表（PurchaseInvoiceItemId={childDto.PurchaseInvoiceItemId}）");
                    }
                    submittedIds.Add(childDto.PurchaseInvoiceItemId);
                    var isUniqueUpdate_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _purchaseInvoiceItemRepository,
                        x => x.PurchaseInvoiceId == x.PurchaseInvoiceId
                && x.LineNumber == x.LineNumber,
                        childDto.PurchaseInvoiceItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique)
                    {
                        throw new TaktBusinessException("采购发票明细的PurchaseInvoiceId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.PurchaseInvoiceItemId;
                    target.PurchaseInvoiceId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchaseInvoiceItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _purchaseInvoiceItemRepository,
                        x => x.PurchaseInvoiceId == x.PurchaseInvoiceId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique)
                    {
                        throw new TaktBusinessException("采购发票明细的PurchaseInvoiceId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktPurchaseInvoiceItem>();
                    child.Id = 0;
                    child.PurchaseInvoiceId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchaseInvoiceItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseInvoiceCode) ? entity.PurchaseInvoiceCode : entity.Id.ToString();
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
                await _purchaseInvoiceItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购发票查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseInvoice, bool>> QueryExpression(TaktPurchaseInvoiceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseInvoice>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchaseInvoiceCode != null && x.PurchaseInvoiceCode.Contains(keywords))
                || (x.FiscalYear != null && x.FiscalYear.Contains(keywords))
                || (x.DocumentType != null && x.DocumentType.Contains(keywords))
                || (x.TransactionEventType != null && x.TransactionEventType.Contains(keywords))
                || (x.ReferenceCode != null && x.ReferenceCode.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.TaxJurisdictionCode != null && x.TaxJurisdictionCode.Contains(keywords))
                || (x.InvoiceFlag != null && x.InvoiceFlag.Contains(keywords))
                || (x.HeaderText != null && x.HeaderText.Contains(keywords))
                || (x.ReversalDocumentCode != null && x.ReversalDocumentCode.Contains(keywords))
                || (x.ReversalFiscalYear != null && x.ReversalFiscalYear.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.SupplyingCountry != null && x.SupplyingCountry.Contains(keywords))
                || (x.TransactionCode != null && x.TransactionCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseInvoiceCode))
        {
            var purchaseInvoiceCode = queryDto.PurchaseInvoiceCode;
            exp = exp.And(x => x.PurchaseInvoiceCode != null && x.PurchaseInvoiceCode.Contains(purchaseInvoiceCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FiscalYear))
        {
            var fiscalYear = queryDto.FiscalYear;
            exp = exp.And(x => x.FiscalYear != null && x.FiscalYear.Contains(fiscalYear));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentType))
        {
            var documentType = queryDto.DocumentType;
            exp = exp.And(x => x.DocumentType != null && x.DocumentType.Contains(documentType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TransactionEventType))
        {
            var transactionEventType = queryDto.TransactionEventType;
            exp = exp.And(x => x.TransactionEventType != null && x.TransactionEventType.Contains(transactionEventType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReferenceCode))
        {
            var referenceCode = queryDto.ReferenceCode;
            exp = exp.And(x => x.ReferenceCode != null && x.ReferenceCode.Contains(referenceCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierCode))
        {
            var supplierCode = queryDto.SupplierCode;
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(supplierCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CurrencyCode))
        {
            var currencyCode = queryDto.CurrencyCode;
            exp = exp.And(x => x.CurrencyCode != null && x.CurrencyCode.Contains(currencyCode));
        }

        if (queryDto?.ExchangeRate.HasValue == true)
        {
            var exchangeRate = queryDto.ExchangeRate.Value;
            exp = exp.And(x => x.ExchangeRate == exchangeRate);
        }

        if (queryDto?.GrossAmount.HasValue == true)
        {
            var grossAmount = queryDto.GrossAmount.Value;
            exp = exp.And(x => x.GrossAmount == grossAmount);
        }

        if (queryDto?.VatAmount.HasValue == true)
        {
            var vatAmount = queryDto.VatAmount.Value;
            exp = exp.And(x => x.VatAmount == vatAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxJurisdictionCode))
        {
            var taxJurisdictionCode = queryDto.TaxJurisdictionCode;
            exp = exp.And(x => x.TaxJurisdictionCode != null && x.TaxJurisdictionCode.Contains(taxJurisdictionCode));
        }

        if (queryDto?.CashDiscountDays1.HasValue == true)
        {
            var cashDiscountDays1 = queryDto.CashDiscountDays1.Value;
            exp = exp.And(x => x.CashDiscountDays1 == cashDiscountDays1);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InvoiceFlag))
        {
            var invoiceFlag = queryDto.InvoiceFlag;
            exp = exp.And(x => x.InvoiceFlag != null && x.InvoiceFlag.Contains(invoiceFlag));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HeaderText))
        {
            var headerText = queryDto.HeaderText;
            exp = exp.And(x => x.HeaderText != null && x.HeaderText.Contains(headerText));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReversalDocumentCode))
        {
            var reversalDocumentCode = queryDto.ReversalDocumentCode;
            exp = exp.And(x => x.ReversalDocumentCode != null && x.ReversalDocumentCode.Contains(reversalDocumentCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReversalFiscalYear))
        {
            var reversalFiscalYear = queryDto.ReversalFiscalYear;
            exp = exp.And(x => x.ReversalFiscalYear != null && x.ReversalFiscalYear.Contains(reversalFiscalYear));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxCode))
        {
            var taxCode = queryDto.TaxCode;
            exp = exp.And(x => x.TaxCode != null && x.TaxCode.Contains(taxCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplyingCountry))
        {
            var supplyingCountry = queryDto.SupplyingCountry;
            exp = exp.And(x => x.SupplyingCountry != null && x.SupplyingCountry.Contains(supplyingCountry));
        }

        if (queryDto?.TaxExchangeRate.HasValue == true)
        {
            var taxExchangeRate = queryDto.TaxExchangeRate.Value;
            exp = exp.And(x => x.TaxExchangeRate == taxExchangeRate);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TransactionCode))
        {
            var transactionCode = queryDto.TransactionCode;
            exp = exp.And(x => x.TransactionCode != null && x.TransactionCode.Contains(transactionCode));
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

        if (queryDto?.DocumentDateStart.HasValue == true)
        {
            var documentDateStart = queryDto.DocumentDateStart.Value;
            exp = exp.And(x => x.DocumentDate >= documentDateStart);
        }

        if (queryDto?.DocumentDateEnd.HasValue == true)
        {
            var documentDateEnd = queryDto.DocumentDateEnd.Value;
            exp = exp.And(x => x.DocumentDate <= documentDateEnd);
        }

        if (queryDto?.PostingDateStart.HasValue == true)
        {
            var postingDateStart = queryDto.PostingDateStart.Value;
            exp = exp.And(x => x.PostingDate >= postingDateStart);
        }

        if (queryDto?.PostingDateEnd.HasValue == true)
        {
            var postingDateEnd = queryDto.PostingDateEnd.Value;
            exp = exp.And(x => x.PostingDate <= postingDateEnd);
        }

        if (queryDto?.BaselineDateStart.HasValue == true)
        {
            var baselineDateStart = queryDto.BaselineDateStart.Value;
            exp = exp.And(x => x.BaselineDate >= baselineDateStart);
        }

        if (queryDto?.BaselineDateEnd.HasValue == true)
        {
            var baselineDateEnd = queryDto.BaselineDateEnd.Value;
            exp = exp.And(x => x.BaselineDate <= baselineDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktPurchaseInvoiceQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseInvoiceCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FiscalYear))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TransactionEventType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReferenceCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CurrencyCode))
        {
            return true;
        }
        if (queryDto.ExchangeRate.HasValue)
        {
            return true;
        }
        if (queryDto.GrossAmount.HasValue)
        {
            return true;
        }
        if (queryDto.VatAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxJurisdictionCode))
        {
            return true;
        }
        if (queryDto.CashDiscountDays1.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InvoiceFlag))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HeaderText))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReversalDocumentCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReversalFiscalYear))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplyingCountry))
        {
            return true;
        }
        if (queryDto.TaxExchangeRate.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TransactionCode))
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
        if (queryDto.DocumentDateStart.HasValue || queryDto.DocumentDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PostingDateStart.HasValue || queryDto.PostingDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.BaselineDateStart.HasValue || queryDto.BaselineDateEnd.HasValue)
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
