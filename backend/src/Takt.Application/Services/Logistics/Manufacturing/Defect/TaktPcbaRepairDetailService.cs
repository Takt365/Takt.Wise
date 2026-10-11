// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：TaktPcbaRepairDetailService.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA改修明细应用服务实现
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
/// PCBA改修明细应用服务
/// </summary>
public class TaktPcbaRepairDetailService : TaktServiceBase, ITaktPcbaRepairDetailService
{
    private readonly ITaktCompanyRepository<TaktPcbaRepairDetail> _pcbaRepairDetailRepository;
    private readonly ITaktCompanyRepository<TaktPcbaRepair> _pcbaRepairRepository;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;
    private readonly ITaktUniqueValidator _uniqueValidator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="pcbaRepairDetailRepository">PCBA改修明细仓储</param>
    /// <param name="pcbaRepairRepository">PCBA改修日报仓储</param>
    /// <param name="lineNumberGenerator">明细行号生成器</param>
    /// <param name="uniqueValidator">唯一性验证器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktPcbaRepairDetailService(
        ITaktCompanyRepository<TaktPcbaRepairDetail> pcbaRepairDetailRepository,
        ITaktCompanyRepository<TaktPcbaRepair> pcbaRepairRepository,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUniqueValidator uniqueValidator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _pcbaRepairDetailRepository = pcbaRepairDetailRepository;
        _pcbaRepairRepository = pcbaRepairRepository;
        _lineNumberGenerator = lineNumberGenerator;
        _uniqueValidator = uniqueValidator;
    }

    /// <summary>
    /// 获取PCBA改修明细列表（分页；无业务查询条件时返回空结果）
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktPcbaRepairDetailDto>> GetPcbaRepairDetailListAsync(TaktPcbaRepairDetailQueryDto queryDto)
    {
        if (!HasAnyListQueryFilter(queryDto))
        {
            return TaktPagedResult<TaktPcbaRepairDetailDto>.Create(
                new List<TaktPcbaRepairDetailDto>(),
                0,
                queryDto.PageIndex,
                queryDto.PageSize);
        }
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _pcbaRepairDetailRepository.GetPagedAsync(
            queryDto.PageIndex,
            queryDto.PageSize,
            predicate);
        return TaktPagedResult<TaktPcbaRepairDetailDto>.Create(
            data.Adapt<List<TaktPcbaRepairDetailDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据ID获取PCBA改修明细
    /// </summary>
    /// <param name="id">PCBA改修明细ID</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaRepairDetailDto?> GetPcbaRepairDetailByIdAsync(long id)
    {
        var entity = await _pcbaRepairDetailRepository.GetByIdAsync(id);
        if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return entity.Adapt<TaktPcbaRepairDetailDto>();
    }

    /// <summary>
    /// 获取PCBA改修明细选项列表
    /// </summary>
    /// <param name="plantCode">工厂代码（可选，用于按工厂过滤）</param>
    /// <param name="keyword">搜索关键字（可选，模糊匹配）</param>
    /// <returns>下拉选项</returns>
    public async Task<List<TaktSelectOption>> GetPcbaRepairDetailOptionsAsync(string? plantCode = null, string? keyword = null)
    {
        EnsureThreeLayerContext();
        var list = await _pcbaRepairDetailRepository.GetListAsync(
            x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.IsObsolete == 0,
            x => x.ProdOrderCode ?? string.Empty,
            false);
        return list.Select(e => new TaktSelectOption
        {
            DictValue = e.ProdOrderCode,
            DictLabel = e.ProdOrderCode,
        }).ToList();
    }

    /// <summary>
    /// 创建PCBA改修明细
    /// </summary>
    /// <param name="dto">创建DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaRepairDetailDto> CreatePcbaRepairDetailAsync(TaktPcbaRepairDetailCreateDto dto)
    {
        var entity = dto.Adapt<TaktPcbaRepairDetail>();
        entity.IsObsolete = 0;
        await StampPcbaRepairDetailPcbaRepairAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaRepairDetailRepository,
            x => x.PcbaRepairId == entity.PcbaRepairId
                && x.LineNumber == entity.LineNumber);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique)
        {
            throw new TaktBusinessException("PCBA改修明细的PcbaRepairId、LineNumber已存在");
        }
        if (entity.LineNumber <= 0)
        {
            var maxLine = await _pcbaRepairDetailRepository.GetMaxIntAsync(
                x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PcbaRepairId == entity.PcbaRepairId,
                x => x.LineNumber);
            var businessCode = entity.PcbaRepairId.ToString();
            entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
        }
        entity = await _pcbaRepairDetailRepository.CreateAsync(entity);
        return await GetPcbaRepairDetailByIdAsync(entity.Id) ?? entity.Adapt<TaktPcbaRepairDetailDto>();
    }

    /// <summary>
    /// 更新PCBA改修明细
    /// </summary>
    /// <param name="id">PCBA改修明细ID</param>
    /// <param name="dto">更新DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaRepairDetailDto> UpdatePcbaRepairDetailAsync(long id, TaktPcbaRepairDetailUpdateDto dto)
    {
        var entity = await _pcbaRepairDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA改修明细不存在");
        }
        dto.Adapt(entity);
        await StampPcbaRepairDetailPcbaRepairAsync(entity, dto);
        var isUnique_ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
            _pcbaRepairDetailRepository,
            x => x.PcbaRepairId == entity.PcbaRepairId
                && x.LineNumber == entity.LineNumber,
            id);
        if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique)
        {
            throw new TaktBusinessException("PCBA改修明细的PcbaRepairId、LineNumber已存在");
        }
        await _pcbaRepairDetailRepository.UpdateAsync(entity);
        return await GetPcbaRepairDetailByIdAsync(id) ?? throw new TaktBusinessException("PCBA改修明细不存在");
    }

    /// <summary>
    /// 删除PCBA改修明细
    /// </summary>
    /// <param name="id">PCBA改修明细ID</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaRepairDetailByIdAsync(long id)
    {
        var entity = await _pcbaRepairDetailRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA改修明细不存在或已删除");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("PCBA改修明细不存在或已删除");
        }
        if (entity.IsObsolete == 1)
        {
            throw new TaktBusinessException("PCBA改修明细已作废");
        }
        entity.IsObsolete = 1;
        await _pcbaRepairDetailRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 批量删除PCBA改修明细
    /// </summary>
    /// <param name="ids">ID列表</param>
    /// <returns>任务</returns>
    public async Task DeletePcbaRepairDetailBatchAsync(IEnumerable<long> ids)
    {
        var idList = ids?.Distinct().ToList() ?? new List<long>();
        if (idList.Count == 0)
        {
            return;
        }
        foreach (var id in idList)
        {
            await DeletePcbaRepairDetailByIdAsync(id);
        }
    }

    /// <summary>
    /// 更新PCBA改修明细作废状态
    /// </summary>
    /// <param name="dto">作废DTO</param>
    /// <returns>DTO</returns>
    public async Task<TaktPcbaRepairDetailDto> UpdatePcbaRepairDetailObsoleteAsync(TaktPcbaRepairDetailObsoleteDto dto)
    {
        var entity = await _pcbaRepairDetailRepository.GetByIdAsync(dto.PcbaRepairDetailId);
        if (entity == null)
        {
            throw new TaktBusinessException("PCBA改修明细不存在");
        }
        if (entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("PCBA改修明细不存在");
        }
        entity.IsObsolete = dto.IsObsolete;
        await _pcbaRepairDetailRepository.UpdateAsync(entity);
        return await GetPcbaRepairDetailByIdAsync(dto.PcbaRepairDetailId) ?? throw new TaktBusinessException("PCBA改修明细不存在");
    }

    /// <summary>
    /// 获取导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetPcbaRepairDetailTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktPcbaRepairDetailTemplateDto>(
            sheetName ?? "PCBA改修明细导入模板",
            fileName ?? "PCBA改修明细导入模板.xlsx");
    }

    /// <summary>
    /// 导入PCBA改修明细
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportPcbaRepairDetailAsync(Stream fileStream, string? sheetName = null)
    {
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktPcbaRepairDetailImportDto>(fileStream, sheetName ?? "PCBA改修明细导入模板");
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
                var entity = rows[i].Adapt<TaktPcbaRepairDetail>();
                var importDto = rows[i].Adapt<TaktPcbaRepairDetailCreateDto>();
                await StampPcbaRepairDetailPcbaRepairAsync(entity, importDto);
                var importKey = $"{entity.PcbaRepairId}|{entity.LineNumber}";
                if (!importSeenKeys.Add(importKey))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（PcbaRepairId、LineNumber）");
                }
                var isUnique_ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique = await _uniqueValidator.IsUniqueAsync(
                    _pcbaRepairDetailRepository,
                    x => x.PcbaRepairId == entity.PcbaRepairId
                        && x.LineNumber == entity.LineNumber);
                if (!isUnique_ix_takt_logistics_manufacturing_defect_pcba_repair_detail_line_unique)
                {
                    throw new TaktBusinessException("PCBA改修明细的PcbaRepairId、LineNumber已存在");
                }
                if (entity.LineNumber <= 0)
                {
                    var maxLine = await _pcbaRepairDetailRepository.GetMaxIntAsync(
                        x => x.TenantCode == CurrentTenantCode && x.CompanyCode == CurrentCompanyCode && x.PcbaRepairId == entity.PcbaRepairId,
                        x => x.LineNumber);
                    var businessCode = entity.PcbaRepairId.ToString();
                    entity.LineNumber = _lineNumberGenerator.GenerateNext(businessCode, maxLine);
                }
                await _pcbaRepairDetailRepository.CreateAsync(entity);
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
    /// 导出PCBA改修明细
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportPcbaRepairDetailAsync(TaktPcbaRepairDetailQueryDto? query = null, string? sheetName = null, string? fileName = null)
    {
        var queryDto = query ?? new TaktPcbaRepairDetailQueryDto();
        if (!HasAnyListQueryFilter(queryDto))
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaRepairDetailExportDto>(),
                sheetName ?? "PCBA改修明细数据",
                fileName ?? "PCBA改修明细导出.xlsx");
        }
        var predicate = QueryExpression(queryDto);
        var list = await _pcbaRepairDetailRepository.GetListAsync(predicate);
        if (list == null || list.Count == 0)
        {
            return await TaktExcelHelper.ExportAsync(
                new List<TaktPcbaRepairDetailExportDto>(),
                sheetName ?? "PCBA改修明细数据",
                fileName ?? "PCBA改修明细导出.xlsx");
        }
        var exportData = list.Adapt<List<TaktPcbaRepairDetailExportDto>>();
        return await TaktExcelHelper.ExportAsync(
            exportData,
            sheetName ?? "PCBA改修明细数据",
            fileName ?? "PCBA改修明细导出.xlsx");
    }

    // ========================================
    // 主表外键同步（ManyToOne）
    // ========================================

    /// <summary>
    /// 同步PCBA改修明细主表外键（ManyToOne → PCBA改修日报）
    /// </summary>
    /// <param name="entity">当前实体</param>
    /// <param name="dto">创建 DTO</param>
    /// <returns>任务</returns>
    private async Task StampPcbaRepairDetailPcbaRepairAsync(TaktPcbaRepairDetail entity, TaktPcbaRepairDetailCreateDto dto)
    {
        if (dto.PcbaRepairId <= 0)
        {
            return;
        }
        var master = await _pcbaRepairRepository.GetByIdAsync(dto.PcbaRepairId);
        if (master == null)
        {
            throw new TaktBusinessException("PCBA改修日报不存在");
        }
        entity.PcbaRepairId = master.Id;
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
        if (string.IsNullOrEmpty(entity.TeamCode))
        {
            entity.TeamCode = master.TeamCode;
        }
    }
    // ========================================
    // 查询表达式
    // ========================================

    /// <summary>
    /// 构建PCBA改修明细查询表达式
    /// </summary>
    /// <param name="queryDto">查询DTO</param>
    /// <returns>查询表达式</returns>
    private static Expression<Func<TaktPcbaRepairDetail, bool>> QueryExpression(TaktPcbaRepairDetailQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktPcbaRepairDetail>();

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
                || (x.TeamCode != null && x.TeamCode.Contains(keywords))
                || (x.CardCode != null && x.CardCode.Contains(keywords))
                || (x.DefectSymptom != null && x.DefectSymptom.Contains(keywords))
                || (x.DefectEngineering != null && x.DefectEngineering.Contains(keywords))
                || (x.DefectReason != null && x.DefectReason.Contains(keywords))
                || (x.DefectResponsibility != null && x.DefectResponsibility.Contains(keywords))
                || (x.DefectNature != null && x.DefectNature.Contains(keywords))
                || (x.RepairOperatorName != null && x.RepairOperatorName.Contains(keywords))
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

        if (queryDto?.PcbaRepairId.HasValue == true)
        {
            var pcbaRepairId = queryDto.PcbaRepairId.Value;
            exp = exp.And(x => x.PcbaRepairId == pcbaRepairId);
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

        if (queryDto?.ProdActualQty.HasValue == true)
        {
            var prodActualQty = queryDto.ProdActualQty.Value;
            exp = exp.And(x => x.ProdActualQty == prodActualQty);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.TeamCode))
        {
            var teamCode = queryDto.TeamCode;
            exp = exp.And(x => x.TeamCode != null && x.TeamCode.Contains(teamCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.CardCode))
        {
            var cardCode = queryDto.CardCode;
            exp = exp.And(x => x.CardCode != null && x.CardCode.Contains(cardCode));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DefectSymptom))
        {
            var defectSymptom = queryDto.DefectSymptom;
            exp = exp.And(x => x.DefectSymptom != null && x.DefectSymptom.Contains(defectSymptom));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DefectEngineering))
        {
            var defectEngineering = queryDto.DefectEngineering;
            exp = exp.And(x => x.DefectEngineering != null && x.DefectEngineering.Contains(defectEngineering));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DefectReason))
        {
            var defectReason = queryDto.DefectReason;
            exp = exp.And(x => x.DefectReason != null && x.DefectReason.Contains(defectReason));
        }

        if (queryDto?.DefectQty.HasValue == true)
        {
            var defectQty = queryDto.DefectQty.Value;
            exp = exp.And(x => x.DefectQty == defectQty);
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DefectResponsibility))
        {
            var defectResponsibility = queryDto.DefectResponsibility;
            exp = exp.And(x => x.DefectResponsibility != null && x.DefectResponsibility.Contains(defectResponsibility));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.DefectNature))
        {
            var defectNature = queryDto.DefectNature;
            exp = exp.And(x => x.DefectNature != null && x.DefectNature.Contains(defectNature));
        }

        if (!string.IsNullOrWhiteSpace(queryDto?.RepairOperatorName))
        {
            var repairOperator = queryDto.RepairOperatorName;
            exp = exp.And(x => x.RepairOperatorName != null && x.RepairOperatorName.Contains(repairOperator));
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
    private static bool HasAnyListQueryFilter(TaktPcbaRepairDetailQueryDto? queryDto)
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
        if (queryDto.PcbaRepairId.HasValue)
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
        if (queryDto.ProdActualQty.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.TeamCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.CardCode))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DefectSymptom))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DefectEngineering))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DefectReason))
        {
            return true;
        }
        if (queryDto.DefectQty.HasValue)
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DefectResponsibility))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.DefectNature))
        {
            return true;
        }
        if (!string.IsNullOrWhiteSpace(queryDto.RepairOperatorName))
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
