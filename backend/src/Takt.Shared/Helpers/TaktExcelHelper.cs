// ========================================
// 项目名称：节拍智控·TaktWise 
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktExcelHelper.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt Excel导入导出帮助类，基于EPPlus实现通用的Excel文件导入导出功能
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Diagnostics;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.Style;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Xml;
using Takt.Shared.Exceptions;
using Takt.Shared.Options;

namespace Takt.Shared.Helpers;

/// <summary>
/// Excel 导入导出帮助类（基于 EPPlus）。
/// </summary>
/// <remarks>
/// 非纯工具网关：工作簿元数据依赖启动时 Configure 注入的 TaktExcelOptions，
/// 或通过各公开方法的 <c>excelOptions</c> 显式传入；导出文件名时间戳方法依赖当前时间（见 GetTimestampString）。
/// </remarks>
public static class TaktExcelHelper
{
    private static TaktExcelOptions? _configuredOptions;
    /// <summary>
    /// Excel（.xlsx）模板或单文件导出使用的 Content-Type。
    /// </summary>
    public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    /// <summary>
    /// 时间戳格式：年月日时分秒（_yyyyMMddHHmmss），模板下载与导出统一使用
    /// </summary>
    private const string TimestampFormat = "yyyyMMddHHmmss";

    /// <summary>
    /// Excel 标准列宽（与 Excel 新建工作簿默认列宽一致；导出固定使用，❌ 禁止按内容 AutoFit）
    /// </summary>
    public const double ExcelStandardColumnWidth = 8.43d;

    private const string ImportRowLimitExceededKey = TaktValidationI18nKeys.DataImportRowLimitExceeded;
    private const string ImportSheetLimitExceededKey = TaktValidationI18nKeys.DataImportSheetLimitExceeded;
    private const string ImportSheetRowLimitExceededKey = TaktValidationI18nKeys.DataImportSheetRowLimitExceeded;
    private const string ExportSheetLimitExceededKey = TaktValidationI18nKeys.DataExportSheetLimitExceeded;
    private const string ExportSheetRowLimitExceededKey = TaktValidationI18nKeys.DataExportSheetRowLimitExceeded;
    private const string ExcelHelperNotConfiguredKey = TaktValidationI18nKeys.SystemNotConfigured;
    private const string ExportMultiSheetAtLeastOneSheetKey = TaktValidationI18nKeys.Insufficient;
    private const string ExportMultiSheetNameRequiredKey = TaktValidationI18nKeys.Required;
    private const string ExportMultiSheetDataRequiredKey = TaktValidationI18nKeys.Required;
    private const string ExcelSheetNotFoundOrEmptyKey = TaktValidationI18nKeys.NotFound;
    private const string ExcelZipNoFilesKey = TaktValidationI18nKeys.NotFound;

    /// <summary>
    /// 静态构造函数，设置EPPlus许可证
    /// </summary>
    static TaktExcelHelper()
    {
        // EPPlus 8.0+ 设置非商业许可证（官方API）
        // 根据EPPlus 8官方文档：https://www.epplussoftware.com/
        ExcelPackage.License.SetNonCommercialPersonal("TaktWise (TDF) ");
    }

    /// <summary>
    /// 获取当前时间戳字符串（yyyyMMddHHmmss）；输出随调用时刻变化。
    /// </summary>
    private static string GetTimestampString() =>
        DateTime.Now.ToString(TimestampFormat, CultureInfo.InvariantCulture);

    /// <summary>
    /// 去掉文件名中的 .xlsx / .xls / .zip 后缀（前后端约定只传名称，不传后缀）
    /// </summary>
    private static string NormalizeFileNameBase(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return name ?? string.Empty;
        var s = name.Trim();
        const StringComparison cmp = StringComparison.OrdinalIgnoreCase;
        if (s.EndsWith(".xlsx", cmp)) return s[..^5];
        if (s.EndsWith(".xls", cmp)) return s[..^4];
        if (s.EndsWith(".zip", cmp)) return s[..^4];
        return s;
    }

    /// <summary>
    /// 生成带时间戳的 Excel 文件名：名称_yyyyMMddHHmmss.xlsx（前端只传名称，后缀由本方法拼接）
    /// </summary>
    /// <param name="baseFileName">基础文件名（不含后缀，若带 .xlsx/.xls 会自动去掉）</param>
    /// <returns>带时间戳的文件名，如 用户数据_20250129143022.xlsx</returns>
    private static string GenerateTimestampFileName(string? baseFileName)
    {
        var baseName = NormalizeFileNameBase(baseFileName);
        if (string.IsNullOrWhiteSpace(baseName)) baseName = "Data";
        return $"{baseName}_{GetTimestampString()}.xlsx";
    }

    /// <summary>
    /// 生成带时间戳的 Zip 文件名：名称_yyyyMMddHHmmss.zip
    /// </summary>
    private static string GenerateTimestampZipFileName(string? baseFileName)
    {
        var baseName = NormalizeFileNameBase(baseFileName);
        if (string.IsNullOrWhiteSpace(baseName)) baseName = "Data";
        return $"{baseName}_{GetTimestampString()}.zip";
    }

