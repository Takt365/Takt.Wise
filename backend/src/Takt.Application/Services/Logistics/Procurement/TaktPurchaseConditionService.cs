// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseConditionService.cs
// 创建时间：2026-10-30
// 创建人：Takt365(Cursor AI)
// 功能描述：采购条件应用服务实现
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
/// 采购条件应用服务
/// </summary>
public class TaktPurchaseConditionService : TaktServiceBase, ITaktPurchaseConditionService
{
    private readonly ITaktCompanyRepository<TaktPurchaseCondition> _purchaseConditionRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseConditionItem> _purchaseConditionItemRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseConditionRepository">采购条件仓储</param>
    /// <param name="purchaseConditionItemRepository">PurchaseConditionItem仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseConditionService(
        ITaktCompanyRepository<TaktPurchaseCondition> purchaseConditionRepository,
        ITaktCompanyRepository<TaktPurchaseConditionItem> purchaseConditionItemRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseConditionRepository = purchaseConditionRepository;
        _purchaseConditionItemRepository = purchaseConditionItemRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取采购条件列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseConditionDto>> GetPurchaseConditionListAsync(TaktPurchaseConditionQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseConditionDto>.Create(
                new List<TaktPurchaseConditionDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseConditionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseConditionDto>.Create(
            data.Adapt<List<TaktPurchaseConditionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购条件
    /// </summary>
    /// <param name="id">采购条件ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseConditionDto?> GetPurchaseConditionByIdAsync(long id)
    {
        var entity = await _purchaseConditionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchaseConditionDto>();
        await FillPurchaseConditionDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseConditionOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseConditionRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialDescription ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseConditionCode,
            DictLabel = e.MaterialDescription ?? e.PurchaseConditionCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购条件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseConditionDto> CreatePurchaseConditionAsync(TaktPurchaseConditionCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseCondition>();
        var isUnique_ix_takt_logistics_procurement_purchase_condition_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseConditionRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseConditionCode == entity.PurchaseConditionCode);
        if (!isUnique_ix_takt_logistics_procurement_purchase_condition_code_unique)
        {
            throw new TaktBusinessException("采购条件的PlantCode、PurchaseConditionCode已存在");
        }
        entity = await _purchaseConditionRepository.CreateAsync(entity);
                await SavePurchaseConditionChildrenAsync(entity, dto);
        return await GetPurchaseConditionByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseConditionDto>();
    }

    /// <summary>
    /// 更新采购条件
    /// </summary>
    /// <param name="id">采购条件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseConditionDto> UpdatePurchaseConditionAsync(long id, TaktPurchaseConditionUpdateDto dto)
    {
        var entity = await _purchaseConditionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购条件不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_condition_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseConditionRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseConditionCode == entity.PurchaseConditionCode,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_condition_code_unique)
        {
            throw new TaktBusinessException("采购条件的PlantCode、PurchaseConditionCode已存在");
        }
        await _purchaseConditionRepository.UpdateAsync(entity);
                await SavePurchaseConditionChildrenAsync(entity, dto);
        return await GetPurchaseConditionByIdAsync(id) ?? throw new TaktBusinessException("采购条件不存在");
    }

    /// <summary>
    /// 删除采购条件
    /// </summary>
    /// <param name="id">采购条件ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseConditionByIdAsync(long id)
    {
        var entity = await _purchaseConditionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购条件不存在或已删除");
        }
        await _purchaseConditionItemRepository.DeleteAsync(x => x.PurchaseConditionId == entity.Id);
        var deleted = await _purchaseConditionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购条件不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购条件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseConditionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseConditionByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseConditionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseConditionTemplateDto>(
            sheetName ?? "采购条件导入模板",
            fileName ?? "采购条件导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购条件
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseConditionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseConditionImportDto>(fileStream, sheetName ?? "采购条件导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseCondition>();
                var importKey = $"{entity.PlantCode}|{entity.PurchaseConditionCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchaseConditionCode）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_condition_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseConditionRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchaseConditionCode == entity.PurchaseConditionCode);
                if (!isUnique_ix_takt_logistics_procurement_purchase_condition_code_unique)
                {
                    throw new TaktBusinessException("采购条件的PlantCode、PurchaseConditionCode已存在");
                }
                await _purchaseConditionRepository.CreateAsync(entity);
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
    /// 导出采购条件
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseConditionAsync(TaktPurchaseConditionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseConditionQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseConditionExportDto>(),
                sheetName ?? "采购条件数据",
                fileName ?? "采购条件导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseConditionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseConditionExportDto>(),
                sheetName ?? "采购条件数据",
                fileName ?? "采购条件导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseConditionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购条件数据",
            fileName ?? "采购条件导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废采购条件行标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchaseConditionId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchaseConditionItemsObsoleteAsync(long purchaseConditionId)
    {
        if (purchaseConditionId <= 0)
        {
            return;
        }
        var rows = await _purchaseConditionItemRepository.GetListAsync(
            x => x.PurchaseConditionId == purchaseConditionId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchaseConditionItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充采购条件详情（加载 OneToMany 子表：采购条件行）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchaseConditionDetailsAsync(TaktPurchaseConditionDto dto, TaktPurchaseCondition entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购条件行 → dto.Items（含作废行）
        var items = await _purchaseConditionItemRepository.GetListAsync(x => x.PurchaseConditionId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchaseConditionItemDto>>();
    }

    /// <summary>
    /// 保存采购条件子表级联（采购条件行；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchaseConditionChildrenAsync(TaktPurchaseCondition entity, TaktPurchaseConditionCreateDto dto)
    {
        // 采购条件行（Items）
        List<TaktPurchaseConditionItemUpdateDto>? itemsForSave;
        if (dto is TaktPurchaseConditionUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktPurchaseConditionItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkPurchaseConditionItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _purchaseConditionItemRepository.GetListAsync(x => x.PurchaseConditionId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchaseConditionItem>();
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.PurchaseConditionId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.PurchaseConditionCode = entity.PurchaseConditionCode;
                childDto.PriceType = entity.PriceType;
                if (childDto.PurchaseConditionItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchaseConditionItemId, out var target))
                    {
                        throw new TaktBusinessException("采购条件行不存在（PurchaseConditionItemId={childDto.PurchaseConditionItemId}）");
                    }
                    if (target.PurchaseConditionId != entity.Id)
                    {
                        throw new TaktBusinessException("采购条件行不属于当前主表（PurchaseConditionItemId={childDto.PurchaseConditionItemId}）");
                    }
                    submittedIds.Add(childDto.PurchaseConditionItemId);
                    childDto.Adapt(target);
                    target.Id = childDto.PurchaseConditionItemId;
                    target.PurchaseConditionId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchaseConditionItemRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktPurchaseConditionItem>();
                    child.Id = 0;
                    child.PurchaseConditionId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchaseConditionItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _purchaseConditionItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购条件查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseCondition, bool>> QueryExpression(TaktPurchaseConditionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseCondition>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchaseConditionCode != null && x.PurchaseConditionCode.Contains(keywords))
                || (x.PriceType != null && x.PriceType.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.PurchaseGroup != null && x.PurchaseGroup.Contains(keywords))
                || (x.TaxCode != null && x.TaxCode.Contains(keywords))
                || (x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseConditionCode))
        {
            var purchaseConditionCode = queryDto.PurchaseConditionCode;
            exp = exp.And(x => x.PurchaseConditionCode != null && x.PurchaseConditionCode.Contains(purchaseConditionCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PriceType))
        {
            var priceType = queryDto.PriceType;
            exp = exp.And(x => x.PriceType != null && x.PriceType.Contains(priceType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierCode))
        {
            var supplierCode = queryDto.SupplierCode;
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(supplierCode));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseGroup))
        {
            var purchaseGroup = queryDto.PurchaseGroup;
            exp = exp.And(x => x.PurchaseGroup != null && x.PurchaseGroup.Contains(purchaseGroup));
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

        if (queryDto?.PurchaseInquiryId.HasValue == true)
        {
            var purchaseInquiryId = queryDto.PurchaseInquiryId.Value;
            exp = exp.And(x => x.PurchaseInquiryId == purchaseInquiryId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseInquiryCode))
        {
            var purchaseInquiryCode = queryDto.PurchaseInquiryCode;
            exp = exp.And(x => x.PurchaseInquiryCode != null && x.PurchaseInquiryCode.Contains(purchaseInquiryCode));
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
    private static bool HasAnyListQueryFilter(TaktPurchaseConditionQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseConditionCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PriceType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseGroup))
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
        if (queryDto.PurchaseInquiryId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseInquiryCode))
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
