// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktZeroCharHelper.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：零宽字符：Remark 全隐 Encode/Decode；可见名称字间 U+200B、英文词间 U+200C
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text;

namespace Takt.Shared.Helpers;

/// <summary>
/// 零宽字符工具（无状态）。
/// <para>Remark：UTF-8 比特流，U+200B=0、U+200C=1，可 Decode 还原。</para>
/// <para>可见名称：InterleaveDisplayName 中文/日文逐字 U+200B，英文按词 U+200C。</para>
/// </summary>
public static class TaktZeroCharHelper
{
    /// <summary>
    /// 零宽空格（比特 0 / 中文等逐字分隔）。
    /// </summary>
    public const char ZeroBit = '\u200B';

    /// <summary>
    /// 零宽非连接符（比特 1 / 英文等按词分隔）。
    /// </summary>
    public const char OneBit = '\u200C';

    /// <summary>
    /// 明文：节拍科技有限公司（中文法定公司名场景）。
    /// </summary>
    public const string PlainCompanyNameZh = "节拍科技有限公司";

    /// <summary>
    /// 明文：Takt Technologies Co., Ltd.（英文法定公司名场景）。
    /// </summary>
    public const string PlainCompanyNameEn = "Takt Technologies Co., Ltd.";

    /// <summary>
    /// 明文：Takt（短品牌场景）。
    /// </summary>
    public const string PlainBrandTakt = "Takt";

    /// <summary>
    /// 明文：Takt365（产品/站点品牌场景）。
    /// </summary>
    public const string PlainBrandTakt365 = "Takt365";

    /// <summary>
    /// 零宽水印：节拍科技有限公司。
    /// 场景：中文公司法定名、对内中文主体标识。
    /// </summary>
    public static readonly string CompanyNameZh = Encode(PlainCompanyNameZh);

    /// <summary>
    /// 零宽水印：Takt Technologies Co., Ltd.。
    /// 场景：英文公司法定名、对外英文主体标识。
    /// </summary>
    public static readonly string CompanyNameEn = Encode(PlainCompanyNameEn);

    /// <summary>
    /// 零宽水印：Takt。
    /// 场景：短品牌、通用平台标记。
    /// </summary>
    public static readonly string BrandTakt = Encode(PlainBrandTakt);

    /// <summary>
    /// 零宽水印：Takt365。
    /// 场景：产品名、站点、租户/环境类备注。
    /// </summary>
    public static readonly string BrandTakt365 = Encode(PlainBrandTakt365);

