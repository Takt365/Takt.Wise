// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesConditionRecordService.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：销售条件记录应用服务实现
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
/// 销售条件记录应用服务
/// </summary>
public class TaktSalesConditionRecordService : TaktServiceBase, ITaktSalesConditionRecordService
{
    private readonly ITaktCompanyRepository<TaktSalesConditionRecord> _salesConditionRecordRepository;
    private readonly ITaktCompanyRepository<TaktSalesConditionRecordItem> _salesConditionRecordItemRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesConditionRecordRepository">销售条件记录仓储</param>
    /// <param name="salesConditionRecordItemRepository">SalesConditionRecordItem仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesConditionRecordService(
        ITaktCompanyRepository<TaktSalesConditionRecord> salesConditionRecordRepository,
        ITaktCompanyRepository<TaktSalesConditionRecordItem> salesConditionRecordItemRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesConditionRecordRepository = salesConditionRecordRepository;
        _salesConditionRecordItemRepository = salesConditionRecordItemRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售条件记录列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesConditionRecordDto>> GetSalesConditionRecordListAsync(TaktSalesConditionRecordQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesConditionRecordDto>.Create(
                new List<TaktSalesConditionRecordDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesConditionRecordRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesConditionRecordDto>.Create(
            data.Adapt<List<TaktSalesConditionRecordDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售条件记录
    /// </summary>
    /// <param name="id">销售条件记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionRecordDto?> GetSalesConditionRecordByIdAsync(long id)
    {
        var entity = await _salesConditionRecordRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesConditionRecordDto>();
        await FillSalesConditionRecordDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesConditionRecordOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesConditionRecordRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SalesConditionRecordCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesConditionRecordCode,
            DictLabel = e.SalesConditionRecordCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售条件记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionRecordDto> CreateSalesConditionRecordAsync(TaktSalesConditionRecordCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesConditionRecord>();
        var isUnique_ix_takt_logistics_sales_condition_record_code_unique = await _uniqueValidator.IsUniqueAsync(
            _salesConditionRecordRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesConditionRecordCode == entity.SalesConditionRecordCode);
        if (!isUnique_ix_takt_logistics_sales_condition_record_code_unique)
        {
            throw new TaktBusinessException("销售条件记录的PlantCode、SalesConditionRecordCode已存在");
        }
        entity = await _salesConditionRecordRepository.CreateAsync(entity);
                await SaveSalesConditionRecordChildrenAsync(entity, dto);
        return await GetSalesConditionRecordByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesConditionRecordDto>();
    }

    /// <summary>
    /// 更新销售条件记录
    /// </summary>
    /// <param name="id">销售条件记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionRecordDto> UpdateSalesConditionRecordAsync(long id, TaktSalesConditionRecordUpdateDto dto)
    {
        var entity = await _salesConditionRecordRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售条件记录不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_condition_record_code_unique = await _uniqueValidator.IsUniqueAsync(
            _salesConditionRecordRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesConditionRecordCode == entity.SalesConditionRecordCode,
            id);
        if (!isUnique_ix_takt_logistics_sales_condition_record_code_unique)
        {
            throw new TaktBusinessException("销售条件记录的PlantCode、SalesConditionRecordCode已存在");
        }
        await _salesConditionRecordRepository.UpdateAsync(entity);
                await SaveSalesConditionRecordChildrenAsync(entity, dto);
        return await GetSalesConditionRecordByIdAsync(id) ?? throw new TaktBusinessException("销售条件记录不存在");
    }

    /// <summary>
    /// 删除销售条件记录
    /// </summary>
    /// <param name="id">销售条件记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesConditionRecordByIdAsync(long id)
    {
        var entity = await _salesConditionRecordRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售条件记录不存在或已删除");
        }
        await _salesConditionRecordItemRepository.DeleteAsync(x => x.SalesConditionRecordId == entity.Id);
        var deleted = await _salesConditionRecordRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售条件记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售条件记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesConditionRecordBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesConditionRecordByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesConditionRecordTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesConditionRecordTemplateDto>(
            sheetName ?? "销售条件记录导入模板",
            fileName ?? "销售条件记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售条件记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesConditionRecordAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesConditionRecordImportDto>(fileStream, sheetName ?? "销售条件记录导入模板");
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
                var entity = rows[i].Adapt<TaktSalesConditionRecord>();
                var importKey = $"{entity.PlantCode}|{entity.SalesConditionRecordCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SalesConditionRecordCode）");
                }
                var isUnique_ix_takt_logistics_sales_condition_record_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesConditionRecordRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SalesConditionRecordCode == entity.SalesConditionRecordCode);
                if (!isUnique_ix_takt_logistics_sales_condition_record_code_unique)
                {
                    throw new TaktBusinessException("销售条件记录的PlantCode、SalesConditionRecordCode已存在");
                }
                await _salesConditionRecordRepository.CreateAsync(entity);
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
    /// 导出销售条件记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesConditionRecordAsync(TaktSalesConditionRecordQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesConditionRecordQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesConditionRecordExportDto>(),
                sheetName ?? "销售条件记录数据",
                fileName ?? "销售条件记录导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesConditionRecordRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesConditionRecordExportDto>(),
                sheetName ?? "销售条件记录数据",
                fileName ?? "销售条件记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesConditionRecordExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售条件记录数据",
            fileName ?? "销售条件记录导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废销售条件记录行标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesConditionRecordId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesConditionRecordItemsObsoleteAsync(long salesConditionRecordId)
    {
        if (salesConditionRecordId <= 0)
        {
            return;
        }
        var rows = await _salesConditionRecordItemRepository.GetListAsync(
            x => x.SalesConditionRecordId == salesConditionRecordId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesConditionRecordItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充销售条件记录详情（加载 OneToMany 子表：销售条件记录行）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesConditionRecordDetailsAsync(TaktSalesConditionRecordDto dto, TaktSalesConditionRecord entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售条件记录行 → dto.Items（含作废行）
        var items = await _salesConditionRecordItemRepository.GetListAsync(x => x.SalesConditionRecordId == entity.Id);
        dto.Items = items.Adapt<List<TaktSalesConditionRecordItemDto>>();
    }

    /// <summary>
    /// 保存销售条件记录子表级联（销售条件记录行；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesConditionRecordChildrenAsync(TaktSalesConditionRecord entity, TaktSalesConditionRecordCreateDto dto)
    {
        // 销售条件记录行（Items）
        List<TaktSalesConditionRecordItemUpdateDto>? itemsForSave;
        if (dto is TaktSalesConditionRecordUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktSalesConditionRecordItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkSalesConditionRecordItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _salesConditionRecordItemRepository.GetListAsync(x => x.SalesConditionRecordId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesConditionRecordItem>();
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.SalesConditionRecordId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.SalesConditionRecordCode = entity.SalesConditionRecordCode;
                childDto.PriceType = entity.PriceType;
                if (childDto.SalesConditionRecordItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesConditionRecordItemId, out var target))
                    {
                        throw new TaktBusinessException("销售条件记录行不存在（SalesConditionRecordItemId={childDto.SalesConditionRecordItemId}）");
                    }
                    if (target.SalesConditionRecordId != entity.Id)
                    {
                        throw new TaktBusinessException("销售条件记录行不属于当前主表（SalesConditionRecordItemId={childDto.SalesConditionRecordItemId}）");
                    }
                    submittedIds.Add(childDto.SalesConditionRecordItemId);
                    childDto.Adapt(target);
                    target.Id = childDto.SalesConditionRecordItemId;
                    target.SalesConditionRecordId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesConditionRecordItemRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSalesConditionRecordItem>();
                    child.Id = 0;
                    child.SalesConditionRecordId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesConditionRecordItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _salesConditionRecordItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售条件记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesConditionRecord, bool>> QueryExpression(TaktSalesConditionRecordQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesConditionRecord>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SalesConditionRecordCode != null && x.SalesConditionRecordCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesConditionRecordCode))
        {
            var salesConditionRecordCode = queryDto.SalesConditionRecordCode;
            exp = exp.And(x => x.SalesConditionRecordCode != null && x.SalesConditionRecordCode.Contains(salesConditionRecordCode));
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
    private static bool HasAnyListQueryFilter(TaktSalesConditionRecordQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SalesConditionRecordCode))
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
