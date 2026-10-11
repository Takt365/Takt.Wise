// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.HelpDesk
// 文件名称：TaktTicketService.cs
// 创建时间：2026-08-28
// 创建人：Takt365(Cursor AI)
// 功能描述：工单应用服务实现
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
/// 工单应用服务
/// </summary>
public class TaktTicketService : TaktServiceBase, ITaktTicketService
{
    private readonly ITaktCompanyRepository<TaktTicket> _ticketRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ticketRepository">工单仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktTicketService(
        ITaktCompanyRepository<TaktTicket> ticketRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ticketRepository = ticketRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取工单列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktTicketDto>> GetTicketListAsync(TaktTicketQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktTicketDto>.Create(
                new List<TaktTicketDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ticketRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktTicketDto>.Create(
            data.Adapt<List<TaktTicketDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto?> GetTicketByIdAsync(long id)
    {
        var entity = await _ticketRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktTicketDto>();
    }

    /// <summary>
    /// 获取工单选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetTicketOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _ticketRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.TicketStatus == 1,
            x => x.SubmitterName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.TicketCode,
            DictLabel = e.SubmitterName ?? e.TicketCode,
        }).ToList();
    }

    /// <summary>
    /// 创建工单
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> CreateTicketAsync(TaktTicketCreateDto dto)
    {
        var entity = dto.Adapt<TaktTicket>();
        var isUnique_ix_ticket_code_unique = await _uniqueValidator.IsUniqueAsync(
            _ticketRepository,
            x => x.TicketCode == entity.TicketCode);
        if (!isUnique_ix_ticket_code_unique)
        {
            throw new TaktBusinessException("工单的TicketCode已存在");
        }
        entity = await _ticketRepository.CreateAsync(entity);
        return await GetTicketByIdAsync(entity.Id) ?? entity.Adapt<TaktTicketDto>();
    }

