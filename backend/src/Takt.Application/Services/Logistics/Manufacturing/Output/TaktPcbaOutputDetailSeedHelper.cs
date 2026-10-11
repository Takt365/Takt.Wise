// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDetailSeedHelper.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报新增时按物料标准工序时间生成子表明细（生产时段=工作中心）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA日报明细默认子表行生成辅助
/// </summary>
internal static class TaktPcbaOutputDetailSeedHelper
{
    /// <summary>
    /// 按标准工序时间工作中心构建默认明细预览列表
    /// </summary>
    /// <param name="operationTimes">标准工序时间列表</param>
    /// <returns>默认明细预览</returns>
    public static List<TaktPcbaOutputDefaultDetailDto> BuildDefaultDetailPreview(
        IReadOnlyList<TaktStandardOperationTime> operationTimes)
    {
        ArgumentNullException.ThrowIfNull(operationTimes);
        if (operationTimes.Count == 0)
        {
            return [];
        }
        var details = new List<TaktPcbaOutputDefaultDetailDto>(operationTimes.Count);
        var lineNumber = 10;
        foreach (var operationTime in operationTimes.OrderBy(x => x.WorkCenter, StringComparer.Ordinal))
        {
            var workCenter = operationTime.WorkCenter?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(workCenter))
            {
                continue;
            }
            details.Add(new TaktPcbaOutputDefaultDetailDto
            {
                LineNumber = lineNumber,
                WorkCenter = workCenter,
                OperationDesc = operationTime.OperationDesc,
                StandardShorts = operationTime.StandardShorts,
                StandardMinutes = operationTime.ConvertedMinutes > 0
                    ? operationTime.ConvertedMinutes
                    : operationTime.StandardMinutes,
            });
            lineNumber += 10;
        }
        return details;
    }

    /// <summary>
    /// 新增 PCBA 日报时确保按工作中心生成子表明细（与客户端已填写的工作中心行合并）
    /// </summary>
    /// <param name="dto">创建 DTO</param>
    /// <param name="operationTimes">标准工序时间列表</param>
    public static void EnsureDefaultDetailsOnCreate(
        TaktPcbaOutputCreateDto dto,
        IReadOnlyList<TaktStandardOperationTime> operationTimes)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(operationTimes);
        if (operationTimes.Count == 0)
        {
            return;
        }
        var submittedByWorkCenter = (dto.PcbaOutputDetails ?? new List<TaktPcbaOutputDetailCreateDto>())
            .Where(d => !string.IsNullOrWhiteSpace(d.TimePeriod))
            .GroupBy(d => d.TimePeriod.Trim(), StringComparer.Ordinal)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);
        var details = new List<TaktPcbaOutputDetailCreateDto>();
        var lineNumber = 10;
        foreach (var operationTime in operationTimes.OrderBy(x => x.WorkCenter, StringComparer.Ordinal))
        {
            var workCenter = operationTime.WorkCenter?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(workCenter))
            {
                continue;
            }
            if (submittedByWorkCenter.TryGetValue(workCenter, out var existing))
            {
                existing.LineNumber = lineNumber;
                if (string.IsNullOrWhiteSpace(existing.ProdOrderCode))
                {
                    existing.ProdOrderCode = dto.ProdOrderCode;
                }
                if (string.IsNullOrWhiteSpace(existing.TenantCode))
                {
                    existing.TenantCode = dto.TenantCode;
                }
                if (string.IsNullOrWhiteSpace(existing.CompanyCode))
                {
                    existing.CompanyCode = dto.CompanyCode;
                }
                if (string.IsNullOrWhiteSpace(existing.CultureCode))
                {
                    existing.CultureCode = dto.CultureCode;
                }
                if (existing.ShiftNo <= 0)
                {
                    existing.ShiftNo = 1;
                }
                existing.TimePeriod = workCenter;
                details.Add(existing);
            }
            else
            {
                details.Add(new TaktPcbaOutputDetailCreateDto
                {
                    TenantCode = dto.TenantCode,
                    CompanyCode = dto.CompanyCode,
                    CultureCode = dto.CultureCode,
                    PcbaOutputId = 0,
                    ProdOrderCode = dto.ProdOrderCode,
                    LineNumber = lineNumber,
                    TimePeriod = workCenter,
                    ShiftNo = 1,
                });
            }
            lineNumber += 10;
        }
        dto.PcbaOutputDetails = details;
    }
}
