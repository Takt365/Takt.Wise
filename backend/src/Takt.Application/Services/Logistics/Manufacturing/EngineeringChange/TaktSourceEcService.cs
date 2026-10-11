// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktSourceEcService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变来源主应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Entities.Logistics.Manufacturing.EngineeringChange;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变来源主应用服务
/// </summary>
public class TaktSourceEcService : TaktServiceBase, ITaktSourceEcService
{
    private readonly ITaktCompanyRepository<TaktSourceEc> _sourceEcRepository;
    private readonly ITaktCompanyRepository<TaktSourceEcDetail> _sourceEcDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sourceEcRepository">设变来源主仓储</param>
    /// <param name="sourceEcDetailRepository">SourceEcDetail仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktSourceEcService(
        ITaktCompanyRepository<TaktSourceEc> sourceEcRepository,
        ITaktCompanyRepository<TaktSourceEcDetail> sourceEcDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _sourceEcRepository = sourceEcRepository;
        _sourceEcDetailRepository = sourceEcDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取设变来源主列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktSourceEcDto>> GetSourceEcListAsync(TaktSourceEcQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktSourceEcDto>.Create(
                new List<TaktSourceEcDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _sourceEcRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktSourceEcDto>.Create(
            data.Adapt<List<TaktSourceEcDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取设变来源主
    /// </summary>
    /// <param name="id">设变来源主ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceEcDto?> GetSourceEcByIdAsync(long id)
    {
        var entity = await _sourceEcRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktSourceEcDto>();
        await FillSourceEcDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取设变来源主选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetSourceEcOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _sourceEcRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.SourceEcCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.SourceEcCode,
            DictLabel = e.SourceEcCode,
        }).ToList();
    }

    /// <summary>
    /// 创建设变来源主
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceEcDto> CreateSourceEcAsync(TaktSourceEcCreateDto dto)
    {
        var entity = dto.Adapt<TaktSourceEc>();
        var isUnique_ix_source_ec_code_unique = await _uniqueValidator.IsUniqueAsync(
            _sourceEcRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SourceEcCode == entity.SourceEcCode);
        if (!isUnique_ix_source_ec_code_unique)
        {
            throw new TaktBusinessException("设变来源主的PlantCode、SourceEcCode已存在");
        }
        entity = await _sourceEcRepository.CreateAsync(entity);
                await SaveSourceEcChildrenAsync(entity, dto);
        return await GetSourceEcByIdAsync(entity.Id) ?? entity.Adapt<TaktSourceEcDto>();
    }

    /// <summary>
    /// 更新设变来源主
    /// </summary>
    /// <param name="id">设变来源主ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceEcDto> UpdateSourceEcAsync(long id, TaktSourceEcUpdateDto dto)
    {
        var entity = await _sourceEcRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变来源主不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_source_ec_code_unique = await _uniqueValidator.IsUniqueAsync(
            _sourceEcRepository,
            x => x.PlantCode == entity.PlantCode
                && x.SourceEcCode == entity.SourceEcCode,
            id);
        if (!isUnique_ix_source_ec_code_unique)
        {
            throw new TaktBusinessException("设变来源主的PlantCode、SourceEcCode已存在");
        }
        await _sourceEcRepository.UpdateAsync(entity);
                await SaveSourceEcChildrenAsync(entity, dto);
        return await GetSourceEcByIdAsync(id) ?? throw new TaktBusinessException("设变来源主不存在");
    }

    /// <summary>
    /// 删除设变来源主
    /// </summary>
    /// <param name="id">设变来源主ID</param>
    /// <returns>任务</returns>
    public async Task DeleteSourceEcByIdAsync(long id)
    {
        var entity = await _sourceEcRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("设变来源主不存在或已删除");
        }
        await _sourceEcDetailRepository.DeleteAsync(x => x.SourceEcId == entity.Id);
        var deleted = await _sourceEcRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("设变来源主不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除设变来源主
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeleteSourceEcBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeleteSourceEcByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新设变来源主状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktSourceEcDto> UpdateSourceEcStatusAsync(TaktSourceEcStatusDto dto)
    {
        var entity = await _sourceEcRepository.GetByIdAsync(dto.SourceEcId);
        if (entity == null)
        {
            throw new TaktBusinessException("设变来源主不存在");
        }
        entity.SourceStatus = dto.SourceStatus;
        await _sourceEcRepository.UpdateAsync(entity);
        return await GetSourceEcByIdAsync(dto.SourceEcId) ?? throw new TaktBusinessException("设变来源主不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetSourceEcTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktSourceEcTemplateDto>(
            sheetName ?? "设变来源主导入模板",
            fileName ?? "设变来源主导入模板.xlsx");
    }

    /// <summary>
    /// 导入设变来源主
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportSourceEcAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktSourceEcImportDto>(fileStream, sheetName ?? "设变来源主导入模板");
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
                var entity = rows[i].Adapt<TaktSourceEc>();
                var importKey = $"{entity.PlantCode}|{entity.SourceEcCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、SourceEcCode）");
                }
                var isUnique_ix_source_ec_code_unique = await _uniqueValidator.IsUniqueAsync(
                    _sourceEcRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.SourceEcCode == entity.SourceEcCode);
                if (!isUnique_ix_source_ec_code_unique)
                {
                    throw new TaktBusinessException("设变来源主的PlantCode、SourceEcCode已存在");
                }
                await _sourceEcRepository.CreateAsync(entity);
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
    /// 导出设变来源主
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportSourceEcAsync(TaktSourceEcQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktSourceEcQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSourceEcExportDto>(),
                sheetName ?? "设变来源主数据",
                fileName ?? "设变来源主导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _sourceEcRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktSourceEcExportDto>(),
                sheetName ?? "设变来源主数据",
                fileName ?? "设变来源主导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktSourceEcExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "设变来源主数据",
            fileName ?? "设变来源主导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废设变来源子标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="sourceEcId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkSourceEcDetailsObsoleteAsync(long sourceEcId)
    {
        if (sourceEcId <= 0)
        {
            return;
        }
        var rows = await _sourceEcDetailRepository.GetListAsync(
            x => x.SourceEcId == sourceEcId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _sourceEcDetailRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充设变来源主详情（加载 OneToMany 子表：设变来源子）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillSourceEcDetailsAsync(TaktSourceEcDto dto, TaktSourceEc entity)
    {
        if (dto == null)
        {
            return;
        }
        // 设变来源子 → dto.SourceEcDetails（含作废行）
        var sourceecdetails = await _sourceEcDetailRepository.GetListAsync(x => x.SourceEcId == entity.Id);
        dto.SourceEcDetails = sourceecdetails.Adapt<List<TaktSourceEcDetailDto>>();
    }

    /// <summary>
    /// 保存设变来源主子表级联（设变来源子；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SaveSourceEcChildrenAsync(TaktSourceEc entity, TaktSourceEcCreateDto dto)
    {
        // 设变来源子（SourceEcDetails）
        List<TaktSourceEcDetailUpdateDto>? sourceEcDetailsForSave;
        if (dto is TaktSourceEcUpdateDto updateDtoForSourceEcDetails && updateDtoForSourceEcDetails.SourceEcDetails != null)
        {
            sourceEcDetailsForSave = updateDtoForSourceEcDetails.SourceEcDetails;
        }
        else if (dto.SourceEcDetails != null)
        {
            sourceEcDetailsForSave = dto.SourceEcDetails.Adapt<List<TaktSourceEcDetailUpdateDto>>();
        }
        else
        {
            sourceEcDetailsForSave = null;
        }
        if (sourceEcDetailsForSave is not { Count: > 0 })
        {
            await MarkSourceEcDetailsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _sourceEcDetailRepository.GetListAsync(x => x.SourceEcId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktSourceEcDetail>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < sourceEcDetailsForSave.Count; i++)
            {
                var childDto = sourceEcDetailsForSave[i];
                childDto.SourceEcId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.SourceEcCode = entity.SourceEcCode;
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.SourceRootMaterialCode}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException("设变来源子第{i + 1}项与本次提交的其他项重复（CompanyCode、SourceEcId、SourceRootMaterialCode、LineNumber）");
                }
                if (childDto.SourceEcDetailId > 0)
                {
                    if (!existingById.TryGetValue(childDto.SourceEcDetailId, out var target))
                    {
                        throw new TaktBusinessException("设变来源子不存在（SourceEcDetailId={childDto.SourceEcDetailId}）");
                    }
                    if (target.SourceEcId != entity.Id)
                    {
                        throw new TaktBusinessException("设变来源子不属于当前主表（SourceEcDetailId={childDto.SourceEcDetailId}）");
                    }
                    submittedIds.Add(childDto.SourceEcDetailId);
                    var updateSourceEcId = entity.Id;
                    var updateRootMaterial = childDto.SourceRootMaterialCode ?? string.Empty;
                    var updateLineNumber = childDto.LineNumber;
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_ec_source_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _sourceEcDetailRepository,
                        x => x.SourceEcId == updateSourceEcId
                            && x.SourceRootMaterialCode == updateRootMaterial
                            && x.LineNumber == updateLineNumber,
                        childDto.SourceEcDetailId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_ec_source_detail_line_unique)
                    {
                        throw new TaktBusinessException("设变来源子的SourceEcId、SourceRootMaterialCode、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.SourceEcDetailId;
                    target.SourceEcId = entity.Id;
                    target.IsObsolete = 0;
                    await _sourceEcDetailRepository.UpdateAsync(target);
                }
                else
                {
                    var createSourceEcId = entity.Id;
                    var createRootMaterial = childDto.SourceRootMaterialCode ?? string.Empty;
                    var createLineNumber = childDto.LineNumber;
                    var isUniqueCreate_ix_takt_logistics_manufacturing_ec_source_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                        _sourceEcDetailRepository,
                        x => x.SourceEcId == createSourceEcId
                            && x.SourceRootMaterialCode == createRootMaterial
                            && x.LineNumber == createLineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_ec_source_detail_line_unique)
                    {
                        throw new TaktBusinessException("设变来源子的SourceEcId、SourceRootMaterialCode、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktSourceEcDetail>();
                    child.Id = 0;
                    child.SourceEcId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _sourceEcDetailRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.SourceEcCode) ? entity.SourceEcCode : entity.Id.ToString();
                    foreach (var grp in needLine.GroupBy(c => c.SourceRootMaterialCode ?? string.Empty, StringComparer.Ordinal))
                    {
                        var fg = grp.Key;
                        var maxLine = existingList
                            .Where(x => string.Equals(x.SourceRootMaterialCode ?? string.Empty, fg, StringComparison.Ordinal))
                            .Select(x => x.LineNumber)
                            .DefaultIfEmpty(0)
                            .Max();
                        var assignedInCreate = toCreate
                            .Where(c => c.LineNumber > 0
                                && string.Equals(c.SourceRootMaterialCode ?? string.Empty, fg, StringComparison.Ordinal))
                            .Select(c => c.LineNumber)
                            .DefaultIfEmpty(0)
                            .Max();
                        maxLine = Math.Max(maxLine, assignedInCreate);
                        var lineSeq = _lineNumberGenerator.GenerateSequence(businessCode, grp.Count(), maxLine).ToList();
                        var lineIdx = 0;
                        foreach (var child in grp)
                        {
                            child.LineNumber = lineSeq[lineIdx++];
                        }
                    }
                }
                await _sourceEcDetailRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建设变来源主查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktSourceEc, bool>> QueryExpression(TaktSourceEcQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktSourceEc>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.SourceEcCode != null && x.SourceEcCode.Contains(keywords))
                || (x.SourceModel != null && x.SourceModel.Contains(keywords))
                || (x.SourceTitle != null && x.SourceTitle.Contains(keywords))
                || (x.SourceStatus != null && x.SourceStatus.Contains(keywords))
                || (x.SourceTcjOwner != null && x.SourceTcjOwner.Contains(keywords))
                || (x.SourceTcjDependency != null && x.SourceTcjDependency.Contains(keywords))
                || (x.SourceEcMeeting != null && x.SourceEcMeeting.Contains(keywords))
                || (x.SourcePpCode != null && x.SourcePpCode.Contains(keywords))
                || (x.SourceTechnicalNoticeCode != null && x.SourceTechnicalNoticeCode.Contains(keywords))
                || (x.SourceImplementation != null && x.SourceImplementation.Contains(keywords))
                || (x.SourceMainChangeReason != null && x.SourceMainChangeReason.Contains(keywords))
                || (x.SourceSecondaryChangeReason != null && x.SourceSecondaryChangeReason.Contains(keywords))
                || (x.SourceSafetyRegulation != null && x.SourceSafetyRegulation.Contains(keywords))
                || (x.SourceProgressStatus != null && x.SourceProgressStatus.Contains(keywords))
                || (x.SourceSerialNumberControl != null && x.SourceSerialNumberControl.Contains(keywords))
                || (x.SourceCustomerApproval != null && x.SourceCustomerApproval.Contains(keywords))
                || (x.SourceServiceManualRevision != null && x.SourceServiceManualRevision.Contains(keywords))
                || (x.SourceUserManualRevision != null && x.SourceUserManualRevision.Contains(keywords))
                || (x.SourcePromotionManualRevision != null && x.SourcePromotionManualRevision.Contains(keywords))
                || (x.SourceStandardDocumentRevision != null && x.SourceStandardDocumentRevision.Contains(keywords))
                || (x.SourceInformationRelease != null && x.SourceInformationRelease.Contains(keywords))
                || (x.SourceCostChange != null && x.SourceCostChange.Contains(keywords))
                || (x.SourceRelatedDrawing != null && x.SourceRelatedDrawing.Contains(keywords))
                || (x.SourceEcContent != null && x.SourceEcContent.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceEcCode))
        {
            var sourceEcCode = queryDto.SourceEcCode;
            exp = exp.And(x => x.SourceEcCode != null && x.SourceEcCode.Contains(sourceEcCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceModel))
        {
            var sourceModel = queryDto.SourceModel;
            exp = exp.And(x => x.SourceModel != null && x.SourceModel.Contains(sourceModel));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceTitle))
        {
            var sourceTitle = queryDto.SourceTitle;
            exp = exp.And(x => x.SourceTitle != null && x.SourceTitle.Contains(sourceTitle));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceStatus))
        {
            var sourceStatus = queryDto.SourceStatus;
            exp = exp.And(x => x.SourceStatus != null && x.SourceStatus.Contains(sourceStatus));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceTcjOwner))
        {
            var sourceTcjOwner = queryDto.SourceTcjOwner;
            exp = exp.And(x => x.SourceTcjOwner != null && x.SourceTcjOwner.Contains(sourceTcjOwner));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceTcjDependency))
        {
            var sourceTcjDependency = queryDto.SourceTcjDependency;
            exp = exp.And(x => x.SourceTcjDependency != null && x.SourceTcjDependency.Contains(sourceTcjDependency));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceEcMeeting))
        {
            var sourceEcMeeting = queryDto.SourceEcMeeting;
            exp = exp.And(x => x.SourceEcMeeting != null && x.SourceEcMeeting.Contains(sourceEcMeeting));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourcePpCode))
        {
            var sourcePpCode = queryDto.SourcePpCode;
            exp = exp.And(x => x.SourcePpCode != null && x.SourcePpCode.Contains(sourcePpCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceTechnicalNoticeCode))
        {
            var sourceTechnicalNoticeCode = queryDto.SourceTechnicalNoticeCode;
            exp = exp.And(x => x.SourceTechnicalNoticeCode != null && x.SourceTechnicalNoticeCode.Contains(sourceTechnicalNoticeCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceImplementation))
        {
            var sourceImplementation = queryDto.SourceImplementation;
            exp = exp.And(x => x.SourceImplementation != null && x.SourceImplementation.Contains(sourceImplementation));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceMainChangeReason))
        {
            var sourceMainChangeReason = queryDto.SourceMainChangeReason;
            exp = exp.And(x => x.SourceMainChangeReason != null && x.SourceMainChangeReason.Contains(sourceMainChangeReason));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceSecondaryChangeReason))
        {
            var sourceSecondaryChangeReason = queryDto.SourceSecondaryChangeReason;
            exp = exp.And(x => x.SourceSecondaryChangeReason != null && x.SourceSecondaryChangeReason.Contains(sourceSecondaryChangeReason));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceSafetyRegulation))
        {
            var sourceSafetyRegulation = queryDto.SourceSafetyRegulation;
            exp = exp.And(x => x.SourceSafetyRegulation != null && x.SourceSafetyRegulation.Contains(sourceSafetyRegulation));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceProgressStatus))
        {
            var sourceProgressStatus = queryDto.SourceProgressStatus;
            exp = exp.And(x => x.SourceProgressStatus != null && x.SourceProgressStatus.Contains(sourceProgressStatus));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceSerialNumberControl))
        {
            var sourceSerialNumberControl = queryDto.SourceSerialNumberControl;
            exp = exp.And(x => x.SourceSerialNumberControl != null && x.SourceSerialNumberControl.Contains(sourceSerialNumberControl));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceCustomerApproval))
        {
            var sourceCustomerApproval = queryDto.SourceCustomerApproval;
            exp = exp.And(x => x.SourceCustomerApproval != null && x.SourceCustomerApproval.Contains(sourceCustomerApproval));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceServiceManualRevision))
        {
            var sourceServiceManualRevision = queryDto.SourceServiceManualRevision;
            exp = exp.And(x => x.SourceServiceManualRevision != null && x.SourceServiceManualRevision.Contains(sourceServiceManualRevision));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceUserManualRevision))
        {
            var sourceUserManualRevision = queryDto.SourceUserManualRevision;
            exp = exp.And(x => x.SourceUserManualRevision != null && x.SourceUserManualRevision.Contains(sourceUserManualRevision));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourcePromotionManualRevision))
        {
            var sourcePromotionManualRevision = queryDto.SourcePromotionManualRevision;
            exp = exp.And(x => x.SourcePromotionManualRevision != null && x.SourcePromotionManualRevision.Contains(sourcePromotionManualRevision));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceStandardDocumentRevision))
        {
            var sourceStandardDocumentRevision = queryDto.SourceStandardDocumentRevision;
            exp = exp.And(x => x.SourceStandardDocumentRevision != null && x.SourceStandardDocumentRevision.Contains(sourceStandardDocumentRevision));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceInformationRelease))
        {
            var sourceInformationRelease = queryDto.SourceInformationRelease;
            exp = exp.And(x => x.SourceInformationRelease != null && x.SourceInformationRelease.Contains(sourceInformationRelease));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceCostChange))
        {
            var sourceCostChange = queryDto.SourceCostChange;
            exp = exp.And(x => x.SourceCostChange != null && x.SourceCostChange.Contains(sourceCostChange));
        }

        if (queryDto?.SourceUnitCost.HasValue == true)
        {
            var sourceUnitCost = queryDto.SourceUnitCost.Value;
            exp = exp.And(x => x.SourceUnitCost == sourceUnitCost);
        }

        if (queryDto?.SourceMoldModificationCost.HasValue == true)
        {
            var sourceMoldModificationCost = queryDto.SourceMoldModificationCost.Value;
            exp = exp.And(x => x.SourceMoldModificationCost == sourceMoldModificationCost);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceRelatedDrawing))
        {
            var sourceRelatedDrawing = queryDto.SourceRelatedDrawing;
            exp = exp.And(x => x.SourceRelatedDrawing != null && x.SourceRelatedDrawing.Contains(sourceRelatedDrawing));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SourceEcContent))
        {
            var sourceEcContent = queryDto.SourceEcContent;
            exp = exp.And(x => x.SourceEcContent != null && x.SourceEcContent.Contains(sourceEcContent));
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

        if (queryDto?.SourceIssueDateStart.HasValue == true)
        {
            var sourceIssueDateStart = queryDto.SourceIssueDateStart.Value;
            exp = exp.And(x => x.SourceIssueDate >= sourceIssueDateStart);
        }

        if (queryDto?.SourceIssueDateEnd.HasValue == true)
        {
            var sourceIssueDateEnd = queryDto.SourceIssueDateEnd.Value;
            exp = exp.And(x => x.SourceIssueDate <= sourceIssueDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktSourceEcQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.SourceEcCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceModel))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceTitle))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceTcjOwner))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceTcjDependency))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceEcMeeting))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourcePpCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceTechnicalNoticeCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceImplementation))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceMainChangeReason))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceSecondaryChangeReason))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceSafetyRegulation))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceProgressStatus))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceSerialNumberControl))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceCustomerApproval))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceServiceManualRevision))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceUserManualRevision))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourcePromotionManualRevision))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceStandardDocumentRevision))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceInformationRelease))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceCostChange))
        {
            return true;
        }
        if (queryDto.SourceUnitCost.HasValue)
        {
            return true;
        }
        if (queryDto.SourceMoldModificationCost.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceRelatedDrawing))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SourceEcContent))
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
        if (queryDto.SourceIssueDateStart.HasValue || queryDto.SourceIssueDateEnd.HasValue)
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