    /// <summary>
    /// 更新工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> UpdateTicketAsync(long id, TaktTicketUpdateDto dto)
    {
        var entity = await _ticketRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("工单不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_ticket_code_unique = await _uniqueValidator.IsUniqueAsync(
            _ticketRepository,
            x => x.TicketCode == entity.TicketCode,
            id);
        if (!isUnique_ix_ticket_code_unique)
        {
            throw new TaktBusinessException("工单的TicketCode已存在");
        }
        await _ticketRepository.UpdateAsync(entity);
        return await GetTicketByIdAsync(id) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <summary>
    /// 删除工单
    /// </summary>
    /// <param name="id">工单ID</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketByIdAsync(long id)
    {
        var deleted = await _ticketRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("工单不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除工单
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteTicketBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteTicketByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新工单状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktTicketDto> UpdateTicketStatusAsync(TaktTicketStatusDto dto)
    {
        var entity = await _ticketRepository.GetByIdAsync(dto.TicketId);
        if (entity == null)
        {
            throw new TaktBusinessException("工单不存在");
        }
        entity.TicketStatus = dto.TicketStatus;
        await _ticketRepository.UpdateAsync(entity);
        return await GetTicketByIdAsync(dto.TicketId) ?? throw new TaktBusinessException("工单不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetTicketTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktTicketTemplateDto>(
            sheetName ?? "工单导入模板",
            fileName ?? "工单导入模板.xlsx");
    }

    /// <summary>
    /// 导入工单
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportTicketAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktTicketImportDto>(fileStream, sheetName ?? "工单导入模板");
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
                var entity = rows[i].Adapt<TaktTicket>();
                var importKey = $"{entity.TicketCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（TicketCode）");
                }
                var isUnique_ix_ticket_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _ticketRepository,
                    x => x.TicketCode == entity.TicketCode);
                if (!isUnique_ix_ticket_code_unique)
                {
                    throw new TaktBusinessException("工单的TicketCode已存在");
                }
                await _ticketRepository.CreateAsync(entity);
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
    /// 导出工单
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportTicketAsync(TaktTicketQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktTicketQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTicketExportDto>(),
                sheetName ?? "工单数据",
                fileName ?? "工单导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _ticketRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktTicketExportDto>(),
                sheetName ?? "工单数据",
                fileName ?? "工单导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktTicketExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "工单数据",
            fileName ?? "工单导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建工单查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktTicket, bool>> QueryExpression(TaktTicketQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktTicket>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.TicketCode != null && x.TicketCode.Contains(keywords))
                || (x.TicketTitle != null && x.TicketTitle.Contains(keywords))
                || (x.TicketContent != null && x.TicketContent.Contains(keywords))
                || (x.attachments != null && x.attachments.Contains(keywords))
                || (x.CategoryCode != null && x.CategoryCode.Contains(keywords))
                || (x.SubmitterName != null && x.SubmitterName.Contains(keywords))
                || (x.AssigneeName != null && x.AssigneeName.Contains(keywords))
                || (x.AssetCode != null && x.AssetCode.Contains(keywords))
                || (x.ApplicantDeptName != null && x.ApplicantDeptName.Contains(keywords))
                || (x.ApplicantName != null && x.ApplicantName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.TicketCode))
        {
            var ticketCode = queryDto.TicketCode;
            exp = exp.And(x => x.TicketCode != null && x.TicketCode.Contains(ticketCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TicketTitle))
        {
            var ticketTitle = queryDto.TicketTitle;
            exp = exp.And(x => x.TicketTitle != null && x.TicketTitle.Contains(ticketTitle));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TicketContent))
        {
            var ticketContent = queryDto.TicketContent;
            exp = exp.And(x => x.TicketContent != null && x.TicketContent.Contains(ticketContent));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.attachments))
        {
            var attachments = queryDto.attachments;
            exp = exp.And(x => x.attachments != null && x.attachments.Contains(attachments));
        }

        if (queryDto?.Priority.HasValue == true)
        {
            var priority = queryDto.Priority.Value;
            exp = exp.And(x => x.Priority == priority);
        }

        if (queryDto?.Urgency.HasValue == true)
        {
            var urgency = queryDto.Urgency.Value;
            exp = exp.And(x => x.Urgency == urgency);
        }

        if (queryDto?.Impact.HasValue == true)
        {
            var impact = queryDto.Impact.Value;
            exp = exp.And(x => x.Impact == impact);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CategoryCode))
        {
            var categoryCode = queryDto.CategoryCode;
            exp = exp.And(x => x.CategoryCode != null && x.CategoryCode.Contains(categoryCode));
        }

        if (queryDto?.TicketSource.HasValue == true)
        {
            var ticketSource = queryDto.TicketSource.Value;
            exp = exp.And(x => x.TicketSource == ticketSource);
        }

        if (queryDto?.SubmitterId.HasValue == true)
        {
            var submitterId = queryDto.SubmitterId.Value;
            exp = exp.And(x => x.SubmitterId == submitterId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SubmitterName))
        {
            var submitterName = queryDto.SubmitterName;
            exp = exp.And(x => x.SubmitterName != null && x.SubmitterName.Contains(submitterName));
        }

        if (queryDto?.AssigneeId.HasValue == true)
        {
            var assigneeId = queryDto.AssigneeId.Value;
            exp = exp.And(x => x.AssigneeId == assigneeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssigneeName))
        {
            var assigneeName = queryDto.AssigneeName;
            exp = exp.And(x => x.AssigneeName != null && x.AssigneeName.Contains(assigneeName));
        }

        if (queryDto?.KnowledgeId.HasValue == true)
        {
            var knowledgeId = queryDto.KnowledgeId.Value;
            exp = exp.And(x => x.KnowledgeId == knowledgeId);
        }

        if (queryDto?.ParentTicketId.HasValue == true)
        {
            var parentTicketId = queryDto.ParentTicketId.Value;
            exp = exp.And(x => x.ParentTicketId == parentTicketId);
        }

        if (queryDto?.ItAssetId.HasValue == true)
        {
            var itAssetId = queryDto.ItAssetId.Value;
            exp = exp.And(x => x.ItAssetId == itAssetId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AssetCode))
        {
            var assetCode = queryDto.AssetCode;
            exp = exp.And(x => x.AssetCode != null && x.AssetCode.Contains(assetCode));
        }

        if (queryDto?.ApplicantDeptId.HasValue == true)
        {
            var applicantDeptId = queryDto.ApplicantDeptId.Value;
            exp = exp.And(x => x.ApplicantDeptId == applicantDeptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApplicantDeptName))
        {
            var applicantDeptName = queryDto.ApplicantDeptName;
            exp = exp.And(x => x.ApplicantDeptName != null && x.ApplicantDeptName.Contains(applicantDeptName));
        }

        if (queryDto?.ApplicantBy.HasValue == true)
        {
            var applicantBy = queryDto.ApplicantBy.Value;
            exp = exp.And(x => x.ApplicantBy == applicantBy);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ApplicantName))
        {
            var applicantName = queryDto.ApplicantName;
            exp = exp.And(x => x.ApplicantName != null && x.ApplicantName.Contains(applicantName));
        }

        if (queryDto?.TicketStatus.HasValue == true)
        {
            var ticketStatus = queryDto.TicketStatus.Value;
            exp = exp.And(x => x.TicketStatus == ticketStatus);
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

        if (queryDto?.FirstResponseAtStart.HasValue == true)
        {
            var firstResponseAtStart = queryDto.FirstResponseAtStart.Value;
            exp = exp.And(x => x.FirstResponseAt >= firstResponseAtStart);
        }

        if (queryDto?.FirstResponseAtEnd.HasValue == true)
        {
            var firstResponseAtEnd = queryDto.FirstResponseAtEnd.Value;
            exp = exp.And(x => x.FirstResponseAt <= firstResponseAtEnd);
        }

        if (queryDto?.FirstResponseDueByStart.HasValue == true)
        {
            var firstResponseDueByStart = queryDto.FirstResponseDueByStart.Value;
            exp = exp.And(x => x.FirstResponseDueBy >= firstResponseDueByStart);
        }

        if (queryDto?.FirstResponseDueByEnd.HasValue == true)
        {
            var firstResponseDueByEnd = queryDto.FirstResponseDueByEnd.Value;
            exp = exp.And(x => x.FirstResponseDueBy <= firstResponseDueByEnd);
        }

        if (queryDto?.ResolvedAtStart.HasValue == true)
        {
            var resolvedAtStart = queryDto.ResolvedAtStart.Value;
            exp = exp.And(x => x.ResolvedAt >= resolvedAtStart);
        }

        if (queryDto?.ResolvedAtEnd.HasValue == true)
        {
            var resolvedAtEnd = queryDto.ResolvedAtEnd.Value;
            exp = exp.And(x => x.ResolvedAt <= resolvedAtEnd);
        }

        if (queryDto?.ResolutionDueByStart.HasValue == true)
        {
            var resolutionDueByStart = queryDto.ResolutionDueByStart.Value;
            exp = exp.And(x => x.ResolutionDueBy >= resolutionDueByStart);
        }

        if (queryDto?.ResolutionDueByEnd.HasValue == true)
        {
            var resolutionDueByEnd = queryDto.ResolutionDueByEnd.Value;
            exp = exp.And(x => x.ResolutionDueBy <= resolutionDueByEnd);
        }

        if (queryDto?.ClosedAtStart.HasValue == true)
        {
            var closedAtStart = queryDto.ClosedAtStart.Value;
            exp = exp.And(x => x.ClosedAt >= closedAtStart);
        }

        if (queryDto?.ClosedAtEnd.HasValue == true)
        {
            var closedAtEnd = queryDto.ClosedAtEnd.Value;
            exp = exp.And(x => x.ClosedAt <= closedAtEnd);
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
    private static bool HasAnyListQueryFilter(TaktTicketQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.TicketCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TicketTitle))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TicketContent))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.attachments))
        {
            return true;
        }
        if (queryDto.Priority.HasValue)
        {
            return true;
        }
        if (queryDto.Urgency.HasValue)
        {
            return true;
        }
        if (queryDto.Impact.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CategoryCode))
        {
            return true;
        }
        if (queryDto.TicketSource.HasValue)
        {
            return true;
        }
        if (queryDto.SubmitterId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SubmitterName))
        {
            return true;
        }
        if (queryDto.AssigneeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssigneeName))
        {
            return true;
        }
        if (queryDto.KnowledgeId.HasValue)
        {
            return true;
        }
        if (queryDto.ParentTicketId.HasValue)
        {
            return true;
        }
        if (queryDto.ItAssetId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AssetCode))
        {
            return true;
        }
        if (queryDto.ApplicantDeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApplicantDeptName))
        {
            return true;
        }
        if (queryDto.ApplicantBy.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ApplicantName))
        {
            return true;
        }
        if (queryDto.TicketStatus.HasValue)
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
        if (queryDto.FirstResponseAtStart.HasValue || queryDto.FirstResponseAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.FirstResponseDueByStart.HasValue || queryDto.FirstResponseDueByEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ResolvedAtStart.HasValue || queryDto.ResolvedAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ResolutionDueByStart.HasValue || queryDto.ResolutionDueByEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ClosedAtStart.HasValue || queryDto.ClosedAtEnd.HasValue)
        {
            return true;
        }
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }

    // ========================================
    // 扩展方法（保留）
    // ========================================

    /// <summary>
    /// 获取服务台工单件数统计（数据看板；按 CreatedAt 与 TicketStatus）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>工单件数统计</returns>
    public async Task<TaktTicketStatDto> GetTicketStatAsync(TaktTicketStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveStatMonthRange(
            queryDto.CreatedAtStart,
            queryDto.CreatedAtEnd,
            queryDto.StatMonth);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        // 字典 sys_ticket_status：0新建 1已分配 2处理中 3待确认 4已完成 5已关闭 6已取消 7重新打开
        Expression<Func<TaktTicket, bool>> inMonth = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end;
        Expression<Func<TaktTicket, bool>> pending = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end
            && (x.TicketStatus == 0 || x.TicketStatus == 1);
        Expression<Func<TaktTicket, bool>> inProgress = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end
            && (x.TicketStatus == 2 || x.TicketStatus == 3 || x.TicketStatus == 7);
        Expression<Func<TaktTicket, bool>> processed = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.CreatedAt >= start
            && x.CreatedAt <= end
            && (x.TicketStatus == 4 || x.TicketStatus == 5 || x.TicketStatus == 6);
        var monthTicketCount = await _ticketRepository.CountAsync(inMonth);
        var monthPendingCount = await _ticketRepository.CountAsync(pending);
        var monthInProgressCount = await _ticketRepository.CountAsync(inProgress);
        var monthProcessedCount = await _ticketRepository.CountAsync(processed);
        return new TaktTicketStatDto
        {
            StatMonth = statMonth,
            MonthTicketCount = monthTicketCount,
            MonthPendingCount = monthPendingCount,
            MonthInProgressCount = monthInProgressCount,
            MonthProcessedCount = monthProcessedCount,
        };
    }
}
