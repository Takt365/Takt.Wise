// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Routine.DocumentCenter
// 文件名称：TaktDocumentVersionService.cs
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：文管文档版本应用服务实现
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
/// 文管文档版本应用服务
/// </summary>
public class TaktDocumentVersionService : TaktServiceBase, ITaktDocumentVersionService
{
    private readonly ITaktCompanyRepository<TaktDocumentVersion> _documentVersionRepository;
    private readonly ITaktApprovalRepository<TaktDocument> _documentRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="documentVersionRepository">文管文档版本仓储</param>
    /// <param name="documentRepository">文管中心仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktDocumentVersionService(
        ITaktCompanyRepository<TaktDocumentVersion> documentVersionRepository,
        ITaktApprovalRepository<TaktDocument> documentRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _documentVersionRepository = documentVersionRepository;
        _documentRepository = documentRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取文管文档版本列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktDocumentVersionDto>> GetDocumentVersionListAsync(TaktDocumentVersionQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktDocumentVersionDto>.Create(
                new List<TaktDocumentVersionDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _documentVersionRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktDocumentVersionDto>.Create(
            data.Adapt<List<TaktDocumentVersionDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取文管文档版本
    /// </summary>
    /// <param name="id">文管文档版本ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentVersionDto?> GetDocumentVersionByIdAsync(long id)
    {
        var entity = await _documentVersionRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktDocumentVersionDto>();
    }

    /// <summary>
    /// 获取文管文档版本选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetDocumentVersionOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _documentVersionRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.VersionNote ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.VersionNote ?? string.Empty,
            DictLabel = e.VersionNote ?? string.Empty,
        }).ToList();
    }

    /// <summary>
    /// 创建文管文档版本
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentVersionDto> CreateDocumentVersionAsync(TaktDocumentVersionCreateDto dto)
    {
        var entity = dto.Adapt<TaktDocumentVersion>();
        entity.IsObsolete = 0;
        await StampDocumentVersionDocumentAsync(entity, dto);
        var isUnique_ix_document_version_unique = await _uniqueValidator.IsUniqueAsync(
            _documentVersionRepository,
            x => x.DocumentId == entity.DocumentId
                && x.VersionNo == entity.VersionNo);
        if (!isUnique_ix_document_version_unique)
        {
            throw new TaktBusinessException("文管文档版本的DocumentId、VersionNo已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _documentVersionRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DocumentId == entity.DocumentId,
                x => x.LineNumber);
            var businessCode = entity.DocumentId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _documentVersionRepository.CreateAsync(entity);
        return await GetDocumentVersionByIdAsync(entity.Id) ?? entity.Adapt<TaktDocumentVersionDto>();
    }

    /// <summary>
    /// 更新文管文档版本
    /// </summary>
    /// <param name="id">文管文档版本ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentVersionDto> UpdateDocumentVersionAsync(long id, TaktDocumentVersionUpdateDto dto)
    {
        var entity = await _documentVersionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("文管文档版本不存在");
        }
        dto.Adapt(entity);
        await StampDocumentVersionDocumentAsync(entity, dto);
        var isUnique_ix_document_version_unique = await _uniqueValidator.IsUniqueAsync(
            _documentVersionRepository,
            x => x.DocumentId == entity.DocumentId
                && x.VersionNo == entity.VersionNo,
            id);
        if (!isUnique_ix_document_version_unique)
        {
            throw new TaktBusinessException("文管文档版本的DocumentId、VersionNo已存在");
        }
        await _documentVersionRepository.UpdateAsync(entity);
        return await GetDocumentVersionByIdAsync(id) ?? throw new TaktBusinessException("文管文档版本不存在");
    }

