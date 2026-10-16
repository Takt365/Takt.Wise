// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow
// 文件名称：TaktFlowVariableService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：流程变量应用服务实现
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
/// 流程变量应用服务
/// </summary>
public class TaktFlowVariableService : TaktServiceBase, ITaktFlowVariableService
{
    private readonly ITaktCompanyRepository<TaktFlowVariable> _flowVariableRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowVariableRepository">流程变量仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFlowVariableService(
        ITaktCompanyRepository<TaktFlowVariable> flowVariableRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowVariableRepository = flowVariableRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取流程变量列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowVariableDto>> GetFlowVariableListAsync(TaktFlowVariableQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktFlowVariableDto>.Create(
                new List<TaktFlowVariableDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _flowVariableRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFlowVariableDto>.Create(
            data.Adapt<List<TaktFlowVariableDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取流程变量
    /// </summary>
    /// <param name="id">流程变量ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowVariableDto?> GetFlowVariableByIdAsync(long id)
    {
        var entity = await _flowVariableRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktFlowVariableDto>();
    }

    /// <summary>
    /// 获取流程变量选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFlowVariableOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _flowVariableRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.VariableName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.VariableName,
            DictLabel = e.VariableName,
        }).ToList();
    }

    /// <summary>
    /// 创建流程变量
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowVariableDto> CreateFlowVariableAsync(TaktFlowVariableCreateDto dto)
    {
        var entity = dto.Adapt<TaktFlowVariable>();
        var isUnique_ix_flow_variable_instance_name_unique = await _uniqueValidator.IsUniqueAsync(
            _flowVariableRepository,
            x => x.InstanceId == entity.InstanceId
                && x.VariableName == entity.VariableName);
        if (!isUnique_ix_flow_variable_instance_name_unique)
        {
            throw new TaktBusinessException("流程变量的InstanceId、VariableName已存在");
        }
        entity = await _flowVariableRepository.CreateAsync(entity);
        return await GetFlowVariableByIdAsync(entity.Id) ?? entity.Adapt<TaktFlowVariableDto>();
    }

    /// <summary>
    /// 更新流程变量
    /// </summary>
    /// <param name="id">流程变量ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowVariableDto> UpdateFlowVariableAsync(long id, TaktFlowVariableUpdateDto dto)
    {
        var entity = await _flowVariableRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("流程变量不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_flow_variable_instance_name_unique = await _uniqueValidator.IsUniqueAsync(
            _flowVariableRepository,
            x => x.InstanceId == entity.InstanceId
                && x.VariableName == entity.VariableName,
            id);
        if (!isUnique_ix_flow_variable_instance_name_unique)
        {
            throw new TaktBusinessException("流程变量的InstanceId、VariableName已存在");
        }
        await _flowVariableRepository.UpdateAsync(entity);
        return await GetFlowVariableByIdAsync(id) ?? throw new TaktBusinessException("流程变量不存在");
    }

    /// <summary>
    /// 删除流程变量
    /// </summary>
    /// <param name="id">流程变量ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowVariableByIdAsync(long id)
    {
        var deleted = await _flowVariableRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("流程变量不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除流程变量
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowVariableBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFlowVariableByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFlowVariableTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFlowVariableTemplateDto>(
            sheetName ?? "流程变量导入模板",
            fileName ?? "流程变量导入模板.xlsx");
    }

    /// <summary>
    /// 导入流程变量
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFlowVariableAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFlowVariableImportDto>(fileStream, sheetName ?? "流程变量导入模板");
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
                var entity = rows[i].Adapt<TaktFlowVariable>();
                var importKey = $"{entity.InstanceId}|{entity.VariableName}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（InstanceId、VariableName）");
                }
                var isUnique_ix_flow_variable_instance_name_unique = await _uniqueValidator.IsUniqueAsync(
                    _flowVariableRepository,
                    x => x.InstanceId == entity.InstanceId
                        && x.VariableName == entity.VariableName);
                if (!isUnique_ix_flow_variable_instance_name_unique)
                {
                    throw new TaktBusinessException("流程变量的InstanceId、VariableName已存在");
                }
                await _flowVariableRepository.CreateAsync(entity);
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
    /// 导出流程变量
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFlowVariableAsync(TaktFlowVariableQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktFlowVariableQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowVariableExportDto>(),
                sheetName ?? "流程变量数据",
                fileName ?? "流程变量导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _flowVariableRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowVariableExportDto>(),
                sheetName ?? "流程变量数据",
                fileName ?? "流程变量导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFlowVariableExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "流程变量数据",
            fileName ?? "流程变量导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建流程变量查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFlowVariable, bool>> QueryExpression(TaktFlowVariableQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFlowVariable>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.VariableName != null && x.VariableName.Contains(keywords))
                || (x.TextValue != null && x.TextValue.Contains(keywords))
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

        if (queryDto?.TaskId.HasValue == true)
        {
            var taskId = queryDto.TaskId.Value;
            exp = exp.And(x => x.TaskId == taskId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.VariableName))
        {
            var variableName = queryDto.VariableName;
            exp = exp.And(x => x.VariableName != null && x.VariableName.Contains(variableName));
        }

        if (queryDto?.VariableType.HasValue == true)
        {
            var variableType = queryDto.VariableType.Value;
            exp = exp.And(x => x.VariableType == (int)variableType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TextValue))
        {
            var textValue = queryDto.TextValue;
            exp = exp.And(x => x.TextValue != null && x.TextValue.Contains(textValue));
        }

        if (queryDto?.LongValue.HasValue == true)
        {
            var longValue = queryDto.LongValue.Value;
            exp = exp.And(x => x.LongValue == longValue);
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
    private static bool HasAnyListQueryFilter(TaktFlowVariableQueryDto? queryDto)
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
        if (queryDto.TaskId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VariableName))
        {
            return true;
        }
        if (queryDto.VariableType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TextValue))
        {
            return true;
        }
        if (queryDto.LongValue.HasValue)
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
