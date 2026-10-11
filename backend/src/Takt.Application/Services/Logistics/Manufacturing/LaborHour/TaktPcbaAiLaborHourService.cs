// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.LaborHour
// 文件名称：TaktPcbaAiLaborHourService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA自插工数统计应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.LaborHour;
using Takt.Domain.Entities.Logistics.Manufacturing.LaborHour;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.LaborHour;

/// <summary>
/// PCBA自插工数统计应用服务
/// </summary>
public class TaktPcbaAiLaborHourService : TaktServiceBase, ITaktPcbaAiLaborHourService
{
    private readonly ITaktCompanyRepository<TaktPcbaAiLaborHour> _pcbaAiLaborHourRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaAiLaborHourRepository">PCBA自插工数统计仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaAiLaborHourService(
        ITaktCompanyRepository<TaktPcbaAiLaborHour> pcbaAiLaborHourRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaAiLaborHourRepository = pcbaAiLaborHourRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取PCBA自插工数统计列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPcbaAiLaborHourDto>> GetPcbaAiLaborHourListAsync(TaktPcbaAiLaborHourQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _pcbaAiLaborHourRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPcbaAiLaborHourDto>.Create(
            data.Adapt<List<TaktPcbaAiLaborHourDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取PCBA自插工数统计
    /// </summary>
    /// <param name="id">PCBA自插工数统计ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaAiLaborHourDto?> GetPcbaAiLaborHourByIdAsync(long id)
    {
        var entity = await _pcbaAiLaborHourRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPcbaAiLaborHourDto>();
    }

    /// <summary>
    /// 获取PCBA自插工数统计选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPcbaAiLaborHourOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _pcbaAiLaborHourRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.TeamCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.TeamCode ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建PCBA自插工数统计
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaAiLaborHourDto> CreatePcbaAiLaborHourAsync(TaktPcbaAiLaborHourCreateDto dto)
    {
        var entity = dto.Adapt<TaktPcbaAiLaborHour>();
        var isUnique_ix_takt_logistics_manufacturing_labor_hour_pcba_ai_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaAiLaborHourRepository,
            x => x.ProdDate == entity.ProdDate
                && x.TeamCode == entity.TeamCode
                && x.ShiftNo == entity.ShiftNo);
        if (!isUnique_ix_takt_logistics_manufacturing_labor_hour_pcba_ai_unique)
        {
            throw new TaktBusinessException("PCBA自插工数统计的ProdDate、TeamCode、ShiftNo已存在");
        }
        entity = await _pcbaAiLaborHourRepository.CreateAsync(entity);
        return await GetPcbaAiLaborHourByIdAsync(entity.Id) ?? entity.Adapt<TaktPcbaAiLaborHourDto>();
    }

    /// <summary>
    /// 更新PCBA自插工数统计
    /// </summary>
    /// <param name="id">PCBA自插工数统计ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaAiLaborHourDto> UpdatePcbaAiLaborHourAsync(long id, TaktPcbaAiLaborHourUpdateDto dto)
    {
        var entity = await _pcbaAiLaborHourRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA自插工数统计不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_labor_hour_pcba_ai_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaAiLaborHourRepository,
            x => x.ProdDate == entity.ProdDate
                && x.TeamCode == entity.TeamCode
                && x.ShiftNo == entity.ShiftNo,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_labor_hour_pcba_ai_unique)
        {
            throw new TaktBusinessException("PCBA自插工数统计的ProdDate、TeamCode、ShiftNo已存在");
        }
        await _pcbaAiLaborHourRepository.UpdateAsync(entity);
        return await GetPcbaAiLaborHourByIdAsync(id) ?? throw new TaktBusinessException("PCBA自插工数统计不存在");
    }

