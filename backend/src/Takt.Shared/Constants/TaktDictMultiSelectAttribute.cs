// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktDictMultiSelectAttribute.cs
// 功能描述：标注字典字段为多选（逗号分隔）；与 TaktDictTypeAttribute 合用，落库走 TaktDictMultiValueHelper
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 多选字典字段标注（须同时标注 TaktDictTypeAttribute）
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class TaktDictMultiSelectAttribute : Attribute
{
}
