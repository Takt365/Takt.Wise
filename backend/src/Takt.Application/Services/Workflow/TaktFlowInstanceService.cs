// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow
// 文件名称：TaktFlowInstanceService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：流程实例应用服务实现
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
using Takt.Shared.Enums;

namespace Takt.Application.Services.Workflow;

/// <summary>
/// 流程实例应用服务
/// </summary>
public class TaktFlowInstanceService : TaktServiceBase, ITaktFlowInstanceService
{
    private readonly ITaktCompanyRepository<TaktFlowInstance> _flowInstanceRepository;
    private readonly ITaktCompanyRepository<TaktFlowTask> _flowTaskRepository;
    private readonly ITaktCompanyRepository<TaktFlowTransition> _flowTransitionRepository;
    private readonly ITaktCompanyRepository<TaktFlowVariable> _flowVariableRepository;
    private readonly ITaktCompanyRepository<TaktFlowAddSign> _flowAddSignRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowInstanceRepository">流程实例仓储</param>
    /// <param name="flowTaskRepository">FlowTask仓储</param>
    /// <param name="flowTransitionRepository">FlowTransition仓储</param>
    /// <param name="flowVariableRepository">FlowVariable仓储</param>
    /// <param name="flowAddSignRepository">FlowAddSign仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFlowInstanceService(
        ITaktCompanyRepository<TaktFlowInstance> flowInstanceRepository,
        ITaktCompanyRepository<TaktFlowTask> flowTaskRepository,
        ITaktCompanyRepository<TaktFlowTransition> flowTransitionRepository,
        ITaktCompanyRepository<TaktFlowVariable> flowVariableRepository,
        ITaktCompanyRepository<TaktFlowAddSign> flowAddSignRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowInstanceRepository = flowInstanceRepository;
        _flowTaskRepository = flowTaskRepository;
        _flowTransitionRepository = flowTransitionRepository;
        _flowVariableRepository = flowVariableRepository;
        _flowAddSignRepository = flowAddSignRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取流程实例列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowInstanceDto>> GetFlowInstanceListAsync(TaktFlowInstanceQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _flowInstanceRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFlowInstanceDto>.Create(
            data.Adapt<List<TaktFlowInstanceDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取流程实例
    /// </summary>
    /// <param name="id">流程实例ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowInstanceDto?> GetFlowInstanceByIdAsync(long id)
    {
        var entity = await _flowInstanceRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktFlowInstanceDto>();
        await FillFlowInstanceDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取流程实例选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFlowInstanceOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _flowInstanceRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ProcessName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.ProcessName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建流程实例
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowInstanceDto> CreateFlowInstanceAsync(TaktFlowInstanceCreateDto dto)
    {
        var entity = dto.Adapt<TaktFlowInstance>();
        var isUnique_ix_flow_instance_code_unique = await _uniqueValidator.IsUniqueAsync(
            _flowInstanceRepository,
            x => x.InstanceCode == entity.InstanceCode);
        if (!isUnique_ix_flow_instance_code_unique)
        {
            throw new TaktBusinessException("流程实例的InstanceCode已存在");
        }
        entity = await _flowInstanceRepository.CreateAsync(entity);
                await SaveFlowInstanceChildrenAsync(entity, dto);
        return await GetFlowInstanceByIdAsync(entity.Id) ?? entity.Adapt<TaktFlowInstanceDto>();
    }

    /// <summary>
    /// 更新流程实例
    /// </summary>
    /// <param name="id">流程实例ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowInstanceDto> UpdateFlowInstanceAsync(long id, TaktFlowInstanceUpdateDto dto)
    {
        var entity = await _flowInstanceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("流程实例不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_flow_instance_code_unique = await _uniqueValidator.IsUniqueAsync(
            _flowInstanceRepository,
            x => x.InstanceCode == entity.InstanceCode,
            id);
        if (!isUnique_ix_flow_instance_code_unique)
        {
            throw new TaktBusinessException("流程实例的InstanceCode已存在");
        }
        await _flowInstanceRepository.UpdateAsync(entity);
                await SaveFlowInstanceChildrenAsync(entity, dto);
        return await GetFlowInstanceByIdAsync(id) ?? throw new TaktBusinessException("流程实例不存在");
    }

    /// <summary>
    /// 删除流程实例
    /// </summary>
    /// <param name="id">流程实例ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowInstanceByIdAsync(long id)
    {
        var entity = await _flowInstanceRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("流程实例不存在或已删除");
        }
        await _flowTaskRepository.DeleteAsync(x => x.InstanceId == entity.Id);
        await _flowTransitionRepository.DeleteAsync(x => x.InstanceId == entity.Id);
        await _flowVariableRepository.DeleteAsync(x => x.InstanceId == entity.Id);
        await _flowAddSignRepository.DeleteAsync(x => x.InstanceId == entity.Id);
        var deleted = await _flowInstanceRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("流程实例不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除流程实例
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowInstanceBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFlowInstanceByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新流程实例状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowInstanceDto> UpdateFlowInstanceStatusAsync(TaktFlowInstanceStatusDto dto)
    {
        var entity = await _flowInstanceRepository.GetByIdAsync(dto.FlowInstanceId);
        if (entity == null)
        {
            throw new TaktBusinessException("流程实例不存在");
        }
        entity.InstanceStatus = (int)dto.InstanceStatus;
        await _flowInstanceRepository.UpdateAsync(entity);
        return await GetFlowInstanceByIdAsync(dto.FlowInstanceId) ?? throw new TaktBusinessException("流程实例不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFlowInstanceTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFlowInstanceTemplateDto>(
            sheetName ?? "流程实例导入模板",
            fileName ?? "流程实例导入模板.xlsx");
    }

    /// <summary>
    /// 导入流程实例
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFlowInstanceAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFlowInstanceImportDto>(fileStream, sheetName ?? "流程实例导入模板");
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
                var entity = rows[i].Adapt<TaktFlowInstance>();
                var importKey = $"{entity.InstanceCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（InstanceCode）");
                }
                var isUnique_ix_flow_instance_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _flowInstanceRepository,
                    x => x.InstanceCode == entity.InstanceCode);
                if (!isUnique_ix_flow_instance_code_unique)
                {
                    throw new TaktBusinessException("流程实例的InstanceCode已存在");
                }
                await _flowInstanceRepository.CreateAsync(entity);
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
    /// 导出流程实例
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFlowInstanceAsync(TaktFlowInstanceQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktFlowInstanceQueryDto());
        var list = await _flowInstanceRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowInstanceExportDto>(),
                sheetName ?? "流程实例数据",
                fileName ?? "流程实例导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFlowInstanceExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "流程实例数据",
            fileName ?? "流程实例导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充流程实例详情（加载 OneToMany 子表：流程用户任务、流程流转历史、流程变量、流程加签记录）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillFlowInstanceDetailsAsync(TaktFlowInstanceDto dto, TaktFlowInstance entity)
    {
        if (dto == null)
        {
            return;
        }
        // 流程用户任务 → dto.Tasks
        var tasks = await _flowTaskRepository.GetListAsync(x => x.InstanceId == entity.Id);
        dto.Tasks = tasks.Adapt<List<TaktFlowTaskDto>>();
        // 流程流转历史 → dto.HistoricActivities
        var historicactivities = await _flowTransitionRepository.GetListAsync(x => x.InstanceId == entity.Id);
        dto.HistoricActivities = historicactivities.Adapt<List<TaktFlowTransitionDto>>();
        // 流程变量 → dto.Variables
        var variables = await _flowVariableRepository.GetListAsync(x => x.InstanceId == entity.Id);
        dto.Variables = variables.Adapt<List<TaktFlowVariableDto>>();
        // 流程加签记录 → dto.AddSigns
        var addsigns = await _flowAddSignRepository.GetListAsync(x => x.InstanceId == entity.Id);
        dto.AddSigns = addsigns.Adapt<List<TaktFlowAddSignDto>>();
    }

    /// <summary>
    /// 保存流程实例子表级联（流程用户任务、流程流转历史、流程变量、流程加签记录；Create/Update 后按主表 Id 先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveFlowInstanceChildrenAsync(TaktFlowInstance entity, TaktFlowInstanceCreateDto dto)
    {
        // 流程用户任务（Tasks）
        if (dto.Tasks is not { Count: > 0 })
        {
            await _flowTaskRepository.DeleteAsync(x => x.InstanceId == entity.Id);
        }
        else
        {
            var tasks = dto.Tasks.Adapt<List<TaktFlowTask>>();
            foreach (var child in tasks)
            {
                child.InstanceId = entity.Id;
            }
            var tasksNeedSort = tasks.Where(c => c.SortOrder <= 0).ToList();
            if (tasksNeedSort.Count > 0)
            {
                var maxSort = await _flowTaskRepository.GetMaxIntAsync(
                    x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InstanceId == entity.Id,
                    x => x.SortOrder);
                var sortSeq = _sortOrderGenerator.GenerateSequenceForMaster(entity.Id, tasksNeedSort.Count, maxSort).ToList();
                var sortIdx = 0;
                foreach (var child in tasks)
                {
                    if (child.SortOrder <= 0)
                    {
                        child.SortOrder = sortSeq[sortIdx++];
                    }
                }
            }
            await _flowTaskRepository.DeleteAsync(x => x.InstanceId == entity.Id);
            foreach (var child in tasks)
            {
            }
            await _flowTaskRepository.CreateRangeAsync(tasks);
        }
        // 流程流转历史（HistoricActivities）
        if (dto.HistoricActivities is not { Count: > 0 })
        {
            await _flowTransitionRepository.DeleteAsync(x => x.InstanceId == entity.Id);
        }
        else
        {
            var historicactivities = dto.HistoricActivities.Adapt<List<TaktFlowTransition>>();
            foreach (var child in historicactivities)
            {
                child.InstanceId = entity.Id;
            }
            await _flowTransitionRepository.DeleteAsync(x => x.InstanceId == entity.Id);
            foreach (var child in historicactivities)
            {
            }
            await _flowTransitionRepository.CreateRangeAsync(historicactivities);
        }
        // 流程变量（Variables）
        if (dto.Variables is not { Count: > 0 })
        {
            await _flowVariableRepository.DeleteAsync(x => x.InstanceId == entity.Id);
        }
        else
        {
            var variables = dto.Variables.Adapt<List<TaktFlowVariable>>();
            foreach (var child in variables)
            {
                child.InstanceId = entity.Id;
            }
                        var seenKeys = new HashSet<string>(StringComparer.Ordinal);
                        for (var i = 0; i < variables.Count; i++)
                        {
                            var key = $"{variables[i].CompanyCode}|{variables[i].InstanceId}|{variables[i].VariableName}";
                            if (!seenKeys.Add(key))
                            {
                                throw new TaktBusinessException($"流程变量第{i + 1}项与本次提交的其他项重复（CompanyCode、InstanceId、VariableName）");
                            }
                        }
            await _flowVariableRepository.DeleteAsync(x => x.InstanceId == entity.Id);
            foreach (var child in variables)
            {
            var isUnique_ix_flow_variable_instance_name = await _uniqueValidator.IsUniqueAsync(
                _flowVariableRepository,
                x => x.CompanyCode == child.CompanyCode
                    && x.InstanceId == child.InstanceId
                    && x.VariableName == child.VariableName);
            if (!isUnique_ix_flow_variable_instance_name)
            {
                throw new TaktBusinessException("流程变量的CompanyCode、InstanceId、VariableName已存在");
            }
            }
            await _flowVariableRepository.CreateRangeAsync(variables);
        }
        // 流程加签记录（AddSigns）
        if (dto.AddSigns is not { Count: > 0 })
        {
            await _flowAddSignRepository.DeleteAsync(x => x.InstanceId == entity.Id);
        }
        else
        {
            var addsigns = dto.AddSigns.Adapt<List<TaktFlowAddSign>>();
            foreach (var child in addsigns)
            {
                child.InstanceId = entity.Id;
            }
            await _flowAddSignRepository.DeleteAsync(x => x.InstanceId == entity.Id);
            foreach (var child in addsigns)
            {
            }
            await _flowAddSignRepository.CreateRangeAsync(addsigns);
        }
    }

    /// <summary>
    /// 获取流程实例统计（数据看板）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>流程实例统计</returns>
    public async Task<TaktWorkflowInstanceStatDto> GetWorkflowInstanceStatAsync(TaktWorkflowInstanceStatQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.StartTimeStart,
            queryDto.StartTimeEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktFlowInstance, bool>> basePredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && ((x.StartTime != null && x.StartTime >= start && x.StartTime <= end)
                || (x.StartTime == null && x.CreatedAt >= start && x.CreatedAt <= end));
        var monthInstanceCount = await _flowInstanceRepository.CountAsync(basePredicate);
        Expression<Func<TaktFlowInstance, bool>> runningPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.InstanceStatus == (int)TaktFlowInstanceStatus.Running
            && ((x.StartTime != null && x.StartTime >= start && x.StartTime <= end)
                || (x.StartTime == null && x.CreatedAt >= start && x.CreatedAt <= end));
        var monthRunningCount = await _flowInstanceRepository.CountAsync(runningPredicate);
        Expression<Func<TaktFlowInstance, bool>> completedPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.InstanceStatus == (int)TaktFlowInstanceStatus.Completed
            && ((x.StartTime != null && x.StartTime >= start && x.StartTime <= end)
                || (x.StartTime == null && x.CreatedAt >= start && x.CreatedAt <= end));
        var monthCompletedCount = await _flowInstanceRepository.CountAsync(completedPredicate);
        Expression<Func<TaktFlowInstance, bool>> rejectedPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.InstanceStatus == (int)TaktFlowInstanceStatus.Rejected
            && ((x.StartTime != null && x.StartTime >= start && x.StartTime <= end)
                || (x.StartTime == null && x.CreatedAt >= start && x.CreatedAt <= end));
        var monthRejectedCount = await _flowInstanceRepository.CountAsync(rejectedPredicate);
        Expression<Func<TaktFlowInstance, bool>> terminatedPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.InstanceStatus == (int)TaktFlowInstanceStatus.Terminated
            && ((x.StartTime != null && x.StartTime >= start && x.StartTime <= end)
                || (x.StartTime == null && x.CreatedAt >= start && x.CreatedAt <= end));
        var monthTerminatedCount = await _flowInstanceRepository.CountAsync(terminatedPredicate);
        return new TaktWorkflowInstanceStatDto
        {
            StatMonth = statMonth,
            MonthInstanceCount = monthInstanceCount,
            MonthRunningCount = monthRunningCount,
            MonthCompletedCount = monthCompletedCount,
            MonthRejectedCount = monthRejectedCount,
            MonthTerminatedCount = monthTerminatedCount,
        };
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建流程实例查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFlowInstance, bool>> QueryExpression(TaktFlowInstanceQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFlowInstance>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.InstanceCode != null && x.InstanceCode.Contains(keywords))
                || SqlFunc.ToString(x.ProcessDefinitionId).Contains(keywords)
                || (x.ProcessKey != null && x.ProcessKey.Contains(keywords))
                || (x.ProcessName != null && x.ProcessName.Contains(keywords))
                || SqlFunc.ToString(x.DefinitionVersion).Contains(keywords)
                || (x.ProcessTitle != null && x.ProcessTitle.Contains(keywords))
                || SqlFunc.ToString(x.InstanceStatus).Contains(keywords)
                || (x.CurrentActivityId != null && x.CurrentActivityId.Contains(keywords))
                || (x.CurrentActivityName != null && x.CurrentActivityName.Contains(keywords))
                || SqlFunc.ToString(x.StartUserId).Contains(keywords)
                || (x.StartUserName != null && x.StartUserName.Contains(keywords))
                || SqlFunc.ToString(x.DurationMs).Contains(keywords)
                || (x.BusinessKey != null && x.BusinessKey.Contains(keywords))
                || (x.BusinessType != null && x.BusinessType.Contains(keywords))
                || SqlFunc.ToString(x.SuperInstanceId).Contains(keywords)
                || (x.DeleteReason != null && x.DeleteReason.Contains(keywords))
                || (x.FrmData != null && x.FrmData.Contains(keywords))
                || SqlFunc.ToString(x.FormId).Contains(keywords)
                || (x.FormCode != null && x.FormCode.Contains(keywords))
                || (x.ProcessContentSnapshot != null && x.ProcessContentSnapshot.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.StartTime).Contains(keywords)
                || SqlFunc.ToString(x.EndTime).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.InstanceCode))
        {
            exp = exp.And(x => x.InstanceCode != null && x.InstanceCode.Contains(queryDto.InstanceCode));
        }

