// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Procurement
// 文件名称：TaktPurchaseForecastService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：采购预测应用服务实现
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
/// 采购预测应用服务
/// </summary>
public class TaktPurchaseForecastService : TaktServiceBase, ITaktPurchaseForecastService
{
    private readonly ITaktApprovalRepository<TaktPurchaseForecast> _purchaseForecastRepository;
    private readonly ITaktCompanyRepository<TaktPurchaseForecastItem> _purchaseForecastItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktNumberingGenerator _numberingGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="purchaseForecastRepository">采购预测仓储</param>
    /// <param name="purchaseForecastItemRepository">PurchaseForecastItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="numberingGenerator">业务编码生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPurchaseForecastService(
        ITaktApprovalRepository<TaktPurchaseForecast> purchaseForecastRepository,
        ITaktCompanyRepository<TaktPurchaseForecastItem> purchaseForecastItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktNumberingGenerator numberingGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _purchaseForecastRepository = purchaseForecastRepository;
        _purchaseForecastItemRepository = purchaseForecastItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
        _numberingGenerator = numberingGenerator;
    }

    /// <summary>
    /// 获取采购预测列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPurchaseForecastDto>> GetPurchaseForecastListAsync(TaktPurchaseForecastQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPurchaseForecastDto>.Create(
                new List<TaktPurchaseForecastDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _purchaseForecastRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPurchaseForecastDto>.Create(
            data.Adapt<List<TaktPurchaseForecastDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取采购预测
    /// </summary>
    /// <param name="id">采购预测ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseForecastDto?> GetPurchaseForecastByIdAsync(long id)
    {
        var entity = await _purchaseForecastRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPurchaseForecastDto>();
        await FillPurchaseForecastDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取采购预测选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPurchaseForecastOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _purchaseForecastRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PlanStatus == 1,
            x => x.PurchaseForecastCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.PurchaseForecastCode,
            DictLabel = e.PurchaseForecastCode,
        }).ToList();
    }

    /// <summary>
    /// 创建采购预测
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseForecastDto> CreatePurchaseForecastAsync(TaktPurchaseForecastCreateDto dto)
    {
        var entity = dto.Adapt<TaktPurchaseForecast>();
        if (!string.IsNullOrWhiteSpace(dto.NumberingRuleCode))
        {
            var generated = await _numberingGenerator.GenerateNextAsync(dto.NumberingRuleCode.Trim());
            if (string.IsNullOrWhiteSpace(generated.BusinessCode))
            {
                throw new TaktBusinessException("业务编码生成失败");
            }
            entity.PurchaseForecastCode = generated.BusinessCode;
        }
        else if (string.IsNullOrWhiteSpace(entity.PurchaseForecastCode))
        {
            throw new TaktBusinessException("采购预测编码不能为空");
        }
        var isUnique_ix_takt_logistics_procurement_purchase_forecast_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseForecastRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseForecastCode == entity.PurchaseForecastCode
                && x.SendVersionNo == entity.SendVersionNo);
        if (!isUnique_ix_takt_logistics_procurement_purchase_forecast_unique)
        {
            throw new TaktBusinessException("采购预测的PlantCode、PurchaseForecastCode、SendVersionNo已存在");
        }
        entity = await _purchaseForecastRepository.CreateAsync(entity);
                await SavePurchaseForecastChildrenAsync(entity, dto);
        return await GetPurchaseForecastByIdAsync(entity.Id) ?? entity.Adapt<TaktPurchaseForecastDto>();
    }

    /// <summary>
    /// 更新采购预测
    /// </summary>
    /// <param name="id">采购预测ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseForecastDto> UpdatePurchaseForecastAsync(long id, TaktPurchaseForecastUpdateDto dto)
    {
        var entity = await _purchaseForecastRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购预测不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_procurement_purchase_forecast_unique = await _uniqueValidator.IsUniqueAsync(
            _purchaseForecastRepository,
            x => x.PlantCode == entity.PlantCode
                && x.PurchaseForecastCode == entity.PurchaseForecastCode
                && x.SendVersionNo == entity.SendVersionNo,
            id);
        if (!isUnique_ix_takt_logistics_procurement_purchase_forecast_unique)
        {
            throw new TaktBusinessException("采购预测的PlantCode、PurchaseForecastCode、SendVersionNo已存在");
        }
        await _purchaseForecastRepository.UpdateAsync(entity);
                await SavePurchaseForecastChildrenAsync(entity, dto);
        return await GetPurchaseForecastByIdAsync(id) ?? throw new TaktBusinessException("采购预测不存在");
    }

    /// <summary>
    /// 删除采购预测
    /// </summary>
    /// <param name="id">采购预测ID</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseForecastByIdAsync(long id)
    {
        var entity = await _purchaseForecastRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("采购预测不存在或已删除");
        }
        await _purchaseForecastItemRepository.DeleteAsync(x => x.PurchaseForecastId == entity.Id);
        var deleted = await _purchaseForecastRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("采购预测不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除采购预测
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePurchaseForecastBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePurchaseForecastByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新采购预测状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPurchaseForecastDto> UpdatePurchaseForecastStatusAsync(TaktPurchaseForecastStatusDto dto)
    {
        var entity = await _purchaseForecastRepository.GetByIdAsync(dto.PurchaseForecastId);
        if (entity == null)
        {
            throw new TaktBusinessException("采购预测不存在");
        }
        entity.PlanStatus = dto.PlanStatus;
        await _purchaseForecastRepository.UpdateAsync(entity);
        return await GetPurchaseForecastByIdAsync(dto.PurchaseForecastId) ?? throw new TaktBusinessException("采购预测不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPurchaseForecastTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPurchaseForecastTemplateDto>(
            sheetName ?? "采购预测导入模板",
            fileName ?? "采购预测导入模板.xlsx");
    }

    /// <summary>
    /// 导入采购预测
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPurchaseForecastAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPurchaseForecastImportDto>(fileStream, sheetName ?? "采购预测导入模板");
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
                var entity = rows[i].Adapt<TaktPurchaseForecast>();
                var importKey = $"{entity.PlantCode}|{entity.PurchaseForecastCode}|{entity.SendVersionNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、PurchaseForecastCode、SendVersionNo）");
                }
                var isUnique_ix_takt_logistics_procurement_purchase_forecast_unique = await _uniqueValidator.IsUniqueAsync(
                    _purchaseForecastRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.PurchaseForecastCode == entity.PurchaseForecastCode
                        && x.SendVersionNo == entity.SendVersionNo);
                if (!isUnique_ix_takt_logistics_procurement_purchase_forecast_unique)
                {
                    throw new TaktBusinessException("采购预测的PlantCode、PurchaseForecastCode、SendVersionNo已存在");
                }
                await _purchaseForecastRepository.CreateAsync(entity);
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
    /// 导出采购预测
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPurchaseForecastAsync(TaktPurchaseForecastQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPurchaseForecastQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseForecastExportDto>(),
                sheetName ?? "采购预测数据",
                fileName ?? "采购预测导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _purchaseForecastRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPurchaseForecastExportDto>(),
                sheetName ?? "采购预测数据",
                fileName ?? "采购预测导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPurchaseForecastExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "采购预测数据",
            fileName ?? "采购预测导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废采购预测明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="purchaseForecastId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPurchaseForecastItemsObsoleteAsync(long purchaseForecastId)
    {
        if (purchaseForecastId <= 0)
        {
            return;
        }
        var rows = await _purchaseForecastItemRepository.GetListAsync(
            x => x.PurchaseForecastId == purchaseForecastId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _purchaseForecastItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充采购预测详情（加载 OneToMany 子表：采购预测明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPurchaseForecastDetailsAsync(TaktPurchaseForecastDto dto, TaktPurchaseForecast entity)
    {
        if (dto == null)
        {
            return;
        }
        // 采购预测明细 → dto.Items（含作废行）
        var items = await _purchaseForecastItemRepository.GetListAsync(x => x.PurchaseForecastId == entity.Id);
        dto.Items = items.Adapt<List<TaktPurchaseForecastItemDto>>();
    }

    /// <summary>
    /// 保存采购预测子表级联（采购预测明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePurchaseForecastChildrenAsync(TaktPurchaseForecast entity, TaktPurchaseForecastCreateDto dto)
    {
        // 采购预测明细（Items）
        List<TaktPurchaseForecastItemUpdateDto>? itemsForSave;
        if (dto is TaktPurchaseForecastUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktPurchaseForecastItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkPurchaseForecastItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _purchaseForecastItemRepository.GetListAsync(x => x.PurchaseForecastId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPurchaseForecastItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.PurchaseForecastId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.PurchaseForecastCode = entity.PurchaseForecastCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("采购预测明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PurchaseForecastId、LineNumber）");
                }
                if (childDto.PurchaseForecastItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PurchaseForecastItemId, out var target))
                    {
                        throw new TaktBusinessException("采购预测明细不存在（PurchaseForecastItemId={childDto.PurchaseForecastItemId}）");
                    }
                    if (target.PurchaseForecastId != entity.Id)
                    {
                        throw new TaktBusinessException("采购预测明细不属于当前主表（PurchaseForecastItemId={childDto.PurchaseForecastItemId}）");
                    }
                    submittedIds.Add(childDto.PurchaseForecastItemId);
                    childDto.Adapt(target);
                    target.Id = childDto.PurchaseForecastItemId;
                    target.PurchaseForecastId = entity.Id;
                    target.IsObsolete = 0;
                    await _purchaseForecastItemRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktPurchaseForecastItem>();
                    child.Id = 0;
                    child.PurchaseForecastId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _purchaseForecastItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.PurchaseForecastCode) ? entity.PurchaseForecastCode : entity.Id.ToString();
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
                await _purchaseForecastItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建采购预测查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPurchaseForecast, bool>> QueryExpression(TaktPurchaseForecastQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPurchaseForecast>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.PurchaseForecastCode != null && x.PurchaseForecastCode.Contains(keywords))
                || (x.SalesProduct != null && x.SalesProduct.Contains(keywords))
                || (x.ProductCategoryCode != null && x.ProductCategoryCode.Contains(keywords))
                || (x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.SupplierCode != null && x.SupplierCode.Contains(keywords))
                || (x.SupplierName1 != null && x.SupplierName1.Contains(keywords))
                || (x.PlannerGroupName != null && x.PlannerGroupName.Contains(keywords))
                || (x.PlanDescription != null && x.PlanDescription.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.PurchaseForecastCode))
        {
            var purchaseForecastCode = queryDto.PurchaseForecastCode;
            exp = exp.And(x => x.PurchaseForecastCode != null && x.PurchaseForecastCode.Contains(purchaseForecastCode));
        }

        if (queryDto?.SendVersionNo.HasValue == true)
        {
            var sendVersionNo = queryDto.SendVersionNo.Value;
            exp = exp.And(x => x.SendVersionNo == sendVersionNo);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesProduct))
        {
            var salesProduct = queryDto.SalesProduct;
            exp = exp.And(x => x.SalesProduct != null && x.SalesProduct.Contains(salesProduct));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductCategoryCode))
        {
            var productCategoryCode = queryDto.ProductCategoryCode;
            exp = exp.And(x => x.ProductCategoryCode != null && x.ProductCategoryCode.Contains(productCategoryCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProfitCenterCode))
        {
            var profitCenterCode = queryDto.ProfitCenterCode;
            exp = exp.And(x => x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(profitCenterCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ModelCode))
        {
            var modelCode = queryDto.ModelCode;
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(modelCode));
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierCode))
        {
            var supplierCode = queryDto.SupplierCode;
            exp = exp.And(x => x.SupplierCode != null && x.SupplierCode.Contains(supplierCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SupplierName1))
        {
            var supplierName1 = queryDto.SupplierName1;
            exp = exp.And(x => x.SupplierName1 != null && x.SupplierName1.Contains(supplierName1));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlannerGroupCode))
        {
            var plannerGroupCode = queryDto.PlannerGroupCode;
            exp = exp.And(x => x.PlannerGroupCode == plannerGroupCode);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlannerGroupName))
        {
            var planBy = queryDto.PlannerGroupName;
            exp = exp.And(x => x.PlannerGroupName != null && x.PlannerGroupName.Contains(planBy));
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

        if (queryDto?.ConvertedQuantity.HasValue == true)
        {
            var convertedQuantity = queryDto.ConvertedQuantity.Value;
            exp = exp.And(x => x.ConvertedQuantity == convertedQuantity);
        }

        if (queryDto?.ConvertedAmount.HasValue == true)
        {
            var convertedAmount = queryDto.ConvertedAmount.Value;
            exp = exp.And(x => x.ConvertedAmount == convertedAmount);
        }

        if (queryDto?.PlanStatus.HasValue == true)
        {
            var planStatus = queryDto.PlanStatus.Value;
            exp = exp.And(x => x.PlanStatus == planStatus);
        }

        if (queryDto?.ConvertedStatus.HasValue == true)
        {
            var convertedStatus = queryDto.ConvertedStatus.Value;
            exp = exp.And(x => x.ConvertedStatus == convertedStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlanDescription))
        {
            var planDescription = queryDto.PlanDescription;
            exp = exp.And(x => x.PlanDescription != null && x.PlanDescription.Contains(planDescription));
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

        if (queryDto?.PlanDateStart.HasValue == true)
        {
            var planDateStart = queryDto.PlanDateStart.Value;
            exp = exp.And(x => x.PlanDate >= planDateStart);
        }

        if (queryDto?.PlanDateEnd.HasValue == true)
        {
            var planDateEnd = queryDto.PlanDateEnd.Value;
            exp = exp.And(x => x.PlanDate <= planDateEnd);
        }

        if (queryDto?.SendDateStart.HasValue == true)
        {
            var sendDateStart = queryDto.SendDateStart.Value;
            exp = exp.And(x => x.SendDate >= sendDateStart);
        }

        if (queryDto?.SendDateEnd.HasValue == true)
        {
            var sendDateEnd = queryDto.SendDateEnd.Value;
            exp = exp.And(x => x.SendDate <= sendDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktPurchaseForecastQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.PurchaseForecastCode))
        {
            return true;
        }
        if (queryDto.SendVersionNo.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SalesProduct))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCategoryCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProfitCenterCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ModelCode))
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
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SupplierName1))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlannerGroupCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlannerGroupName))
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
        if (queryDto.ConvertedQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.ConvertedAmount.HasValue)
        {
            return true;
        }
        if (queryDto.PlanStatus.HasValue)
        {
            return true;
        }
        if (queryDto.ConvertedStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlanDescription))
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
        if (queryDto.PlanDateStart.HasValue || queryDto.PlanDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.SendDateStart.HasValue || queryDto.SendDateEnd.HasValue)
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
