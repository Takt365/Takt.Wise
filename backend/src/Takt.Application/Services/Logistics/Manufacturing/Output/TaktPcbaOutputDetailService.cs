// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetailService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报明细应用服务实现
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
/// PCBA日报明细应用服务
/// </summary>
public class TaktPcbaOutputDetailService : TaktServiceBase, ITaktPcbaOutputDetailService
{
    private readonly ITaktCompanyRepository<TaktPcbaOutputDetail> _pcbaOutputDetailRepository;
    private readonly ITaktCompanyRepository<TaktPcbaOutput> _pcbaOutputRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaOutputDetailRepository">PCBA日报明细仓储</param>
    /// <param name="pcbaOutputRepository">PCBA日报仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaOutputDetailService(
        ITaktCompanyRepository<TaktPcbaOutputDetail> pcbaOutputDetailRepository,
        ITaktCompanyRepository<TaktPcbaOutput> pcbaOutputRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaOutputDetailRepository = pcbaOutputDetailRepository;
        _pcbaOutputRepository = pcbaOutputRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取PCBA日报明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPcbaOutputDetailDto>> GetPcbaOutputDetailListAsync(TaktPcbaOutputDetailQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPcbaOutputDetailDto>.Create(
                new List<TaktPcbaOutputDetailDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _pcbaOutputDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPcbaOutputDetailDto>.Create(
            data.Adapt<List<TaktPcbaOutputDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取PCBA日报明细
    /// </summary>
    /// <param name="id">PCBA日报明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDetailDto?> GetPcbaOutputDetailByIdAsync(long id)
    {
        var entity = await _pcbaOutputDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPcbaOutputDetailDto>();
    }

    /// <summary>
    /// 获取PCBA日报明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPcbaOutputDetailOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _pcbaOutputDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.CompletedStatus == 1 && x.IsObsolete == 0,
            x => x.ProdOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ProdOrderCode,
            DictLabel = e.ProdOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建PCBA日报明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDetailDto> CreatePcbaOutputDetailAsync(TaktPcbaOutputDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktPcbaOutputDetail>();
        entity.IsObsolete = 0;
        await StampPcbaOutputDetailPcbaOutputAsync(entity, dto);
        NormalizePcbaOutputDetailUniqueFields(entity);
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _pcbaOutputDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PcbaOutputId == entity.PcbaOutputId,
                x => x.LineNumber);
            var businessCode = entity.PcbaOutputId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        await EnsurePcbaOutputDetailUniqueAsync(entity, excludeId: null);
        entity = await _pcbaOutputDetailRepository.CreateAsync(entity);
        return await GetPcbaOutputDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktPcbaOutputDetailDto>();
    }

    /// <summary>
    /// 更新PCBA日报明细
    /// </summary>
    /// <param name="id">PCBA日报明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDetailDto> UpdatePcbaOutputDetailAsync(long id, TaktPcbaOutputDetailUpdateDto dto)
    {
        var entity = await _pcbaOutputDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA日报明细不存在");
        }
        dto.Adapt(entity);
        await StampPcbaOutputDetailPcbaOutputAsync(entity, dto);
        NormalizePcbaOutputDetailUniqueFields(entity);
        await EnsurePcbaOutputDetailUniqueAsync(entity, excludeId: id);
        await _pcbaOutputDetailRepository.UpdateAsync(entity);
        return await GetPcbaOutputDetailByIdAsync(id) ?? throw new TaktBusinessException("PCBA日报明细不存在");
    }

    /// <summary>
    /// 删除PCBA日报明细
    /// </summary>
    /// <param name="id">PCBA日报明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaOutputDetailByIdAsync(long id)
    {
        var entity = await _pcbaOutputDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA日报明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("PCBA日报明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("PCBA日报明细已作废");
        }
        entity.IsObsolete = 1;
        await _pcbaOutputDetailRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除PCBA日报明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaOutputDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePcbaOutputDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新PCBA日报明细状态
    /// </summary>
    /// <param name="dto">状态DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDetailDto> UpdatePcbaOutputDetailStatusAsync(TaktPcbaOutputDetailStatusDto dto)
    {
        var entity = await _pcbaOutputDetailRepository.GetByIdAsync(dto.PcbaOutputDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA日报明细不存在");
        }
        entity.CompletedStatus = dto.CompletedStatus;
        await _pcbaOutputDetailRepository.UpdateAsync(entity);
        return await GetPcbaOutputDetailByIdAsync(dto.PcbaOutputDetailId) ?? throw new TaktBusinessException("PCBA日报明细不存在");
    }

    /// <summary>
    /// 更新PCBA日报明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaOutputDetailDto> UpdatePcbaOutputDetailObsoleteAsync(TaktPcbaOutputDetailObsoleteDto dto)
    {
        var entity = await _pcbaOutputDetailRepository.GetByIdAsync(dto.PcbaOutputDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA日报明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("PCBA日报明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _pcbaOutputDetailRepository.UpdateAsync(entity);
        return await GetPcbaOutputDetailByIdAsync(dto.PcbaOutputDetailId) ?? throw new TaktBusinessException("PCBA日报明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPcbaOutputDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPcbaOutputDetailTemplateDto>(
            sheetName ?? "PCBA日报明细导入模板",
            fileName ?? "PCBA日报明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入PCBA日报明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPcbaOutputDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPcbaOutputDetailImportDto>(fileStream, sheetName ?? "PCBA日报明细导入模板");
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
                var entity = rows[i].Adapt<TaktPcbaOutputDetail>();
                var importDto = rows[i].Adapt<TaktPcbaOutputDetailCreateDto>();
                await StampPcbaOutputDetailPcbaOutputAsync(entity, importDto);
                NormalizePcbaOutputDetailUniqueFields(entity);
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _pcbaOutputDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PcbaOutputId == entity.PcbaOutputId,
                        x => x.LineNumber);
                    var businessCode = entity.PcbaOutputId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                var importKey = BuildPcbaOutputDetailUniqueKey(
                    entity.PcbaOutputId,
                    entity.TeamCode,
                    entity.ShiftNo,
                    entity.PcbBoardType,
                    entity.PanelSide,
                    entity.LineNumber);
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、PanelSide、LineNumber）");
                }
                await EnsurePcbaOutputDetailUniqueAsync(entity, excludeId: null);
                await _pcbaOutputDetailRepository.CreateAsync(entity);
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
    /// 导出PCBA日报明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPcbaOutputDetailAsync(TaktPcbaOutputDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPcbaOutputDetailQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaOutputDetailExportDto>(),
                sheetName ?? "PCBA日报明细数据",
                fileName ?? "PCBA日报明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _pcbaOutputDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaOutputDetailExportDto>(),
                sheetName ?? "PCBA日报明细数据",
                fileName ?? "PCBA日报明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPcbaOutputDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "PCBA日报明细数据",
            fileName ?? "PCBA日报明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    // ========================================
    // 唯一键（PcbaOutputId + TeamCode + ShiftNo + PcbBoardType + PanelSide + LineNumber）
    // ========================================

    /// <summary>
    /// 规范化参与唯一键的字符串字段
    /// </summary>
    /// <param name="entity">明细实体</param>
    private static void NormalizePcbaOutputDetailUniqueFields(TaktPcbaOutputDetail entity)
    {
        entity.TeamCode = (entity.TeamCode ?? string.Empty).Trim();
        entity.PcbBoardType = (entity.PcbBoardType ?? string.Empty).Trim();
        entity.PanelSide = (entity.PanelSide ?? string.Empty).Trim();
    }

    /// <summary>
    /// 构建 PCBA 明细唯一键字符串（导入/批次去重）
    /// </summary>
    private static string BuildPcbaOutputDetailUniqueKey(
        long pcbaOutputId,
        string? teamCode,
        int shiftNo,
        string? pcbBoardType,
        string? panelSide,
        int lineNumber)
    {
        return $"{pcbaOutputId}|{(teamCode ?? string.Empty).Trim()}|{shiftNo}|{(pcbBoardType ?? string.Empty).Trim()}|{(panelSide ?? string.Empty).Trim()}|{lineNumber}";
    }

    /// <summary>
    /// 校验 PCBA 明细唯一键（对齐 ix_takt_logistics_manufacturing_output_pcba_detail_unique）
    /// </summary>
    /// <param name="entity">明细实体</param>
    /// <param name="excludeId">更新时排除的主键</param>
    private async Task EnsurePcbaOutputDetailUniqueAsync(TaktPcbaOutputDetail entity, long? excludeId)
    {
        var isUnique = await _uniqueValidator.IsUniqueAsync(
            _pcbaOutputDetailRepository,
            x => x.PcbaOutputId == entity.PcbaOutputId
                && x.TeamCode == entity.TeamCode
                && x.ShiftNo == entity.ShiftNo
                && x.PcbBoardType == entity.PcbBoardType
                && x.PanelSide == entity.PanelSide
                && x.LineNumber == entity.LineNumber,
            excludeId);
        if (!isUnique)
        {
            throw new TaktBusinessException("PCBA日报明细的PcbaOutputId、TeamCode、ShiftNo、PcbBoardType、PanelSide、LineNumber已存在");
        }
    }

    /// <summary>
    /// 同步PCBA日报明细主表外键（ManyToOne → PCBA日报）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampPcbaOutputDetailPcbaOutputAsync(TaktPcbaOutputDetail entity, TaktPcbaOutputDetailCreateDto dto)
    {
        if (dto.PcbaOutputId <= 0)
        {
            return;
        }
        var master = await _pcbaOutputRepository.GetByIdAsync(dto.PcbaOutputId);
        if (master == null)
        {
            throw new TaktBusinessException("PCBA日报不存在");
        }
        entity.PcbaOutputId = master.Id;
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
        if (string.IsNullOrEmpty(entity.SerialCode))
        {
            entity.SerialCode = master.SerialCode ?? string.Empty;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建PCBA日报明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPcbaOutputDetail, bool>> QueryExpression(TaktPcbaOutputDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPcbaOutputDetail>();

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
                || (x.TimePeriod != null && x.TimePeriod.Contains(keywords))
                || (x.TeamCode != null && x.TeamCode.Contains(keywords))
                || (x.ProdEquipCode != null && x.ProdEquipCode.Contains(keywords))
                || (x.PcbBoardType != null && x.PcbBoardType.Contains(keywords))
                || (x.PanelSide != null && x.PanelSide.Contains(keywords))
                || (x.SerialCode != null && x.SerialCode.Contains(keywords))
                || (x.DowntimeReason != null && x.DowntimeReason.Contains(keywords))
                || (x.DowntimeDescription != null && x.DowntimeDescription.Contains(keywords))
                || (x.UnachievedReason != null && x.UnachievedReason.Contains(keywords))
                || (x.UnachievedDescription != null && x.UnachievedDescription.Contains(keywords))
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

        if (queryDto?.PcbaOutputId.HasValue == true)
        {
            var pcbaOutputId = queryDto.PcbaOutputId.Value;
            exp = exp.And(x => x.PcbaOutputId == pcbaOutputId);
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

        if (!string.IsNullOrWhiteSpace(queryDto?.TimePeriod))
        {
            var timePeriod = queryDto.TimePeriod;
            exp = exp.And(x => x.TimePeriod != null && x.TimePeriod.Contains(timePeriod));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TeamCode))
        {
            var teamCode = queryDto.TeamCode;
            exp = exp.And(x => x.TeamCode != null && x.TeamCode.Contains(teamCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.ProdEquipCode))
        {
            var prodEquipCode = queryDto.ProdEquipCode;
            exp = exp.And(x => x.ProdEquipCode != null && x.ProdEquipCode.Contains(prodEquipCode));
        }

        if (queryDto?.DirectLabor.HasValue == true)
        {
            var directLabor = queryDto.DirectLabor.Value;
            exp = exp.And(x => x.DirectLabor == directLabor);
        }

        if (queryDto?.IndirectLabor.HasValue == true)
        {
            var indirectLabor = queryDto.IndirectLabor.Value;
            exp = exp.And(x => x.IndirectLabor == indirectLabor);
        }

        if (queryDto?.ShiftNo.HasValue == true)
        {
            var shiftNo = queryDto.ShiftNo.Value;
            exp = exp.And(x => x.ShiftNo == shiftNo);
        }

        if (queryDto?.StdMinutes.HasValue == true)
        {
            var stdMinutes = queryDto.StdMinutes.Value;
            exp = exp.And(x => x.StdMinutes == stdMinutes);
        }

        if (queryDto?.StdLaborCapacity.HasValue == true)
        {
            var stdLaborCapacity = queryDto.StdLaborCapacity.Value;
            exp = exp.And(x => x.StdLaborCapacity == stdLaborCapacity);
        }

        if (queryDto?.StdShorts.HasValue == true)
        {
            var stdShorts = queryDto.StdShorts.Value;
            exp = exp.And(x => x.StdShorts == stdShorts);
        }

        if (queryDto?.StdEquipmentCapacity.HasValue == true)
        {
            var stdEquipmentCapacity = queryDto.StdEquipmentCapacity.Value;
            exp = exp.And(x => x.StdEquipmentCapacity == stdEquipmentCapacity);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PcbBoardType))
        {
            var pcbBoardType = queryDto.PcbBoardType;
            exp = exp.And(x => x.PcbBoardType != null && x.PcbBoardType.Contains(pcbBoardType));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.PanelSide))
        {
            var panelSide = queryDto.PanelSide;
            exp = exp.And(x => x.PanelSide != null && x.PanelSide.Contains(panelSide));
        }

        if (queryDto?.BatchQty.HasValue == true)
        {
            var batchQty = queryDto.BatchQty.Value;
            exp = exp.And(x => x.BatchQty == batchQty);
        }

        if (queryDto?.DailyCompletedQty.HasValue == true)
        {
            var dailyCompletedQty = queryDto.DailyCompletedQty.Value;
            exp = exp.And(x => x.DailyCompletedQty == dailyCompletedQty);
        }

        if (queryDto?.TotalCompletedQty.HasValue == true)
        {
            var totalCompletedQty = queryDto.TotalCompletedQty.Value;
            exp = exp.And(x => x.TotalCompletedQty == totalCompletedQty);
        }

        if (queryDto?.CompletedStatus.HasValue == true)
        {
            var completedStatus = queryDto.CompletedStatus.Value;
            exp = exp.And(x => x.CompletedStatus == completedStatus);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.SerialCode))
        {
            var serialCode = queryDto.SerialCode;
            exp = exp.And(x => x.SerialCode != null && x.SerialCode.Contains(serialCode));
        }

        if (queryDto?.DefectCount.HasValue == true)
        {
            var defectCount = queryDto.DefectCount.Value;
            exp = exp.And(x => x.DefectCount == defectCount);
        }

        if (queryDto?.DowntimeMinutes.HasValue == true)
        {
            var downtimeMinutes = queryDto.DowntimeMinutes.Value;
            exp = exp.And(x => x.DowntimeMinutes == downtimeMinutes);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DowntimeReason))
        {
            var downtimeReason = queryDto.DowntimeReason;
            exp = exp.And(x => x.DowntimeReason != null && x.DowntimeReason.Contains(downtimeReason));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DowntimeDescription))
        {
            var downtimeDescription = queryDto.DowntimeDescription;
            exp = exp.And(x => x.DowntimeDescription != null && x.DowntimeDescription.Contains(downtimeDescription));
        }

        if (queryDto?.InputMinutes.HasValue == true)
        {
            var inputMinutes = queryDto.InputMinutes.Value;
            exp = exp.And(x => x.InputMinutes == inputMinutes);
        }

        if (queryDto?.ActualMinutes.HasValue == true)
        {
            var actualMinutes = queryDto.ActualMinutes.Value;
            exp = exp.And(x => x.ActualMinutes == actualMinutes);
        }

        if (queryDto?.RepairMinutes.HasValue == true)
        {
            var repairMinutes = queryDto.RepairMinutes.Value;
            exp = exp.And(x => x.RepairMinutes == repairMinutes);
        }

        if (queryDto?.SwitchCount.HasValue == true)
        {
            var switchCount = queryDto.SwitchCount.Value;
            exp = exp.And(x => x.SwitchCount == switchCount);
        }

        if (queryDto?.SwitchTime.HasValue == true)
        {
            var switchTime = queryDto.SwitchTime.Value;
            exp = exp.And(x => x.SwitchTime == switchTime);
        }

        if (queryDto?.StopTime.HasValue == true)
        {
            var stopTime = queryDto.StopTime.Value;
            exp = exp.And(x => x.StopTime == stopTime);
        }

        if (queryDto?.TotalMinutes.HasValue == true)
        {
            var totalMinutes = queryDto.TotalMinutes.Value;
            exp = exp.And(x => x.TotalMinutes == totalMinutes);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UnachievedReason))
        {
            var unachievedReason = queryDto.UnachievedReason;
            exp = exp.And(x => x.UnachievedReason != null && x.UnachievedReason.Contains(unachievedReason));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.UnachievedDescription))
        {
            var unachievedDescription = queryDto.UnachievedDescription;
            exp = exp.And(x => x.UnachievedDescription != null && x.UnachievedDescription.Contains(unachievedDescription));
        }

        if (queryDto?.ConfirmMinutes.HasValue == true)
        {
            var confirmMinutes = queryDto.ConfirmMinutes.Value;
            exp = exp.And(x => x.ConfirmMinutes == confirmMinutes);
        }

        if (queryDto?.MixedProd.HasValue == true)
        {
            var mixedProd = queryDto.MixedProd.Value;
            exp = exp.And(x => x.MixedProd == mixedProd);
        }

        if (queryDto?.AchievementRate.HasValue == true)
        {
            var achievementRate = queryDto.AchievementRate.Value;
            exp = exp.And(x => x.AchievementRate == achievementRate);
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
    private static bool HasAnyListQueryFilter(TaktPcbaOutputDetailQueryDto? queryDto)
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
        if (queryDto.PcbaOutputId.HasValue)
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
        if (!string.IsNullOrWhiteSpace(queryDto.TimePeriod))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TeamCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.ProdEquipCode))
        {
            return true;
        }
        if (queryDto.DirectLabor.HasValue)
        {
            return true;
        }
        if (queryDto.IndirectLabor.HasValue)
        {
            return true;
        }
        if (queryDto.ShiftNo.HasValue)
        {
            return true;
        }
        if (queryDto.StdMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.StdLaborCapacity.HasValue)
        {
            return true;
        }
        if (queryDto.StdShorts.HasValue)
        {
            return true;
        }
        if (queryDto.StdEquipmentCapacity.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PcbBoardType))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.PanelSide))
        {
            return true;
        }
        if (queryDto.BatchQty.HasValue)
        {
            return true;
        }
        if (queryDto.DailyCompletedQty.HasValue)
        {
            return true;
        }
        if (queryDto.TotalCompletedQty.HasValue)
        {
            return true;
        }
        if (queryDto.CompletedStatus.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.SerialCode))
        {
            return true;
        }
        if (queryDto.DefectCount.HasValue)
        {
            return true;
        }
        if (queryDto.DowntimeMinutes.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DowntimeReason))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DowntimeDescription))
        {
            return true;
        }
        if (queryDto.InputMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.ActualMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.RepairMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.SwitchCount.HasValue)
        {
            return true;
        }
        if (queryDto.SwitchTime.HasValue)
        {
            return true;
        }
        if (queryDto.StopTime.HasValue)
        {
            return true;
        }
        if (queryDto.TotalMinutes.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UnachievedReason))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.UnachievedDescription))
        {
            return true;
        }
        if (queryDto.ConfirmMinutes.HasValue)
        {
            return true;
        }
        if (queryDto.MixedProd.HasValue)
        {
            return true;
        }
        if (queryDto.AchievementRate.HasValue)
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
