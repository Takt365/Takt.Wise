// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseRequestItemService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：采购申请明细应用服务实现
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
/// 采购申请明细应用服务
/// </summary>
public class TaktPurchaseRequestItemService : TaktServiceBase, ITaktPurchaseRequestItemService
{
    private readonly ITaktCompanyRepository<TaktPurchaseRequestItem> _purchaseRequestItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseRequestItemRepository">采购申请明细仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseRequestItemService(
        ITaktCompanyRepository<TaktPurchaseRequestItem> purchaseRequestItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseRequestItemRepository = purchaseRequestItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购申请明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseRequestItemDto>> GetPurchaseRequestItemListAsync(TaktPurchaseRequestItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseRequestItemDto>.Create(
                new List<TaktPurchaseRequestItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseRequestItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseRequestItemDto>.Create(
            data.Adapt<List<TaktPurchaseRequestItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购申请明细
    /// </summary>
    /// <param name="id">采购申请明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestItemDto?> GetPurchaseRequestItemByIdAsync(long id)
    {
        var entity = await _purchaseRequestItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPurchaseRequestItemDto>();
    }

    /// <summary>
    /// 获取采购申请明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseRequestItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseRequestItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.MaterialDescription ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseRequestCode,
            DictLabel = e.MaterialDescription ?? e.PurchaseRequestCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购申请明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestItemDto> CreatePurchaseRequestItemAsync(TaktPurchaseRequestItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseRequestItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_procurement_purchase_request_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseRequestItemRepository,
            x => x.PurchaseRequestId == entity.PurchaseRequestId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode);
        if (!isUnique_ix_takt_logistics_procurement_purchase_request_item_request_line_unique)
        {
            throw new TaktBusinessException("采购申请明细的PurchaseRequestId、LineNumber、MaterialCode已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _purchaseRequestItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseRequestId == entity.PurchaseRequestId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseRequestCode) ? entity.PurchaseRequestCode : entity.PurchaseRequestId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _purchaseRequestItemRepository.CreateAsync(entity);
        return await GetPurchaseRequestItemByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseRequestItemDto>();
    }

    /// <summary>
    /// 更新采购申请明细
    /// </summary>
    /// <param name="id">采购申请明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestItemDto> UpdatePurchaseRequestItemAsync(long id, TaktPurchaseRequestItemUpdateDto dto)
    {
        var entity = await _purchaseRequestItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购申请明细不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_request_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseRequestItemRepository,
            x => x.PurchaseRequestId == entity.PurchaseRequestId
                && x.LineNumber == entity.LineNumber
                && x.MaterialCode == entity.MaterialCode,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_request_item_request_line_unique)
        {
            throw new TaktBusinessException("采购申请明细的PurchaseRequestId、LineNumber、MaterialCode已存在");
        }
        await _purchaseRequestItemRepository.UpdateAsync(entity);
        return await GetPurchaseRequestItemByIdAsync(id) ?? throw new TaktBusinessException("采购申请明细不存在");
    }

    /// <summary>
    /// 删除采购申请明细
    /// </summary>
    /// <param name="id">采购申请明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseRequestItemByIdAsync(long id)
    {
        var entity = await _purchaseRequestItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购申请明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购申请明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("采购申请明细已作废");
        }
        entity.IsObsolete = 1;
        await _purchaseRequestItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除采购申请明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseRequestItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseRequestItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购申请明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseRequestItemDto> UpdatePurchaseRequestItemObsoleteAsync(TaktPurchaseRequestItemObsoleteDto dto)
    {
        var entity = await _purchaseRequestItemRepository.GetByIdAsync(dto.PurchaseRequestItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购申请明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购申请明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _purchaseRequestItemRepository.UpdateAsync(entity);
        return await GetPurchaseRequestItemByIdAsync(dto.PurchaseRequestItemId) ?? throw new TaktBusinessException("采购申请明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseRequestItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseRequestItemTemplateDto>(
            sheetName ?? "采购申请明细导入模板",
            fileName ?? "采购申请明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购申请明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseRequestItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseRequestItemImportDto>(fileStream, sheetName ?? "采购申请明细导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseRequestItem>();
                var importKey = $"{entity.PurchaseRequestId}|{entity.LineNumber}|{entity.MaterialCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchaseRequestId、LineNumber、MaterialCode）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_request_item_request_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseRequestItemRepository,
                    x => x.PurchaseRequestId == entity.PurchaseRequestId
                        && x.LineNumber == entity.LineNumber
                        && x.MaterialCode == entity.MaterialCode);
                if (!isUnique_ix_takt_logistics_procurement_purchase_request_item_request_line_unique)
                {
                    throw new TaktBusinessException("采购申请明细的PurchaseRequestId、LineNumber、MaterialCode已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _purchaseRequestItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PurchaseRequestId == entity.PurchaseRequestId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseRequestCode) ? entity.PurchaseRequestCode : entity.PurchaseRequestId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _purchaseRequestItemRepository.CreateAsync(entity);
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
    /// 导出采购申请明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseRequestItemAsync(TaktPurchaseRequestItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseRequestItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseRequestItemExportDto>(),
                sheetName ?? "采购申请明细数据",
                fileName ?? "采购申请明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseRequestItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseRequestItemExportDto>(),
                sheetName ?? "采购申请明细数据",
                fileName ?? "采购申请明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseRequestItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购申请明细数据",
            fileName ?? "采购申请明细导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购申请明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseRequestItem, bool>> QueryExpression(TaktPurchaseRequestItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseRequestItem>();

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
                || (x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(keywords))
                || (x.AllocationCategory != null && x.AllocationCategory.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.RequestUnit != null && x.RequestUnit.Contains(keywords))
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

        if (queryDto?.PurchaseRequestId.HasValue == true)
        {
            var purchaseRequestId = queryDto.PurchaseRequestId.Value;
            exp = exp.And(x => x.PurchaseRequestId == purchaseRequestId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseRequestCode))
        {
            var purchaseRequestCode = queryDto.PurchaseRequestCode;
            exp = exp.And(x => x.PurchaseRequestCode != null && x.PurchaseRequestCode.Contains(purchaseRequestCode));
        }

        if (queryDto?.PurchasePlanItemId.HasValue == true)
        {
            var purchasePlanItemId = queryDto.PurchasePlanItemId.Value;
            exp = exp.And(x => x.PurchasePlanItemId == purchasePlanItemId);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AllocationCategory))
        {
            var allocationCategory = queryDto.AllocationCategory;
            exp = exp.And(x => x.AllocationCategory != null && x.AllocationCategory.Contains(allocationCategory));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.RequestUnit))
        {
            var requestUnit = queryDto.RequestUnit;
            exp = exp.And(x => x.RequestUnit != null && x.RequestUnit.Contains(requestUnit));
        }

        if (queryDto?.RequestQuantity.HasValue == true)
        {
            var requestQuantity = queryDto.RequestQuantity.Value;
            exp = exp.And(x => x.RequestQuantity == requestQuantity);
        }

        if (queryDto?.ConvertedQuantity.HasValue == true)
        {
            var convertedQuantity = queryDto.ConvertedQuantity.Value;
            exp = exp.And(x => x.ConvertedQuantity == convertedQuantity);
        }

        if (queryDto?.PurchasePerUnit.HasValue == true)
        {
            var purchasePerUnit = queryDto.PurchasePerUnit.Value;
            exp = exp.And(x => x.PurchasePerUnit == purchasePerUnit);
        }

        if (queryDto?.PurchaseRequestUnitPrice.HasValue == true)
        {
            var purchaseRequestUnitPrice = queryDto.PurchaseRequestUnitPrice.Value;
            exp = exp.And(x => x.PurchaseRequestUnitPrice == purchaseRequestUnitPrice);
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

        if (queryDto?.RequestAmount.HasValue == true)
        {
            var requestAmount = queryDto.RequestAmount.Value;
            exp = exp.And(x => x.RequestAmount == requestAmount);
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
    private static bool HasAnyListQueryFilter(TaktPurchaseRequestItemQueryDto? queryDto)
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
        if (queryDto.PurchaseRequestId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseRequestCode))
        {
            return true;
        }
        if (queryDto.PurchasePlanItemId.HasValue)
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AllocationCategory))
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
        if (!string.IsNullOrWhiteSpace(queryDto.RequestUnit))
        {
            return true;
        }
        if (queryDto.RequestQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.ConvertedQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.PurchasePerUnit.HasValue)
        {
            return true;
        }
        if (queryDto.PurchaseRequestUnitPrice.HasValue)
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
        if (queryDto.RequestAmount.HasValue)
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
