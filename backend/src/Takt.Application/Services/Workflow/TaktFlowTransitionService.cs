// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow
// 文件名称：TaktFlowTransitionService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：流程流转历史应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Workflow;
using Takt.Domain.Entities.Workflow;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Workflow;

/// <summary>
/// 流程流转历史应用服务
/// </summary>
public class TaktFlowTransitionService : TaktServiceBase, ITaktFlowTransitionService
{
    private readonly ITaktCompanyRepository<TaktFlowTransition> _flowTransitionRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowTransitionRepository">流程流转历史仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFlowTransitionService(
        ITaktCompanyRepository<TaktFlowTransition> flowTransitionRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowTransitionRepository = flowTransitionRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取流程流转历史列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowTransitionDto>> GetFlowTransitionListAsync(TaktFlowTransitionQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktFlowTransitionDto>.Create(
                new List<TaktFlowTransitionDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _flowTransitionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFlowTransitionDto>.Create(
            data.Adapt<List<TaktFlowTransitionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取流程流转历史
    /// </summary>
    /// <param name="id">流程流转历史ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTransitionDto?> GetFlowTransitionByIdAsync(long id)
    {
        var entity = await _flowTransitionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktFlowTransitionDto>();
    }

    /// <summary>
    /// 获取流程流转历史选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFlowTransitionOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _flowTransitionRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ActivityName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ActivityName ?? string.Empty,
            DictLabel = e.ActivityName ?? string.Empty,
        }).ToList();
    }

    /// <summary>
    /// 创建流程流转历史
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTransitionDto> CreateFlowTransitionAsync(TaktFlowTransitionCreateDto dto)
    {
        var entity = dto.Adapt<TaktFlowTransition>();
        entity = await _flowTransitionRepository.CreateAsync(entity);
        return await GetFlowTransitionByIdAsync(entity.Id) ?? entity.Adapt<TaktFlowTransitionDto>();
    }

