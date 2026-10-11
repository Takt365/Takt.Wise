// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：TaktApsOrderService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：APS排程订单应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Aps;
using Takt.Domain.Entities.Logistics.Manufacturing.Aps;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Aps;

/// <summary>
/// APS排程订单应用服务
/// </summary>
public class TaktApsOrderService : TaktServiceBase, ITaktApsOrderService
{
    private readonly ITaktCompanyRepository<TaktApsOrder> _apsOrderRepository;
    private readonly ITaktCompanyRepository<TaktApsOperation> _apsOperationRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsOrderRepository">APS排程订单仓储</param>
    /// <param name="apsOperationRepository">ApsOperation仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktApsOrderService(
        ITaktCompanyRepository<TaktApsOrder> apsOrderRepository,
        ITaktCompanyRepository<TaktApsOperation> apsOperationRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _apsOrderRepository = apsOrderRepository;
        _apsOperationRepository = apsOperationRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取APS排程订单列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktApsOrderDto>> GetApsOrderListAsync(TaktApsOrderQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktApsOrderDto>.Create(
                new List<TaktApsOrderDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _apsOrderRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktApsOrderDto>.Create(
            data.Adapt<List<TaktApsOrderDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取APS排程订单
    /// </summary>
    /// <param name="id">APS排程订单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOrderDto?> GetApsOrderByIdAsync(long id)
    {
        var entity = await _apsOrderRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktApsOrderDto>();
        await FillApsOrderDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取APS排程订单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetApsOrderOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _apsOrderRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.OrderStatus == 1,
            x => x.ApsOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ApsOrderCode,
            DictLabel = e.ApsOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建APS排程订单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOrderDto> CreateApsOrderAsync(TaktApsOrderCreateDto dto)
    {
        var entity = dto.Adapt<TaktApsOrder>();
        var isUnique_ix_takt_logistics_manufacturing_aps_order_unique = await _uniqueValidator.IsUniqueAsync(
            _apsOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ApsOrderCode == entity.ApsOrderCode);
        if (!isUnique_ix_takt_logistics_manufacturing_aps_order_unique)
        {
            throw new TaktBusinessException("APS排程订单的PlantCode、ApsOrderCode已存在");
        }
        entity = await _apsOrderRepository.CreateAsync(entity);
                await SaveApsOrderChildrenAsync(entity, dto);
        return await GetApsOrderByIdAsync(entity.Id) ?? entity.Adapt<TaktApsOrderDto>();
    }

    /// <summary>
    /// 更新APS排程订单
    /// </summary>
    /// <param name="id">APS排程订单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOrderDto> UpdateApsOrderAsync(long id, TaktApsOrderUpdateDto dto)
    {
        var entity = await _apsOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程订单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_aps_order_unique = await _uniqueValidator.IsUniqueAsync(
            _apsOrderRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ApsOrderCode == entity.ApsOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_aps_order_unique)
        {
            throw new TaktBusinessException("APS排程订单的PlantCode、ApsOrderCode已存在");
        }
        await _apsOrderRepository.UpdateAsync(entity);
                await SaveApsOrderChildrenAsync(entity, dto);
        return await GetApsOrderByIdAsync(id) ?? throw new TaktBusinessException("APS排程订单不存在");
    }

    /// <summary>
    /// 删除APS排程订单
    /// </summary>
    /// <param name="id">APS排程订单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteApsOrderByIdAsync(long id)
    {
        var entity = await _apsOrderRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程订单不存在或已删除");
        }
        await _apsOperationRepository.DeleteAsync(x => x.ApsOrderId == entity.Id);
        var deleted = await _apsOrderRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("APS排程订单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除APS排程订单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteApsOrderBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteApsOrderByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新APS排程订单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsOrderDto> UpdateApsOrderStatusAsync(TaktApsOrderStatusDto dto)
    {
        var entity = await _apsOrderRepository.GetByIdAsync(dto.ApsOrderId);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程订单不存在");
        }
        entity.OrderStatus = dto.OrderStatus;
        await _apsOrderRepository.UpdateAsync(entity);
        return await GetApsOrderByIdAsync(dto.ApsOrderId) ?? throw new TaktBusinessException("APS排程订单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetApsOrderTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktApsOrderTemplateDto>(
            sheetName ?? "APS排程订单导入模板",
            fileName ?? "APS排程订单导入模板.xlsx");
    }

    /// <summary>
    /// 导入APS排程订单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportApsOrderAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktApsOrderImportDto>(fileStream, sheetName ?? "APS排程订单导入模板");
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
                var entity = rows[i].Adapt<TaktApsOrder>();
                var importKey = $"{entity.PlantCode}|{entity.ApsOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ApsOrderCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_aps_order_unique = await _uniqueValidator.IsUniqueAsync(
                    _apsOrderRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ApsOrderCode == entity.ApsOrderCode);
                if (!isUnique_ix_takt_logistics_manufacturing_aps_order_unique)
                {
                    throw new TaktBusinessException("APS排程订单的PlantCode、ApsOrderCode已存在");
                }
                await _apsOrderRepository.CreateAsync(entity);
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
    /// 导出APS排程订单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportApsOrderAsync(TaktApsOrderQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktApsOrderQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsOrderExportDto>(),
                sheetName ?? "APS排程订单数据",
                fileName ?? "APS排程订单导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _apsOrderRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsOrderExportDto>(),
                sheetName ?? "APS排程订单数据",
                fileName ?? "APS排程订单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktApsOrderExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "APS排程订单数据",
            fileName ?? "APS排程订单导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废APS工序排程标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="apsOrderId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkApsOperationsObsoleteAsync(long apsOrderId)
    {
        if (apsOrderId <= 0)
        {
            return;
        }
        var rows = await _apsOperationRepository.GetListAsync(
            x => x.ApsOrderId == apsOrderId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _apsOperationRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充APS排程订单详情（加载 OneToMany 子表：APS工序排程）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillApsOrderDetailsAsync(TaktApsOrderDto dto, TaktApsOrder entity)
    {
        if (dto == null)
        {
            return;
        }
        // APS工序排程 → dto.Operations（含作废行）
        var operations = await _apsOperationRepository.GetListAsync(x => x.ApsOrderId == entity.Id);
        dto.Operations = operations.Adapt<List<TaktApsOperationDto>>();
    }

    /// <summary>
    /// 保存APS排程订单子表级联（APS工序排程；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveApsOrderChildrenAsync(TaktApsOrder entity, TaktApsOrderCreateDto dto)
    {
        // APS工序排程（Operations）
        List<TaktApsOperationUpdateDto>? operationsForSave;
        if (dto is TaktApsOrderUpdateDto updateDtoForOperations && updateDtoForOperations.Operations != null)
        {
            operationsForSave = updateDtoForOperations.Operations;
        }
        else if (dto.Operations != null)
        {
            operationsForSave = dto.Operations.Adapt<List<TaktApsOperationUpdateDto>>();
        }
        else
        {
            operationsForSave = null;
        }
        if (operationsForSave is not { Count: > 0 })
        {
            await MarkApsOperationsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _apsOperationRepository.GetListAsync(x => x.ApsOrderId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktApsOperation>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < operationsForSave.Count; i++)
            {
                var childDto = operationsForSave[i];
                childDto.ApsOrderId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.ApsOrderCode = entity.ApsOrderCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("APS工序排程第{i + 1}项与本次提交的其他项重复（CompanyCode、ApsOrderId、LineNumber）");
                }
                if (childDto.ApsOperationId > 0)
                {
                    if (!existingById.TryGetValue(childDto.ApsOperationId, out var target))
                    {
                        throw new TaktBusinessException("APS工序排程不存在（ApsOperationId={childDto.ApsOperationId}）");
                    }
                    if (target.ApsOrderId != entity.Id)
                    {
                        throw new TaktBusinessException("APS工序排程不属于当前主表（ApsOperationId={childDto.ApsOperationId}）");
                    }
                    submittedIds.Add(childDto.ApsOperationId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_aps_operation_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _apsOperationRepository,
                        x => x.ApsOrderId == x.ApsOrderId
                && x.LineNumber == x.LineNumber,
                        childDto.ApsOperationId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_aps_operation_line_unique)
                    {
                        throw new TaktBusinessException("APS工序排程的ApsOrderId、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.ApsOperationId;
                    target.ApsOrderId = entity.Id;
                    target.IsObsolete = 0;
                    await _apsOperationRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_aps_operation_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _apsOperationRepository,
                        x => x.ApsOrderId == x.ApsOrderId
                && x.LineNumber == x.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_aps_operation_line_unique)
                    {
                        throw new TaktBusinessException("APS工序排程的ApsOrderId、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktApsOperation>();
                    child.Id = 0;
                    child.ApsOrderId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _apsOperationRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.ApsOrderCode) ? entity.ApsOrderCode : entity.Id.ToString();
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
                await _apsOperationRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建APS排程订单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktApsOrder, bool>> QueryExpression(TaktApsOrderQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktApsOrder>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ApsOrderCode != null && x.ApsOrderCode.Contains(keywords))
                || (x.PlannedOrderCode != null && x.PlannedOrderCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(keywords))
                || (x.RoutingCode != null && x.RoutingCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ApsOrderCode))
        {
            var apsOrderCode = queryDto.ApsOrderCode;
            exp = exp.And(x => x.ApsOrderCode != null && x.ApsOrderCode.Contains(apsOrderCode));
        }

        if (queryDto?.PlannedOrderId.HasValue == true)
        {
            var plannedOrderId = queryDto.PlannedOrderId.Value;
            exp = exp.And(x => x.PlannedOrderId == plannedOrderId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PlannedOrderCode))
        {
            var plannedOrderCode = queryDto.PlannedOrderCode;
            exp = exp.And(x => x.PlannedOrderCode != null && x.PlannedOrderCode.Contains(plannedOrderCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (queryDto?.OrderQuantity.HasValue == true)
        {
            var orderQuantity = queryDto.OrderQuantity.Value;
            exp = exp.And(x => x.OrderQuantity == orderQuantity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UnitOfMeasure))
        {
            var unitOfMeasure = queryDto.UnitOfMeasure;
            exp = exp.And(x => x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(unitOfMeasure));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RoutingCode))
        {
            var routingCode = queryDto.RoutingCode;
            exp = exp.And(x => x.RoutingCode != null && x.RoutingCode.Contains(routingCode));
        }

        if (queryDto?.OrderStatus.HasValue == true)
        {
            var orderStatus = queryDto.OrderStatus.Value;
            exp = exp.And(x => x.OrderStatus == orderStatus);
        }

        if (queryDto?.ApsScheduleId.HasValue == true)
        {
            var apsScheduleId = queryDto.ApsScheduleId.Value;
            exp = exp.And(x => x.ApsScheduleId == apsScheduleId);
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

        if (queryDto?.PlannedStartTimeStart.HasValue == true)
        {
            var plannedStartTimeStart = queryDto.PlannedStartTimeStart.Value;
            exp = exp.And(x => x.PlannedStartTime >= plannedStartTimeStart);
        }

        if (queryDto?.PlannedStartTimeEnd.HasValue == true)
        {
            var plannedStartTimeEnd = queryDto.PlannedStartTimeEnd.Value;
            exp = exp.And(x => x.PlannedStartTime <= plannedStartTimeEnd);
        }

        if (queryDto?.PlannedEndTimeStart.HasValue == true)
        {
            var plannedEndTimeStart = queryDto.PlannedEndTimeStart.Value;
            exp = exp.And(x => x.PlannedEndTime >= plannedEndTimeStart);
        }

        if (queryDto?.PlannedEndTimeEnd.HasValue == true)
        {
            var plannedEndTimeEnd = queryDto.PlannedEndTimeEnd.Value;
            exp = exp.And(x => x.PlannedEndTime <= plannedEndTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktApsOrderQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ApsOrderCode))
        {
            return true;
        }
        if (queryDto.PlannedOrderId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PlannedOrderCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (queryDto.OrderQuantity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UnitOfMeasure))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RoutingCode))
        {
            return true;
        }
        if (queryDto.OrderStatus.HasValue)
        {
            return true;
        }
        if (queryDto.ApsScheduleId.HasValue)
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
        if (queryDto.PlannedStartTimeStart.HasValue || queryDto.PlannedStartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PlannedEndTimeStart.HasValue || queryDto.PlannedEndTimeEnd.HasValue)
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
