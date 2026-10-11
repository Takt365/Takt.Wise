// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.DocumentCenter
// 文件名称：TaktDocumentService.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：文管中心应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Routine.DocumentCenter;
using Takt.Domain.Entities.Routine.DocumentCenter;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Routine.DocumentCenter;

/// <summary>
/// 文管中心应用服务
/// </summary>
public class TaktDocumentService : TaktServiceBase, ITaktDocumentService
{
    private readonly ITaktApprovalRepository<TaktDocument> _documentRepository;
    private readonly ITaktCompanyRepository<TaktDocumentVersion> _documentVersionRepository;
    private readonly ITaktSortOrderGenerator _sortOrderGenerator;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="documentRepository">文管中心仓储</param>
    /// <param name="documentVersionRepository">DocumentVersion仓储</param>
    /// <param name="sortOrderGenerator">排序号生成器</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDocumentService(
        ITaktApprovalRepository<TaktDocument> documentRepository,
        ITaktCompanyRepository<TaktDocumentVersion> documentVersionRepository,
        ITaktSortOrderGenerator sortOrderGenerator,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _documentRepository = documentRepository;
        _documentVersionRepository = documentVersionRepository;
        _sortOrderGenerator = sortOrderGenerator;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取文管中心列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDocumentDto>> GetDocumentListAsync(TaktDocumentQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktDocumentDto>.Create(
                new List<TaktDocumentDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _documentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDocumentDto>.Create(
            data.Adapt<List<TaktDocumentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取文管中心
    /// </summary>
    /// <param name="id">文管中心ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentDto?> GetDocumentByIdAsync(long id)
    {
        var entity = await _documentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktDocumentDto>();
        await FillDocumentDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取文管中心选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetDocumentOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _documentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DocumentStatus == 1,
            x => x.PublisherName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.DocumentCode,
            DictLabel = e.PublisherName ?? e.DocumentCode,
        }).ToList();
    }

    /// <summary>
    /// 创建文管中心
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentDto> CreateDocumentAsync(TaktDocumentCreateDto dto)
    {
        var entity = dto.Adapt<TaktDocument>();
        var isUnique_ix_document_code_unique = await _uniqueValidator.IsUniqueAsync(
            _documentRepository,
            x => x.DocumentCode == entity.DocumentCode);
        if (!isUnique_ix_document_code_unique)
        {
            throw new TaktBusinessException("文管中心的DocumentCode已存在");
        }
        if (entity.SortOrder <= 0)
        {
            var maxSort = await _documentRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PublisherId == entity.PublisherId,
                x => x.SortOrder);
            entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.PublisherId, maxSort);
        }
        entity = await _documentRepository.CreateAsync(entity);
                await SaveDocumentChildrenAsync(entity, dto);
        return await GetDocumentByIdAsync(entity.Id) ?? entity.Adapt<TaktDocumentDto>();
    }

    /// <summary>
    /// 更新文管中心
    /// </summary>
    /// <param name="id">文管中心ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentDto> UpdateDocumentAsync(long id, TaktDocumentUpdateDto dto)
    {
        var entity = await _documentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("文管中心不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_document_code_unique = await _uniqueValidator.IsUniqueAsync(
            _documentRepository,
            x => x.DocumentCode == entity.DocumentCode,
            id);
        if (!isUnique_ix_document_code_unique)
        {
            throw new TaktBusinessException("文管中心的DocumentCode已存在");
        }
        await _documentRepository.UpdateAsync(entity);
                await SaveDocumentChildrenAsync(entity, dto);
        return await GetDocumentByIdAsync(id) ?? throw new TaktBusinessException("文管中心不存在");
    }

    /// <summary>
    /// 删除文管中心
    /// </summary>
    /// <param name="id">文管中心ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDocumentByIdAsync(long id)
    {
        var entity = await _documentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("文管中心不存在或已删除");
        }
        await _documentVersionRepository.DeleteAsync(x => x.DocumentId == entity.Id);
        var deleted = await _documentRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("文管中心不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除文管中心
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDocumentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteDocumentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新文管中心状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentDto> UpdateDocumentStatusAsync(TaktDocumentStatusDto dto)
    {
        var entity = await _documentRepository.GetByIdAsync(dto.DocumentId);
        if (entity == null)
        {
            throw new TaktBusinessException("文管中心不存在");
        }
        entity.DocumentStatus = dto.DocumentStatus;
        await _documentRepository.UpdateAsync(entity);
        return await GetDocumentByIdAsync(dto.DocumentId) ?? throw new TaktBusinessException("文管中心不存在");
    }

    /// <summary>
    /// 更新文管中心排序
    /// </summary>
    /// <param name="dto">排序DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentDto> UpdateDocumentSortAsync(TaktDocumentSortDto dto)
    {
        var entity = await _documentRepository.GetByIdAsync(dto.DocumentId);
        if (entity == null)
        {
            throw new TaktBusinessException("文管中心不存在");
        }
        entity.SortOrder = dto.SortOrder;
        await _documentRepository.UpdateAsync(entity);
        return await GetDocumentByIdAsync(dto.DocumentId) ?? throw new TaktBusinessException("文管中心不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetDocumentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktDocumentTemplateDto>(
            sheetName ?? "文管中心导入模板",
            fileName ?? "文管中心导入模板.xlsx");
    }

    /// <summary>
    /// 导入文管中心
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportDocumentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktDocumentImportDto>(fileStream, sheetName ?? "文管中心导入模板");
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
                var entity = rows[i].Adapt<TaktDocument>();
                var importKey = $"{entity.DocumentCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DocumentCode）");
                }
                var isUnique_ix_document_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _documentRepository,
                    x => x.DocumentCode == entity.DocumentCode);
                if (!isUnique_ix_document_code_unique)
                {
                    throw new TaktBusinessException("文管中心的DocumentCode已存在");
                }
                if (entity.SortOrder <= 0)
                {
                    var maxSort = await _documentRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PublisherId == entity.PublisherId,
                        x => x.SortOrder);
                    entity.SortOrder = _sortOrderGenerator.GenerateNextForMaster(entity.PublisherId, maxSort);
                }
                await _documentRepository.CreateAsync(entity);
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
    /// 导出文管中心
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDocumentAsync(TaktDocumentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktDocumentQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDocumentExportDto>(),
                sheetName ?? "文管中心数据",
                fileName ?? "文管中心导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _documentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDocumentExportDto>(),
                sheetName ?? "文管中心数据",
                fileName ?? "文管中心导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDocumentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "文管中心数据",
            fileName ?? "文管中心导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废文管文档版本标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="documentId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkDocumentVersionsObsoleteAsync(long documentId)
    {
        if (documentId <= 0)
        {
            return;
        }
        var rows = await _documentVersionRepository.GetListAsync(
            x => x.DocumentId == documentId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _documentVersionRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充文管中心详情（加载 OneToMany 子表：文管文档版本）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillDocumentDetailsAsync(TaktDocumentDto dto, TaktDocument entity)
    {
        if (dto == null)
        {
            return;
        }
        // 文管文档版本 → dto.Versions（含作废行）
        var versions = await _documentVersionRepository.GetListAsync(x => x.DocumentId == entity.Id);
        dto.Versions = versions.Adapt<List<TaktDocumentVersionDto>>();
    }

    /// <summary>
    /// 保存文管中心子表级联（文管文档版本；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveDocumentChildrenAsync(TaktDocument entity, TaktDocumentCreateDto dto)
    {
        // 文管文档版本（Versions）
        List<TaktDocumentVersionUpdateDto>? versionsForSave;
        if (dto is TaktDocumentUpdateDto updateDtoForVersions && updateDtoForVersions.Versions != null)
        {
            versionsForSave = updateDtoForVersions.Versions;
        }
        else if (dto.Versions != null)
        {
            versionsForSave = dto.Versions.Adapt<List<TaktDocumentVersionUpdateDto>>();
        }
        else
        {
            versionsForSave = null;
        }
        if (versionsForSave is not { Count: > 0 })
        {
            await MarkDocumentVersionsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _documentVersionRepository.GetListAsync(x => x.DocumentId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktDocumentVersion>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < versionsForSave.Count; i++)
            {
                var childDto = versionsForSave[i];
                childDto.DocumentId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("文管文档版本第{i + 1}项与本次提交的其他项重复（CompanyCode、DocumentId、LineNumber）");
                }
                if (childDto.DocumentVersionId > 0)
                {
                    if (!existingById.TryGetValue(childDto.DocumentVersionId, out var target))
                    {
                        throw new TaktBusinessException("文管文档版本不存在（DocumentVersionId={childDto.DocumentVersionId}）");
                    }
                    if (target.DocumentId != entity.Id)
                    {
                        throw new TaktBusinessException("文管文档版本不属于当前主表（DocumentVersionId={childDto.DocumentVersionId}）");
                    }
                    submittedIds.Add(childDto.DocumentVersionId);
                    childDto.Adapt(target);
                    target.Id = childDto.DocumentVersionId;
                    target.DocumentId = entity.Id;
                    target.IsObsolete = 0;
                    await _documentVersionRepository.UpdateAsync(target);
                }
                else
                {
                    var child = childDto.Adapt<TaktDocumentVersion>();
                    child.Id = 0;
                    child.DocumentId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _documentVersionRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.DocumentCode) ? entity.DocumentCode : entity.Id.ToString();
                    var maxLine = existingList.Count > 0 ? existingList.Max(x => x.LineNumber) : 0;
                    var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, needLine.Count, maxLine).ToList();
                    var lineIdx = 0;
                    foreach (var child in toCreate)
                    {
                        if (child.LineNumber <= 0)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _documentVersionRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建文管中心查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDocument, bool>> QueryExpression(TaktDocumentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDocument>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.DocumentCode != null && x.DocumentCode.Contains(keywords))
                || (x.DocumentTitle != null && x.DocumentTitle.Contains(keywords))
                || (x.DocumentContent != null && x.DocumentContent.Contains(keywords))
                || (x.DocumentSummary != null && x.DocumentSummary.Contains(keywords))
                || (x.DocumentTags != null && x.DocumentTags.Contains(keywords))
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.AccessUrl != null && x.AccessUrl.Contains(keywords))
                || (x.PublisherName != null && x.PublisherName.Contains(keywords))
                || (x.DeptName != null && x.DeptName.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentCode))
        {
            var documentCode = queryDto.DocumentCode;
            exp = exp.And(x => x.DocumentCode != null && x.DocumentCode.Contains(documentCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentTitle))
        {
            var documentTitle = queryDto.DocumentTitle;
            exp = exp.And(x => x.DocumentTitle != null && x.DocumentTitle.Contains(documentTitle));
        }

        if (queryDto?.DocumentCategory.HasValue == true)
        {
            var documentCategory = queryDto.DocumentCategory.Value;
            exp = exp.And(x => x.DocumentCategory == documentCategory);
        }

        if (queryDto?.ConfidentialLevel.HasValue == true)
        {
            var confidentialLevel = queryDto.ConfidentialLevel.Value;
            exp = exp.And(x => x.ConfidentialLevel == confidentialLevel);
        }

        if (queryDto?.Version.HasValue == true)
        {
            var version = queryDto.Version.Value;
            exp = exp.And(x => x.Version == version);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentContent))
        {
            var documentContent = queryDto.DocumentContent;
            exp = exp.And(x => x.DocumentContent != null && x.DocumentContent.Contains(documentContent));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentSummary))
        {
            var documentSummary = queryDto.DocumentSummary;
            exp = exp.And(x => x.DocumentSummary != null && x.DocumentSummary.Contains(documentSummary));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocumentTags))
        {
            var documentTags = queryDto.DocumentTags;
            exp = exp.And(x => x.DocumentTags != null && x.DocumentTags.Contains(documentTags));
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

        if (queryDto?.PublisherId.HasValue == true)
        {
            var publisherId = queryDto.PublisherId.Value;
            exp = exp.And(x => x.PublisherId == publisherId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PublisherName))
        {
            var publisherName = queryDto.PublisherName;
            exp = exp.And(x => x.PublisherName != null && x.PublisherName.Contains(publisherName));
        }

        if (queryDto?.DeptId.HasValue == true)
        {
            var deptId = queryDto.DeptId.Value;
            exp = exp.And(x => x.DeptId == deptId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DeptName))
        {
            var deptName = queryDto.DeptName;
            exp = exp.And(x => x.DeptName != null && x.DeptName.Contains(deptName));
        }

        if (queryDto?.DocumentIsTop.HasValue == true)
        {
            var documentIsTop = queryDto.DocumentIsTop.Value;
            exp = exp.And(x => x.DocumentIsTop == documentIsTop);
        }

        if (queryDto?.DocumentViewCount.HasValue == true)
        {
            var documentViewCount = queryDto.DocumentViewCount.Value;
            exp = exp.And(x => x.DocumentViewCount == documentViewCount);
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

        if (queryDto?.SortOrder.HasValue == true)
        {
            var sortOrder = queryDto.SortOrder.Value;
            exp = exp.And(x => x.SortOrder == sortOrder);
        }

        if (queryDto?.DocumentStatus.HasValue == true)
        {
            var documentStatus = queryDto.DocumentStatus.Value;
            exp = exp.And(x => x.DocumentStatus == documentStatus);
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

        if (queryDto?.DocumentEffectiveTimeStart.HasValue == true)
        {
            var documentEffectiveTimeStart = queryDto.DocumentEffectiveTimeStart.Value;
            exp = exp.And(x => x.DocumentEffectiveTime >= documentEffectiveTimeStart);
        }

        if (queryDto?.DocumentEffectiveTimeEnd.HasValue == true)
        {
            var documentEffectiveTimeEnd = queryDto.DocumentEffectiveTimeEnd.Value;
            exp = exp.And(x => x.DocumentEffectiveTime <= documentEffectiveTimeEnd);
        }

        if (queryDto?.DocumentExpireTimeStart.HasValue == true)
        {
            var documentExpireTimeStart = queryDto.DocumentExpireTimeStart.Value;
            exp = exp.And(x => x.DocumentExpireTime >= documentExpireTimeStart);
        }

        if (queryDto?.DocumentExpireTimeEnd.HasValue == true)
        {
            var documentExpireTimeEnd = queryDto.DocumentExpireTimeEnd.Value;
            exp = exp.And(x => x.DocumentExpireTime <= documentExpireTimeEnd);
        }

        if (queryDto?.DocumentPublishTimeStart.HasValue == true)
        {
            var documentPublishTimeStart = queryDto.DocumentPublishTimeStart.Value;
            exp = exp.And(x => x.DocumentPublishTime >= documentPublishTimeStart);
        }

        if (queryDto?.DocumentPublishTimeEnd.HasValue == true)
        {
            var documentPublishTimeEnd = queryDto.DocumentPublishTimeEnd.Value;
            exp = exp.And(x => x.DocumentPublishTime <= documentPublishTimeEnd);
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
    private static bool HasAnyListQueryFilter(TaktDocumentQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentTitle))
        {
            return true;
        }
        if (queryDto.DocumentCategory.HasValue)
        {
            return true;
        }
        if (queryDto.ConfidentialLevel.HasValue)
        {
            return true;
        }
        if (queryDto.Version.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentContent))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentSummary))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocumentTags))
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
        if (queryDto.PublisherId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PublisherName))
        {
            return true;
        }
        if (queryDto.DeptId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DeptName))
        {
            return true;
        }
        if (queryDto.DocumentIsTop.HasValue)
        {
            return true;
        }
        if (queryDto.DocumentViewCount.HasValue)
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
        if (queryDto.SortOrder.HasValue)
        {
            return true;
        }
        if (queryDto.DocumentStatus.HasValue)
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
        if (queryDto.DocumentEffectiveTimeStart.HasValue || queryDto.DocumentEffectiveTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.DocumentExpireTimeStart.HasValue || queryDto.DocumentExpireTimeEnd.HasValue)
        {
            return true;
        }
        if (queryDto.DocumentPublishTimeStart.HasValue || queryDto.DocumentPublishTimeEnd.HasValue)
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
