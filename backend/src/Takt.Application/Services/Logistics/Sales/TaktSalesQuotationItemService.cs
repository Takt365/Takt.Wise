// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesQuotationItemService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：销售报价明细应用服务实现
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
/// 销售报价明细应用服务
/// </summary>
public class TaktSalesQuotationItemService : TaktServiceBase, ITaktSalesQuotationItemService
{
    private readonly ITaktCompanyRepository<TaktSalesQuotationItem> _salesQuotationItemRepository;
    private readonly ITaktCompanyRepository<TaktSalesQuotation> _salesQuotationRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesQuotationItemRepository">销售报价明细仓储</param>
    /// <param name="salesQuotationRepository">销售报价仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesQuotationItemService(
        ITaktCompanyRepository<TaktSalesQuotationItem> salesQuotationItemRepository,
        ITaktCompanyRepository<TaktSalesQuotation> salesQuotationRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesQuotationItemRepository = salesQuotationItemRepository;
        _salesQuotationRepository = salesQuotationRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售报价明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesQuotationItemDto>> GetSalesQuotationItemListAsync(TaktSalesQuotationItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesQuotationItemDto>.Create(
                new List<TaktSalesQuotationItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesQuotationItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesQuotationItemDto>.Create(
            data.Adapt<List<TaktSalesQuotationItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售报价明细
    /// </summary>
    /// <param name="id">销售报价明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesQuotationItemDto?> GetSalesQuotationItemByIdAsync(long id)
    {
        var entity = await _salesQuotationItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSalesQuotationItemDto>();
    }

    /// <summary>
    /// 获取销售报价明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesQuotationItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesQuotationItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.MaterialDescription ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesQuotationCode,
            DictLabel = e.MaterialDescription ?? e.SalesQuotationCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售报价明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesQuotationItemDto> CreateSalesQuotationItemAsync(TaktSalesQuotationItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesQuotationItem>();
        entity.IsObsolete = 0;
        await StampSalesQuotationItemSalesQuotationAsync(entity, dto);
        var isUnique_ix_takt_logistics_sales_quotation_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesQuotationItemRepository,
            x => x.SalesQuotationId == entity.SalesQuotationId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_sales_quotation_item_line_unique)
        {
            throw new TaktBusinessException("销售报价明细的SalesQuotationId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _salesQuotationItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesQuotationId == entity.SalesQuotationId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.SalesQuotationCode) ? entity.SalesQuotationCode : entity.SalesQuotationId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _salesQuotationItemRepository.CreateAsync(entity);
        return await GetSalesQuotationItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesQuotationItemDto>();
    }

    /// <summary>
    /// 更新销售报价明细
    /// </summary>
    /// <param name="id">销售报价明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesQuotationItemDto> UpdateSalesQuotationItemAsync(long id, TaktSalesQuotationItemUpdateDto dto)
    {
        var entity = await _salesQuotationItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售报价明细不存在");
        }
        dto.Adapt(entity);
        await StampSalesQuotationItemSalesQuotationAsync(entity, dto);
        var isUnique_ix_takt_logistics_sales_quotation_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _salesQuotationItemRepository,
            x => x.SalesQuotationId == entity.SalesQuotationId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_sales_quotation_item_line_unique)
        {
            throw new TaktBusinessException("销售报价明细的SalesQuotationId、LineNumber已存在");
        }
        await _salesQuotationItemRepository.UpdateAsync(entity);
        return await GetSalesQuotationItemByIdAsync(id) ?? throw new TaktBusinessException("销售报价明细不存在");
    }

    /// <summary>
    /// 删除销售报价明细
    /// </summary>
    /// <param name="id">销售报价明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesQuotationItemByIdAsync(long id)
    {
        var entity = await _salesQuotationItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售报价明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售报价明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("销售报价明细已作废");
        }
        entity.IsObsolete = 1;
        await _salesQuotationItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除销售报价明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesQuotationItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesQuotationItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售报价明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesQuotationItemDto> UpdateSalesQuotationItemObsoleteAsync(TaktSalesQuotationItemObsoleteDto dto)
    {
        var entity = await _salesQuotationItemRepository.GetByIdAsync(dto.SalesQuotationItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售报价明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售报价明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _salesQuotationItemRepository.UpdateAsync(entity);
        return await GetSalesQuotationItemByIdAsync(dto.SalesQuotationItemId) ?? throw new TaktBusinessException("销售报价明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesQuotationItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesQuotationItemTemplateDto>(
            sheetName ?? "销售报价明细导入模板",
            fileName ?? "销售报价明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售报价明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesQuotationItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesQuotationItemImportDto>(fileStream, sheetName ?? "销售报价明细导入模板");
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
                var entity = rows[i].Adapt<TaktSalesQuotationItem>();
                var importDto = rows[i].Adapt<TaktSalesQuotationItemCreateDto>();
                await StampSalesQuotationItemSalesQuotationAsync(entity, importDto);
                var importKey = $"{entity.SalesQuotationId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SalesQuotationId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_sales_quotation_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesQuotationItemRepository,
                    x => x.SalesQuotationId == entity.SalesQuotationId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_sales_quotation_item_line_unique)
                {
                    throw new TaktBusinessException("销售报价明细的SalesQuotationId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _salesQuotationItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.SalesQuotationId == entity.SalesQuotationId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SalesQuotationCode) ? entity.SalesQuotationCode : entity.SalesQuotationId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _salesQuotationItemRepository.CreateAsync(entity);
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
    /// 导出销售报价明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesQuotationItemAsync(TaktSalesQuotationItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesQuotationItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesQuotationItemExportDto>(),
                sheetName ?? "销售报价明细数据",
                fileName ?? "销售报价明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesQuotationItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesQuotationItemExportDto>(),
                sheetName ?? "销售报价明细数据",
                fileName ?? "销售报价明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesQuotationItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售报价明细数据",
            fileName ?? "销售报价明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步销售报价明细主表外键（ManyToOne → 销售报价）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSalesQuotationItemSalesQuotationAsync(TaktSalesQuotationItem entity, TaktSalesQuotationItemCreateDto dto)
    {
        if (dto.SalesQuotationId <= 0)
        {
            return;
        }
        var master = await _salesQuotationRepository.GetByIdAsync(dto.SalesQuotationId);
        if (master == null)
        {
            throw new TaktBusinessException("销售报价不存在");
        }
        entity.SalesQuotationId = master.Id;
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
        if (string.IsNullOrEmpty(entity.SalesQuotationCode))
        {
            entity.SalesQuotationCode = master.SalesQuotationCode;
        }
        if (string.IsNullOrEmpty(entity.TaxCode))
        {
            entity.TaxCode = master.TaxCode ?? string.Empty;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售报价明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesQuotationItem, bool>> QueryExpression(TaktSalesQuotationItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesQuotationItem>();

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
                || (x.SalesQuotationCode != null && x.SalesQuotationCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.MaterialSpecification != null && x.MaterialSpecification.Contains(keywords))
                || (x.SalesUnit != null && x.SalesUnit.Contains(keywords))
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

        if (queryDto?.SalesQuotationId.HasValue == true)
        {
            var salesQuotationId = queryDto.SalesQuotationId.Value;
            exp = exp.And(x => x.SalesQuotationId == salesQuotationId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesQuotationCode))
        {
            var salesQuotationCode = queryDto.SalesQuotationCode;
            exp = exp.And(x => x.SalesQuotationCode != null && x.SalesQuotationCode.Contains(salesQuotationCode));
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

        if (queryDto?.QuotationQuantity.HasValue == true)
        {
            var quotationQuantity = queryDto.QuotationQuantity.Value;
            exp = exp.And(x => x.QuotationQuantity == quotationQuantity);
        }

        if (queryDto?.SalesPerUnit.HasValue == true)
        {
            var salesPerUnit = queryDto.SalesPerUnit.Value;
            exp = exp.And(x => x.SalesPerUnit == salesPerUnit);
        }

        if (queryDto?.QuotationUnitPrice.HasValue == true)
        {
            var quotationUnitPrice = queryDto.QuotationUnitPrice.Value;
            exp = exp.And(x => x.QuotationUnitPrice == quotationUnitPrice);
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

        if (queryDto?.QuotationAmount.HasValue == true)
        {
            var quotationAmount = queryDto.QuotationAmount.Value;
            exp = exp.And(x => x.QuotationAmount == quotationAmount);
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
    private static bool HasAnyListQueryFilter(TaktSalesQuotationItemQueryDto? queryDto)
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
        if (queryDto.SalesQuotationId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesQuotationCode))
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
        if (queryDto.QuotationQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.SalesPerUnit.HasValue)
        {
            return true;
        }
        if (queryDto.QuotationUnitPrice.HasValue)
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
        if (queryDto.QuotationAmount.HasValue)
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