    /// <summary>
    /// 更新流程流转历史
    /// </summary>
    /// <param name="id">流程流转历史ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTransitionDto> UpdateFlowTransitionAsync(long id, TaktFlowTransitionUpdateDto dto)
    {
        var entity = await _flowTransitionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("流程流转历史不存在");
        }
        dto.Adapt(entity);
        await _flowTransitionRepository.UpdateAsync(entity);
        return await GetFlowTransitionByIdAsync(id) ?? throw new TaktBusinessException("流程流转历史不存在");
    }

    /// <summary>
    /// 删除流程流转历史
    /// </summary>
    /// <param name="id">流程流转历史ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowTransitionByIdAsync(long id)
    {
        var deleted = await _flowTransitionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("流程流转历史不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除流程流转历史
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowTransitionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFlowTransitionByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFlowTransitionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFlowTransitionTemplateDto>(
            sheetName ?? "流程流转历史导入模板",
            fileName ?? "流程流转历史导入模板.xlsx");
    }

    /// <summary>
    /// 导入流程流转历史
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFlowTransitionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFlowTransitionImportDto>(fileStream, sheetName ?? "流程流转历史导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktFlowTransition>();
                await _flowTransitionRepository.CreateAsync(entity);
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
    /// 导出流程流转历史
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFlowTransitionAsync(TaktFlowTransitionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktFlowTransitionQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowTransitionExportDto>(),
                sheetName ?? "流程流转历史数据",
                fileName ?? "流程流转历史导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _flowTransitionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowTransitionExportDto>(),
                sheetName ?? "流程流转历史数据",
                fileName ?? "流程流转历史导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFlowTransitionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "流程流转历史数据",
            fileName ?? "流程流转历史导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建流程流转历史查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFlowTransition, bool>> QueryExpression(TaktFlowTransitionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFlowTransition>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ActivityId != null && x.ActivityId.Contains(keywords))
                || (x.ActivityName != null && x.ActivityName.Contains(keywords))
                || (x.ActivityType != null && x.ActivityType.Contains(keywords))
                || (x.FromNodeId != null && x.FromNodeId.Contains(keywords))
                || (x.FromNodeName != null && x.FromNodeName.Contains(keywords))
                || (x.ToNodeId != null && x.ToNodeId.Contains(keywords))
                || (x.ToNodeName != null && x.ToNodeName.Contains(keywords))
                || (x.TransitionUserName != null && x.TransitionUserName.Contains(keywords))
                || (x.TransitionComment != null && x.TransitionComment.Contains(keywords))
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

        if (queryDto?.InstanceId.HasValue == true)
        {
            var instanceId = queryDto.InstanceId.Value;
            exp = exp.And(x => x.InstanceId == instanceId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ActivityId))
        {
            var activityId = queryDto.ActivityId;
            exp = exp.And(x => x.ActivityId != null && x.ActivityId.Contains(activityId));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ActivityName))
        {
            var activityName = queryDto.ActivityName;
            exp = exp.And(x => x.ActivityName != null && x.ActivityName.Contains(activityName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ActivityType))
        {
            var activityType = queryDto.ActivityType;
            exp = exp.And(x => x.ActivityType != null && x.ActivityType.Contains(activityType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FromNodeId))
        {
            var fromNodeId = queryDto.FromNodeId;
            exp = exp.And(x => x.FromNodeId != null && x.FromNodeId.Contains(fromNodeId));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FromNodeName))
        {
            var fromNodeName = queryDto.FromNodeName;
            exp = exp.And(x => x.FromNodeName != null && x.FromNodeName.Contains(fromNodeName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ToNodeId))
        {
            var toNodeId = queryDto.ToNodeId;
            exp = exp.And(x => x.ToNodeId != null && x.ToNodeId.Contains(toNodeId));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ToNodeName))
        {
            var toNodeName = queryDto.ToNodeName;
            exp = exp.And(x => x.ToNodeName != null && x.ToNodeName.Contains(toNodeName));
        }

        if (queryDto?.TransitionUserId.HasValue == true)
        {
            var transitionUserId = queryDto.TransitionUserId.Value;
            exp = exp.And(x => x.TransitionUserId == transitionUserId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TransitionUserName))
        {
            var transitionUserName = queryDto.TransitionUserName;
            exp = exp.And(x => x.TransitionUserName != null && x.TransitionUserName.Contains(transitionUserName));
        }

        if (queryDto?.DurationMs.HasValue == true)
        {
            var durationMs = queryDto.DurationMs.Value;
            exp = exp.And(x => x.DurationMs == durationMs);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TransitionComment))
        {
            var transitionComment = queryDto.TransitionComment;
            exp = exp.And(x => x.TransitionComment != null && x.TransitionComment.Contains(transitionComment));
        }

        if (queryDto?.ActionType.HasValue == true)
        {
            var actionType = queryDto.ActionType.Value;
            exp = exp.And(x => x.ActionType == (int)actionType);
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

        if (queryDto?.StartTimeStart.HasValue == true)
        {
            var startTimeStart = queryDto.StartTimeStart.Value;
            exp = exp.And(x => x.StartTime >= startTimeStart);
        }

        if (queryDto?.StartTimeEnd.HasValue == true)
        {
            var startTimeEnd = queryDto.StartTimeEnd.Value;
            exp = exp.And(x => x.StartTime <= startTimeEnd);
        }

        if (queryDto?.TransitionTimeStart.HasValue == true)
        {
            var transitionTimeStart = queryDto.TransitionTimeStart.Value;
            exp = exp.And(x => x.TransitionTime >= transitionTimeStart);
        }

        if (queryDto?.TransitionTimeEnd.HasValue == true)
        {
            var transitionTimeEnd = queryDto.TransitionTimeEnd.Value;
            exp = exp.And(x => x.TransitionTime <= transitionTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktFlowTransitionQueryDto? queryDto)
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
        if (queryDto.InstanceId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ActivityId))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ActivityName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ActivityType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FromNodeId))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FromNodeName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ToNodeId))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ToNodeName))
        {
            return true;
        }
        if (queryDto.TransitionUserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TransitionUserName))
        {
            return true;
        }
        if (queryDto.DurationMs.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TransitionComment))
        {
            return true;
        }
        if (queryDto.ActionType.HasValue)
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
        if (queryDto.StartTimeStart.HasValue || queryDto.StartTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.TransitionTimeStart.HasValue || queryDto.TransitionTimeEnd.HasValue)
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
