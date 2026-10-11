// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA日报应用服务
/// </summary>
public class TaktPcbaOutputService : TaktServiceBase, ITaktPcbaOutputService
{
    private readonly ITaktCompanyRepository<TaktPcbaOutput> _pcbaOutputRepository;
    private readonly ITaktCompanyRepository<TaktPcbaOutputDetail> _pcbaOutputDetailRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaOutputRepository">PCBA日报仓储</param>
    /// <param name="pcbaOutputDetailRepository">PcbaOutputDetail仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaOutputService(
        ITaktCompanyRepository<TaktPcbaOutput> pcbaOutputRepository,
        ITaktCompanyRepository<TaktPcbaOutputDetail> pcbaOutputDetailRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaOutputRepository = pcbaOutputRepository;
        _pcbaOutputDetailRepository = pcbaOutputDetailRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取PCBA日报列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPcbaOutputDto>> GetPcbaOutputListAsync(TaktPcbaOutputQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPcbaOutputDto>.Create(
                new List<TaktPcbaOutputDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _pcbaOutputRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPcbaOutputDto>.Create(
            data.Adapt<List<TaktPcbaOutputDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取PCBA日报
    /// </summary>
    /// <param name="id">PCBA日报ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDto?> GetPcbaOutputByIdAsync(long id)
    {
        var entity = await _pcbaOutputRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var dto = entity.Adapt<TaktPcbaOutputDto>();
        await FillPcbaOutputDetailsAsync(dto, entity);
        return dto;    }

    /// <summary>
    /// 获取PCBA日报选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPcbaOutputOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _pcbaOutputRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode,
            x => x.ProdOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ProdOrderCode,
            DictLabel = e.ProdOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建PCBA日报
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDto> CreatePcbaOutputAsync(TaktPcbaOutputCreateDto dto)
    {
        var entity = dto.Adapt<TaktPcbaOutput>();
        var isUnique_ix_takt_logistics_manufacturing_output_pcba_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaOutputRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProdCategory == entity.ProdCategory
                && x.ProdDate == entity.ProdDate
                && x.ProdOrderCode == entity.ProdOrderCode);
        if (!isUnique_ix_takt_logistics_manufacturing_output_pcba_unique)
        {
            throw new TaktBusinessException("PCBA日报的PlantCode、ProdCategory、ProdDate、ProdOrderCode已存在");
        }
        entity = await _pcbaOutputRepository.CreateAsync(entity);
                await SavePcbaOutputChildrenAsync(entity, dto);
        return await GetPcbaOutputByIdAsync(entity.Id) ?? entity.Adapt<TaktPcbaOutputDto>();
    }

    /// <summary>
    /// 更新PCBA日报
    /// </summary>
    /// <param name="id">PCBA日报ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDto> UpdatePcbaOutputAsync(long id, TaktPcbaOutputUpdateDto dto)
    {
        var entity = await _pcbaOutputRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA日报不存在");
        }
        dto.Adapt(entity);
        var isUnique_ix_takt_logistics_manufacturing_output_pcba_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaOutputRepository,
            x => x.PlantCode == entity.PlantCode
                && x.ProdCategory == entity.ProdCategory
                && x.ProdDate == entity.ProdDate
                && x.ProdOrderCode == entity.ProdOrderCode,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_output_pcba_unique)
        {
            throw new TaktBusinessException("PCBA日报的PlantCode、ProdCategory、ProdDate、ProdOrderCode已存在");
        }
        await _pcbaOutputRepository.UpdateAsync(entity);
                await SavePcbaOutputChildrenAsync(entity, dto);
        return await GetPcbaOutputByIdAsync(id) ?? throw new TaktBusinessException("PCBA日报不存在");
    }

