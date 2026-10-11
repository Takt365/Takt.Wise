// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：TaktTicketEvaluationService.cs
// 创建时间：2026-06-23
// 创建人：Takt365(Cursor AI)
// 功能描述：工单服务评价应用服务实现
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
/// 工单服务评价应用服务
/// </summary>
public class TaktTicketEvaluationService : TaktServiceBase, ITaktTicketEvaluationService
{
    private readonly ITaktCompanyRepository<TaktTicketEvaluation> _ticketEvaluationRepository;
    private readonly ITaktCompanyRepository<TaktTicket> _ticketRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketEvaluationRepository">工单服务评价仓储</param>
    /// <param name="ticketRepository">工单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTicketEvaluationService(
        ITaktCompanyRepository<TaktTicketEvaluation> ticketEvaluationRepository,
        ITaktCompanyRepository<TaktTicket> ticketRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ticketEvaluationRepository = ticketEvaluationRepository;
        _ticketRepository = ticketRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工单服务评价列表（分页）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTicketEvaluationDto>> GetTicketEvaluationListAsync(TaktTicketEvaluationQueryDto queryDto)
    {
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ticketEvaluationRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTicketEvaluationDto>.Create(
            data.Adapt<List<TaktTicketEvaluationDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工单服务评价
    /// </summary>
    /// <param name="id">工单服务评价ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketEvaluationDto?> GetTicketEvaluationByIdAsync(long id)
    {
        var entity = await _ticketEvaluationRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTicketEvaluationDto>();
    }

    /// <summary>
    /// 获取工单服务评价选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTicketEvaluationOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _ticketEvaluationRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.EvaluatorName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Id,
            DictLabel = e.EvaluatorName ?? e.Id.ToString(),
        }).ToList();
    }

    /// <summary>
    /// 创建工单服务评价
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketEvaluationDto> CreateTicketEvaluationAsync(TaktTicketEvaluationCreateDto dto)
    {
        var entity = dto.Adapt<TaktTicketEvaluation>();
        await StampTicketEvaluationTicketAsync(entity, dto);
        var isUnique_ix_ticket_evaluation_ticket_unique = await _uniqueValidator.IsUniqueAsync(
            _ticketEvaluationRepository,
            x => x.TicketId == entity.TicketId);
        if (!isUnique_ix_ticket_evaluation_ticket_unique)
        {
            throw new TaktBusinessException("工单服务评价的TicketId已存在");
        }
        entity = await _ticketEvaluationRepository.CreateAsync(entity);
        return await GetTicketEvaluationByIdAsync(entity.Id) ?? entity.Adapt<TaktTicketEvaluationDto>();
    }

    /// <summary>
    /// 更新工单服务评价
    /// </summary>
    /// <param name="id">工单服务评价ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketEvaluationDto> UpdateTicketEvaluationAsync(long id, TaktTicketEvaluationUpdateDto dto)
    {
        var entity = await _ticketEvaluationRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工单服务评价不存在");
        }
        dto.Adapt(entity);
        await StampTicketEvaluationTicketAsync(entity, dto);
        var isUnique_ix_ticket_evaluation_ticket_unique = await _uniqueValidator.IsUniqueAsync(
            _ticketEvaluationRepository,
            x => x.TicketId == entity.TicketId,
            id);
        if (!isUnique_ix_ticket_evaluation_ticket_unique)
        {
            throw new TaktBusinessException("工单服务评价的TicketId已存在");
        }
        await _ticketEvaluationRepository.UpdateAsync(entity);
        return await GetTicketEvaluationByIdAsync(id) ?? throw new TaktBusinessException("工单服务评价不存在");
    }

    /// <summary>
    /// 删除工单服务评价
    /// </summary>
    /// <param name="id">工单服务评价ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketEvaluationByIdAsync(long id)
    {
        var deleted = await _ticketEvaluationRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工单服务评价不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工单服务评价
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketEvaluationBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTicketEvaluationByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTicketEvaluationTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTicketEvaluationTemplateDto>(
            sheetName ?? "工单服务评价导入模板",
            fileName ?? "工单服务评价导入模板.xlsx");
    }

    /// <summary>
    /// 导入工单服务评价
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTicketEvaluationAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTicketEvaluationImportDto>(fileStream, sheetName ?? "工单服务评价导入模板");
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
                var entity = rows[i].Adapt<TaktTicketEvaluation>();
                var importDto = rows[i].Adapt<TaktTicketEvaluationCreateDto>();
                await StampTicketEvaluationTicketAsync(entity, importDto);
                var importKey = $"{entity.TicketId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（TicketId）");
                }
                var isUnique_ix_ticket_evaluation_ticket_unique = await _uniqueValidator.IsUniqueAsync(
                    _ticketEvaluationRepository,
                    x => x.TicketId == entity.TicketId);
                if (!isUnique_ix_ticket_evaluation_ticket_unique)
                {
                    throw new TaktBusinessException("工单服务评价的TicketId已存在");
                }
                await _ticketEvaluationRepository.CreateAsync(entity);
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
    /// 导出工单服务评价
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTicketEvaluationAsync(TaktTicketEvaluationQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var predicate = QueryExpression(query ?? new TaktTicketEvaluationQueryDto());
        var list = await _ticketEvaluationRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTicketEvaluationExportDto>(),
                sheetName ?? "工单服务评价数据",
                fileName ?? "工单服务评价导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTicketEvaluationExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工单服务评价数据",
            fileName ?? "工单服务评价导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步工单服务评价主表外键（ManyToOne → 工单）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampTicketEvaluationTicketAsync(TaktTicketEvaluation entity, TaktTicketEvaluationCreateDto dto)
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
    /// 构建工单服务评价查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTicketEvaluation, bool>> QueryExpression(TaktTicketEvaluationQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTicketEvaluation>();

        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                SqlFunc.ToString(x.TicketId).Contains(keywords)
                || SqlFunc.ToString(x.Score).Contains(keywords)
                || (x.Comment != null && x.Comment.Contains(keywords))
                || SqlFunc.ToString(x.EvaluatorId).Contains(keywords)
                || (x.EvaluatorName != null && x.EvaluatorName.Contains(keywords))
                || (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.ExtField != null && x.ExtField.Contains(keywords))
                || (x.Remark != null && x.Remark.Contains(keywords))
                || SqlFunc.ToString(x.EvaluatedAt).Contains(keywords)
                || SqlFunc.ToString(x.CreatedAt).Contains(keywords)
            );
        }

        if (queryDto?.TicketId.HasValue == true)
        {
            exp = exp.And(x => x.TicketId == queryDto.TicketId);
        }

        if (queryDto?.Score.HasValue == true)
        {
            exp = exp.And(x => x.Score == queryDto.Score);
        }

        if (!string.IsNullOrEmpty(queryDto?.Comment))
        {
            exp = exp.And(x => x.Comment != null && x.Comment.Contains(queryDto.Comment));
        }

        if (queryDto?.EvaluatorId.HasValue == true)
        {
            exp = exp.And(x => x.EvaluatorId == queryDto.EvaluatorId);
        }

        if (!string.IsNullOrEmpty(queryDto?.EvaluatorName))
        {
            exp = exp.And(x => x.EvaluatorName != null && x.EvaluatorName.Contains(queryDto.EvaluatorName));
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

        if (queryDto?.EvaluatedAtStart.HasValue == true)
        {
            exp = exp.And(x => x.EvaluatedAt >= queryDto.EvaluatedAtStart);
        }

        if (queryDto?.EvaluatedAtEnd.HasValue == true)
        {
            exp = exp.And(x => x.EvaluatedAt <= queryDto.EvaluatedAtEnd);
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
