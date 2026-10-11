// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Generator.GenEngine
// 文件名称：ITaktGenWorkflowService.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成工作流服务接口，支持有表/无表两条流程，均通过 ITaktGenEngine 生成后端/前端代码
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Code.Generator;

namespace Takt.Application.Services.Code.Generator.GenEngine;

/// <summary>
/// 代码生成工作流服务接口
/// </summary>
/// <remarks>
/// 两大流程：
/// 1. 数据表存在：通过 TaktDatabaseInfos 选表 → 导入为 TaktGenTable + TaktGenTableColumn → 根据模板生成代码。
/// 2. 数据表不存在：创建 TaktGenTable + TaktGenTableColumn → 生成代码 → 按实体初始化数据表。
/// </remarks>
public interface ITaktGenWorkflowService
{
    /// <summary>
    /// 从数据库导入指定表：读取表及列元数据，写入 TaktGenTable、TaktGenTableColumn（用于“数据表存在”流程：导入）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="tableName">要导入的数据表名</param>
    /// <param name="tableOverrides">表配置覆盖（可选，用于补充实体类名、业务名等）</param>
    /// <returns>导入后的表配置 DTO（含表 ID，可用于后续生成代码）</returns>
    Task<TaktGenTableDto> ImportTableFromDatabaseAsync(string tenantCode, string tableName, TaktGenTableCreateDto? tableOverrides = null);

    /// <summary>
    /// 从数据库同步指定表的列元数据：已存在列更新库表字段属性，新增列插入，库表已删除列物理移除（保留用户生成配置）
    /// </summary>
    /// <param name="tableId">代码生成表配置 ID</param>
    /// <returns>同步后的表配置 DTO（含列列表）</returns>
    Task<TaktGenTableDto> SyncTableColumnsFromDatabaseAsync(long tableId);

    /// <summary>
    /// 按实体类型初始化数据表（无表流程：代码生成后，手动指定实体类型全名）。与项目内 TaktTableInitializer 一致，使用 SqlSugar CodeFirst.InitTables。
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="entityTypeFullName">实体类型全名（如 Takt.Domain.Entities.Code.Generator.TaktGenTable，对应生成的实体文件中的类）</param>
    /// <returns>任务</returns>
    Task InitializeTableFromEntityTypeAsync(string tenantCode, string entityTypeFullName);

    /// <summary>
    /// 获取可用于“按实体初始化表”的实体类型全名列表（Domain 中三档实体基类派生类型）。
    /// </summary>
    /// <returns>实体类型全名列表</returns>
    Task<IReadOnlyList<string>> GetAvailableEntityTypeFullNamesAsync();

    /// <summary>
    /// 根据表配置与模板生成代码，并按 GenMethod 交付：0=zip；1=自定义路径；2=当前项目落盘。
    /// </summary>
    /// <param name="tableId">代码生成表配置 ID</param>
    /// <param name="request">生成请求（模板、GenMethod、GenPath 可覆盖表配置）</param>
    /// <param name="sqlCreateBy">生成 SQL 时写入 create_by</param>
    /// <returns>生成交付结果</returns>
    Task<TaktCodeGenGenerateResultDto> GenerateCodeAsync(long tableId, TaktGenerateCodeRequestDto request, string? sqlCreateBy = null);

    /// <summary>
    /// 根据表配置与模板映射渲染预览文件（目标相对路径 + 内容 + 是否已存在），仅用于模板正确性校验，不执行落盘生成。
    /// </summary>
    /// <param name="tableId">代码生成表配置 ID</param>
    /// <param name="templates">模板键（如 "Backend/Crud/Csharp/Entity.cs"）→ Scriban 模板内容</param>
    /// <param name="resolveTargetRelativePath">根据模板键解析目标相对路径（可空，为空时使用内置规则）</param>
    /// <param name="targetBasePath">目标根路径（可空；为空时不检查是否已存在）</param>
    /// <param name="sqlCreateBy">生成 SQL 时写入 create_by 的当前登录用户名（可空）</param>
    /// <param name="pathMappings">路径映射：模板键 → 目标相对路径（可空，优先级高于内置规则）</param>
    /// <returns>预览渲染结果（成功文件 + 校验问题）</returns>
    Task<TaktCodeGenPreviewResultDto> GeneratePreviewFilesAsync(
        long tableId,
        IReadOnlyDictionary<string, string> templates,
        Func<TaktGenTableDto, string, string?>? resolveTargetRelativePath = null,
        string? targetBasePath = null,
        string? sqlCreateBy = null,
        IReadOnlyDictionary<string, string>? pathMappings = null);
}
