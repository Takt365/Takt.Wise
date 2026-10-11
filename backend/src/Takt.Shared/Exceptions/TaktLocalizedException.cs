// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Exceptions
// 文件名称：TaktLocalizedException.cs
// 功能描述：承载翻译键与参数的异常，用于跨层统一本地化
// ========================================

namespace Takt.Shared.Exceptions;

/// <summary>
/// 本地化异常：消息键 + 资源类型 + 格式化参数。
/// </summary>
public class TaktLocalizedException : Exception
{
    /// <summary>
    /// 翻译键（ResourceKey）
    /// </summary>
    public string MessageKey { get; }

    /// <summary>
    /// 资源类型（Frontend/Backend）
    /// </summary>
    public string ResourceType { get; }

    /// <summary>
    /// 格式化参数（string.Format 占位 {0}、{1}）
    /// </summary>
    public object[] Arguments { get; }

    /// <summary>
    /// 字段标签键（entity.* / common.field.*），用于 {field}、{feature} 占位
    /// </summary>
    public string? FieldKey { get; }

    /// <summary>
    /// 命名占位符（如 count、max、name）
    /// </summary>
    public IReadOnlyDictionary<string, string>? NamedTokens { get; }

    /// <summary>
    /// 拼在字段名后的附加值
    /// </summary>
    public object[] FieldExtras { get; }

    /// <summary>
    /// 创建本地化异常（string.Format 参数）
    /// </summary>
    public TaktLocalizedException(string messageKey, string resourceType = "Backend", params object[] arguments)
        : this(messageKey, resourceType, null, null, null, arguments)
    {
    }

    /// <summary>
    /// 创建本地化异常（抽象键 + 字段/命名占位）
    /// </summary>
    public TaktLocalizedException(
        string messageKey,
        string resourceType,
        string? fieldKey,
        IReadOnlyDictionary<string, string>? namedTokens,
        object[]? fieldExtras,
        params object[] formatArguments)
        : base(messageKey)
    {
        MessageKey = messageKey;
        ResourceType = resourceType;
        FieldKey = fieldKey;
        NamedTokens = namedTokens;
        FieldExtras = fieldExtras ?? Array.Empty<object>();
        Arguments = formatArguments ?? Array.Empty<object>();
    }
}