    /// <summary>
    /// 根据服务端生成的最终文件名选择导出 Content-Type（<c>.zip</c> 为分批打包，否则为 Excel）。
    /// </summary>
    /// <param name="fileName">含扩展名的文件名</param>
    /// <returns>HTTP Content-Type</returns>
    /// <exception cref="ArgumentException"><paramref name="fileName"/> 为空</exception>
    public static string GetExportContentType(string fileName)
    {
        ArgumentException.ThrowIfNullOrEmpty(fileName);
        return fileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)
            ? "application/zip"
            : ExcelContentType;
    }

    /// <summary>
    /// 启动期注入 Excel 工作簿元数据（作者、公司等）；应用生命周期内调用一次。
    /// </summary>
    /// <param name="options">Excel 配置选项</param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> 为 null</exception>
    public static void Configure(TaktExcelOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _configuredOptions = options;
        TaktLogger.Information("[TaktExcelHelper] Excel配置已初始化，作者: {Author}, 标题: {Title}", options.Author, options.Title);
    }

    /// <summary>
    /// 解析工作簿元数据配置（显式参数优先，否则使用 Configure 结果）。
    /// </summary>
    /// <param name="excelOptions">可选显式配置</param>
    /// <returns>非 null 的 Excel 配置</returns>
    /// <exception cref="TaktLocalizedException">未 Configure 且未传入 <paramref name="excelOptions"/></exception>
    private static TaktExcelOptions ResolveExcelOptions(TaktExcelOptions? excelOptions)
    {
        if (excelOptions != null)
        {
            return excelOptions;
        }

        if (_configuredOptions != null)
        {
            return _configuredOptions;
        }

        TaktLogger.Error("[TaktExcelHelper] Excel配置未初始化，请在应用程序启动时调用 TaktExcelHelper.Configure() 方法");
        throw new TaktLocalizedException(
            ExcelHelperNotConfiguredKey,
            "Frontend",
            TaktValidationI18nKeys.FieldExcelHelper,
            null,
            null);
    }

    /// <summary>
    /// 解析单文件最大导入行数（<c>Excel:Import:MaxRowsPerFile</c>）
    /// </summary>
    /// <param name="excelOptions">可选显式配置</param>
    /// <returns>最大导入行数</returns>
    private static int ResolveMaxImportRowsPerFile(TaktExcelOptions? excelOptions = null) =>
        ResolveExcelOptions(excelOptions).Import.MaxRowsPerFile;

    /// <summary>
    /// 解析单 Sheet 最大导出行数（<c>Excel:Export:MaxRowsPerSheet</c>）
    /// </summary>
    /// <param name="excelOptions">可选显式配置</param>
    /// <returns>单 Sheet 最大行数</returns>
    private static int ResolveMaxExportRowsPerSheet(TaktExcelOptions? excelOptions = null) =>
        ResolveExcelOptions(excelOptions).Export.MaxRowsPerSheet;

    /// <summary>
    /// 解析单文件最大导入 Sheet 数量（<c>Excel:Import:MaxSheetsPerFile</c>）
    /// </summary>
    /// <param name="excelOptions">可选显式配置</param>
    /// <returns>最大 Sheet 数量</returns>
    private static int ResolveMaxImportSheetsPerFile(TaktExcelOptions? excelOptions = null) =>
        ResolveExcelOptions(excelOptions).Import.MaxSheetsPerFile;

    /// <summary>
    /// 解析单 Sheet 最大导入行数（<c>Excel:Import:MaxRowsPerSheet</c>）
    /// </summary>
    /// <param name="excelOptions">可选显式配置</param>
    /// <returns>单 Sheet 最大导入行数</returns>
    private static int ResolveMaxImportRowsPerSheet(TaktExcelOptions? excelOptions = null) =>
        ResolveExcelOptions(excelOptions).Import.MaxRowsPerSheet;

    /// <summary>
    /// 解析单文件最大导出 Sheet 数量（<c>Excel:Export:MaxSheetsPerFile</c>）
    /// </summary>
    /// <param name="excelOptions">可选显式配置</param>
    /// <returns>最大 Sheet 数量</returns>
    private static int ResolveMaxExportSheetsPerFile(TaktExcelOptions? excelOptions = null) =>
        ResolveExcelOptions(excelOptions).Export.MaxSheetsPerFile;

    /// <summary>
    /// 设置工作簿属性。作者、公司、应用程序等取配置；标题、主题、类别、关键词、备注按本次导出文件与工作表生成，配置值仅在无法从文件识别时兜底。
    /// </summary>
    /// <param name="workbook">目标工作簿</param>
    /// <param name="options">工作簿元数据配置（固定身份与兜底文案）</param>
    /// <param name="actualFileName">本次导出文件名（可含时间戳与扩展名）</param>
    /// <param name="sheetNames">工作表名称</param>
    private static void SetWorkbookProperties(
        ExcelWorkbook workbook,
        TaktExcelOptions options,
        string actualFileName,
        params string[] sheetNames)
    {
        ArgumentNullException.ThrowIfNull(workbook);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(actualFileName);

        var title = ResolveWorkbookTitle(actualFileName, options.Title);
        var sheets = sheetNames
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => RemovePlantTokens(name.Trim()))
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .Distinct(StringComparer.Ordinal)
            .ToList();
        var subject = ResolveWorkbookSubject(title, sheets, options.Subject);
        var category = ResolveWorkbookCategory(options.Category);
        var props = workbook.Properties;
        props.Author = options.Author;
        props.Title = TruncateWorkbookProperty(title);
        props.Subject = TruncateWorkbookProperty(subject);
        props.Category = TruncateWorkbookProperty(category);
        props.Keywords = TruncateWorkbookProperty(BuildWorkbookKeywords(title, subject, category, options.Keywords));
        props.Comments = TruncateWorkbookProperty(BuildWorkbookComments(options.Comments, title));
        props.Status = options.Status;
        props.Application = options.Application;
        props.Company = options.Company;
        props.Manager = options.Manager;
        props.LastModifiedBy = options.Author;
        props.Created = DateTime.Now;
        props.Modified = DateTime.Now;
    }

    /// <summary>
    /// 从导出文件名去掉扩展名和末尾 _yyyyMMddHHmmss，得到文档标题。
    /// </summary>
    /// <param name="actualFileName">导出文件名</param>
    /// <returns>如 C100_采购价格推移_20261009185446.xlsx → C100_采购价格推移</returns>
    private static string DocumentTitleFromFileName(string actualFileName)
    {
        var baseName = NormalizeFileNameBase(actualFileName);
        const int stampLength = 14;
        if (baseName.Length > stampLength + 1
            && baseName[^(stampLength + 1)] == '_'
            && baseName[^stampLength..].All(char.IsDigit))
        {
            baseName = baseName[..^(stampLength + 1)];
        }
        return baseName.Trim();
    }

    /// <summary>
    /// 工厂前缀文件（如 C100_采购价格推移）拆出工厂码与报表名。工厂码为 1 位字母 + 3 位数字。
    /// </summary>
    /// <param name="title">文档标题</param>
    /// <param name="plantCode">工厂码</param>
    /// <param name="reportLabel">报表名</param>
    /// <returns>是否匹配工厂前缀</returns>
    private static bool TrySplitPlantPrefixedTitle(string title, out string plantCode, out string reportLabel)
    {
        plantCode = string.Empty;
        reportLabel = string.Empty;
        var splitAt = title.IndexOf('_');
        if (splitAt != 4 || title.Length <= 5)
        {
            return false;
        }
        var head = title[..4];
        if (!char.IsLetter(head[0]) || !char.IsDigit(head[1]) || !char.IsDigit(head[2]) || !char.IsDigit(head[3]))
        {
            return false;
        }
        var label = title[(splitAt + 1)..].Trim();
        if (string.IsNullOrWhiteSpace(label))
        {
            return false;
        }
        plantCode = head.ToUpperInvariant();
        reportLabel = label;
        return true;
    }

    /// <summary>
    /// 去掉分批导出附在报表名后的 _序号（1～3 位数字）。
    /// </summary>
    /// <param name="label">报表名</param>
    /// <returns>去掉分批序号后的报表名</returns>
    private static string TrimTrailingBatchIndex(string label)
    {
        var splitAt = label.LastIndexOf('_');
        if (splitAt <= 0 || splitAt >= label.Length - 1)
        {
            return label;
        }
        var tail = label[(splitAt + 1)..];
        if (tail.Length <= 3 && tail.All(char.IsDigit))
        {
            return label[..splitAt];
        }
        return label;
    }

    /// <summary>
    /// 标题：报表或实体名。工厂前缀（如 C100_）去掉，不含工厂码。
    /// </summary>
    /// <param name="actualFileName">导出文件名</param>
    /// <param name="fallback">配置兜底</param>
    /// <returns>标题</returns>
    private static string ResolveWorkbookTitle(string actualFileName, string fallback)
    {
        var title = TrimTrailingBatchIndex(DocumentTitleFromFileName(actualFileName));
        if (TrySplitPlantPrefixedTitle(title, out _, out var reportLabel))
        {
            title = TrimTrailingBatchIndex(reportLabel);
        }
        title = RemovePlantTokens(title);
        return string.IsNullOrWhiteSpace(title) ? fallback : title;
    }

    /// <summary>
    /// 主题：工作表名（已去掉工厂码）。没有工作表名时用标题。
    /// </summary>
    /// <param name="title">文档标题</param>
    /// <param name="sheetNames">工作表名称</param>
    /// <param name="fallback">配置兜底</param>
    /// <returns>主题</returns>
    private static string ResolveWorkbookSubject(string title, IReadOnlyList<string> sheetNames, string fallback)
    {
        if (sheetNames.Count > 0)
        {
            return string.Join(", ", sheetNames);
        }
        if (!string.IsNullOrWhiteSpace(title))
        {
            return title;
        }
        return fallback;
    }

    /// <summary>
    /// 类别：调用方服务所在模块路径（菜单目录名，用 | 连接）。识别不到时用配置兜底。
    /// </summary>
    /// <param name="fallback">配置兜底</param>
    /// <returns>类别，如 后勤管理|采购管理</returns>
    private static string ResolveWorkbookCategory(string fallback)
    {
        var category = ResolveCallerModuleCategory();
        return string.IsNullOrWhiteSpace(category) ? fallback : category;
    }

    /// <summary>
    /// 从调用栈取应用服务命名空间，转成模块路径。跳过 Excel 帮助类自身。
    /// </summary>
    /// <returns>模块路径；调用栈上没有应用服务时为空</returns>
    private static string ResolveCallerModuleCategory()
    {
        const string servicePrefix = "Takt.Application.Services.";
        var frames = new StackTrace().GetFrames();
        if (frames == null)
        {
            return string.Empty;
        }
        foreach (var frame in frames)
        {
            var type = frame.GetMethod()?.DeclaringType;
            var ns = type?.Namespace;
            if (string.IsNullOrEmpty(ns) || !ns.StartsWith(servicePrefix, StringComparison.Ordinal))
            {
                continue;
            }
            return TaktNamingHelper.ExportModuleCategory(ns[servicePrefix.Length..]);
        }
        return string.Empty;
    }

    /// <summary>
    /// 去掉文本中的工厂码片段（1 位字母 + 3 位数字，如 C100）。
    /// </summary>
    /// <param name="value">原始文本</param>
    /// <returns>去掉工厂码后的文本</returns>
    private static string RemovePlantTokens(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }
        var tokens = value.Split(['_', ' ', '　'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var kept = tokens.Where(token => !IsPlantCode(token)).ToArray();
        return kept.Length == 0 ? string.Empty : string.Join("_", kept);
    }

    /// <summary>
    /// 是否为工厂码（1 位字母 + 3 位数字）。
    /// </summary>
    /// <param name="token">片段</param>
    /// <returns>是工厂码时为 true</returns>
    private static bool IsPlantCode(string token)
    {
        return token.Length == 4
            && char.IsLetter(token[0])
            && char.IsDigit(token[1])
            && char.IsDigit(token[2])
            && char.IsDigit(token[3]);
    }

    /// <summary>
    /// 关键词：本次标题、主题、类别中的词，再并上配置关键词，按忽略大小写去重。
    /// </summary>
    /// <param name="title">文档标题</param>
    /// <param name="subject">主题</param>
    /// <param name="category">类别</param>
    /// <param name="configuredKeywords">配置关键词</param>
    /// <returns>逗号分隔关键词</returns>
    private static string BuildWorkbookKeywords(string title, string subject, string category, string configuredKeywords)
    {
        var parts = new List<string>();
        AddKeywordTokens(parts, title);
        AddKeywordTokens(parts, subject);
        AddKeywordTokens(parts, category);
        AddKeywordTokens(parts, configuredKeywords);
        return string.Join(",", parts);
    }

    /// <summary>
    /// 按逗号、下划线和空白拆词并去重。纯数字片段（分批序号、时间戳）不进入关键词。
    /// </summary>
    /// <param name="parts">已收集的关键词</param>
    /// <param name="value">待拆分文本</param>
    private static void AddKeywordTokens(List<string> parts, string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }
        var tokens = value.Split([',', '，', '_', '|', ' ', '　'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var token in tokens)
        {
            if (token.All(char.IsDigit) || IsPlantCode(token))
            {
                continue;
            }
            if (parts.Contains(token, StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }
            parts.Add(token);
        }
    }

    /// <summary>
    /// 备注：配置说明后追加报表名。不写入带工厂码的文件名。
    /// </summary>
    /// <param name="configuredComments">配置备注</param>
    /// <param name="documentTitle">报表名（不含工厂码）</param>
    /// <returns>备注</returns>
    private static string BuildWorkbookComments(string configuredComments, string documentTitle)
    {
        if (string.IsNullOrWhiteSpace(configuredComments))
        {
            return documentTitle;
        }
        if (string.IsNullOrWhiteSpace(documentTitle))
        {
            return configuredComments.Trim();
        }
        return $"{configuredComments.Trim()} | {documentTitle}";
    }

    /// <summary>
    /// 工作簿核心属性截断到 255，避免超出 Office 属性常用长度。
    /// </summary>
    /// <param name="value">属性值</param>
    /// <returns>截断后的值</returns>
    private static string TruncateWorkbookProperty(string value)
    {
        const int maxLength = 255;
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
        {
            return value ?? string.Empty;
        }
        return value[..maxLength];
    }

    #region 单Sheet导入导出

    /// <summary>
    /// 导出Excel(单个Sheet，支持超大数据分批导出多个文件)
    /// </summary>
    /// <typeparam name="T">要导出的数据类型</typeparam>
    /// <param name="data">数据集合</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名（仅名称，不含 .xlsx；为空时用 sheetName；后端自动拼接 名称_yyyyMMddHHmmss.xlsx）</param>
    /// <param name="excelOptions">可选显式工作簿元数据；为空时使用 Configure 注入的配置</param>
    /// <returns>最终文件名与内容（fileName 已含时间戳与 .xlsx）</returns>
    public static async Task<(string fileName, byte[] content)> ExportAsync<T>(
        IEnumerable<T> data,
        string sheetName = "Data",
        string? fileName = null,
        TaktExcelOptions? excelOptions = null) where T : class
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentException.ThrowIfNullOrEmpty(sheetName);
        var workbookOptions = ResolveExcelOptions(excelOptions);
        var maxRowsPerSheet = ResolveMaxExportRowsPerSheet(excelOptions);

        try
        {
            var dataList = data.ToList();
            int total = dataList.Count;

            if (total == 0)
            {
                // 生成只有表头的空Excel
                using var package = new ExcelPackage();
                var actualFileName = GenerateTimestampFileName(fileName ?? sheetName);
                SetWorkbookProperties(package.Workbook, workbookOptions, actualFileName, sheetName);
                await Task.Run(() => ExportToSheetAsync(package, dataList, sheetName));
                var content = await package.GetAsByteArrayAsync();
                TaktLogger.Information("[TaktExcelHelper] 导出Excel成功（空数据），Sheet: {SheetName}, 文件名: {FileName}", sheetName, actualFileName);
                return (actualFileName, content);
            }

            if (total <= maxRowsPerSheet)
            {
                // 只生成一个Excel
                using var package = new ExcelPackage();
                var actualFileName = GenerateTimestampFileName(fileName ?? sheetName);
                SetWorkbookProperties(package.Workbook, workbookOptions, actualFileName, sheetName);
                await Task.Run(() => ExportToSheetAsync(package, dataList, sheetName));
                var content = await package.GetAsByteArrayAsync();
                TaktLogger.Information("[TaktExcelHelper] 导出Excel成功，Sheet: {SheetName}, 文件名: {FileName}, 数据行数: {RowCount}", sheetName, actualFileName, total);
                return (actualFileName, content);
            }
            else
            {
                // 超过 MaxRowsPerSheet，分批生成多个 Excel 并打包 zip
                int fileCount = (int)Math.Ceiling(total / (double)maxRowsPerSheet);
                var fileList = new List<(string fileName, byte[] content)>();
                var exportBaseName = NormalizeFileNameBase(fileName ?? sheetName);
                if (string.IsNullOrWhiteSpace(exportBaseName)) exportBaseName = sheetName;
                for (int i = 0; i < fileCount; i++)
                {
                    var batch = dataList.Skip(i * maxRowsPerSheet).Take(maxRowsPerSheet).ToList();
                    using var package = new ExcelPackage();
                    var batchFileName = GenerateTimestampFileName($"{exportBaseName}_{i + 1}");
                    SetWorkbookProperties(package.Workbook, workbookOptions, batchFileName, sheetName);
                    await Task.Run(() => ExportToSheetAsync(package, batch, sheetName));
                    var batchContent = await package.GetAsByteArrayAsync();
                    fileList.Add((batchFileName, batchContent));
                }

                // 打包为zip
                using var ms = new MemoryStream();
                using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    foreach (var (batchFileName, batchContent) in fileList)
                    {
                        var entry = zip.CreateEntry(batchFileName, System.IO.Compression.CompressionLevel.Optimal);
                        using var entryStream = entry.Open();
                        await entryStream.WriteAsync(batchContent, 0, batchContent.Length);
                    }
                }
                ms.Position = 0;
                var zipName = GenerateTimestampZipFileName(fileName ?? sheetName);
                TaktLogger.Information("[TaktExcelHelper] 导出Excel成功（分批打包），Sheet: {SheetName}, 文件名: {FileName}, 总数据行数: {RowCount}, 文件数: {FileCount}", sheetName, zipName, total, fileCount);
                return (zipName, ms.ToArray());
            }
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktExcelHelper] 导出Excel失败，Sheet: {SheetName}, 文件名: {FileName}", sheetName, fileName ?? sheetName);
            throw;
        }
    }

    /// <summary>
    /// 导出动态字典行（定制报表查询结果）。单元格须写入真实 CLR 类型：DateTime/DateOnly→Excel 日期，decimal/int→数值；禁止先 ToString 成文本。
    /// </summary>
    /// <param name="rows">数据行</param>
    /// <param name="columnKeys">列键（与行字典键一致）</param>
    /// <param name="columnLabels">列标题（与 columnKeys 一一对应）</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名（不含扩展名）</param>
    /// <param name="excelOptions">工作簿元数据</param>
    /// <param name="columnWidths">按列键覆盖标准列宽；为空则全部标准宽</param>
    /// <param name="autoFitUnspecifiedColumns">未指定宽度的列按内容自适应；指定宽度的列保持给定宽度</param>
    /// <param name="keepNaturalRowHeight">true 时不按换行抬高行高，全部保持 Excel 默认行高</param>
    /// <returns>文件名与内容</returns>
    public static async Task<(string fileName, byte[] content)> ExportDictionaryRowsAsync(
        IReadOnlyList<IReadOnlyDictionary<string, object?>> rows,
        IReadOnlyList<string> columnKeys,
        IReadOnlyList<string> columnLabels,
        string sheetName = "Data",
        string? fileName = null,
        TaktExcelOptions? excelOptions = null,
        IReadOnlyDictionary<string, double>? columnWidths = null,
        bool autoFitUnspecifiedColumns = false,
        bool keepNaturalRowHeight = false)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(columnKeys);
        ArgumentNullException.ThrowIfNull(columnLabels);
        ArgumentException.ThrowIfNullOrEmpty(sheetName);
        ValidateDictionarySheetColumns(columnKeys, columnLabels);
        var workbookOptions = ResolveExcelOptions(excelOptions);
        using var package = new ExcelPackage();
        var actualFileName = GenerateTimestampFileName(fileName ?? sheetName);
        SetWorkbookProperties(package.Workbook, workbookOptions, actualFileName, sheetName);
        WriteDictionarySheet(
            package,
            sheetName,
            rows,
            columnKeys,
            columnLabels,
            columnWidths,
            autoFitUnspecifiedColumns,
            keepNaturalRowHeight);
        var content = await package.GetAsByteArrayAsync();
        TaktLogger.Information("[TaktExcelHelper] 导出动态报表成功，Sheet: {SheetName}, 文件名: {FileName}, 行数: {RowCount}", sheetName, actualFileName, rows.Count);
        return (actualFileName, content);
    }

    /// <summary>
    /// 导出多个字典行动态 Sheet（列键可按 Sheet 不同）。单表超过 10 万行时按上限拆成多个 xlsx 并打包 zip，行不丢弃
    /// </summary>
    /// <param name="sheets">工作表清单（至少 1 个）</param>
    /// <param name="fileName">文件名（不含扩展名）</param>
    /// <param name="excelOptions">工作簿元数据</param>
    /// <returns>文件名与内容</returns>
    /// <exception cref="ArgumentNullException">sheets 为空</exception>
    /// <exception cref="ArgumentException">Sheet 名称/列为空或列标题数量不一致</exception>
    public static async Task<(string fileName, byte[] content)> ExportDictionarySheetsAsync(
        IReadOnlyList<TaktExcelDictionarySheet> sheets,
        string? fileName = null,
        TaktExcelOptions? excelOptions = null)
    {
        ArgumentNullException.ThrowIfNull(sheets);
        if (sheets.Count == 0)
        {
            throw new ArgumentException("sheets 不能为空", nameof(sheets));
        }
        var workbookOptions = ResolveExcelOptions(excelOptions);
        var maxSheetsPerFile = ResolveMaxExportSheetsPerFile(excelOptions);
        var maxRowsPerSheet = ResolveMaxExportRowsPerSheet(excelOptions);
        foreach (var sheet in sheets)
        {
            ArgumentNullException.ThrowIfNull(sheet);
            ArgumentException.ThrowIfNullOrWhiteSpace(sheet.SheetName);
            ArgumentNullException.ThrowIfNull(sheet.Rows);
            ArgumentNullException.ThrowIfNull(sheet.ColumnKeys);
            ArgumentNullException.ThrowIfNull(sheet.ColumnLabels);
            ValidateDictionarySheetColumns(sheet.ColumnKeys, sheet.ColumnLabels);
        }
        if (sheets.Count > maxSheetsPerFile)
        {
            throw new TaktLocalizedException(
                ExportSheetLimitExceededKey,
                "Frontend",
                null,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["count"] = sheets.Count.ToString(CultureInfo.InvariantCulture),
                    ["max"] = maxSheetsPerFile.ToString(CultureInfo.InvariantCulture),
                },
                null);
        }
        var totalRows = sheets.Sum(sheet => sheet.Rows.Count);
        var exportBaseName = NormalizeFileNameBase(fileName ?? "Data");
        if (string.IsNullOrWhiteSpace(exportBaseName))
        {
            exportBaseName = "Data";
        }
        if (sheets.All(sheet => sheet.Rows.Count <= maxRowsPerSheet))
        {
            var actualFileName = GenerateTimestampFileName(exportBaseName);
            var content = await WriteDictionaryWorkbookAsync(sheets, workbookOptions, actualFileName);
            TaktLogger.Information(
                "[TaktExcelHelper] 导出多Sheet动态报表成功，Sheet数量: {SheetCount}, 总数据行数: {RowCount}, 文件名: {FileName}",
                sheets.Count,
                totalRows,
                actualFileName);
            return (actualFileName, content);
        }
        var files = new List<(string fileName, byte[] content)>();
        var companions = sheets.Where(sheet => sheet.Rows.Count <= maxRowsPerSheet).ToList();
        var partIndex = 0;
        foreach (var sheet in sheets.Where(sheet => sheet.Rows.Count > maxRowsPerSheet))
        {
            var partCount = (int)Math.Ceiling(sheet.Rows.Count / (double)maxRowsPerSheet);
            for (var i = 0; i < partCount; i++)
            {
                partIndex++;
                var chunk = new TaktExcelDictionarySheet
                {
                    SheetName = partCount == 1 ? sheet.SheetName : BuildSplitSheetName(sheet.SheetName, i + 1),
                    Rows = sheet.Rows.Skip(i * maxRowsPerSheet).Take(maxRowsPerSheet).ToList(),
                    ColumnKeys = sheet.ColumnKeys,
                    ColumnLabels = sheet.ColumnLabels,
                    ColumnWidths = sheet.ColumnWidths,
                };
                var workbookSheets = new List<TaktExcelDictionarySheet> { chunk };
                if (partIndex == 1)
                {
                    workbookSheets.AddRange(companions);
                }
                var partName = GenerateTimestampFileName($"{exportBaseName}_{partIndex}");
                var partContent = await WriteDictionaryWorkbookAsync(workbookSheets, workbookOptions, partName);
                files.Add((partName, partContent));
            }
        }
        var packed = await PackToZipAsync(files, exportBaseName);
        TaktLogger.Information(
            "[TaktExcelHelper] 导出多Sheet动态报表成功（超过 {MaxRows} 行，打包 zip），总数据行数: {RowCount}, 文件名: {FileName}, 文件数: {FileCount}",
            maxRowsPerSheet,
            totalRows,
            packed.fileName,
            files.Count);
        return packed;
    }

    /// <summary>
    /// 分表名称：原名加 _序号，总长不超过 Excel 工作表名 31 字符。
    /// </summary>
    /// <param name="sheetName">原工作表名</param>
    /// <param name="part">从 1 开始的分表序号</param>
    /// <returns>分表名</returns>
    private static string BuildSplitSheetName(string sheetName, int part)
    {
        var suffix = $"_{part}";
        var baseName = sheetName.Length + suffix.Length <= 31
            ? sheetName
            : sheetName[..(31 - suffix.Length)];
        return baseName + suffix;
    }

    /// <summary>
    /// 把已切分的字典表写入一个工作簿。
    /// </summary>
    /// <param name="sheets">工作表（每表行数已不超过上限）</param>
    /// <param name="workbookOptions">工作簿元数据</param>
    /// <param name="actualFileName">本次文件名</param>
    /// <returns>xlsx 内容</returns>
    private static async Task<byte[]> WriteDictionaryWorkbookAsync(
        IReadOnlyList<TaktExcelDictionarySheet> sheets,
        TaktExcelOptions workbookOptions,
        string actualFileName)
    {
        using var package = new ExcelPackage();
        SetWorkbookProperties(package.Workbook, workbookOptions, actualFileName, sheets.Select(sheet => sheet.SheetName).ToArray());
        foreach (var sheet in sheets)
        {
            WriteDictionarySheet(
                package,
                sheet.SheetName,
                sheet.Rows,
                sheet.ColumnKeys,
                sheet.ColumnLabels,
                sheet.ColumnWidths);
        }
        return await package.GetAsByteArrayAsync();
    }

    /// <summary>
    /// 导出列一律固定为 Excel 标准列宽（不按单元格内容 AutoFit）
    /// </summary>
    /// <param name="worksheet">工作表</param>
    /// <param name="columnCount">列数（≥1）</param>
    private static void ApplyFixedStandardColumnWidths(ExcelWorksheet worksheet, int columnCount)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        if (columnCount <= 0)
        {
            return;
        }
        worksheet.DefaultColWidth = ExcelStandardColumnWidth;
        for (var i = 1; i <= columnCount; i++)
        {
            worksheet.Column(i).Width = ExcelStandardColumnWidth;
        }
    }

    /// <summary>
    /// 未给出固定宽度的列按表头与单元格内容自适应
    /// </summary>
    /// <param name="worksheet">工作表</param>
    /// <param name="columnKeys">列键顺序</param>
    /// <param name="columnWidths">已指定固定宽度的列键</param>
    private static void ApplyAutoFitUnspecifiedColumns(
        ExcelWorksheet worksheet,
        IReadOnlyList<string> columnKeys,
        IReadOnlyDictionary<string, double>? columnWidths)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(columnKeys);
        for (var i = 0; i < columnKeys.Count; i++)
        {
            if (columnWidths != null
                && columnWidths.TryGetValue(columnKeys[i], out var width)
                && width > 0)
            {
                continue;
            }
            worksheet.Column(i + 1).AutoFit();
        }
    }

    /// <summary>
    /// 按列键覆盖指定列宽（未给出的列保持标准宽或自适应宽）
    /// </summary>
    /// <param name="worksheet">工作表</param>
    /// <param name="columnKeys">列键顺序</param>
    /// <param name="columnWidths">列键→宽度</param>
    private static void ApplyNamedColumnWidths(
        ExcelWorksheet worksheet,
        IReadOnlyList<string> columnKeys,
        IReadOnlyDictionary<string, double>? columnWidths)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(columnKeys);
        if (columnWidths == null || columnWidths.Count == 0)
        {
            return;
        }
        for (var i = 0; i < columnKeys.Count; i++)
        {
            if (columnWidths.TryGetValue(columnKeys[i], out var width) && width > 0)
            {
                worksheet.Column(i + 1).Width = width;
            }
        }
    }

    /// <summary>
    /// 含换行的单元格开启自动换行，并按行内最大行数抬高行高
    /// </summary>
    /// <param name="worksheet">工作表</param>
    private static void ApplyWrapTextForMultilineCells(ExcelWorksheet worksheet)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        if (worksheet.Dimension == null)
        {
            return;
        }
        const double defaultRowHeight = 15d;
        const int maxVisibleLines = 16;
        var endRow = worksheet.Dimension.End.Row;
        var endCol = worksheet.Dimension.End.Column;
        for (var r = 1; r <= endRow; r++)
        {
            var maxLines = 1;
            for (var c = 1; c <= endCol; c++)
            {
                if (worksheet.Cells[r, c].Value is not string text || text.IndexOf('\n') < 0)
                {
                    continue;
                }
                worksheet.Cells[r, c].Style.WrapText = true;
                var lines = 1;
                foreach (var ch in text)
                {
                    if (ch == '\n')
                    {
                        lines = checked(lines + 1);
                    }
                }
                if (lines > maxLines)
                {
                    maxLines = lines;
                }
            }
            if (maxLines > 1)
            {
                var visible = Math.Min(maxLines, maxVisibleLines);
                worksheet.Row(r).Height = defaultRowHeight * visible;
            }
        }
    }

    /// <summary>
    /// 导入Excel(单个Sheet)
    /// </summary>
    /// <typeparam name="T">要导入的数据类型</typeparam>
    /// <param name="fileStream">Excel文件流</param>
    /// <param name="sheetName">工作表名称</param>
    /// <returns>数据集合</returns>
    public static Task<List<T>> ImportAsync<T>(
        Stream fileStream, 
        string sheetName = "Data") where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(fileStream);
        ArgumentException.ThrowIfNullOrEmpty(sheetName);

        try
        {
            using var package = new ExcelPackage(fileStream);
            var result = ImportFromSheet<T>(package, sheetName);
            var maxImportRows = ResolveMaxImportRowsPerFile();
            if (result.Count > maxImportRows)
                throw new TaktLocalizedException(
                    ImportRowLimitExceededKey,
                    "Frontend",
                    null,
                    new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        ["count"] = result.Count.ToString(CultureInfo.InvariantCulture),
                        ["max"] = maxImportRows.ToString(CultureInfo.InvariantCulture),
                    },
                    null);
            TaktLogger.Information("[TaktExcelHelper] 导入Excel成功，Sheet: {SheetName}, 数据类型: {TypeName}, 数据行数: {RowCount}", sheetName, typeof(T).Name, result.Count);
            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktExcelHelper] 导入Excel失败，Sheet: {SheetName}, 数据类型: {TypeName}", sheetName, typeof(T).Name);
            throw;
        }
    }

    #endregion 单Sheet导入导出

    #region 多Sheet导入导出

    /// <summary>
    /// 导出Excel(多个Sheet)
    /// </summary>
    /// <param name="sheets">Sheet数据字典，key为sheet名称，value为数据集合</param>
    /// <param name="excelOptions">可选显式工作簿元数据；为空时使用 Configure 注入的配置</param>
    /// <returns>包含文件名和内容的元组</returns>
    public static async Task<(string fileName, byte[] content)> ExportMultiSheetAsync<T>(
        Dictionary<string, IEnumerable<T>> sheets,
        TaktExcelOptions? excelOptions = null) where T : class
    {
        ArgumentNullException.ThrowIfNull(sheets);
        var workbookOptions = ResolveExcelOptions(excelOptions);
        var maxSheetsPerFile = ResolveMaxExportSheetsPerFile(excelOptions);
        var maxRowsPerSheet = ResolveMaxExportRowsPerSheet(excelOptions);
        if (!sheets.Any())
        {
            throw new TaktLocalizedException(
                ExportMultiSheetAtLeastOneSheetKey,
                "Frontend",
                TaktValidationI18nKeys.FieldSheet,
                null,
                null);
        }

        if (sheets.Count > maxSheetsPerFile)
        {
            throw new TaktLocalizedException(
                ExportSheetLimitExceededKey,
                "Frontend",
                null,
                new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["count"] = sheets.Count.ToString(CultureInfo.InvariantCulture),
                    ["max"] = maxSheetsPerFile.ToString(CultureInfo.InvariantCulture),
                },
                null);
        }

        try
        {
            using var package = new ExcelPackage();

            int totalRows = 0;
            foreach (var sheet in sheets)
            {
                if (string.IsNullOrEmpty(sheet.Key))
                {
                    throw new TaktLocalizedException(
                        ExportMultiSheetNameRequiredKey,
                        "Frontend",
                        TaktValidationI18nKeys.FieldSheetName,
                        null,
                        null);
                }

                if (sheet.Value == null)
                {
                    throw new TaktLocalizedException(
                        ExportMultiSheetDataRequiredKey,
                        "Frontend",
                        TaktValidationI18nKeys.FieldSheetData,
                        null,
                        [sheet.Key]);
                }

                var sheetData = sheet.Value.ToList();
                if (sheetData.Count > maxRowsPerSheet)
                {
                    throw new TaktLocalizedException(
                        ExportSheetRowLimitExceededKey,
                        "Frontend",
                        null,
                        new Dictionary<string, string>(StringComparer.Ordinal)
                        {
                            ["sheet"] = sheet.Key,
                            ["count"] = sheetData.Count.ToString(CultureInfo.InvariantCulture),
                            ["max"] = maxRowsPerSheet.ToString(CultureInfo.InvariantCulture),
                        },
                        null);
                }

                totalRows += sheetData.Count;
                ExportToSheetAsync(package, sheetData, sheet.Key);
            }

            var fileName = GenerateTimestampFileName("多Sheet数据");
            SetWorkbookProperties(package.Workbook, workbookOptions, fileName, sheets.Keys.ToArray());
            var content = await package.GetAsByteArrayAsync();
            TaktLogger.Information("[TaktExcelHelper] 导出多Sheet Excel成功，Sheet数量: {SheetCount}, 总数据行数: {RowCount}, 文件名: {FileName}", sheets.Count, totalRows, fileName);
            return (fileName, content);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktExcelHelper] 导出多Sheet Excel失败，Sheet数量: {SheetCount}", sheets.Count);
            throw;
        }
    }

    /// <summary>
    /// 导入Excel(多个Sheet)
    /// </summary>
    /// <typeparam name="T">要导入的数据类型</typeparam>
    /// <param name="fileStream">Excel文件流</param>
    /// <returns>数据字典，key为sheet名称，value为数据集合</returns>
    public static Task<Dictionary<string, List<T>>> ImportMultiSheetAsync<T>(
        Stream fileStream) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(fileStream);

        try
        {
            using var package = new ExcelPackage(fileStream);
            var result = new Dictionary<string, List<T>>();
            var worksheets = package.Workbook.Worksheets
                .Where(worksheet => worksheet?.Name != null)
                .ToList();
            var maxSheetsPerFile = ResolveMaxImportSheetsPerFile();
            var maxRowsPerSheet = ResolveMaxImportRowsPerSheet();
            var maxImportRows = ResolveMaxImportRowsPerFile();

            if (worksheets.Count > maxSheetsPerFile)
            {
                throw new TaktLocalizedException(
                    ImportSheetLimitExceededKey,
                    "Frontend",
                    null,
                    new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        ["count"] = worksheets.Count.ToString(CultureInfo.InvariantCulture),
                        ["max"] = maxSheetsPerFile.ToString(CultureInfo.InvariantCulture),
                    },
                    null);
            }

            foreach (var worksheet in worksheets)
            {
                var sheetName = worksheet.Name;
                var sheetRows = ImportFromSheet<T>(package, sheetName);
                if (sheetRows.Count > maxRowsPerSheet)
                {
                    throw new TaktLocalizedException(
                        ImportSheetRowLimitExceededKey,
                        "Frontend",
                        null,
                        new Dictionary<string, string>(StringComparer.Ordinal)
                        {
                            ["sheet"] = sheetName,
                            ["count"] = sheetRows.Count.ToString(CultureInfo.InvariantCulture),
                            ["max"] = maxRowsPerSheet.ToString(CultureInfo.InvariantCulture),
                        },
                        null);
                }

                result[sheetName] = sheetRows;
            }

            var totalRows = result.Values.Sum(list => list.Count);
            if (totalRows > maxImportRows)
                throw new TaktLocalizedException(
                    ImportRowLimitExceededKey,
                    "Frontend",
                    null,
                    new Dictionary<string, string>(StringComparer.Ordinal)
                    {
                        ["count"] = totalRows.ToString(CultureInfo.InvariantCulture),
                        ["max"] = maxImportRows.ToString(CultureInfo.InvariantCulture),
                    },
                    null);
            TaktLogger.Information("[TaktExcelHelper] 导入多Sheet Excel成功，Sheet数量: {SheetCount}, 数据类型: {TypeName}, 总数据行数: {RowCount}", result.Count, typeof(T).Name, totalRows);
            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[TaktExcelHelper] 导入多Sheet Excel失败，数据类型: {TypeName}", typeof(T).Name);
            throw;
        }
    }

    #endregion 多Sheet导入导出

    #region 模板导入导出

    /// <summary>
    /// 生成Excel导入模板
    /// </summary>
    /// <typeparam name="T">模板类型</typeparam>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="fileName">文件名（仅名称，不含 .xlsx；为空时用 <paramref name="sheetName"/> + <c>Template</c> 作基础设施兜底，不含业务文案；业务下载应由应用层传入已本地化基名；后端自动拼接 名称_yyyyMMddHHmmss.xlsx）</param>
    /// <param name="excelOptions">可选显式工作簿元数据；为空时使用 Configure 注入的配置</param>
    /// <returns>最终文件名与内容（fileName 已含时间戳与 .xlsx）</returns>
    public static async Task<(string fileName, byte[] content)> GenerateTemplateAsync<T>(
        string sheetName = "Data",
        string? fileName = null,
        TaktExcelOptions? excelOptions = null) where T : class, new()
    {
        ArgumentException.ThrowIfNullOrEmpty(sheetName);
        var workbookOptions = ResolveExcelOptions(excelOptions);
        var baseName = string.IsNullOrWhiteSpace(fileName) ? $"{sheetName}Template" : fileName.Trim();
        var finalFileName = GenerateTimestampFileName(baseName);

        using var package = new ExcelPackage();
        SetWorkbookProperties(package.Workbook, workbookOptions, finalFileName, sheetName);
        var worksheet = package.Workbook.Worksheets.Add(sheetName);

        // 获取属性信息
        var properties = typeof(T).GetProperties()
            .Where(p => p.GetCustomAttribute<BrowsableAttribute>()?.Browsable != false && p.Name != "Id")
            .ToList();

        // 读取XML注释
        var xmlDoc = new XmlDocument();
        string? xmlPath = null;
        try
        {
            var asm = typeof(T).Assembly;
            xmlPath = Path.ChangeExtension(asm.Location, ".xml");
            if (File.Exists(xmlPath))
                xmlDoc.Load(xmlPath);
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "[TaktExcelHelper] 加载类型 XML 注释失败，回退属性名: {TypeName}", typeof(T).Name);
        }

        // 两行表头
        var headers = new string[properties.Count];     // XML注释
        var fields = new string[properties.Count];      // 字段名

        for (int i = 0; i < properties.Count; i++)
        {
            fields[i] = properties[i].Name;
            string summary = properties[i].Name;
            if (xmlDoc.DocumentElement != null)
            {
                var memberName = $"P:{typeof(T).FullName}.{properties[i].Name}";
                var node = xmlDoc.SelectSingleNode($"//member[@name='{memberName}']/summary");
                if (node != null && !string.IsNullOrWhiteSpace(node.InnerText))
                    summary = node.InnerText.Trim();
            }
            headers[i] = summary;
        }

        worksheet.Cells[1, 1].LoadFromArrays(new[] { headers });  // 第一行：XML注释
        worksheet.Cells[2, 1].LoadFromArrays(new[] { fields });   // 第二行：字段名

        // 根据属性类型添加数据验证，从第3行开始
        for (int i = 0; i < properties.Count; i++)
        {
            var propertyType = properties[i].PropertyType;
            var column = worksheet.Cells[3, i + 1, 100, i + 1].Address;

            if (propertyType == typeof(string))
            {
                // 字符串类型添加长度验证
                var validation = worksheet.DataValidations.AddTextLengthValidation(column);
                if (validation != null)
                {
                    validation.ShowErrorMessage = true;
                    validation.Error = "请输入有效的文本";
                    validation.Operator = ExcelDataValidationOperator.between;
                    validation.Formula.Value = 0;
                    validation.Formula2.Value = 255;
                }
            }
            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?)
                || propertyType == typeof(DateOnly) || propertyType == typeof(DateOnly?))
            {
                // 日期类型添加日期验证
                var validation = worksheet.DataValidations.AddDateTimeValidation(column);
                if (validation != null)
                {
                    validation.ShowErrorMessage = true;
                    validation.Error = "请输入有效的日期";
                    validation.Operator = ExcelDataValidationOperator.between;
                    validation.Formula.Value = new DateTime(1900, 1, 1);
                    validation.Formula2.Value = new DateTime(9999, 12, 31, 23, 59, 59);
                }
                var underlying = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                worksheet.Column(i + 1).Style.Numberformat.Format =
                    underlying == typeof(DateOnly) ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm:ss";
            }
            else if (propertyType == typeof(decimal) || propertyType == typeof(decimal?) ||
                     propertyType == typeof(double) || propertyType == typeof(double?) ||
                     propertyType == typeof(float) || propertyType == typeof(float?))
            {
                // 数值类型添加数值验证
                var validation = worksheet.DataValidations.AddDecimalValidation(column);
                if (validation != null)
                {
                    validation.ShowErrorMessage = true;
                    validation.Error = "请输入有效的数值";
                    validation.Operator = ExcelDataValidationOperator.between;
                    validation.Formula.Value = -999999999.0;
                    validation.Formula2.Value = 999999999.0;
                }
                worksheet.Column(i + 1).Style.Numberformat.Format = "#,##0.00";
            }
            else if (propertyType == typeof(int) || propertyType == typeof(int?) ||
                     propertyType == typeof(long) || propertyType == typeof(long?))
            {
                // 整数类型添加整数验证
                var validation = worksheet.DataValidations.AddIntegerValidation(column);
                if (validation != null)
                {
                    validation.ShowErrorMessage = true;
                    validation.Error = "请输入有效的整数";
                    validation.Operator = ExcelDataValidationOperator.between;
                    validation.Formula.Value = -2147483648;
                    validation.Formula2.Value = 2147483647;
                }
                worksheet.Column(i + 1).Style.Numberformat.Format = "#,##0";
            }
        }

        // 模板列宽：固定 Excel 标准列宽（不按内容 AutoFit）
        ApplyFixedStandardColumnWidths(worksheet, properties.Count);

        var content = await package.GetAsByteArrayAsync();
        TaktLogger.Information("[TaktExcelHelper] 生成Excel模板成功，Sheet: {SheetName}, 数据类型: {TypeName}, 文件名: {FileName}", sheetName, typeof(T).Name, finalFileName);
        return (finalFileName, content);
    }

    #endregion 模板导入导出

    #region 私有辅助方法

    /// <summary>
    /// 导出数据到指定Sheet
    /// </summary>
    private static void ExportToSheetAsync<T>(ExcelPackage package, IEnumerable<T> data, string sheetName) where T : class
    {
        ArgumentNullException.ThrowIfNull(package);
        ArgumentNullException.ThrowIfNull(data);
        ArgumentException.ThrowIfNullOrEmpty(sheetName);

        var worksheet = package.Workbook.Worksheets.Add(sheetName);

        // 获取属性信息
        var properties = typeof(T).GetProperties()
            .Where(p => p.GetCustomAttribute<BrowsableAttribute>()?.Browsable != false && p.Name != "Id")
            .ToList();

        // 读取XML注释
        var xmlDoc = new XmlDocument();
        string? xmlPath = null;
        try
        {
            var asm = typeof(T).Assembly;
            xmlPath = Path.ChangeExtension(asm.Location, ".xml");
            if (File.Exists(xmlPath))
                xmlDoc.Load(xmlPath);
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "[TaktExcelHelper] 加载类型 XML 注释失败，回退属性名: {TypeName}", typeof(T).Name);
        }

        // 优化：使用数组存储表头
        var headers = new string[properties.Count];
        for (int i = 0; i < properties.Count; i++)
        {
            // 优先使用XML注释
            string header = properties[i].Name;
            if (xmlDoc.DocumentElement != null)
            {
                var memberName = $"P:{typeof(T).FullName}.{properties[i].Name}";
                var node = xmlDoc.SelectSingleNode($"//member[@name='{memberName}']/summary");
                if (node != null && !string.IsNullOrWhiteSpace(node.InnerText))
                    header = node.InnerText.Trim();
            }
            headers[i] = header;
        }

        // 优化：批量写入表头
        worksheet.Cells[1, 1].LoadFromArrays(new[] { headers });

        // 优化：使用数组批量写入数据
        var dataArray = new List<object[]>();
        foreach (var item in data)
        {
            var row = new object[properties.Count];
            for (int i = 0; i < properties.Count; i++)
            {
                row[i] = NormalizeExportPropertyCellValue(properties[i].GetValue(item));
            }
            dataArray.Add(row);
        }

        if (dataArray.Any())
        {
            worksheet.Cells[2, 1].LoadFromArrays(dataArray);
        }

        // 按属性 CLR 类型 + 样例值套 Excel 格式（纯日期 vs 日期时间）
        for (int i = 0; i < properties.Count; i++)
        {
            var propertyType = Nullable.GetUnderlyingType(properties[i].PropertyType) ?? properties[i].PropertyType;
            var column = worksheet.Column(i + 1);
            if (propertyType == typeof(DateOnly))
            {
                column.Style.Numberformat.Format = "yyyy-MM-dd";
            }
            else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTimeOffset))
            {
                column.Style.Numberformat.Format = ResolveTypedExportDateTimeFormat(dataArray, i);
            }
            else if (propertyType == typeof(decimal) || propertyType == typeof(double) || propertyType == typeof(float))
            {
                column.Style.Numberformat.Format = "#,##0.00";
            }
            else if (propertyType == typeof(int) || propertyType == typeof(long) || propertyType == typeof(short)
                || propertyType == typeof(byte) || propertyType == typeof(uint) || propertyType == typeof(ulong)
                || propertyType == typeof(ushort))
            {
                column.Style.Numberformat.Format = "#,##0";
            }
        }

        // 列宽：固定 Excel 标准列宽（不按内容 AutoFit）
        ApplyFixedStandardColumnWidths(worksheet, properties.Count);

        // 冻结首行
        worksheet.View.FreezePanes(2, 1);
    }

    /// <summary>
    /// 从指定 Sheet 解析行数据（EPPlus 内存访问，无 await 型 I/O；与黄金法则一致：纯内存用同步）。
    /// </summary>
    private static List<T> ImportFromSheet<T>(ExcelPackage package, string sheetName) where T : class, new()
    {
        ArgumentNullException.ThrowIfNull(package);
        ArgumentException.ThrowIfNullOrEmpty(sheetName);

        var worksheet = package.Workbook.Worksheets[sheetName];
        if (worksheet == null || worksheet.Dimension == null)
            throw new TaktLocalizedException(
                ExcelSheetNotFoundOrEmptyKey,
                "Frontend",
                TaktValidationI18nKeys.FieldSheetName,
                null,
                [sheetName]);

        var result = new List<T>();
        var properties = typeof(T).GetProperties()
            .Where(p => p.GetCustomAttribute<BrowsableAttribute>()?.Browsable != false && p.Name != "Id")
            .ToList();

        // 获取表头与属性的映射关系
        var headerMap = new Dictionary<string, PropertyInfo>();
        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
            // 从第二行（字段名）获取表头
            var headerValue = worksheet.Cells[2, col].Value?.ToString();
            if (string.IsNullOrEmpty(headerValue)) continue;

            var property = properties.FirstOrDefault(p => p.Name == headerValue);
            if (property != null)
                headerMap[headerValue] = property;
        }

        // 读取数据（从第3行开始，前两行是表头）
        for (int row = 3; row <= worksheet.Dimension.End.Row; row++)
        {
            var item = new T();
            foreach (var header in headerMap)
            {
                var property = header.Value;
                var col = GetColumnByHeader(worksheet, header.Key);
                if (col == -1) continue;

                var cell = worksheet.Cells[row, col];
                var cellValue = cell.Value?.ToString();
                if (string.IsNullOrEmpty(cellValue)) continue;
                if (!IsRichContentProperty(property.Name) && TaktRegexHelper.ContainsFullWidth(cellValue))
                {
                    throw new TaktLocalizedException(TaktValidationI18nKeys.FullWidthForbidden, "Frontend");
                }

                var value = ConvertValue(cellValue, property.PropertyType);
                if (value != null)
                {
                    property.SetValue(item, value);
                }
            }

            result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// 是否为正文类字段（设变内容、邮件内容、会议议程/纪要等）
    /// </summary>
    /// <param name="name">属性名</param>
    /// <returns>正文类字段时为 true</returns>
    private static bool IsRichContentProperty(string name)
    {
        return name.Equals("content", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("Content", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("Agenda", StringComparison.OrdinalIgnoreCase)
            || name.EndsWith("Minutes", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 根据表头获取列号
    /// </summary>
    private static int GetColumnByHeader(ExcelWorksheet worksheet, string headerText)
    {
        if (worksheet.Dimension == null) return -1;
        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        {
            // 查找第二行（字段名）
            if (worksheet.Cells[2, col].Value?.ToString() == headerText)
                return col;
        }
        return -1;
    }

    /// <summary>
    /// 转换值；无法转换时记录 Debug 并返回 null（调用方跳过该单元格）。
    /// </summary>
    private static object? ConvertValue(string value, Type targetType)
    {
        if (string.IsNullOrEmpty(value)) 
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;

        try
        {
            if (targetType == typeof(string))
                return value;

            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (underlyingType.IsEnum)
                return Enum.Parse(underlyingType, value);

            if (underlyingType == typeof(DateTime))
            {
                return DateTime.Parse(value);
            }

            if (underlyingType == typeof(bool))
            {
                var strValue = value.ToLower();
                return strValue == "是" || strValue == "1" || strValue == "true";
            }

            return Convert.ChangeType(value, underlyingType, CultureInfo.CurrentCulture);
        }
        catch (Exception ex)
        {
            TaktLogger.Debug(ex, "[TaktExcelHelper] 值转换失败，值: {Value}, 目标类型: {TargetType}", value, targetType.Name);
            return null;
        }
    }

    /// <summary>
    /// 将多个Excel文件打包为zip并返回
    /// </summary>
    /// <param name="files">要打包的文件列表</param>
    /// <param name="zipFileName">zip 文件名（仅名称，不含 .zip；后端自动拼接 名称_yyyyMMddHHmmss.zip）</param>
    /// <returns>最终 zip 文件名与内容</returns>
    /// <exception cref="ArgumentNullException"><paramref name="files"/> 为 null</exception>
    /// <exception cref="ArgumentException"><paramref name="files"/> 为空或 <paramref name="zipFileName"/> 为空</exception>
    public static async Task<(string fileName, byte[] content)> PackToZipAsync(
        List<(string fileName, byte[] content)> files,
        string zipFileName)
    {
        ArgumentNullException.ThrowIfNull(files);
        ArgumentException.ThrowIfNullOrWhiteSpace(zipFileName);
        if (files.Count == 0)
            throw new TaktLocalizedException(
                ExcelZipNoFilesKey,
                "Frontend",
                TaktValidationI18nKeys.FieldFile,
                null,
                null);
        
        using var ms = new MemoryStream();
        using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
        {
            foreach (var (fileName, content) in files)
            {
                var entry = zip.CreateEntry(fileName, System.IO.Compression.CompressionLevel.Optimal);
                using var entryStream = entry.Open();
                await entryStream.WriteAsync(content, 0, content.Length);
            }
        }
        ms.Position = 0;
        var zipName = GenerateTimestampZipFileName(zipFileName);
        return (zipName, ms.ToArray());
    }

    /// <summary>
    /// 校验动态报表列键与标题
    /// </summary>
    /// <param name="columnKeys">列键</param>
    /// <param name="columnLabels">列标题</param>
    /// <exception cref="ArgumentException">列为空或数量不一致</exception>
    private static void ValidateDictionarySheetColumns(
        IReadOnlyList<string> columnKeys,
        IReadOnlyList<string> columnLabels)
    {
        if (columnKeys.Count == 0)
        {
            throw new ArgumentException("columnKeys 不能为空", nameof(columnKeys));
        }
        if (columnLabels.Count != columnKeys.Count)
        {
            throw new ArgumentException("columnLabels 必须与 columnKeys 数量一致", nameof(columnLabels));
        }
    }

    /// <summary>
    /// 向工作簿写入一个字典行动态 Sheet
    /// </summary>
    /// <param name="package">工作簿</param>
    /// <param name="sheetName">工作表名称</param>
    /// <param name="rows">数据行</param>
    /// <param name="columnKeys">列键</param>
    /// <param name="columnLabels">列标题</param>
    /// <param name="columnWidths">按列键覆盖列宽</param>
    /// <param name="autoFitUnspecifiedColumns">未指定宽度的列按内容自适应</param>
    /// <param name="keepNaturalRowHeight">true 时不按换行抬高行高</param>
    private static void WriteDictionarySheet(
        ExcelPackage package,
        string sheetName,
        IReadOnlyList<IReadOnlyDictionary<string, object?>> rows,
        IReadOnlyList<string> columnKeys,
        IReadOnlyList<string> columnLabels,
        IReadOnlyDictionary<string, double>? columnWidths,
        bool autoFitUnspecifiedColumns = false,
        bool keepNaturalRowHeight = false)
    {
        ArgumentNullException.ThrowIfNull(package);
        ArgumentException.ThrowIfNullOrEmpty(sheetName);
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(columnKeys);
        ArgumentNullException.ThrowIfNull(columnLabels);
        var worksheet = package.Workbook.Worksheets.Add(sheetName);
        worksheet.Cells[1, 1].LoadFromArrays(new[] { columnLabels.ToArray() });
        if (rows.Count > 0)
        {
            var dataArray = new List<object[]>(rows.Count);
            foreach (var row in rows)
            {
                var line = new object[columnKeys.Count];
                for (var i = 0; i < columnKeys.Count; i++)
                {
                    row.TryGetValue(columnKeys[i], out var cell);
                    line[i] = NormalizeDictionaryCellValue(cell, columnKeys[i]);
                }
                dataArray.Add(line);
            }
            worksheet.Cells[2, 1].LoadFromArrays(dataArray);
        }
        ApplyFixedStandardColumnWidths(worksheet, columnKeys.Count);
        if (autoFitUnspecifiedColumns)
        {
            ApplyAutoFitUnspecifiedColumns(worksheet, columnKeys, columnWidths);
        }
        ApplyNamedColumnWidths(worksheet, columnKeys, columnWidths);
        if (!keepNaturalRowHeight)
        {
            ApplyWrapTextForMultilineCells(worksheet);
        }
        ApplyDictionaryRichTextCells(worksheet, rows, columnKeys);
        ApplyDictionaryTypedColumnFormats(worksheet, rows, columnKeys);
        ApplyDictionaryTrendValueFontColors(worksheet, rows, columnKeys);
        ApplyDictionarySheetFilterAndFreeze(worksheet);
    }

    /// <summary>
    /// 字典 Sheet：表头自动筛选 + 冻结首行（便于全量清单筛部件）
    /// </summary>
    /// <param name="worksheet">工作表</param>
    private static void ApplyDictionarySheetFilterAndFreeze(ExcelWorksheet worksheet)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        if (worksheet.Dimension == null)
        {
            return;
        }
        worksheet.Cells[worksheet.Dimension.Address].AutoFilter = true;
        worksheet.View.FreezePanes(2, 1);
    }

    /// <summary>
    /// 字典 Sheet：按单元格 CLR 类型套 Excel 数字/日期格式（与字段数据类型对齐）
    /// </summary>
    /// <param name="worksheet">工作表</param>
    /// <param name="rows">数据行（须写入 DateTime/DateOnly/数值等真实类型，勿先 ToString）</param>
    /// <param name="columnKeys">列键</param>
    private static void ApplyDictionaryTypedColumnFormats(
        ExcelWorksheet worksheet,
        IReadOnlyList<IReadOnlyDictionary<string, object?>> rows,
        IReadOnlyList<string> columnKeys)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(columnKeys);
        if (rows.Count == 0 || worksheet.Dimension == null)
        {
            return;
        }
        var lastRow = checked(rows.Count + 1);
        for (var c = 0; c < columnKeys.Count; c++)
        {
            var key = columnKeys[c];
            object? sample = null;
            foreach (var row in rows)
            {
                if (!row.TryGetValue(key, out var value) || value == null || value is TaktExcelRichText)
                {
                    continue;
                }
                // 与写入一致：先把 ISO 日期字符串等强制为真实类型再判格式
                sample = CoerceDictionaryCellType(value);
                if (sample == null || sample is TaktExcelRichText || sample is string)
                {
                    continue;
                }
                break;
            }
            if (sample == null || sample is string)
            {
                continue;
            }
            var format = ResolveExcelNumberFormatForValue(sample, key);
            if (string.IsNullOrEmpty(format))
            {
                continue;
            }
            var col = checked(c + 1);
            worksheet.Cells[2, col, lastRow, col].Style.Numberformat.Format = format;
        }
    }

    /// <summary>
    /// 按 CLR 值类型解析 Excel 数字格式（日期 / 年月 / 小数 / 整数 / 比率）
    /// </summary>
    /// <param name="value">样例单元格值（已 Coerce）</param>
    /// <param name="columnKey">列键（比率/年月列名约定）</param>
    /// <returns>EPPlus Numberformat；无需格式时返回 null</returns>
    private static string? ResolveExcelNumberFormatForValue(object value, string columnKey)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(columnKey);
        if (value is DateOnly)
        {
            return "yyyy-MM-dd";
        }
        if (value is DateTime dt)
        {
            return ResolveExcelDateTimeNumberFormat(dt, columnKey);
        }
        if (value is DateTimeOffset dto)
        {
            return ResolveExcelDateTimeNumberFormat(dto.DateTime, columnKey);
        }
        if (value is decimal or double or float)
        {
            if (columnKey.EndsWith("Percent", StringComparison.OrdinalIgnoreCase)
                || string.Equals(columnKey, "adjacentPercent", StringComparison.OrdinalIgnoreCase)
                || string.Equals(columnKey, "adjacent", StringComparison.OrdinalIgnoreCase))
            {
                return "0.00%";
            }
            return "#,##0.000##";
        }
        if (value is int or long or short or byte or uint or ulong or ushort)
        {
            return "#,##0";
        }
        return null;
    }

    /// <summary>
    /// DateTime → Excel 日期/年月/日期时间格式
    /// </summary>
    /// <param name="dateTime">日期时间</param>
    /// <param name="columnKey">列键</param>
    /// <returns>Numberformat</returns>
    private static string ResolveExcelDateTimeNumberFormat(DateTime dateTime, string columnKey)
    {
        if (dateTime.TimeOfDay != TimeSpan.Zero)
        {
            return "yyyy-MM-dd HH:mm:ss";
        }
        if (IsYearMonthExcelColumnKey(columnKey) && dateTime.Day == 1)
        {
            return "yyyy-MM";
        }
        return "yyyy-MM-dd";
    }

    /// <summary>
    /// 年月语义列（非 period_yyyy-MM 数值列）：导出为 yyyy-MM
    /// </summary>
    /// <param name="columnKey">列键</param>
    /// <returns>是否年月列</returns>
    private static bool IsYearMonthExcelColumnKey(string columnKey)
    {
        if (string.Equals(columnKey, "month", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        // basePeriod / comparePeriod / focusPeriod 等；排除 period_2024-01 数值列
        if (columnKey.EndsWith("Period", StringComparison.OrdinalIgnoreCase)
            && !columnKey.StartsWith("period_", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 涨：与列表 text-red-500 对齐
    /// </summary>
    private const int TrendUpFontArgb = unchecked((int)0xFFEF4444);

    /// <summary>
    /// 跌：与列表 text-green-600 对齐
    /// </summary>
    private const int TrendDownFontArgb = unchecked((int)0xFF16A34A);

    /// <summary>
    /// 字典 Sheet：涨跌相关列按正负 / 「涨」「跌」着字体色（保留数值便于筛选）
    /// </summary>
    /// <param name="worksheet">工作表</param>
    /// <param name="rows">原始字典行</param>
    /// <param name="columnKeys">列键</param>
    private static void ApplyDictionaryTrendValueFontColors(
        ExcelWorksheet worksheet,
        IReadOnlyList<IReadOnlyDictionary<string, object?>> rows,
        IReadOnlyList<string> columnKeys)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(columnKeys);
        if (rows.Count == 0)
        {
            return;
        }
        for (var r = 0; r < rows.Count; r++)
        {
            var row = rows[r];
            for (var c = 0; c < columnKeys.Count; c++)
            {
                var key = columnKeys[c];
                if (!IsTrendColorColumnKey(key))
                {
                    continue;
                }
                if (!row.TryGetValue(key, out var value) || value == null || value is TaktExcelRichText)
                {
                    continue;
                }
                var argb = ResolveTrendFontArgbFromCellValue(value);
                if (!argb.HasValue)
                {
                    continue;
                }
                worksheet.Cells[checked(r + 2), checked(c + 1)].Style.Font.Color.SetColor(Color.FromArgb(argb.Value));
            }
        }
    }

    /// <summary>
    /// 是否为需按涨跌着色的列键
    /// </summary>
    /// <param name="key">列键</param>
    /// <returns>需要着色则为 true</returns>
    private static bool IsTrendColorColumnKey(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return false;
        }
        if (string.Equals(key, "adjacent", StringComparison.OrdinalIgnoreCase)
            || string.Equals(key, "adjacentPercent", StringComparison.OrdinalIgnoreCase)
            || string.Equals(key, "varianceAmount", StringComparison.OrdinalIgnoreCase)
            || string.Equals(key, "variancePercent", StringComparison.OrdinalIgnoreCase)
            || string.Equals(key, "trend", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return key.EndsWith("VarianceAmount", StringComparison.OrdinalIgnoreCase)
            || key.EndsWith("VariancePercent", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 由单元格值解析涨跌字体色（正/涨=红，负/跌=绿）
    /// </summary>
    /// <param name="value">单元格原始值</param>
    /// <returns>ARGB；无需着色则为空</returns>
    private static int? ResolveTrendFontArgbFromCellValue(object value)
    {
        switch (value)
        {
            case decimal d:
                return ResolveTrendFontArgbFromSign(d);
            case double db:
                return ResolveTrendFontArgbFromSign((decimal)db);
            case float f:
                return ResolveTrendFontArgbFromSign((decimal)f);
            case int i:
                return ResolveTrendFontArgbFromSign(i);
            case long l:
                return ResolveTrendFontArgbFromSign(l);
            case short s:
                return ResolveTrendFontArgbFromSign(s);
            case string text:
                return ResolveTrendFontArgbFromLabel(text);
            default:
                return null;
        }
    }

    /// <summary>
    /// 按数值正负解析涨跌色
    /// </summary>
    /// <param name="value">有符号数值</param>
    /// <returns>ARGB；零则为空</returns>
    private static int? ResolveTrendFontArgbFromSign(decimal value)
    {
        if (value > 0m)
        {
            return TrendUpFontArgb;
        }
        if (value < 0m)
        {
            return TrendDownFontArgb;
        }
        return null;
    }

    /// <summary>
    /// 按涨跌文案解析字体色
    /// </summary>
    /// <param name="text">涨 / 跌 / up / down 等</param>
    /// <returns>ARGB；非涨跌文案则为空</returns>
    private static int? ResolveTrendFontArgbFromLabel(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }
        var trimmed = text.Trim();
        if (string.Equals(trimmed, "涨", StringComparison.Ordinal)
            || string.Equals(trimmed, "up", StringComparison.OrdinalIgnoreCase))
        {
            return TrendUpFontArgb;
        }
        if (string.Equals(trimmed, "跌", StringComparison.Ordinal)
            || string.Equals(trimmed, "down", StringComparison.OrdinalIgnoreCase))
        {
            return TrendDownFontArgb;
        }
        return null;
    }

    /// <summary>
    /// 字典单元格：富文本先落成纯文字，便于 LoadFromArrays
    /// </summary>
    /// <param name="cell">单元格值</param>
    /// <returns>写入数组的值</returns>
    /// <summary>
    /// 强类型导出：按列内首个 DateTime 样例决定纯日期或日期时间格式
    /// </summary>
    /// <param name="dataArray">已规范化的导出行</param>
    /// <param name="columnIndex">列下标（0 起）</param>
    /// <returns>Numberformat</returns>
    private static string ResolveTypedExportDateTimeFormat(List<object[]> dataArray, int columnIndex)
    {
        ArgumentNullException.ThrowIfNull(dataArray);
        foreach (var row in dataArray)
        {
            if (columnIndex < 0 || columnIndex >= row.Length)
            {
                break;
            }
            var cell = row[columnIndex];
            if (cell is DateTime dt)
            {
                return dt.TimeOfDay == TimeSpan.Zero ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm:ss";
            }
        }
        return "yyyy-MM-dd HH:mm:ss";
    }

    /// <summary>
    /// 强类型导出行：DateOnly / DateTimeOffset / ISO 日期字符串 → EPPlus 可写的 DateTime
    /// </summary>
    /// <param name="value">属性值</param>
    /// <returns>单元格值</returns>
    private static object NormalizeExportPropertyCellValue(object? value)
    {
        var coerced = CoerceDictionaryCellType(value);
        return coerced ?? DBNull.Value;
    }

    /// <summary>
    /// 字典 Sheet：涨跌% 列值为 0 时写空（仅该单元格，不删行）
    /// </summary>
    /// <param name="cell">原始单元格</param>
    /// <param name="columnKey">列键</param>
    /// <returns>写入 Excel 的值</returns>
    private static object NormalizeDictionaryCellValue(object? cell, string columnKey)
    {
        ArgumentNullException.ThrowIfNull(columnKey);
        if (cell is TaktExcelRichText rich)
        {
            return rich.ToPlainText();
        }
        if (IsAnalysisVariancePercentColumnKey(columnKey) && IsNearZeroNumericCell(cell))
        {
            return DBNull.Value;
        }
        return CoerceDictionaryCellType(cell) ?? DBNull.Value;
    }

    /// <summary>
    /// 分析涨跌百分比列（0% 不导出）
    /// </summary>
    /// <param name="columnKey">列键</param>
    /// <returns>是否涨跌%列</returns>
    private static bool IsAnalysisVariancePercentColumnKey(string columnKey)
    {
        if (string.Equals(columnKey, "adjacentPercent", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        if (string.Equals(columnKey, "adjacent", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        if (string.Equals(columnKey, "variancePercent", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        if (string.Equals(columnKey, "modelVariancePercent", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 数值是否接近 0（含 Excel 百分比小数与百分点）
    /// </summary>
    /// <param name="cell">单元格值</param>
    /// <returns>接近 0 则为 true</returns>
    private static bool IsNearZeroNumericCell(object? cell)
    {
        return cell switch
        {
            null => true,
            decimal d => d == 0m,
            double db => Math.Abs(db) < 1e-12,
            float f => Math.Abs(f) < 1e-12f,
            int i => i == 0,
            long l => l == 0L,
            _ => false,
        };
    }

    /// <summary>
    /// 将导出单元格强制为 Excel 可识别的真实类型（DateOnly/DateTimeOffset/ISO 日期字符串→DateTime）
    /// </summary>
    /// <param name="cell">原始值</param>
    /// <returns>强制后的值；null 保持 null</returns>
    private static object? CoerceDictionaryCellType(object? cell)
    {
        if (cell == null)
        {
            return null;
        }
        if (cell is DateOnly dateOnly)
        {
            return dateOnly.ToDateTime(TimeOnly.MinValue);
        }
        if (cell is DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.DateTime;
        }
        if (cell is string text && TryCoerceExcelDateFromString(text, out var parsed))
        {
            return parsed;
        }
        return cell;
    }

    /// <summary>
    /// 仅识别严格 ISO 日期/年月/日期时间字符串，避免业务编码被误当日期
    /// </summary>
    /// <param name="text">原始文本</param>
    /// <param name="date">解析结果</param>
    /// <returns>是否解析成功</returns>
    private static bool TryCoerceExcelDateFromString(string text, out DateTime date)
    {
        date = default;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }
        var trimmed = text.Trim();
        if (trimmed.Length == 10
            && DateTime.TryParseExact(
                trimmed,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
        {
            date = date.Date;
            return true;
        }
        if (trimmed.Length == 7
            && DateTime.TryParseExact(
                trimmed,
                "yyyy-MM",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
        {
            date = new DateTime(date.Year, date.Month, 1);
            return true;
        }
        if (DateTime.TryParseExact(
                trimmed,
                "yyyy-MM-ddTHH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
        {
            return true;
        }
        if (DateTime.TryParseExact(
                trimmed,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 将字典行中的富文本覆盖到已写入的单元格（涨跌百分比着色）
    /// </summary>
    /// <param name="worksheet">工作表</param>
    /// <param name="rows">原始字典行</param>
    /// <param name="columnKeys">列键</param>
    private static void ApplyDictionaryRichTextCells(
        ExcelWorksheet worksheet,
        IReadOnlyList<IReadOnlyDictionary<string, object?>> rows,
        IReadOnlyList<string> columnKeys)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(columnKeys);
        for (var r = 0; r < rows.Count; r++)
        {
            var row = rows[r];
            for (var c = 0; c < columnKeys.Count; c++)
            {
                if (!row.TryGetValue(columnKeys[c], out var value) || value is not TaktExcelRichText rich)
                {
                    continue;
                }
                WriteExcelRichText(worksheet.Cells[checked(r + 2), checked(c + 1)], rich);
            }
        }
    }

    /// <summary>
    /// 写入带字体色的富文本
    /// </summary>
    /// <param name="cell">目标单元格</param>
    /// <param name="rich">富文本</param>
    private static void WriteExcelRichText(ExcelRange cell, TaktExcelRichText rich)
    {
        ArgumentNullException.ThrowIfNull(cell);
        ArgumentNullException.ThrowIfNull(rich);
        cell.Value = null;
        cell.RichText.Clear();
        var hasNewline = false;
        foreach (var run in rich.Runs)
        {
            if (string.IsNullOrEmpty(run.Text))
            {
                continue;
            }
            if (!hasNewline && run.Text.IndexOf('\n') >= 0)
            {
                hasNewline = true;
            }
            var excelRun = cell.RichText.Add(run.Text);
            if (run.FontArgb.HasValue)
            {
                excelRun.Color = Color.FromArgb(run.FontArgb.Value);
            }
        }
        if (hasNewline)
        {
            cell.Style.WrapText = true;
        }
    }

    #endregion 私有辅助方法
}

/// <summary>
/// 字典行动态工作表（多 Sheet 导出；列键可按 Sheet 不同）
/// </summary>
public sealed class TaktExcelDictionarySheet
{
    /// <summary>
    /// 工作表名称
    /// </summary>
    public string SheetName { get; init; } = "Data";

    /// <summary>
    /// 数据行（键与 ColumnKeys 对齐）
    /// </summary>
    public IReadOnlyList<IReadOnlyDictionary<string, object?>> Rows { get; init; } = Array.Empty<IReadOnlyDictionary<string, object?>>();

    /// <summary>
    /// 列键
    /// </summary>
    public IReadOnlyList<string> ColumnKeys { get; init; } = Array.Empty<string>();

    /// <summary>
    /// 列标题（与 ColumnKeys 一一对应）
    /// </summary>
    public IReadOnlyList<string> ColumnLabels { get; init; } = Array.Empty<string>();

    /// <summary>
    /// 按列键覆盖标准列宽
    /// </summary>
    public IReadOnlyDictionary<string, double>? ColumnWidths { get; init; }
}

/// <summary>
/// Excel 单元格富文本（用于同一格内涨跌百分比红/绿着色）
/// </summary>
public sealed class TaktExcelRichText
{
    /// <summary>
    /// 文本片段
    /// </summary>
    public IReadOnlyList<TaktExcelRichRun> Runs { get; init; } = Array.Empty<TaktExcelRichRun>();

    /// <summary>
    /// 拼接为纯文字
    /// </summary>
    /// <returns>纯文字</returns>
    public string ToPlainText()
    {
        if (Runs.Count == 0)
        {
            return string.Empty;
        }
        var builder = new StringBuilder();
        foreach (var run in Runs)
        {
            builder.Append(run.Text);
        }
        return builder.ToString();
    }

    /// <summary>
    /// 纯文字（供 LoadFromArrays 与调试）
    /// </summary>
    /// <returns>纯文字</returns>
    public override string ToString()
    {
        return ToPlainText();
    }

    /// <summary>
    /// 单片段富文本
    /// </summary>
    /// <param name="text">文本</param>
    /// <param name="fontArgb">字体 ARGB；空则默认色</param>
    /// <returns>富文本</returns>
    /// <exception cref="ArgumentNullException">text 为空</exception>
    public static TaktExcelRichText FromSingle(string text, int? fontArgb)
    {
        ArgumentNullException.ThrowIfNull(text);
        return new TaktExcelRichText
        {
            Runs = new[] { new TaktExcelRichRun { Text = text, FontArgb = fontArgb } },
        };
    }
}

/// <summary>
/// Excel 富文本片段
/// </summary>
public sealed class TaktExcelRichRun
{
    /// <summary>
    /// 文本
    /// </summary>
    public string Text { get; init; } = string.Empty;

    /// <summary>
    /// 字体 ARGB（含 alpha）；空则使用 Excel 默认色
    /// </summary>
    public int? FontArgb { get; init; }
}
