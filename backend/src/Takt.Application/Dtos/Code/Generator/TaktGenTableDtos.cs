// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Code.Generator
// 文件名称：TaktGenTableDtos.cs
// 创建时间：2026-08-22
// 创建人：Takt365(Auto Generated)
// 功能描述：GenTable 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktGenTable 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Code.Generator;

// ========================================
// GenTable 响应 DTO
// ========================================

/// <summary>
/// Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// 对应前端 TaktGenTableDto
/// 继承 TaktTenantCoreDtoBase
/// </summary>
public class TaktGenTableDto : TaktTenantCoreDtoBase
{
    /// <summary>
    /// GenTableID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableId { get; set; }

    /// <summary>
    /// 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
    /// </summary>
    public string DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; } = string.Empty;

    /// <summary>
    /// 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
    /// </summary>
    public string? SubTableName { get; set; } = string.Empty;

    /// <summary>
    /// 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
    /// </summary>
    public string? SubTableFkName { get; set; } = string.Empty;

    /// <summary>
    /// 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeCode { get; set; } = string.Empty;

    /// <summary>
    /// 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeParentCode { get; set; } = string.Empty;

    /// <summary>
    /// 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeName { get; set; } = string.Empty;

    /// <summary>
    /// 库表标识（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int InDatabase { get; set; } = 0;

    /// <summary>
    /// 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
    /// </summary>
    public string GenTemplateCategory { get; set; } = string.Empty;

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    public string GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 功能名（用于接口与注释的中文名称，如：公司、部门）
    /// </summary>
    public string? GenFunctionName { get; set; } = string.Empty;

    /// <summary>
    /// 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
    /// </summary>
    public string PermsPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
    /// </summary>
    public string? MenuButtonGroup { get; set; } = string.Empty;

    /// <summary>
    /// 命名空间前缀（用于生成类名、方法名等的前缀）
    /// </summary>
    public string? NamePrefix { get; set; } = string.Empty;

    /// <summary>
    /// 实体命名空间（默认当前项目：Takt.Domain.Entities）
    /// </summary>
    public string? EntityNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 实体类名称（首字母大写，驼峰命名）
    /// </summary>
    public string EntityClassName { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
    /// </summary>
    public string? DtoNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象 Dto 类名
    /// </summary>
    public string? DtoClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务命名空间（默认当前项目：Takt.Application.Services）
    /// </summary>
    public string? ServiceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 服务接口类名称
    /// </summary>
    public string? IServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类名称
    /// </summary>
    public string? ServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
    /// </summary>
    public string? ControllerNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 控制器类名称
    /// </summary>
    public string? ControllerClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储层（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsRepository { get; set; } = 0;

    /// <summary>
    /// 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
    /// </summary>
    public string? RepositoryInterfaceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储接口类名称
    /// </summary>
    public string? IRepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
    /// </summary>
    public string? RepositoryNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储类名称
    /// </summary>
    public string? RepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
    /// </summary>
    public string? GenFunction { get; set; } = string.Empty;

    /// <summary>
    /// 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
    /// </summary>
    public int GenMethod { get; set; } = 0;

    /// <summary>
    /// 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
    /// </summary>
    public string GenPath { get; set; } = string.Empty;

    /// <summary>
    /// 生成菜单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsGenMenu { get; set; } = 0;

    /// <summary>
    /// 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentMenuId { get; set; }

    /// <summary>
    /// 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
    /// </summary>
    public string? ParentMenuName { get; set; }

    /// <summary>
    /// 生成翻译（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsGenTranslation { get; set; } = 0;

    /// <summary>
    /// 排序字段（选项本表 columnList.databaseColumnName）
    /// </summary>
    public string SortField { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
    /// </summary>
    public string SortType { get; set; } = string.Empty;

    /// <summary>
    /// 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
    /// </summary>
    public int FrontUi { get; set; } = 0;

    /// <summary>
    /// 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
    /// </summary>
    public int FrontFormLayout { get; set; } = 0;

    /// <summary>
    /// 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
    /// </summary>
    public int FrontBtnStyle { get; set; } = 0;

    /// <summary>
    /// 是否生成（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsGenCode { get; set; } = 0;

    /// <summary>
    /// 代码生成次数（每次生成成功后自增）
    /// </summary>
    public int GenCodeCount { get; set; } = 0;

    /// <summary>
    /// 使用tabs（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsUseTabs { get; set; } = 0;

    /// <summary>
    /// tabs标签中字段的数量
    /// </summary>
    public int TabsFieldCount { get; set; } = 0;

    /// <summary>
    /// 作者
    /// </summary>
    public string GenAuthor { get; set; } = string.Empty;

    /// <summary>
    /// 其他生成选项（JSON格式，存储其他生成配置）
    /// </summary>
    public string? OtherGenOptions { get; set; } = string.Empty;

    /// <summary>
    /// 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）
    /// （子表：TaktGenTableColumn）
    /// </summary>
    public List<TaktGenTableColumnDto>? Columns { get; set; }

    /// <summary>
    /// 子表列配置最大行号（含已作废行，供前端续编行号）
    /// </summary>
    public int MaxGenTableColumnLineNumber { get; set; }

}

// ========================================
// GenTable 查询 DTO
// ========================================

/// <summary>
/// GenTable 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktGenTableQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
    /// </summary>
    public string? DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; } = string.Empty;

    /// <summary>
    /// 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
    /// </summary>
    public string? SubTableName { get; set; } = string.Empty;

    /// <summary>
    /// 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
    /// </summary>
    public string? SubTableFkName { get; set; } = string.Empty;

    /// <summary>
    /// 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeCode { get; set; } = string.Empty;

    /// <summary>
    /// 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeParentCode { get; set; } = string.Empty;

    /// <summary>
    /// 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeName { get; set; } = string.Empty;

    /// <summary>
    /// 库表标识（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? InDatabase { get; set; }

    /// <summary>
    /// 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
    /// </summary>
    public string? GenTemplateCategory { get; set; } = string.Empty;

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    public string? GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 功能名（用于接口与注释的中文名称，如：公司、部门）
    /// </summary>
    public string? GenFunctionName { get; set; } = string.Empty;

    /// <summary>
    /// 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
    /// </summary>
    public string? PermsPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
    /// </summary>
    public string? MenuButtonGroup { get; set; } = string.Empty;

    /// <summary>
    /// 命名空间前缀（用于生成类名、方法名等的前缀）
    /// </summary>
    public string? NamePrefix { get; set; } = string.Empty;

    /// <summary>
    /// 实体命名空间（默认当前项目：Takt.Domain.Entities）
    /// </summary>
    public string? EntityNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 实体类名称（首字母大写，驼峰命名）
    /// </summary>
    public string? EntityClassName { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
    /// </summary>
    public string? DtoNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象 Dto 类名
    /// </summary>
    public string? DtoClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务命名空间（默认当前项目：Takt.Application.Services）
    /// </summary>
    public string? ServiceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 服务接口类名称
    /// </summary>
    public string? IServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类名称
    /// </summary>
    public string? ServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
    /// </summary>
    public string? ControllerNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 控制器类名称
    /// </summary>
    public string? ControllerClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储层（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsRepository { get; set; }

    /// <summary>
    /// 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
    /// </summary>
    public string? RepositoryInterfaceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储接口类名称
    /// </summary>
    public string? IRepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
    /// </summary>
    public string? RepositoryNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储类名称
    /// </summary>
    public string? RepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
    /// </summary>
    public string? GenFunction { get; set; } = string.Empty;

    /// <summary>
    /// 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
    /// </summary>
    public int? GenMethod { get; set; }

    /// <summary>
    /// 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
    /// </summary>
    public string? GenPath { get; set; } = string.Empty;

    /// <summary>
    /// 生成菜单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsGenMenu { get; set; }

    /// <summary>
    /// 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentMenuId { get; set; }

    /// <summary>
    /// 生成翻译（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsGenTranslation { get; set; }

    /// <summary>
    /// 排序字段（选项本表 columnList.databaseColumnName）
    /// </summary>
    public string? SortField { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
    /// </summary>
    public string? SortType { get; set; } = string.Empty;

    /// <summary>
    /// 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
    /// </summary>
    public int? FrontUi { get; set; }

    /// <summary>
    /// 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
    /// </summary>
    public int? FrontFormLayout { get; set; }

    /// <summary>
    /// 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
    /// </summary>
    public int? FrontBtnStyle { get; set; }

    /// <summary>
    /// 是否生成（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsGenCode { get; set; }

    /// <summary>
    /// 代码生成次数（每次生成成功后自增）
    /// </summary>
    public int? GenCodeCount { get; set; }

    /// <summary>
    /// 使用tabs（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsUseTabs { get; set; }

    /// <summary>
    /// tabs标签中字段的数量
    /// </summary>
    public int? TabsFieldCount { get; set; }

    /// <summary>
    /// 作者
    /// </summary>
    public string? GenAuthor { get; set; } = string.Empty;

    /// <summary>
    /// 其他生成选项（JSON格式，存储其他生成配置）
    /// </summary>
    public string? OtherGenOptions { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建GenTable DTO
// ========================================

/// <summary>
/// 创建GenTable DTO
/// </summary>
public class TaktGenTableCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
    /// </summary>
    [Required(ErrorMessage = "数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）不能为空")]
    public string DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
    /// </summary>
    [Required(ErrorMessage = "表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）不能为空")]
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; } = string.Empty;

    /// <summary>
    /// 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
    /// </summary>
    public string? SubTableName { get; set; } = string.Empty;

    /// <summary>
    /// 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
    /// </summary>
    public string? SubTableFkName { get; set; } = string.Empty;

    /// <summary>
    /// 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeCode { get; set; } = string.Empty;

    /// <summary>
    /// 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeParentCode { get; set; } = string.Empty;

    /// <summary>
    /// 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeName { get; set; } = string.Empty;

    /// <summary>
    /// 库表标识（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int InDatabase { get; set; } = 0;

    /// <summary>
    /// 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
    /// </summary>
    [Required(ErrorMessage = "生成模板类型（字典 code_generator_template_type；crud/sub/tree）不能为空")]
    public string GenTemplateCategory { get; set; } = string.Empty;

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    [Required(ErrorMessage = "业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）不能为空")]
    public string GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 功能名（用于接口与注释的中文名称，如：公司、部门）
    /// </summary>
    public string? GenFunctionName { get; set; } = string.Empty;

    /// <summary>
    /// 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
    /// </summary>
    [Required(ErrorMessage = "权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。不能为空")]
    public string PermsPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
    /// </summary>
    public string? MenuButtonGroup { get; set; } = string.Empty;

    /// <summary>
    /// 命名空间前缀（用于生成类名、方法名等的前缀）
    /// </summary>
    public string? NamePrefix { get; set; } = string.Empty;

    /// <summary>
    /// 实体命名空间（默认当前项目：Takt.Domain.Entities）
    /// </summary>
    public string? EntityNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 实体类名称（首字母大写，驼峰命名）
    /// </summary>
    [Required(ErrorMessage = "实体类名称（首字母大写，驼峰命名）不能为空")]
    public string EntityClassName { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
    /// </summary>
    public string? DtoNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象 Dto 类名
    /// </summary>
    public string? DtoClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务命名空间（默认当前项目：Takt.Application.Services）
    /// </summary>
    public string? ServiceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 服务接口类名称
    /// </summary>
    public string? IServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类名称
    /// </summary>
    public string? ServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
    /// </summary>
    public string? ControllerNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 控制器类名称
    /// </summary>
    public string? ControllerClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储层（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsRepository { get; set; } = 0;

    /// <summary>
    /// 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
    /// </summary>
    public string? RepositoryInterfaceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储接口类名称
    /// </summary>
    public string? IRepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
    /// </summary>
    public string? RepositoryNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储类名称
    /// </summary>
    public string? RepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
    /// </summary>
    public string? GenFunction { get; set; } = string.Empty;

    /// <summary>
    /// 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
    /// </summary>
    public int GenMethod { get; set; } = 0;

    /// <summary>
    /// 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
    /// </summary>
    [Required(ErrorMessage = "生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）不能为空")]
    public string GenPath { get; set; } = string.Empty;

    /// <summary>
    /// 生成菜单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsGenMenu { get; set; } = 0;

    /// <summary>
    /// 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentMenuId { get; set; }

    /// <summary>
    /// 生成翻译（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsGenTranslation { get; set; } = 0;

    /// <summary>
    /// 排序字段（选项本表 columnList.databaseColumnName）
    /// </summary>
    [Required(ErrorMessage = "排序字段（选项本表 columnList.databaseColumnName）不能为空")]
    public string SortField { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
    /// </summary>
    [Required(ErrorMessage = "排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）不能为空")]
    public string SortType { get; set; } = string.Empty;

    /// <summary>
    /// 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
    /// </summary>
    public int FrontUi { get; set; } = 0;

    /// <summary>
    /// 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
    /// </summary>
    public int FrontFormLayout { get; set; } = 0;

    /// <summary>
    /// 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
    /// </summary>
    public int FrontBtnStyle { get; set; } = 0;

    /// <summary>
    /// 是否生成（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsGenCode { get; set; } = 0;

    /// <summary>
    /// 代码生成次数（每次生成成功后自增）
    /// </summary>
    public int GenCodeCount { get; set; } = 0;

    /// <summary>
    /// 使用tabs（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsUseTabs { get; set; } = 0;

    /// <summary>
    /// tabs标签中字段的数量
    /// </summary>
    public int TabsFieldCount { get; set; } = 0;

    /// <summary>
    /// 作者
    /// </summary>
    [Required(ErrorMessage = "作者不能为空")]
    public string GenAuthor { get; set; } = string.Empty;

    /// <summary>
    /// 其他生成选项（JSON格式，存储其他生成配置）
    /// </summary>
    public string? OtherGenOptions { get; set; } = string.Empty;

    /// <summary>
    /// 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）（子表，级联保存）
    /// </summary>
    public List<TaktGenTableColumnCreateDto>? Columns { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新GenTable DTO
// ========================================

/// <summary>
/// 更新GenTable DTO
/// 继承 TaktGenTableCreateDto，添加 GenTableId 字段
/// </summary>
public class TaktGenTableUpdateDto : TaktGenTableCreateDto
{
    /// <summary>
    /// GenTableID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableId { get; set; }

    /// <summary>
    /// 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）（子表，级联保存）
    /// </summary>
    public new List<TaktGenTableColumnUpdateDto>? Columns { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// GenTable 导入模板行 DTO
/// </summary>
public class TaktGenTableTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
    /// </summary>
    public string? DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; } = string.Empty;

    /// <summary>
    /// 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
    /// </summary>
    public string? SubTableName { get; set; } = string.Empty;

    /// <summary>
    /// 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
    /// </summary>
    public string? SubTableFkName { get; set; } = string.Empty;

    /// <summary>
    /// 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeCode { get; set; } = string.Empty;

    /// <summary>
    /// 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeParentCode { get; set; } = string.Empty;

    /// <summary>
    /// 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeName { get; set; } = string.Empty;

    /// <summary>
    /// 库表标识（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? InDatabase { get; set; }

    /// <summary>
    /// 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
    /// </summary>
    public string? GenTemplateCategory { get; set; } = string.Empty;

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    public string? GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 功能名（用于接口与注释的中文名称，如：公司、部门）
    /// </summary>
    public string? GenFunctionName { get; set; } = string.Empty;

    /// <summary>
    /// 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
    /// </summary>
    public string? PermsPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
    /// </summary>
    public string? MenuButtonGroup { get; set; } = string.Empty;

    /// <summary>
    /// 命名空间前缀（用于生成类名、方法名等的前缀）
    /// </summary>
    public string? NamePrefix { get; set; } = string.Empty;

    /// <summary>
    /// 实体命名空间（默认当前项目：Takt.Domain.Entities）
    /// </summary>
    public string? EntityNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 实体类名称（首字母大写，驼峰命名）
    /// </summary>
    public string? EntityClassName { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
    /// </summary>
    public string? DtoNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象 Dto 类名
    /// </summary>
    public string? DtoClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务命名空间（默认当前项目：Takt.Application.Services）
    /// </summary>
    public string? ServiceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 服务接口类名称
    /// </summary>
    public string? IServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类名称
    /// </summary>
    public string? ServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
    /// </summary>
    public string? ControllerNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 控制器类名称
    /// </summary>
    public string? ControllerClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储层（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsRepository { get; set; }

    /// <summary>
    /// 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
    /// </summary>
    public string? RepositoryInterfaceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储接口类名称
    /// </summary>
    public string? IRepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
    /// </summary>
    public string? RepositoryNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储类名称
    /// </summary>
    public string? RepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
    /// </summary>
    public string? GenFunction { get; set; } = string.Empty;

    /// <summary>
    /// 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
    /// </summary>
    public int? GenMethod { get; set; }

    /// <summary>
    /// 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
    /// </summary>
    public string? GenPath { get; set; } = string.Empty;

    /// <summary>
    /// 生成菜单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsGenMenu { get; set; }

    /// <summary>
    /// 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentMenuId { get; set; }

    /// <summary>
    /// 生成翻译（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsGenTranslation { get; set; }

    /// <summary>
    /// 排序字段（选项本表 columnList.databaseColumnName）
    /// </summary>
    public string? SortField { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
    /// </summary>
    public string? SortType { get; set; } = string.Empty;

    /// <summary>
    /// 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
    /// </summary>
    public int? FrontUi { get; set; }

    /// <summary>
    /// 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
    /// </summary>
    public int? FrontFormLayout { get; set; }

    /// <summary>
    /// 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
    /// </summary>
    public int? FrontBtnStyle { get; set; }

    /// <summary>
    /// 是否生成（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsGenCode { get; set; }

    /// <summary>
    /// 代码生成次数（每次生成成功后自增）
    /// </summary>
    public int? GenCodeCount { get; set; }

    /// <summary>
    /// 使用tabs（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsUseTabs { get; set; }

    /// <summary>
    /// tabs标签中字段的数量
    /// </summary>
    public int? TabsFieldCount { get; set; }

    /// <summary>
    /// 作者
    /// </summary>
    public string? GenAuthor { get; set; } = string.Empty;

    /// <summary>
    /// 其他生成选项（JSON格式，存储其他生成配置）
    /// </summary>
    public string? OtherGenOptions { get; set; } = string.Empty;

    /// <summary>
    /// 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）（子表，级联保存）
    /// </summary>
    public List<TaktGenTableColumnCreateDto>? Columns { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// GenTable 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktGenTableImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
    /// </summary>
    public string? DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
    /// </summary>
    public string? TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; } = string.Empty;

    /// <summary>
    /// 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
    /// </summary>
    public string? SubTableName { get; set; } = string.Empty;

    /// <summary>
    /// 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
    /// </summary>
    public string? SubTableFkName { get; set; } = string.Empty;

    /// <summary>
    /// 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeCode { get; set; } = string.Empty;

    /// <summary>
    /// 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeParentCode { get; set; } = string.Empty;

    /// <summary>
    /// 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeName { get; set; } = string.Empty;

    /// <summary>
    /// 库表标识（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? InDatabase { get; set; }

    /// <summary>
    /// 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
    /// </summary>
    public string? GenTemplateCategory { get; set; } = string.Empty;

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    public string? GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 功能名（用于接口与注释的中文名称，如：公司、部门）
    /// </summary>
    public string? GenFunctionName { get; set; } = string.Empty;

    /// <summary>
    /// 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
    /// </summary>
    public string? PermsPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
    /// </summary>
    public string? MenuButtonGroup { get; set; } = string.Empty;

    /// <summary>
    /// 命名空间前缀（用于生成类名、方法名等的前缀）
    /// </summary>
    public string? NamePrefix { get; set; } = string.Empty;

    /// <summary>
    /// 实体命名空间（默认当前项目：Takt.Domain.Entities）
    /// </summary>
    public string? EntityNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 实体类名称（首字母大写，驼峰命名）
    /// </summary>
    public string? EntityClassName { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
    /// </summary>
    public string? DtoNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象 Dto 类名
    /// </summary>
    public string? DtoClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务命名空间（默认当前项目：Takt.Application.Services）
    /// </summary>
    public string? ServiceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 服务接口类名称
    /// </summary>
    public string? IServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类名称
    /// </summary>
    public string? ServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
    /// </summary>
    public string? ControllerNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 控制器类名称
    /// </summary>
    public string? ControllerClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储层（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsRepository { get; set; }

    /// <summary>
    /// 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
    /// </summary>
    public string? RepositoryInterfaceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储接口类名称
    /// </summary>
    public string? IRepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
    /// </summary>
    public string? RepositoryNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储类名称
    /// </summary>
    public string? RepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
    /// </summary>
    public string? GenFunction { get; set; } = string.Empty;

    /// <summary>
    /// 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
    /// </summary>
    public int? GenMethod { get; set; }

    /// <summary>
    /// 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
    /// </summary>
    public string? GenPath { get; set; } = string.Empty;

    /// <summary>
    /// 生成菜单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsGenMenu { get; set; }

    /// <summary>
    /// 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long? ParentMenuId { get; set; }

    /// <summary>
    /// 生成翻译（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsGenTranslation { get; set; }

    /// <summary>
    /// 排序字段（选项本表 columnList.databaseColumnName）
    /// </summary>
    public string? SortField { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
    /// </summary>
    public string? SortType { get; set; } = string.Empty;

    /// <summary>
    /// 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
    /// </summary>
    public int? FrontUi { get; set; }

    /// <summary>
    /// 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
    /// </summary>
    public int? FrontFormLayout { get; set; }

    /// <summary>
    /// 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
    /// </summary>
    public int? FrontBtnStyle { get; set; }

    /// <summary>
    /// 是否生成（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsGenCode { get; set; }

    /// <summary>
    /// 代码生成次数（每次生成成功后自增）
    /// </summary>
    public int? GenCodeCount { get; set; }

    /// <summary>
    /// 使用tabs（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int? IsUseTabs { get; set; }

    /// <summary>
    /// tabs标签中字段的数量
    /// </summary>
    public int? TabsFieldCount { get; set; }

    /// <summary>
    /// 作者
    /// </summary>
    public string? GenAuthor { get; set; } = string.Empty;

    /// <summary>
    /// 其他生成选项（JSON格式，存储其他生成配置）
    /// </summary>
    public string? OtherGenOptions { get; set; } = string.Empty;

    /// <summary>
    /// 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）（子表，级联保存）
    /// </summary>
    public List<TaktGenTableColumnCreateDto>? Columns { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// GenTable 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktGenTableExportDto
{
    /// <summary>
    /// GenTableID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long GenTableId { get; set; }

    /// <summary>
    /// 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
    /// </summary>
    public string DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; } = string.Empty;

    /// <summary>
    /// 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
    /// </summary>
    public string? SubTableName { get; set; } = string.Empty;

    /// <summary>
    /// 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
    /// </summary>
    public string? SubTableFkName { get; set; } = string.Empty;

    /// <summary>
    /// 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeCode { get; set; } = string.Empty;

    /// <summary>
    /// 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeParentCode { get; set; } = string.Empty;

    /// <summary>
    /// 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    public string? TreeName { get; set; } = string.Empty;

    /// <summary>
    /// 库表标识（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int InDatabase { get; set; } = 0;

    /// <summary>
    /// 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
    /// </summary>
    public string GenTemplateCategory { get; set; } = string.Empty;

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    public string GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 功能名（用于接口与注释的中文名称，如：公司、部门）
    /// </summary>
    public string? GenFunctionName { get; set; } = string.Empty;

    /// <summary>
    /// 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
    /// </summary>
    public string PermsPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
    /// </summary>
    public string? MenuButtonGroup { get; set; } = string.Empty;

    /// <summary>
    /// 命名空间前缀（用于生成类名、方法名等的前缀）
    /// </summary>
    public string? NamePrefix { get; set; } = string.Empty;

    /// <summary>
    /// 实体命名空间（默认当前项目：Takt.Domain.Entities）
    /// </summary>
    public string? EntityNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 实体类名称（首字母大写，驼峰命名）
    /// </summary>
    public string EntityClassName { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
    /// </summary>
    public string? DtoNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象 Dto 类名
    /// </summary>
    public string? DtoClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务命名空间（默认当前项目：Takt.Application.Services）
    /// </summary>
    public string? ServiceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 服务接口类名称
    /// </summary>
    public string? IServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 服务类名称
    /// </summary>
    public string? ServiceClassName { get; set; } = string.Empty;

    /// <summary>
    /// 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
    /// </summary>
    public string? ControllerNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 控制器类名称
    /// </summary>
    public string? ControllerClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储层（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsRepository { get; set; } = 0;

    /// <summary>
    /// 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
    /// </summary>
    public string? RepositoryInterfaceNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储接口类名称
    /// </summary>
    public string? IRepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
    /// </summary>
    public string? RepositoryNamespace { get; set; } = string.Empty;

    /// <summary>
    /// 仓储类名称
    /// </summary>
    public string? RepositoryClassName { get; set; } = string.Empty;

    /// <summary>
    /// 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下： Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto； Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
    /// </summary>
    public string? GenFunction { get; set; } = string.Empty;

    /// <summary>
    /// 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
    /// </summary>
    public int GenMethod { get; set; } = 0;

    /// <summary>
    /// 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
    /// </summary>
    public string GenPath { get; set; } = string.Empty;

    /// <summary>
    /// 生成菜单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsGenMenu { get; set; } = 0;

    /// <summary>
    /// 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
    /// </summary>
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ParentMenuId { get; set; }

    /// <summary>
    /// 生成翻译（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsGenTranslation { get; set; } = 0;

    /// <summary>
    /// 排序字段（选项本表 columnList.databaseColumnName）
    /// </summary>
    public string SortField { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
    /// </summary>
    public string SortType { get; set; } = string.Empty;

    /// <summary>
    /// 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
    /// </summary>
    public int FrontUi { get; set; } = 0;

    /// <summary>
    /// 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
    /// </summary>
    public int FrontFormLayout { get; set; } = 0;

    /// <summary>
    /// 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
    /// </summary>
    public int FrontBtnStyle { get; set; } = 0;

    /// <summary>
    /// 是否生成（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsGenCode { get; set; } = 0;

    /// <summary>
    /// 代码生成次数（每次生成成功后自增）
    /// </summary>
    public int GenCodeCount { get; set; } = 0;

    /// <summary>
    /// 使用tabs（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    public int IsUseTabs { get; set; } = 0;

    /// <summary>
    /// tabs标签中字段的数量
    /// </summary>
    public int TabsFieldCount { get; set; } = 0;

    /// <summary>
    /// 作者
    /// </summary>
    public string GenAuthor { get; set; } = string.Empty;

    /// <summary>
    /// 其他生成选项（JSON格式，存储其他生成配置）
    /// </summary>
    public string? OtherGenOptions { get; set; } = string.Empty;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
