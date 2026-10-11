// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.NewsCenter
// 文件名称：TaktNewsCommentLikeService.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心评论点赞记录应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.NewsCenter;
using Takt.Domain.Entities.Routine.NewsCenter;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.NewsCenter;

/// <summary>
/// 新闻中心评论点赞记录应用服务
/// </summary>
public class TaktNewsCommentLikeService : TaktServiceBase, ITaktNewsCommentLikeService
{
    private readonly ITaktCompanyRepository<TaktNewsCommentLike> _newsCommentLikeRepository;
    private readonly ITaktApprovalRepository<TaktNewsComment> _newsCommentRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsCommentLikeRepository">新闻中心评论点赞记录仓储</param>
    /// <param name="newsCommentRepository">新闻中心评论仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNewsCommentLikeService(
        ITaktCompanyRepository<TaktNewsCommentLike> newsCommentLikeRepository,
        ITaktApprovalRepository<TaktNewsComment> newsCommentRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _newsCommentLikeRepository = newsCommentLikeRepository;
        _newsCommentRepository = newsCommentRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取新闻中心评论点赞记录列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktNewsCommentLikeDto>> GetNewsCommentLikeListAsync(TaktNewsCommentLikeQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktNewsCommentLikeDto>.Create(
                new List<TaktNewsCommentLikeDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _newsCommentLikeRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktNewsCommentLikeDto>.Create(
            data.Adapt<List<TaktNewsCommentLikeDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取新闻中心评论点赞记录
    /// </summary>
    /// <param name="id">新闻中心评论点赞记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentLikeDto?> GetNewsCommentLikeByIdAsync(long id)
    {
        var entity = await _newsCommentLikeRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktNewsCommentLikeDto>();
    }

    /// <summary>
    /// 获取新闻中心评论点赞记录选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetNewsCommentLikeOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _newsCommentLikeRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.UserName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.UserName,
            DictLabel = e.UserName,
        }).ToList();
    }

    /// <summary>
    /// 创建新闻中心评论点赞记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentLikeDto> CreateNewsCommentLikeAsync(TaktNewsCommentLikeCreateDto dto)
    {
        var entity = dto.Adapt<TaktNewsCommentLike>();
        entity.IsObsolete = 0;
        await StampNewsCommentLikeNewsCommentAsync(entity, dto);
        var isUnique_ix_news_comment_like_unique = await _uniqueValidator.IsUniqueAsync(
            _newsCommentLikeRepository,
            x => x.CommentId == entity.CommentId
                && x.UserId == entity.UserId);
        if (!isUnique_ix_news_comment_like_unique)
        {
            throw new TaktBusinessException("新闻中心评论点赞记录的CommentId、UserId已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _newsCommentLikeRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CommentId == entity.CommentId,
                x => x.LineNumber);
            var businessCode = entity.CommentId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _newsCommentLikeRepository.CreateAsync(entity);
        return await GetNewsCommentLikeByIdAsync(entity.Id) ?? entity.Adapt<TaktNewsCommentLikeDto>();
    }

    /// <summary>
    /// 更新新闻中心评论点赞记录
    /// </summary>
    /// <param name="id">新闻中心评论点赞记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentLikeDto> UpdateNewsCommentLikeAsync(long id, TaktNewsCommentLikeUpdateDto dto)
    {
        var entity = await _newsCommentLikeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心评论点赞记录不存在");
        }
        dto.Adapt(entity);
        await StampNewsCommentLikeNewsCommentAsync(entity, dto);
        var isUnique_ix_news_comment_like_unique = await _uniqueValidator.IsUniqueAsync(
            _newsCommentLikeRepository,
            x => x.CommentId == entity.CommentId
                && x.UserId == entity.UserId,
            id);
        if (!isUnique_ix_news_comment_like_unique)
        {
            throw new TaktBusinessException("新闻中心评论点赞记录的CommentId、UserId已存在");
        }
        await _newsCommentLikeRepository.UpdateAsync(entity);
        return await GetNewsCommentLikeByIdAsync(id) ?? throw new TaktBusinessException("新闻中心评论点赞记录不存在");
    }

