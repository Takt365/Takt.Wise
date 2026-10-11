// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow
// 文件名称：TaktFlowTaskService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：流程用户任务应用服务实现
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
/// 流程用户任务应用服务
/// </summary>
public class TaktFlowTaskService : TaktServiceBase, ITaktFlowTaskService
{
    private readonly ITaktCompanyRepository<TaktFlowTask> _flowTaskRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowTaskRepository">流程用户任务仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFlowTaskService(
        ITaktCompanyRepository<TaktFlowTask> flowTaskRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowTaskRepository = flowTaskRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取流程用户任务列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowTaskDto>> GetFlowTaskListAsync(TaktFlowTaskQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktFlowTaskDto>.Create(
                new List<TaktFlowTaskDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _flowTaskRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFlowTaskDto>.Create(
            data.Adapt<List<TaktFlowTaskDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取流程用户任务
    /// </summary>
    /// <param name="id">流程用户任务ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTaskDto?> GetFlowTaskByIdAsync(long id)
    {
        var entity = await _flowTaskRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktFlowTaskDto>();
    }

    /// <summary>
    /// 获取流程用户任务选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFlowTaskOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _flowTaskRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TaskStatus == 1,
            x => x.TaskName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.TaskName ?? string.Empty,
            DictLabel = e.TaskName ?? string.Empty,
        }).ToList();
    }