    /// <summary>
    /// 删除PCBA自插工数统计
    /// </summary>
    /// <param name="id">PCBA自插工数统计ID</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaAiLaborHourByIdAsync(long id)
    {
        var deleted = await _pcbaAiLaborHourRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("PCBA自插工数统计不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除PCBA自插工数统计
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaAiLaborHourBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePcbaAiLaborHourByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPcbaAiLaborHourTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPcbaAiLaborHourTemplateDto>(
            sheetName ?? "PCBA自插工数统计导入模板",
            fileName ?? "PCBA自插工数统计导入模板.xlsx");
    }

    /// <summary>
    /// 导入PCBA自插工数统计
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPcbaAiLaborHourAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPcbaAiLaborHourImportDto>(fileStream, sheetName ?? "PCBA自插工数统计导入模板");
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
                var entity = rows[i].Adapt<TaktPcbaAiLaborHour>();
                var importKey = $"{entity.ProdDate}|{entity.TeamCode}|{entity.ShiftNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（ProdDate、TeamCode、ShiftNo）");
                }
                var isUnique_ix_takt_logistics_manufacturing_labor_hour_pcba_ai_unique = await _uniqueValidator.IsUniqueAsync(
                    _pcbaAiLaborHourRepository,
                    x => x.ProdDate == entity.ProdDate
                        && x.TeamCode == entity.TeamCode
                        && x.ShiftNo == entity.ShiftNo);
                if (!isUnique_ix_takt_logistics_manufacturing_labor_hour_pcba_ai_unique)
                {
                    throw new TaktBusinessException("PCBA自插工数统计的ProdDate、TeamCode、ShiftNo已存在");
                }
                await _pcbaAiLaborHourRepository.CreateAsync(entity);
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
    /// 导出PCBA自插工数统计
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPcbaAiLaborHourAsync(TaktPcbaAiLaborHourQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktPcbaAiLaborHourQueryDto());
        var list = await _pcbaAiLaborHourRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaAiLaborHourExportDto>(),
                sheetName ?? "PCBA自插工数统计数据",
                fileName ?? "PCBA自插工数统计导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPcbaAiLaborHourExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "PCBA自插工数统计数据",
            fileName ?? "PCBA自插工数统计导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建PCBA自插工数统计查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPcbaAiLaborHour, bool>> QueryExpression(TaktPcbaAiLaborHourQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPcbaAiLaborHour>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.TeamCode != null && x.TeamCode.Contains(keywords))
                || SqlFunc.ToString(x.ShiftNo).Contains(keywords)
                || SqlFunc.ToString(x.StdCapacity).Contains(keywords)
                || SqlFunc.ToString(x.ProdActualQty).Contains(keywords)
                || SqlFunc.ToString(x.InputMinutes).Contains(keywords)
                || SqlFunc.ToString(x.DowntimeMinutes).Contains(keywords)
                || SqlFunc.ToString(x.ConfirmMinutes).Contains(keywords)
                || SqlFunc.ToString(x.ActualMinutes).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.ProdDate).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.TeamCode))
        {
            exp = exp.And(x => x.TeamCode != null && x.TeamCode.Contains(queryDto.TeamCode));
        }

        if (queryDto?.ShiftNo.HasValue == true)
        {
            exp = exp.And(x => x.ShiftNo == queryDto.ShiftNo);
        }

        if (queryDto?.StdCapacity.HasValue == true)
        {
            exp = exp.And(x => x.StdCapacity == queryDto.StdCapacity);
        }

        if (queryDto?.ProdActualQty.HasValue == true)
        {
            exp = exp.And(x => x.ProdActualQty == queryDto.ProdActualQty);
        }

        if (queryDto?.InputMinutes.HasValue == true)
        {
            exp = exp.And(x => x.InputMinutes == queryDto.InputMinutes);
        }

        if (queryDto?.DowntimeMinutes.HasValue == true)
        {
            exp = exp.And(x => x.DowntimeMinutes == queryDto.DowntimeMinutes);
        }

        if (queryDto?.ConfirmMinutes.HasValue == true)
        {
            exp = exp.And(x => x.ConfirmMinutes == queryDto.ConfirmMinutes);
        }

        if (queryDto?.ActualMinutes.HasValue == true)
        {
            exp = exp.And(x => x.ActualMinutes == queryDto.ActualMinutes);
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

        if (queryDto?.ProdDateStart.HasValue == true)
        {
            exp = exp.And(x => x.ProdDate >= queryDto.ProdDateStart);
        }

        if (queryDto?.ProdDateEnd.HasValue == true)
        {
            exp = exp.And(x => x.ProdDate <= queryDto.ProdDateEnd);
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
