// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesQuotationService.cs
// 创建时间：2026-09-04
// 创建人：Takt365(Cursor AI)
// 功能描述：销售报价应用服务实现
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
/// 销售报价应用服务
/// </summary>
public class TaktSalesQuotationService : TaktServiceBase, ITaktSalesQuotationService
{
    private readonly ITaktCompanyRepository<TaktSalesQuotation> _salesQuotationRepository;
    private readonly ITaktCompanyRepository<TaktSalesQuotationItem> _salesQuotationItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesQuotationRepository">销售报价仓储</param>
    /// <param name="salesQuotationItemRepository">SalesQuotationItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesQuotationService(
        ITaktCompanyRepository<TaktSalesQuotation> salesQuotationRepository,
        ITaktCompanyRepository<TaktSalesQuotationItem> salesQuotationItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesQuotationRepository = salesQuotationRepository;
        _salesQuotationItemRepository = salesQuotationItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售报价列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesQuotationDto>> GetSalesQuotationListAsync(TaktSalesQuotationQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesQuotationDto>.Create(
                new List<TaktSalesQuotationDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesQuotationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesQuotationDto>.Create(
            data.Adapt<List<TaktSalesQuotationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售报价
    /// </summary>
    /// <param name="id">销售报价ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesQuotationDto?> GetSalesQuotationByIdAsync(long id)
    {
        var entity = await _salesQuotationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesQuotationDto>();
        await FillSalesQuotationDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售报价选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesQuotationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesQuotationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.QuotationStatus == 1,
            x => x.SalesQuotationCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesQuotationCode,
            DictLabel = e.SalesQuotationCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售报价
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesQuotationDto> CreateSalesQuotationAsync(TaktSalesQuotationCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesQuotation>();
        var isUnique_ix_takt_logistics_sales_quotation_code_unique = await _uniqueValidator.IsUniqueAsync(
            _salesQuotationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesQuotationCode == entity.SalesQuotationCode);
        if (!isUnique_ix_takt_logistics_sales_quotation_code_unique)
        {
            throw new TaktBusinessException("销售报价的PlantCode、SalesQuotationCode已存在");
        }
        entity = await _salesQuotationRepository.CreateAsync(entity);
                await SaveSalesQuotationChildrenAsync(entity, dto);
        return await GetSalesQuotationByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesQuotationDto>();
    }

    /// <summary>
    /// 更新销售报价
    /// </summary>
    /// <param name="id">销售报价ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesQuotationDto> UpdateSalesQuotationAsync(long id, TaktSalesQuotationUpdateDto dto)
    {
        var entity = await _salesQuotationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售报价不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_quotation_code_unique = await _uniqueValidator.IsUniqueAsync(
            _salesQuotationRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesQuotationCode == entity.SalesQuotationCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_quotation_code_unique)
        {
            throw new TaktBusinessException("销售报价的PlantCode、SalesQuotationCode已存在");
        }
        await _salesQuotationRepository.UpdateAsync(entity);
                await SaveSalesQuotationChildrenAsync(entity, dto);
        return await GetSalesQuotationByIdAsync(id) ?? throw new TaktBusinessException("销售报价不存在");
    }

    /// <summary>
    /// 删除销售报价
    /// </summary>
    /// <param name="id">销售报价ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesQuotationByIdAsync(long id)
    {
        var entity = await _salesQuotationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售报价不存在或已删除");
        }
        await _salesQuotationItemRepository.DeleteAsync(x => x.SalesQuotationId == entity.Id);
        var deleted = await _salesQuotationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售报价不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售报价
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesQuotationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesQuotationByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售报价状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesQuotationDto> UpdateSalesQuotationStatusAsync(TaktSalesQuotationStatusDto dto)
    {
        var entity = await _salesQuotationRepository.GetByIdAsync(dto.SalesQuotationId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售报价不存在");
        }
        entity.QuotationStatus = dto.QuotationStatus;
        await _salesQuotationRepository.UpdateAsync(entity);
        return await GetSalesQuotationByIdAsync(dto.SalesQuotationId) ?? throw new TaktBusinessException("销售报价不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesQuotationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesQuotationTemplateDto>(
            sheetName ?? "销售报价导入模板",
            fileName ?? "销售报价导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售报价
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesQuotationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesQuotationImportDto>(fileStream, sheetName ?? "销售报价导入模板");
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
                var entity = rows[i].Adapt<TaktSalesQuotation>();
                var importKey = $"{entity.PlantCode}|{entity.SalesQuotationCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SalesQuotationCode）");
                }
                var isUnique_ix_takt_logistics_sales_quotation_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesQuotationRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SalesQuotationCode == entity.SalesQuotationCode);
                if (!isUnique_ix_takt_logistics_sales_quotation_code_unique)
                {
                    throw new TaktBusinessException("销售报价的PlantCode、SalesQuotationCode已存在");
                }
                await _salesQuotationRepository.CreateAsync(entity);
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
    /// 导出销售报价
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesQuotationAsync(TaktSalesQuotationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesQuotationQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesQuotationExportDto>(),
                sheetName ?? "销售报价数据",
                fileName ?? "销售报价导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesQuotationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesQuotationExportDto>(),
                sheetName ?? "销售报价数据",
                fileName ?? "销售报价导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesQuotationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售报价数据",
            fileName ?? "销售报价导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废销售报价明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesQuotationId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesQuotationItemsObsoleteAsync(long salesQuotationId)
    {
        if (salesQuotationId <= 0)
        {
            return;
        }
        var rows = await _salesQuotationItemRepository.GetListAsync(
            x => x.SalesQuotationId == salesQuotationId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesQuotationItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充销售报价详情（加载 OneToMany 子表：销售报价明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesQuotationDetailsAsync(TaktSalesQuotationDto dto, TaktSalesQuotation entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售报价明细 → dto.Items（含作废行）
        var items = await _salesQuotationItemRepository.GetListAsync(x => x.SalesQuotationId == entity.Id);
        dto.Items = items.Adapt<List<TaktSalesQuotationItemDto>>();
    }

    /// <summary>
    /// 保存销售报价子表级联（销售报价明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesQuotationChildrenAsync(TaktSalesQuotation entity, TaktSalesQuotationCreateDto dto)
    {
        // 销售报价明细（Items）
        List<TaktSalesQuotationItemUpdateDto>? itemsForSave;
        if (dto is TaktSalesQuotationUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktSalesQuotationItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkSalesQuotationItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _salesQuotationItemRepository.GetListAsync(x => x.SalesQuotationId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesQuotationItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.SalesQuotationId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.SalesQuotationCode = entity.SalesQuotationCode;
                childDto.TaxCode = entity.TaxCode ?? string.Empty;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("销售报价明细第{i + 1}项与本次提交的其他项重复（CompanyCode、SalesQuotationId、LineNumber）");
                }
                if (childDto.SalesQuotationItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesQuotationItemId, out var target))
                    {
                        throw new TaktBusinessException("销售报价明细不存在（SalesQuotationItemId={childDto.SalesQuotationItemId}）");
                    }
                    if (target.SalesQuotationId != entity.Id)
                    {
                        throw new TaktBusinessException("销售报价明细不属于当前主表（SalesQuotationItemId={childDto.SalesQuotationItemId}）");
                    }
                    submittedIds.Add(childDto.SalesQuotationItemId);
                    var isUniqueUpdate_ix_takt_logistics_sales_quotation_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _salesQuotationItemRepository,
                        x => x.SalesQuotationId == x.SalesQuotationId
                && x.LineNumber == x.LineNumber,
                        childDto.SalesQuotationItemId);
                    if (!isUniqueUpdate_ix_takt_logistics_sales_quotation_item_line_unique)
                    {
                        throw new TaktBusinessException("销售报价明细的SalesQuotationId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.SalesQuotationItemId;
                    target.SalesQuotationId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesQuotationItemRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_sales_quotation_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _salesQuotationItemRepository,
                        x => x.SalesQuotationId == x.SalesQuotationId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_sales_quotation_item_line_unique)
                    {
                        throw new TaktBusinessException("销售报价明细的SalesQuotationId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktSalesQuotationItem>();
                    child.Id = 0;
                    child.SalesQuotationId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesQuotationItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SalesQuotationCode) ? entity.SalesQuotationCode : entity.Id.ToString();
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
                await _salesQuotationItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售报价查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesQuotation, bool>> QueryExpression(TaktSalesQuotationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesQuotation>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SalesQuotationCode != null && x.SalesQuotationCode.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.CustomerName1 != null && x.CustomerName1.Contains(keywords))
                || (x.SalesGroup != null && x.SalesGroup.Contains(keywords))
                || (x.SalesQuotationType != null && x.SalesQuotationType.Contains(keywords))
                || (x.PricingProcedure != null && x.PricingProcedure.Contains(keywords))
                || (x.PricingConditionCode != null && x.PricingConditionCode.Contains(keywords))
                || (x.CurrencyCode != null && x.CurrencyCode.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.SalesOrderCode != null && x.SalesOrderCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesQuotationCode))
        {
            var salesQuotationCode = queryDto.SalesQuotationCode;
            exp = exp.And(x => x.SalesQuotationCode != null && x.SalesQuotationCode.Contains(salesQuotationCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerCode))
        {
            var customerCode = queryDto.CustomerCode;
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(customerCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerName1))
        {
            var customerName1 = queryDto.CustomerName1;
            exp = exp.And(x => x.CustomerName1 != null && x.CustomerName1.Contains(customerName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesGroup))
        {
            var salesGroup = queryDto.SalesGroup;
            exp = exp.And(x => x.SalesGroup != null && x.SalesGroup.Contains(salesGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesQuotationType))
        {
            var salesQuotationType = queryDto.SalesQuotationType;
            exp = exp.And(x => x.SalesQuotationType != null && x.SalesQuotationType.Contains(salesQuotationType));
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

        if (queryDto?.DiscountAmount.HasValue == true)
        {
            var discountAmount = queryDto.DiscountAmount.Value;
            exp = exp.And(x => x.DiscountAmount == discountAmount);
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

        if (queryDto?.ActualAmount.HasValue == true)
        {
            var actualAmount = queryDto.ActualAmount.Value;
            exp = exp.And(x => x.ActualAmount == actualAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesOrderCode))
        {
            var salesOrderCode = queryDto.SalesOrderCode;
            exp = exp.And(x => x.SalesOrderCode != null && x.SalesOrderCode.Contains(salesOrderCode));
        }

        if (queryDto?.QuotationStatus.HasValue == true)
        {
            var quotationStatus = queryDto.QuotationStatus.Value;
            exp = exp.And(x => x.QuotationStatus == quotationStatus);
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

        if (queryDto?.QuotationDateStart.HasValue == true)
        {
            var quotationDateStart = queryDto.QuotationDateStart.Value;
            exp = exp.And(x => x.QuotationDate >= quotationDateStart);
        }

        if (queryDto?.QuotationDateEnd.HasValue == true)
        {
            var quotationDateEnd = queryDto.QuotationDateEnd.Value;
            exp = exp.And(x => x.QuotationDate <= quotationDateEnd);
        }

        if (queryDto?.ValidUntilDateStart.HasValue == true)
        {
            var validUntilDateStart = queryDto.ValidUntilDateStart.Value;
            exp = exp.And(x => x.ValidUntilDate >= validUntilDateStart);
        }

        if (queryDto?.ValidUntilDateEnd.HasValue == true)
        {
            var validUntilDateEnd = queryDto.ValidUntilDateEnd.Value;
            exp = exp.And(x => x.ValidUntilDate <= validUntilDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktSalesQuotationQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SalesQuotationCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesQuotationType))
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
        if (queryDto.TotalQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.TotalAmount.HasValue)
        {
            return true;
        }
        if (queryDto.DiscountAmount.HasValue)
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
        if (queryDto.ActualAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesOrderCode))
        {
            return true;
        }
        if (queryDto.QuotationStatus.HasValue)
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
        if (queryDto.QuotationDateStart.HasValue || queryDto.QuotationDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ValidUntilDateStart.HasValue || queryDto.ValidUntilDateEnd.HasValue)
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
