// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaInspectionDetailService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA检查明细应用服务实现
// 
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Linq.Expressions;
using Mapster;
using SqlSugar;
using Takt.Application.Dtos.Logistics.Manufacturing.Defect;
using Takt.Domain.Entities.Logistics.Manufacturing.Defect;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA检查明细应用服务
/// </summary>
public class TaktPcbaInspectionDetailService : TaktServiceBase, ITaktPcbaInspectionDetailService
{
    private readonly ITaktCompanyRepository<TaktPcbaInspectionDetail> _pcbaInspectionDetailRepository;
    private readonly ITaktCompanyRepository<TaktPcbaInspection> _pcbaInspectionRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaInspectionDetailRepository">PCBA检查明细仓储</param>
    /// <param name="pcbaInspectionRepository">PCBA检查日报仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaInspectionDetailService(
        ITaktCompanyRepository<TaktPcbaInspectionDetail> pcbaInspectionDetailRepository,
        ITaktCompanyRepository<TaktPcbaInspection> pcbaInspectionRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaInspectionDetailRepository = pcbaInspectionDetailRepository;
        _pcbaInspectionRepository = pcbaInspectionRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取PCBA检查明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPcbaInspectionDetailDto>> GetPcbaInspectionDetailListAsync(TaktPcbaInspectionDetailQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPcbaInspectionDetailDto>.Create(
                new List<TaktPcbaInspectionDetailDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _pcbaInspectionDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPcbaInspectionDetailDto>.Create(
            data.Adapt<List<TaktPcbaInspectionDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取PCBA检查明细
    /// </summary>
    /// <param name="id">PCBA检查明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDetailDto?> GetPcbaInspectionDetailByIdAsync(long id)
    {
        var entity = await _pcbaInspectionDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPcbaInspectionDetailDto>();
    }

    /// <summary>
    /// 获取PCBA检查明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPcbaInspectionDetailOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _pcbaInspectionDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.InspectionStatus == 1 && x.IsObsolete == 0,
            x => x.InspectorName ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ProdOrderCode,
            DictLabel = e.InspectorName ?? e.ProdOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建PCBA检查明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDetailDto> CreatePcbaInspectionDetailAsync(TaktPcbaInspectionDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktPcbaInspectionDetail>();
        entity.IsObsolete = 0;
        await StampPcbaInspectionDetailPcbaInspectionAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaInspectionDetailRepository,
            x => x.PcbaInspectionId == entity.PcbaInspectionId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique)
        {
            throw new TaktBusinessException("PCBA检查明细的PcbaInspectionId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _pcbaInspectionDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PcbaInspectionId == entity.PcbaInspectionId,
                x => x.LineNumber);
            var businessCode = entity.PcbaInspectionId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _pcbaInspectionDetailRepository.CreateAsync(entity);
        return await GetPcbaInspectionDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktPcbaInspectionDetailDto>();
    }

    /// <summary>
    /// 更新PCBA检查明细
    /// </summary>
    /// <param name="id">PCBA检查明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDetailDto> UpdatePcbaInspectionDetailAsync(long id, TaktPcbaInspectionDetailUpdateDto dto)
    {
        var entity = await _pcbaInspectionDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA检查明细不存在");
        }
        dto.Adapt(entity);
        await StampPcbaInspectionDetailPcbaInspectionAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaInspectionDetailRepository,
            x => x.PcbaInspectionId == entity.PcbaInspectionId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique)
        {
            throw new TaktBusinessException("PCBA检查明细的PcbaInspectionId、LineNumber已存在");
        }
        await _pcbaInspectionDetailRepository.UpdateAsync(entity);
        return await GetPcbaInspectionDetailByIdAsync(id) ?? throw new TaktBusinessException("PCBA检查明细不存在");
    }

    /// <summary>
    /// 删除PCBA检查明细
    /// </summary>
    /// <param name="id">PCBA检查明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaInspectionDetailByIdAsync(long id)
    {
        var entity = await _pcbaInspectionDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA检查明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("PCBA检查明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("PCBA检查明细已作废");
        }
        entity.IsObsolete = 1;
        await _pcbaInspectionDetailRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除PCBA检查明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaInspectionDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePcbaInspectionDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新PCBA检查明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDetailDto> UpdatePcbaInspectionDetailStatusAsync(TaktPcbaInspectionDetailStatusDto dto)
    {
        var entity = await _pcbaInspectionDetailRepository.GetByIdAsync(dto.PcbaInspectionDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA检查明细不存在");
        }
        entity.InspectionStatus = dto.InspectionStatus;
        await _pcbaInspectionDetailRepository.UpdateAsync(entity);
        return await GetPcbaInspectionDetailByIdAsync(dto.PcbaInspectionDetailId) ?? throw new TaktBusinessException("PCBA检查明细不存在");
    }

    /// <summary>
    /// 更新PCBA检查明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaInspectionDetailDto> UpdatePcbaInspectionDetailObsoleteAsync(TaktPcbaInspectionDetailObsoleteDto dto)
    {
        var entity = await _pcbaInspectionDetailRepository.GetByIdAsync(dto.PcbaInspectionDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA检查明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("PCBA检查明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _pcbaInspectionDetailRepository.UpdateAsync(entity);
        return await GetPcbaInspectionDetailByIdAsync(dto.PcbaInspectionDetailId) ?? throw new TaktBusinessException("PCBA检查明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPcbaInspectionDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPcbaInspectionDetailTemplateDto>(
            sheetName ?? "PCBA检查明细导入模板",
            fileName ?? "PCBA检查明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入PCBA检查明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPcbaInspectionDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPcbaInspectionDetailImportDto>(fileStream, sheetName ?? "PCBA检查明细导入模板");
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
                var entity = rows[i].Adapt<TaktPcbaInspectionDetail>();
                var importDto = rows[i].Adapt<TaktPcbaInspectionDetailCreateDto>();
                await StampPcbaInspectionDetailPcbaInspectionAsync(entity, importDto);
                var importKey = $"{entity.PcbaInspectionId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PcbaInspectionId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _pcbaInspectionDetailRepository,
                    x => x.PcbaInspectionId == entity.PcbaInspectionId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_inspection_detail_line_unique)
                {
                    throw new TaktBusinessException("PCBA检查明细的PcbaInspectionId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _pcbaInspectionDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PcbaInspectionId == entity.PcbaInspectionId,
                        x => x.LineNumber);
                    var businessCode = entity.PcbaInspectionId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _pcbaInspectionDetailRepository.CreateAsync(entity);
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
    /// 导出PCBA检查明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPcbaInspectionDetailAsync(TaktPcbaInspectionDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPcbaInspectionDetailQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaInspectionDetailExportDto>(),
                sheetName ?? "PCBA检查明细数据",
                fileName ?? "PCBA检查明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _pcbaInspectionDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaInspectionDetailExportDto>(),
                sheetName ?? "PCBA检查明细数据",
                fileName ?? "PCBA检查明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPcbaInspectionDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "PCBA检查明细数据",
            fileName ?? "PCBA检查明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步PCBA检查明细主表外键（ManyToOne → PCBA检查日报）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampPcbaInspectionDetailPcbaInspectionAsync(TaktPcbaInspectionDetail entity, TaktPcbaInspectionDetailCreateDto dto)
    {
        if (dto.PcbaInspectionId <= 0)
        {
            return;
        }
        var master = await _pcbaInspectionRepository.GetByIdAsync(dto.PcbaInspectionId);
        if (master == null)
        {
            throw new TaktBusinessException("PCBA检查日报不存在");
        }
        entity.PcbaInspectionId = master.Id;
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
        if (string.IsNullOrEmpty(entity.ProdOrderCode))
        {
            entity.ProdOrderCode = master.ProdOrderCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建PCBA检查明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPcbaInspectionDetail, bool>> QueryExpression(TaktPcbaInspectionDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPcbaInspectionDetail>();

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
                || (x.ProdOrderCode != null && x.ProdOrderCode.Contains(keywords))
                || (x.PcbaBoardType != null && x.PcbaBoardType.Contains(keywords))
                || (x.VisualInspectionLine != null && x.VisualInspectionLine.Contains(keywords))
                || (x.AoiLine != null && x.AoiLine.Contains(keywords))
                || (x.InspectorName != null && x.InspectorName.Contains(keywords))
                || (x.TeamCode != null && x.TeamCode.Contains(keywords))
                || (x.HandPlacement != null && x.HandPlacement.Contains(keywords))
                || (x.SerialNumber != null && x.SerialNumber.Contains(keywords))
                || (x.Content != null && x.Content.Contains(keywords))
                || (x.DefectLocation != null && x.DefectLocation.Contains(keywords))
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

        if (queryDto?.PcbaInspectionId.HasValue == true)
        {
            var pcbaInspectionId = queryDto.PcbaInspectionId.Value;
            exp = exp.And(x => x.PcbaInspectionId == pcbaInspectionId);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdOrderCode))
        {
            var prodOrderCode = queryDto.ProdOrderCode;
            exp = exp.And(x => x.ProdOrderCode != null && x.ProdOrderCode.Contains(prodOrderCode));
        }

        if (queryDto?.LineNumber.HasValue == true)
        {
            var lineNumber = queryDto.LineNumber.Value;
            exp = exp.And(x => x.LineNumber == lineNumber);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PcbaBoardType))
        {
            var pcbaBoardType = queryDto.PcbaBoardType;
            exp = exp.And(x => x.PcbaBoardType != null && x.PcbaBoardType.Contains(pcbaBoardType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.VisualInspectionLine))
        {
            var visualInspectionLine = queryDto.VisualInspectionLine;
            exp = exp.And(x => x.VisualInspectionLine != null && x.VisualInspectionLine.Contains(visualInspectionLine));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.AoiLine))
        {
            var aoiLine = queryDto.AoiLine;
            exp = exp.And(x => x.AoiLine != null && x.AoiLine.Contains(aoiLine));
        }

        if (queryDto?.ShiftNo.HasValue == true)
        {
            var shiftNo = queryDto.ShiftNo.Value;
            exp = exp.And(x => x.ShiftNo == shiftNo);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.InspectorName))
        {
            var inspectorName = queryDto.InspectorName;
            exp = exp.And(x => x.InspectorName != null && x.InspectorName.Contains(inspectorName));
        }

        if (queryDto?.DailyCompletedQty.HasValue == true)
        {
            var dailyCompletedQty = queryDto.DailyCompletedQty.Value;
            exp = exp.And(x => x.DailyCompletedQty == dailyCompletedQty);
        }

        if (queryDto?.InspectionQty.HasValue == true)
        {
            var inspectionQty = queryDto.InspectionQty.Value;
            exp = exp.And(x => x.InspectionQty == inspectionQty);
        }

        if (queryDto?.InspectionStatus.HasValue == true)
        {
            var inspectionStatus = queryDto.InspectionStatus.Value;
            exp = exp.And(x => x.InspectionStatus == inspectionStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TeamCode))
        {
            var teamCode = queryDto.TeamCode;
            exp = exp.And(x => x.TeamCode != null && x.TeamCode.Contains(teamCode));
        }

        if (queryDto?.InspectionWorkHours.HasValue == true)
        {
            var inspectionWorkHours = queryDto.InspectionWorkHours.Value;
            exp = exp.And(x => x.InspectionWorkHours == inspectionWorkHours);
        }

        if (queryDto?.AoiWorkHours.HasValue == true)
        {
            var aoiWorkHours = queryDto.AoiWorkHours.Value;
            exp = exp.And(x => x.AoiWorkHours == aoiWorkHours);
        }

        if (queryDto?.DefectQty.HasValue == true)
        {
            var defectQty = queryDto.DefectQty.Value;
            exp = exp.And(x => x.DefectQty == defectQty);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.HandPlacement))
        {
            var handPlacement = queryDto.HandPlacement;
            exp = exp.And(x => x.HandPlacement != null && x.HandPlacement.Contains(handPlacement));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SerialNumber))
        {
            var serialNumber = queryDto.SerialNumber;
            exp = exp.And(x => x.SerialNumber != null && x.SerialNumber.Contains(serialNumber));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.Content))
        {
            var content = queryDto.Content;
            exp = exp.And(x => x.Content != null && x.Content.Contains(content));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DefectLocation))
        {
            var defectLocation = queryDto.DefectLocation;
            exp = exp.And(x => x.DefectLocation != null && x.DefectLocation.Contains(defectLocation));
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

        if (queryDto?.BSideAssemblyDateStart.HasValue == true)
        {
            var bSideAssemblyDateStart = queryDto.BSideAssemblyDateStart.Value;
            exp = exp.And(x => x.BSideAssemblyDate >= bSideAssemblyDateStart);
        }

        if (queryDto?.BSideAssemblyDateEnd.HasValue == true)
        {
            var bSideAssemblyDateEnd = queryDto.BSideAssemblyDateEnd.Value;
            exp = exp.And(x => x.BSideAssemblyDate <= bSideAssemblyDateEnd);
        }

        if (queryDto?.TSideAssemblyDateStart.HasValue == true)
        {
            var tSideAssemblyDateStart = queryDto.TSideAssemblyDateStart.Value;
            exp = exp.And(x => x.TSideAssemblyDate >= tSideAssemblyDateStart);
        }

        if (queryDto?.TSideAssemblyDateEnd.HasValue == true)
        {
            var tSideAssemblyDateEnd = queryDto.TSideAssemblyDateEnd.Value;
            exp = exp.And(x => x.TSideAssemblyDate <= tSideAssemblyDateEnd);
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
    private static bool HasAnyListQueryFilter(TaktPcbaInspectionDetailQueryDto? queryDto)
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
        if (queryDto.PcbaInspectionId.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProdOrderCode))
        {
            return true;
        }
        if (queryDto.LineNumber.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PcbaBoardType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.VisualInspectionLine))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.AoiLine))
        {
            return true;
        }
        if (queryDto.ShiftNo.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.InspectorName))
        {
            return true;
        }
        if (queryDto.DailyCompletedQty.HasValue)
        {
            return true;
        }
        if (queryDto.InspectionQty.HasValue)
        {
            return true;
        }
        if (queryDto.InspectionStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TeamCode))
        {
            return true;
        }
        if (queryDto.InspectionWorkHours.HasValue)
        {
            return true;
        }
        if (queryDto.AoiWorkHours.HasValue)
        {
            return true;
        }
        if (queryDto.DefectQty.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.HandPlacement))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SerialNumber))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.Content))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DefectLocation))
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
        if (queryDto.BSideAssemblyDateStart.HasValue || queryDto.BSideAssemblyDateEnd.HasValue)
        {
            return true;
        }
        if (queryDto.TSideAssemblyDateStart.HasValue || queryDto.TSideAssemblyDateEnd.HasValue)
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
