// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktConvertConstants.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：下游单据转换进度常量；ConvertedStatus 字段共用字典类型码
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 下游单据转换进度常量（与字典 sys_convert_status 对齐）
/// </summary>
public static class TaktConvertConstants
{
    /// <summary>
    /// 转换进度字典类型码（ConvertedStatus；采购申请/询价、销售/生产/采购计划等共用）
    /// </summary>
    public const string SysConvertStatusDictType = "sys_convert_status";
}
