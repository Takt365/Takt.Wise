// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopCallService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP安灯呼叫应用服务实现
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
/// SOP安灯呼叫应用服务
/// </summary>
public class TaktSopCallService : TaktServiceBase, ITaktSopCallService
{
    private readonly ITaktCompanyRepository<TaktSopCall> _sopCallRepository;
    private readonly ITaktCompanyRepository<TaktSopWorkstation> _sopWorkstationRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopCallRepository">SOP安灯呼叫仓储</param>
    /// <param name="sopWorkstationRepository">SOP工位主数据仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopCallService(
        ITaktCompanyRepository<TaktSopCall> sopCallRepository,
        ITaktCompanyRepository<TaktSopWorkstation> sopWorkstationRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopCallRepository = sopCallRepository;
        _sopWorkstationRepository = sopWorkstationRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP安灯呼叫列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopCallDto>> GetSopCallListAsync(TaktSopCallQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopCallRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopCallDto>.Create(
            data.Adapt<List<TaktSopCallDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP安灯呼叫
    /// </summary>
    /// <param name="id">SOP安灯呼叫ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopCallDto?> GetSopCallByIdAsync(long id)
    {
        var entity = await _sopCallRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktSopCallDto>();
    }

    /// <summary>
    /// 获取SOP安灯呼叫选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopCallOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sopCallRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CallStatus == 1,
            x => x.Id,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建SOP安灯呼叫
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopCallDto> CreateSopCallAsync(TaktSopCallCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopCall>();
        await StampSopCallSopWorkstationAsync(entity, dto);
        entity = await _sopCallRepository.CreateAsync(entity);
        return await GetSopCallByIdAsync(entity.Id) ?? entity.Adapt<TaktSopCallDto>();
    }

    /// <summary>
    /// 更新SOP安灯呼叫
    /// </summary>
    /// <param name="id">SOP安灯呼叫ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopCallDto> UpdateSopCallAsync(long id, TaktSopCallUpdateDto dto)
    {
        var entity = await _sopCallRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP安灯呼叫不存在");
        }
        dto.Adapt(entity);
        await StampSopCallSopWorkstationAsync(entity, dto);
        await _sopCallRepository.UpdateAsync(entity);
        return await GetSopCallByIdAsync(id) ?? throw new TaktBusinessException("SOP安灯呼叫不存在");
    }

    /// <summary>
    /// 删除SOP安灯呼叫
    /// </summary>
    /// <param name="id">SOP安灯呼叫ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopCallByIdAsync(long id)
    {
        var deleted = await _sopCallRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP安灯呼叫不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP安灯呼叫
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopCallBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopCallByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新SOP安灯呼叫状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopCallDto> UpdateSopCallStatusAsync(TaktSopCallStatusDto dto)
    {
        var entity = await _sopCallRepository.GetByIdAsync(dto.SopCallId);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP安灯呼叫不存在");
        }
        entity.CallStatus = dto.CallStatus;
        await _sopCallRepository.UpdateAsync(entity);
        return await GetSopCallByIdAsync(dto.SopCallId) ?? throw new TaktBusinessException("SOP安灯呼叫不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopCallTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopCallTemplateDto>(
            sheetName ?? "SOP安灯呼叫导入模板",
            fileName ?? "SOP安灯呼叫导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP安灯呼叫
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopCallAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopCallImportDto>(fileStream, sheetName ?? "SOP安灯呼叫导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktSopCall>();
                var importDto = rows[i].Adapt<TaktSopCallCreateDto>();
                await StampSopCallSopWorkstationAsync(entity, importDto);
                await _sopCallRepository.CreateAsync(entity);
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
    /// 导出SOP安灯呼叫
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopCallAsync(TaktSopCallQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktSopCallQueryDto());
        var list = await _sopCallRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopCallExportDto>(),
                sheetName ?? "SOP安灯呼叫数据",
                fileName ?? "SOP安灯呼叫导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopCallExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP安灯呼叫数据",
            fileName ?? "SOP安灯呼叫导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步SOP安灯呼叫主表外键（ManyToOne → SOP工位主数据）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampSopCallSopWorkstationAsync(TaktSopCall entity, TaktSopCallCreateDto dto)
    {
        if (dto.WorkstationId <= 0)
        {
            return;
        }
        var master = await _sopWorkstationRepository.GetByIdAsync(dto.WorkstationId);
        if (master == null)
        {
            throw new TaktBusinessException("SOP工位主数据不存在");
        }
        entity.WorkstationId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP安灯呼叫查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopCall, bool>> QueryExpression(TaktSopCallQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopCall>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || SqlFunc.ToString(x.WorkstationId).Contains(keywords)
                || SqlFunc.ToString(x.ExecId).Contains(keywords)
                || SqlFunc.ToString(x.CallType).Contains(keywords)
                || SqlFunc.ToString(x.CallerId).Contains(keywords)
                || SqlFunc.ToString(x.RespondedBy).Contains(keywords)
                || SqlFunc.ToString(x.ResponseSeconds).Contains(keywords)
                || SqlFunc.ToString(x.CallStatus).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CalledAt).Contains(keywords)
                || SqlFunc.ToString(x.RespondedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (!string.IsNullOrEmpty(queryDto?.PlantCode))
        {
            exp = exp.And(x => x.PlantCode != null && x.PlantCode.Contains(queryDto.PlantCode));
        }

        if (queryDto?.WorkstationId.HasValue == true)
        {
            exp = exp.And(x => x.WorkstationId == queryDto.WorkstationId);
        }

        if (queryDto?.ExecId.HasValue == true)
        {
            exp = exp.And(x => x.ExecId == queryDto.ExecId);
        }

        if (queryDto?.CallType.HasValue == true)
        {
            exp = exp.And(x => x.CallType == queryDto.CallType);
        }

        if (queryDto?.CallerId.HasValue == true)
        {
            exp = exp.And(x => x.CallerId == queryDto.CallerId);
        }

        if (queryDto?.RespondedBy.HasValue == true)
        {
            exp = exp.And(x => x.RespondedBy == queryDto.RespondedBy);
        }

        if (queryDto?.ResponseSeconds.HasValue == true)
        {
            exp = exp.And(x => x.ResponseSeconds == queryDto.ResponseSeconds);
        }

        if (queryDto?.CallStatus.HasValue == true)
        {
            exp = exp.And(x => x.CallStatus == queryDto.CallStatus);
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

        if (queryDto?.CalledAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CalledAt >= queryDto.CalledAtStart);
        }

        if (queryDto?.CalledAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CalledAt <= queryDto.CalledAtEnd);
        }

        if (queryDto?.RespondedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.RespondedAt >= queryDto.RespondedAtStart);
        }

        if (queryDto?.RespondedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.RespondedAt <= queryDto.RespondedAtEnd);
        }

        if (queryDto?.CreatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt >= queryDto.CreatedAtStart);
        }

        if (queryDto?.CreatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.CreatedAt <= queryDto.CreatedAtEnd);
        }

        return exp.ToExpression();
    }
}