    /// <summary>
    /// 为可见名称插入零宽分隔：含 CJK/假名则逐字 U+200B，否则英文按空白分词 U+200C（无空格则逐字）。
    /// 例：节拍科技有限公司 → 节\u200B拍\u200B技…；Takt Technologies Co., Ltd. → Takt\u200CTechnologies\u200C…
    /// </summary>
    /// <param name="text">明文名称</param>
    /// <returns>带零宽分隔的显示名</returns>
    /// <exception cref="ArgumentException"><paramref name="text"/> 为空</exception>
    public static string InterleaveDisplayName(string text)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);
        var plain = StripInterleaved(text);
        if (plain.Length <= 1)
        {
            return plain;
        }

        return ContainsCjkOrKana(plain)
            ? InterleaveChars(plain, ZeroBit)
            : InterleaveLatinWords(plain, OneBit);
    }

    /// <summary>
    /// 去除名称中已插入的 U+200B/U+200C，得到明文。
    /// </summary>
    /// <param name="text">可能含零宽分隔的名称</param>
    /// <returns>明文；空输入返回原值</returns>
    public static string StripInterleaved(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text ?? string.Empty;
        }

        return text.Replace(ZeroBit.ToString(), string.Empty, StringComparison.Ordinal)
            .Replace(OneBit.ToString(), string.Empty, StringComparison.Ordinal);
    }

    /// <summary>
    /// 在字符串每个字符之间插入零宽分隔符。
    /// </summary>
    /// <param name="text">明文</param>
    /// <param name="separator">零宽分隔符（通常为 U+200B）</param>
    /// <returns>插分隔后的字符串</returns>
    /// <exception cref="ArgumentException"><paramref name="text"/> 为空</exception>
    public static string InterleaveChars(string text, char separator)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);
        var plain = StripInterleaved(text);
        if (plain.Length <= 1)
        {
            return plain;
        }

        var sb = new StringBuilder(checked(plain.Length * 2 - 1));
        for (var i = 0; i < plain.Length; i++)
        {
            if (i > 0)
            {
                sb.Append(separator);
            }

            sb.Append(plain[i]);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 英文名称按空白分词，词间插入零宽分隔符；单词则逐字插入。
    /// </summary>
    /// <param name="text">明文</param>
    /// <param name="separator">零宽分隔符（通常为 U+200C）</param>
    /// <returns>插分隔后的字符串</returns>
    /// <exception cref="ArgumentException"><paramref name="text"/> 为空</exception>
    public static string InterleaveLatinWords(string text, char separator)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);
        var plain = StripInterleaved(text);
        var parts = plain.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length <= 1)
        {
            return InterleaveChars(plain, separator);
        }

        return string.Join(separator, parts);
    }

    /// <summary>
    /// 将文本按 UTF-8 编码为零宽字符串（Remark 全隐水印，可 Decode 还原）。
    /// </summary>
    /// <param name="text">待编码明文（支持中文与 ASCII）</param>
    /// <returns>仅含 U+200B/U+200C 的水印字符串</returns>
    /// <exception cref="ArgumentException"><paramref name="text"/> 为空</exception>
    public static string Encode(string text)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);
        var bytes = Encoding.UTF8.GetBytes(text);
        var sb = new StringBuilder(checked(bytes.Length * 8));
        foreach (var b in bytes)
        {
            for (var bit = 7; bit >= 0; bit--)
            {
                sb.Append(((b >> bit) & 1) == 0 ? ZeroBit : OneBit);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 尝试将零宽字符串按 UTF-8 解码为明文。
    /// </summary>
    /// <param name="watermark">仅含 U+200B/U+200C 且长度为 8 倍数的字符串</param>
    /// <param name="text">解码结果；失败时为 null</param>
    /// <returns>是否解码成功</returns>
    public static bool TryDecode(string? watermark, out string? text)
    {
        text = null;
        if (string.IsNullOrEmpty(watermark) || watermark.Length % 8 != 0)
        {
            return false;
        }

        var bytes = new byte[watermark.Length / 8];
        for (var i = 0; i < bytes.Length; i++)
        {
            var value = 0;
            for (var bit = 0; bit < 8; bit++)
            {
                var zc = watermark[checked(i * 8 + bit)];
                if (zc == OneBit)
                {
                    value = (value << 1) | 1;
                }
                else if (zc == ZeroBit)
                {
                    value <<= 1;
                }
                else
                {
                    return false;
                }
            }

            bytes[i] = (byte)value;
        }

        try
        {
            text = Encoding.UTF8.GetString(bytes);
            return true;
        }
        catch (DecoderFallbackException)
        {
            text = null;
            return false;
        }
    }

    /// <summary>
    /// 将零宽字符串按 UTF-8 解码为明文。
    /// </summary>
    /// <param name="watermark">仅含 U+200B/U+200C 且长度为 8 倍数的字符串</param>
    /// <returns>解码后的明文</returns>
    /// <exception cref="ArgumentException"><paramref name="watermark"/> 非法</exception>
    public static string Decode(string watermark)
    {
        ArgumentException.ThrowIfNullOrEmpty(watermark);
        if (!TryDecode(watermark, out var text) || text == null)
        {
            throw new ArgumentException("零宽水印格式非法（须为 U+200B/U+200C 且长度为 8 的倍数，且为合法 UTF-8）。", nameof(watermark));
        }

        return text;
    }

    /// <summary>
    /// 是否含 CJK 汉字或日文假名（用于选择逐字 U+200B 策略）。
    /// </summary>
    /// <param name="text">待检测文本</param>
    /// <returns>含 CJK/假名则为 true</returns>
    private static bool ContainsCjkOrKana(string text)
    {
        foreach (var ch in text)
        {
            if (IsCjkUnified(ch) || IsHiragana(ch) || IsKatakana(ch))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsCjkUnified(char ch) => ch is >= '\u4E00' and <= '\u9FFF';

    private static bool IsHiragana(char ch) => ch is >= '\u3040' and <= '\u309F';

    private static bool IsKatakana(char ch) => ch is >= '\u30A0' and <= '\u30FF';
}
