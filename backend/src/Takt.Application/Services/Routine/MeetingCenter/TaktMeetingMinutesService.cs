// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.MeetingCenter
// 文件名称：TaktMeetingMinutesService.cs
// 创建时间：2026-07-09
// 创建人：Takt365(Cursor AI)
// 功能描述：会后纪要应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.MeetingCenter;
using Takt.Domain.Entities.Routine.MeetingCenter;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.MeetingCenter;

/// <summary>
/// 会后纪要应用服务
/// </summary>
public class TaktMeetingMinutesService : TaktServiceBase, ITaktMeetingMinutesService
{
    private readonly ITaktCompanyRepository<TaktMeetingMinutes> _meetingMinutesRepository;
    private readonly ITaktApprovalRepository<TaktMeeting> _meetingRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="meetingMinutesRepository">会后纪要仓储</param>
    /// <param name="meetingRepository">会议中心仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktMeetingMinutesService(
        ITaktCompanyRepository<TaktMeetingMinutes> meetingMinutesRepository,
        ITaktApprovalRepository<TaktMeeting> meetingRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _meetingMinutesRepository = meetingMinutesRepository;
        _meetingRepository = meetingRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取会后纪要列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktMeetingMinutesDto>> GetMeetingMinutesListAsync(TaktMeetingMinutesQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _meetingMinutesRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktMeetingMinutesDto>.Create(
            data.Adapt<List<TaktMeetingMinutesDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取会后纪要
    /// </summary>
    /// <param name="id">会后纪要ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingMinutesDto?> GetMeetingMinutesByIdAsync(long id)
    {
        var entity = await _meetingMinutesRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktMeetingMinutesDto>();
    }

    /// <summary>
    /// 获取会后纪要选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetMeetingMinutesOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _meetingMinutesRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.MeetingTitle ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = string.IsNullOrWhiteSpace(e.MeetingTitle) ? e.Id.ToString() : e.MeetingTitle,
        }).ToList();
    }

    /// <summary>
    /// 创建会后纪要
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingMinutesDto> CreateMeetingMinutesAsync(TaktMeetingMinutesCreateDto dto)
    {
        var entity = dto.Adapt<TaktMeetingMinutes>();
        entity.IsObsolete = 0;
        await StampMeetingMinutesMeetingAsync(entity, dto);
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _meetingMinutesRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MeetingId == entity.MeetingId,
                x => x.LineNumber);
            var businessCode = entity.MeetingId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _meetingMinutesRepository.CreateAsync(entity);
        return await GetMeetingMinutesByIdAsync(entity.Id) ?? entity.Adapt<TaktMeetingMinutesDto>();
    }

    /// <summary>
    /// 更新会后纪要
    /// </summary>
    /// <param name="id">会后纪要ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingMinutesDto> UpdateMeetingMinutesAsync(long id, TaktMeetingMinutesUpdateDto dto)
    {
        var entity = await _meetingMinutesRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会后纪要不存在");
        }
        dto.Adapt(entity);
        await StampMeetingMinutesMeetingAsync(entity, dto);
        await _meetingMinutesRepository.UpdateAsync(entity);
        return await GetMeetingMinutesByIdAsync(id) ?? throw new TaktBusinessException("会后纪要不存在");
    }

    /// <summary>
    /// 删除会后纪要
    /// </summary>
    /// <param name="id">会后纪要ID</param>
    /// <returns>任务</returns>
    public async Task DeleteMeetingMinutesByIdAsync(long id)
    {
        var entity = await _meetingMinutesRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("会后纪要不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("会后纪要不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("会后纪要已作废");
        }
        entity.IsObsolete = 1;
        await _meetingMinutesRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除会后纪要
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteMeetingMinutesBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteMeetingMinutesByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新会后纪要作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktMeetingMinutesDto> UpdateMeetingMinutesObsoleteAsync(TaktMeetingMinutesObsoleteDto dto)
    {
        var entity = await _meetingMinutesRepository.GetByIdAsync(dto.MeetingMinutesId);
        if (entity == null)
        {
            throw new TaktBusinessException("会后纪要不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("会后纪要不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _meetingMinutesRepository.UpdateAsync(entity);
        return await GetMeetingMinutesByIdAsync(dto.MeetingMinutesId) ?? throw new TaktBusinessException("会后纪要不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetMeetingMinutesTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktMeetingMinutesTemplateDto>(
            sheetName ?? "会后纪要导入模板",
            fileName ?? "会后纪要导入模板.xlsx");
    }

    /// <summary>
    /// 导入会后纪要
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportMeetingMinutesAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktMeetingMinutesImportDto>(fileStream, sheetName ?? "会后纪要导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktMeetingMinutes>();
                var importDto = rows[i].Adapt<TaktMeetingMinutesCreateDto>();
                await StampMeetingMinutesMeetingAsync(entity, importDto);
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _meetingMinutesRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.MeetingId == entity.MeetingId,
                        x => x.LineNumber);
                    var businessCode = entity.MeetingId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _meetingMinutesRepository.CreateAsync(entity);
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
    /// 导出会后纪要
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportMeetingMinutesAsync(TaktMeetingMinutesQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktMeetingMinutesQueryDto());
        var list = await _meetingMinutesRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktMeetingMinutesExportDto>(),
                sheetName ?? "会后纪要数据",
                fileName ?? "会后纪要导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktMeetingMinutesExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "会后纪要数据",
            fileName ?? "会后纪要导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步会后纪要主表外键（ManyToOne → 会议中心）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampMeetingMinutesMeetingAsync(TaktMeetingMinutes entity, TaktMeetingMinutesCreateDto dto)
    {
        if (dto.MeetingId <= 0)
        {
            return;
        }
        var master = await _meetingRepository.GetByIdAsync(dto.MeetingId);
        if (master == null)
        {
            throw new TaktBusinessException("会议中心不存在");
        }
        entity.MeetingId = master.Id;
        entity.MeetingTitle = master.MeetingTitle;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建会后纪要查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktMeetingMinutes, bool>> QueryExpression(TaktMeetingMinutesQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktMeetingMinutes>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.MeetingId).Contains(keywords)
                || (x.MeetingTitle != null && x.MeetingTitle.Contains(keywords))
                || SqlFunc.ToString(x.LineNumber).Contains(keywords)
                || (x.MeetingMinutes != null && x.MeetingMinutes.Contains(keywords))
                || (x.MeetingSummary != null && x.MeetingSummary.Contains(keywords))
                || SqlFunc.ToString(x.RecorderId).Contains(keywords)
                || (x.RecorderName != null && x.RecorderName.Contains(keywords))
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.AccessUrl != null && x.AccessUrl.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.MeetingId.HasValue == true)
        {
            exp = exp.And(x => x.MeetingId == queryDto.MeetingId);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            exp = exp.And(x => x.LineNumber == queryDto.LineNumber);
        }

        if (!string.IsNullOrEmpty(queryDto?.MeetingMinutes))
        {
            exp = exp.And(x => x.MeetingMinutes != null && x.MeetingMinutes.Contains(queryDto.MeetingMinutes));
        }

        if (!string.IsNullOrEmpty(queryDto?.MeetingSummary))
        {
            exp = exp.And(x => x.MeetingSummary != null && x.MeetingSummary.Contains(queryDto.MeetingSummary));
        }

        if (!string.IsNullOrEmpty(queryDto?.MeetingTitle))
        {
            exp = exp.And(x => x.MeetingTitle != null && x.MeetingTitle.Contains(queryDto.MeetingTitle));
        }

        if (queryDto?.RecorderId.HasValue == true)
        {
            exp = exp.And(x => x.RecorderId == queryDto.RecorderId);
        }

        if (!string.IsNullOrEmpty(queryDto?.RecorderName))
        {
            exp = exp.And(x => x.RecorderName != null && x.RecorderName.Contains(queryDto.RecorderName));
        }

        if (!string.IsNullOrEmpty(queryDto?.FileName))
        {
            exp = exp.And(x => x.FileName != null && x.FileName.Contains(queryDto.FileName));
        }

        if (!string.IsNullOrEmpty(queryDto?.AccessUrl))
        {
            exp = exp.And(x => x.AccessUrl != null && x.AccessUrl.Contains(queryDto.AccessUrl));
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
}
