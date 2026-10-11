// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow
// 文件名称：TaktFlowSchemeService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：流程定义应用服务实现
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
/// 流程定义应用服务
/// </summary>
public class TaktFlowSchemeService : TaktServiceBase, ITaktFlowSchemeService
{
    private readonly ITaktCompanyRepository<TaktFlowScheme> _flowSchemeRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;
    private readonly ITaktWorkflowSignalRNotifier _workflowSignalRNotifier;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowSchemeRepository">流程定义仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="workflowSignalRNotifier">工作流 SignalR 推送编排</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFlowSchemeService(
        ITaktCompanyRepository<TaktFlowScheme> flowSchemeRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktWorkflowSignalRNotifier workflowSignalRNotifier,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowSchemeRepository = flowSchemeRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _uniqueValidator = uniqueValidator;
        _workflowSignalRNotifier = workflowSignalRNotifier;
    }

    /// <summary>
    /// 获取流程定义列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowSchemeDto>> GetFlowSchemeListAsync(TaktFlowSchemeQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _flowSchemeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFlowSchemeDto>.Create(
            data.Adapt<List<TaktFlowSchemeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取流程定义
    /// </summary>
    /// <param name="id">流程定义ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowSchemeDto?> GetFlowSchemeByIdAsync(long id)
    {
        var entity = await _flowSchemeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktFlowSchemeDto>();
    }

    /// <summary>
    /// 获取流程定义选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFlowSchemeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _flowSchemeRepository.GetListAsync(
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
    /// 创建流程定义
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowSchemeDto> CreateFlowSchemeAsync(TaktFlowSchemeCreateDto dto)
    {
        var entity = dto.Adapt<TaktFlowScheme>();
        var isUnique_ix_flow_scheme_key_version_unique = await _uniqueValidator.IsUniqueAsync(
            _flowSchemeRepository,
            x => x.ProcessKey == entity.ProcessKey
                && x.DefinitionVersion == entity.DefinitionVersion);
        if (!isUnique_ix_flow_scheme_key_version_unique)
        {
            throw new TaktBusinessException("流程定义的ProcessKey、DefinitionVersion已存在");
        }
        EnsureSchemeFormLinked(entity.FormId.GetValueOrDefault(), entity.FormCode);
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _flowSchemeRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FormId == entity.FormId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.FormId.GetValueOrDefault(), maxSort);
        }
        entity = await _flowSchemeRepository.CreateAsync(entity);
        await NotifySchemeChangedAsync(entity, "create");
        return await GetFlowSchemeByIdAsync(entity.Id) ?? entity.Adapt<TaktFlowSchemeDto>();
    }

    /// <summary>
    /// 更新流程定义
    /// </summary>
    /// <param name="id">流程定义ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowSchemeDto> UpdateFlowSchemeAsync(long id, TaktFlowSchemeUpdateDto dto)
    {
        var entity = await _flowSchemeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("流程定义不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_flow_scheme_key_version_unique = await _uniqueValidator.IsUniqueAsync(
            _flowSchemeRepository,
            x => x.ProcessKey == entity.ProcessKey
                && x.DefinitionVersion == entity.DefinitionVersion,
            id);
        if (!isUnique_ix_flow_scheme_key_version_unique)
        {
            throw new TaktBusinessException("流程定义的ProcessKey、DefinitionVersion已存在");
        }
        EnsureSchemeFormLinked(entity.FormId.GetValueOrDefault(), entity.FormCode);
        await _flowSchemeRepository.UpdateAsync(entity);
        await NotifySchemeChangedAsync(entity, "update");
        return await GetFlowSchemeByIdAsync(id) ?? throw new TaktBusinessException("流程定义不存在");
    }

    /// <summary>
    /// 删除流程定义
    /// </summary>
    /// <param name="id">流程定义ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowSchemeByIdAsync(long id)
    {
        var entity = await _flowSchemeRepository.GetByIdAsync(id);
        var deleted = await _flowSchemeRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("流程定义不存在或已删除");
        }
        if (entity != null)
        {
            await NotifySchemeChangedAsync(entity, "delete");
        }
    }

    /// <summary>
    /// 批量删除流程定义
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowSchemeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFlowSchemeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新流程定义状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowSchemeDto> UpdateFlowSchemeStatusAsync(TaktFlowSchemeStatusDto dto)
    {
        var entity = await _flowSchemeRepository.GetByIdAsync(dto.FlowSchemeId);
        if (entity == null)
        {
            throw new TaktBusinessException("流程定义不存在");
        }
        if (dto.ProcessStatus == 1)
        {
            EnsureSchemeFormLinked(entity.FormId.GetValueOrDefault(), entity.FormCode);
        }
        entity.ProcessStatus = dto.ProcessStatus;
        await _flowSchemeRepository.UpdateAsync(entity);
        await NotifySchemeChangedAsync(entity, "status");
        return await GetFlowSchemeByIdAsync(dto.FlowSchemeId) ?? throw new TaktBusinessException("流程定义不存在");
    }

    /// <summary>
    /// 更新流程定义排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowSchemeDto> UpdateFlowSchemeSortAsync(TaktFlowSchemeSortDto dto)
    {
        var entity = await _flowSchemeRepository.GetByIdAsync(dto.FlowSchemeId);
        if (entity == null)
        {
            throw new TaktBusinessException("流程定义不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _flowSchemeRepository.UpdateAsync(entity);
        return await GetFlowSchemeByIdAsync(dto.FlowSchemeId) ?? throw new TaktBusinessException("流程定义不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFlowSchemeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFlowSchemeTemplateDto>(
            sheetName ?? "流程定义导入模板",
            fileName ?? "流程定义导入模板.xlsx");
    }

    /// <summary>
    /// 导入流程定义
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFlowSchemeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFlowSchemeImportDto>(fileStream, sheetName ?? "流程定义导入模板");
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
                var entity = rows[i].Adapt<TaktFlowScheme>();
                var importKey = $"{entity.ProcessKey}|{entity.DefinitionVersion}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ProcessKey、DefinitionVersion）");
                }
                var isUnique_ix_flow_scheme_key_version_unique = await _uniqueValidator.IsUniqueAsync(
                    _flowSchemeRepository,
                    x => x.ProcessKey == entity.ProcessKey
                        && x.DefinitionVersion == entity.DefinitionVersion);
                if (!isUnique_ix_flow_scheme_key_version_unique)
                {
                    throw new TaktBusinessException("流程定义的ProcessKey、DefinitionVersion已存在");
                }
                EnsureSchemeFormLinked(entity.FormId.GetValueOrDefault(), entity.FormCode);
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _flowSchemeRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.FormId == entity.FormId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.FormId.GetValueOrDefault(), maxSort);
                }
                await _flowSchemeRepository.CreateAsync(entity);
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
    /// 导出流程定义
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFlowSchemeAsync(TaktFlowSchemeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktFlowSchemeQueryDto());
        var list = await _flowSchemeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowSchemeExportDto>(),
                sheetName ?? "流程定义数据",
                fileName ?? "流程定义导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFlowSchemeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "流程定义数据",
            fileName ?? "流程定义导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建流程定义查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFlowScheme, bool>> QueryExpression(TaktFlowSchemeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFlowScheme>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.ProcessKey != null && x.ProcessKey.Contains(keywords))
                || (x.ProcessName != null && x.ProcessName.Contains(keywords))
                || SqlFunc.ToString(x.DefinitionVersion).Contains(keywords)
                || (x.ProcessVersion != null && x.ProcessVersion.Contains(keywords))
                || SqlFunc.ToString(x.IsLatest).Contains(keywords)
                || SqlFunc.ToString(x.ProcessCategory).Contains(keywords)
                || (x.ProcessDescription != null && x.ProcessDescription.Contains(keywords))
                || SqlFunc.ToString(x.ProcessStatus).Contains(keywords)
                || SqlFunc.ToString(x.SuspensionState).Contains(keywords)
                || (x.ProcessContent != null && x.ProcessContent.Contains(keywords))
                || (x.DeploymentId != null && x.DeploymentId.Contains(keywords))
                || SqlFunc.ToString(x.FormId).Contains(keywords)
                || (x.FormCode != null && x.FormCode.Contains(keywords))
                || SqlFunc.ToString(x.SortOrder).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
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

        if (!string.IsNullOrEmpty(queryDto?.ProcessVersion))
        {
            exp = exp.And(x => x.ProcessVersion != null && x.ProcessVersion.Contains(queryDto.ProcessVersion));
        }

        if (queryDto?.IsLatest.HasValue == true)
        {
            exp = exp.And(x => x.IsLatest == queryDto.IsLatest);
        }

        if (queryDto?.ProcessCategory.HasValue == true)
        {
            exp = exp.And(x => x.ProcessCategory == queryDto.ProcessCategory);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessDescription))
        {
            exp = exp.And(x => x.ProcessDescription != null && x.ProcessDescription.Contains(queryDto.ProcessDescription));
        }

        if (queryDto?.ProcessStatus.HasValue == true)
        {
            exp = exp.And(x => x.ProcessStatus == queryDto.ProcessStatus);
        }

        if (queryDto?.SuspensionState.HasValue == true)
        {
            exp = exp.And(x => x.SuspensionState == (int)queryDto.SuspensionState.Value);
        }

        if (!string.IsNullOrEmpty(queryDto?.ProcessContent))
        {
            exp = exp.And(x => x.ProcessContent != null && x.ProcessContent.Contains(queryDto.ProcessContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.DeploymentId))
        {
            exp = exp.And(x => x.DeploymentId != null && x.DeploymentId.Contains(queryDto.DeploymentId));
        }

        if (queryDto?.FormId.HasValue == true)
        {
            exp = exp.And(x => x.FormId == queryDto.FormId);
        }

        if (!string.IsNullOrEmpty(queryDto?.FormCode))
        {
            exp = exp.And(x => x.FormCode != null && x.FormCode.Contains(queryDto.FormCode));
        }

        if (queryDto?.SortOrder.HasValue == true)
        {
            exp = exp.And(x => x.SortOrder == queryDto.SortOrder);
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

    /// <summary>
    /// 校验流程方案已关联表单（审批流程必须绑定表单，与是否关联后台表无关）
    /// </summary>
    /// <param name="formId">表单 ID</param>
    /// <param name="formCode">表单编码</param>
    private static void EnsureSchemeFormLinked(long formId, string? formCode)
    {
        if (formId <= 0)
        {
            throw new TaktBusinessException("流程方案必须关联表单");
        }
        if (string.IsNullOrWhiteSpace(formCode))
        {
            throw new TaktBusinessException("流程方案关联表单编码不能为空");
        }
    }

    /// <summary>
    /// 推送流程定义 SignalR 变更（失败抛异常，阻断主流程）
    /// </summary>
    /// <param name="entity">流程定义实体</param>
    /// <param name="changeType">变更类型</param>
    /// <returns>任务</returns>
    /// <exception cref="TaktBusinessException">推送失败或实体无效时抛出</exception>
    private Task NotifySchemeChangedAsync(TaktFlowScheme entity, string changeType)
    {
        return _workflowSignalRNotifier.NotifySchemeChangedAsync(
            CurrentTenantCode,
            CurrentCompanyCode,
            entity,
            changeType,
            CurrentUserName);
    }
}
