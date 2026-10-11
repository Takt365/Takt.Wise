// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesInvoiceItemService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：销售发票明细应用服务实现
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
/// 销售发票明细应用服务
/// </summary>
public class TaktSalesInvoiceItemService : TaktServiceBase, ITaktSalesInvoiceItemService
{
    private readonly ITaktCompanyRepository<TaktSalesInvoiceItem> _salesInvoiceItemRepository;
    private readonly ITaktCompanyRepository<TaktSalesInvoice> _salesInvoiceRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesInvoiceItemRepository">销售发票明细仓储</param>
    /// <param name="salesInvoiceRepository">销售发票仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesInvoiceItemService(
        ITaktCompanyRepository<TaktSalesInvoiceItem> salesInvoiceItemRepository,
        ITaktCompanyRepository<TaktSalesInvoice> salesInvoiceRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesInvoiceItemRepository = salesInvoiceItemRepository;
        _salesInvoiceRepository = salesInvoiceRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售发票明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesInvoiceItemDto>> GetSalesInvoiceItemListAsync(TaktSalesInvoiceItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesInvoiceItemDto>.Create(
                new List<TaktSalesInvoiceItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesInvoiceItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesInvoiceItemDto>.Create(
            data.Adapt<List<TaktSalesInvoiceItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售发票明细
    /// </summary>
    /// <param name="id">销售发票明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceItemDto?> GetSalesInvoiceItemByIdAsync(long id)
    {
        var entity = await _salesInvoiceItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalesInvoiceItemDto>();
    }

    /// <summary>
    /// 获取销售发票明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesInvoiceItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesInvoiceItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.BillingDocumentCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.BillingDocumentCode,
            DictLabel = e.BillingDocumentCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售发票明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceItemDto> CreateSalesInvoiceItemAsync(TaktSalesInvoiceItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesInvoiceItem>();
        entity.IsObsolete = 0;
        await StampSalesInvoiceItemSalesInvoiceAsync(entity, dto);
        var isUnique_ix_takt_logistics_sales_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesInvoiceItemRepository,
            x => x.SalesInvoiceId == entity.SalesInvoiceId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_sales_invoice_item_invoice_line_unique)
        {
            throw new TaktBusinessException("销售发票明细的SalesInvoiceId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _salesInvoiceItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesInvoiceId == entity.SalesInvoiceId,
                x => x.LineNumber);
            var businessCode = entity.SalesInvoiceId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _salesInvoiceItemRepository.CreateAsync(entity);
        return await GetSalesInvoiceItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesInvoiceItemDto>();
    }

    /// <summary>
    /// 更新销售发票明细
    /// </summary>
    /// <param name="id">销售发票明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceItemDto> UpdateSalesInvoiceItemAsync(long id, TaktSalesInvoiceItemUpdateDto dto)
    {
        var entity = await _salesInvoiceItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售发票明细不存在");
        }
        dto.Adapt(entity);
        await StampSalesInvoiceItemSalesInvoiceAsync(entity, dto);
        var isUnique_ix_takt_logistics_sales_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesInvoiceItemRepository,
            x => x.SalesInvoiceId == entity.SalesInvoiceId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_sales_invoice_item_invoice_line_unique)
        {
            throw new TaktBusinessException("销售发票明细的SalesInvoiceId、LineNumber已存在");
        }
        await _salesInvoiceItemRepository.UpdateAsync(entity);
        return await GetSalesInvoiceItemByIdAsync(id) ?? throw new TaktBusinessException("销售发票明细不存在");
    }

    /// <summary>
    /// 删除销售发票明细
    /// </summary>
    /// <param name="id">销售发票明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesInvoiceItemByIdAsync(long id)
    {
        var entity = await _salesInvoiceItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售发票明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售发票明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("销售发票明细已作废");
        }
        entity.IsObsolete = 1;
        await _salesInvoiceItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除销售发票明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesInvoiceItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesInvoiceItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售发票明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesInvoiceItemDto> UpdateSalesInvoiceItemObsoleteAsync(TaktSalesInvoiceItemObsoleteDto dto)
    {
        var entity = await _salesInvoiceItemRepository.GetByIdAsync(dto.SalesInvoiceItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售发票明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售发票明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _salesInvoiceItemRepository.UpdateAsync(entity);
        return await GetSalesInvoiceItemByIdAsync(dto.SalesInvoiceItemId) ?? throw new TaktBusinessException("销售发票明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesInvoiceItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesInvoiceItemTemplateDto>(
            sheetName ?? "销售发票明细导入模板",
            fileName ?? "销售发票明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售发票明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesInvoiceItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesInvoiceItemImportDto>(fileStream, sheetName ?? "销售发票明细导入模板");
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
                var entity = rows[i].Adapt<TaktSalesInvoiceItem>();
                var importDto = rows[i].Adapt<TaktSalesInvoiceItemCreateDto>();
                await StampSalesInvoiceItemSalesInvoiceAsync(entity, importDto);
                var importKey = $"{entity.SalesInvoiceId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SalesInvoiceId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_sales_invoice_item_invoice_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesInvoiceItemRepository,
                    x => x.SalesInvoiceId == entity.SalesInvoiceId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_sales_invoice_item_invoice_line_unique)
                {
                    throw new TaktBusinessException("销售发票明细的SalesInvoiceId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _salesInvoiceItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesInvoiceId == entity.SalesInvoiceId,
                        x => x.LineNumber);
                    var businessCode = entity.SalesInvoiceId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _salesInvoiceItemRepository.CreateAsync(entity);
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
    /// 导出销售发票明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesInvoiceItemAsync(TaktSalesInvoiceItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesInvoiceItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesInvoiceItemExportDto>(),
                sheetName ?? "销售发票明细数据",
                fileName ?? "销售发票明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesInvoiceItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesInvoiceItemExportDto>(),
                sheetName ?? "销售发票明细数据",
                fileName ?? "销售发票明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesInvoiceItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售发票明细数据",
            fileName ?? "销售发票明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步销售发票明细主表外键（ManyToOne → 销售发票）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSalesInvoiceItemSalesInvoiceAsync(TaktSalesInvoiceItem entity, TaktSalesInvoiceItemCreateDto dto)
    {
        if (dto.SalesInvoiceId <= 0)
        {
            return;
        }
        var master = await _salesInvoiceRepository.GetByIdAsync(dto.SalesInvoiceId);
        if (master == null)
        {
            throw new TaktBusinessException("销售发票不存在");
        }
        entity.SalesInvoiceId = master.Id;
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
        if (string.IsNullOrEmpty(entity.BillingDocumentCode))
        {
            entity.BillingDocumentCode = master.BillingDocumentCode;
        }
        if (string.IsNullOrEmpty(entity.Division))
        {
            entity.Division = master.Division ?? string.Empty;
        }
        if (string.IsNullOrEmpty(entity.DocumentCategory))
        {
            entity.DocumentCategory = master.DocumentCategory ?? string.Empty;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售发票明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesInvoiceItem, bool>> QueryExpression(TaktSalesInvoiceItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesInvoiceItem>();

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
                || (x.BillingDocumentCode != null && x.BillingDocumentCode.Contains(keywords))
                || (x.SalesUnit != null && x.SalesUnit.Contains(keywords))
                || (x.BaseUnit != null && x.BaseUnit.Contains(keywords))
                || (x.WeightUnit != null && x.WeightUnit.Contains(keywords))
                || (x.BusinessAreaCode != null && x.BusinessAreaCode.Contains(keywords))
                || (x.ReferenceDocumentCode != null && x.ReferenceDocumentCode.Contains(keywords))
                || (x.ReferenceDocumentCategory != null && x.ReferenceDocumentCategory.Contains(keywords))
                || (x.SalesDocumentCode != null && x.SalesDocumentCode.Contains(keywords))
                || (x.SalesDocumentReferenceFlag != null && x.SalesDocumentReferenceFlag.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.PricingReferenceMaterialCode != null && x.PricingReferenceMaterialCode.Contains(keywords))
                || (x.BatchCode != null && x.BatchCode.Contains(keywords))
                || (x.MaterialGroup != null && x.MaterialGroup.Contains(keywords))
                || (x.SalesItemCategory != null && x.SalesItemCategory.Contains(keywords))
                || (x.ProductHierarchy != null && x.ProductHierarchy.Contains(keywords))
                || (x.ShippingPoint != null && x.ShippingPoint.Contains(keywords))
                || (x.Division != null && x.Division.Contains(keywords))
                || (x.DepartureCountry != null && x.DepartureCountry.Contains(keywords))
                || (x.PlantRegion != null && x.PlantRegion.Contains(keywords))
                || (x.PricingFlag != null && x.PricingFlag.Contains(keywords))
                || (x.WarehouseCode != null && x.WarehouseCode.Contains(keywords))
                || (x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(keywords))
                || (x.CustomerGroupSalesOrder != null && x.CustomerGroupSalesOrder.Contains(keywords))
                || (x.DestinationCountryOrder != null && x.DestinationCountryOrder.Contains(keywords))
                || (x.RegionOrder != null && x.RegionOrder.Contains(keywords))
                || (x.SalesOrganizationOrder != null && x.SalesOrganizationOrder.Contains(keywords))
                || (x.DistributionChannelOrder != null && x.DistributionChannelOrder.Contains(keywords))
                || (x.DocumentCategory != null && x.DocumentCategory.Contains(keywords))
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

        if (queryDto?.SalesInvoiceId.HasValue == true)
        {
            var salesInvoiceId = queryDto.SalesInvoiceId.Value;
            exp = exp.And(x => x.SalesInvoiceId == salesInvoiceId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BillingDocumentCode))
        {
            var billingDocumentCode = queryDto.BillingDocumentCode;
            exp = exp.And(x => x.BillingDocumentCode != null && x.BillingDocumentCode.Contains(billingDocumentCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.BillingQuantity.HasValue == true)
        {
            var billingQuantity = queryDto.BillingQuantity.Value;
            exp = exp.And(x => x.BillingQuantity == billingQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesUnit))
        {
            var salesUnit = queryDto.SalesUnit;
            exp = exp.And(x => x.SalesUnit != null && x.SalesUnit.Contains(salesUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BaseUnit))
        {
            var baseUnit = queryDto.BaseUnit;
            exp = exp.And(x => x.BaseUnit != null && x.BaseUnit.Contains(baseUnit));
        }

        if (queryDto?.ScaleQuantity.HasValue == true)
        {
            var scaleQuantity = queryDto.ScaleQuantity.Value;
            exp = exp.And(x => x.ScaleQuantity == scaleQuantity);
        }

        if (queryDto?.BillingQuantitySku.HasValue == true)
        {
            var billingQuantitySku = queryDto.BillingQuantitySku.Value;
            exp = exp.And(x => x.BillingQuantitySku == billingQuantitySku);
        }

        if (queryDto?.NetWeight.HasValue == true)
        {
            var netWeight = queryDto.NetWeight.Value;
            exp = exp.And(x => x.NetWeight == netWeight);
        }

        if (queryDto?.GrossWeight.HasValue == true)
        {
            var grossWeight = queryDto.GrossWeight.Value;
            exp = exp.And(x => x.GrossWeight == grossWeight);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WeightUnit))
        {
            var weightUnit = queryDto.WeightUnit;
            exp = exp.And(x => x.WeightUnit != null && x.WeightUnit.Contains(weightUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BusinessAreaCode))
        {
            var businessAreaCode = queryDto.BusinessAreaCode;
            exp = exp.And(x => x.BusinessAreaCode != null && x.BusinessAreaCode.Contains(businessAreaCode));
        }

        if (queryDto?.PricingExchangeRate.HasValue == true)
        {
            var pricingExchangeRate = queryDto.PricingExchangeRate.Value;
            exp = exp.And(x => x.PricingExchangeRate == pricingExchangeRate);
        }

        if (queryDto?.NetAmount.HasValue == true)
        {
            var netAmount = queryDto.NetAmount.Value;
            exp = exp.And(x => x.NetAmount == netAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReferenceDocumentCode))
        {
            var referenceDocumentCode = queryDto.ReferenceDocumentCode;
            exp = exp.And(x => x.ReferenceDocumentCode != null && x.ReferenceDocumentCode.Contains(referenceDocumentCode));
        }

        if (queryDto?.ReferenceDocumentItem.HasValue == true)
        {
            var referenceDocumentItem = queryDto.ReferenceDocumentItem.Value;
            exp = exp.And(x => x.ReferenceDocumentItem == referenceDocumentItem);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ReferenceDocumentCategory))
        {
            var referenceDocumentCategory = queryDto.ReferenceDocumentCategory;
            exp = exp.And(x => x.ReferenceDocumentCategory != null && x.ReferenceDocumentCategory.Contains(referenceDocumentCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesDocumentCode))
        {
            var salesDocumentCode = queryDto.SalesDocumentCode;
            exp = exp.And(x => x.SalesDocumentCode != null && x.SalesDocumentCode.Contains(salesDocumentCode));
        }

        if (queryDto?.SalesDocumentItem.HasValue == true)
        {
            var salesDocumentItem = queryDto.SalesDocumentItem.Value;
            exp = exp.And(x => x.SalesDocumentItem == salesDocumentItem);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesDocumentReferenceFlag))
        {
            var salesDocumentReferenceFlag = queryDto.SalesDocumentReferenceFlag;
            exp = exp.And(x => x.SalesDocumentReferenceFlag != null && x.SalesDocumentReferenceFlag.Contains(salesDocumentReferenceFlag));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialDescription))
        {
            var materialDescription = queryDto.MaterialDescription;
            exp = exp.And(x => x.MaterialDescription != null && x.MaterialDescription.Contains(materialDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PricingReferenceMaterialCode))
        {
            var pricingReferenceMaterialCode = queryDto.PricingReferenceMaterialCode;
            exp = exp.And(x => x.PricingReferenceMaterialCode != null && x.PricingReferenceMaterialCode.Contains(pricingReferenceMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BatchCode))
        {
            var batchCode = queryDto.BatchCode;
            exp = exp.And(x => x.BatchCode != null && x.BatchCode.Contains(batchCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialGroup))
        {
            var materialGroup = queryDto.MaterialGroup;
            exp = exp.And(x => x.MaterialGroup != null && x.MaterialGroup.Contains(materialGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesItemCategory))
        {
            var salesItemCategory = queryDto.SalesItemCategory;
            exp = exp.And(x => x.SalesItemCategory != null && x.SalesItemCategory.Contains(salesItemCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductHierarchy))
        {
            var productHierarchy = queryDto.ProductHierarchy;
            exp = exp.And(x => x.ProductHierarchy != null && x.ProductHierarchy.Contains(productHierarchy));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ShippingPoint))
        {
            var shippingPoint = queryDto.ShippingPoint;
            exp = exp.And(x => x.ShippingPoint != null && x.ShippingPoint.Contains(shippingPoint));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Division))
        {
            var division = queryDto.Division;
            exp = exp.And(x => x.Division != null && x.Division.Contains(division));
        }

        if (queryDto?.PartnerItem.HasValue == true)
        {
            var partnerItem = queryDto.PartnerItem.Value;
            exp = exp.And(x => x.PartnerItem == partnerItem);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DepartureCountry))
        {
            var departureCountry = queryDto.DepartureCountry;
            exp = exp.And(x => x.DepartureCountry != null && x.DepartureCountry.Contains(departureCountry));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlantRegion))
        {
            var plantRegion = queryDto.PlantRegion;
            exp = exp.And(x => x.PlantRegion != null && x.PlantRegion.Contains(plantRegion));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PricingFlag))
        {
            var pricingFlag = queryDto.PricingFlag;
            exp = exp.And(x => x.PricingFlag != null && x.PricingFlag.Contains(pricingFlag));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WarehouseCode))
        {
            var warehouseCode = queryDto.WarehouseCode;
            exp = exp.And(x => x.WarehouseCode != null && x.WarehouseCode.Contains(warehouseCode));
        }

        if (queryDto?.CostAmount.HasValue == true)
        {
            var costAmount = queryDto.CostAmount.Value;
            exp = exp.And(x => x.CostAmount == costAmount);
        }

        if (queryDto?.Subtotal1.HasValue == true)
        {
            var subtotal1 = queryDto.Subtotal1.Value;
            exp = exp.And(x => x.Subtotal1 == subtotal1);
        }

        if (queryDto?.Subtotal2.HasValue == true)
        {
            var subtotal2 = queryDto.Subtotal2.Value;
            exp = exp.And(x => x.Subtotal2 == subtotal2);
        }

        if (queryDto?.Subtotal3.HasValue == true)
        {
            var subtotal3 = queryDto.Subtotal3.Value;
            exp = exp.And(x => x.Subtotal3 == subtotal3);
        }

        if (queryDto?.Subtotal4.HasValue == true)
        {
            var subtotal4 = queryDto.Subtotal4.Value;
            exp = exp.And(x => x.Subtotal4 == subtotal4);
        }

        if (queryDto?.Subtotal5.HasValue == true)
        {
            var subtotal5 = queryDto.Subtotal5.Value;
            exp = exp.And(x => x.Subtotal5 == subtotal5);
        }

        if (queryDto?.Subtotal6.HasValue == true)
        {
            var subtotal6 = queryDto.Subtotal6.Value;
            exp = exp.And(x => x.Subtotal6 == subtotal6);
        }

        if (queryDto?.StatisticsExchangeRate.HasValue == true)
        {
            var statisticsExchangeRate = queryDto.StatisticsExchangeRate.Value;
            exp = exp.And(x => x.StatisticsExchangeRate == statisticsExchangeRate);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProfitCenterCode))
        {
            var profitCenterCode = queryDto.ProfitCenterCode;
            exp = exp.And(x => x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(profitCenterCode));
        }

        if (queryDto?.CreditPrice.HasValue == true)
        {
            var creditPrice = queryDto.CreditPrice.Value;
            exp = exp.And(x => x.CreditPrice == creditPrice);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerGroupSalesOrder))
        {
            var customerGroupSalesOrder = queryDto.CustomerGroupSalesOrder;
            exp = exp.And(x => x.CustomerGroupSalesOrder != null && x.CustomerGroupSalesOrder.Contains(customerGroupSalesOrder));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DestinationCountryOrder))
        {
            var destinationCountryOrder = queryDto.DestinationCountryOrder;
            exp = exp.And(x => x.DestinationCountryOrder != null && x.DestinationCountryOrder.Contains(destinationCountryOrder));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RegionOrder))
        {
            var regionOrder = queryDto.RegionOrder;
            exp = exp.And(x => x.RegionOrder != null && x.RegionOrder.Contains(regionOrder));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesOrganizationOrder))
        {
            var salesOrganizationOrder = queryDto.SalesOrganizationOrder;
            exp = exp.And(x => x.SalesOrganizationOrder != null && x.SalesOrganizationOrder.Contains(salesOrganizationOrder));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DistributionChannelOrder))
        {
            var distributionChannelOrder = queryDto.DistributionChannelOrder;
            exp = exp.And(x => x.DistributionChannelOrder != null && x.DistributionChannelOrder.Contains(distributionChannelOrder));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentCategory))
        {
            var documentCategory = queryDto.DocumentCategory;
            exp = exp.And(x => x.DocumentCategory != null && x.DocumentCategory.Contains(documentCategory));
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            var taxAmount = queryDto.TaxAmount.Value;
            exp = exp.And(x => x.TaxAmount == taxAmount);
        }

