// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Sop
// 文件名称：TaktSopRevisionService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：SOP版本应用服务实现
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
/// SOP版本应用服务
/// </summary>
public class TaktSopRevisionService : TaktServiceBase, ITaktSopRevisionService
{
    private readonly ITaktCompanyRepository<TaktSopRevision> _sopRevisionRepository;
    private readonly ITaktCompanyRepository<TaktSopContent> _sopContentRepository;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sopRevisionRepository">SOP版本仓储</param>
    /// <param name="sopContentRepository">SopContent仓储</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSopRevisionService(
        ITaktCompanyRepository<TaktSopRevision> sopRevisionRepository,
        ITaktCompanyRepository<TaktSopContent> sopContentRepository,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sopRevisionRepository = sopRevisionRepository;
        _sopContentRepository = sopContentRepository;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取SOP版本列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSopRevisionDto>> GetSopRevisionListAsync(TaktSopRevisionQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSopRevisionDto>.Create(
                new List<TaktSopRevisionDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sopRevisionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSopRevisionDto>.Create(
            data.Adapt<List<TaktSopRevisionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取SOP版本
    /// </summary>
    /// <param name="id">SOP版本ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopRevisionDto?> GetSopRevisionByIdAsync(long id)
    {
        var entity = await _sopRevisionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSopRevisionDto>();
        await FillSopRevisionDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取SOP版本选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSopRevisionOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sopRevisionRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.RevisionStatus == 1,
            x => x.Revision ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.Revision,
            DictLabel = e.Revision,
        }).ToList();
    }

    /// <summary>
    /// 创建SOP版本
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopRevisionDto> CreateSopRevisionAsync(TaktSopRevisionCreateDto dto)
    {
        var entity = dto.Adapt<TaktSopRevision>();
        var isUnique_ix_takt_logistics_manufacturing_sop_revision_unique = await _uniqueValidator.IsUniqueAsync(
            _sopRevisionRepository,
            x => x.SopId == entity.SopId
                && x.Revision == entity.Revision);
        if (!isUnique_ix_takt_logistics_manufacturing_sop_revision_unique)
        {
            throw new TaktBusinessException("SOP版本的SopId、Revision已存在");
        }
        entity = await _sopRevisionRepository.CreateAsync(entity);
                await SaveSopRevisionChildrenAsync(entity, dto);
        return await GetSopRevisionByIdAsync(entity.Id) ?? entity.Adapt<TaktSopRevisionDto>();
    }

    /// <summary>
    /// 更新SOP版本
    /// </summary>
    /// <param name="id">SOP版本ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopRevisionDto> UpdateSopRevisionAsync(long id, TaktSopRevisionUpdateDto dto)
    {
        var entity = await _sopRevisionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP版本不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_sop_revision_unique = await _uniqueValidator.IsUniqueAsync(
            _sopRevisionRepository,
            x => x.SopId == entity.SopId
                && x.Revision == entity.Revision,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_sop_revision_unique)
        {
            throw new TaktBusinessException("SOP版本的SopId、Revision已存在");
        }
        await _sopRevisionRepository.UpdateAsync(entity);
                await SaveSopRevisionChildrenAsync(entity, dto);
        return await GetSopRevisionByIdAsync(id) ?? throw new TaktBusinessException("SOP版本不存在");
    }

    /// <summary>
    /// 删除SOP版本
    /// </summary>
    /// <param name="id">SOP版本ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSopRevisionByIdAsync(long id)
    {
        var entity = await _sopRevisionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP版本不存在或已删除");
        }
        await _sopContentRepository.DeleteAsync(x => x.RevisionId == entity.Id);
        var deleted = await _sopRevisionRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("SOP版本不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除SOP版本
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSopRevisionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSopRevisionByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新SOP版本状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSopRevisionDto> UpdateSopRevisionStatusAsync(TaktSopRevisionStatusDto dto)
    {
        var entity = await _sopRevisionRepository.GetByIdAsync(dto.SopRevisionId);
        if (entity == null)
        {
            throw new TaktBusinessException("SOP版本不存在");
        }
        entity.RevisionStatus = dto.RevisionStatus;
        await _sopRevisionRepository.UpdateAsync(entity);
        return await GetSopRevisionByIdAsync(dto.SopRevisionId) ?? throw new TaktBusinessException("SOP版本不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSopRevisionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSopRevisionTemplateDto>(
            sheetName ?? "SOP版本导入模板",
            fileName ?? "SOP版本导入模板.xlsx");
    }

    /// <summary>
    /// 导入SOP版本
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSopRevisionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSopRevisionImportDto>(fileStream, sheetName ?? "SOP版本导入模板");
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
                var entity = rows[i].Adapt<TaktSopRevision>();
                var importKey = $"{entity.SopId}|{entity.Revision}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（SopId、Revision）");
                }
                var isUnique_ix_takt_logistics_manufacturing_sop_revision_unique = await _uniqueValidator.IsUniqueAsync(
                    _sopRevisionRepository,
                    x => x.SopId == entity.SopId
                        && x.Revision == entity.Revision);
                if (!isUnique_ix_takt_logistics_manufacturing_sop_revision_unique)
                {
                    throw new TaktBusinessException("SOP版本的SopId、Revision已存在");
                }
                await _sopRevisionRepository.CreateAsync(entity);
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
    /// 导出SOP版本
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSopRevisionAsync(TaktSopRevisionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSopRevisionQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopRevisionExportDto>(),
                sheetName ?? "SOP版本数据",
                fileName ?? "SOP版本导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _sopRevisionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSopRevisionExportDto>(),
                sheetName ?? "SOP版本数据",
                fileName ?? "SOP版本导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSopRevisionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "SOP版本数据",
            fileName ?? "SOP版本导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 填充SOP版本详情（加载 OneToMany 子表：SOP多语言正文）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSopRevisionDetailsAsync(TaktSopRevisionDto dto, TaktSopRevision entity)
    {
        if (dto == null)
        {
            return;
        }
        // SOP多语言正文 → dto.Contents
        var contents = await _sopContentRepository.GetListAsync(x => x.RevisionId == entity.Id);
        dto.Contents = contents.Adapt<List<TaktSopContentDto>>();
    }

    /// <summary>
    /// 保存SOP版本子表级联（SOP多语言正文；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSopRevisionChildrenAsync(TaktSopRevision entity, TaktSopRevisionCreateDto dto)
    {
        // SOP多语言正文（Contents）
        List<TaktSopContentUpdateDto>? contentsForSave;
        if (dto is TaktSopRevisionUpdateDto updateDtoForContents && updateDtoForContents.Contents != null)
        {
            contentsForSave = updateDtoForContents.Contents;
        }
        else if (dto.Contents != null)
        {
            contentsForSave = dto.Contents.Adapt<List<TaktSopContentUpdateDto>>();
        }
        else
        {
            contentsForSave = null;
        }
        if (contentsForSave is not { Count: > 0 })
        {
            await _sopContentRepository.DeleteAsync(x => x.RevisionId == entity.Id);
        }
        else
        {
            var existingList = await _sopContentRepository.GetListAsync(x => x.RevisionId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSopContent>();
            for (var i = 0; i < contentsForSave.Count; i++)
            {
                var childDto = contentsForSave[i];
                childDto.RevisionId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                if (childDto.SopContentId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SopContentId, out var target))
                    {
                        throw new TaktBusinessException("SOP多语言正文不存在（SopContentId={childDto.SopContentId}）");
                    }
                    if (target.RevisionId != entity.Id)
                    {
                        throw new TaktBusinessException("SOP多语言正文不属于当前主表（SopContentId={childDto.SopContentId}）");
                    }
                    submittedIds.Add(childDto.SopContentId);
                    childDto.Adapt(target);
                    target.Id = childDto.SopContentId;
                    target.RevisionId = entity.Id;
                    await _sopContentRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktSopContent>();
                    child.Id = 0;
                    child.RevisionId = entity.Id;
                    toCreate.Add(child);
                }
            }
            foreach (var removed in existingList.Where(x => !submittedIds.Contains(x.Id)))
            {
                await _sopContentRepository.DeleteAsync(removed.Id);
            }
            if (toCreate.Count > 0)
            {
                await _sopContentRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建SOP版本查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSopRevision, bool>> QueryExpression(TaktSopRevisionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSopRevision>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.Revision != null && x.Revision.Contains(keywords))
                || (x.FileUrl != null && x.FileUrl.Contains(keywords))
                || (x.ChangeDesc != null && x.ChangeDesc.Contains(keywords))
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

        if (queryDto?.SopId.HasValue == true)
        {
            var sopId = queryDto.SopId.Value;
            exp = exp.And(x => x.SopId == sopId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Revision))
        {
            var revision = queryDto.Revision;
            exp = exp.And(x => x.Revision != null && x.Revision.Contains(revision));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.FileUrl))
        {
            var fileUrl = queryDto.FileUrl;
            exp = exp.And(x => x.FileUrl != null && x.FileUrl.Contains(fileUrl));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ChangeDesc))
        {
            var changeDesc = queryDto.ChangeDesc;
            exp = exp.And(x => x.ChangeDesc != null && x.ChangeDesc.Contains(changeDesc));
        }

        if (queryDto?.EcnId.HasValue == true)
        {
            var ecnId = queryDto.EcnId.Value;
            exp = exp.And(x => x.EcnId == ecnId);
        }

        if (queryDto?.IsLocked.HasValue == true)
        {
            var isLocked = queryDto.IsLocked.Value;
            exp = exp.And(x => x.IsLocked == isLocked);
        }

        if (queryDto?.ForceLeaderAck.HasValue == true)
        {
            var forceLeaderAck = queryDto.ForceLeaderAck.Value;
            exp = exp.And(x => x.ForceLeaderAck == forceLeaderAck);
        }

        if (queryDto?.RevisionStatus.HasValue == true)
        {
            var revisionStatus = queryDto.RevisionStatus.Value;
            exp = exp.And(x => x.RevisionStatus == revisionStatus);
        }

        if (queryDto?.EffectiveRule.HasValue == true)
        {
            var effectiveRule = queryDto.EffectiveRule.Value;
            exp = exp.And(x => x.EffectiveRule == effectiveRule);
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
    private static bool HasAnyListQueryFilter(TaktSopRevisionQueryDto? queryDto)
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
        if (queryDto.SopId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Revision))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.FileUrl))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ChangeDesc))
        {
            return true;
        }
        if (queryDto.EcnId.HasValue)
        {
            return true;
        }
        if (queryDto.IsLocked.HasValue)
        {
            return true;
        }
        if (queryDto.ForceLeaderAck.HasValue)
        {
            return true;
        }
        if (queryDto.RevisionStatus.HasValue)
        {
            return true;
        }
        if (queryDto.EffectiveRule.HasValue)
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
