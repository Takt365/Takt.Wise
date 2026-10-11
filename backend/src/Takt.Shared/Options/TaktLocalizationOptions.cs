// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Options
// 文件名称：TaktLocalizationOptions.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：本地化配置选项
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Options;

/// <summary>
/// 本地化配置选项
/// </summary>
public class TaktLocalizationOptions
{
    public const string SectionName = "Localization";

    /// <summary>
    /// 多种语言内容通用码默认值（ISO 639-3 <c>mul</c>）
    /// </summary>
    public const string DefaultMultiCultureCode = "mul";

    /// <summary>
    /// 默认语言（appsettings Localization:DefaultCulture；租户无启用语言时兜底）
    /// </summary>
    public string DefaultCulture { get; set; } = "en-US";

    /// <summary>
    /// 多种语言内容通用码（appsettings Localization:MultiCultureCode；字典等业务表 CultureCode 默认；≠ Database:CultureCodes 公司区域映射）
    /// </summary>
    public string MultiCultureCode { get; set; } = DefaultMultiCultureCode;

    /// <summary>
    /// 资源文件路径
    /// </summary>
    public string ResourcesPath { get; set; } = "Resources";

    /// <summary>
    /// 是否从数据库加载翻译（true=数据库，false=resx文件）
    /// </summary>
    public bool UseDatabaseLocalization { get; set; } = true;

    /// <summary>
    /// 规范化 MultiCultureCode（Trim；空则回退 DefaultMultiCultureCode）
    /// </summary>
    public void Normalize()
    {
        MultiCultureCode = string.IsNullOrWhiteSpace(MultiCultureCode)
            ? DefaultMultiCultureCode
            : MultiCultureCode.Trim();
        DefaultCulture = DefaultCulture?.Trim() ?? string.Empty;
        ResourcesPath = ResourcesPath?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// 验证配置
    /// </summary>
    public void Validate()
    {
        Normalize();

        if (string.IsNullOrWhiteSpace(DefaultCulture))
        {
            throw new InvalidOperationException($"{SectionName}:DefaultCulture 不能为空");
        }

        if (string.IsNullOrWhiteSpace(MultiCultureCode))
        {
            throw new InvalidOperationException($"{SectionName}:MultiCultureCode 不能为空");
        }

        if (MultiCultureCode.Length > 5)
        {
            throw new InvalidOperationException($"{SectionName}:MultiCultureCode 长度不能超过 5（与 culture_code 列 Length=5 对齐）");
        }

        if (string.IsNullOrWhiteSpace(ResourcesPath))
        {
            throw new InvalidOperationException($"{SectionName}:ResourcesPath 不能为空");
        }
    }
}