        if (queryDto?.GrossAmount.HasValue == true)
        {
            var grossAmount = queryDto.GrossAmount.Value;
            exp = exp.And(x => x.GrossAmount == grossAmount);
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

        if (queryDto?.PricingDateStart.HasValue == true)
        {
            var pricingDateStart = queryDto.PricingDateStart.Value;
            exp = exp.And(x => x.PricingDate >= pricingDateStart);
        }

        if (queryDto?.PricingDateEnd.HasValue == true)
        {
            var pricingDateEnd = queryDto.PricingDateEnd.Value;
            exp = exp.And(x => x.PricingDate <= pricingDateEnd);
        }

        if (queryDto?.ServiceRenderedDateStart.HasValue == true)
        {
            var serviceRenderedDateStart = queryDto.ServiceRenderedDateStart.Value;
            exp = exp.And(x => x.ServiceRenderedDate >= serviceRenderedDateStart);
        }

        if (queryDto?.ServiceRenderedDateEnd.HasValue == true)
        {
            var serviceRenderedDateEnd = queryDto.ServiceRenderedDateEnd.Value;
            exp = exp.And(x => x.ServiceRenderedDate <= serviceRenderedDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktSalesInvoiceItemQueryDto? queryDto)
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
        if (queryDto.SalesInvoiceId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BillingDocumentCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.BillingQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BaseUnit))
        {
            return true;
        }
        if (queryDto.ScaleQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.BillingQuantitySku.HasValue)
        {
            return true;
        }
        if (queryDto.NetWeight.HasValue)
        {
            return true;
        }
        if (queryDto.GrossWeight.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WeightUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BusinessAreaCode))
        {
            return true;
        }
        if (queryDto.PricingExchangeRate.HasValue)
        {
            return true;
        }
        if (queryDto.NetAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReferenceDocumentCode))
        {
            return true;
        }
        if (queryDto.ReferenceDocumentItem.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ReferenceDocumentCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesDocumentCode))
        {
            return true;
        }
        if (queryDto.SalesDocumentItem.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesDocumentReferenceFlag))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PricingReferenceMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BatchCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesItemCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductHierarchy))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ShippingPoint))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Division))
        {
            return true;
        }
        if (queryDto.PartnerItem.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DepartureCountry))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlantRegion))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PricingFlag))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WarehouseCode))
        {
            return true;
        }
        if (queryDto.CostAmount.HasValue)
        {
            return true;
        }
        if (queryDto.Subtotal1.HasValue)
        {
            return true;
        }
        if (queryDto.Subtotal2.HasValue)
        {
            return true;
        }
        if (queryDto.Subtotal3.HasValue)
        {
            return true;
        }
        if (queryDto.Subtotal4.HasValue)
        {
            return true;
        }
        if (queryDto.Subtotal5.HasValue)
        {
            return true;
        }
        if (queryDto.Subtotal6.HasValue)
        {
            return true;
        }
        if (queryDto.StatisticsExchangeRate.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProfitCenterCode))
        {
            return true;
        }
        if (queryDto.CreditPrice.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerGroupSalesOrder))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DestinationCountryOrder))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RegionOrder))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesOrganizationOrder))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DistributionChannelOrder))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentCategory))
        {
            return true;
        }
        if (queryDto.TaxAmount.HasValue)
        {
            return true;
        }
        if (queryDto.GrossAmount.HasValue)
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
        if (queryDto.PricingDateStart.HasValue || queryDto.PricingDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ServiceRenderedDateStart.HasValue || queryDto.ServiceRenderedDateEnd.HasValue)
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
