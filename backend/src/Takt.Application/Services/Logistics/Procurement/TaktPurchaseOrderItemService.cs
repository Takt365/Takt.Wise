// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseOrderItemService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：采购订单明细应用服务实现
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
/// 采购订单明细应用服务
/// </summary>
public class TaktPurchaseOrderItemService : TaktServiceBase, ITaktPurchaseOrderItemService
{
    private readonly ITaktCompanyRepository<TaktPurchaseOrderItem> _purchaseOrderItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseOrderItemRepository">采购订单明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseOrderItemService(
        ITaktCompanyRepository<TaktPurchaseOrderItem> purchaseOrderItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购订单明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseOrderItemDto>> GetPurchaseOrderItemListAsync(TaktPurchaseOrderItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseOrderItemDto>.Create(
                new List<TaktPurchaseOrderItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseOrderItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseOrderItemDto>.Create(
            data.Adapt<List<TaktPurchaseOrderItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购订单明细
    /// </summary>
    /// <param name="id">采购订单明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderItemDto?> GetPurchaseOrderItemByIdAsync(long id)
    {
        var entity = await _purchaseOrderItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchaseOrderItemDto>();
    }

    /// <summary>
    /// 获取采购订单明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseOrderItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseOrderItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DeliveryStatus == 1 && x.IsObsolete == 0,
            x => x.MaterialDescription ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseOrderCode,
            DictLabel = e.MaterialDescription ?? e.PurchaseOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购订单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderItemDto> CreatePurchaseOrderItemAsync(TaktPurchaseOrderItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseOrderItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_procurement_purchase_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseOrderItemRepository,
            x => x.PurchaseOrderId == entity.PurchaseOrderId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_procurement_purchase_order_item_order_line_unique)
        {
            throw new TaktBusinessException("采购订单明细的PurchaseOrderId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _purchaseOrderItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseOrderId == entity.PurchaseOrderId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseOrderCode) ? entity.PurchaseOrderCode : entity.PurchaseOrderId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _purchaseOrderItemRepository.CreateAsync(entity);
        return await GetPurchaseOrderItemByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseOrderItemDto>();
    }

    /// <summary>
    /// 更新采购订单明细
    /// </summary>
    /// <param name="id">采购订单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderItemDto> UpdatePurchaseOrderItemAsync(long id, TaktPurchaseOrderItemUpdateDto dto)
    {
        var entity = await _purchaseOrderItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购订单明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseOrderItemRepository,
            x => x.PurchaseOrderId == entity.PurchaseOrderId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_order_item_order_line_unique)
        {
            throw new TaktBusinessException("采购订单明细的PurchaseOrderId、LineNumber、MaterialCode已存在");
        }
        await _purchaseOrderItemRepository.UpdateAsync(entity);
        return await GetPurchaseOrderItemByIdAsync(id) ?? throw new TaktBusinessException("采购订单明细不存在");
    }

    /// <summary>
    /// 删除采购订单明细
    /// </summary>
    /// <param name="id">采购订单明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseOrderItemByIdAsync(long id)
    {
        var entity = await _purchaseOrderItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购订单明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购订单明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("采购订单明细已作废");
        }
        entity.IsObsolete = 1;
        await _purchaseOrderItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除采购订单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseOrderItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseOrderItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购订单明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderItemDto> UpdatePurchaseOrderItemStatusAsync(TaktPurchaseOrderItemStatusDto dto)
    {
        var entity = await _purchaseOrderItemRepository.GetByIdAsync(dto.PurchaseOrderItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购订单明细不存在");
        }
        entity.DeliveryStatus = dto.DeliveryStatus;
        await _purchaseOrderItemRepository.UpdateAsync(entity);
        return await GetPurchaseOrderItemByIdAsync(dto.PurchaseOrderItemId) ?? throw new TaktBusinessException("采购订单明细不存在");
    }

    /// <summary>
    /// 更新采购订单明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseOrderItemDto> UpdatePurchaseOrderItemObsoleteAsync(TaktPurchaseOrderItemObsoleteDto dto)
    {
        var entity = await _purchaseOrderItemRepository.GetByIdAsync(dto.PurchaseOrderItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购订单明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购订单明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _purchaseOrderItemRepository.UpdateAsync(entity);
        return await GetPurchaseOrderItemByIdAsync(dto.PurchaseOrderItemId) ?? throw new TaktBusinessException("采购订单明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseOrderItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseOrderItemTemplateDto>(
            sheetName ?? "采购订单明细导入模板",
            fileName ?? "采购订单明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购订单明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseOrderItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseOrderItemImportDto>(fileStream, sheetName ?? "采购订单明细导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseOrderItem>();
                var importKey = $"{entity.PurchaseOrderId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchaseOrderId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseOrderItemRepository,
                    x => x.PurchaseOrderId == entity.PurchaseOrderId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_procurement_purchase_order_item_order_line_unique)
                {
                    throw new TaktBusinessException("采购订单明细的PurchaseOrderId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _purchaseOrderItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseOrderId == entity.PurchaseOrderId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseOrderCode) ? entity.PurchaseOrderCode : entity.PurchaseOrderId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _purchaseOrderItemRepository.CreateAsync(entity);
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
    /// 导出采购订单明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseOrderItemAsync(TaktPurchaseOrderItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseOrderItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseOrderItemExportDto>(),
                sheetName ?? "采购订单明细数据",
                fileName ?? "采购订单明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseOrderItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseOrderItemExportDto>(),
                sheetName ?? "采购订单明细数据",
                fileName ?? "采购订单明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseOrderItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购订单明细数据",
            fileName ?? "采购订单明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购订单明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseOrderItem, bool>> QueryExpression(TaktPurchaseOrderItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseOrderItem>();

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
                || (x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(keywords))
                || (x.RequestCode != null && x.RequestCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.PurchaseInfoRecordCode != null && x.PurchaseInfoRecordCode.Contains(keywords))
                || (x.SupplierMaterialCode != null && x.SupplierMaterialCode.Contains(keywords))
                || (x.PurchaseUnit != null && x.PurchaseUnit.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.WeightUnit != null && x.WeightUnit.Contains(keywords))
                || (x.VolumeUnit != null && x.VolumeUnit.Contains(keywords))
                || (x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(keywords))
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

        if (queryDto?.PurchaseOrderId.HasValue == true)
        {
            var purchaseOrderId = queryDto.PurchaseOrderId.Value;
            exp = exp.And(x => x.PurchaseOrderId == purchaseOrderId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseOrderCode))
        {
            var purchaseOrderCode = queryDto.PurchaseOrderCode;
            exp = exp.And(x => x.PurchaseOrderCode != null && x.PurchaseOrderCode.Contains(purchaseOrderCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RequestCode))
        {
            var requestCode = queryDto.RequestCode;
            exp = exp.And(x => x.RequestCode != null && x.RequestCode.Contains(requestCode));
        }

        if (queryDto?.RequestLineNumber.HasValue == true)
        {
            var requestLineNumber = queryDto.RequestLineNumber.Value;
            exp = exp.And(x => x.RequestLineNumber == requestLineNumber);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialSpecification))
        {
            var materialSpecification = queryDto.MaterialSpecification;
            exp = exp.And(x => x.MaterialSpecification != null && x.MaterialSpecification.Contains(materialSpecification));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseInfoRecordCode))
        {
            var purchaseInfoRecordCode = queryDto.PurchaseInfoRecordCode;
            exp = exp.And(x => x.PurchaseInfoRecordCode != null && x.PurchaseInfoRecordCode.Contains(purchaseInfoRecordCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierMaterialCode))
        {
            var supplierMaterialCode = queryDto.SupplierMaterialCode;
            exp = exp.And(x => x.SupplierMaterialCode != null && x.SupplierMaterialCode.Contains(supplierMaterialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseUnit))
        {
            var purchaseUnit = queryDto.PurchaseUnit;
            exp = exp.And(x => x.PurchaseUnit != null && x.PurchaseUnit.Contains(purchaseUnit));
        }

        if (queryDto?.OrderQuantity.HasValue == true)
        {
            var orderQuantity = queryDto.OrderQuantity.Value;
            exp = exp.And(x => x.OrderQuantity == orderQuantity);
        }

        if (queryDto?.ReceivedQuantity.HasValue == true)
        {
            var receivedQuantity = queryDto.ReceivedQuantity.Value;
            exp = exp.And(x => x.ReceivedQuantity == receivedQuantity);
        }

        if (queryDto?.PurchasePerUnit.HasValue == true)
        {
            var purchasePerUnit = queryDto.PurchasePerUnit.Value;
            exp = exp.And(x => x.PurchasePerUnit == purchasePerUnit);
        }

        if (queryDto?.PurchaseUnitPrice.HasValue == true)
        {
            var purchaseUnitPrice = queryDto.PurchaseUnitPrice.Value;
            exp = exp.And(x => x.PurchaseUnitPrice == purchaseUnitPrice);
        }

        if (queryDto?.DiscountRate.HasValue == true)
        {
            var discountRate = queryDto.DiscountRate.Value;
            exp = exp.And(x => x.DiscountRate == discountRate);
        }

        if (queryDto?.DiscountAmount.HasValue == true)
        {
            var discountAmount = queryDto.DiscountAmount.Value;
            exp = exp.And(x => x.DiscountAmount == discountAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxCode))
        {
            var taxCode = queryDto.TaxCode;
            exp = exp.And(x => x.TaxCode != null && x.TaxCode.Contains(taxCode));
        }

        if (queryDto?.TaxIncludedAmount.HasValue == true)
        {
            var taxIncludedAmount = queryDto.TaxIncludedAmount.Value;
            exp = exp.And(x => x.TaxIncludedAmount == taxIncludedAmount);
        }

        if (queryDto?.UntaxedAmount.HasValue == true)
        {
            var untaxedAmount = queryDto.UntaxedAmount.Value;
            exp = exp.And(x => x.UntaxedAmount == untaxedAmount);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            var taxAmount = queryDto.TaxAmount.Value;
            exp = exp.And(x => x.TaxAmount == taxAmount);
        }

        if (queryDto?.PurchaseAmount.HasValue == true)
        {
            var purchaseAmount = queryDto.PurchaseAmount.Value;
            exp = exp.And(x => x.PurchaseAmount == purchaseAmount);
        }

        if (queryDto?.GrossWeight.HasValue == true)
        {
            var grossWeight = queryDto.GrossWeight.Value;
            exp = exp.And(x => x.GrossWeight == grossWeight);
        }

        if (queryDto?.NetWeight.HasValue == true)
        {
            var netWeight = queryDto.NetWeight.Value;
            exp = exp.And(x => x.NetWeight == netWeight);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WeightUnit))
        {
            var weightUnit = queryDto.WeightUnit;
            exp = exp.And(x => x.WeightUnit != null && x.WeightUnit.Contains(weightUnit));
        }

        if (queryDto?.Volume.HasValue == true)
        {
            var volume = queryDto.Volume.Value;
            exp = exp.And(x => x.Volume == volume);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.VolumeUnit))
        {
            var volumeUnit = queryDto.VolumeUnit;
            exp = exp.And(x => x.VolumeUnit != null && x.VolumeUnit.Contains(volumeUnit));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProfitCenterCode))
        {
            var profitCenterCode = queryDto.ProfitCenterCode;
            exp = exp.And(x => x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(profitCenterCode));
        }

        if (queryDto?.DeliveryStatus.HasValue == true)
        {
            var deliveryStatus = queryDto.DeliveryStatus.Value;
            exp = exp.And(x => x.DeliveryStatus == deliveryStatus);
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
    private static bool HasAnyListQueryFilter(TaktPurchaseOrderItemQueryDto? queryDto)
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
        if (queryDto.PurchaseOrderId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseOrderCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RequestCode))
        {
            return true;
        }
        if (queryDto.RequestLineNumber.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialSpecification))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseInfoRecordCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierMaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseUnit))
        {
            return true;
        }
        if (queryDto.OrderQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.ReceivedQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.PurchasePerUnit.HasValue)
        {
            return true;
        }
        if (queryDto.PurchaseUnitPrice.HasValue)
        {
            return true;
        }
        if (queryDto.DiscountRate.HasValue)
        {
            return true;
        }
        if (queryDto.DiscountAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxCode))
        {
            return true;
        }
        if (queryDto.TaxIncludedAmount.HasValue)
        {
            return true;
        }
        if (queryDto.UntaxedAmount.HasValue)
        {
            return true;
        }
        if (queryDto.TaxAmount.HasValue)
        {
            return true;
        }
        if (queryDto.PurchaseAmount.HasValue)
        {
            return true;
        }
        if (queryDto.GrossWeight.HasValue)
        {
            return true;
        }
        if (queryDto.NetWeight.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WeightUnit))
        {
            return true;
        }
        if (queryDto.Volume.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VolumeUnit))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProfitCenterCode))
        {
            return true;
        }
        if (queryDto.DeliveryStatus.HasValue)
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
