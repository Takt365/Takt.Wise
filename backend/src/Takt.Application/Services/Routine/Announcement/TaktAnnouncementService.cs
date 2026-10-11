// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.Announcement
// 文件名称：TaktAnnouncementService.cs
// 创建时间：2026-09-11
// 创建人：Takt365(Cursor AI)
// 功能描述：公告通知应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.Announcement;
using Takt.Domain.Entities.Routine.Announcement;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.Announcement;

/// <summary>
/// 公告通知应用服务
/// </summary>
public class TaktAnnouncementService : TaktServiceBase, ITaktAnnouncementService
{
    private readonly ITaktApprovalRepository<TaktAnnouncement> _announcementRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="announcementRepository">公告通知仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktAnnouncementService(
        ITaktApprovalRepository<TaktAnnouncement> announcementRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _announcementRepository = announcementRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取公告通知列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktAnnouncementDto>> GetAnnouncementListAsync(TaktAnnouncementQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktAnnouncementDto>.Create(
                new List<TaktAnnouncementDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _announcementRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktAnnouncementDto>.Create(
            data.Adapt<List<TaktAnnouncementDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取公告通知
    /// </summary>
    /// <param name="id">公告通知ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktAnnouncementDto?> GetAnnouncementByIdAsync(long id)
    {
        var entity = await _announcementRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktAnnouncementDto>();
    }

    /// <summary>
    /// 获取公告通知选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetAnnouncementOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _announcementRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.AnnouncementStatus == 1,
            x => x.AnnouncementCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.AnnouncementCode,
            DictLabel = e.AnnouncementCode,
        }).ToList();
    }

    /// <summary>
    /// 创建公告通知
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAnnouncementDto> CreateAnnouncementAsync(TaktAnnouncementCreateDto dto)
    {
        var entity = dto.Adapt<TaktAnnouncement>();
        var isUnique_ix_announcement_code_unique = await _uniqueValidator.IsUniqueAsync(
            _announcementRepository,
            x => x.AnnouncementCode == entity.AnnouncementCode);
        if (!isUnique_ix_announcement_code_unique)
        {
            throw new TaktBusinessException("公告通知的AnnouncementCode已存在");
        }
        entity = await _announcementRepository.CreateAsync(entity);
        return await GetAnnouncementByIdAsync(entity.Id) ?? entity.Adapt<TaktAnnouncementDto>();
    }

    /// <summary>
    /// 更新公告通知
    /// </summary>
    /// <param name="id">公告通知ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAnnouncementDto> UpdateAnnouncementAsync(long id, TaktAnnouncementUpdateDto dto)
    {
        var entity = await _announcementRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("公告通知不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_announcement_code_unique = await _uniqueValidator.IsUniqueAsync(
            _announcementRepository,
            x => x.AnnouncementCode == entity.AnnouncementCode,
            id);
        if (!isUnique_ix_announcement_code_unique)
        {
            throw new TaktBusinessException("公告通知的AnnouncementCode已存在");
        }
        await _announcementRepository.UpdateAsync(entity);
        return await GetAnnouncementByIdAsync(id) ?? throw new TaktBusinessException("公告通知不存在");
    }

    /// <summary>
    /// 删除公告通知
    /// </summary>
    /// <param name="id">公告通知ID</param>
    /// <returns>任务</returns>
    public async Task DeleteAnnouncementByIdAsync(long id)
    {
        var deleted = await _announcementRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("公告通知不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除公告通知
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteAnnouncementBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteAnnouncementByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新公告通知状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktAnnouncementDto> UpdateAnnouncementStatusAsync(TaktAnnouncementStatusDto dto)
    {
        var entity = await _announcementRepository.GetByIdAsync(dto.AnnouncementId);
        if (entity == null)
        {
            throw new TaktBusinessException("公告通知不存在");
        }
        entity.AnnouncementStatus = dto.AnnouncementStatus;
        await _announcementRepository.UpdateAsync(entity);
        return await GetAnnouncementByIdAsync(dto.AnnouncementId) ?? throw new TaktBusinessException("公告通知不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetAnnouncementTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktAnnouncementTemplateDto>(
            sheetName ?? "公告通知导入模板",
            fileName ?? "公告通知导入模板.xlsx");
    }

    /// <summary>
    /// 导入公告通知
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportAnnouncementAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktAnnouncementImportDto>(fileStream, sheetName ?? "公告通知导入模板");
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
                var entity = rows[i].Adapt<TaktAnnouncement>();
                var importKey = $"{entity.AnnouncementCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（AnnouncementCode）");
                }
                var isUnique_ix_announcement_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _announcementRepository,
                    x => x.AnnouncementCode == entity.AnnouncementCode);
                if (!isUnique_ix_announcement_code_unique)
                {
                    throw new TaktBusinessException("公告通知的AnnouncementCode已存在");
                }
                await _announcementRepository.CreateAsync(entity);
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
    /// 导出公告通知
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportAnnouncementAsync(TaktAnnouncementQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktAnnouncementQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAnnouncementExportDto>(),
                sheetName ?? "公告通知数据",
                fileName ?? "公告通知导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _announcementRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktAnnouncementExportDto>(),
                sheetName ?? "公告通知数据",
                fileName ?? "公告通知导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktAnnouncementExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "公告通知数据",
            fileName ?? "公告通知导出.xlsx");
    }

    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建公告通知查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktAnnouncement, bool>> QueryExpression(TaktAnnouncementQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktAnnouncement>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.AnnouncementCode != null && x.AnnouncementCode.Contains(keywords))
                || (x.AnnouncementTitle != null && x.AnnouncementTitle.Contains(keywords))
                || (x.Content != null && x.Content.Contains(keywords))
                || (x.Summary != null && x.Summary.Contains(keywords))
                || (x.Tags != null && x.Tags.Contains(keywords))
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.AccessUrl != null && x.AccessUrl.Contains(keywords))
                || (x.TargetDepartments != null && x.TargetDepartments.Contains(keywords))
                || (x.TargetUsers != null && x.TargetUsers.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.AnnouncementCode))
        {
            var announcementCode = queryDto.AnnouncementCode;
            exp = exp.And(x => x.AnnouncementCode != null && x.AnnouncementCode.Contains(announcementCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AnnouncementTitle))
        {
            var announcementTitle = queryDto.AnnouncementTitle;
            exp = exp.And(x => x.AnnouncementTitle != null && x.AnnouncementTitle.Contains(announcementTitle));
        }

        if (queryDto?.AnnouncementType.HasValue == true)
        {
            var announcementType = queryDto.AnnouncementType.Value;
            exp = exp.And(x => x.AnnouncementType == announcementType);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Content))
        {
            var content = queryDto.Content;
            exp = exp.And(x => x.Content != null && x.Content.Contains(content));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Summary))
        {
            var summary = queryDto.Summary;
            exp = exp.And(x => x.Summary != null && x.Summary.Contains(summary));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Tags))
        {
            var tags = queryDto.Tags;
            exp = exp.And(x => x.Tags != null && x.Tags.Contains(tags));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FileName))
        {
            var fileName = queryDto.FileName;
            exp = exp.And(x => x.FileName != null && x.FileName.Contains(fileName));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AccessUrl))
        {
            var accessUrl = queryDto.AccessUrl;
            exp = exp.And(x => x.AccessUrl != null && x.AccessUrl.Contains(accessUrl));
        }

        if (queryDto?.IsScheduled.HasValue == true)
        {
            var isScheduled = queryDto.IsScheduled.Value;
            exp = exp.And(x => x.IsScheduled == isScheduled);
        }

        if (queryDto?.IsTop.HasValue == true)
        {
            var isTop = queryDto.IsTop.Value;
            exp = exp.And(x => x.IsTop == isTop);
        }

        if (queryDto?.TopPriority.HasValue == true)
        {
            var topPriority = queryDto.TopPriority.Value;
            exp = exp.And(x => x.TopPriority == topPriority);
        }

        if (queryDto?.ViewCount.HasValue == true)
        {
            var viewCount = queryDto.ViewCount.Value;
            exp = exp.And(x => x.ViewCount == viewCount);
        }

        if (queryDto?.TargetScope.HasValue == true)
        {
            var targetScope = queryDto.TargetScope.Value;
            exp = exp.And(x => x.TargetScope == targetScope);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TargetDepartments))
        {
            var targetDepartments = queryDto.TargetDepartments;
            exp = exp.And(x => x.TargetDepartments != null && x.TargetDepartments.Contains(targetDepartments));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TargetUsers))
        {
            var targetUsers = queryDto.TargetUsers;
            exp = exp.And(x => x.TargetUsers != null && x.TargetUsers.Contains(targetUsers));
        }

        if (queryDto?.AnnouncementStatus.HasValue == true)
        {
            var announcementStatus = queryDto.AnnouncementStatus.Value;
            exp = exp.And(x => x.AnnouncementStatus == announcementStatus);
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

        if (queryDto?.PublishTimeStart.HasValue == true)
        {
            var publishTimeStart = queryDto.PublishTimeStart.Value;
            exp = exp.And(x => x.PublishTime >= publishTimeStart);
        }

        if (queryDto?.PublishTimeEnd.HasValue == true)
        {
            var publishTimeEnd = queryDto.PublishTimeEnd.Value;
            exp = exp.And(x => x.PublishTime <= publishTimeEnd);
        }

        if (queryDto?.ExpireTimeStart.HasValue == true)
        {
            var expireTimeStart = queryDto.ExpireTimeStart.Value;
            exp = exp.And(x => x.ExpireTime >= expireTimeStart);
        }

        if (queryDto?.ExpireTimeEnd.HasValue == true)
        {
            var expireTimeEnd = queryDto.ExpireTimeEnd.Value;
            exp = exp.And(x => x.ExpireTime <= expireTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktAnnouncementQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.AnnouncementCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AnnouncementTitle))
        {
            return true;
        }
        if (queryDto.AnnouncementType.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Content))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Summary))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Tags))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FileName))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AccessUrl))
        {
            return true;
        }
        if (queryDto.IsScheduled.HasValue)
        {
            return true;
        }
        if (queryDto.IsTop.HasValue)
        {
            return true;
        }
        if (queryDto.TopPriority.HasValue)
        {
            return true;
        }
        if (queryDto.ViewCount.HasValue)
        {
            return true;
        }
        if (queryDto.TargetScope.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TargetDepartments))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TargetUsers))
        {
            return true;
        }
        if (queryDto.AnnouncementStatus.HasValue)
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
        if (queryDto.PublishTimeStart.HasValue || queryDto.PublishTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.ExpireTimeStart.HasValue || queryDto.ExpireTimeEnd.HasValue)
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
