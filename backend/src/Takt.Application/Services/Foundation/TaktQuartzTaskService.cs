// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：TaktQuartzTaskService.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Cursor AI)
// 功能描述：定时任务应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Foundation;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 定时任务应用服务（CRUD + Quartz 调度挂接）
/// </summary>
public class TaktQuartzTaskService : TaktServiceBase, ITaktQuartzTaskService
{
    /// <summary>
    /// 任务状态：正常（字典 sys_quartz_task_status）
    /// </summary>
    private const int TaskStatusNormal = 0;
    /// <summary>
    /// 任务状态：暂停（字典 sys_quartz_task_status）
    /// </summary>
    private const int TaskStatusPaused = 1;
    private readonly ITaktCompanyRepository<TaktQuartzTask> _quartzTaskRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktQuartzSchedulerManager _quartzSchedulerManager;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="quartzTaskRepository">定时任务仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="quartzSchedulerManager">Quartz 调度管理器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktQuartzTaskService(
        ITaktCompanyRepository<TaktQuartzTask> quartzTaskRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktQuartzSchedulerManager quartzSchedulerManager,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _quartzTaskRepository = quartzTaskRepository;
        _uniqueValidator = uniqueValidator;
        _quartzSchedulerManager = quartzSchedulerManager;
    }

