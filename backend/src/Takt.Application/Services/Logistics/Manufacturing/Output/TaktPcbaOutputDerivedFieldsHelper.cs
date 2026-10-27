// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktPcbaOutputDerivedFieldsHelper.cs
// 创建时间：2026-07-06
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA日报明细派生字段计算（标准工时/标准点数/人员与设备标准产能、投入工数、达成率）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Services.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Bom;
using Takt.Domain.Entities.Logistics.Manufacturing.Mps;
using Takt.Domain.Entities.Logistics.Manufacturing.Output;
using Takt.Domain.Repositories;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// PCBA日报明细派生字段计算辅助
/// </summary>
internal static class TaktPcbaOutputDerivedFieldsHelper
{
    /// <summary>
    /// 设备稼动率时间类别：天
    /// </summary>
    private const int EquipmentOperationRateTimeCategoryDay = 1;

    /// <summary>
    /// 按工作中心解析标准工时(分钟)
    /// </summary>
    /// <param name="operationTimes">标准工时列表</param>
    /// <param name="workCenter">工作中心（生产时段）</param>
    /// <returns>标准工时(分钟)</returns>
    public static decimal CalculateStdMinutesForWorkCenter(
        IReadOnlyList<TaktStandardOperationTime> operationTimes,
        string workCenter)
    {
        ArgumentNullException.ThrowIfNull(operationTimes);
        var row = ResolveOperationTimeForWorkCenter(operationTimes, workCenter);
        if (row == null)
        {
            return 0;
        }
        return row.ConvertedMinutes > 0 ? row.ConvertedMinutes : row.StandardMinutes;
    }

    /// <summary>
    /// 按工作中心解析标准点数
    /// </summary>
    /// <param name="operationTimes">标准工时列表</param>
    /// <param name="workCenter">工作中心（生产时段）</param>
    /// <returns>标准点数</returns>
    public static int CalculateStdShortsForWorkCenter(
        IReadOnlyList<TaktStandardOperationTime> operationTimes,
        string workCenter)
    {
        ArgumentNullException.ThrowIfNull(operationTimes);
        var row = ResolveOperationTimeForWorkCenter(operationTimes, workCenter);
        return row?.StandardShorts ?? 0;
    }

    /// <summary>
    /// 回填明细标准工时/标准点数并计算人员与设备标准产能及工数派生字段
    /// </summary>
    /// <param name="standardOperationTimeRepository">标准工时仓储</param>
    /// <param name="standardOperationRateRepository">标准生产稼动率仓储</param>
    /// <param name="equipmentOperationRateRepository">机器稼动率仓储</param>
    /// <param name="master">PCBA日报主表</param>
    /// <param name="detail">PCBA日报明细</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <returns>任务</returns>
    public static async Task ApplyDerivedFieldsToDetailAsync(
        ITaktApprovalRepository<TaktStandardOperationTime> standardOperationTimeRepository,
        ITaktCompanyRepository<TaktStandardOperationRate> standardOperationRateRepository,
        ITaktCompanyRepository<TaktEquipmentOperationRate> equipmentOperationRateRepository,
        TaktPcbaOutput master,
        TaktPcbaOutputDetail detail,
        string tenantCode,
        string companyCode)
    {
        ArgumentNullException.ThrowIfNull(master);
        ArgumentNullException.ThrowIfNull(detail);
        var operationTimes = await TaktAssyOutputDerivedFieldsHelper.ResolveStandardOperationTimesByMaterialAsync(
            standardOperationTimeRepository,
            tenantCode,
            companyCode,
            master.MaterialCode,
            master.PlantCode,
            master.ProdDate);
        detail.StdMinutes = CalculateStdMinutesForWorkCenter(operationTimes, detail.TimePeriod);
        detail.StdShorts = CalculateStdShortsForWorkCenter(operationTimes, detail.TimePeriod);
        var personnelRatePercent = await TaktAssyOutputDerivedFieldsHelper.ResolvePersonnelOperationRatePercentAsync(
            standardOperationRateRepository,
            tenantCode,
            companyCode,
            master.PlantCode,
            master.ProdDate);
        detail.StdLaborCapacity = TaktProductionStatHelper.CalculateAssyStdCapacity(
            detail.DirectLabor,
            detail.StdMinutes,
            personnelRatePercent);
        var equipmentRatePercent = await ResolveEquipmentOperationRatePercentAsync(
            equipmentOperationRateRepository,
            tenantCode,
            companyCode,
            master.PlantCode,
            master.ProdDate,
            detail.ProdEquipCode,
            detail.ShiftNo);
        detail.StdEquipmentCapacity = TaktProductionStatHelper.CalculateAssyStdCapacity(
            1,
            detail.StdMinutes,
            equipmentRatePercent);
        TaktPcbaOutputDetailDerivedFieldsHelper.ApplyLaborDerivedFields(detail);
    }

    /// <summary>
    /// 按生产日期解析设备时间稼动率（%）；未匹配到记录时返回 0
    /// </summary>
    /// <param name="equipmentOperationRateRepository">机器稼动率仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="prodDate">生产日期</param>
    /// <param name="EquipCode">设备编码</param>
    /// <param name="shiftNo">班次</param>
    /// <returns>时间稼动率(%)</returns>
    public static async Task<decimal> ResolveEquipmentOperationRatePercentAsync(
        ITaktCompanyRepository<TaktEquipmentOperationRate> equipmentOperationRateRepository,
        string tenantCode,
        string companyCode,
        string plantCode,
        DateTime prodDate,
        string EquipCode,
        int shiftNo)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(companyCode);
        if (string.IsNullOrWhiteSpace(plantCode) || string.IsNullOrWhiteSpace(EquipCode))
        {
            return 0;
        }
        var prodDateOnly = prodDate.Date;
        var normalizedEquipCode = EquipCode.Trim();
        var row = await equipmentOperationRateRepository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.PlantCode == plantCode.Trim()
            && x.EquipCode == normalizedEquipCode
            && x.ShiftNo == shiftNo
            && x.TimeCategory == EquipmentOperationRateTimeCategoryDay
            && x.StartDate <= prodDateOnly
            && x.EndDate >= prodDateOnly);
        return row?.EquipmentOperationRate ?? 0;
    }

    /// <summary>
    /// 从标准工时列表中解析指定工作中心最新有效版本
    /// </summary>
    /// <param name="operationTimes">标准工时列表</param>
    /// <param name="workCenter">工作中心</param>
    /// <returns>有效标准工时；未匹配时返回 null</returns>
    private static TaktStandardOperationTime? ResolveOperationTimeForWorkCenter(
        IReadOnlyList<TaktStandardOperationTime> operationTimes,
        string workCenter)
    {
        var normalizedWorkCenter = workCenter?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedWorkCenter))
        {
            return null;
        }
        return TaktStandardOperationTimeEffectiveResolver.SelectLatestPerWorkCenter(operationTimes)
            .FirstOrDefault(x => string.Equals(x.WorkCenter?.Trim(), normalizedWorkCenter, StringComparison.OrdinalIgnoreCase));
    }
}
