// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow
// 文件名称：TaktFlowAddSignService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：流程加签记录应用服务实现
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
/// 流程加签记录应用服务
/// </summary>
public class TaktFlowAddSignService : TaktServiceBase, ITaktFlowAddSignService
{
    private readonly ITaktCompanyRepository<TaktFlowAddSign> _flowAddSignRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="flowAddSignRepository">流程加签记录仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktFlowAddSignService(
        ITaktCompanyRepository<TaktFlowAddSign> flowAddSignRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _flowAddSignRepository = flowAddSignRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取流程加签记录列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktFlowAddSignDto>> GetFlowAddSignListAsync(TaktFlowAddSignQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktFlowAddSignDto>.Create(
                new List<TaktFlowAddSignDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _flowAddSignRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktFlowAddSignDto>.Create(
            data.Adapt<List<TaktFlowAddSignDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取流程加签记录
    /// </summary>
    /// <param name="id">流程加签记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowAddSignDto?> GetFlowAddSignByIdAsync(long id)
    {
        var entity = await _flowAddSignRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktFlowAddSignDto>();
    }

    /// <summary>
    /// 获取流程加签记录选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetFlowAddSignOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _flowAddSignRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SignUserName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SignUserName ?? string.Empty,
            DictLabel = e.SignUserName ?? string.Empty,
        }).ToList();
    }

    /// <summary>
    /// 创建流程加签记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowAddSignDto> CreateFlowAddSignAsync(TaktFlowAddSignCreateDto dto)
    {
        var entity = dto.Adapt<TaktFlowAddSign>();
        entity = await _flowAddSignRepository.CreateAsync(entity);
        return await GetFlowAddSignByIdAsync(entity.Id) ?? entity.Adapt<TaktFlowAddSignDto>();
    }

    /// <summary>
    /// 更新流程加签记录
    /// </summary>
    /// <param name="id">流程加签记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktFlowAddSignDto> UpdateFlowAddSignAsync(long id, TaktFlowAddSignUpdateDto dto)
    {
        var entity = await _flowAddSignRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("流程加签记录不存在");
        }
        dto.Adapt(entity);
        await _flowAddSignRepository.UpdateAsync(entity);
        return await GetFlowAddSignByIdAsync(id) ?? throw new TaktBusinessException("流程加签记录不存在");
    }

    /// <summary>
    /// 删除流程加签记录
    /// </summary>
    /// <param name="id">流程加签记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowAddSignByIdAsync(long id)
    {
        var deleted = await _flowAddSignRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("流程加签记录不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除流程加签记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteFlowAddSignBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteFlowAddSignByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetFlowAddSignTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktFlowAddSignTemplateDto>(
            sheetName ?? "流程加签记录导入模板",
            fileName ?? "流程加签记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入流程加签记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportFlowAddSignAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktFlowAddSignImportDto>(fileStream, sheetName ?? "流程加签记录导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktFlowAddSign>();
                await _flowAddSignRepository.CreateAsync(entity);
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
    /// 导出流程加签记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportFlowAddSignAsync(TaktFlowAddSignQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktFlowAddSignQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowAddSignExportDto>(),
                sheetName ?? "流程加签记录数据",
                fileName ?? "流程加签记录导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _flowAddSignRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktFlowAddSignExportDto>(),
                sheetName ?? "流程加签记录数据",
                fileName ?? "流程加签记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktFlowAddSignExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "流程加签记录数据",
            fileName ?? "流程加签记录导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建流程加签记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktFlowAddSign, bool>> QueryExpression(TaktFlowAddSignQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktFlowAddSign>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.NodeId != null && x.NodeId.Contains(keywords))
                || (x.SignUserName != null && x.SignUserName.Contains(keywords))
                || (x.SignType != null && x.SignType.Contains(keywords))
                || (x.Reason != null && x.Reason.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.NodeId))
        {
            var nodeId = queryDto.NodeId;
            exp = exp.And(x => x.NodeId != null && x.NodeId.Contains(nodeId));
        }

        if (queryDto?.SignUserId.HasValue == true)
        {
            var signUserId = queryDto.SignUserId.Value;
            exp = exp.And(x => x.SignUserId == signUserId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SignUserName))
        {
            var signUserName = queryDto.SignUserName;
            exp = exp.And(x => x.SignUserName != null && x.SignUserName.Contains(signUserName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SignType))
        {
            var signType = queryDto.SignType;
            exp = exp.And(x => x.SignType != null && x.SignType.Contains(signType));
        }

        if (queryDto?.ReturnToSignNode.HasValue == true)
        {
            var returnToSignNode = queryDto.ReturnToSignNode.Value;
            exp = exp.And(x => x.ReturnToSignNode == returnToSignNode);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Reason))
        {
            var reason = queryDto.Reason;
            exp = exp.And(x => x.Reason != null && x.Reason.Contains(reason));
        }

        if (queryDto?.IsHandled.HasValue == true)
        {
            var isHandled = queryDto.IsHandled.Value;
            exp = exp.And(x => x.IsHandled == isHandled);
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
    private static bool HasAnyListQueryFilter(TaktFlowAddSignQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.NodeId))
        {
            return true;
        }
        if (queryDto.SignUserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SignUserName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SignType))
        {
            return true;
        }
        if (queryDto.ReturnToSignNode.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Reason))
        {
            return true;
        }
        if (queryDto.IsHandled.HasValue)
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
