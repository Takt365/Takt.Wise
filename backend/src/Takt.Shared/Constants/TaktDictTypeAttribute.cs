// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktDictTypeAttribute.cs
// 创建时间：2026-06-11
// 创建人：Takt365(Cursor AI)
// 功能描述：标注 DTO 显示名字段对应的字典类型编码（值须与种子/库表 dict_type_code 一致，勿建平行常量类）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 字典类型标注（用于导出/显示名字段；值为 takt_foundation_dict_type.dict_type_code，如 sys_user_type；全量字典以库表为准，禁止维护 TaktDictTypeCodes 式硬编码子集）
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class TaktDictTypeAttribute : Attribute
{
    /// <summary>
    /// 字典类型编码
    /// </summary>
    public string DictTypeCode { get; }

    /// <summary>
    /// 初始化字典类型标注
    /// </summary>
    /// <param name="dictTypeCode">字典类型编码（与 TaktDictType.DictTypeCode 一致）</param>
    /// <exception cref="ArgumentException"><paramref name="dictTypeCode"/> 为空</exception>
    public TaktDictTypeAttribute(string dictTypeCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(dictTypeCode);
        DictTypeCode = dictTypeCode.Trim();
    }
}
