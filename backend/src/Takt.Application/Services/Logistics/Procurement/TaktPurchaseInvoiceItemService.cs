// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseInvoiceItemService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：采购发票明细应用服务实现
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
/// 采购发票明细应用服务
/// </summary>
public class TaktPurchaseInvoiceItemService : TaktServiceBase, ITaktPurchaseInvoiceItemService
{
    private readonly ITaktCompanyRepository<TaktPurchaseInvoiceItem> _purchaseInvoiceItemRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInvoice> _purchaseInvoiceRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInvoiceItemRepository">采购发票明细仓储</param>
    /// <param name="purchaseInvoiceRepository">采购发票仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseInvoiceItemService(
        ITaktCompanyRepository<TaktPurchaseInvoiceItem> purchaseInvoiceItemRepository,
        ITaktCompanyRepository<TaktPurchaseInvoice> purchaseInvoiceRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInvoiceItemRepository = purchaseInvoiceItemRepository;
        _purchaseInvoiceRepository = purchaseInvoiceRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购发票明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseInvoiceItemDto>> GetPurchaseInvoiceItemListAsync(TaktPurchaseInvoiceItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseInvoiceItemDto>.Create(
                new List<TaktPurchaseInvoiceItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseInvoiceItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseInvoiceItemDto>.Create(
            data.Adapt<List<TaktPurchaseInvoiceItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购发票明细
    /// </summary>
    /// <param name="id">采购发票明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInvoiceItemDto?> GetPurchaseInvoiceItemByIdAsync(long id)
    {
        var entity = await _purchaseInvoiceItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchaseInvoiceItemDto>();
    }

    /// <summary>
    /// 获取采购发票明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseInvoiceItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseInvoiceItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.PurchaseInvoiceCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseInvoiceCode,
            DictLabel = e.PurchaseInvoiceCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购发票明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInvoiceItemDto> CreatePurchaseInvoiceItemAsync(TaktPurchaseInvoiceItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseInvoiceItem>();
        entity.IsObsolete = 0;
        await StampPurchaseInvoiceItemPurchaseInvoiceAsync(entity, dto);
        var isUnique_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInvoiceItemRepository,
            x => x.PurchaseInvoiceId == entity.PurchaseInvoiceId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique)
        {
            throw new TaktBusinessException("采购发票明细的PurchaseInvoiceId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _purchaseInvoiceItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseInvoiceId == entity.PurchaseInvoiceId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseInvoiceCode) ? entity.PurchaseInvoiceCode : entity.PurchaseInvoiceId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _purchaseInvoiceItemRepository.CreateAsync(entity);
        return await GetPurchaseInvoiceItemByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseInvoiceItemDto>();
    }

    /// <summary>
    /// 更新采购发票明细
    /// </summary>
    /// <param name="id">采购发票明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInvoiceItemDto> UpdatePurchaseInvoiceItemAsync(long id, TaktPurchaseInvoiceItemUpdateDto dto)
    {
        var entity = await _purchaseInvoiceItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购发票明细不存在");
        }
        dto.Adapt(entity);
        await StampPurchaseInvoiceItemPurchaseInvoiceAsync(entity, dto);
        var isUnique_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInvoiceItemRepository,
            x => x.PurchaseInvoiceId == entity.PurchaseInvoiceId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique)
        {
            throw new TaktBusinessException("采购发票明细的PurchaseInvoiceId、LineNumber已存在");
        }
        await _purchaseInvoiceItemRepository.UpdateAsync(entity);
        return await GetPurchaseInvoiceItemByIdAsync(id) ?? throw new TaktBusinessException("采购发票明细不存在");
    }

    /// <summary>
    /// 删除采购发票明细
    /// </summary>
    /// <param name="id">采购发票明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInvoiceItemByIdAsync(long id)
    {
        var entity = await _purchaseInvoiceItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购发票明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购发票明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("采购发票明细已作废");
        }
        entity.IsObsolete = 1;
        await _purchaseInvoiceItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除采购发票明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInvoiceItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseInvoiceItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购发票明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInvoiceItemDto> UpdatePurchaseInvoiceItemObsoleteAsync(TaktPurchaseInvoiceItemObsoleteDto dto)
    {
        var entity = await _purchaseInvoiceItemRepository.GetByIdAsync(dto.PurchaseInvoiceItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购发票明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购发票明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _purchaseInvoiceItemRepository.UpdateAsync(entity);
        return await GetPurchaseInvoiceItemByIdAsync(dto.PurchaseInvoiceItemId) ?? throw new TaktBusinessException("采购发票明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseInvoiceItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseInvoiceItemTemplateDto>(
            sheetName ?? "采购发票明细导入模板",
            fileName ?? "采购发票明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购发票明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseInvoiceItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseInvoiceItemImportDto>(fileStream, sheetName ?? "采购发票明细导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseInvoiceItem>();
                var importDto = rows[i].Adapt<TaktPurchaseInvoiceItemCreateDto>();
                await StampPurchaseInvoiceItemPurchaseInvoiceAsync(entity, importDto);
                var importKey = $"{entity.PurchaseInvoiceId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchaseInvoiceId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseInvoiceItemRepository,
                    x => x.PurchaseInvoiceId == entity.PurchaseInvoiceId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_procurement_purchase_invoice_item_invoice_line_unique)
                {
                    throw new TaktBusinessException("采购发票明细的PurchaseInvoiceId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _purchaseInvoiceItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseInvoiceId == entity.PurchaseInvoiceId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseInvoiceCode) ? entity.PurchaseInvoiceCode : entity.PurchaseInvoiceId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _purchaseInvoiceItemRepository.CreateAsync(entity);
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
    /// 导出采购发票明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseInvoiceItemAsync(TaktPurchaseInvoiceItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseInvoiceItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInvoiceItemExportDto>(),
                sheetName ?? "采购发票明细数据",
                fileName ?? "采购发票明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseInvoiceItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInvoiceItemExportDto>(),
                sheetName ?? "采购发票明细数据",
                fileName ?? "采购发票明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseInvoiceItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购发票明细数据",
            fileName ?? "采购发票明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步采购发票明细主表外键（ManyToOne → 采购发票）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampPurchaseInvoiceItemPurchaseInvoiceAsync(TaktPurchaseInvoiceItem entity, TaktPurchaseInvoiceItemCreateDto dto)
    {
        if (dto.PurchaseInvoiceId <= 0)
        {
            return;
        }
        var master = await _purchaseInvoiceRepository.GetByIdAsync(dto.PurchaseInvoiceId);
        if (master == null)
        {
            throw new TaktBusinessException("采购发票不存在");
        }
        entity.PurchaseInvoiceId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
        if (string.IsNullOrEmpty(entity.PurchaseInvoiceCode))
        {
            entity.PurchaseInvoiceCode = master.PurchaseInvoiceCode;
        }
        if (string.IsNullOrEmpty(entity.TaxCode))
        {
            entity.TaxCode = master.TaxCode ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.ReferenceCode))
        {
            entity.ReferenceCode = master.ReferenceCode ?? string.Empty;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购发票明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseInvoiceItem, bool>> QueryExpression(TaktPurchaseInvoiceItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseInvoiceItem>();

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
                || (x.PurchaseInvoiceCode != null && x.PurchaseInvoiceCode.Contains(keywords))
                || (x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(keywords))
                || (x.AccountAssignmentSeq != null && x.AccountAssignmentSeq.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.ValuationArea != null && x.ValuationArea.Contains(keywords))
                || (x.DebitCreditIndicator != null && x.DebitCreditIndicator.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.OrderUnit != null && x.OrderUnit.Contains(keywords))
                || (x.PoPriceUnit != null && x.PoPriceUnit.Contains(keywords))
                || (x.BaseUnit != null && x.BaseUnit.Contains(keywords))
                || (x.ValuationClass != null && x.ValuationClass.Contains(keywords))
                || (x.UpdatePoHistoryFlag != null && x.UpdatePoHistoryFlag.Contains(keywords))
                || (x.SubsequentDebitCredit != null && x.SubsequentDebitCredit.Contains(keywords))
                || (x.BlockReasonPrice != null && x.BlockReasonPrice.Contains(keywords))
                || (x.BlockReasonQuantity != null && x.BlockReasonQuantity.Contains(keywords))
                || (x.BlockReasonQuality != null && x.BlockReasonQuality.Contains(keywords))
                || (x.BlockReasonEnhanced != null && x.BlockReasonEnhanced.Contains(keywords))
                || (x.ValueString != null && x.ValueString.Contains(keywords))
                || (x.ReferenceCode != null && x.ReferenceCode.Contains(keywords))
                || (x.ConditionType != null && x.ConditionType.Contains(keywords))
                || (x.ReferenceDocumentCode != null && x.ReferenceDocumentCode.Contains(keywords))
                || (x.ReferenceDocumentYear != null && x.ReferenceDocumentYear.Contains(keywords))
                || (x.StockManagedMaterialCode != null && x.StockManagedMaterialCode.Contains(keywords))
                || (x.ItemText != null && x.ItemText.Contains(keywords))
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

        if (queryDto?.PurchaseInvoiceId.HasValue == true)
        {
            var purchaseInvoiceId = queryDto.PurchaseInvoiceId.Value;
            exp = exp.And(x => x.PurchaseInvoiceId == purchaseInvoiceId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseInvoiceCode))
        {
            var purchaseInvoiceCode = queryDto.PurchaseInvoiceCode;
            exp = exp.And(x => x.PurchaseInvoiceCode != null && x.PurchaseInvoiceCode.Contains(purchaseInvoiceCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseOrderCode))
        {
            var purchaseOrderCode = queryDto.PurchaseOrderCode;
            exp = exp.And(x => x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(purchaseOrderCode));
        }

        if (queryDto?.PurchaseOrderItem.HasValue == true)
        {
            var purchaseOrderItem = queryDto.PurchaseOrderItem.Value;
            exp = exp.And(x => x.PurchaseOrderItem == purchaseOrderItem);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccountAssignmentSeq))
        {
            var accountAssignmentSeq = queryDto.AccountAssignmentSeq;
            exp = exp.And(x => x.AccountAssignmentSeq != null && x.AccountAssignmentSeq.Contains(accountAssignmentSeq));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ValuationArea))
        {
            var valuationArea = queryDto.ValuationArea;
            exp = exp.And(x => x.ValuationArea != null && x.ValuationArea.Contains(valuationArea));
        }

        if (queryDto?.Amount.HasValue == true)
        {
            var amount = queryDto.Amount.Value;
            exp = exp.And(x => x.Amount == amount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DebitCreditIndicator))
        {
            var debitCreditIndicator = queryDto.DebitCreditIndicator;
            exp = exp.And(x => x.DebitCreditIndicator != null && x.DebitCreditIndicator.Contains(debitCreditIndicator));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxCode))
        {
            var taxCode = queryDto.TaxCode;
            exp = exp.And(x => x.TaxCode != null && x.TaxCode.Contains(taxCode));
        }

        if (queryDto?.Quantity.HasValue == true)
        {
            var quantity = queryDto.Quantity.Value;
            exp = exp.And(x => x.Quantity == quantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OrderUnit))
        {
            var orderUnit = queryDto.OrderUnit;
            exp = exp.And(x => x.OrderUnit != null && x.OrderUnit.Contains(orderUnit));
        }

        if (queryDto?.PoPriceQuantity.HasValue == true)
        {
            var poPriceQuantity = queryDto.PoPriceQuantity.Value;
            exp = exp.And(x => x.PoPriceQuantity == poPriceQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PoPriceUnit))
        {
            var poPriceUnit = queryDto.PoPriceUnit;
            exp = exp.And(x => x.PoPriceUnit != null && x.PoPriceUnit.Contains(poPriceUnit));
        }

        if (queryDto?.ValuatedStockQuantity.HasValue == true)
        {
            var valuatedStockQuantity = queryDto.ValuatedStockQuantity.Value;
            exp = exp.And(x => x.ValuatedStockQuantity == valuatedStockQuantity);
        }

        if (queryDto?.PreviousPeriodStock.HasValue == true)
        {
            var previousPeriodStock = queryDto.PreviousPeriodStock.Value;
            exp = exp.And(x => x.PreviousPeriodStock == previousPeriodStock);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BaseUnit))
        {
            var baseUnit = queryDto.BaseUnit;
            exp = exp.And(x => x.BaseUnit != null && x.BaseUnit.Contains(baseUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ValuationClass))
        {
            var valuationClass = queryDto.ValuationClass;
            exp = exp.And(x => x.ValuationClass != null && x.ValuationClass.Contains(valuationClass));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UpdatePoHistoryFlag))
        {
            var updatePoHistoryFlag = queryDto.UpdatePoHistoryFlag;
            exp = exp.And(x => x.UpdatePoHistoryFlag != null && x.UpdatePoHistoryFlag.Contains(updatePoHistoryFlag));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SubsequentDebitCredit))
        {
            var subsequentDebitCredit = queryDto.SubsequentDebitCredit;
            exp = exp.And(x => x.SubsequentDebitCredit != null && x.SubsequentDebitCredit.Contains(subsequentDebitCredit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BlockReasonPrice))
        {
            var blockReasonPrice = queryDto.BlockReasonPrice;
            exp = exp.And(x => x.BlockReasonPrice != null && x.BlockReasonPrice.Contains(blockReasonPrice));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BlockReasonQuantity))
        {
            var blockReasonQuantity = queryDto.BlockReasonQuantity;
            exp = exp.And(x => x.BlockReasonQuantity != null && x.BlockReasonQuantity.Contains(blockReasonQuantity));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BlockReasonQuality))
        {
            var blockReasonQuality = queryDto.BlockReasonQuality;
            exp = exp.And(x => x.BlockReasonQuality != null && x.BlockReasonQuality.Contains(blockReasonQuality));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BlockReasonEnhanced))
        {
            var blockReasonEnhanced = queryDto.BlockReasonEnhanced;
            exp = exp.And(x => x.BlockReasonEnhanced != null && x.BlockReasonEnhanced.Contains(blockReasonEnhanced));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ValueString))
        {
            var valueString = queryDto.ValueString;
            exp = exp.And(x => x.ValueString != null && x.ValueString.Contains(valueString));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReferenceCode))
        {
            var referenceCode = queryDto.ReferenceCode;
            exp = exp.And(x => x.ReferenceCode != null && x.ReferenceCode.Contains(referenceCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConditionType))
        {
            var conditionType = queryDto.ConditionType;
            exp = exp.And(x => x.ConditionType != null && x.ConditionType.Contains(conditionType));
        }

        if (queryDto?.TotalValuatedStockValue.HasValue == true)
        {
            var totalValuatedStockValue = queryDto.TotalValuatedStockValue.Value;
            exp = exp.And(x => x.TotalValuatedStockValue == totalValuatedStockValue);
        }

        if (queryDto?.PreviousPeriodValue.HasValue == true)
        {
            var previousPeriodValue = queryDto.PreviousPeriodValue.Value;
            exp = exp.And(x => x.PreviousPeriodValue == previousPeriodValue);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReferenceDocumentCode))
        {
            var referenceDocumentCode = queryDto.ReferenceDocumentCode;
            exp = exp.And(x => x.ReferenceDocumentCode != null && x.ReferenceDocumentCode.Contains(referenceDocumentCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReferenceDocumentYear))
        {
            var referenceDocumentYear = queryDto.ReferenceDocumentYear;
            exp = exp.And(x => x.ReferenceDocumentYear != null && x.ReferenceDocumentYear.Contains(referenceDocumentYear));
        }

        if (queryDto?.ReferenceDocumentItem.HasValue == true)
        {
            var referenceDocumentItem = queryDto.ReferenceDocumentItem.Value;
            exp = exp.And(x => x.ReferenceDocumentItem == referenceDocumentItem);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.StockManagedMaterialCode))
        {
            var stockManagedMaterialCode = queryDto.StockManagedMaterialCode;
            exp = exp.And(x => x.StockManagedMaterialCode != null && x.StockManagedMaterialCode.Contains(stockManagedMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ItemText))
        {
            var itemText = queryDto.ItemText;
            exp = exp.And(x => x.ItemText != null && x.ItemText.Contains(itemText));
        }

        if (queryDto?.MaterialDocumentItem.HasValue == true)
        {
            var materialDocumentItem = queryDto.MaterialDocumentItem.Value;
            exp = exp.And(x => x.MaterialDocumentItem == materialDocumentItem);
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
    private static bool HasAnyListQueryFilter(TaktPurchaseInvoiceItemQueryDto? queryDto)
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
        if (queryDto.PurchaseInvoiceId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseInvoiceCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseOrderCode))
        {
            return true;
        }
        if (queryDto.PurchaseOrderItem.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccountAssignmentSeq))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ValuationArea))
        {
            return true;
        }
        if (queryDto.Amount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DebitCreditIndicator))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxCode))
        {
            return true;
        }
        if (queryDto.Quantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.OrderUnit))
        {
            return true;
        }
        if (queryDto.PoPriceQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PoPriceUnit))
        {
            return true;
        }
        if (queryDto.ValuatedStockQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.PreviousPeriodStock.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BaseUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ValuationClass))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UpdatePoHistoryFlag))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SubsequentDebitCredit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BlockReasonPrice))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BlockReasonQuantity))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BlockReasonQuality))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BlockReasonEnhanced))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ValueString))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReferenceCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConditionType))
        {
            return true;
        }
        if (queryDto.TotalValuatedStockValue.HasValue)
        {
            return true;
        }
        if (queryDto.PreviousPeriodValue.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReferenceDocumentCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReferenceDocumentYear))
        {
            return true;
        }
        if (queryDto.ReferenceDocumentItem.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.StockManagedMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ItemText))
        {
            return true;
        }
        if (queryDto.MaterialDocumentItem.HasValue)
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
