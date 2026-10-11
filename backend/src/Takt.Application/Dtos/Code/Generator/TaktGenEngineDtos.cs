// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Code.Generator
// 文件名称：TaktGenEngineDtos.cs
// 创建时间：2026-06-02
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成引擎 DTO（生成结果、预览校验、生成/预览请求）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Application.Dtos;
using Takt.Shared.Models;
using Takt.Application.Dtos.Code.Generator;

namespace Takt.Application.Dtos.Code.Generator;

/// <summary>
/// 代码生成结果：文件名（或相对路径） -> 生成后的内容
/// </summary>
public partial class TaktCodeGenResultDto
{
    /// <summary>
    /// 生成的文件名或相对路径（如 Entity.cs、Dto.cs）
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 生成后的代码/文本内容
    /// </summary>
    public string Content { get; set; } = string.Empty;
}

/// <summary>
/// 代码预览文件：目标路径、渲染内容、目标路径是否已存在
/// </summary>
public partial class TaktCodeGenPreviewFileDto
{
    /// <summary>
    /// 目标相对路径（如 backend/src/Takt.Domain/Entities/Identity/TaktUser.cs）
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// 渲染后的代码/文本内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 目标路径下文件是否已存在（仅 GenMethod=1/2 且路径可解析时有效）
    /// </summary>
    public bool IsExisting { get; set; }
}

/// <summary>
/// 预览模板校验问题：记录模板键、目标路径与错误信息
/// </summary>
public partial class TaktCodeGenPreviewValidationIssueDto
{
    /// <summary>
    /// 模板键（如 Backend/Crud/Csharp/Dto.cs）
    /// </summary>
    public string TemplateKey { get; set; } = string.Empty;

    /// <summary>
    /// 解析后的目标相对路径（可能为空）
    /// </summary>
    public string? TargetPath { get; set; }

    /// <summary>
    /// 校验错误信息（模板解析失败、模板渲染失败等）
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// 预览渲染结果：包含可预览文件与模板校验问题
/// </summary>
public partial class TaktCodeGenPreviewResultDto
{
    /// <summary>
    /// 预览渲染是否通过（无校验问题则为 true）
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// 渲染成功的预览文件列表
    /// </summary>
    public List<TaktCodeGenPreviewFileDto> PreviewFiles { get; set; } = [];

    /// <summary>
    /// 模板校验问题列表（按模板逐项记录）
    /// </summary>
    public List<TaktCodeGenPreviewValidationIssueDto> ValidationIssues { get; set; } = [];
}

/// <summary>
/// 从数据库导入表请求（有表导入）：TenantCode、表名，可选表配置覆盖。
/// 注意：无表配置请直接使用 TaktGenTable 标准 CRUD 接口创建。
/// </summary>
public partial class TaktImportTableFromDatabaseRequestDto
{
    /// <summary>
    /// 租户编码（3 位，对应 appsettings Database:TenantCodes）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 要导入的数据表名
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表配置覆盖（可选，用于补充实体类名、业务名等）
    /// </summary>
    public TaktGenTableCreateDto? TableOverrides { get; set; }
}

/// <summary>
/// 代码生成表是否生成代码DTO
/// </summary>
public partial class TaktIsGenCodeDto
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public TaktIsGenCodeDto()
    {
    }

    /// <summary>
    /// 代码生成表配置ID（适配字段，序列化为string以避免Javascript精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(SqlSugar.ValueToStringConverter))]
    public long GenTableId { get; set; }

    /// <summary>
    /// 是否生成代码（0=否，1=是）
    /// </summary>
    public int IsGenCode { get; set; }
}

/// <summary>
/// 从实体初始化表请求DTO（无表流程）
/// </summary>
public partial class TaktInitializeTableFromEntityRequestDto
{
    /// <summary>
    /// 租户编码（3 位，对应 appsettings Database:TenantCodes）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 实体类型全名（如 Takt.Domain.Entities.Code.Generator.TaktGenTable）
    /// </summary>
    public string EntityTypeFullName { get; set; } = string.Empty;
}

/// <summary>
/// 代码生成结果（含 GenMethod 交付：0=zip 字节；1/2=落盘路径与文件列表）
/// </summary>
public partial class TaktCodeGenGenerateResultDto
{
    /// <summary>
    /// 生成方式（0=zip，1=自定义路径，2=当前项目）
    /// </summary>
    public int GenMethod { get; set; }

    /// <summary>
    /// zip 内容（GenMethod=0，由控制器直接 File 响应）
    /// </summary>
    public byte[]? ZipBytes { get; set; }

    /// <summary>
    /// zip 文件名（GenMethod=0）
    /// </summary>
    public string? ZipFileName { get; set; }

    /// <summary>
    /// 落盘根路径（GenMethod=1/2）
    /// </summary>
    public string? BasePath { get; set; }

    /// <summary>
    /// 已写入相对路径（GenMethod=1/2）
    /// </summary>
    public List<string> WrittenFilePaths { get; set; } = [];

    /// <summary>
    /// 文件数
    /// </summary>
    public int FileCount { get; set; }
}

/// <summary>
/// 代码生成请求DTO
/// </summary>
public partial class TaktGenerateCodeRequestDto
{
    /// <summary>
    /// 模板字典：模板键 → Scriban模板内容
    /// 例如：{ "Entity.cs": "{{ for col in Columns }}..." }
    /// </summary>
    public Dictionary<string, string> Templates { get; set; } = new();

    /// <summary>
    /// 生成方式覆盖（可空，默认取表配置 GenMethod）
    /// </summary>
    public int? GenMethod { get; set; }

    /// <summary>
    /// 目标根路径覆盖（GenMethod=1 时使用；GenMethod=2 由服务端解析仓库根）
    /// </summary>
    public string? GenPath { get; set; }
}

/// <summary>
/// 代码预览请求DTO
/// </summary>
public partial class TaktPreviewCodeRequestDto
{
    /// <summary>
    /// 模板字典：模板键 → Scriban模板内容
    /// </summary>
    public Dictionary<string, string> Templates { get; set; } = new();

    /// <summary>
    /// 路径映射：模板键 → 目标相对路径
    /// 例如：{ "Backend/Crud/Csharp/Entity.cs": "src/Takt.Domain/Entities/TaktGenTable.cs" }
    /// </summary>
    public Dictionary<string, string>? PathMappings { get; set; }

    /// <summary>
    /// 目标根路径（可空；为空时不检查是否已存在）
    /// </summary>
    public string? TargetBasePath { get; set; }
}
