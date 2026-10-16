// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktNamingHelper.cs
// 功能描述：通用命名约定：实体类名与 Takt 前缀、Excel 导入导出 sheet/file 默认值、entity.xxx._self / org.dept.* 资源键推导、推移导出 {工厂}_{报表名} 等。
// ========================================

using System.IO;

namespace Takt.Shared.Helpers;

/// <summary>
/// 通用命名辅助（如 Excel 模板/导入/导出：fileName 默认为领域实体类名；sheetName 默认为去掉 <c>Takt</c> 后的英文业务名；与种子 <c>entity.xxx._self</c>、<c>org.dept.{编码}</c> 规则对齐）。
/// </summary>
public static class TaktNamingHelper
{
    /// <summary>
    /// 去掉 <c>Takt</c> 前缀后的类型后缀（如 <c>TaktUser</c> → <c>User</c>），常用作默认工作表业务英文名。
    /// </summary>
    public static string DefaultSheetNameEnglish(string entityTypeName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(entityTypeName);
        return entityTypeName.StartsWith("Takt", StringComparison.Ordinal) && entityTypeName.Length > 4
            ? entityTypeName[4..]
            : entityTypeName;
    }

    /// <summary>
    /// 与实体翻译种子一致的 <c>entity.xxx._self</c> 键（<c>xxx</c> 为 DefaultSheetNameEnglish 结果的全小写）。
    /// </summary>
    /// <param name="entityTypeName">领域实体类名，如 <c>TaktUser</c></param>
    /// <returns>资源键，如 <c>entity.user._self</c></returns>
    /// <exception cref="ArgumentException"><paramref name="entityTypeName"/> 为空</exception>
    public static string EntitySelfResourceKey(string entityTypeName)
    {
        var tail = DefaultSheetNameEnglish(entityTypeName);
        return $"entity.{tail.ToLowerInvariant()}._self";
    }

