// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopExecStepService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP工步执行明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Sop;
using Takt.Domain.Entities.Logistics.Manufacturing.Sop;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Sop;

/// <summary>
/// SOP工步执行明细应用服务
/// </summary>
public class TaktSopExecStepService : TaktServiceBase, ITaktSopExecStepService
{
    private readonly ITaktCompanyRepository<TaktSopExecStep> _sopExecStepRepository;
    private readonly ITaktCompanyRepository<TaktSopExec> _sopExecRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopExecStepRepository">SOP工步执行明细仓储</param>
    /// <param name="sopExecRepository">SOP工位执行仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopExecStepService(
        ITaktCompanyRepository<TaktSopExecStep> sopExecStepRepository,
        ITaktCompanyRepository<TaktSopExec> sopExecRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopExecStepRepository = sopExecStepRepository;
        _sopExecRepository = sopExecRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP工步执行明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopExecStepDto>> GetSopExecStepListAsync(TaktSopExecStepQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSopExecStepDto>.Create(
                new List<TaktSopExecStepDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopExecStepRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopExecStepDto>.Create(
            data.Adapt<List<TaktSopExecStepDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP工步执行明细
    /// </summary>
    /// <param name="id">SOP工步执行明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecStepDto?> GetSopExecStepByIdAsync(long id)
    {
        var entity = await _sopExecStepRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSopExecStepDto>();
    }

    /// <summary>
    /// 获取SOP工步执行明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopExecStepOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sopExecStepRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.StepNo.ToString(),
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.StepNo.ToString(),
            DictLabel = e.StepNo.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP工步执行明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecStepDto> CreateSopExecStepAsync(TaktSopExecStepCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopExecStep>();
        await StampSopExecStepSopExecAsync(entity, dto);
        entity = await _sopExecStepRepository.CreateAsync(entity);
        return await GetSopExecStepByIdAsync(entity.Id) ?? entity.Adapt<TaktSopExecStepDto>();
    }

    /// <summary>
    /// 更新SOP工步执行明细
    /// </summary>
    /// <param name="id">SOP工步执行明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopExecStepDto> UpdateSopExecStepAsync(long id, TaktSopExecStepUpdateDto dto)
    {
        var entity = await _sopExecStepRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP工步执行明细不存在");
        }
        dto.Adapt(entity);
        await StampSopExecStepSopExecAsync(entity, dto);
        await _sopExecStepRepository.UpdateAsync(entity);
        return await GetSopExecStepByIdAsync(id) ?? throw new TaktBusinessException("SOP工步执行明细不存在");
    }

    /// <summary>
    /// 删除SOP工步执行明细
    /// </summary>
    /// <param name="id">SOP工步执行明细ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopExecStepByIdAsync(long id)
    {
        var deleted = await _sopExecStepRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP工步执行明细不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP工步执行明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopExecStepBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopExecStepByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopExecStepTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopExecStepTemplateDto>(
            sheetName ?? "SOP工步执行明细导入模板",
            fileName ?? "SOP工步执行明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP工步执行明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopExecStepAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopExecStepImportDto>(fileStream, sheetName ?? "SOP工步执行明细导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopExecStep>();
                var importDto = rows[i].Adapt<TaktSopExecStepCreateDto>();
                await StampSopExecStepSopExecAsync(entity, importDto);
                await _sopExecStepRepository.CreateAsync(entity);
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
    /// 导出SOP工步执行明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopExecStepAsync(TaktSopExecStepQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSopExecStepQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopExecStepExportDto>(),
                sheetName ?? "SOP工步执行明细数据",
                fileName ?? "SOP工步执行明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _sopExecStepRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopExecStepExportDto>(),
                sheetName ?? "SOP工步执行明细数据",
                fileName ?? "SOP工步执行明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopExecStepExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP工步执行明细数据",
            fileName ?? "SOP工步执行明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步SOP工步执行明细主表外键（ManyToOne → SOP工位执行）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSopExecStepSopExecAsync(TaktSopExecStep entity, TaktSopExecStepCreateDto dto)
    {
        if (dto.ExecId <= 0)
        {
            return;
        }
        var master = await _sopExecRepository.GetByIdAsync(dto.ExecId);
        if (master == null)
        {
            throw new TaktBusinessException("SOP工位执行不存在");
        }
        entity.ExecId = master.Id;
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
    /// 构建SOP工步执行明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopExecStep, bool>> QueryExpression(TaktSopExecStepQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopExecStep>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
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

        if (queryDto?.ExecId.HasValue == true)
        {
            var execId = queryDto.ExecId.Value;
            exp = exp.And(x => x.ExecId == execId);
        }

        if (queryDto?.StepId.HasValue == true)
        {
            var stepId = queryDto.StepId.Value;
            exp = exp.And(x => x.StepId == stepId);
        }

        if (queryDto?.StepNo.HasValue == true)
        {
            var stepNo = queryDto.StepNo.Value;
            exp = exp.And(x => x.StepNo == stepNo);
        }

        if (queryDto?.StepResult.HasValue == true)
        {
            var stepResult = queryDto.StepResult.Value;
            exp = exp.And(x => x.StepResult == stepResult);
        }

        if (queryDto?.ConfirmedBy.HasValue == true)
        {
            var confirmedBy = queryDto.ConfirmedBy.Value;
            exp = exp.And(x => x.ConfirmedBy == confirmedBy);
        }

        if (queryDto?.BlockNextStep.HasValue == true)
        {
            var blockNextStep = queryDto.BlockNextStep.Value;
            exp = exp.And(x => x.BlockNextStep == blockNextStep);
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

        if (queryDto?.StartedAtStart.HasValue == true)
        {
            var startedAtStart = queryDto.StartedAtStart.Value;
            exp = exp.And(x => x.StartedAt >= startedAtStart);
        }

        if (queryDto?.StartedAtEnd.HasValue == true)
        {
            var startedAtEnd = queryDto.StartedAtEnd.Value;
            exp = exp.And(x => x.StartedAt <= startedAtEnd);
        }

        if (queryDto?.EndedAtStart.HasValue == true)
        {
            var endedAtStart = queryDto.EndedAtStart.Value;
            exp = exp.And(x => x.EndedAt >= endedAtStart);
        }

        if (queryDto?.EndedAtEnd.HasValue == true)
        {
            var endedAtEnd = queryDto.EndedAtEnd.Value;
            exp = exp.And(x => x.EndedAt <= endedAtEnd);
        }

        if (queryDto?.ConfirmedAtStart.HasValue == true)
        {
            var confirmedAtStart = queryDto.ConfirmedAtStart.Value;
            exp = exp.And(x => x.ConfirmedAt >= confirmedAtStart);
        }

        if (queryDto?.ConfirmedAtEnd.HasValue == true)
        {
            var confirmedAtEnd = queryDto.ConfirmedAtEnd.Value;
            exp = exp.And(x => x.ConfirmedAt <= confirmedAtEnd);
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
    private static bool HasAnyListQueryFilter(TaktSopExecStepQueryDto? queryDto)
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
        if (queryDto.ExecId.HasValue)
        {
            return true;
        }
        if (queryDto.StepId.HasValue)
        {
            return true;
        }
        if (queryDto.StepNo.HasValue)
        {
            return true;
        }
        if (queryDto.StepResult.HasValue)
        {
            return true;
        }
        if (queryDto.ConfirmedBy.HasValue)
        {
            return true;
        }
        if (queryDto.BlockNextStep.HasValue)
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
        if (queryDto.StartedAtStart.HasValue || queryDto.StartedAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.EndedAtStart.HasValue || queryDto.EndedAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ConfirmedAtStart.HasValue || queryDto.ConfirmedAtEnd.HasValue)
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
