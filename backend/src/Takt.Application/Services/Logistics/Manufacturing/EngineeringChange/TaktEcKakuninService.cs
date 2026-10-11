// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：TaktEcKakuninService.cs
// 创建时间：2026-06-22
// 创建人：Takt365(Cursor AI)
// 功能描述：设变物料确认应用服务实现
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

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变物料确认应用服务
/// </summary>
public class TaktEcKakuninService : TaktServiceBase, ITaktEcKakuninService
{
    private readonly ITaktCompanyRepository<TaktEcDetail> _ecDetailRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ecDetailRepository">设变明细仓储</param>
    /// <param name="userContext">用户上下文</param>
    /// <param name="localizationService">本地化服务</param>
    public TaktEcKakuninService(
        ITaktCompanyRepository<TaktEcDetail> ecDetailRepository,
        ITaktUserContext? userContext = null,
        ITaktLocalizationService? localizationService = null)
        : base(userContext, localizationService)
    {
        _ecDetailRepository = ecDetailRepository;
    }

    /// <summary>
    /// 获取物料确认列表（分页）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>分页结果</returns>
    public async Task<TaktPagedResult<TaktEcKakuninDto>> GetEcKakuninListAsync(TaktEcKakuninQueryDto queryDto)
    {
        EnsureThreeLayerContext();
        var predicate = QueryExpression(queryDto);
        var (data, total) = await _ecDetailRepository.GetPagedAsync(
            predicate,
            queryDto.PageIndex,
            queryDto.PageSize,
            x => x.EcCode,
            false);
        return TaktPagedResult<TaktEcKakuninDto>.Create(
            data.Adapt<List<TaktEcKakuninDto>>(),
            total,
            queryDto.PageIndex,
            queryDto.PageSize);
    }

    /// <summary>
    /// 根据设变明细 ID 获取物料确认行
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <returns>物料确认 DTO</returns>
    public async Task<TaktEcKakuninDto?> GetEcKakuninByEcDetailIdAsync(long ecDetailId)
    {
        EnsureThreeLayerContext();
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null || detail.TenantCode != CurrentTenantCode || detail.CompanyCode != CurrentCompanyCode)
        {
            return null;
        }
        return detail.Adapt<TaktEcKakuninDto>();
    }

    /// <summary>
    /// 更新物料确认
    /// </summary>
    /// <param name="ecDetailId">设变明细 ID</param>
    /// <param name="dto">更新 DTO</param>
    /// <returns>物料确认 DTO</returns>
    public async Task<TaktEcKakuninDto> UpdateEcKakuninAsync(long ecDetailId, TaktEcKakuninUpdateDto dto)
    {
        EnsureThreeLayerContext();
        var detail = await _ecDetailRepository.GetByIdAsync(ecDetailId);
        if (detail == null || detail.TenantCode != CurrentTenantCode || detail.CompanyCode != CurrentCompanyCode)
        {
            throw new TaktBusinessException("设变明细不存在");
        }
        detail.EcOldPurchaseType = dto.EcOldPurchaseType;
        detail.EcOldRequiresInspection = dto.EcOldRequiresInspection;
        detail.EcNewPurchaseType = dto.EcNewPurchaseType;
        detail.EcNewRequiresInspection = dto.EcNewRequiresInspection;
        await _ecDetailRepository.UpdateAsync(detail);
        return detail.Adapt<TaktEcKakuninDto>();
    }

    /// <summary>
    /// 导出物料确认
    /// </summary>
    /// <param name="query">查询条件</param>
    /// <param name="sheetName">工作表名</param>
    /// <param name="fileName">文件名</param>
    /// <returns>Excel 文件</returns>
    public async Task<(string fileName, byte[] fileContent)> ExportEcKakuninAsync(
        TaktEcKakuninQueryDto? query = null,
        string? sheetName = null,
        string? fileName = null)
    {
        EnsureThreeLayerContext();
        var predicate = QueryExpression(query ?? new TaktEcKakuninQueryDto());
        var list = await _ecDetailRepository.GetListForExportAsync(predicate);
        return await TaktExcelHelper.ExportAsync(
            list.Adapt<List<TaktEcKakuninDto>>(),
            sheetName ?? "物料确认",
            fileName ?? "物料确认导出.xlsx");
    }

    /// <summary>
    /// 构建物料确认查询表达式
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>表达式</returns>
    private static Expression<Func<TaktEcDetail, bool>> QueryExpression(TaktEcKakuninQueryDto? queryDto)
    {
        var exp = Expressionable.Create<TaktEcDetail>();
        if (!string.IsNullOrEmpty(queryDto?.KeyWords))
        {
            var keywords = queryDto.KeyWords;
            exp = exp.And(x =>
                (x.EcCode != null && x.EcCode.Contains(keywords))
                || (x.EcModelCode != null && x.EcModelCode.Contains(keywords))
                || (x.EcOldMaterialCode != null && x.EcOldMaterialCode.Contains(keywords))
                || (x.EcNewMaterialCode != null && x.EcNewMaterialCode.Contains(keywords)));
        }
        if (!string.IsNullOrEmpty(queryDto?.EcCode))
        {
            exp = exp.And(x => x.EcCode != null && x.EcCode.Contains(queryDto.EcCode));
        }
        if (!string.IsNullOrEmpty(queryDto?.EcModelCode))
        {
            exp = exp.And(x => x.EcModelCode != null && x.EcModelCode.Contains(queryDto.EcModelCode));
        }
        if (!string.IsNullOrEmpty(queryDto?.EcNewMaterialCode))
        {
            exp = exp.And(x => x.EcNewMaterialCode != null && x.EcNewMaterialCode.Contains(queryDto.EcNewMaterialCode));
        }
        if (queryDto?.EcOldRequiresInspection.HasValue == true)
        {
            exp = exp.And(x => x.EcOldRequiresInspection == queryDto.EcOldRequiresInspection);
        }
        if (queryDto?.EcNewRequiresInspection.HasValue == true)
        {
            exp = exp.And(x => x.EcNewRequiresInspection == queryDto.EcNewRequiresInspection);
        }

        if (!string.IsNullOrEmpty(queryDto?.CultureCode))
        {
            exp = exp.And(x => x.CultureCode != null && x.CultureCode.Contains(queryDto.CultureCode));
        }

        return exp.ToExpression();
    }
}
