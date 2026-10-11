// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.NewsCenter
// 文件名称：TaktNewsShareService.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：新闻中心分享记录应用服务实现
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
/// 新闻中心分享记录应用服务
/// </summary>
public class TaktNewsShareService : TaktServiceBase, ITaktNewsShareService
{
    private readonly ITaktCompanyRepository<TaktNewsShare> _newsShareRepository;
    private readonly ITaktApprovalRepository<TaktNews> _newsRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="newsShareRepository">新闻中心分享记录仓储</param>
    /// <param name="newsRepository">新闻中心仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktNewsShareService(
        ITaktCompanyRepository<TaktNewsShare> newsShareRepository,
        ITaktApprovalRepository<TaktNews> newsRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _newsShareRepository = newsShareRepository;
        _newsRepository = newsRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取新闻中心分享记录列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktNewsShareDto>> GetNewsShareListAsync(TaktNewsShareQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktNewsShareDto>.Create(
                new List<TaktNewsShareDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _newsShareRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktNewsShareDto>.Create(
            data.Adapt<List<TaktNewsShareDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取新闻中心分享记录
    /// </summary>
    /// <param name="id">新闻中心分享记录ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsShareDto?> GetNewsShareByIdAsync(long id)
    {
        var entity = await _newsShareRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktNewsShareDto>();
    }

    /// <summary>
    /// 获取新闻中心分享记录选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetNewsShareOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _newsShareRepository.GetListAsync(
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
    /// 创建新闻中心分享记录
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsShareDto> CreateNewsShareAsync(TaktNewsShareCreateDto dto)
    {
        var entity = dto.Adapt<TaktNewsShare>();
        entity.IsObsolete = 0;
        await StampNewsShareNewsAsync(entity, dto);
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _newsShareRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.NewsId == entity.NewsId,
                x => x.LineNumber);
            var businessCode = entity.NewsId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _newsShareRepository.CreateAsync(entity);
        return await GetNewsShareByIdAsync(entity.Id) ?? entity.Adapt<TaktNewsShareDto>();
    }

    /// <summary>
    /// 更新新闻中心分享记录
    /// </summary>
    /// <param name="id">新闻中心分享记录ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsShareDto> UpdateNewsShareAsync(long id, TaktNewsShareUpdateDto dto)
    {
        var entity = await _newsShareRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心分享记录不存在");
        }
        dto.Adapt(entity);
        await StampNewsShareNewsAsync(entity, dto);
        await _newsShareRepository.UpdateAsync(entity);
        return await GetNewsShareByIdAsync(id) ?? throw new TaktBusinessException("新闻中心分享记录不存在");
    }

    /// <summary>
    /// 删除新闻中心分享记录
    /// </summary>
    /// <param name="id">新闻中心分享记录ID</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsShareByIdAsync(long id)
    {
        var entity = await _newsShareRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心分享记录不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("新闻中心分享记录不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("新闻中心分享记录已作废");
        }
        entity.IsObsolete = 1;
        await _newsShareRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除新闻中心分享记录
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteNewsShareBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteNewsShareByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新新闻中心分享记录作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktNewsShareDto> UpdateNewsShareObsoleteAsync(TaktNewsShareObsoleteDto dto)
    {
        var entity = await _newsShareRepository.GetByIdAsync(dto.NewsShareId);
        if (entity == null)
        {
            throw new TaktBusinessException("新闻中心分享记录不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("新闻中心分享记录不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _newsShareRepository.UpdateAsync(entity);
        return await GetNewsShareByIdAsync(dto.NewsShareId) ?? throw new TaktBusinessException("新闻中心分享记录不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetNewsShareTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktNewsShareTemplateDto>(
            sheetName ?? "新闻中心分享记录导入模板",
            fileName ?? "新闻中心分享记录导入模板.xlsx");
    }

    /// <summary>
    /// 导入新闻中心分享记录
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportNewsShareAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktNewsShareImportDto>(fileStream, sheetName ?? "新闻中心分享记录导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var entity = rows[i].Adapt<TaktNewsShare>();
                var importDto = rows[i].Adapt<TaktNewsShareCreateDto>();
                await StampNewsShareNewsAsync(entity, importDto);
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _newsShareRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.NewsId == entity.NewsId,
                        x => x.LineNumber);
                    var businessCode = entity.NewsId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _newsShareRepository.CreateAsync(entity);
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
    /// 导出新闻中心分享记录
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportNewsShareAsync(TaktNewsShareQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktNewsShareQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsShareExportDto>(),
                sheetName ?? "新闻中心分享记录数据",
                fileName ?? "新闻中心分享记录导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _newsShareRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktNewsShareExportDto>(),
                sheetName ?? "新闻中心分享记录数据",
                fileName ?? "新闻中心分享记录导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktNewsShareExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "新闻中心分享记录数据",
            fileName ?? "新闻中心分享记录导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步新闻中心分享记录主表外键（ManyToOne → 新闻中心）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampNewsShareNewsAsync(TaktNewsShare entity, TaktNewsShareCreateDto dto)
    {
        if (dto.NewsId <= 0)
        {
            return;
        }
        var master = await _newsRepository.GetByIdAsync(dto.NewsId);
        if (master == null)
        {
            throw new TaktBusinessException("新闻中心不存在");
        }
        entity.NewsId = master.Id;
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
    /// 构建新闻中心分享记录查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktNewsShare, bool>> QueryExpression(TaktNewsShareQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktNewsShare>();

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
                || (x.ShareChannel != null && x.ShareChannel.Contains(keywords))
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

        if (queryDto?.NewsId.HasValue == true)
        {
            var newsId = queryDto.NewsId.Value;
            exp = exp.And(x => x.NewsId == newsId);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ShareChannel))
        {
            var shareChannel = queryDto.ShareChannel;
            exp = exp.And(x => x.ShareChannel != null && x.ShareChannel.Contains(shareChannel));
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

        if (queryDto?.ShareTimeStart.HasValue == true)
        {
            var shareTimeStart = queryDto.ShareTimeStart.Value;
            exp = exp.And(x => x.ShareTime >= shareTimeStart);
        }

        if (queryDto?.ShareTimeEnd.HasValue == true)
        {
            var shareTimeEnd = queryDto.ShareTimeEnd.Value;
            exp = exp.And(x => x.ShareTime <= shareTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktNewsShareQueryDto? queryDto)
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
        if (queryDto.NewsId.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ShareChannel))
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
        if (queryDto.ShareTimeStart.HasValue || queryDto.ShareTimeEnd.HasValue)
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
