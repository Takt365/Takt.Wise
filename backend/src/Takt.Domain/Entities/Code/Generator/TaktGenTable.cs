// ========================================
// 项目名称：节拍智控 ·TaktWise (TDF) 
// 命名空间：Takt.Domain.Entities.Code.Generator
// 文件名称：TaktGenTable.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：Takt代码生成表配置实体，参考主流代码生成器设计（RuoYi、MyBatis-Plus）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using SqlSugar;
using Takt.Domain.Entities;

namespace Takt.Domain.Entities.Code.Generator;

/// <summary>
/// Takt代码生成表配置实体
/// 特例：继承组合 4：无关联工厂、无语言（TaktTenantCoreEntityBase）
/// </summary>
[SugarTable("takt_code_generator_gen_table", "代码生成数据表配置")]
[SugarIndex("ix_gen_table_tenant", nameof(TenantCode), OrderByType.Asc, false)]
[SugarIndex("ix_gen_table_is_deleted", nameof(TenantCode), OrderByType.Asc, nameof(IsDeleted), OrderByType.Asc, false)]
[SugarIndex("ix_gen_table_datasource_table_unique", nameof(TenantCode), OrderByType.Asc, nameof(DataSource), OrderByType.Asc, nameof(TableName), OrderByType.Asc, true)]
public class TaktGenTable : TaktTenantCoreEntityBase
{
    /// <summary>
    /// 数据源（选项 TaktDatabaseInfos/list；持久化 displayName:tenantCode）
    /// </summary>
    [SugarColumn(ColumnName = "data_source", ColumnDescription = "数据源", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string DataSource { get; set; } = string.Empty;

    /// <summary>
    /// 表名称（选项 TaktDatabaseInfos/tables；新建可手输；租户内与 DataSource 唯一）
    /// </summary>
    [SugarColumn(ColumnName = "table_name", ColumnDescription = "表名称", ColumnDataType = "nvarchar", Length = 200, IsNullable = false)]
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    [SugarColumn(ColumnName = "table_comment", ColumnDescription = "表描述", ColumnDataType = "nvarchar", Length = 500, IsNullable = true)]
    public string? TableComment { get; set; }

    /// <summary>
    /// 关联父表（选项 TaktDatabaseInfos/tables 同库其他表；sub 模板必填）
    /// </summary>
    [SugarColumn(ColumnName = "sub_table_name", ColumnDescription = "关联父表", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? SubTableName { get; set; }

    /// <summary>
    /// 关联外键（选项本表 columnList.databaseColumnName；sub 模板必填）
    /// </summary>
    [SugarColumn(ColumnName = "sub_table_fk_name", ColumnDescription = "关联外键", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? SubTableFkName { get; set; }

    /// <summary>
    /// 树编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    [SugarColumn(ColumnName = "tree_code", ColumnDescription = "树编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? TreeCode { get; set; }

    /// <summary>
    /// 树父编码（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    [SugarColumn(ColumnName = "tree_parent_code", ColumnDescription = "树父编码", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? TreeParentCode { get; set; }

    /// <summary>
    /// 树名称（选项本表 columnList.databaseColumnName；tree 模板必填）
    /// </summary>
    [SugarColumn(ColumnName = "tree_name", ColumnDescription = "树名称", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? TreeName { get; set; }

    /// <summary>
    /// 库表标识（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "in_database", ColumnDescription = "库表标识", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int InDatabase { get; set; } = 1;

    /// <summary>
    /// 生成模板类型（字典 code_generator_template_type；crud/sub/tree）
    /// </summary>
    [SugarColumn(ColumnName = "gen_template_category", ColumnDescription = "生成模板类型", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "crud")]
    public string GenTemplateCategory { get; set; } = "crud";
    
    /// <summary>
  /// 模块名（功能模块名称）
  /// </summary>
  [SugarColumn(ColumnName = "module_name", ColumnDescription = "模块名", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? GenModuleName { get; set; }

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    [SugarColumn(ColumnName = "business_name", ColumnDescription = "业务名", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 功能名（用于接口与注释的中文名称，如：公司、部门）
    /// </summary>
    [SugarColumn(ColumnName = "function_name", ColumnDescription = "功能名", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? GenFunctionName { get; set; }

    /// <summary>
    /// 权限前缀（与生成控制器/菜单/前端权限一致；对应库列 <c>perms_prefix</c>）。
    /// </summary>
    [SugarColumn(ColumnName = "perms_prefix", ColumnDescription = "权限前缀", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string PermsPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 菜单权限组（字典 code_generator_button_category 多选逗号；仅用于生成 menu_and_translation.sql 按钮 INSERT，不参与控制器/前端代码生成）
    /// </summary>
    [SugarColumn(ColumnName = "menu_button_group", ColumnDescription = "菜单权限组", ColumnDataType = "ntext", IsNullable = true)]
    public string? MenuButtonGroup { get; set; }

    /// <summary>
  /// 命名空间前缀（用于生成类名、方法名等的前缀）
  /// </summary>
  [SugarColumn(ColumnName = "name_prefix", ColumnDescription = "命名空间前缀", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
    public string? NamePrefix { get; set; }

    /// <summary>
    /// 实体命名空间（默认当前项目：Takt.Domain.Entities）
    /// </summary>
    [SugarColumn(ColumnName = "entity_namespace", ColumnDescription = "实体命名空间", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? EntityNamespace { get; set; } = "Takt.Domain.Entities";

    /// <summary>
    /// 实体类名称（首字母大写，驼峰命名）
    /// </summary>
    [SugarColumn(ColumnName = "class_name", ColumnDescription = "实体类名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = false)]
    public string EntityClassName { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象Dto命名空间（默认当前项目：Takt.Application.Dtos）
    /// </summary>
    [SugarColumn(ColumnName = "dto_namespace", ColumnDescription = "传输对象Dto命名空间", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? DtoNamespace { get; set; } = "Takt.Application.Dtos";

    /// <summary>
    /// 传输对象 Dto 类名
    /// </summary>
    [SugarColumn(ColumnName = "dto_class_name", ColumnDescription = "传输对象Dto类名", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? DtoClassName { get; set; }
    
    /// <summary>
    /// 服务命名空间（默认当前项目：Takt.Application.Services）
    /// </summary>
    [SugarColumn(ColumnName = "service_namespace", ColumnDescription = "服务命名空间", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ServiceNamespace { get; set; } = "Takt.Application.Services";
    
    /// <summary>
    /// 服务接口类名称
    /// </summary>
    [SugarColumn(ColumnName = "i_service_class_name", ColumnDescription = "服务接口类名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? IServiceClassName { get; set; }
    
    /// <summary>
    /// 服务类名称
    /// </summary>
    [SugarColumn(ColumnName = "service_class_name", ColumnDescription = "服务类名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ServiceClassName { get; set; }
    
    /// <summary>
    /// 控制器命名空间（默认当前项目：Takt.WebApi.Controllers）
    /// </summary>
    [SugarColumn(ColumnName = "controller_namespace", ColumnDescription = "控制器命名空间", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? ControllerNamespace { get; set; } = "Takt.WebApi.Controllers";
    
    /// <summary>
    /// 控制器类名称
    /// </summary>
    [SugarColumn(ColumnName = "controller_class_name", ColumnDescription = "控制器类名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? ControllerClassName { get; set; }
    
    /// <summary>
    /// 仓储层（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_repository", ColumnDescription = "仓储层", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsRepository { get; set; } = 0;
    
    /// <summary>
    /// 仓储接口命名空间（默认当前项目：Takt.Domain.Repositories）
    /// </summary>
    [SugarColumn(ColumnName = "repository_interface_namespace", ColumnDescription = "仓储接口命名空间", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? RepositoryInterfaceNamespace { get; set; } = "Takt.Domain.Repositories";
    
    /// <summary>
    /// 仓储接口类名称
    /// </summary>
    [SugarColumn(ColumnName = "i_repository_class_name", ColumnDescription = "仓储接口类名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? IRepositoryClassName { get; set; }
    
    /// <summary>
    /// 仓储命名空间（默认当前项目：Takt.Infrastructure.Repositories）
    /// </summary>
    [SugarColumn(ColumnName = "repository_namespace", ColumnDescription = "仓储命名空间", ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
    public string? RepositoryNamespace { get; set; } = "Takt.Infrastructure.Repositories";
    
    /// <summary>
    /// 仓储类名称
    /// </summary>
    [SugarColumn(ColumnName = "repository_class_name", ColumnDescription = "仓储类名称", ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
    public string? RepositoryClassName { get; set; }
    
    /// <summary>
    /// 生成功能（字典 code_generator_function 多选逗号；亦支持 JSON/数组）。
    /// 核心设计：GenFunction 不仅决定生成哪些 Controller Actions 和 Service Methods，还决定生成哪些 DTO 类。功能与 DTO 的映射关系如下：
    /// Query → QueryDto；Create → CreateDto；Update → UpdateDto；Status → StatusDto；Sort → SortDto；
    /// Import → TemplateDto + ImportDto；Export → ExportDto；所有功能 → Dto（基础传输对象，包含所有字段）。
    /// </summary>
    [SugarColumn(ColumnName = "code_generator_function", ColumnDescription = "生成功能", ColumnDataType = "ntext", IsNullable = true)]
    public string? GenFunction { get; set; }

    /// <summary>
    /// 生成方式（字典 code_generator_method；0=zip 1=自定义路径 2=当前项目）
    /// </summary>
    [SugarColumn(ColumnName = "code_generator_method", ColumnDescription = "生成方式", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int GenMethod { get; set; } = 0;

    /// <summary>
    /// 生成路径（字典 code_generator_path_type；GenMethod=1 时选择；0 默认 /；2 由 GenMethod 解析）
    /// </summary>
    [SugarColumn(ColumnName = "gen_path", ColumnDescription = "生成路径", ColumnDataType = "nvarchar", Length = 500, IsNullable = false, DefaultValue = "/")]
    public string GenPath { get; set; } = "/";

    /// <summary>
    /// 生成菜单（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_gen_menu", ColumnDescription = "生成菜单", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsGenMenu { get; set; } = 1;

    /// <summary>
    /// 上级菜单（关联 TaktMenu.Id，选项 TaktMenus/tree-options）
    /// </summary>
    [SugarColumn(ColumnName = "parent_menu_id", ColumnDescription = "上级菜单ID", ColumnDataType = "bigint", IsNullable = false, DefaultValue = "0")]
    public long ParentMenuId { get; set; }

    /// <summary>
    /// 生成翻译（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_gen_translation", ColumnDescription = "生成翻译", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int IsGenTranslation { get; set; } = 1;

    /// <summary>
    /// 排序字段（选项本表 columnList.databaseColumnName）
    /// </summary>
    [SugarColumn(ColumnName = "sort_field", ColumnDescription = "排序字段", ColumnDataType = "nvarchar", Length = 100, IsNullable = false, DefaultValue = "")]
    public string SortField { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（字典 sys_sort_type；ASC=升序 DESC=降序）
    /// </summary>
    [SugarColumn(ColumnName = "sort_type", ColumnDescription = "排序方向", ColumnDataType = "nvarchar", Length = 10, IsNullable = false, DefaultValue = "asc")]
    public string SortType { get; set; } = "asc";

    /// <summary>
    /// 前端UI框架（字典 code_generator_frontend_ui_type；1=element plus 2=ant design vue）
    /// </summary>
    [SugarColumn(ColumnName = "front_ui", ColumnDescription = "前端UI框架", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int FrontUi { get; set; } = 2;

    /// <summary>
    /// 前端表单布局（字典 code_generator_frontend_form_layout；12=一行一列 24=一行两列）
    /// </summary>
    [SugarColumn(ColumnName = "front_form_layout", ColumnDescription = "前端表单布局", ColumnDataType = "int", IsNullable = false, DefaultValue = "24")]
    public int FrontFormLayout { get; set; } = 24;

    /// <summary>
    /// 前端按钮样式（字典 code_generator_button_style；0=文本 1=标准）
    /// </summary>
    [SugarColumn(ColumnName = "front_btn_style", ColumnDescription = "前端按钮样式", ColumnDataType = "int", IsNullable = false, DefaultValue = "1")]
    public int FrontBtnStyle { get; set; } = 1;

    /// <summary>
    /// 是否生成（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_gen_code", ColumnDescription = "是否生成", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsGenCode { get; set; } = 0;

    /// <summary>
    /// 代码生成次数（每次生成成功后自增）
    /// </summary>
    [SugarColumn(ColumnName = "gen_code_count", ColumnDescription = "代码生成次数", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int GenCodeCount { get; set; } = 0;

    /// <summary>
    /// 使用tabs（字典 sys_yes_no；0=否 1=是）
    /// </summary>
    [SugarColumn(ColumnName = "is_use_tabs", ColumnDescription = "使用tabs", ColumnDataType = "int", IsNullable = false, DefaultValue = "0")]
    public int IsUseTabs { get; set; } = 1;

    /// <summary>
    /// tabs标签中字段的数量
    /// </summary>
    [SugarColumn(ColumnName = "tabs_field_count", ColumnDescription = "tabs标签字段", ColumnDataType = "int", IsNullable = false, DefaultValue = "10")]
    public int TabsFieldCount { get; set; } = 10;

    /// <summary>
    /// 作者
    /// </summary>
    [SugarColumn(ColumnName = "gen_author", ColumnDescription = "作者", ColumnDataType = "nvarchar", Length = 50, IsNullable = false, DefaultValue = "")]
    public string GenAuthor { get; set; } = string.Empty;

    /// <summary>
    /// 其他生成选项（JSON格式，存储其他生成配置）
    /// </summary>
    [SugarColumn(ColumnName = "other_gen_options", ColumnDescription = "其他生成选项", ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]
    public string? OtherGenOptions { get; set; }

    // ========================================
    // 导航属性区域
    // ========================================

    /// <summary>
    /// 字段配置列表（子表，外键：TaktGenTableColumn.GenTableId 关联本表 Id）
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(TaktGenTableColumn.GenTableId))]
    public List<TaktGenTableColumn>? Columns { get; set; }
}