    /// <summary>
    /// 获取定时任务列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktQuartzTaskDto>> GetQuartzTaskListAsync(TaktQuartzTaskQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktQuartzTaskDto>.Create(
                new List<TaktQuartzTaskDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _quartzTaskRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktQuartzTaskDto>.Create(
            data.Adapt<List<TaktQuartzTaskDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取定时任务
    /// </summary>
    /// <param name="id">定时任务ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzTaskDto?> GetQuartzTaskByIdAsync(long id)
    {
        var entity = await _quartzTaskRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktQuartzTaskDto>();
    }

    /// <summary>
    /// 获取定时任务选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetQuartzTaskOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _quartzTaskRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TaskStatus == TaskStatusNormal,
            x => x.TaskName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.TaskCode,
            DictLabel = e.TaskName ?? e.TaskCode,
        }).ToList();
    }

    /// <summary>
    /// 创建定时任务
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzTaskDto> CreateQuartzTaskAsync(TaktQuartzTaskCreateDto dto)
    {
        var entity = dto.Adapt<TaktQuartzTask>();
        var isUnique_ix_quartz_task_code_unique = await _uniqueValidator.IsUniqueAsync(
            _quartzTaskRepository,
            x => x.TaskCode == entity.TaskCode);
        if (!isUnique_ix_quartz_task_code_unique)
        {
            throw new TaktBusinessException("定时任务的TaskCode已存在");
        }
        entity = await _quartzTaskRepository.CreateAsync(entity);
        await _quartzSchedulerManager.ScheduleQuartzTaskAsync(entity, CurrentUserName);
        return await GetQuartzTaskByIdAsync(entity.Id) ?? entity.Adapt<TaktQuartzTaskDto>();
    }

    /// <summary>
    /// 更新定时任务
    /// </summary>
    /// <param name="id">定时任务ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzTaskDto> UpdateQuartzTaskAsync(long id, TaktQuartzTaskUpdateDto dto)
    {
        var entity = await _quartzTaskRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("定时任务不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_quartz_task_code_unique = await _uniqueValidator.IsUniqueAsync(
            _quartzTaskRepository,
            x => x.TaskCode == entity.TaskCode,
            id);
        if (!isUnique_ix_quartz_task_code_unique)
        {
            throw new TaktBusinessException("定时任务的TaskCode已存在");
        }
        await _quartzTaskRepository.UpdateAsync(entity);
        await _quartzSchedulerManager.ScheduleQuartzTaskAsync(entity, CurrentUserName);
        return await GetQuartzTaskByIdAsync(id) ?? throw new TaktBusinessException("定时任务不存在");
    }

    /// <summary>
    /// 删除定时任务
    /// </summary>
    /// <param name="id">定时任务ID</param>
    /// <returns>任务</returns>
    public async Task DeleteQuartzTaskByIdAsync(long id)
    {
        var entity = await _quartzTaskRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("定时任务不存在或已删除");
        }
        await _quartzSchedulerManager.RemoveQuartzTaskAsync(entity);
        var deleted = await _quartzTaskRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("定时任务不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除定时任务
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteQuartzTaskBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteQuartzTaskByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新定时任务状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzTaskDto> UpdateQuartzTaskStatusAsync(TaktQuartzTaskStatusDto dto)
    {
        var entity = await _quartzTaskRepository.GetByIdAsync(dto.QuartzTaskId);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("定时任务不存在");
        }
        entity.TaskStatus = dto.TaskStatus;
        await _quartzTaskRepository.UpdateAsync(entity);
        if (dto.TaskStatus == TaskStatusPaused)
        {
            await _quartzSchedulerManager.PauseQuartzTaskAsync(entity);
        }
        else
        {
            await _quartzSchedulerManager.StartQuartzTaskAsync(entity);
        }
        return await GetQuartzTaskByIdAsync(dto.QuartzTaskId) ?? throw new TaktBusinessException("定时任务不存在");
    }

    /// <summary>
    /// 立即执行一次定时任务（触发调度器 RunNow）
    /// </summary>
    /// <param name="id">定时任务ID</param>
    /// <param name="executeParams">本次执行参数（非空则覆盖任务配置 ExecuteParams）</param>
    /// <returns>DTO</returns>
    public async Task<TaktQuartzTaskDto> ExecuteQuartzTaskNowAsync(long id, string? executeParams = null)
    {
        var entity = await _quartzTaskRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("定时任务不存在");
        }
        await _quartzSchedulerManager.RunQuartzTaskNowAsync(entity, CurrentUserName, executeParams);
        return await GetQuartzTaskByIdAsync(id) ?? throw new TaktBusinessException("定时任务不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetQuartzTaskTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktQuartzTaskTemplateDto>(
            sheetName ?? "定时任务导入模板",
            fileName ?? "定时任务导入模板.xlsx");
    }

    /// <summary>
    /// 导入定时任务
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportQuartzTaskAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktQuartzTaskImportDto>(fileStream, sheetName ?? "定时任务导入模板");
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
                var entity = rows[i].Adapt<TaktQuartzTask>();
                var importKey = $"{entity.TaskCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（TaskCode）");
                }
                var isUnique_ix_quartz_task_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _quartzTaskRepository,
                    x => x.TaskCode == entity.TaskCode);
                if (!isUnique_ix_quartz_task_code_unique)
                {
                    throw new TaktBusinessException("定时任务的TaskCode已存在");
                }
                entity = await _quartzTaskRepository.CreateAsync(entity);
                await _quartzSchedulerManager.ScheduleQuartzTaskAsync(entity, CurrentUserName);
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
    /// 导出定时任务
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportQuartzTaskAsync(TaktQuartzTaskQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktQuartzTaskQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQuartzTaskExportDto>(),
                sheetName ?? "定时任务数据",
                fileName ?? "定时任务导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _quartzTaskRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktQuartzTaskExportDto>(),
                sheetName ?? "定时任务数据",
                fileName ?? "定时任务导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktQuartzTaskExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "定时任务数据",
            fileName ?? "定时任务导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建定时任务查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktQuartzTask, bool>> QueryExpression(TaktQuartzTaskQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktQuartzTask>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.TaskCode != null && x.TaskCode.Contains(keywords))
                || (x.TaskName != null && x.TaskName.Contains(keywords))
                || (x.JobName != null && x.JobName.Contains(keywords))
                || (x.JobGroup != null && x.JobGroup.Contains(keywords))
                || (x.TaskType != null && x.TaskType.Contains(keywords))
                || (x.AssemblyName != null && x.AssemblyName.Contains(keywords))
                || (x.ClassName != null && x.ClassName.Contains(keywords))
                || (x.ApiUrl != null && x.ApiUrl.Contains(keywords))
                || (x.RequestMethod != null && x.RequestMethod.Contains(keywords))
                || (x.SqlScript != null && x.SqlScript.Contains(keywords))
                || (x.CronExpression != null && x.CronExpression.Contains(keywords))
                || (x.ExecuteParams != null && x.ExecuteParams.Contains(keywords))
                || (x.TaskDescription != null && x.TaskDescription.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.TaskCode))
        {
            var taskCode = queryDto.TaskCode;
            exp = exp.And(x => x.TaskCode != null && x.TaskCode.Contains(taskCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaskName))
        {
            var taskName = queryDto.TaskName;
            exp = exp.And(x => x.TaskName != null && x.TaskName.Contains(taskName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.JobName))
        {
            var jobName = queryDto.JobName;
            exp = exp.And(x => x.JobName != null && x.JobName.Contains(jobName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.JobGroup))
        {
            var jobGroup = queryDto.JobGroup;
            exp = exp.And(x => x.JobGroup != null && x.JobGroup.Contains(jobGroup));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaskType))
        {
            var taskType = queryDto.TaskType;
            exp = exp.And(x => x.TaskType != null && x.TaskType.Contains(taskType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssemblyName))
        {
            var assemblyName = queryDto.AssemblyName;
            exp = exp.And(x => x.AssemblyName != null && x.AssemblyName.Contains(assemblyName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ClassName))
        {
            var className = queryDto.ClassName;
            exp = exp.And(x => x.ClassName != null && x.ClassName.Contains(className));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApiUrl))
        {
            var apiUrl = queryDto.ApiUrl;
            exp = exp.And(x => x.ApiUrl != null && x.ApiUrl.Contains(apiUrl));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RequestMethod))
        {
            var requestMethod = queryDto.RequestMethod;
            exp = exp.And(x => x.RequestMethod != null && x.RequestMethod.Contains(requestMethod));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SqlScript))
        {
            var sqlScript = queryDto.SqlScript;
            exp = exp.And(x => x.SqlScript != null && x.SqlScript.Contains(sqlScript));
        }

        if (queryDto?.TriggerType.HasValue == true)
        {
            var triggerType = queryDto.TriggerType.Value;
            exp = exp.And(x => x.TriggerType == triggerType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CronExpression))
        {
            var cronExpression = queryDto.CronExpression;
            exp = exp.And(x => x.CronExpression != null && x.CronExpression.Contains(cronExpression));
        }

        if (queryDto?.IntervalSeconds.HasValue == true)
        {
            var intervalSeconds = queryDto.IntervalSeconds.Value;
            exp = exp.And(x => x.IntervalSeconds == intervalSeconds);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ExecuteParams))
        {
            var executeParams = queryDto.ExecuteParams;
            exp = exp.And(x => x.ExecuteParams != null && x.ExecuteParams.Contains(executeParams));
        }

        if (queryDto?.Concurrent.HasValue == true)
        {
            var concurrent = queryDto.Concurrent.Value;
            exp = exp.And(x => x.Concurrent == concurrent);
        }

        if (queryDto?.MisfirePolicy.HasValue == true)
        {
            var misfirePolicy = queryDto.MisfirePolicy.Value;
            exp = exp.And(x => x.MisfirePolicy == misfirePolicy);
        }

        if (queryDto?.ExecuteCount.HasValue == true)
        {
            var executeCount = queryDto.ExecuteCount.Value;
            exp = exp.And(x => x.ExecuteCount == executeCount);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaskDescription))
        {
            var taskDescription = queryDto.TaskDescription;
            exp = exp.And(x => x.TaskDescription != null && x.TaskDescription.Contains(taskDescription));
        }

        if (queryDto?.TaskStatus.HasValue == true)
        {
            var taskStatus = queryDto.TaskStatus.Value;
            exp = exp.And(x => x.TaskStatus == taskStatus);
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

        if (queryDto?.FirstRunAtStart.HasValue == true)
        {
            var firstRunAtStart = queryDto.FirstRunAtStart.Value;
            exp = exp.And(x => x.FirstRunAt >= firstRunAtStart);
        }

        if (queryDto?.FirstRunAtEnd.HasValue == true)
        {
            var firstRunAtEnd = queryDto.FirstRunAtEnd.Value;
            exp = exp.And(x => x.FirstRunAt <= firstRunAtEnd);
        }

        if (queryDto?.LastRunAtStart.HasValue == true)
        {
            var lastRunAtStart = queryDto.LastRunAtStart.Value;
            exp = exp.And(x => x.LastRunAt >= lastRunAtStart);
        }

        if (queryDto?.LastRunAtEnd.HasValue == true)
        {
            var lastRunAtEnd = queryDto.LastRunAtEnd.Value;
            exp = exp.And(x => x.LastRunAt <= lastRunAtEnd);
        }

        if (queryDto?.NextRunAtStart.HasValue == true)
        {
            var nextRunAtStart = queryDto.NextRunAtStart.Value;
            exp = exp.And(x => x.NextRunAt >= nextRunAtStart);
        }

        if (queryDto?.NextRunAtEnd.HasValue == true)
        {
            var nextRunAtEnd = queryDto.NextRunAtEnd.Value;
            exp = exp.And(x => x.NextRunAt <= nextRunAtEnd);
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
    private static bool HasAnyListQueryFilter(TaktQuartzTaskQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.TaskCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaskName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.JobName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.JobGroup))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaskType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssemblyName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ClassName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApiUrl))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RequestMethod))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SqlScript))
        {
            return true;
        }
        if (queryDto.TriggerType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CronExpression))
        {
            return true;
        }
        if (queryDto.IntervalSeconds.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ExecuteParams))
        {
            return true;
        }
        if (queryDto.Concurrent.HasValue)
        {
            return true;
        }
        if (queryDto.MisfirePolicy.HasValue)
        {
            return true;
        }
        if (queryDto.ExecuteCount.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaskDescription))
        {
            return true;
        }
        if (queryDto.TaskStatus.HasValue)
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
        if (queryDto.FirstRunAtStart.HasValue || queryDto.FirstRunAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.LastRunAtStart.HasValue || queryDto.LastRunAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.NextRunAtStart.HasValue || queryDto.NextRunAtEnd.HasValue)
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
