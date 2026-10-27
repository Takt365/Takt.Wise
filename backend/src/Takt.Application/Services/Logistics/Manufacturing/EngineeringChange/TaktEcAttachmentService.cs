// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcAttachmentService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变附件应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Application.Services.Foundation;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变附件应用服务
/// </summary>
public class TaktEcAttachmentService : TaktServiceBase, ITaktEcAttachmentService
{
    private readonly ITaktCompanyRepository<TaktEcAttachment> _ecAttachmentRepository;
    private readonly ITaktCompanyRepository<TaktEcTe> _ecTeRepository;
    private readonly ITaktFileService _fileService;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecAttachmentRepository">设变附件仓储</param>
    /// <param name="ecTeRepository">设变技术课主仓储</param>
    /// <param name="fileService">文件服务（按 AccessUrl 打开物理流）</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcAttachmentService(
        ITaktCompanyRepository<TaktEcAttachment> ecAttachmentRepository,
        ITaktCompanyRepository<TaktEcTe> ecTeRepository,
        ITaktFileService fileService,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecAttachmentRepository = ecAttachmentRepository;
        _ecTeRepository = ecTeRepository;
        _fileService = fileService;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变附件列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcAttachmentDto>> GetEcAttachmentListAsync(TaktEcAttachmentQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktEcAttachmentDto>.Create(
                new List<TaktEcAttachmentDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecAttachmentRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktEcAttachmentDto>.Create(
            data.Adapt<List<TaktEcAttachmentDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变附件
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcAttachmentDto?> GetEcAttachmentByIdAsync(long id)
    {
        var entity = await _ecAttachmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktEcAttachmentDto>();
    }

    /// <summary>
    /// 预览设变附件（按 AccessUrl 打开 TaktFile 物理流）
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>可读流与文件名</returns>
    public async Task<TaktFileDownloadStreamResult> PreviewEcAttachmentAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        EnsureThreeLayerContext();
        var entity = await _ecAttachmentRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变附件不存在");
        }
        if (string.IsNullOrWhiteSpace(entity.AccessUrl) || entity.AccessUrl.Trim() == "-")
        {
            throw new TaktBusinessException("文件不存在");
        }
        return await _fileService.DownloadFileByAccessUrlAsync(entity.AccessUrl, cancellationToken);
    }

    /// <summary>
    /// 获取设变附件选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetEcAttachmentOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _ecAttachmentRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.EcCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.EcCode,
            DictLabel = e.EcCode,
        }).ToList();
    }

    /// <summary>
    /// 创建设变附件
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcAttachmentDto> CreateEcAttachmentAsync(TaktEcAttachmentCreateDto dto)
    {
        EnsureEcAttachmentDocCodeFormat(dto.AttachmentType, dto.DocCode, dto.EcCode);
        var entity = dto.Adapt<TaktEcAttachment>();
        entity.IsObsolete = 0;
        ApplyEcAttachmentFileNameFromDocCode(entity);
        await StampEcAttachmentEcTeAsync(entity, dto);
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _ecAttachmentRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcTeId == entity.EcTeId,
                x => x.LineNumber);
            var businessCode = !string.IsNullOrWhiteSpace(entity.EcCode) ? entity.EcCode : entity.EcTeId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        await EnsureEcAttachmentUniqueAsync(entity.EcTeId, entity.DocCode, entity.LineNumber, excludeId: null);
        entity = await _ecAttachmentRepository.CreateAsync(entity);
        return await GetEcAttachmentByIdAsync(entity.Id) ?? entity.Adapt<TaktEcAttachmentDto>();
    }

    /// <summary>
    /// 更新设变附件
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcAttachmentDto> UpdateEcAttachmentAsync(long id, TaktEcAttachmentUpdateDto dto)
    {
        var entity = await _ecAttachmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变附件不存在");
        }
        EnsureEcAttachmentDocCodeFormat(dto.AttachmentType, dto.DocCode, dto.EcCode);
        dto.Adapt(entity);
        ApplyEcAttachmentFileNameFromDocCode(entity);
        await StampEcAttachmentEcTeAsync(entity, dto);
        await EnsureEcAttachmentUniqueAsync(entity.EcTeId, entity.DocCode, entity.LineNumber, excludeId: id);
        await _ecAttachmentRepository.UpdateAsync(entity);
        return await GetEcAttachmentByIdAsync(id) ?? throw new TaktBusinessException("设变附件不存在");
    }

    /// <summary>
    /// 删除设变附件
    /// </summary>
    /// <param name="id">设变附件ID</param>
    /// <returns>任务</returns>
    public async Task DeleteEcAttachmentByIdAsync(long id)
    {
        var entity = await _ecAttachmentRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变附件不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变附件不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("设变附件已作废");
        }
        entity.IsObsolete = 1;
        await _ecAttachmentRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除设变附件
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteEcAttachmentBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteEcAttachmentByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变附件作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktEcAttachmentDto> UpdateEcAttachmentObsoleteAsync(TaktEcAttachmentObsoleteDto dto)
    {
        var entity = await _ecAttachmentRepository.GetByIdAsync(dto.EcAttachmentId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变附件不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变附件不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _ecAttachmentRepository.UpdateAsync(entity);
        return await GetEcAttachmentByIdAsync(dto.EcAttachmentId) ?? throw new TaktBusinessException("设变附件不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetEcAttachmentTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcAttachmentTemplateDto>(
            sheetName ?? "设变附件导入模板",
            fileName ?? "设变附件导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变附件
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportEcAttachmentAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcAttachmentImportDto>(fileStream, sheetName ?? "设变附件导入模板");
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
                var entity = rows[i].Adapt<TaktEcAttachment>();
                var importDto = rows[i].Adapt<TaktEcAttachmentCreateDto>();
                EnsureEcAttachmentDocCodeFormat(entity.AttachmentType, entity.DocCode, entity.EcCode);
                ApplyEcAttachmentFileNameFromDocCode(entity);
                await StampEcAttachmentEcTeAsync(entity, importDto);
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _ecAttachmentRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.EcTeId == entity.EcTeId,
                        x => x.LineNumber);
                    var businessCode = !string.IsNullOrWhiteSpace(entity.EcCode) ? entity.EcCode : entity.EcTeId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                var importKey = $"{entity.EcTeId}|{(entity.DocCode ?? string.Empty).Trim()}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（EcTeId、DocCode、LineNumber）");
                }
                await EnsureEcAttachmentUniqueAsync(entity.EcTeId, entity.DocCode, entity.LineNumber, excludeId: null);
                await _ecAttachmentRepository.CreateAsync(entity);
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
    /// 导出设变附件
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcAttachmentAsync(TaktEcAttachmentQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktEcAttachmentQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcAttachmentExportDto>(),
                sheetName ?? "设变附件数据",
                fileName ?? "设变附件导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _ecAttachmentRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktEcAttachmentExportDto>(),
                sheetName ?? "设变附件数据",
                fileName ?? "设变附件导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktEcAttachmentExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变附件数据",
            fileName ?? "设变附件导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 文件名称强制等于文件编码 + 原扩展名（与源文件基名无关）
    /// </summary>
    /// <param name="entity">附件实体</param>
    private static void ApplyEcAttachmentFileNameFromDocCode(TaktEcAttachment entity)
    {
        var source = string.IsNullOrWhiteSpace(entity.FileName) ? entity.AccessUrl : entity.FileName;
        entity.FileName = TaktEcAttachmentDocCodeHelper.BuildFileNameFromDocCode(
            entity.DocCode,
            source,
            entity.AccessUrl);
    }

    /// <summary>
    /// 校验文件编码格式（按 AttachmentType）
    /// </summary>
    /// <param name="attachmentType">文件类别</param>
    /// <param name="docCode">文件编码</param>
    /// <param name="ecCode">设变单号</param>
    private static void EnsureEcAttachmentDocCodeFormat(string? attachmentType, string? docCode, string? ecCode)
    {
        if (TaktEcAttachmentDocCodeHelper.IsValidDocCode(attachmentType, docCode, ecCode))
        {
            return;
        }
        var hint = TaktEcAttachmentDocCodeHelper.GetFormatHint(attachmentType);
        throw new TaktBusinessException(string.IsNullOrEmpty(hint)
            ? "文件编码格式不正确"
            : $"文件编码格式不正确（{hint}）");
    }

    /// <summary>
    /// 设变附件唯一键：EcTeId + DocCode + LineNumber（对齐 ix_takt_logistics_manufacturing_ec_attachment_unique）
    /// </summary>
    /// <param name="ecTeId">设变主表ID</param>
    /// <param name="docCode">文件编码</param>
    /// <param name="lineNumber">行号</param>
    /// <param name="excludeId">更新时排除的主键</param>
    private async Task EnsureEcAttachmentUniqueAsync(long ecTeId, string? docCode, int lineNumber, long? excludeId)
    {
        var trimmed = (docCode ?? string.Empty).Trim();
        var isUnique = await _uniqueValidator.IsUniqueAsync(
            _ecAttachmentRepository,
            x => x.EcTeId == ecTeId
                && x.DocCode == trimmed
                && x.LineNumber == lineNumber,
            excludeId);
        if (!isUnique)
        {
            throw new TaktBusinessException("设变附件的EcTeId、DocCode、LineNumber已存在");
        }
    }

    /// <summary>
    /// 同步设变附件主表外键（ManyToOne → 设变技术课主）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampEcAttachmentEcTeAsync(TaktEcAttachment entity, TaktEcAttachmentCreateDto dto)
    {
        if (dto.EcTeId <= 0)
        {
            return;
        }
        var master = await _ecTeRepository.GetByIdAsync(dto.EcTeId);
        if (master == null)
        {
            throw new TaktBusinessException("设变技术课主不存在");
        }
        entity.EcTeId = master.Id;
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
        if (string.IsNullOrEmpty(entity.EcCode))
        {
            entity.EcCode = master.EcCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变附件查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktEcAttachment, bool>> QueryExpression(TaktEcAttachmentQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcAttachment>();

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
                || (x.EcCode != null && x.EcCode.Contains(keywords))
                || (x.AttachmentType != null && x.AttachmentType.Contains(keywords))
                || (x.DocCode != null && x.DocCode.Contains(keywords))
                || (x.FileName != null && x.FileName.Contains(keywords))
                || (x.AccessUrl != null && x.AccessUrl.Contains(keywords))
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

        if (queryDto?.EcTeId.HasValue == true)
        {
            var ecTeId = queryDto.EcTeId.Value;
            exp = exp.And(x => x.EcTeId == ecTeId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.EcCode))
        {
            var ecCode = queryDto.EcCode;
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(ecCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AttachmentType))
        {
            var attachmentType = queryDto.AttachmentType;
            exp = exp.And(x => x.AttachmentType != null && x.AttachmentType.Contains(attachmentType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DocCode))
        {
            var docCode = queryDto.DocCode;
            exp = exp.And(x => x.DocCode != null && x.DocCode.Contains(docCode));
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
    private static bool HasAnyListQueryFilter(TaktEcAttachmentQueryDto? queryDto)
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
        if (queryDto.EcTeId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.EcCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AttachmentType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DocCode))
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
        if (queryDto.CreatedAtStart.HasValue || queryDto.CreatedAtEnd.HasValue)
        {
            return true;
        }
        return false;
    }
}
