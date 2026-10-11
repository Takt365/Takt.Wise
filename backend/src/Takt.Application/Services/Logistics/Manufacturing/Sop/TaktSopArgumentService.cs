// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopArgumentService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP作业参数应用服务实现
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
/// SOP作业参数应用服务
/// </summary>
public class TaktSopArgumentService : TaktServiceBase, ITaktSopArgumentService
{
    private readonly ITaktCompanyRepository<TaktSopArgument> _sopArgumentRepository;
    private readonly ITaktCompanyRepository<TaktSopExec> _sopExecRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopArgumentRepository">SOP作业参数仓储</param>
    /// <param name="sopExecRepository">SOP工位执行仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopArgumentService(
        ITaktCompanyRepository<TaktSopArgument> sopArgumentRepository,
        ITaktCompanyRepository<TaktSopExec> sopExecRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopArgumentRepository = sopArgumentRepository;
        _sopExecRepository = sopExecRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP作业参数列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopArgumentDto>> GetSopArgumentListAsync(TaktSopArgumentQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSopArgumentDto>.Create(
                new List<TaktSopArgumentDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopArgumentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopArgumentDto>.Create(
            data.Adapt<List<TaktSopArgumentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP作业参数
    /// </summary>
    /// <param name="id">SOP作业参数ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopArgumentDto?> GetSopArgumentByIdAsync(long id)
    {
        var entity = await _sopArgumentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSopArgumentDto>();
    }

    /// <summary>
    /// 获取SOP作业参数选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopArgumentOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sopArgumentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ParamCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ParamCode,
            DictLabel = e.ParamCode,
        }).ToList();
    }

    /// <summary>
    /// 创建SOP作业参数
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopArgumentDto> CreateSopArgumentAsync(TaktSopArgumentCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopArgument>();
        await StampSopArgumentSopExecAsync(entity, dto);
        entity = await _sopArgumentRepository.CreateAsync(entity);
        return await GetSopArgumentByIdAsync(entity.Id) ?? entity.Adapt<TaktSopArgumentDto>();
    }

    /// <summary>
    /// 更新SOP作业参数
    /// </summary>
    /// <param name="id">SOP作业参数ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopArgumentDto> UpdateSopArgumentAsync(long id, TaktSopArgumentUpdateDto dto)
    {
        var entity = await _sopArgumentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP作业参数不存在");
        }
        dto.Adapt(entity);
        await StampSopArgumentSopExecAsync(entity, dto);
        await _sopArgumentRepository.UpdateAsync(entity);
        return await GetSopArgumentByIdAsync(id) ?? throw new TaktBusinessException("SOP作业参数不存在");
    }

    /// <summary>
    /// 删除SOP作业参数
    /// </summary>
    /// <param name="id">SOP作业参数ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopArgumentByIdAsync(long id)
    {
        var deleted = await _sopArgumentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP作业参数不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP作业参数
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopArgumentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopArgumentByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopArgumentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopArgumentTemplateDto>(
            sheetName ?? "SOP作业参数导入模板",
            fileName ?? "SOP作业参数导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP作业参数
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopArgumentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopArgumentImportDto>(fileStream, sheetName ?? "SOP作业参数导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopArgument>();
                var importDto = rows[i].Adapt<TaktSopArgumentCreateDto>();
                await StampSopArgumentSopExecAsync(entity, importDto);
                await _sopArgumentRepository.CreateAsync(entity);
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
    /// 导出SOP作业参数
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopArgumentAsync(TaktSopArgumentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSopArgumentQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopArgumentExportDto>(),
                sheetName ?? "SOP作业参数数据",
                fileName ?? "SOP作业参数导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _sopArgumentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopArgumentExportDto>(),
                sheetName ?? "SOP作业参数数据",
                fileName ?? "SOP作业参数导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopArgumentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP作业参数数据",
            fileName ?? "SOP作业参数导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步SOP作业参数主表外键（ManyToOne → SOP工位执行）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSopArgumentSopExecAsync(TaktSopArgument entity, TaktSopArgumentCreateDto dto)
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
    /// 构建SOP作业参数查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopArgument, bool>> QueryExpression(TaktSopArgumentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopArgument>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ParamCode != null && x.ParamCode.Contains(keywords))
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

        if (queryDto?.ExecStepId.HasValue == true)
        {
            var execStepId = queryDto.ExecStepId.Value;
            exp = exp.And(x => x.ExecStepId == execStepId);
        }

        if (queryDto?.RoutingItemParameterId.HasValue == true)
        {
            var routingItemParameterId = queryDto.RoutingItemParameterId.Value;
            exp = exp.And(x => x.RoutingItemParameterId == routingItemParameterId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ParamCode))
        {
            var paramCode = queryDto.ParamCode;
            exp = exp.And(x => x.ParamCode != null && x.ParamCode.Contains(paramCode));
        }

        if (queryDto?.ActualValue.HasValue == true)
        {
            var actualValue = queryDto.ActualValue.Value;
            exp = exp.And(x => x.ActualValue == actualValue);
        }

        if (queryDto?.IsOutOfRange.HasValue == true)
        {
            var isOutOfRange = queryDto.IsOutOfRange.Value;
            exp = exp.And(x => x.IsOutOfRange == isOutOfRange);
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

        if (queryDto?.RecordedAtStart.HasValue == true)
        {
            var recordedAtStart = queryDto.RecordedAtStart.Value;
            exp = exp.And(x => x.RecordedAt >= recordedAtStart);
        }

        if (queryDto?.RecordedAtEnd.HasValue == true)
        {
            var recordedAtEnd = queryDto.RecordedAtEnd.Value;
            exp = exp.And(x => x.RecordedAt <= recordedAtEnd);
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
    private static bool HasAnyListQueryFilter(TaktSopArgumentQueryDto? queryDto)
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
        if (queryDto.ExecStepId.HasValue)
        {
            return true;
        }
        if (queryDto.RoutingItemParameterId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ParamCode))
        {
            return true;
        }
        if (queryDto.ActualValue.HasValue)
        {
            return true;
        }
        if (queryDto.IsOutOfRange.HasValue)
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
        if (queryDto.RecordedAtStart.HasValue || queryDto.RecordedAtEnd.HasValue)
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
