// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseInfoItemService.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：采购信息行应用服务实现
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
/// 采购信息行应用服务
/// </summary>
public class TaktPurchaseInfoItemService : TaktServiceBase, ITaktPurchaseInfoItemService
{
    private readonly ITaktCompanyRepository<TaktPurchaseInfoItem> _purchaseInfoItemRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInfoScaleQuantity> _purchaseInfoScaleQuantityRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInfoScaleValue> _purchaseInfoScaleValueRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInfoItemRepository">采购信息行仓储</param>
    /// <param name="purchaseInfoScaleQuantityRepository">PurchaseInfoScaleQuantity仓储</param>
    /// <param name="purchaseInfoScaleValueRepository">PurchaseInfoScaleValue仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseInfoItemService(
        ITaktCompanyRepository<TaktPurchaseInfoItem> purchaseInfoItemRepository,
        ITaktCompanyRepository<TaktPurchaseInfoScaleQuantity> purchaseInfoScaleQuantityRepository,
        ITaktCompanyRepository<TaktPurchaseInfoScaleValue> purchaseInfoScaleValueRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInfoItemRepository = purchaseInfoItemRepository;
        _purchaseInfoScaleQuantityRepository = purchaseInfoScaleQuantityRepository;
        _purchaseInfoScaleValueRepository = purchaseInfoScaleValueRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购信息行列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseInfoItemDto>> GetPurchaseInfoItemListAsync(TaktPurchaseInfoItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseInfoItemDto>.Create(
                new List<TaktPurchaseInfoItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseInfoItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseInfoItemDto>.Create(
            data.Adapt<List<TaktPurchaseInfoItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购信息行
    /// </summary>
    /// <param name="id">采购信息行ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoItemDto?> GetPurchaseInfoItemByIdAsync(long id)
    {
        var entity = await _purchaseInfoItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchaseInfoItemDto>();
        await FillPurchaseInfoItemDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购信息行选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseInfoItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseInfoItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.PurchaseInfoCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseInfoCode,
            DictLabel = e.PurchaseInfoCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购信息行
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoItemDto> CreatePurchaseInfoItemAsync(TaktPurchaseInfoItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseInfoItem>();
        entity.IsObsolete = 0;
        var isUnique_ix_takt_logistics_procurement_purchase_info_item_seq_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInfoItemRepository,
            x => x.PurchaseInfoId == entity.PurchaseInfoId
                && x.PurchaseInfoSeq == entity.PurchaseInfoSeq);
        if (!isUnique_ix_takt_logistics_procurement_purchase_info_item_seq_unique)
        {
            throw new TaktBusinessException("采购信息行的PurchaseInfoId、PurchaseInfoSeq已存在");
        }
        entity = await _purchaseInfoItemRepository.CreateAsync(entity);
                await SavePurchaseInfoItemChildrenAsync(entity, dto);
        return await GetPurchaseInfoItemByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseInfoItemDto>();
    }

    /// <summary>
    /// 更新采购信息行
    /// </summary>
    /// <param name="id">采购信息行ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoItemDto> UpdatePurchaseInfoItemAsync(long id, TaktPurchaseInfoItemUpdateDto dto)
    {
        var entity = await _purchaseInfoItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购信息行不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_info_item_seq_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInfoItemRepository,
            x => x.PurchaseInfoId == entity.PurchaseInfoId
                && x.PurchaseInfoSeq == entity.PurchaseInfoSeq,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_info_item_seq_unique)
        {
            throw new TaktBusinessException("采购信息行的PurchaseInfoId、PurchaseInfoSeq已存在");
        }
        await _purchaseInfoItemRepository.UpdateAsync(entity);
                await SavePurchaseInfoItemChildrenAsync(entity, dto);
        return await GetPurchaseInfoItemByIdAsync(id) ?? throw new TaktBusinessException("采购信息行不存在");
    }

    /// <summary>
    /// 删除采购信息行
    /// </summary>
    /// <param name="id">采购信息行ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInfoItemByIdAsync(long id)
    {
        var entity = await _purchaseInfoItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购信息行不存在或已删除");
        }
        await _purchaseInfoScaleQuantityRepository.DeleteAsync(x => x.PurchaseInfoItemId == entity.Id);
        await _purchaseInfoScaleValueRepository.DeleteAsync(x => x.PurchaseInfoItemId == entity.Id);
        var deleted = await _purchaseInfoItemRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购信息行不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购信息行
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInfoItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseInfoItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购信息行作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoItemDto> UpdatePurchaseInfoItemObsoleteAsync(TaktPurchaseInfoItemObsoleteDto dto)
    {
        var entity = await _purchaseInfoItemRepository.GetByIdAsync(dto.PurchaseInfoItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购信息行不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("采购信息行不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _purchaseInfoItemRepository.UpdateAsync(entity);
        return await GetPurchaseInfoItemByIdAsync(dto.PurchaseInfoItemId) ?? throw new TaktBusinessException("采购信息行不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseInfoItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseInfoItemTemplateDto>(
            sheetName ?? "采购信息行导入模板",
            fileName ?? "采购信息行导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购信息行
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseInfoItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseInfoItemImportDto>(fileStream, sheetName ?? "采购信息行导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseInfoItem>();
                var importKey = $"{entity.PurchaseInfoId}|{entity.PurchaseInfoSeq}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PurchaseInfoId、PurchaseInfoSeq）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_info_item_seq_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseInfoItemRepository,
                    x => x.PurchaseInfoId == entity.PurchaseInfoId
                        && x.PurchaseInfoSeq == entity.PurchaseInfoSeq);
                if (!isUnique_ix_takt_logistics_procurement_purchase_info_item_seq_unique)
                {
                    throw new TaktBusinessException("采购信息行的PurchaseInfoId、PurchaseInfoSeq已存在");
                }
                await _purchaseInfoItemRepository.CreateAsync(entity);
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
    /// 导出采购信息行
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseInfoItemAsync(TaktPurchaseInfoItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseInfoItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInfoItemExportDto>(),
                sheetName ?? "采购信息行数据",
                fileName ?? "采购信息行导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseInfoItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInfoItemExportDto>(),
                sheetName ?? "采购信息行数据",
                fileName ?? "采购信息行导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseInfoItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购信息行数据",
            fileName ?? "采购信息行导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废采购阶梯数量标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchaseInfoItemId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchaseInfoScaleQuantitysObsoleteAsync(long purchaseInfoItemId)
    {
        if (purchaseInfoItemId <= 0)
        {
            return;
        }
        var rows = await _purchaseInfoScaleQuantityRepository.GetListAsync(
            x => x.PurchaseInfoItemId == purchaseInfoItemId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchaseInfoScaleQuantityRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 将指定主表下全部未作废采购阶梯价格标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchaseInfoItemId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchaseInfoScaleValuesObsoleteAsync(long purchaseInfoItemId)
    {
        if (purchaseInfoItemId <= 0)
        {
            return;
        }
        var rows = await _purchaseInfoScaleValueRepository.GetListAsync(
            x => x.PurchaseInfoItemId == purchaseInfoItemId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchaseInfoScaleValueRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充采购信息行详情（加载 OneToMany 子表：采购阶梯数量、采购阶梯价格）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchaseInfoItemDetailsAsync(TaktPurchaseInfoItemDto dto, TaktPurchaseInfoItem entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购阶梯数量 → dto.ScaleQuantities（含作废行）
        var scalequantities = await _purchaseInfoScaleQuantityRepository.GetListAsync(x => x.PurchaseInfoItemId == entity.Id);
        dto.ScaleQuantities = scalequantities.Adapt<List<TaktPurchaseInfoScaleQuantityDto>>();
        // 采购阶梯价格 → dto.ScaleValues（含作废行）
        var scalevalues = await _purchaseInfoScaleValueRepository.GetListAsync(x => x.PurchaseInfoItemId == entity.Id);
        dto.ScaleValues = scalevalues.Adapt<List<TaktPurchaseInfoScaleValueDto>>();
    }

    /// <summary>
    /// 保存采购信息行子表级联（采购阶梯数量、采购阶梯价格；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchaseInfoItemChildrenAsync(TaktPurchaseInfoItem entity, TaktPurchaseInfoItemCreateDto dto)
    {
        // 采购阶梯数量（ScaleQuantities）
        List<TaktPurchaseInfoScaleQuantityUpdateDto>? scaleQuantitiesForSave;
        if (dto is TaktPurchaseInfoItemUpdateDto updateDtoForScaleQuantities && updateDtoForScaleQuantities.ScaleQuantities != null)
        {
            scaleQuantitiesForSave = updateDtoForScaleQuantities.ScaleQuantities;
        }
        else if (dto.ScaleQuantities != null)
        {
            scaleQuantitiesForSave = dto.ScaleQuantities.Adapt<List<TaktPurchaseInfoScaleQuantityUpdateDto>>();
        }
        else
        {
            scaleQuantitiesForSave = null;
        }
        if (scaleQuantitiesForSave is not { Count: > 0 })
        {
            await MarkPurchaseInfoScaleQuantitysObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _purchaseInfoScaleQuantityRepository.GetListAsync(x => x.PurchaseInfoItemId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchaseInfoScaleQuantity>();
            for (var i = 0; i < scaleQuantitiesForSave.Count; i++)
            {
                var childDto = scaleQuantitiesForSave[i];
                childDto.PurchaseInfoItemId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.PurchaseInfoCode = entity.PurchaseInfoCode;
                if (childDto.PurchaseInfoScaleQuantityId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchaseInfoScaleQuantityId, out var target))
                    {
                        throw new TaktBusinessException("采购阶梯数量不存在（PurchaseInfoScaleQuantityId={childDto.PurchaseInfoScaleQuantityId}）");
                    }
                    if (target.PurchaseInfoItemId != entity.Id)
                    {
                        throw new TaktBusinessException("采购阶梯数量不属于当前主表（PurchaseInfoScaleQuantityId={childDto.PurchaseInfoScaleQuantityId}）");
                    }
                    submittedIds.Add(childDto.PurchaseInfoScaleQuantityId);
                    childDto.Adapt(target);
                    target.Id = childDto.PurchaseInfoScaleQuantityId;
                    target.PurchaseInfoItemId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchaseInfoScaleQuantityRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktPurchaseInfoScaleQuantity>();
                    child.Id = 0;
                    child.PurchaseInfoItemId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchaseInfoScaleQuantityRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _purchaseInfoScaleQuantityRepository.CreateRangeAsync(toCreate);
            }
        }
        // 采购阶梯价格（ScaleValues）
        List<TaktPurchaseInfoScaleValueUpdateDto>? scaleValuesForSave;
        if (dto is TaktPurchaseInfoItemUpdateDto updateDtoForScaleValues && updateDtoForScaleValues.ScaleValues != null)
        {
            scaleValuesForSave = updateDtoForScaleValues.ScaleValues;
        }
        else if (dto.ScaleValues != null)
        {
            scaleValuesForSave = dto.ScaleValues.Adapt<List<TaktPurchaseInfoScaleValueUpdateDto>>();
        }
        else
        {
            scaleValuesForSave = null;
        }
        if (scaleValuesForSave is not { Count: > 0 })
        {
            await MarkPurchaseInfoScaleValuesObsoleteAsync(entity.Id);
        }
        else
        {
            var existingList = await _purchaseInfoScaleValueRepository.GetListAsync(x => x.PurchaseInfoItemId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchaseInfoScaleValue>();
            for (var i = 0; i < scaleValuesForSave.Count; i++)
            {
                var childDto = scaleValuesForSave[i];
                childDto.PurchaseInfoItemId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.PurchaseInfoCode = entity.PurchaseInfoCode;
                if (childDto.PurchaseInfoScaleValueId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchaseInfoScaleValueId, out var target))
                    {
                        throw new TaktBusinessException("采购阶梯价格不存在（PurchaseInfoScaleValueId={childDto.PurchaseInfoScaleValueId}）");
                    }
                    if (target.PurchaseInfoItemId != entity.Id)
                    {
                        throw new TaktBusinessException("采购阶梯价格不属于当前主表（PurchaseInfoScaleValueId={childDto.PurchaseInfoScaleValueId}）");
                    }
                    submittedIds.Add(childDto.PurchaseInfoScaleValueId);
                    childDto.Adapt(target);
                    target.Id = childDto.PurchaseInfoScaleValueId;
                    target.PurchaseInfoItemId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchaseInfoScaleValueRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktPurchaseInfoScaleValue>();
                    child.Id = 0;
                    child.PurchaseInfoItemId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchaseInfoScaleValueRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _purchaseInfoScaleValueRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购信息行查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseInfoItem, bool>> QueryExpression(TaktPurchaseInfoItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseInfoItem>();

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
                || (x.PurchaseInfoCode != null && x.PurchaseInfoCode.Contains(keywords))
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

        if (queryDto?.PurchaseInfoId.HasValue == true)
        {
            var purchaseInfoId = queryDto.PurchaseInfoId.Value;
            exp = exp.And(x => x.PurchaseInfoId == purchaseInfoId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseInfoCode))
        {
            var purchaseInfoCode = queryDto.PurchaseInfoCode;
            exp = exp.And(x => x.PurchaseInfoCode != null && x.PurchaseInfoCode.Contains(purchaseInfoCode));
        }

        if (queryDto?.PurchaseInfoSeq.HasValue == true)
        {
            var purchaseInfoSeq = queryDto.PurchaseInfoSeq.Value;
            exp = exp.And(x => x.PurchaseInfoSeq == purchaseInfoSeq);
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
    private static bool HasAnyListQueryFilter(TaktPurchaseInfoItemQueryDto? queryDto)
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
        if (queryDto.PurchaseInfoId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseInfoCode))
        {
            return true;
        }
        if (queryDto.PurchaseInfoSeq.HasValue)
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
