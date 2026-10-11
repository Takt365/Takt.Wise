// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Output
// 文件名称：TaktOutputDetailDictImportBindings.cs
// 功能描述：产出日报明细 Excel 导入字典绑定（dict_type_code 与前端 dict-type 一致；仅导入旁路，UI 提交由前端转换）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Output;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Logistics.Manufacturing.Output;

/// <summary>
/// 产出日报明细导入字典字段绑定（非 UI 入口；与 views 中 TaktSelect dict-type 对齐）
/// </summary>
internal static class TaktOutputDetailDictImportBindings
{
    /// <summary>
    /// 组立日报明细导入
    /// </summary>
    public static readonly IReadOnlyList<TaktDictFieldStorageBinding> AssyDetail =
    [
        new(nameof(TaktAssyOutputDetailCreateDto.DowntimeReason), "logistics_manufacturing_stop_reason", true),
        new(nameof(TaktAssyOutputDetailCreateDto.UnachievedReason), "logistics_manufacturing_nonachievement_reason", true),
    ];

    /// <summary>
    /// PCBA 日报明细导入
    /// </summary>
    public static readonly IReadOnlyList<TaktDictFieldStorageBinding> PcbaDetail =
    [
        new(nameof(TaktPcbaOutputDetailCreateDto.DowntimeReason), "logistics_manufacturing_stop_reason", true),
        new(nameof(TaktPcbaOutputDetailCreateDto.UnachievedReason), "logistics_manufacturing_nonachievement_reason", true),
        new(nameof(TaktPcbaOutputDetailCreateDto.PcbBoardType), "logistics_manufacturing_pcba_function", false),
    ];
}
