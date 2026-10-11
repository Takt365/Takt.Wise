// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：TaktApsScheduleItemService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：APS排程明细应用服务实现
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
/// APS排程明细应用服务
/// </summary>
public class TaktApsScheduleItemService : TaktServiceBase, ITaktApsScheduleItemService
{
    private readonly ITaktCompanyRepository<TaktApsScheduleItem> _apsScheduleItemRepository;
    private readonly ITaktCompanyRepository<TaktApsSchedule> _apsScheduleRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="apsScheduleItemRepository">APS排程明细仓储</param>
    /// <param name="apsScheduleRepository">APS排程主仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktApsScheduleItemService(
        ITaktCompanyRepository<TaktApsScheduleItem> apsScheduleItemRepository,
        ITaktCompanyRepository<TaktApsSchedule> apsScheduleRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _apsScheduleItemRepository = apsScheduleItemRepository;
        _apsScheduleRepository = apsScheduleRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取APS排程明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktApsScheduleItemDto>> GetApsScheduleItemListAsync(TaktApsScheduleItemQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktApsScheduleItemDto>.Create(
                new List<TaktApsScheduleItemDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _apsScheduleItemRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktApsScheduleItemDto>.Create(
            data.Adapt<List<TaktApsScheduleItemDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取APS排程明细
    /// </summary>
    /// <param name="id">APS排程明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleItemDto?> GetApsScheduleItemByIdAsync(long id)
    {
        var entity = await _apsScheduleItemRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktApsScheduleItemDto>();
    }

    /// <summary>
    /// 获取APS排程明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetApsScheduleItemOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _apsScheduleItemRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ProcessStatus == 1 && x.IsObsolete == 0,
            x => x.ProductName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ApsScheduleCode,
            DictLabel = e.ProductName ?? e.ApsScheduleCode,
        }).ToList();
    }

    /// <summary>
    /// 创建APS排程明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleItemDto> CreateApsScheduleItemAsync(TaktApsScheduleItemCreateDto dto)
    {
        var entity = dto.Adapt<TaktApsScheduleItem>();
        entity.IsObsolete = 0;
        await StampApsScheduleItemApsScheduleAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_aps_schedule_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _apsScheduleItemRepository,
            x => x.ApsScheduleId == entity.ApsScheduleId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_aps_schedule_item_line_unique)
        {
            throw new TaktBusinessException("APS排程明细的ApsScheduleId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _apsScheduleItemRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ApsScheduleId == entity.ApsScheduleId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.ApsScheduleCode) ? entity.ApsScheduleCode : entity.ApsScheduleId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _apsScheduleItemRepository.CreateAsync(entity);
        return await GetApsScheduleItemByIdAsync(entity.Id) ?? entity.Adapt<TaktApsScheduleItemDto>();
    }

    /// <summary>
    /// 更新APS排程明细
    /// </summary>
    /// <param name="id">APS排程明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleItemDto> UpdateApsScheduleItemAsync(long id, TaktApsScheduleItemUpdateDto dto)
    {
        var entity = await _apsScheduleItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程明细不存在");
        }
        dto.Adapt(entity);
        await StampApsScheduleItemApsScheduleAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_aps_schedule_item_line_unique = await _uniqueValidator.IsUniqueAsync(
            _apsScheduleItemRepository,
            x => x.ApsScheduleId == entity.ApsScheduleId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_aps_schedule_item_line_unique)
        {
            throw new TaktBusinessException("APS排程明细的ApsScheduleId、LineNumber已存在");
        }
        await _apsScheduleItemRepository.UpdateAsync(entity);
        return await GetApsScheduleItemByIdAsync(id) ?? throw new TaktBusinessException("APS排程明细不存在");
    }