    /// <summary>
    /// 删除文管文档版本
    /// </summary>
    /// <param name="id">文管文档版本ID</param>
    /// <returns>任务</returns>
    public async Task DeleteDocumentVersionByIdAsync(long id)
    {
        var entity = await _documentVersionRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("文管文档版本不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("文管文档版本不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("文管文档版本已作废");
        }
        entity.IsObsolete = 1;
        await _documentVersionRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除文管文档版本
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteDocumentVersionBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteDocumentVersionByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新文管文档版本作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktDocumentVersionDto> UpdateDocumentVersionObsoleteAsync(TaktDocumentVersionObsoleteDto dto)
    {
        var entity = await _documentVersionRepository.GetByIdAsync(dto.DocumentVersionId);
        if (entity == null)
        {
            throw new TaktBusinessException("文管文档版本不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("文管文档版本不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _documentVersionRepository.UpdateAsync(entity);
        return await GetDocumentVersionByIdAsync(dto.DocumentVersionId) ?? throw new TaktBusinessException("文管文档版本不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetDocumentVersionTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktDocumentVersionTemplateDto>(
            sheetName ?? "文管文档版本导入模板",
            fileName ?? "文管文档版本导入模板.xlsx");
    }

    /// <summary>
    /// 导入文管文档版本
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportDocumentVersionAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktDocumentVersionImportDto>(fileStream, sheetName ?? "文管文档版本导入模板");
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
                var entity = rows[i].Adapt<TaktDocumentVersion>();
                var importDto = rows[i].Adapt<TaktDocumentVersionCreateDto>();
                await StampDocumentVersionDocumentAsync(entity, importDto);
                var importKey = $"{entity.DocumentId}|{entity.VersionNo}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（DocumentId、VersionNo）");
                }
                var isUnique_ix_document_version_unique = await _uniqueValidator.IsUniqueAsync(
                    _documentVersionRepository,
                    x => x.DocumentId == entity.DocumentId
                        && x.VersionNo == entity.VersionNo);
                if (!isUnique_ix_document_version_unique)
                {
                    throw new TaktBusinessException("文管文档版本的DocumentId、VersionNo已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _documentVersionRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.DocumentId == entity.DocumentId,
                        x => x.LineNumber);
                    var businessCode = entity.DocumentId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _documentVersionRepository.CreateAsync(entity);
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
    /// 导出文管文档版本
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDocumentVersionAsync(TaktDocumentVersionQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktDocumentVersionQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDocumentVersionExportDto>(),
                sheetName ?? "文管文档版本数据",
                fileName ?? "文管文档版本导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _documentVersionRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktDocumentVersionExportDto>(),
                sheetName ?? "文管文档版本数据",
                fileName ?? "文管文档版本导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktDocumentVersionExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "文管文档版本数据",
            fileName ?? "文管文档版本导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步文管文档版本主表外键（ManyToOne → 文管中心）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampDocumentVersionDocumentAsync(TaktDocumentVersion entity, TaktDocumentVersionCreateDto dto)
    {
        if (dto.DocumentId <= 0)
        {
            return;
        }
        var master = await _documentRepository.GetByIdAsync(dto.DocumentId);
        if (master == null)
        {
            throw new TaktBusinessException("文管中心不存在");
        }
        entity.DocumentId = master.Id;
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
    /// 构建文管文档版本查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktDocumentVersion, bool>> QueryExpression(TaktDocumentVersionQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktDocumentVersion>();

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
                || (x.VersionNote != null && x.VersionNote.Contains(keywords))
                || (x.RevisedByName != null && x.RevisedByName.Contains(keywords))
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

        if (queryDto?.DocumentId.HasValue == true)
        {
            var documentId = queryDto.DocumentId.Value;
            exp = exp.And(x => x.DocumentId == documentId);
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (queryDto?.VersionNo.HasValue == true)
        {
            var versionNo = queryDto.VersionNo.Value;
            exp = exp.And(x => x.VersionNo == versionNo);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.VersionNote))
        {
            var versionNote = queryDto.VersionNote;
            exp = exp.And(x => x.VersionNote != null && x.VersionNote.Contains(versionNote));
        }

        if (queryDto?.FileId.HasValue == true)
        {
            var fileId = queryDto.FileId.Value;
            exp = exp.And(x => x.FileId == fileId);
        }

        if (queryDto?.RevisedBy.HasValue == true)
        {
            var revisedBy = queryDto.RevisedBy.Value;
            exp = exp.And(x => x.RevisedBy == revisedBy);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RevisedByName))
        {
            var revisedByName = queryDto.RevisedByName;
            exp = exp.And(x => x.RevisedByName != null && x.RevisedByName.Contains(revisedByName));
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

        if (queryDto?.RevisedAtStart.HasValue == true)
        {
            var revisedAtStart = queryDto.RevisedAtStart.Value;
            exp = exp.And(x => x.RevisedAt >= revisedAtStart);
        }

        if (queryDto?.RevisedAtEnd.HasValue == true)
        {
            var revisedAtEnd = queryDto.RevisedAtEnd.Value;
            exp = exp.And(x => x.RevisedAt <= revisedAtEnd);
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
    private static bool HasAnyListQueryFilter(TaktDocumentVersionQueryDto? queryDto)
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
        if (queryDto.DocumentId.HasValue)
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (queryDto.VersionNo.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VersionNote))
        {
            return true;
        }
        if (queryDto.FileId.HasValue)
        {
            return true;
        }
        if (queryDto.RevisedBy.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RevisedByName))
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
        if (queryDto.RevisedAtStart.HasValue || queryDto.RevisedAtEnd.HasValue)
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
