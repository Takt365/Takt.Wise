// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcDeptViewServiceBase.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变各部门视图服务基类（TaktEcDetail + 对应部门执行表合并展示）
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
using Takt.Shared.Constants;
using Takt.Shared.Exceptions;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门视图服务基类
/// </summary>
public abstract class TaktEcDeptViewServiceBase : TaktServiceBase
{
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;
    private readonly TaktEcExecPersistence _ecExecPersistence;
    private readonly ITaktLineNumberGenerator _lineNumberGenerator;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="deptCode">固定部门编码</param>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="ecExecPersistence">执行聚合持久化</param>
    /// <param name="lineNumberGenerator">行号生成器</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    protected TaktEcDeptViewServiceBase(
        string deptCode,
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        TaktEcExecPersistence ecExecPersistence,
        ITaktLineNumberGenerator lineNumberGenerator,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        DeptCode = deptCode;
        _ecDetailRepository = ecDetailRepository;
        _ecExecPersistence = ecExecPersistence;
        _lineNumberGenerator = lineNumberGenerator;
    }

    /// <summary>
    /// 当前视图固定部门编码
    /// </summary>
    protected string DeptCode { get; }

    /// <summary>
    /// 获取部门视图列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcDeptViewDto>> GetDeptViewListAsync(TaktEcDeptViewQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var predicate = BuildDetailQueryExpression(queryDto);
        var (details, total) = await _ecDetailRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.EcCode,
            false);
        var detailIds = details.Select(x => x.Id).ToList();
        var execMap = await _ecExecPersistence.LoadMapByDetailIdsAsync(detailIds, DeptCode);
        var rows = details.Select(d => MapDeptViewRow(d, execMap.GetValueOrDefault(d.Id))).ToList();
        return TaktPagedResult<TaktEcDeptViewDto>.Create(rows, total, queryDto.PageIndex, queryDto.PageSize);
    }

    /// <summary>
    /// 根据设变明细 ID 获取部门视图行
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <returns>视图 DTO</returns>
    public async Task<TaktEcDeptViewDto?> GetDeptViewByEcDetailIdAsync(long ecDetailId)
    {
        EnsureThreeLayerContext();
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null || detail.TenantCode != CurrentTenantCode || detail.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        var exec = await _ecExecPersistence.LoadByDetailAndDeptAsync(ecDetailId, DeptCode);
        return MapDeptViewRow(detail, exec);
    }

    /// <summary>
    /// 更新部门视图（不存在则创建执行记录）
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="dto">更新 DTO</param>
    /// <returns>视图 DTO</returns>
    public async Task<TaktEcDeptViewDto> UpdateDeptViewAsync(long ecDetailId, TaktEcDeptViewUpdateDto dto)
    {
        EnsureThreeLayerContext();
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null || detail.TenantCode != CurrentTenantCode || detail.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        var exec = await _ecExecPersistence.UpsertFromViewUpdateAsync(
            detail,
            DeptCode,
            dto,
            async () =>
            {
                var maxLine = await _ecExecPersistence.GetMaxLineNumberForDetailDeptAsync(
                    detail.Id, DeptCode, CurrentTenantCode, CurrentCompanyCode);
                return _lineNumberGenerator.GenerateNext(detail.Id.ToString(), maxLine);
            });
        return MapDeptViewRow(detail, exec);
    }

    /// <summary>
    /// 获取部门视图导入模板
    /// </summary>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] content)> GetDeptViewTemplateAsync(string? sheetName = null, string? fileName = null)
    {
        return await TaktExcelHelper.GenerateTemplateAsync<TaktEcDeptViewTemplateDto>(
            sheetName ?? $"设变部门视图_{DeptCode}导入模板",
            fileName ?? $"设变部门视图_{DeptCode}导入模板.xlsx");
    }

    /// <summary>
    /// 导入部门视图（按行 upsert 当前 DeptCode 执行记录）
    /// </summary>
    /// <param name="fileStream">Excel 文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>导入结果</returns>
    public async Task<(int success, int fail, List<string> errors)> ImportDeptViewAsync(Stream fileStream, string? sheetName = null)
    {
        EnsureThreeLayerContext();
        var errors = new List<string>();
        var success = 0;
        var fail = 0;
        var rows = await TaktExcelHelper.ImportAsync<TaktEcDeptViewImportDto>(
            fileStream,
            sheetName ?? $"设变部门视图_{DeptCode}导入模板");
        if (rows == null || rows.Count == 0)
        {
            errors.Add("Excel文件中没有数据");
            return (0, 0, errors);
        }
        var importSeenKeys = new HashSet<long>();
        for (var i = 0; i < rows.Count; i++)
        {
            try
            {
                var row = rows[i];
                var detail = await ResolveDetailForImportAsync(row);
                if (detail == null)
                {
                    throw new TaktBusinessException("未找到设变明细，请填写 EcDetailId 或 EcCode+LineNumber");
                }
                if (!importSeenKeys.Add(detail.Id))
                {
                    throw new TaktBusinessException("与Excel中其他行重复（同一设变明细）");
                }
                var existing = await _ecExecPersistence.LoadByDetailAndDeptAsync(detail.Id, DeptCode);
                var updateDto = TaktEcDeptViewMapper.MergeImportRow(row, detail.Id, existing);
                await UpdateDeptViewAsync(detail.Id, updateDto);
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
    /// 导出部门视图
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportDeptViewAsync(
        TaktEcDeptViewQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        EnsureThreeLayerContext();
        var predicate = BuildDetailQueryExpression(query ?? new TaktEcDeptViewQueryDto());
        var list = await _ecDetailRepository.GetListForExportAsync(predicate);
        var detailIds = list.Select(x => x.Id).ToList();
        var execMap = await _ecExecPersistence.LoadMapByDetailIdsAsync(detailIds, DeptCode);
        var exportRows = list.Select(d => MapDeptViewRow(d, execMap.GetValueOrDefault(d.Id))).ToList();
        return await TaktExcelHelper.ExportAsync(
            exportRows,
            sheetName ?? "设变部门视图",
            fileName ?? "设变部门视图导出.xlsx");
    }

    /// <summary>
    /// 合并明细与部门执行实体为视图 DTO
    /// </summary>
    /// <param name="detail">设变明细</param>
    /// <param name="exec">部门执行实体（可为空）</param>
    /// <returns>视图 DTO</returns>
    private TaktEcDeptViewDto MapDeptViewRow(TaktEcDetail detail, object? exec)
    {
        var dto = detail.Adapt<TaktEcDeptViewDto>();
        dto.EcDetailId = detail.Id;
        dto.DeptCode = DeptCode;
        if (exec != null)
        {
            TaktEcDeptViewMapper.ApplyToViewDto(dto, exec);
        }
        return dto;
    }

    /// <summary>
    /// 解析导入行对应的设变明细
    /// </summary>
    /// <param name="row">导入行</param>
    /// <returns>设变明细</returns>
    private async Task<TaktEcDetail?> ResolveDetailForImportAsync(TaktEcDeptViewImportDto row)
    {
        if (row.EcDetailId.HasValue && row.EcDetailId.Value > 0)
        {
            var byId = await _ecDetailRepository.GetByIdAsync(row.EcDetailId.Value);
            if (byId != null
                && byId.TenantCode == CurrentTenantCode
                && byId.CompanyCode == CurrentCompanyCode)
            {
                return byId;
            }
            return null;
        }
        if (string.IsNullOrWhiteSpace(row.EcCode) || !row.LineNumber.HasValue)
        {
            return null;
        }
        return await _ecDetailRepository.FirstAsync(x =>
            x.EcCode == row.EcCode
            && x.LineNumber == row.LineNumber.Value);
    }

    /// <summary>
    /// 构建设变明细查询表达式（含部门实施状态过滤）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private Expression<Func<TaktEcDetail, bool>> BuildDetailQueryExpression(TaktEcDeptViewQueryDto queryDto)
    {
        var exp = Expressionable.Create<TaktEcDetail>();
        if (DeptCode == TaktEcDeptCodes.Pmc)
        {
            exp = exp.And(TaktEcSeikanQueryHelper.VisibleDetailExpression());
        }
        else if (DeptCode == TaktEcDeptCodes.Mp)
        {
            exp = exp.And(TaktEcKoubaiQueryHelper.VisibleDetailExpression());
        }
        else if (DeptCode == TaktEcDeptCodes.Iqc)
        {
            exp = exp.And(TaktEcUkekenQueryHelper.VisibleDetailExpression());
        }
        else if (DeptCode == TaktEcDeptCodes.Mc)
        {
            exp = exp.And(TaktEcBukanQueryHelper.VisibleDetailExpression());
        }
        else if (DeptCode == TaktEcDeptCodes.Assy)
        {
            exp = exp.And(TaktEcSeizouikkaQueryHelper.VisibleDetailExpression());
        }
        else if (DeptCode == TaktEcDeptCodes.Qa)
        {
            exp = exp.And(TaktEcHinkanQueryHelper.VisibleDetailExpression());
        }
        else if (DeptCode == TaktEcDeptCodes.Te)
        {
            exp = exp.And(TaktEcSeizougijutsuQueryHelper.VisibleDetailExpression());
        }
        if (!string.IsNullOrEmpty(queryDto.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcCode != null && x.EcCode.Contains(keywords))
                || (x.EcModelCode != null && x.EcModelCode.Contains(keywords))
                || (x.EcOldMaterialCode != null && x.EcOldMaterialCode.Contains(keywords))
                || (x.EcNewMaterialCode != null && x.EcNewMaterialCode.Contains(keywords)));
        }
        if (!string.IsNullOrEmpty(queryDto.EcCode))
        {
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(queryDto.EcCode));
        }
        if (!string.IsNullOrEmpty(queryDto.EcModelCode))
        {
            exp = exp.And(x => x.EcModelCode != null && x.EcModelCode.Contains(queryDto.EcModelCode));
        }
        if (!string.IsNullOrEmpty(queryDto.EcOldMaterialCode))
        {
            exp = exp.And(x => x.EcOldMaterialCode != null && x.EcOldMaterialCode.Contains(queryDto.EcOldMaterialCode));
        }
        if (!string.IsNullOrEmpty(queryDto.EcNewMaterialCode))
        {
            exp = exp.And(x => x.EcNewMaterialCode != null && x.EcNewMaterialCode.Contains(queryDto.EcNewMaterialCode));
        }
        if (queryDto.IsImplemented.HasValue)
        {
            var flag = queryDto.IsImplemented.Value;
            exp = DeptCode switch
            {
                TaktEcDeptCodes.Pmc => exp.And(x => SqlFunc.Subqueryable<TaktEcSeikan>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Mp => exp.And(x => SqlFunc.Subqueryable<TaktEcKoubai>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Iqc => exp.And(x => SqlFunc.Subqueryable<TaktEcUkeken>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Mc => exp.And(x => SqlFunc.Subqueryable<TaktEcBukan>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Pcba => exp.And(x =>
                    SqlFunc.Subqueryable<TaktEcEmtl>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()
                    || SqlFunc.Subqueryable<TaktEcSeizounika>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Assy => exp.And(x => SqlFunc.Subqueryable<TaktEcSeizouikka>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Qa => exp.And(x => SqlFunc.Subqueryable<TaktEcHinkan>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                TaktEcDeptCodes.Te => exp.And(x => SqlFunc.Subqueryable<TaktEcSeizougijutsu>().Where(d => d.EcDetailId == x.Id && d.IsImplemented == flag).Any()),
                _ => exp
            };
        }
        return exp.ToExpression();
    }
}