    /// <summary>
    /// 部门显示名翻译键，与 TaktDeptI18nSeedData 的 org.dept.{编码小写} 一致（如 D0420 → org.dept.d0420）。
    /// </summary>
    /// <param name="deptCode">TaktDept.DeptCode，如 D0420、T000、1000</param>
    /// <returns>org.dept.{编码小写}</returns>
    /// <exception cref="ArgumentException">deptCode 为空，或含非字母数字字符</exception>
    public static string OrgDeptResourceKey(string deptCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deptCode);
        var code = deptCode.Trim().ToLowerInvariant();
        for (var i = 0; i < code.Length; i++)
        {
            if (!char.IsLetterOrDigit(code[i]))
            {
                throw new ArgumentException("部门编码只能包含字母和数字。", nameof(deptCode));
            }
        }
        return $"org.dept.{code}";
    }

    /// <summary>
    /// 与本地化 <c>entity.xxx._self</c> 拼接构成 Excel 导入模板默认文件基名的翻译键（各语种文案为后缀片段，如「导入模板」）。
    /// </summary>
    public const string EntityTemplateNameResourceKey = "entity.template.name";

    /// <summary>
    /// 解析 Excel 模板/导入/导出使用的工作表名与文件基名（不含扩展名；时间戳与扩展名由 Excel 帮助类拼接）。
    /// </summary>
    /// <param name="sheetName">调用方传入的工作表名；空则使用 <paramref name="defaultSheetNameEnglishOverride"/> 或实体默认英文名。</param>
    /// <param name="fileName">调用方传入的文件基名；空则使用 <paramref name="entityTypeName"/>。</param>
    /// <param name="entityTypeName">领域实体类名，如 <c>TaktUser</c>。</param>
    /// <param name="defaultSheetNameEnglishOverride">非标准 CRUD 时可指定默认工作表英文名（如 <c>FlowTodo</c>）。</param>
    public static (string Sheet, string FileBase) ResolveExcelImportExport(
        string? sheetName,
        string? fileName,
        string entityTypeName,
        string? defaultSheetNameEnglishOverride = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(entityTypeName);
        var defaultSheet = string.IsNullOrWhiteSpace(defaultSheetNameEnglishOverride)
            ? DefaultSheetNameEnglish(entityTypeName)
            : defaultSheetNameEnglishOverride.Trim();
        var sheet = string.IsNullOrWhiteSpace(sheetName) ? defaultSheet : sheetName.Trim();
        var file = string.IsNullOrWhiteSpace(fileName) ? entityTypeName : fileName.Trim();
        return (sheet, file);
    }

    /// <summary>
    /// 推移分析报表导出文件基名：{工厂码}_{报表名}（不含扩展名；TaktExcelHelper 再追加 _yyyyMMddHHmmss.xlsx）。
    /// </summary>
    /// <param name="plantCode">选中工厂码，如 C100</param>
    /// <param name="reportLabel">报表短名（建议已本地化）；可带或不带 .xlsx</param>
    /// <returns>如 C100_采购价格推移 / C100_Purchase Price Trend</returns>
    /// <exception cref="ArgumentException">plantCode 或 reportLabel 为空</exception>
    public static string PlantPrefixedTrendExportBase(string plantCode, string reportLabel)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(plantCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(reportLabel);
        var plant = SanitizeFileNameSegment(plantCode.Trim());
        var label = reportLabel.Trim();
        if (label.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            label = label[..^5].TrimEnd();
        }
        label = SanitizeFileNameSegment(label);
        if (string.IsNullOrWhiteSpace(plant) || string.IsNullOrWhiteSpace(label))
        {
            throw new ArgumentException("plantCode / reportLabel 不能为空", nameof(reportLabel));
        }
        return $"{plant}_{label}";
    }

    /// <summary>
    /// 应用服务命名空间尾段转成导出工作簿类别（菜单目录名，| 连接）。与菜单种子 MenuName 一致。
    /// </summary>
    /// <param name="namespaceTail">Takt.Application.Services. 之后的命名空间，如 Logistics.Procurement</param>
    /// <returns>如 后勤管理|采购管理；无法识别时为空</returns>
    /// <exception cref="ArgumentException"><paramref name="namespaceTail"/> 为空</exception>
    public static string ExportModuleCategory(string namespaceTail)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(namespaceTail);
        var names = new List<string>();
        foreach (var segment in namespaceTail.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (ExportModuleMenuNames.TryGetValue(segment, out var menuName)
                && (names.Count == 0 || !string.Equals(names[^1], menuName, StringComparison.Ordinal)))
            {
                names.Add(menuName);
            }
        }
        return string.Join("|", names);
    }

    /// <summary>
    /// 服务目录段 → 菜单目录名。只收模块目录，不收页面名。
    /// </summary>
    private static readonly Dictionary<string, string> ExportModuleMenuNames = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Dashboard"] = "仪表盘",
        ["Routine"] = "日常事务",
        ["Announcement"] = "公告通知",
        ["MeetingCenter"] = "会议中心",
        ["DocumentCenter"] = "文管中心",
        ["NewsCenter"] = "新闻中心",
        ["HelpDesk"] = "服务台",
        ["Visitor"] = "访客中心",
        ["Accounting"] = "财务核算",
        ["Financial"] = "管理会计",
        ["Controlling"] = "控制会计",
        ["Logistics"] = "后勤管理",
        ["Sales"] = "销售管理",
        ["Materials"] = "物料管理",
        ["Procurement"] = "采购管理",
        ["Manufacturing"] = "生产执行",
        ["Quality"] = "质量管理",
        ["CustomerService"] = "客户服务",
        ["Maintenance"] = "工厂维护",
        ["Serial"] = "序列号管理",
        ["Bom"] = "BOM管理",
        ["EngineeringChange"] = "工程设变",
        ["Mds"] = "MDS计划",
        ["Mps"] = "MPS计划",
        ["Mrp"] = "MRP计划",
        ["Aps"] = "APS排程",
        ["Output"] = "OPH管理",
        ["Defect"] = "生产不良",
        ["LaborHour"] = "OPH工数",
        ["Sop"] = "SOP管理",
        ["Operation"] = "质量业务",
        ["Complaint"] = "客诉管理",
        ["Cost"] = "品质成本",
        ["HumanResource"] = "人力资源",
        ["Organization"] = "组织管理",
        ["Personnel"] = "人事管理",
        ["Attendance"] = "考勤管理",
        ["Compensation"] = "薪酬管理",
        ["Benefits"] = "福利管理",
        ["Performance"] = "绩效管理",
        ["Training"] = "教育培训",
        ["Talent"] = "人才管理",
        ["Identity"] = "身份认证",
        ["Workflow"] = "工作流",
        ["Code"] = "代码管理",
        ["Generator"] = "代码生成",
        ["Database"] = "数据库信息",
        ["Foundation"] = "基础数据",
        ["Statistics"] = "统计看板",
        ["QuickQuery"] = "快速查询",
        ["Logging"] = "日志管理",
    };

    /// <summary>
    /// 去除文件名非法字符，空白压成单空格。
    /// </summary>
    private static string SanitizeFileNameSegment(string value)
    {
        var invalid = Path.GetInvalidFileNameChars();
        Span<char> buffer = stackalloc char[value.Length];
        var n = 0;
        var prevSpace = false;
        foreach (var c in value)
        {
            if (c <= 31 || Array.IndexOf(invalid, c) >= 0)
            {
                if (!prevSpace)
                {
                    buffer[n++] = '_';
                    prevSpace = true;
                }
                continue;
            }
            if (char.IsWhiteSpace(c))
            {
                if (!prevSpace && n > 0)
                {
                    buffer[n++] = ' ';
                    prevSpace = true;
                }
                continue;
            }
            buffer[n++] = c;
            prevSpace = false;
        }
        return n == 0 ? string.Empty : new string(buffer[..n]).Trim().Trim('_');
    }
}
