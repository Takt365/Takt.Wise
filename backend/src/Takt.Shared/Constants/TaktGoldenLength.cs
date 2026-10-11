// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktGoldenLength.cs
// 功能描述：业务实体字段黄金长度约定（Code/Name 统一 40）。
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 业务实体字段黄金长度（排除 RBAC、ABAC 关联表、字典、i18n 实体及固定格式基础设施编码）。
/// </summary>
public static class TaktGoldenLength
{
    /// <summary>
    /// 业务 <c>*Code</c> / <c>*Name</c> 列默认 varchar/nvarchar 长度。
    /// </summary>
    public const int CodeName = 40;
}