        if (queryDto?.ProcessDefinitionId.HasValue == true)
        {
            exp = exp.And(x => x.ProcessDefinitionId == queryDto.ProcessDefinitionId);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessKey))
        {
            exp = exp.And(x => x.ProcessKey != null && x.ProcessKey.Contains(queryDto.ProcessKey));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessName))
        {
            exp = exp.And(x => x.ProcessName != null && x.ProcessName.Contains(queryDto.ProcessName));
        }

        if (queryDto?.DefinitionVersion.HasValue == true)
        {
            exp = exp.And(x => x.DefinitionVersion == queryDto.DefinitionVersion);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessTitle))
        {
            exp = exp.And(x => x.ProcessTitle != null && x.ProcessTitle.Contains(queryDto.ProcessTitle));
        }

        if (queryDto?.InstanceStatus.HasValue == true)
        {
            exp = exp.And(x => x.InstanceStatus == (int)queryDto.InstanceStatus);
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrentActivityId))
        {
            exp = exp.And(x => x.CurrentActivityId != null && x.CurrentActivityId.Contains(queryDto.CurrentActivityId));
        }

        if (!string.IsNullOrEmpty(queryDto?.CurrentActivityName))
        {
            exp = exp.And(x => x.CurrentActivityName != null && x.CurrentActivityName.Contains(queryDto.CurrentActivityName));
        }

        if (queryDto?.StartUserId.HasValue == true)
        {
            exp = exp.And(x => x.StartUserId == queryDto.StartUserId);
        }

        if (!string.IsNullOrEmpty(queryDto?.StartUserName))
        {
            exp = exp.And(x => x.StartUserName != null && x.StartUserName.Contains(queryDto.StartUserName));
        }

        if (queryDto?.DurationMs.HasValue == true)
        {
            exp = exp.And(x => x.DurationMs == queryDto.DurationMs);
        }

        if (!string.IsNullOrEmpty(queryDto?.BusinessKey))
        {
            exp = exp.And(x => x.BusinessKey != null && x.BusinessKey.Contains(queryDto.BusinessKey));
        }

        if (!string.IsNullOrEmpty(queryDto?.BusinessType))
        {
            exp = exp.And(x => x.BusinessType != null && x.BusinessType.Contains(queryDto.BusinessType));
        }

        if (queryDto?.SuperInstanceId.HasValue == true)
        {
            exp = exp.And(x => x.SuperInstanceId == queryDto.SuperInstanceId);
        }

        if (!string.IsNullOrEmpty(queryDto?.DeleteReason))
        {
            exp = exp.And(x => x.DeleteReason != null && x.DeleteReason.Contains(queryDto.DeleteReason));
        }

        if (!string.IsNullOrEmpty(queryDto?.FrmData))
        {
            exp = exp.And(x => x.FrmData != null && x.FrmData.Contains(queryDto.FrmData));
        }

        if (queryDto?.FormId.HasValue == true)
        {
            exp = exp.And(x => x.FormId == queryDto.FormId);
        }

        if (!string.IsNullOrEmpty(queryDto?.FormCode))
        {
            exp = exp.And(x => x.FormCode != null && x.FormCode.Contains(queryDto.FormCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessContentSnapshot))
        {
            exp = exp.And(x => x.ProcessContentSnapshot != null && x.ProcessContentSnapshot.Contains(queryDto.ProcessContentSnapshot));
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        if (!string.IsNullOrEmpty(queryDto?.ExtField))
        {
            exp = exp.And(x => x.ExtField != null && x.ExtField.Contains(queryDto.ExtField));
        }

        if (!string.IsNullOrEmpty(queryDto?.Remark))
        {
            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.Remark));
        }

        if (queryDto?.StartTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.StartTime >= queryDto.StartTimeStart);
        }

        if (queryDto?.StartTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.StartTime <= queryDto.StartTimeEnd);
        }

        if (queryDto?.EndTimeStart.HasValue == true)
        {
            exp = exp.And(x => x.EndTime >= queryDto.EndTimeStart);
        }

        if (queryDto?.EndTimeEnd.HasValue == true)
        {
            exp = exp.And(x => x.EndTime <= queryDto.EndTimeEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }
        if (!string.IsNullOrWhiteSpace(queryDto?.PlantCode))
        {
            var plantCode = queryDto.PlantCode;
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(plantCode));
        }


        return exp.ToExpression();
    }
}