    /// <summary>
    /// 创建流程用户任务
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTaskDto> CreateFlowTaskAsync(TaktFlowTaskCreateDto dto)
    {
        var entity = dto.Adapt<TaktFlowTask>();
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _flowTaskRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InstanceId == entity.InstanceId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.InstanceId, maxSort);
        }
        entity = await _flowTaskRepository.CreateAsync(entity);
        return await GetFlowTaskByIdAsync(entity.Id) ?? entity.Adapt<TaktFlowTaskDto>();
    }

    /// <summary>
    /// 更新流程用户任务
    /// </summary>
    /// <param name="id">流程用户任务ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTaskDto> UpdateFlowTaskAsync(long id, TaktFlowTaskUpdateDto dto)
    {
        var entity = await _flowTaskRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("流程用户任务不存在");
        }
        dto.Adapt(entity);
        await _flowTaskRepository.UpdateAsync(entity);
        return await GetFlowTaskByIdAsync(id) ?? throw new TaktBusinessException("流程用户任务不存在");
    }

    /// <summary>
    /// 删除流程用户任务
    /// </summary>
    /// <param name="id">流程用户任务ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowTaskByIdAsync(long id)
    {
        var deleted = await _flowTaskRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("流程用户任务不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除流程用户任务
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowTaskBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFlowTaskByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新流程用户任务状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTaskDto> UpdateFlowTaskStatusAsync(TaktFlowTaskStatusDto dto)
    {
        var entity = await _flowTaskRepository.GetByIdAsync(dto.FlowTaskId);
        if (entity == null)
        {
            throw new TaktBusinessException("流程用户任务不存在");
        }
        entity.TaskStatus = (int)dto.TaskStatus;
        await _flowTaskRepository.UpdateAsync(entity);
        return await GetFlowTaskByIdAsync(dto.FlowTaskId) ?? throw new TaktBusinessException("流程用户任务不存在");
    }

    /// <summary>
    /// 更新流程用户任务排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowTaskDto> UpdateFlowTaskSortAsync(TaktFlowTaskSortDto dto)
    {
        var entity = await _flowTaskRepository.GetByIdAsync(dto.FlowTaskId);
        if (entity == null)
        {
            throw new TaktBusinessException("流程用户任务不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _flowTaskRepository.UpdateAsync(entity);
        return await GetFlowTaskByIdAsync(dto.FlowTaskId) ?? throw new TaktBusinessException("流程用户任务不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFlowTaskTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFlowTaskTemplateDto>(
            sheetName ?? "流程用户任务导入模板",
            fileName ?? "流程用户任务导入模板.xlsx");
    }

    /// <summary>
    /// 导入流程用户任务
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFlowTaskAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFlowTaskImportDto>(fileStream, sheetName ?? "流程用户任务导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktFlowTask>();
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _flowTaskRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InstanceId == entity.InstanceId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.InstanceId, maxSort);
                }
                await _flowTaskRepository.CreateAsync(entity);
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
    /// 导出流程用户任务
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFlowTaskAsync(TaktFlowTaskQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktFlowTaskQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowTaskExportDto>(),
                sheetName ?? "流程用户任务数据",
                fileName ?? "流程用户任务导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _flowTaskRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowTaskExportDto>(),
                sheetName ?? "流程用户任务数据",
                fileName ?? "流程用户任务导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFlowTaskExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "流程用户任务数据",
            fileName ?? "流程用户任务导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建流程用户任务查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFlowTask, bool>> QueryExpression(TaktFlowTaskQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFlowTask>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.TaskDefinitionKey != null && x.TaskDefinitionKey.Contains(keywords))
                || (x.TaskName != null && x.TaskName.Contains(keywords))
                || (x.AssigneeUserName != null && x.AssigneeUserName.Contains(keywords))
                || (x.OwnerUserName != null && x.OwnerUserName.Contains(keywords))
                || (x.Comment != null && x.Comment.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.TaskDefinitionKey))
        {
            var taskDefinitionKey = queryDto.TaskDefinitionKey;
            exp = exp.And(x => x.TaskDefinitionKey != null && x.TaskDefinitionKey.Contains(taskDefinitionKey));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TaskName))
        {
            var taskName = queryDto.TaskName;
            exp = exp.And(x => x.TaskName != null && x.TaskName.Contains(taskName));
        }

        if (queryDto?.AssigneeUserId.HasValue == true)
        {
            var assigneeUserId = queryDto.AssigneeUserId.Value;
            exp = exp.And(x => x.AssigneeUserId == assigneeUserId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssigneeUserName))
        {
            var assigneeUserName = queryDto.AssigneeUserName;
            exp = exp.And(x => x.AssigneeUserName != null && x.AssigneeUserName.Contains(assigneeUserName));
        }

        if (queryDto?.OwnerUserId.HasValue == true)
        {
            var ownerUserId = queryDto.OwnerUserId.Value;
            exp = exp.And(x => x.OwnerUserId == ownerUserId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.OwnerUserName))
        {
            var ownerUserName = queryDto.OwnerUserName;
            exp = exp.And(x => x.OwnerUserName != null && x.OwnerUserName.Contains(ownerUserName));
        }

        if (queryDto?.SignType.HasValue == true)
        {
            var signType = queryDto.SignType.Value;
            exp = exp.And(x => x.SignType == (int)signType);
        }

        if (queryDto?.Priority.HasValue == true)
        {
            var priority = queryDto.Priority.Value;
            exp = exp.And(x => x.Priority == priority);
        }

        if (queryDto?.IsAddSign.HasValue == true)
        {
            var isAddSign = queryDto.IsAddSign.Value;
            exp = exp.And(x => x.IsAddSign == isAddSign);
        }

        if (queryDto?.AddSignId.HasValue == true)
        {
            var addSignId = queryDto.AddSignId.Value;
            exp = exp.And(x => x.AddSignId == addSignId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Comment))
        {
            var comment = queryDto.Comment;
            exp = exp.And(x => x.Comment != null && x.Comment.Contains(comment));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.TaskStatus.HasValue == true)
        {
            var taskStatus = queryDto.TaskStatus.Value;
            exp = exp.And(x => x.TaskStatus == (int)taskStatus);
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

        if (queryDto?.DueDateStart.HasValue == true)
        {
            var dueDateStart = queryDto.DueDateStart.Value;
            exp = exp.And(x => x.DueDate >= dueDateStart);
        }

        if (queryDto?.DueDateEnd.HasValue == true)
        {
            var dueDateEnd = queryDto.DueDateEnd.Value;
            exp = exp.And(x => x.DueDate <= dueDateEnd);
        }

        if (queryDto?.ClaimTimeStart.HasValue == true)
        {
            var claimTimeStart = queryDto.ClaimTimeStart.Value;
            exp = exp.And(x => x.ClaimTime >= claimTimeStart);
        }

        if (queryDto?.ClaimTimeEnd.HasValue == true)
        {
            var claimTimeEnd = queryDto.ClaimTimeEnd.Value;
            exp = exp.And(x => x.ClaimTime <= claimTimeEnd);
        }

        if (queryDto?.CompletedAtStart.HasValue == true)
        {
            var completedAtStart = queryDto.CompletedAtStart.Value;
            exp = exp.And(x => x.CompletedAt >= completedAtStart);
        }

        if (queryDto?.CompletedAtEnd.HasValue == true)
        {
            var completedAtEnd = queryDto.CompletedAtEnd.Value;
            exp = exp.And(x => x.CompletedAt <= completedAtEnd);
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
    private static bool HasAnyListQueryFilter(TaktFlowTaskQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.TaskDefinitionKey))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TaskName))
        {
            return true;
        }
        if (queryDto.AssigneeUserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssigneeUserName))
        {
            return true;
        }
        if (queryDto.OwnerUserId.HasValue)
        {
            return true;
        }
        if (queryDto.SignType.HasValue)
        {
            return true;
        }
        if (queryDto.Priority.HasValue)
        {
            return true;
        }
        if (queryDto.IsAddSign.HasValue)
        {
            return true;
        }
        if (queryDto.AddSignId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Comment))
        {
            return true;
        }
        if (queryDto.SortOrder.HasValue)
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
        if (queryDto.DueDateStart.HasValue || queryDto.DueDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ClaimTimeStart.HasValue || queryDto.ClaimTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CompletedAtStart.HasValue || queryDto.CompletedAtEnd.HasValue)
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