    /// <summary>
    /// 删除PCBA日报
    /// </summary>
    /// <param name="id">PCBA日报ID</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaOutputByIdAsync(long id)
    {
        var entity = await _pcbaOutputRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA日报不存在或已删除");
        }
        await _pcbaOutputDetailRepository.DeleteAsync(x => x.PcbaOutputId == entity.Id);
        var deleted = await _pcbaOutputRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new TaktBusinessException("PCBA日报不存在或已删除");
        }
    }

    /// <summary>
    /// 批量删除PCBA日报
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaOutputBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePcbaOutputByIdAsync(id);
        }
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPcbaOutputTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPcbaOutputTemplateDto>(
            sheetName ?? "PCBA日报导入模板",
            fileName ?? "PCBA日报导入模板.xlsx");
    }

    /// <summary>
    /// 导入PCBA日报
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPcbaOutputAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPcbaOutputImportDto>(fileStream, sheetName ?? "PCBA日报导入模板");
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
                var entity = rows[i].Adapt<TaktPcbaOutput>();
                var importKey = $"{entity.PlantCode}|{entity.ProdCategory}|{entity.ProdDate}|{entity.ProdOrderCode}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PlantCode、ProdCategory、ProdDate、ProdOrderCode）");
                }
                var isUnique_ix_takt_logistics_manufacturing_output_pcba_unique = await _uniqueValidator.IsUniqueAsync(
                    _pcbaOutputRepository,
                    x => x.PlantCode == entity.PlantCode
                        && x.ProdCategory == entity.ProdCategory
                        && x.ProdDate == entity.ProdDate
                        && x.ProdOrderCode == entity.ProdOrderCode);
                if (!isUnique_ix_takt_logistics_manufacturing_output_pcba_unique)
                {
                    throw new TaktBusinessException("PCBA日报的PlantCode、ProdCategory、ProdDate、ProdOrderCode已存在");
                }
                await _pcbaOutputRepository.CreateAsync(entity);
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
    /// 导出PCBA日报
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPcbaOutputAsync(TaktPcbaOutputQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPcbaOutputQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaOutputExportDto>(),
                sheetName ?? "PCBA日报数据",
                fileName ?? "PCBA日报导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _pcbaOutputRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaOutputExportDto>(),
                sheetName ?? "PCBA日报数据",
                fileName ?? "PCBA日报导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPcbaOutputExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "PCBA日报数据",
            fileName ?? "PCBA日报导出.xlsx");
    }

    // ========================================
    // 主子表级联（OneToMany）
    // ========================================

    /// <summary>
    /// 将指定主表下全部未作废PCBA日报明细标记为作废（编辑清空子表）
    /// </summary>
    /// <param name="pcbaOutputId">主表主键</param>
    /// <returns>任务</returns>
    private async Task MarkPcbaOutputDetailsObsoleteAsync(long pcbaOutputId)
    {
        if (pcbaOutputId <= 0)
        {
            return;
        }
        var rows = await _pcbaOutputDetailRepository.GetListAsync(
            x => x.PcbaOutputId == pcbaOutputId && x.IsObsolete == 0);
        if (rows.Count == 0)
        {
            return;
        }
        foreach (var row in rows)
        {
            row.IsObsolete = 1;
        }
        await _pcbaOutputDetailRepository.UpdateRangeAsync(rows);
    }

    /// <summary>
    /// 填充PCBA日报详情（加载 OneToMany 子表：PCBA日报明细）
    /// </summary>
    /// <param name="dto">响应 DTO</param>
    /// <param name="entity">主表实体</param>
    /// <returns>任务</returns>
    private async Task FillPcbaOutputDetailsAsync(TaktPcbaOutputDto dto, TaktPcbaOutput entity)
    {
        if (dto == null)
        {
            return;
        }
        // PCBA日报明细 → dto.PcbaOutputDetails（含作废行）
        var pcbaoutputdetails = await _pcbaOutputDetailRepository.GetListAsync(x => x.PcbaOutputId == entity.Id);
        dto.PcbaOutputDetails = pcbaoutputdetails.Adapt<List<TaktPcbaOutputDetailDto>>();
    }

    /// <summary>
    /// 保存PCBA日报子表级联（PCBA日报明细；按子表 Id 增量新增/更新；未提交行标记作废，禁止先删后插）
    /// </summary>
    /// <param name="entity">主表实体</param>
    /// <param name="dto">创建/更新 DTO（含子表集合；UpdateDto 须继承 CreateDto）</param>
    /// <returns>任务</returns>
    private async Task SavePcbaOutputChildrenAsync(TaktPcbaOutput entity, TaktPcbaOutputCreateDto dto)
    {
        // PCBA日报明细（PcbaOutputDetails）
        List<TaktPcbaOutputDetailUpdateDto>? pcbaOutputDetailsForSave;
        if (dto is TaktPcbaOutputUpdateDto updateDtoForPcbaOutputDetails && updateDtoForPcbaOutputDetails.PcbaOutputDetails != null)
        {
            pcbaOutputDetailsForSave = updateDtoForPcbaOutputDetails.PcbaOutputDetails;
        }
        else if (dto.PcbaOutputDetails != null)
        {
            pcbaOutputDetailsForSave = dto.PcbaOutputDetails.Adapt<List<TaktPcbaOutputDetailUpdateDto>>();
        }
        else
        {
            pcbaOutputDetailsForSave = null;
        }
        if (pcbaOutputDetailsForSave is not { Count: > 0 })
        {
            await MarkPcbaOutputDetailsObsoleteAsync(entity.Id);
            return;
        }
        else
        {
            var existingList = await _pcbaOutputDetailRepository.GetListAsync(x => x.PcbaOutputId == entity.Id);
            var existingById = existingList.ToDictionary(x => x.Id);
            var submittedIds = new HashSet<long>();
            var toCreate = new List<TaktPcbaOutputDetail>();
            var seenLineKeys = new HashSet<string>(StringComparer.Ordinal);
            for (var i = 0; i < pcbaOutputDetailsForSave.Count; i++)
            {
                var childDto = pcbaOutputDetailsForSave[i];
                childDto.PcbaOutputId = entity.Id;
                childDto.TenantCode = entity.TenantCode;
                childDto.CompanyCode = entity.CompanyCode;
                childDto.CultureCode = entity.CultureCode;
                childDto.PlantCode = entity.PlantCode;
                childDto.ProdOrderCode = entity.ProdOrderCode;
                childDto.SerialCode = entity.SerialCode ?? string.Empty;
                childDto.TeamCode = (childDto.TeamCode ?? string.Empty).Trim();
                childDto.PcbBoardType = (childDto.PcbBoardType ?? string.Empty).Trim();
                childDto.PanelSide = (childDto.PanelSide ?? string.Empty).Trim();
                var lineKey = $"{entity.CompanyCode}|{entity.Id}|{childDto.TeamCode}|{childDto.ShiftNo}|{childDto.PcbBoardType}|{childDto.PanelSide}|{childDto.LineNumber}";
                if (!seenLineKeys.Add(lineKey))
                {
                    throw new TaktBusinessException($"PCBA日报明细第{i + 1}项与本次提交的其他项重复（CompanyCode、PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、PanelSide、LineNumber）");
                }
                if (childDto.PcbaOutputDetailId > 0)
                {
                    if (!existingById.TryGetValue(childDto.PcbaOutputDetailId, out var target))
                    {
                        throw new TaktBusinessException("PCBA日报明细不存在（PcbaOutputDetailId={childDto.PcbaOutputDetailId}）");
                    }
                    if (target.PcbaOutputId != entity.Id)
                    {
                        throw new TaktBusinessException("PCBA日报明细不属于当前主表（PcbaOutputDetailId={childDto.PcbaOutputDetailId}）");
                    }
                    submittedIds.Add(childDto.PcbaOutputDetailId);
                    var isUniqueUpdate_ix_takt_logistics_manufacturing_output_pcba_detail_unique = await _uniqueValidator.IsUniqueAsync(
                        _pcbaOutputDetailRepository,
                        x => x.PcbaOutputId == entity.Id
                            && x.TeamCode == childDto.TeamCode
                            && x.ShiftNo == childDto.ShiftNo
                            && x.PcbBoardType == childDto.PcbBoardType
                            && x.PanelSide == childDto.PanelSide
                            && x.LineNumber == childDto.LineNumber,
                        childDto.PcbaOutputDetailId);
                    if (!isUniqueUpdate_ix_takt_logistics_manufacturing_output_pcba_detail_unique)
                    {
                        throw new TaktBusinessException("PCBA日报明细的PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、PanelSide、LineNumber已存在");
                    }
                    childDto.Adapt(target);
                    target.Id = childDto.PcbaOutputDetailId;
                    target.PcbaOutputId = entity.Id;
                    target.IsObsolete = 0;
                    await _pcbaOutputDetailRepository.UpdateAsync(target);
                }
                else
                {
                    var isUniqueCreate_ix_takt_logistics_manufacturing_output_pcba_detail_unique = await _uniqueValidator.IsUniqueAsync(
                        _pcbaOutputDetailRepository,
                        x => x.PcbaOutputId == entity.Id
                            && x.TeamCode == childDto.TeamCode
                            && x.ShiftNo == childDto.ShiftNo
                            && x.PcbBoardType == childDto.PcbBoardType
                            && x.PanelSide == childDto.PanelSide
                            && x.LineNumber == childDto.LineNumber);
                    if (!isUniqueCreate_ix_takt_logistics_manufacturing_output_pcba_detail_unique)
                    {
                        throw new TaktBusinessException("PCBA日报明细的PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、PanelSide、LineNumber已存在");
                    }
                    var child = childDto.Adapt<TaktPcbaOutputDetail>();
                    child.Id = 0;
                    child.PcbaOutputId = entity.Id;
                    child.IsObsolete = 0;
                    toCreate.Add(child);
                }
            }
            var toObsolete = existingList.Where(x => !submittedIds.Contains(x.Id) && x.IsObsolete == 0).ToList();
            foreach (var removed in toObsolete)
            {
                removed.IsObsolete = 1;
                await _pcbaOutputDetailRepository.UpdateAsync(removed);
            }
            if (toCreate.Count > 0)
            {
                var needLine = toCreate.Where(c => c.LineNumber <= 0).ToList();
                if (needLine.Count > 0)
                {
                    var businessCode = !string.IsNullOrWhiteSpace(entity.ProdOrderCode) ? entity.ProdOrderCode : entity.Id.ToString();
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
                await _pcbaOutputDetailRepository.CreateRangeAsync(toCreate);
            }
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建PCBA日报查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPcbaOutput, bool>> QueryExpression(TaktPcbaOutputQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPcbaOutput>();

        if (!string.IsNullOrWhiteSpace(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords!.Trim();
            exp = exp.And(x =>
                (x.CultureCode != null && x.CultureCode.Contains(keywords))
                || (x.PlantCode != null && x.PlantCode.Contains(keywords))
                || (x.ProdCategory != null && x.ProdCategory.Contains(keywords))
                || (x.ProdOrderType != null && x.ProdOrderType.Contains(keywords))
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || (x.ModelCode != null && x.ModelCode.Contains(keywords))
                || (x.MaterialCode != null && x.MaterialCode.Contains(keywords))
                || (x.BatchCode != null && x.BatchCode.Contains(keywords))
                || (x.SerialCode != null && x.SerialCode.Contains(keywords))
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

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdCategory))
        {
            var prodCategory = queryDto.ProdCategory;
            exp = exp.And(x => x.ProdCategory != null && x.ProdCategory.Contains(prodCategory));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdOrderType))
        {
            var prodOrderType = queryDto.ProdOrderType;
            exp = exp.And(x => x.ProdOrderType != null && x.ProdOrderType.Contains(prodOrderType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdOrderCode))
        {
            var prodOrderCode = queryDto.ProdOrderCode;
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(prodOrderCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ModelCode))
        {
            var modelCode = queryDto.ModelCode;
            exp = exp.And(x => x.ModelCode != null && x.ModelCode.Contains(modelCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.MaterialCode))
        {
            var materialCode = queryDto.MaterialCode;
            exp = exp.And(x => x.MaterialCode != null && x.MaterialCode.Contains(materialCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.BatchCode))
        {
            var batchCode = queryDto.BatchCode;
            exp = exp.And(x => x.BatchCode != null && x.BatchCode.Contains(batchCode));
        }

        if (queryDto?.ProdOrderQty.HasValue == true)
        {
            var prodOrderQty = queryDto.ProdOrderQty.Value;
            exp = exp.And(x => x.ProdOrderQty == prodOrderQty);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SerialCode))
        {
            var serialCode = queryDto.SerialCode;
            exp = exp.And(x => x.SerialCode != null && x.SerialCode.Contains(serialCode));
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

        if (queryDto?.ProdDateStart.HasValue == true)
        {
            var prodDateStart = queryDto.ProdDateStart.Value;
            exp = exp.And(x => x.ProdDate >= prodDateStart);
        }

        if (queryDto?.ProdDateEnd.HasValue == true)
        {
            var prodDateEnd = queryDto.ProdDateEnd.Value;
            exp = exp.And(x => x.ProdDate <= prodDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktPcbaOutputQueryDto? queryDto)
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
        if (!string.IsNullOrWhiteSpace(queryDto.ProdCategory))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProdOrderType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProdOrderCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ModelCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.MaterialCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.BatchCode))
        {
            return true;
        }
        if (queryDto.ProdOrderQty.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SerialCode))
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
        if (queryDto.ProdDateStart.HasValue || queryDto.ProdDateEnd.HasValue)
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
    // 扩展方法（数据看板）
    // ========================================

    /// <summary>
    /// 获取 PCBA 生产统计（数据看板 production-stat；按生产日期）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>PCBA 生产统计</returns>
    public async Task<TaktPcbaOutputProductionStatDto> GetPcbaOutputProductionStatAsync(TaktOutputProductionStatQueryDto queryDto)
    {
        ArgumentNullException.ThrowIfNull(queryDto);
        EnsureThreeLayerContext();
        var (start, end, statMonth) = TaktStatMonthRangeHelper.ResolveMonthRange(
            queryDto.ProdDateStart,
            queryDto.ProdDateEnd);
        var tenantCode = CurrentTenantCode;
        var companyCode = CurrentCompanyCode;
        Expression<Func<TaktPcbaOutputDetail, bool>> detailPredicate = x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.IsObsolete == 0
            && SqlFunc.Subqueryable<TaktPcbaOutput>()
                .Where(h =>
                    h.Id == x.PcbaOutputId
                    && h.TenantCode == tenantCode
                    && h.CompanyCode == companyCode
                    && h.ProdDate >= start
                    && h.ProdDate <= end
                    && h.IsDeleted == 0)
                .Any();
        var details = await _pcbaOutputDetailRepository.GetListAsync(detailPredicate);
        var teams = details
            .GroupBy(d => d.TeamCode ?? string.Empty, StringComparer.Ordinal)
            .OrderBy(g => g.Key, StringComparer.Ordinal)
            .Select(g =>
            {
                var plan = g.Sum(d => d.StdLaborCapacity);
                var actual = g.Sum(d => d.DailyCompletedQty);
                return new TaktOutputProductionStatTeamItemDto
                {
                    TeamCode = g.Key,
                    StdCapacity = plan,
                    ProdActualQty = actual,
                    AchievementRate = TaktProductionStatHelper.CalculateAchievementRatePercent(actual, plan),
                };
            })
            .ToList();
        var monthStdCapacity = teams.Sum(t => t.StdCapacity);
        var monthProdActualQty = teams.Sum(t => t.ProdActualQty);
        var monthStopTime = details.Sum(d => d.StopTime);
        var monthSwitchTime = details.Sum(d => d.SwitchTime);
        var monthInputMinutes = details.Sum(d => d.InputMinutes);
        var monthProdMinutes = details.Sum(d => d.TotalMinutes);
        var monthRepairMinutes = details.Sum(d => d.RepairMinutes);
        return new TaktPcbaOutputProductionStatDto
        {
            StatMonth = statMonth,
            MonthStdCapacity = monthStdCapacity,
            MonthProdActualQty = monthProdActualQty,
            MonthAchievementRate = TaktProductionStatHelper.CalculateAchievementRatePercent(monthProdActualQty, monthStdCapacity),
            MonthDowntimeMinutes = monthStopTime + monthSwitchTime,
            MonthInputMinutes = monthInputMinutes,
            MonthProdMinutes = monthProdMinutes,
            MonthActualMinutes = monthInputMinutes + monthRepairMinutes,
            Teams = teams,
        };
    }
}