    /// <summary>
    /// 删除新闻中心评论点赞记录
    /// </summary>
    /// <param name="id">新闻中心评论点赞记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsCommentLikeByIdAsync(long id)
    {
        var entity = await _newsCommentLikeRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心评论点赞记录不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("新闻中心评论点赞记录不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("新闻中心评论点赞记录已作废");
        }
        entity.IsObsolete = 1;
        await _newsCommentLikeRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除新闻中心评论点赞记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsCommentLikeBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteNewsCommentLikeByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新新闻中心评论点赞记录作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsCommentLikeDto> UpdateNewsCommentLikeObsoleteAsync(TaktNewsCommentLikeObsoleteDto dto)
    {
        var entity = await _newsCommentLikeRepository.GetByIdAsync(dto.NewsCommentLikeId);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心评论点赞记录不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("新闻中心评论点赞记录不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _newsCommentLikeRepository.UpdateAsync(entity);
        return await GetNewsCommentLikeByIdAsync(dto.NewsCommentLikeId) ?? throw new TaktBusinessException("新闻中心评论点赞记录不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetNewsCommentLikeTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktNewsCommentLikeTemplateDto>(
            sheetName ?? "新闻中心评论点赞记录导入模板",
            fileName ?? "新闻中心评论点赞记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入新闻中心评论点赞记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportNewsCommentLikeAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktNewsCommentLikeImportDto>(fileStream, sheetName ?? "新闻中心评论点赞记录导入模板");
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
                var entity = rows[i].Adapt<TaktNewsCommentLike>();
                var importDto = rows[i].Adapt<TaktNewsCommentLikeCreateDto>();
                await StampNewsCommentLikeNewsCommentAsync(entity, importDto);
                var importKey = $"{entity.CommentId}|{entity.UserId}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（CommentId、UserId）");
                }
                var isUnique_ix_news_comment_like_unique = await _uniqueValidator.IsUniqueAsync(
                    _newsCommentLikeRepository,
                    x => x.CommentId == entity.CommentId
                        && x.UserId == entity.UserId);
                if (!isUnique_ix_news_comment_like_unique)
                {
                    throw new TaktBusinessException("新闻中心评论点赞记录的CommentId、UserId已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _newsCommentLikeRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CommentId == entity.CommentId,
                        x => x.LineNumber);
                    var businessCode = entity.CommentId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _newsCommentLikeRepository.CreateAsync(entity);
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
    /// 导出新闻中心评论点赞记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportNewsCommentLikeAsync(TaktNewsCommentLikeQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktNewsCommentLikeQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsCommentLikeExportDto>(),
                sheetName ?? "新闻中心评论点赞记录数据",
                fileName ?? "新闻中心评论点赞记录导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _newsCommentLikeRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsCommentLikeExportDto>(),
                sheetName ?? "新闻中心评论点赞记录数据",
                fileName ?? "新闻中心评论点赞记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktNewsCommentLikeExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "新闻中心评论点赞记录数据",
            fileName ?? "新闻中心评论点赞记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步新闻中心评论点赞记录主表外键（ManyToOne → 新闻中心评论）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampNewsCommentLikeNewsCommentAsync(TaktNewsCommentLike entity, TaktNewsCommentLikeCreateDto dto)
    {
        if (dto.CommentId <= 0)
        {
            return;
        }
        var master = await _newsCommentRepository.GetByIdAsync(dto.CommentId);
        if (master == null)
        {
            throw new TaktBusinessException("新闻中心评论不存在");
        }
        entity.CommentId = master.Id;
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
        if (string.IsNullOrEmpty(entity.UserName))
        {
            entity.UserName = master.UserName;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建新闻中心评论点赞记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktNewsCommentLike, bool>> QueryExpression(TaktNewsCommentLikeQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktNewsCommentLike>();

        if (queryDto?.IsObsolete.HasValue == true)
        {
            exp = exp.And(x => x.IsObsolete == queryDto.IsObsolete);
        }
        else
        {
            exp = exp.And(x => x.IsObsolete == 0);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.UserName != null && x.UserName.Contains(keywords))
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

        if (queryDto?.CommentId.HasValue == true)
        {
            var commentId = queryDto.CommentId.Value;
            exp = exp.And(x => x.CommentId == commentId);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.UserId.HasValue == true)
        {
            var userId = queryDto.UserId.Value;
            exp = exp.And(x => x.UserId == userId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UserName))
        {
            var UserName = queryDto.UserName;
            exp = exp.And(x => x.UserName != null && x.UserName.Contains(UserName));
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

        if (queryDto?.LikeTimeStart.HasValue == true)
        {
            var likeTimeStart = queryDto.LikeTimeStart.Value;
            exp = exp.And(x => x.LikeTime >= likeTimeStart);
        }

        if (queryDto?.LikeTimeEnd.HasValue == true)
        {
            var likeTimeEnd = queryDto.LikeTimeEnd.Value;
            exp = exp.And(x => x.LikeTime <= likeTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktNewsCommentLikeQueryDto? queryDto)
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
        if (queryDto.CommentId.HasValue)
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.UserId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UserName))
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
        if (queryDto.IsObsolete.HasValue)
        {
            return true;
        }
        if (queryDto.LikeTimeStart.HasValue || queryDto.LikeTimeEnd.HasValue)
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
