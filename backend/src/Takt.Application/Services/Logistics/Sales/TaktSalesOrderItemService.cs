// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesOrderItemService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：销售订单明细应用服务实现
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
/// 销售订单明细应用服务
/// </summary>
public class TaktSalesOrderItemService : TaktServiceBase, ITaktSalesOrderItemService
{
    private readonly ITaktCompanyRepository<TaktSalesOrderItem> _salesOrderItemRepository;
    private readonly ITaktCompanyRepository<TaktSalesOrder> _salesOrderRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesOrderItemRepository">销售订单明细仓储</param>
    /// <param name="salesOrderRepository">销售订单仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesOrderItemService(
        ITaktCompanyRepository<TaktSalesOrderItem> salesOrderItemRepository,
        ITaktCompanyRepository<TaktSalesOrder> salesOrderRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesOrderItemRepository = salesOrderItemRepository;
        _salesOrderRepository = salesOrderRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售订单明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesOrderItemDto>> GetSalesOrderItemListAsync(TaktSalesOrderItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesOrderItemDto>.Create(
                new List<TaktSalesOrderItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesOrderItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesOrderItemDto>.Create(
            data.Adapt<List<TaktSalesOrderItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售订单明细
    /// </summary>
    /// <param name="id">销售订单明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderItemDto?> GetSalesOrderItemByIdAsync(long id)
    {
        var entity = await _salesOrderItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalesOrderItemDto>();
    }

    /// <summary>
    /// 获取销售订单明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesOrderItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesOrderItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DeliveryStatus == 1 && x.IsObsolete == 0,
            x => x.MaterialDescription ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesOrderCode,
            DictLabel = e.MaterialDescription ?? e.SalesOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售订单明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderItemDto> CreateSalesOrderItemAsync(TaktSalesOrderItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesOrderItem>();
        entity.IsObsolete = 0;
        await StampSalesOrderItemSalesOrderAsync(entity, dto);
        var isUnique_ix_takt_logistics_sales_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesOrderItemRepository,
            x => x.SalesOrderId == entity.SalesOrderId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_sales_order_item_order_line_unique)
        {
            throw new TaktBusinessException("销售订单明细的SalesOrderId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _salesOrderItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesOrderId == entity.SalesOrderId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.SalesOrderCode) ? entity.SalesOrderCode : entity.SalesOrderId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _salesOrderItemRepository.CreateAsync(entity);
        return await GetSalesOrderItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesOrderItemDto>();
    }

    /// <summary>
    /// 更新销售订单明细
    /// </summary>
    /// <param name="id">销售订单明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderItemDto> UpdateSalesOrderItemAsync(long id, TaktSalesOrderItemUpdateDto dto)
    {
        var entity = await _salesOrderItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售订单明细不存在");
        }
        dto.Adapt(entity);
        await StampSalesOrderItemSalesOrderAsync(entity, dto);
        var isUnique_ix_takt_logistics_sales_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesOrderItemRepository,
            x => x.SalesOrderId == entity.SalesOrderId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_sales_order_item_order_line_unique)
        {
            throw new TaktBusinessException("销售订单明细的SalesOrderId、LineNumber已存在");
        }
        await _salesOrderItemRepository.UpdateAsync(entity);
        return await GetSalesOrderItemByIdAsync(id) ?? throw new TaktBusinessException("销售订单明细不存在");
    }

    /// <summary>
    /// 删除销售订单明细
    /// </summary>
    /// <param name="id">销售订单明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesOrderItemByIdAsync(long id)
    {
        var entity = await _salesOrderItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售订单明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售订单明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("销售订单明细已作废");
        }
        entity.IsObsolete = 1;
        await _salesOrderItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除销售订单明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesOrderItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesOrderItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售订单明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderItemDto> UpdateSalesOrderItemStatusAsync(TaktSalesOrderItemStatusDto dto)
    {
        var entity = await _salesOrderItemRepository.GetByIdAsync(dto.SalesOrderItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售订单明细不存在");
        }
        entity.DeliveryStatus = dto.DeliveryStatus;
        await _salesOrderItemRepository.UpdateAsync(entity);
        return await GetSalesOrderItemByIdAsync(dto.SalesOrderItemId) ?? throw new TaktBusinessException("销售订单明细不存在");
    }

    /// <summary>
    /// 更新销售订单明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesOrderItemDto> UpdateSalesOrderItemObsoleteAsync(TaktSalesOrderItemObsoleteDto dto)
    {
        var entity = await _salesOrderItemRepository.GetByIdAsync(dto.SalesOrderItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售订单明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售订单明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _salesOrderItemRepository.UpdateAsync(entity);
        return await GetSalesOrderItemByIdAsync(dto.SalesOrderItemId) ?? throw new TaktBusinessException("销售订单明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesOrderItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesOrderItemTemplateDto>(
            sheetName ?? "销售订单明细导入模板",
            fileName ?? "销售订单明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售订单明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesOrderItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesOrderItemImportDto>(fileStream, sheetName ?? "销售订单明细导入模板");
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
                var entity = rows[i].Adapt<TaktSalesOrderItem>();
                var importDto = rows[i].Adapt<TaktSalesOrderItemCreateDto>();
                await StampSalesOrderItemSalesOrderAsync(entity, importDto);
                var importKey = $"{entity.SalesOrderId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SalesOrderId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_sales_order_item_order_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesOrderItemRepository,
                    x => x.SalesOrderId == entity.SalesOrderId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_sales_order_item_order_line_unique)
                {
                    throw new TaktBusinessException("销售订单明细的SalesOrderId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _salesOrderItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesOrderId == entity.SalesOrderId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SalesOrderCode) ? entity.SalesOrderCode : entity.SalesOrderId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _salesOrderItemRepository.CreateAsync(entity);
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
    /// 导出销售订单明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesOrderItemAsync(TaktSalesOrderItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesOrderItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesOrderItemExportDto>(),
                sheetName ?? "销售订单明细数据",
                fileName ?? "销售订单明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesOrderItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesOrderItemExportDto>(),
                sheetName ?? "销售订单明细数据",
                fileName ?? "销售订单明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesOrderItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售订单明细数据",
            fileName ?? "销售订单明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步销售订单明细主表外键（ManyToOne → 销售订单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSalesOrderItemSalesOrderAsync(TaktSalesOrderItem entity, TaktSalesOrderItemCreateDto dto)
    {
        if (dto.SalesOrderId <= 0)
        {
            return;
        }
        var master = await _salesOrderRepository.GetByIdAsync(dto.SalesOrderId);
        if (master == null)
        {
            throw new TaktBusinessException("销售订单不存在");
        }
        entity.SalesOrderId = master.Id;
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
        if (string.IsNullOrEmpty(entity.SalesOrderCode))
        {
            entity.SalesOrderCode = master.SalesOrderCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售订单明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesOrderItem, bool>> QueryExpression(TaktSalesOrderItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesOrderItem>();

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
                || (x.SalesOrderCode != null && x.SalesOrderCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.SalesUnit != null && x.SalesUnit.Contains(keywords))
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

        if (queryDto?.SalesOrderId.HasValue == true)
        {
            var salesOrderId = queryDto.SalesOrderId.Value;
            exp = exp.And(x => x.SalesOrderId == salesOrderId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesOrderCode))
        {
            var salesOrderCode = queryDto.SalesOrderCode;
            exp = exp.And(x => x.SalesOrderCode != null && x.SalesOrderCode.Contains(salesOrderCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesUnit))
        {
            var salesUnit = queryDto.SalesUnit;
            exp = exp.And(x => x.SalesUnit != null && x.SalesUnit.Contains(salesUnit));
        }

        if (queryDto?.OrderQuantity.HasValue == true)
        {
            var orderQuantity = queryDto.OrderQuantity.Value;
            exp = exp.And(x => x.OrderQuantity == orderQuantity);
        }

        if (queryDto?.TargetQuantity.HasValue == true)
        {
            var targetQuantity = queryDto.TargetQuantity.Value;
            exp = exp.And(x => x.TargetQuantity == targetQuantity);
        }

        if (queryDto?.ShippedQuantity.HasValue == true)
        {
            var shippedQuantity = queryDto.ShippedQuantity.Value;
            exp = exp.And(x => x.ShippedQuantity == shippedQuantity);
        }

        if (queryDto?.SalesPerUnit.HasValue == true)
        {
            var salesPerUnit = queryDto.SalesPerUnit.Value;
            exp = exp.And(x => x.SalesPerUnit == salesPerUnit);
        }

        if (queryDto?.SalesUnitPrice.HasValue == true)
        {
            var salesUnitPrice = queryDto.SalesUnitPrice.Value;
            exp = exp.And(x => x.SalesUnitPrice == salesUnitPrice);
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

        if (queryDto?.SalesAmount.HasValue == true)
        {
            var salesAmount = queryDto.SalesAmount.Value;
            exp = exp.And(x => x.SalesAmount == salesAmount);
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
    private static bool HasAnyListQueryFilter(TaktSalesOrderItemQueryDto? queryDto)
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
        if (queryDto.SalesOrderId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesOrderCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SalesUnit))
        {
            return true;
        }
        if (queryDto.OrderQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.TargetQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.ShippedQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.SalesPerUnit.HasValue)
        {
            return true;
        }
        if (queryDto.SalesUnitPrice.HasValue)
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
        if (queryDto.SalesAmount.HasValue)
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
