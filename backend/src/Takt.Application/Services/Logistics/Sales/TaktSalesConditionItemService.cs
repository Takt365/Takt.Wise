// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Sales
// 文件名称：TaktSalesConditionItemService.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：销售条件行应用服务实现
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
/// 销售条件行应用服务
/// </summary>
public class TaktSalesConditionItemService : TaktServiceBase, ITaktSalesConditionItemService
{
    private readonly ITaktCompanyRepository<TaktSalesConditionItem> _salesConditionItemRepository;
    private readonly ITaktCompanyRepository<TaktSalesConditionScaleQuantity> _salesConditionScaleQuantityRepository;
    private readonly ITaktCompanyRepository<TaktSalesConditionScaleValue> _salesConditionScaleValueRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesConditionItemRepository">销售条件行仓储</param>
    /// <param name="salesConditionScaleQuantityRepository">SalesConditionScaleQuantity仓储</param>
    /// <param name="salesConditionScaleValueRepository">SalesConditionScaleValue仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesConditionItemService(
        ITaktCompanyRepository<TaktSalesConditionItem> salesConditionItemRepository,
        ITaktCompanyRepository<TaktSalesConditionScaleQuantity> salesConditionScaleQuantityRepository,
        ITaktCompanyRepository<TaktSalesConditionScaleValue> salesConditionScaleValueRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesConditionItemRepository = salesConditionItemRepository;
        _salesConditionScaleQuantityRepository = salesConditionScaleQuantityRepository;
        _salesConditionScaleValueRepository = salesConditionScaleValueRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售条件行列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesConditionItemDto>> GetSalesConditionItemListAsync(TaktSalesConditionItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesConditionItemDto>.Create(
                new List<TaktSalesConditionItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesConditionItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesConditionItemDto>.Create(
            data.Adapt<List<TaktSalesConditionItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售条件行
    /// </summary>
    /// <param name="id">销售条件行ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionItemDto?> GetSalesConditionItemByIdAsync(long id)
    {
        var entity = await _salesConditionItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesConditionItemDto>();
        await FillSalesConditionItemDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售条件行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesConditionItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesConditionItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.SalesConditionCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesConditionCode,
            DictLabel = e.SalesConditionCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售条件行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionItemDto> CreateSalesConditionItemAsync(TaktSalesConditionItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesConditionItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_sales_condition_item_seq_unique = await _uniqueValidator.IsUniqueAsync(
            _salesConditionItemRepository,
            x => x.SalesConditionId == entity.SalesConditionId
                && x.SalesConditionSeq == entity.SalesConditionSeq);
        if (!isUnique_ix_takt_logistics_sales_condition_item_seq_unique)
        {
            throw new TaktBusinessException("销售条件行的SalesConditionId、SalesConditionSeq已存在");
        }
        entity = await _salesConditionItemRepository.CreateAsync(entity);
                await SaveSalesConditionItemChildrenAsync(entity, dto);
        return await GetSalesConditionItemByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesConditionItemDto>();
    }

    /// <summary>
    /// 更新销售条件行
    /// </summary>
    /// <param name="id">销售条件行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionItemDto> UpdateSalesConditionItemAsync(long id, TaktSalesConditionItemUpdateDto dto)
    {
        var entity = await _salesConditionItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售条件行不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_sales_condition_item_seq_unique = await _uniqueValidator.IsUniqueAsync(
            _salesConditionItemRepository,
            x => x.SalesConditionId == entity.SalesConditionId
                && x.SalesConditionSeq == entity.SalesConditionSeq,
            id);
        if (!isUnique_ix_takt_logistics_sales_condition_item_seq_unique)
        {
            throw new TaktBusinessException("销售条件行的SalesConditionId、SalesConditionSeq已存在");
        }
        await _salesConditionItemRepository.UpdateAsync(entity);
                await SaveSalesConditionItemChildrenAsync(entity, dto);
        return await GetSalesConditionItemByIdAsync(id) ?? throw new TaktBusinessException("销售条件行不存在");
    }

    /// <summary>
    /// 删除销售条件行
    /// </summary>
    /// <param name="id">销售条件行ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesConditionItemByIdAsync(long id)
    {
        var entity = await _salesConditionItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售条件行不存在或已删除");
        }
        await _salesConditionScaleQuantityRepository.DeleteAsync(x => x.SalesConditionItemId == entity.Id);
        await _salesConditionScaleValueRepository.DeleteAsync(x => x.SalesConditionItemId == entity.Id);
        var deleted = await _salesConditionItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售条件行不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售条件行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesConditionItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesConditionItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售条件行作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesConditionItemDto> UpdateSalesConditionItemObsoleteAsync(TaktSalesConditionItemObsoleteDto dto)
    {
        var entity = await _salesConditionItemRepository.GetByIdAsync(dto.SalesConditionItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售条件行不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("销售条件行不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _salesConditionItemRepository.UpdateAsync(entity);
        return await GetSalesConditionItemByIdAsync(dto.SalesConditionItemId) ?? throw new TaktBusinessException("销售条件行不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesConditionItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesConditionItemTemplateDto>(
            sheetName ?? "销售条件行导入模板",
            fileName ?? "销售条件行导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售条件行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesConditionItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesConditionItemImportDto>(fileStream, sheetName ?? "销售条件行导入模板");
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
                var entity = rows[i].Adapt<TaktSalesConditionItem>();
                var importKey = $"{entity.SalesConditionId}|{entity.SalesConditionSeq}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SalesConditionId、SalesConditionSeq）");
                }
                var isUnique_ix_takt_logistics_sales_condition_item_seq_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesConditionItemRepository,
                    x => x.SalesConditionId == entity.SalesConditionId
                        && x.SalesConditionSeq == entity.SalesConditionSeq);
                if (!isUnique_ix_takt_logistics_sales_condition_item_seq_unique)
                {
                    throw new TaktBusinessException("销售条件行的SalesConditionId、SalesConditionSeq已存在");
                }
                await _salesConditionItemRepository.CreateAsync(entity);
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
    /// 导出销售条件行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesConditionItemAsync(TaktSalesConditionItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesConditionItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesConditionItemExportDto>(),
                sheetName ?? "销售条件行数据",
                fileName ?? "销售条件行导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesConditionItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesConditionItemExportDto>(),
                sheetName ?? "销售条件行数据",
                fileName ?? "销售条件行导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesConditionItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售条件行数据",
            fileName ?? "销售条件行导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废销售阶梯数量标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesConditionItemId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesConditionScaleQuantitysObsoleteAsync(long salesConditionItemId)
    {
        if (salesConditionItemId <= 0)
        {
            return;
        }
        var rows = await _salesConditionScaleQuantityRepository.GetListAsync(
            x => x.SalesConditionItemId == salesConditionItemId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesConditionScaleQuantityRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废销售阶梯价格标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesConditionItemId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesConditionScaleValuesObsoleteAsync(long salesConditionItemId)
    {
        if (salesConditionItemId <= 0)
        {
            return;
        }
        var rows = await _salesConditionScaleValueRepository.GetListAsync(
            x => x.SalesConditionItemId == salesConditionItemId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesConditionScaleValueRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充销售条件行详情（加载 OneToMany 子表：销售阶梯数量、销售阶梯价格）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesConditionItemDetailsAsync(TaktSalesConditionItemDto dto, TaktSalesConditionItem entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售阶梯数量 → dto.ScaleQuantities（含作废行）
        var scalequantities = await _salesConditionScaleQuantityRepository.GetListAsync(x => x.SalesConditionItemId == entity.Id);
        dto.ScaleQuantities = scalequantities.Adapt<List<TaktSalesConditionScaleQuantityDto>>();
        // 销售阶梯价格 → dto.ScaleValues（含作废行）
        var scalevalues = await _salesConditionScaleValueRepository.GetListAsync(x => x.SalesConditionItemId == entity.Id);
        dto.ScaleValues = scalevalues.Adapt<List<TaktSalesConditionScaleValueDto>>();
    }

    /// <summary>
    /// 保存销售条件行子表级联（销售阶梯数量、销售阶梯价格；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesConditionItemChildrenAsync(TaktSalesConditionItem entity, TaktSalesConditionItemCreateDto dto)
    {
        // 销售阶梯数量（ScaleQuantities）
        List<TaktSalesConditionScaleQuantityUpdateDto>? scaleQuantitiesForSave;
        if (dto is TaktSalesConditionItemUpdateDto updateDtoForScaleQuantities && updateDtoForScaleQuantities.ScaleQuantities != null)
        {
            scaleQuantitiesForSave = updateDtoForScaleQuantities.ScaleQuantities;
        }
        else if (dto.ScaleQuantities != null)
        {
            scaleQuantitiesForSave = dto.ScaleQuantities.Adapt<List<TaktSalesConditionScaleQuantityUpdateDto>>();
        }
        else
        {
            scaleQuantitiesForSave = null;
        }
        if (scaleQuantitiesForSave is not { Count: > 0 })
        {
            await MarkSalesConditionScaleQuantitysObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _salesConditionScaleQuantityRepository.GetListAsync(x => x.SalesConditionItemId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesConditionScaleQuantity>();
            for (var i = 0; i < scaleQuantitiesForSave.Count; i++)
            {
                var childDto = scaleQuantitiesForSave[i];
                childDto.SalesConditionItemId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.SalesConditionCode = entity.SalesConditionCode;
                if (childDto.SalesConditionScaleQuantityId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesConditionScaleQuantityId, out var target))
                    {
                        throw new TaktBusinessException("销售阶梯数量不存在（SalesConditionScaleQuantityId={childDto.SalesConditionScaleQuantityId}）");
                    }
                    if (target.SalesConditionItemId != entity.Id)
                    {
                        throw new TaktBusinessException("销售阶梯数量不属于当前主表（SalesConditionScaleQuantityId={childDto.SalesConditionScaleQuantityId}）");
                    }
                    submittedIds.Add(childDto.SalesConditionScaleQuantityId);
                    childDto.Adapt(target);
                    target.Id = childDto.SalesConditionScaleQuantityId;
                    target.SalesConditionItemId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesConditionScaleQuantityRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSalesConditionScaleQuantity>();
                    child.Id = 0;
                    child.SalesConditionItemId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesConditionScaleQuantityRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _salesConditionScaleQuantityRepository.CreateRangeAsync(toCreate);
            }
        }
        // 销售阶梯价格（ScaleValues）
        List<TaktSalesConditionScaleValueUpdateDto>? scaleValuesForSave;
        if (dto is TaktSalesConditionItemUpdateDto updateDtoForScaleValues && updateDtoForScaleValues.ScaleValues != null)
        {
            scaleValuesForSave = updateDtoForScaleValues.ScaleValues;
        }
        else if (dto.ScaleValues != null)
        {
            scaleValuesForSave = dto.ScaleValues.Adapt<List<TaktSalesConditionScaleValueUpdateDto>>();
        }
        else
        {
            scaleValuesForSave = null;
        }
        if (scaleValuesForSave is not { Count: > 0 })
        {
            await MarkSalesConditionScaleValuesObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _salesConditionScaleValueRepository.GetListAsync(x => x.SalesConditionItemId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesConditionScaleValue>();
            for (var i = 0; i < scaleValuesForSave.Count; i++)
            {
                var childDto = scaleValuesForSave[i];
                childDto.SalesConditionItemId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.SalesConditionCode = entity.SalesConditionCode;
                if (childDto.SalesConditionScaleValueId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesConditionScaleValueId, out var target))
                    {
                        throw new TaktBusinessException("销售阶梯价格不存在（SalesConditionScaleValueId={childDto.SalesConditionScaleValueId}）");
                    }
                    if (target.SalesConditionItemId != entity.Id)
                    {
                        throw new TaktBusinessException("销售阶梯价格不属于当前主表（SalesConditionScaleValueId={childDto.SalesConditionScaleValueId}）");
                    }
                    submittedIds.Add(childDto.SalesConditionScaleValueId);
                    childDto.Adapt(target);
                    target.Id = childDto.SalesConditionScaleValueId;
                    target.SalesConditionItemId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesConditionScaleValueRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSalesConditionScaleValue>();
                    child.Id = 0;
                    child.SalesConditionItemId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesConditionScaleValueRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _salesConditionScaleValueRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售条件行查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesConditionItem, bool>> QueryExpression(TaktSalesConditionItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesConditionItem>();

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
                || (x.SalesConditionCode != null && x.SalesConditionCode.Contains(keywords))
                || (x.PriceType != null && x.PriceType.Contains(keywords))
                || (x.ScaleType != null && x.ScaleType.Contains(keywords))
                || (x.ScaleBasis != null && x.ScaleBasis.Contains(keywords))
                || (x.ScaleUnit != null && x.ScaleUnit.Contains(keywords))
                || (x.ScaleCurrencyCode != null && x.ScaleCurrencyCode.Contains(keywords))
                || (x.CalculationType != null && x.CalculationType.Contains(keywords))
                || (x.ConditionCurrencyCode != null && x.ConditionCurrencyCode.Contains(keywords))
                || (x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(keywords))
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

        if (queryDto?.SalesConditionId.HasValue == true)
        {
            var salesConditionId = queryDto.SalesConditionId.Value;
            exp = exp.And(x => x.SalesConditionId == salesConditionId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesConditionCode))
        {
            var salesConditionCode = queryDto.SalesConditionCode;
            exp = exp.And(x => x.SalesConditionCode != null && x.SalesConditionCode.Contains(salesConditionCode));
        }

        if (queryDto?.SalesConditionSeq.HasValue == true)
        {
            var salesConditionSeq = queryDto.SalesConditionSeq.Value;
            exp = exp.And(x => x.SalesConditionSeq == salesConditionSeq);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PriceType))
        {
            var priceType = queryDto.PriceType;
            exp = exp.And(x => x.PriceType != null && x.PriceType.Contains(priceType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScaleType))
        {
            var scaleType = queryDto.ScaleType;
            exp = exp.And(x => x.ScaleType != null && x.ScaleType.Contains(scaleType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScaleBasis))
        {
            var scaleBasis = queryDto.ScaleBasis;
            exp = exp.And(x => x.ScaleBasis != null && x.ScaleBasis.Contains(scaleBasis));
        }

        if (queryDto?.ScaleQuantity.HasValue == true)
        {
            var scaleQuantity = queryDto.ScaleQuantity.Value;
            exp = exp.And(x => x.ScaleQuantity == scaleQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScaleUnit))
        {
            var scaleUnit = queryDto.ScaleUnit;
            exp = exp.And(x => x.ScaleUnit != null && x.ScaleUnit.Contains(scaleUnit));
        }

        if (queryDto?.ScaleValue.HasValue == true)
        {
            var scaleValue = queryDto.ScaleValue.Value;
            exp = exp.And(x => x.ScaleValue == scaleValue);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ScaleCurrencyCode))
        {
            var scaleCurrencyCode = queryDto.ScaleCurrencyCode;
            exp = exp.And(x => x.ScaleCurrencyCode != null && x.ScaleCurrencyCode.Contains(scaleCurrencyCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CalculationType))
        {
            var calculationType = queryDto.CalculationType;
            exp = exp.And(x => x.CalculationType != null && x.CalculationType.Contains(calculationType));
        }

        if (queryDto?.Price.HasValue == true)
        {
            var price = queryDto.Price.Value;
            exp = exp.And(x => x.Price == price);
        }

        if (queryDto?.UntaxedPrice.HasValue == true)
        {
            var untaxedPrice = queryDto.UntaxedPrice.Value;
            exp = exp.And(x => x.UntaxedPrice == untaxedPrice);
        }

        if (queryDto?.TaxIncludedPrice.HasValue == true)
        {
            var taxIncludedPrice = queryDto.TaxIncludedPrice.Value;
            exp = exp.And(x => x.TaxIncludedPrice == taxIncludedPrice);
        }

        if (queryDto?.TaxAmount.HasValue == true)
        {
            var taxAmount = queryDto.TaxAmount.Value;
            exp = exp.And(x => x.TaxAmount == taxAmount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ConditionCurrencyCode))
        {
            var conditionCurrencyCode = queryDto.ConditionCurrencyCode;
            exp = exp.And(x => x.ConditionCurrencyCode != null && x.ConditionCurrencyCode.Contains(conditionCurrencyCode));
        }

        if (queryDto?.PriceUnit.HasValue == true)
        {
            var priceUnit = queryDto.PriceUnit.Value;
            exp = exp.And(x => x.PriceUnit == priceUnit);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UnitOfMeasure))
        {
            var unitOfMeasure = queryDto.UnitOfMeasure;
            exp = exp.And(x => x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(unitOfMeasure));
        }

        if (queryDto?.MinOrderQuantity.HasValue == true)
        {
            var minOrderQuantity = queryDto.MinOrderQuantity.Value;
            exp = exp.And(x => x.MinOrderQuantity == minOrderQuantity);
        }

        if (queryDto?.RoundingValue.HasValue == true)
        {
            var roundingValue = queryDto.RoundingValue.Value;
            exp = exp.And(x => x.RoundingValue == roundingValue);
        }

        if (queryDto?.PlannedDeliveryTimeDays.HasValue == true)
        {
            var plannedDeliveryTimeDays = queryDto.PlannedDeliveryTimeDays.Value;
            exp = exp.And(x => x.PlannedDeliveryTimeDays == plannedDeliveryTimeDays);
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
    private static bool HasAnyListQueryFilter(TaktSalesConditionItemQueryDto? queryDto)
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
        if (queryDto.SalesConditionId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesConditionCode))
        {
            return true;
        }
        if (queryDto.SalesConditionSeq.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PriceType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScaleType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScaleBasis))
        {
            return true;
        }
        if (queryDto.ScaleQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScaleUnit))
        {
            return true;
        }
        if (queryDto.ScaleValue.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ScaleCurrencyCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CalculationType))
        {
            return true;
        }
        if (queryDto.Price.HasValue)
        {
            return true;
        }
        if (queryDto.UntaxedPrice.HasValue)
        {
            return true;
        }
        if (queryDto.TaxIncludedPrice.HasValue)
        {
            return true;
        }
        if (queryDto.TaxAmount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ConditionCurrencyCode))
        {
            return true;
        }
        if (queryDto.PriceUnit.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UnitOfMeasure))
        {
            return true;
        }
        if (queryDto.MinOrderQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.RoundingValue.HasValue)
        {
            return true;
        }
        if (queryDto.PlannedDeliveryTimeDays.HasValue)
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
