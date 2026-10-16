// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesConditionService.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：销售条件应用服务实现
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
/// 销售条件应用服务
/// </summary>
public class TaktSalesConditionService : TaktServiceBase, ITaktSalesConditionService
{
    private readonly ITaktCompanyRepository<TaktSalesCondition> _salesConditionRepository;
    private readonly ITaktCompanyRepository<TaktSalesConditionItem> _salesConditionItemRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesConditionRepository">销售条件仓储</param>
    /// <param name="salesConditionItemRepository">SalesConditionItem仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesConditionService(
        ITaktCompanyRepository<TaktSalesCondition> salesConditionRepository,
        ITaktCompanyRepository<TaktSalesConditionItem> salesConditionItemRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesConditionRepository = salesConditionRepository;
        _salesConditionItemRepository = salesConditionItemRepository;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取销售条件列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesConditionDto>> GetSalesConditionListAsync(TaktSalesConditionQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesConditionDto>.Create(
                new List<TaktSalesConditionDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesConditionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesConditionDto>.Create(
            data.Adapt<List<TaktSalesConditionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售条件
    /// </summary>
    /// <param name="id">销售条件ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionDto?> GetSalesConditionByIdAsync(long id)
    {
        var entity = await _salesConditionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesConditionDto>();
        await FillSalesConditionDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesConditionOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesConditionRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SalesConditionCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesConditionCode,
            DictLabel = e.SalesConditionCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售条件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionDto> CreateSalesConditionAsync(TaktSalesConditionCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesCondition>();
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var generated = await _numberingGenerator.GenerateNextAsync(dto.NumberingRuleCode.Trim());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.SalesConditionCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.SalesConditionCode))
        {
            throw new TaktBusinessException("定价记录号不能为空");
        }
        var isUnique_ix_takt_logistics_sales_condition_code_unique = await _uniqueValidator.IsUniqueAsync(
            _salesConditionRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesConditionCode == entity.SalesConditionCode);
        if (!isUnique_ix_takt_logistics_sales_condition_code_unique)
        {
            throw new TaktBusinessException("销售条件的PlantCode、SalesConditionCode已存在");
        }
        entity = await _salesConditionRepository.CreateAsync(entity);
                await SaveSalesConditionChildrenAsync(entity, dto);
        return await GetSalesConditionByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesConditionDto>();
    }

    /// <summary>
    /// 更新销售条件
    /// </summary>
    /// <param name="id">销售条件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionDto> UpdateSalesConditionAsync(long id, TaktSalesConditionUpdateDto dto)
    {
        var entity = await _salesConditionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售条件不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_condition_code_unique = await _uniqueValidator.IsUniqueAsync(
            _salesConditionRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesConditionCode == entity.SalesConditionCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_condition_code_unique)
        {
            throw new TaktBusinessException("销售条件的PlantCode、SalesConditionCode已存在");
        }
        await _salesConditionRepository.UpdateAsync(entity);
                await SaveSalesConditionChildrenAsync(entity, dto);
        return await GetSalesConditionByIdAsync(id) ?? throw new TaktBusinessException("销售条件不存在");
    }

    /// <summary>
    /// 删除销售条件
    /// </summary>
    /// <param name="id">销售条件ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesConditionByIdAsync(long id)
    {
        var entity = await _salesConditionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售条件不存在或已删除");
        }
        await _salesConditionItemRepository.DeleteAsync(x => x.SalesConditionId == entity.Id);
        var deleted = await _salesConditionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售条件不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售条件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesConditionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesConditionByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesConditionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesConditionTemplateDto>(
            sheetName ?? "销售条件导入模板",
            fileName ?? "销售条件导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售条件
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesConditionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesConditionImportDto>(fileStream, sheetName ?? "销售条件导入模板");
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
                var entity = rows[i].Adapt<TaktSalesCondition>();
                var importKey = $"{entity.PlantCode}|{entity.SalesConditionCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SalesConditionCode）");
                }
                var isUnique_ix_takt_logistics_sales_condition_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesConditionRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SalesConditionCode == entity.SalesConditionCode);
                if (!isUnique_ix_takt_logistics_sales_condition_code_unique)
                {
                    throw new TaktBusinessException("销售条件的PlantCode、SalesConditionCode已存在");
                }
                await _salesConditionRepository.CreateAsync(entity);
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
    /// 导出销售条件
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesConditionAsync(TaktSalesConditionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesConditionQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesConditionExportDto>(),
                sheetName ?? "销售条件数据",
                fileName ?? "销售条件导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesConditionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesConditionExportDto>(),
                sheetName ?? "销售条件数据",
                fileName ?? "销售条件导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesConditionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售条件数据",
            fileName ?? "销售条件导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废销售条件行标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesConditionId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesConditionItemsObsoleteAsync(long salesConditionId)
    {
        if (salesConditionId <= 0)
        {
            return;
        }
        var rows = await _salesConditionItemRepository.GetListAsync(
            x => x.SalesConditionId == salesConditionId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesConditionItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充销售条件详情（加载 OneToMany 子表：销售条件行）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesConditionDetailsAsync(TaktSalesConditionDto dto, TaktSalesCondition entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售条件行 → dto.Items（含作废行）
        var items = await _salesConditionItemRepository.GetListAsync(x => x.SalesConditionId == entity.Id);
        dto.Items = items.Adapt<List<TaktSalesConditionItemDto>>();
    }

    /// <summary>
    /// 保存销售条件子表级联（销售条件行；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesConditionChildrenAsync(TaktSalesCondition entity, TaktSalesConditionCreateDto dto)
    {
        // 销售条件行（Items）
        List<TaktSalesConditionItemUpdateDto>? itemsForSave;
        if (dto is TaktSalesConditionUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktSalesConditionItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkSalesConditionItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _salesConditionItemRepository.GetListAsync(x => x.SalesConditionId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesConditionItem>();
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.SalesConditionId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.SalesConditionCode = entity.SalesConditionCode;
                childDto.PriceType = entity.PriceType;
                if (childDto.SalesConditionItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesConditionItemId, out var target))
                    {
                        throw new TaktBusinessException("销售条件行不存在（SalesConditionItemId={childDto.SalesConditionItemId}）");
                    }
                    if (target.SalesConditionId != entity.Id)
                    {
                        throw new TaktBusinessException("销售条件行不属于当前主表（SalesConditionItemId={childDto.SalesConditionItemId}）");
                    }
                    submittedIds.Add(childDto.SalesConditionItemId);
                    childDto.Adapt(target);
                    target.Id = childDto.SalesConditionItemId;
                    target.SalesConditionId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesConditionItemRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSalesConditionItem>();
                    child.Id = 0;
                    child.SalesConditionId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesConditionItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _salesConditionItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售条件查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesCondition, bool>> QueryExpression(TaktSalesConditionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesCondition>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SalesConditionCode != null && x.SalesConditionCode.Contains(keywords))
                || (x.PriceType != null && x.PriceType.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.SalesGroup != null && x.SalesGroup.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.SalesQuotationCode != null && x.SalesQuotationCode.Contains(keywords))
                || (x.VariableKey != null && x.VariableKey.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesConditionCode))
        {
            var salesConditionCode = queryDto.SalesConditionCode;
            exp = exp.And(x => x.SalesConditionCode != null && x.SalesConditionCode.Contains(salesConditionCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PriceType))
        {
            var priceType = queryDto.PriceType;
            exp = exp.And(x => x.PriceType != null && x.PriceType.Contains(priceType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CustomerCode))
        {
            var customerCode = queryDto.CustomerCode;
            exp = exp.And(x => x.CustomerCode != null && x.CustomerCode.Contains(customerCode));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesGroup))
        {
            var salesGroup = queryDto.SalesGroup;
            exp = exp.And(x => x.SalesGroup != null && x.SalesGroup.Contains(salesGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaxCode))
        {
            var taxCode = queryDto.TaxCode;
            exp = exp.And(x => x.TaxCode != null && x.TaxCode.Contains(taxCode));
        }

        if (queryDto?.GrBasedInvoiceInspection.HasValue == true)
        {
            var grBasedInvoiceInspection = queryDto.GrBasedInvoiceInspection.Value;
            exp = exp.And(x => x.GrBasedInvoiceInspection == grBasedInvoiceInspection);
        }

        if (queryDto?.PricingDateControl.HasValue == true)
        {
            var pricingDateControl = queryDto.PricingDateControl.Value;
            exp = exp.And(x => x.PricingDateControl == pricingDateControl);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.VariableKey))
        {
            var variableKey = queryDto.VariableKey;
            exp = exp.And(x => x.VariableKey != null && x.VariableKey.Contains(variableKey));
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

        if (queryDto?.ValidFromStart.HasValue == true)
        {
            var validFromStart = queryDto.ValidFromStart.Value;
            exp = exp.And(x => x.ValidFrom >= validFromStart);
        }

        if (queryDto?.ValidFromEnd.HasValue == true)
        {
            var validFromEnd = queryDto.ValidFromEnd.Value;
            exp = exp.And(x => x.ValidFrom <= validFromEnd);
        }

        if (queryDto?.ValidToStart.HasValue == true)
        {
            var validToStart = queryDto.ValidToStart.Value;
            exp = exp.And(x => x.ValidTo >= validToStart);
        }

        if (queryDto?.ValidToEnd.HasValue == true)
        {
            var validToEnd = queryDto.ValidToEnd.Value;
            exp = exp.And(x => x.ValidTo <= validToEnd);
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
    private static bool HasAnyListQueryFilter(TaktSalesConditionQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SalesConditionCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PriceType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
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
        if (!string.IsNullOrWhiteSpace(queryDto.SalesGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaxCode))
        {
            return true;
        }
        if (queryDto.GrBasedInvoiceInspection.HasValue)
        {
            return true;
        }
        if (queryDto.PricingDateControl.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.VariableKey))
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
        if (queryDto.ValidFromStart.HasValue || queryDto.ValidFromEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ValidToStart.HasValue || queryDto.ValidToEnd.HasValue)
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
