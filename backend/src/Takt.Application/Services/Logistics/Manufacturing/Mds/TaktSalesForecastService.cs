// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Mds
// 文件名称：TaktSalesForecastService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：销售预测应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Mds;
using Takt.Domain.Entities.Logistics.Manufacturing.Mds;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Mds;

/// <summary>
/// 销售预测应用服务
/// </summary>
public class TaktSalesForecastService : TaktServiceBase, ITaktSalesForecastService
{
    private readonly ITaktApprovalRepository<TaktSalesForecast> _salesForecastRepository;
    private readonly ITaktCompanyRepository<TaktSalesForecastItem> _salesForecastItemRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="salesForecastRepository">销售预测仓储</param>
    /// <param name="salesForecastItemRepository">SalesForecastItem仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSalesForecastService(
        ITaktApprovalRepository<TaktSalesForecast> salesForecastRepository,
        ITaktCompanyRepository<TaktSalesForecastItem> salesForecastItemRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _salesForecastRepository = salesForecastRepository;
        _salesForecastItemRepository = salesForecastItemRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取销售预测列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSalesForecastDto>> GetSalesForecastListAsync(TaktSalesForecastQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSalesForecastDto>.Create(
                new List<TaktSalesForecastDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _salesForecastRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSalesForecastDto>.Create(
            data.Adapt<List<TaktSalesForecastDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取销售预测
    /// </summary>
    /// <param name="id">销售预测ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesForecastDto?> GetSalesForecastByIdAsync(long id)
    {
        var entity = await _salesForecastRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSalesForecastDto>();
        await FillSalesForecastDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取销售预测选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSalesForecastOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _salesForecastRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PlanStatus == 1,
            x => x.SalesForecastCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SalesForecastCode,
            DictLabel = e.SalesForecastCode,
        }).ToList();
    }

    /// <summary>
    /// 创建销售预测
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesForecastDto> CreateSalesForecastAsync(TaktSalesForecastCreateDto dto)
    {
        var entity = dto.Adapt<TaktSalesForecast>();
        var isUnique_ix_takt_logistics_manufacturing_mds_sales_forecast_unique = await _uniqueValidator.IsUniqueAsync(
            _salesForecastRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesForecastCode == entity.SalesForecastCode
                && x.ReceiveVersionNo == entity.ReceiveVersionNo);
        if (!isUnique_ix_takt_logistics_manufacturing_mds_sales_forecast_unique)
        {
            throw new TaktBusinessException("销售预测的PlantCode、SalesForecastCode、ReceiveVersionNo已存在");
        }
        entity = await _salesForecastRepository.CreateAsync(entity);
                await SaveSalesForecastChildrenAsync(entity, dto);
        return await GetSalesForecastByIdAsync(entity.Id) ?? entity.Adapt<TaktSalesForecastDto>();
    }

    /// <summary>
    /// 更新销售预测
    /// </summary>
    /// <param name="id">销售预测ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesForecastDto> UpdateSalesForecastAsync(long id, TaktSalesForecastUpdateDto dto)
    {
        var entity = await _salesForecastRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售预测不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_mds_sales_forecast_unique = await _uniqueValidator.IsUniqueAsync(
            _salesForecastRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SalesForecastCode == entity.SalesForecastCode
                && x.ReceiveVersionNo == entity.ReceiveVersionNo,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_mds_sales_forecast_unique)
        {
            throw new TaktBusinessException("销售预测的PlantCode、SalesForecastCode、ReceiveVersionNo已存在");
        }
        await _salesForecastRepository.UpdateAsync(entity);
                await SaveSalesForecastChildrenAsync(entity, dto);
        return await GetSalesForecastByIdAsync(id) ?? throw new TaktBusinessException("销售预测不存在");
    }

    /// <summary>
    /// 删除销售预测
    /// </summary>
    /// <param name="id">销售预测ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesForecastByIdAsync(long id)
    {
        var entity = await _salesForecastRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("销售预测不存在或已删除");
        }
        await _salesForecastItemRepository.DeleteAsync(x => x.SalesForecastId == entity.Id);
        var deleted = await _salesForecastRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("销售预测不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除销售预测
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSalesForecastBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSalesForecastByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新销售预测状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSalesForecastDto> UpdateSalesForecastStatusAsync(TaktSalesForecastStatusDto dto)
    {
        var entity = await _salesForecastRepository.GetByIdAsync(dto.SalesForecastId);
        if (entity == null)
        {
            throw new TaktBusinessException("销售预测不存在");
        }
        entity.PlanStatus = dto.PlanStatus;
        await _salesForecastRepository.UpdateAsync(entity);
        return await GetSalesForecastByIdAsync(dto.SalesForecastId) ?? throw new TaktBusinessException("销售预测不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSalesForecastTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSalesForecastTemplateDto>(
            sheetName ?? "销售预测导入模板",
            fileName ?? "销售预测导入模板.xlsx");
    }

    /// <summary>
    /// 导入销售预测
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSalesForecastAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSalesForecastImportDto>(fileStream, sheetName ?? "销售预测导入模板");
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
                var entity = rows[i].Adapt<TaktSalesForecast>();
                var importKey = $"{entity.PlantCode}|{entity.SalesForecastCode}|{entity.ReceiveVersionNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SalesForecastCode、ReceiveVersionNo）");
                }
                var isUnique_ix_takt_logistics_manufacturing_mds_sales_forecast_unique = await _uniqueValidator.IsUniqueAsync(
                    _salesForecastRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SalesForecastCode == entity.SalesForecastCode
                        && x.ReceiveVersionNo == entity.ReceiveVersionNo);
                if (!isUnique_ix_takt_logistics_manufacturing_mds_sales_forecast_unique)
                {
                    throw new TaktBusinessException("销售预测的PlantCode、SalesForecastCode、ReceiveVersionNo已存在");
                }
                await _salesForecastRepository.CreateAsync(entity);
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
    /// 导出销售预测
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSalesForecastAsync(TaktSalesForecastQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSalesForecastQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesForecastExportDto>(),
                sheetName ?? "销售预测数据",
                fileName ?? "销售预测导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _salesForecastRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSalesForecastExportDto>(),
                sheetName ?? "销售预测数据",
                fileName ?? "销售预测导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSalesForecastExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "销售预测数据",
            fileName ?? "销售预测导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废销售预测明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="salesForecastId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSalesForecastItemsObsoleteAsync(long salesForecastId)
    {
        if (salesForecastId <= 0)
        {
            return;
        }
        var rows = await _salesForecastItemRepository.GetListAsync(
            x => x.SalesForecastId == salesForecastId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _salesForecastItemRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充销售预测详情（加载 OneToMany 子表：销售预测明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSalesForecastDetailsAsync(TaktSalesForecastDto dto, TaktSalesForecast entity)
    {
        if (dto == null)
        {
            return;
        }
        // 销售预测明细 → dto.Items（含作废行）
        var items = await _salesForecastItemRepository.GetListAsync(x => x.SalesForecastId == entity.Id);
        dto.Items = items.Adapt<List<TaktSalesForecastItemDto>>();
    }

    /// <summary>
    /// 保存销售预测子表级联（销售预测明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSalesForecastChildrenAsync(TaktSalesForecast entity, TaktSalesForecastCreateDto dto)
    {
        // 销售预测明细（Items）
        List<TaktSalesForecastItemUpdateDto>? itemsForSave;
        if (dto is TaktSalesForecastUpdateDto updateDtoForItems && updateDtoForItems.Items != null)
        {
            itemsForSave = updateDtoForItems.Items;
        }
        else if (dto.Items != null)
        {
            itemsForSave = dto.Items.Adapt<List<TaktSalesForecastItemUpdateDto>>();
        }
        else
        {
            itemsForSave = null;
        }
        if (itemsForSave is not { Count: > 0 })
        {
            await MarkSalesForecastItemsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _salesForecastItemRepository.GetListAsync(x => x.SalesForecastId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSalesForecastItem>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < itemsForSave.Count; i++)
            {
                var childDto = itemsForSave[i];
                childDto.SalesForecastId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.SalesForecastCode = entity.SalesForecastCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("销售预测明细第{i + 1}项与本次提交的其他项重复（CompanyCode、SalesForecastId、LineNumber）");
                }
                if (childDto.SalesForecastItemId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SalesForecastItemId, out var target))
                    {
                        throw new TaktBusinessException("销售预测明细不存在（SalesForecastItemId={childDto.SalesForecastItemId}）");
                    }
                    if (target.SalesForecastId != entity.Id)
                    {
                        throw new TaktBusinessException("销售预测明细不属于当前主表（SalesForecastItemId={childDto.SalesForecastItemId}）");
                    }
                    submittedIds.Add(childDto.SalesForecastItemId);
                    childDto.Adapt(target);
                    target.Id = childDto.SalesForecastItemId;
                    target.SalesForecastId = entity.Id;
                    target.IsObsolete = 0;
                    await _salesForecastItemRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSalesForecastItem>();
                    child.Id = 0;
                    child.SalesForecastId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _salesForecastItemRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SalesForecastCode) ? entity.SalesForecastCode : entity.Id.ToString();
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
                await _salesForecastItemRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建销售预测查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSalesForecast, bool>> QueryExpression(TaktSalesForecastQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSalesForecast>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SalesForecastCode != null && x.SalesForecastCode.Contains(keywords))
                || (x.SalesProduct != null && x.SalesProduct.Contains(keywords))
                || (x.ProductCategoryCode != null && x.ProductCategoryCode.Contains(keywords))
                || (x.ProfitCenterCode != null && x.ProfitCenterCode.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.MaterialDescription != null && x.MaterialDescription.Contains(keywords))
                || (x.CustomerCode != null && x.CustomerCode.Contains(keywords))
                || (x.CustomerName1 != null && x.CustomerName1.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SalesForecastCode))
        {
            var salesForecastCode = queryDto.SalesForecastCode;
            exp = exp.And(x => x.SalesForecastCode != null && x.SalesForecastCode.Contains(salesForecastCode));
        }

        if (queryDto?.ReceiveVersionNo.HasValue == true)
        {
            var receiveVersionNo = queryDto.ReceiveVersionNo.Value;
            exp = exp.And(x => x.ReceiveVersionNo == receiveVersionNo);
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

        if (queryDto?.ReceiveDateStart.HasValue == true)
        {
            var receiveDateStart = queryDto.ReceiveDateStart.Value;
            exp = exp.And(x => x.ReceiveDate >= receiveDateStart);
        }

        if (queryDto?.ReceiveDateEnd.HasValue == true)
        {
            var receiveDateEnd = queryDto.ReceiveDateEnd.Value;
            exp = exp.And(x => x.ReceiveDate <= receiveDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktSalesForecastQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SalesForecastCode))
        {
            return true;
        }
        if (queryDto.ReceiveVersionNo.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CustomerName1))
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
        if (queryDto.ReceiveDateStart.HasValue || queryDto.ReceiveDateEnd.HasValue)
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
