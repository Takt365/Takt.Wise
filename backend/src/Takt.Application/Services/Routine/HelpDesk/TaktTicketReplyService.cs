// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：TaktTicketReplyService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工单回复应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.HelpDesk;
using Takt.Domain.Entities.Routine.HelpDesk;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.HelpDesk;

/// <summary>
/// 工单回复应用服务
/// </summary>
public class TaktTicketReplyService : TaktServiceBase, ITaktTicketReplyService
{
    private readonly ITaktCompanyRepository<TaktTicketReply> _ticketReplyRepository;
    private readonly ITaktCompanyRepository<TaktTicket> _ticketRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketReplyRepository">工单回复仓储</param>
    /// <param name="ticketRepository">工单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTicketReplyService(
        ITaktCompanyRepository<TaktTicketReply> ticketReplyRepository,
        ITaktCompanyRepository<TaktTicket> ticketRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ticketReplyRepository = ticketReplyRepository;
        _ticketRepository = ticketRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工单回复列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTicketReplyDto>> GetTicketReplyListAsync(TaktTicketReplyQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ticketReplyRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTicketReplyDto>.Create(
            data.Adapt<List<TaktTicketReplyDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工单回复
    /// </summary>
    /// <param name="id">工单回复ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketReplyDto?> GetTicketReplyByIdAsync(long id)
    {
        var entity = await _ticketReplyRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTicketReplyDto>();
    }

    /// <summary>
    /// 获取工单回复选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTicketReplyOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _ticketReplyRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.AuthorName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.AuthorName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工单回复
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketReplyDto> CreateTicketReplyAsync(TaktTicketReplyCreateDto dto)
    {
        var entity = dto.Adapt<TaktTicketReply>();
        await StampTicketReplyTicketAsync(entity, dto);
        entity = await _ticketReplyRepository.CreateAsync(entity);
        return await GetTicketReplyByIdAsync(entity.Id) ?? entity.Adapt<TaktTicketReplyDto>();
    }

    /// <summary>
    /// 更新工单回复
    /// </summary>
    /// <param name="id">工单回复ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketReplyDto> UpdateTicketReplyAsync(long id, TaktTicketReplyUpdateDto dto)
    {
        var entity = await _ticketReplyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工单回复不存在");
        }
        dto.Adapt(entity);
        await StampTicketReplyTicketAsync(entity, dto);
        await _ticketReplyRepository.UpdateAsync(entity);
        return await GetTicketReplyByIdAsync(id) ?? throw new TaktBusinessException("工单回复不存在");
    }

    /// <summary>
    /// 删除工单回复
    /// </summary>
    /// <param name="id">工单回复ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketReplyByIdAsync(long id)
    {
        var deleted = await _ticketReplyRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工单回复不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工单回复
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketReplyBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTicketReplyByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTicketReplyTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTicketReplyTemplateDto>(
            sheetName ?? "工单回复导入模板",
            fileName ?? "工单回复导入模板.xlsx");
    }

    /// <summary>
    /// 导入工单回复
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTicketReplyAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTicketReplyImportDto>(fileStream, sheetName ?? "工单回复导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktTicketReply>();
                var importDto = rows[i].Adapt<TaktTicketReplyCreateDto>();
                await StampTicketReplyTicketAsync(entity, importDto);
                await _ticketReplyRepository.CreateAsync(entity);
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
    /// 导出工单回复
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTicketReplyAsync(TaktTicketReplyQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTicketReplyQueryDto());
        var list = await _ticketReplyRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTicketReplyExportDto>(),
                sheetName ?? "工单回复数据",
                fileName ?? "工单回复导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTicketReplyExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工单回复数据",
            fileName ?? "工单回复导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步工单回复主表外键（ManyToOne → 工单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampTicketReplyTicketAsync(TaktTicketReply entity, TaktTicketReplyCreateDto dto)
    {
        if (dto.TicketId <= 0)
        {
            return;
        }
        var master = await _ticketRepository.GetByIdAsync(dto.TicketId);
        if (master == null)
        {
            throw new TaktBusinessException("工单不存在");
        }
        entity.TicketId = master.Id;
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工单回复查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTicketReply, bool>> QueryExpression(TaktTicketReplyQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTicketReply>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.TicketId).Contains(keywords)
                || SqlFunc.ToString(x.AuthorType).Contains(keywords)
                || SqlFunc.ToString(x.AuthorId).Contains(keywords)
                || (x.AuthorName != null && x.AuthorName.Contains(keywords))
                || (x.TicketReplyContent != null && x.TicketReplyContent.Contains(keywords))
                || (x.Attachments != null && x.Attachments.Contains(keywords))
                || SqlFunc.ToString(x.IsInternal).Contains(keywords)
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.TicketId.HasValue == true)
        {
            exp = exp.And(x => x.TicketId == queryDto.TicketId);
        }

        if (queryDto?.AuthorType.HasValue == true)
        {
            exp = exp.And(x => x.AuthorType == queryDto.AuthorType);
        }

        if (queryDto?.AuthorId.HasValue == true)
        {
            exp = exp.And(x => x.AuthorId == queryDto.AuthorId);
        }

        if (!string.IsNullOrEmpty(queryDto?.AuthorName))
        {
            exp = exp.And(x => x.AuthorName != null && x.AuthorName.Contains(queryDto.AuthorName));
        }

        if (!string.IsNullOrEmpty(queryDto?.TicketReplyContent))
        {
            exp = exp.And(x => x.TicketReplyContent != null && x.TicketReplyContent.Contains(queryDto.TicketReplyContent));
        }

        if (!string.IsNullOrEmpty(queryDto?.Attachments))
        {
            exp = exp.And(x => x.Attachments != null && x.Attachments.Contains(queryDto.Attachments));
        }

        if (queryDto?.IsInternal.HasValue == true)
        {
            exp = exp.And(x => x.IsInternal == queryDto.IsInternal);
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