    /// <summary>
    /// 删除APS排程明细
    /// </summary>
    /// <param name="id">APS排程明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteApsScheduleItemByIdAsync(long id)
    {
        var entity = await _apsScheduleItemRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("APS排程明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("APS排程明细已作废");
        }
        entity.IsObsolete = 1;
        await _apsScheduleItemRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除APS排程明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteApsScheduleItemBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteApsScheduleItemByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新APS排程明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleItemDto> UpdateApsScheduleItemStatusAsync(TaktApsScheduleItemStatusDto dto)
    {
        var entity = await _apsScheduleItemRepository.GetByIdAsync(dto.ApsScheduleItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程明细不存在");
        }
        entity.ProcessStatus = dto.ProcessStatus;
        await _apsScheduleItemRepository.UpdateAsync(entity);
        return await GetApsScheduleItemByIdAsync(dto.ApsScheduleItemId) ?? throw new TaktBusinessException("APS排程明细不存在");
    }

    /// <summary>
    /// 更新APS排程明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktApsScheduleItemDto> UpdateApsScheduleItemObsoleteAsync(TaktApsScheduleItemObsoleteDto dto)
    {
        var entity = await _apsScheduleItemRepository.GetByIdAsync(dto.ApsScheduleItemId);
        if (entity == null)
        {
            throw new TaktBusinessException("APS排程明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("APS排程明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _apsScheduleItemRepository.UpdateAsync(entity);
        return await GetApsScheduleItemByIdAsync(dto.ApsScheduleItemId) ?? throw new TaktBusinessException("APS排程明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetApsScheduleItemTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktApsScheduleItemTemplateDto>(
            sheetName ?? "APS排程明细导入模板",
            fileName ?? "APS排程明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入APS排程明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportApsScheduleItemAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktApsScheduleItemImportDto>(fileStream, sheetName ?? "APS排程明细导入模板");
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
                var entity = rows[i].Adapt<TaktApsScheduleItem>();
                var importDto = rows[i].Adapt<TaktApsScheduleItemCreateDto>();
                await StampApsScheduleItemApsScheduleAsync(entity, importDto);
                var importKey = $"{entity.ApsScheduleId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ApsScheduleId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_aps_schedule_item_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _apsScheduleItemRepository,
                    x => x.ApsScheduleId == entity.ApsScheduleId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_aps_schedule_item_line_unique)
                {
                    throw new TaktBusinessException("APS排程明细的ApsScheduleId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _apsScheduleItemRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.ApsScheduleId == entity.ApsScheduleId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.ApsScheduleCode) ? entity.ApsScheduleCode : entity.ApsScheduleId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _apsScheduleItemRepository.CreateAsync(entity);
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
    /// 导出APS排程明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportApsScheduleItemAsync(TaktApsScheduleItemQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktApsScheduleItemQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsScheduleItemExportDto>(),
                sheetName ?? "APS排程明细数据",
                fileName ?? "APS排程明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _apsScheduleItemRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktApsScheduleItemExportDto>(),
                sheetName ?? "APS排程明细数据",
                fileName ?? "APS排程明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktApsScheduleItemExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "APS排程明细数据",
            fileName ?? "APS排程明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步APS排程明细主表外键（ManyToOne → APS排程主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampApsScheduleItemApsScheduleAsync(TaktApsScheduleItem entity, TaktApsScheduleItemCreateDto dto)
    {
        if (dto.ApsScheduleId <= 0)
        {
            return;
        }
        var master = await _apsScheduleRepository.GetByIdAsync(dto.ApsScheduleId);
        if (master == null)
        {
            throw new TaktBusinessException("APS排程主不存在");
        }
        entity.ApsScheduleId = master.Id;
        if (string.IsNullOrEmpty(entity.TenantCode))
        {
            entity.TenantCode = master.TenantCode;
        }
        if (string.IsNullOrEmpty(entity.CompanyCode))
        {
            entity.CompanyCode = master.CompanyCode;
        }
        if (string.IsNullOrEmpty(entity.CultureCode))
        {
            entity.CultureCode = master.CultureCode;
        }
        if (string.IsNullOrEmpty(entity.PlantCode))
        {
            entity.PlantCode = master.PlantCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建APS排程明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktApsScheduleItem, bool>> QueryExpression(TaktApsScheduleItemQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktApsScheduleItem>();

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
                || (x.ApsScheduleCode != null && x.ApsScheduleCode.Contains(keywords))
                || (x.WorkOrderCode != null && x.WorkOrderCode.Contains(keywords))
                || (x.ProductCode != null && x.ProductCode.Contains(keywords))
                || (x.ProductName != null && x.ProductName.Contains(keywords))
                || (x.WorkCenterCode != null && x.WorkCenterCode.Contains(keywords))
                || (x.WorkCenterDescription != null && x.WorkCenterDescription.Contains(keywords))
                || (x.ProcessCode != null && x.ProcessCode.Contains(keywords))
                || (x.ProcessName != null && x.ProcessName.Contains(keywords))
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

        if (queryDto?.ApsScheduleId.HasValue == true)
        {
            var apsScheduleId = queryDto.ApsScheduleId.Value;
            exp = exp.And(x => x.ApsScheduleId == apsScheduleId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApsScheduleCode))
        {
            var apsScheduleCode = queryDto.ApsScheduleCode;
            exp = exp.And(x => x.ApsScheduleCode != null && x.ApsScheduleCode.Contains(apsScheduleCode));
        }

        if (queryDto?.ApsOrderId.HasValue == true)
        {
            var apsOrderId = queryDto.ApsOrderId.Value;
            exp = exp.And(x => x.ApsOrderId == apsOrderId);
        }

        if (queryDto?.ApsOperationId.HasValue == true)
        {
            var apsOperationId = queryDto.ApsOperationId.Value;
            exp = exp.And(x => x.ApsOperationId == apsOperationId);
        }

        if (queryDto?.RoutingItemId.HasValue == true)
        {
            var routingItemId = queryDto.RoutingItemId.Value;
            exp = exp.And(x => x.RoutingItemId == routingItemId);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkOrderCode))
        {
            var workOrderCode = queryDto.WorkOrderCode;
            exp = exp.And(x => x.WorkOrderCode != null && x.WorkOrderCode.Contains(workOrderCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductCode))
        {
            var productCode = queryDto.ProductCode;
            exp = exp.And(x => x.ProductCode != null && x.ProductCode.Contains(productCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProductName))
        {
            var productName = queryDto.ProductName;
            exp = exp.And(x => x.ProductName != null && x.ProductName.Contains(productName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkCenterCode))
        {
            var workCenterCode = queryDto.WorkCenterCode;
            exp = exp.And(x => x.WorkCenterCode != null && x.WorkCenterCode.Contains(workCenterCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.WorkCenterDescription))
        {
            var workCenterDescription = queryDto.WorkCenterDescription;
            exp = exp.And(x => x.WorkCenterDescription != null && x.WorkCenterDescription.Contains(workCenterDescription));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProcessCode))
        {
            var processCode = queryDto.ProcessCode;
            exp = exp.And(x => x.ProcessCode != null && x.ProcessCode.Contains(processCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProcessName))
        {
            var processName = queryDto.ProcessName;
            exp = exp.And(x => x.ProcessName != null && x.ProcessName.Contains(processName));
        }

        if (queryDto?.ProcessSequence.HasValue == true)
        {
            var processSequence = queryDto.ProcessSequence.Value;
            exp = exp.And(x => x.ProcessSequence == processSequence);
        }

        if (queryDto?.ProcessStandardST.HasValue == true)
        {
            var processStandardST = queryDto.ProcessStandardST.Value;
            exp = exp.And(x => x.ProcessStandardST == processStandardST);
        }

        if (queryDto?.ProcessStandardSTUnit.HasValue == true)
        {
            var processStandardSTUnit = queryDto.ProcessStandardSTUnit.Value;
            exp = exp.And(x => x.ProcessStandardSTUnit == processStandardSTUnit);
        }

        if (queryDto?.ExtraMinutes.HasValue == true)
        {
            var extraMinutes = queryDto.ExtraMinutes.Value;
            exp = exp.And(x => x.ExtraMinutes == extraMinutes);
        }

        if (queryDto?.PlanQuantity.HasValue == true)
        {
            var planQuantity = queryDto.PlanQuantity.Value;
            exp = exp.And(x => x.PlanQuantity == planQuantity);
        }

        if (queryDto?.ProcessStatus.HasValue == true)
        {
            var processStatus = queryDto.ProcessStatus.Value;
            exp = exp.And(x => x.ProcessStatus == processStatus);
        }

        if (queryDto?.Priority.HasValue == true)
        {
            var priority = queryDto.Priority.Value;
            exp = exp.And(x => x.Priority == priority);
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

        if (queryDto?.PlanStartTimeStart.HasValue == true)
        {
            var planStartTimeStart = queryDto.PlanStartTimeStart.Value;
            exp = exp.And(x => x.PlanStartTime >= planStartTimeStart);
        }

        if (queryDto?.PlanStartTimeEnd.HasValue == true)
        {
            var planStartTimeEnd = queryDto.PlanStartTimeEnd.Value;
            exp = exp.And(x => x.PlanStartTime <= planStartTimeEnd);
        }

        if (queryDto?.PlanEndTimeStart.HasValue == true)
        {
            var planEndTimeStart = queryDto.PlanEndTimeStart.Value;
            exp = exp.And(x => x.PlanEndTime >= planEndTimeStart);
        }

        if (queryDto?.PlanEndTimeEnd.HasValue == true)
        {
            var planEndTimeEnd = queryDto.PlanEndTimeEnd.Value;
            exp = exp.And(x => x.PlanEndTime <= planEndTimeEnd);
        }

        if (queryDto?.ActualStartTimeStart.HasValue == true)
        {
            var actualStartTimeStart = queryDto.ActualStartTimeStart.Value;
            exp = exp.And(x => x.ActualStartTime >= actualStartTimeStart);
        }

        if (queryDto?.ActualStartTimeEnd.HasValue == true)
        {
            var actualStartTimeEnd = queryDto.ActualStartTimeEnd.Value;
            exp = exp.And(x => x.ActualStartTime <= actualStartTimeEnd);
        }

        if (queryDto?.ActualEndTimeStart.HasValue == true)
        {
            var actualEndTimeStart = queryDto.ActualEndTimeStart.Value;
            exp = exp.And(x => x.ActualEndTime >= actualEndTimeStart);
        }

        if (queryDto?.ActualEndTimeEnd.HasValue == true)
        {
            var actualEndTimeEnd = queryDto.ActualEndTimeEnd.Value;
            exp = exp.And(x => x.ActualEndTime <= actualEndTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktApsScheduleItemQueryDto? queryDto)
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
        if (queryDto.ApsScheduleId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApsScheduleCode))
        {
            return true;
        }
        if (queryDto.ApsOrderId.HasValue)
        {
            return true;
        }
        if (queryDto.ApsOperationId.HasValue)
        {
            return true;
        }
        if (queryDto.RoutingItemId.HasValue)
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkOrderCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProductName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkCenterCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.WorkCenterDescription))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProcessCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProcessName))
        {
            return true;
        }
        if (queryDto.ProcessSequence.HasValue)
        {
            return true;
        }
        if (queryDto.ProcessStandardST.HasValue)
        {
            return true;
        }
        if (queryDto.ProcessStandardSTUnit.HasValue)
        {
            return true;
        }
        if (queryDto.ExtraMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.PlanQuantity.HasValue)
        {
            return true;
        }
        if (queryDto.ProcessStatus.HasValue)
        {
            return true;
        }
        if (queryDto.Priority.HasValue)
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
        if (queryDto.PlanStartTimeStart.HasValue || queryDto.PlanStartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.PlanEndTimeStart.HasValue || queryDto.PlanEndTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ActualStartTimeStart.HasValue || queryDto.ActualStartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ActualEndTimeStart.HasValue || queryDto.ActualEndTimeEnd.HasValue)
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
