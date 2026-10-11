// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktOutputOrderUniqueHelper.cs
// 创建时间：2026-07-08
// 创建人：Takt365(Cursor AI)
// 功能描述：产出日报类实体唯一性辅助（导入键与自然键比对；组立为 TeamCode+ProdCategory+ProdDate+ProdOrderCode）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 产出日报唯一性辅助（组立：TeamCode + 生产类别 + 生产日期 + 工单号；其它可用工厂维）
/// </summary>
internal static class TaktOutputOrderUniqueHelper
{
    /// <summary>
    /// 构建导入/批处理去重键（工厂 + 生产日期 + 工单号）
    /// </summary>
    /// <param name="plantCode">工厂代码</param>
    /// <param name="prodDate">生产日期</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <returns>去重键</returns>
    public static string BuildImportKey(string plantCode, DateTime prodDate, string prodOrderCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(prodOrderCode);
        return $"{plantCode.Trim()}|{prodDate.Date:yyyy-MM-dd}|{prodOrderCode.Trim()}";
    }

    /// <summary>
    /// 构建组立日报导入去重键（班组 + 生产类别 + 生产日期 + 工单号）
    /// </summary>
    /// <param name="teamCode">生产班组</param>
    /// <param name="prodCategory">生产类别</param>
    /// <param name="prodDate">生产日期</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <returns>去重键</returns>
    public static string BuildTeamDailyOrderImportKey(
        string teamCode,
        string prodCategory,
        DateTime prodDate,
        string prodOrderCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(teamCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(prodCategory);
        ArgumentException.ThrowIfNullOrWhiteSpace(prodOrderCode);
        return $"{teamCode.Trim()}|{prodCategory.Trim()}|{prodDate.Date:yyyy-MM-dd}|{prodOrderCode.Trim()}";
    }

    /// <summary>
    /// 构建导入/批处理去重键（生产日期 + 工单号；无班组/工厂维时使用）
    /// </summary>
    /// <param name="prodDate">生产日期</param>
    /// <param name="prodOrderCode">工单号</param>
    /// <returns>去重键</returns>
    public static string BuildImportKey(DateTime prodDate, string prodOrderCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(prodOrderCode);
        return $"{prodDate.Date:yyyy-MM-dd}|{prodOrderCode.Trim()}";
    }

    /// <summary>
    /// 判断两条记录是否为同一「生产日期 + 工单号」自然键
    /// </summary>
    /// <param name="beforeDate">变更前生产日期</param>
    /// <param name="beforeOrderCode">变更前工单号</param>
    /// <param name="afterDate">变更后生产日期</param>
    /// <param name="afterOrderCode">变更后工单号</param>
    /// <returns>自然键相同为 true</returns>
    public static bool IsSameDailyOrderKey(
        DateTime beforeDate,
        string beforeOrderCode,
        DateTime afterDate,
        string afterOrderCode)
    {
        return beforeDate.Date == afterDate.Date
            && string.Equals(beforeOrderCode?.Trim(), afterOrderCode?.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 判断两条记录是否为同一「工厂 + 生产日期 + 工单号」自然键
    /// </summary>
    /// <param name="beforePlant">变更前工厂</param>
    /// <param name="beforeDate">变更前生产日期</param>
    /// <param name="beforeOrderCode">变更前工单号</param>
    /// <param name="afterPlant">变更后工厂</param>
    /// <param name="afterDate">变更后生产日期</param>
    /// <param name="afterOrderCode">变更后工单号</param>
    /// <returns>自然键相同为 true</returns>
    public static bool IsSamePlantDailyOrderKey(
        string beforePlant,
        DateTime beforeDate,
        string beforeOrderCode,
        string afterPlant,
        DateTime afterDate,
        string afterOrderCode)
    {
        return string.Equals(beforePlant?.Trim(), afterPlant?.Trim(), StringComparison.OrdinalIgnoreCase)
            && IsSameDailyOrderKey(beforeDate, beforeOrderCode, afterDate, afterOrderCode);
    }
}
