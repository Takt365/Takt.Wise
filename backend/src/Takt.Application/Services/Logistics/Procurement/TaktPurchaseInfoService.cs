// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseInfoService.cs
// 创建时间：2026-09-21
// 创建人：Takt365(Cursor AI)
// 功能描述：采购信息应用服务实现
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
/// 采购信息应用服务
/// </summary>
public class TaktPurchaseInfoService : TaktServiceBase, ITaktPurchaseInfoService
{
    private readonly ITaktCompanyRepository<TaktPurchaseInfo> _purchaseInfoRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseInfoItem> _purchaseInfoItemRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseInfoRepository">采购信息仓储</param>
    /// <param name="purchaseInfoItemRepository">PurchaseInfoItem仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseInfoService(
        ITaktCompanyRepository<TaktPurchaseInfo> purchaseInfoRepository,
        ITaktCompanyRepository<TaktPurchaseInfoItem> purchaseInfoItemRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseInfoRepository = purchaseInfoRepository;
        _purchaseInfoItemRepository = purchaseInfoItemRepository;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取采购信息列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseInfoDto>> GetPurchaseInfoListAsync(TaktPurchaseInfoQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseInfoDto>.Create(
                new List<TaktPurchaseInfoDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseInfoRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseInfoDto>.Create(
            data.Adapt<List<TaktPurchaseInfoDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购信息
    /// </summary>
    /// <param name="id">采购信息ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoDto?> GetPurchaseInfoByIdAsync(long id)
    {
        var entity = await _purchaseInfoRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchaseInfoDto>();
        await FillPurchaseInfoDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购价格选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseInfoOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseInfoRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MaterialDescription ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseInfoCode,
            DictLabel = e.MaterialDescription ?? e.PurchaseInfoCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购信息
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoDto> CreatePurchaseInfoAsync(TaktPurchaseInfoCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseInfo>();
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var generated = await _numberingGenerator.GenerateNextAsync(dto.NumberingRuleCode.Trim());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.PurchaseInfoCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.PurchaseInfoCode))
        {
            throw new TaktBusinessException("定价记录号不能为空");
        }
        var isUnique_ix_takt_logistics_procurement_purchase_info_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInfoRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseInfoCode == entity.PurchaseInfoCode);
        if (!isUnique_ix_takt_logistics_procurement_purchase_info_code_unique)
        {
            throw new TaktBusinessException("采购信息的PlantCode、PurchaseInfoCode已存在");
        }
        entity = await _purchaseInfoRepository.CreateAsync(entity);
                await SavePurchaseInfoChildrenAsync(entity, dto);
        return await GetPurchaseInfoByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseInfoDto>();
    }

    /// <summary>
    /// 更新采购信息
    /// </summary>
    /// <param name="id">采购信息ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseInfoDto> UpdatePurchaseInfoAsync(long id, TaktPurchaseInfoUpdateDto dto)
    {
        var entity = await _purchaseInfoRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购信息不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_info_code_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseInfoRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseInfoCode == entity.PurchaseInfoCode,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_info_code_unique)
        {
            throw new TaktBusinessException("采购信息的PlantCode、PurchaseInfoCode已存在");
        }
        await _purchaseInfoRepository.UpdateAsync(entity);
                await SavePurchaseInfoChildrenAsync(entity, dto);
        return await GetPurchaseInfoByIdAsync(id) ?? throw new TaktBusinessException("采购信息不存在");
    }

    /// <summary>
    /// 删除采购信息
    /// </summary>
    /// <param name="id">采购信息ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInfoByIdAsync(long id)
    {
        var entity = await _purchaseInfoRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购信息不存在或已删除");
        }
        await _purchaseInfoItemRepository.DeleteAsync(x => x.PurchaseInfoId == entity.Id);
        var deleted = await _purchaseInfoRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购信息不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购信息
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseInfoBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseInfoByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseInfoTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseInfoTemplateDto>(
            sheetName ?? "采购信息导入模板",
            fileName ?? "采购信息导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购信息
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseInfoAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseInfoImportDto>(fileStream, sheetName ?? "采购信息导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseInfo>();
                var importKey = $"{entity.PlantCode}|{entity.PurchaseInfoCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchaseInfoCode）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_info_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseInfoRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchaseInfoCode == entity.PurchaseInfoCode);
                if (!isUnique_ix_takt_logistics_procurement_purchase_info_code_unique)
                {
                    throw new TaktBusinessException("采购信息的PlantCode、PurchaseInfoCode已存在");
                }
                await _purchaseInfoRepository.CreateAsync(entity);
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
    /// 导出采购信息
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseInfoAsync(TaktPurchaseInfoQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseInfoQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInfoExportDto>(),
                sheetName ?? "采购信息数据",
                fileName ?? "采购信息导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseInfoRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseInfoExportDto>(),
                sheetName ?? "采购信息数据",
                fileName ?? "采购信息导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseInfoExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购信息数据",
            fileName ?? "采购信息导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废采购信息行标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchaseInfoId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchaseInfoItemsObsoleteAsync(long purchaseInfoId)
    {
        if (purchaseInfoId <= 0)
        {
            return;
        }
        var rows = await _purchaseInfoItemRepository.GetListAsync(
            x => x.PurchaseInfoId == purchaseInfoId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchaseInfoItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充采购信息详情（加载 OneToMany 子表：采购信息行）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchaseInfoDetailsAsync(TaktPurchaseInfoDto dto, TaktPurchaseInfo entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购信息行 → dto.Items（含作废行）
        var items = await _purchaseInfoItemRepository.GetListAsync(x => x.PurchaseInfoId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchaseInfoItemDto>>();
    }

    /// <summary>
    /// 保存采购信息子表级联（采购信息行；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchaseInfoChildrenAsync(TaktPurchaseInfo entity, TaktPurchaseInfoCreateDto dto)
    {
        // 采购信息行（Items）
        List<TaktPurchaseInfoItemUpdateDto>? itemsForSave;
        if (dto is TaktPurchaseInfoUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktPurchaseInfoItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkPurchaseInfoItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _purchaseInfoItemRepository.GetListAsync(x => x.PurchaseInfoId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchaseInfoItem>();
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.PurchaseInfoId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.PurchaseInfoCode = entity.PurchaseInfoCode;
                childDto.PriceType = entity.PriceType;
                if (childDto.PurchaseInfoItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchaseInfoItemId, out var target))
                    {
                        throw new TaktBusinessException("采购信息行不存在（PurchaseInfoItemId={childDto.PurchaseInfoItemId}）");
                    }
                    if (target.PurchaseInfoId != entity.Id)
                    {
                        throw new TaktBusinessException("采购信息行不属于当前主表（PurchaseInfoItemId={childDto.PurchaseInfoItemId}）");
                    }
                    submittedIds.Add(childDto.PurchaseInfoItemId);
                    childDto.Adapt(target);
                    target.Id = childDto.PurchaseInfoItemId;
                    target.PurchaseInfoId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchaseInfoItemRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktPurchaseInfoItem>();
                    child.Id = 0;
                    child.PurchaseInfoId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchaseInfoItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                await _purchaseInfoItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购信息查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseInfo, bool>> QueryExpression(TaktPurchaseInfoQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseInfo>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchaseInfoCode != null && x.PurchaseInfoCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseInfoCode))
        {
            var purchaseInfoCode = queryDto.PurchaseInfoCode;
            exp = exp.And(x => x.PurchaseInfoCode != null && x.PurchaseInfoCode.Contains(purchaseInfoCode));
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
    private static bool HasAnyListQueryFilter(TaktPurchaseInfoQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseInfoCode))
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
