// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Generator.GenEngine
// 文件名称：TaktGenTemplateContext.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成模板上下文，供 Scriban 模板绑定；与 Domain 实体 TaktGenTable、TaktGenTableColumn 一一对应，可从两实体构建
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Newtonsoft.Json.Linq;
using Takt.Domain.Entities.Code.Generator;

namespace Takt.Application.Services.Code.Generator.GenEngine;

/// <summary>
/// 控制器 Action 描述符，由 GenFunction 解析后生成，供模板循环渲染，不写死功能关键字。
/// </summary>
public class TaktGenControllerActionDescriptor
{
    /// <summary>
    /// Action 摘要说明（如：获取xxx列表（分页））
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// HTTP 方法（如 HttpGet、HttpPost）
    /// </summary>
    public string HttpMethod { get; set; } = string.Empty;

    /// <summary>
    /// 路由（如 list、{id}、batch）
    /// </summary>
    public string Route { get; set; } = string.Empty;

    /// <summary>
    /// 权限键（如 logistics:materials:plant:list，四段小写冒号分隔：领域:目录:实体:key）
    /// </summary>
    public string PermissionKey { get; set; } = string.Empty;

    /// <summary>
    /// 权限显示名（如：查询列表）
    /// </summary>
    public string PermissionName { get; set; } = string.Empty;

    /// <summary>
    /// 方法名（如 GetListAsync）
    /// </summary>
    public string MethodName { get; set; } = string.Empty;

    /// <summary>
    /// 方法参数签名（如 [FromQuery] xxxQueryDto queryDto）
    /// </summary>
    public string Signature { get; set; } = string.Empty;

    /// <summary>
    /// 返回类型（如 TaktPagedResult&lt;EntityDto&gt;、IActionResult）
    /// </summary>
    public string ResponseType { get; set; } = string.Empty;

    /// <summary>
    /// 方法体代码（多行字符串）
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// 前端 API 方法名（驼峰，如 getList、getById、create、update、remove、deleteBatch）
    /// </summary>
    public string FrontendMethodName { get; set; } = string.Empty;

    /// <summary>
    /// 前端方法参数签名（如 params: DeptQuery、id: string、data: DeptCreate、id: string, data: DeptUpdate、ids: string[]）
    /// </summary>
    public string FrontendSignature { get; set; } = string.Empty;

    /// <summary>
    /// 前端返回类型（如 Promise&lt;TaktPagedResult&lt;Dept&gt;&gt;、Promise&lt;Dept&gt;、Promise&lt;void&gt;）
    /// </summary>
    public string FrontendReturnType { get; set; } = string.Empty;

    /// <summary>
    /// 前端请求体键（params、data、data: ids 或空，供 request 配置）
    /// </summary>
    public string FrontendRequestKey { get; set; } = string.Empty;
}

/// <summary>
/// 服务方法描述符，由 GenFunction 解析后生成，供模板循环渲染，不写死功能关键字。
/// </summary>
public class TaktGenServiceMethodDescriptor
{
    /// <summary>
    /// 方法摘要说明（如：获取xxx列表（分页））
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// XML 注释中的 param/returns 片段（供接口声明用）
    /// </summary>
    public string ParamsXml { get; set; } = string.Empty;

    /// <summary>
    /// 方法名（如 GetListAsync、QueryExpression）
    /// </summary>
    public string MethodName { get; set; } = string.Empty;

    /// <summary>
    /// 方法参数签名（如 xxxQueryDto queryDto）
    /// </summary>
    public string Signature { get; set; } = string.Empty;

    /// <summary>
    /// 返回类型（如 Task&lt;TaktPagedResult&lt;EntityDto&gt;&gt;）
    /// </summary>
    public string ReturnType { get; set; } = string.Empty;

    /// <summary>
    /// 方法体代码（多行字符串）
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// 是否为私有方法（如 QueryExpression）
    /// </summary>
    public bool IsPrivate { get; set; }
}

/// <summary>
/// DTO 类别描述符，由 DtoCategory JSON 数组项反序列化得到，供模板按 BodyKind/BaseClass 生成，不写死类型名。
/// </summary>
public class TaktDtoCategoryDescriptor
{
    /// <summary>
    /// 类别名（如 Dto、QueryDto、CreateDto、StatusDto）
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 主体形态：Full=Id+列，NoId=仅列，OnlyId=仅Id（需基类），Query=继承 TaktPagedQuery+列
    /// </summary>
    public string BodyKind { get; set; } = "NoId";

    /// <summary>
    /// 基类全名（可选）；相对名如 CreateDto 会在构建时解析为 EntityClassName+CreateDto
    /// </summary>
    public string? BaseClass { get; set; }

    /// <summary>
    /// 前端 TypeScript 接口名（如 Dept、DeptQuery、DeptCreate、DeptUpdate、DeptStatus）
    /// </summary>
    public string TsInterfaceName { get; set; } = string.Empty;

    /// <summary>
    /// 前端 extends 类型名（如 TaktTenantEntityBase、TaktPagedQuery、DeptCreate；空表示不继承）
    /// </summary>
    public string TsExtendsName { get; set; } = string.Empty;
}

/// <summary>
/// 翻译 SQL 单行数据，供 menu_and_translation.sql 模板使用，每行 Id 由 SnowFlakeSingle.Instance.NextId() 生成。ResourceGroup：菜单标题为 menu，其他字段为 page。
/// </summary>
public class TaktSqlTranslationRowItem
{
    public long Id { get; set; }
    public string Culture { get; set; } = string.Empty;
    public string ResourceKey { get; set; } = string.Empty;
    public string TranslationValue { get; set; } = string.Empty;
    /// <summary>
    /// 资源分组：menu=菜单翻译，page=页面字段翻译
    /// </summary>
    public string ResourceGroup { get; set; } = "page";
    public int SortOrder { get; set; }
}

/// <summary>
/// 按钮菜单 SQL 单行（<c>takt_identity_menu</c>，<c>menu_type=2</c>），供 menu_and_translation.sql；<c>parent_id</c> 为页面菜单 TaktGenTableTemplateModel.SqlMenuId。
/// </summary>
public class TaktSqlMenuButtonRowItem
{
    public long Id { get; set; }
    public string MenuCode { get; set; } = string.Empty;
    public string MenuName { get; set; } = string.Empty;
    public string Permission { get; set; } = string.Empty;
    public string MenuL10nKey { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

/// <summary>
/// 代码生成模板上下文：表配置 + 列配置列表，模板中可使用 Table 与 Columns。
/// 使用 From 从 TaktGenTable、TaktGenTableColumn 构建，充分利用两实体全部字段。
/// </summary>
public class TaktGenTemplateContext
{
    /// <summary>
    /// 表配置（模板中通过 Table 访问，如 {{ Table.TableName }}、{{ Table.EntityClassName }}）
    /// </summary>
    public TaktGenTableTemplateModel Table { get; set; } = new();

    /// <summary>
    /// 列配置列表（模板中通过 Columns 访问，如 {{ for col in Columns }} {{ col.CsharpColumnName }} {{ end }}）
    /// </summary>
    public List<TaktGenColumnTemplateModel> Columns { get; set; } = new();

    /// <summary>
    /// 从 Domain 实体构建模板上下文，充分利用 TaktGenTable 与 TaktGenTableColumn 全部字段。
    /// </summary>
    /// <param name="table">代码生成表配置实体</param>
    /// <param name="columns">该表下的字段配置列表（可为空）</param>
    /// <returns>供 Scriban 使用的模板上下文</returns>
    public static TaktGenTemplateContext From(TaktGenTable table, IEnumerable<TaktGenTableColumn>? columns = null)
    {
        if (table == null)
            throw new ArgumentNullException(nameof(table));
        var ctx = new TaktGenTemplateContext
        {
            Table = TaktGenTableTemplateModel.From(table) ?? new TaktGenTableTemplateModel(),
            Columns = (columns ?? Array.Empty<TaktGenTableColumn>())
                .OrderBy(c => c.LineNumber)
                .Select(TaktGenColumnTemplateModel.From)
                .ToList()
        };
        return ctx;
    }
}

/// <summary>
/// 表级模板模型，与 TaktGenTable 实体字段一一对应，便于 Scriban 绑定并充分利用实体全部属性。
/// </summary>
public class TaktGenTableTemplateModel
{
    /// <summary>
    /// 数据源（前面是数据库名称，后面是 TenantCode，如：Takt_000_Dev:000）
    /// </summary>
    public string? DataSource { get; set; }

    /// <summary>
    /// 数据表名称
    /// </summary>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// 表描述（表注释）
    /// </summary>
    public string? TableComment { get; set; }

    /// <summary>
    /// 表注释展示值（TableComment ?? GenBusinessName，模板中直接用 Table.Comment）
    /// </summary>
    public string Comment => string.IsNullOrWhiteSpace(TableComment) ? (GenBusinessName ?? string.Empty) : TableComment;

    /// <summary>
    /// 关联父表名（用于主子表）
    /// </summary>
    public string? SubTableName { get; set; }

    /// <summary>
    /// 本表关联父表的外键名（用于主子表）
    /// </summary>
    public string? SubTableFkName { get; set; }

    /// <summary>
    /// 树编码字段（用于树形结构）
    /// </summary>
    public string? TreeCode { get; set; }

    /// <summary>
    /// 树父编码字段（用于树形结构）
    /// </summary>
    public string? TreeParentCode { get; set; }

    /// <summary>
    /// 树名称字段（用于树形结构）
    /// </summary>
    public string? TreeName { get; set; }

    /// <summary>
    /// 是否在数据库中（1=是库表，0=不是库表）
    /// </summary>
    public int InDatabase { get; set; }

    /// <summary>
    /// 生成模板类型（crud=单表，tree=树表，sub=主子表）
    /// </summary>
    public string GenTemplateCategory { get; set; } = "crud";

    /// <summary>
    /// 命名空间前缀（用于生成类名、方法名等的前缀）
    /// </summary>
    public string? NamePrefix { get; set; }

    /// <summary>
    /// 实体命名空间
    /// </summary>
    public string? EntityNamespace { get; set; }

    /// <summary>
    /// 实体类名称（首字母大写，驼峰命名）
    /// </summary>
    public string EntityClassName { get; set; } = string.Empty;

    /// <summary>
    /// 实体名帕斯卡（前端用，如 TaktDept→Dept）
    /// </summary>
    public string EntityNamePascal { get; set; } = string.Empty;

    /// <summary>
    /// 实体名驼峰（前端用，如 Dept→dept）
    /// </summary>
    public string EntityNameCamel { get; set; } = string.Empty;

    /// <summary>
    /// 实体名 kebab（前端用，如 StandardWageRate→standard-wage-rate，用于表单组件文件名等）
    /// </summary>
    public string EntityNameKebab { get; set; } = string.Empty;

    /// <summary>
    /// API 基础路径（如 /api/TaktDepts）
    /// </summary>
    public string ApiBasePath { get; set; } = string.Empty;

    /// <summary>
    /// 传输对象 Dto 命名空间
    /// </summary>
    public string? DtoNamespace { get; set; }

    /// <summary>
    /// 传输对象 Dto 类名
    /// </summary>
    public string? DtoClassName { get; set; }

    /// <summary>
    /// 传输对象 Dto 类别列表（由 GenFunction 自动推断，供模板循环）
    /// </summary>
    public List<string> DtoCategories { get; set; } = new();

    /// <summary>
    /// 传输对象 Dto 类别描述列表（含名称、基类、BodyKind，供模板按数据循环生成，不写死类型名）
    /// </summary>
    public List<TaktDtoCategoryDescriptor> DtoCategoryDescriptors { get; set; } = new();

    /// <summary>
    /// 服务命名空间
    /// </summary>
    public string? ServiceNamespace { get; set; }

    /// <summary>
    /// 服务接口类名称
    /// </summary>
    public string? IServiceClassName { get; set; }

    /// <summary>
    /// 服务类名称
    /// </summary>
    public string? ServiceClassName { get; set; }

    /// <summary>
    /// 控制器命名空间
    /// </summary>
    public string? ControllerNamespace { get; set; }

    /// <summary>
    /// 控制器类名称
    /// </summary>
    public string? ControllerClassName { get; set; }

    /// <summary>
    /// 仓储接口命名空间
    /// </summary>
    public string? RepositoryInterfaceNamespace { get; set; }

    /// <summary>
    /// 仓储接口类名称
    /// </summary>
    public string? IRepositoryClassName { get; set; }

    /// <summary>
    /// 仓储命名空间
    /// </summary>
    public string? RepositoryNamespace { get; set; }

    /// <summary>
    /// 仓储类名称
    /// </summary>
    public string? RepositoryClassName { get; set; }

    /// <summary>
    /// 模块名（功能模块名称）
    /// </summary>
    public string? GenModuleName { get; set; }

    /// <summary>
    /// ApiModule 顶级业务领域键（GenModuleName 首段 PascalCase，如 accounting_financial→Accounting）
    /// </summary>
    public string ApiModuleKey { get; set; } = "Default";

    /// <summary>
    /// ApiModule 顶级业务领域名称（与 ApiModuleKey 对应的中文显示名，须与 SwaggerDoc/OpenAPI 分组 Title 一致，如 Accounting→会计核算）
    /// </summary>
    public string ApiModuleName { get; set; } = "通用";

    /// <summary>
    /// 前端模块路径（GenModuleName 转小写且下划线改 /，如 accounting_financial→accounting/financial，用于 import 路径）
    /// </summary>
    public string FrontendModulePath { get; set; } = string.Empty;

    /// <summary>
    /// API 文件中 import request 的相对路径（根据模块深度，如 ../request 或 ../../request）
    /// </summary>
    public string RequestImportPath { get; set; } = "../request";

    /// <summary>
    /// 业务名（用于类名，如 Company，与模块拼接为 Takt.模块+类名）
    /// </summary>
    public string GenBusinessName { get; set; } = string.Empty;

    /// <summary>
    /// 功能名（用于接口与注释的中文名称，如：公司、部门）
    /// </summary>
    public string? GenFunctionName { get; set; }

    /// <summary>
    /// 生成功能（查询，新增，更新，删除，模板，导入，导出）
    /// </summary>
    public string? GenFunction { get; set; }

    /// <summary>
    /// 是否生成查询相关（0=否，1=是，由 GenFunction 解析）
    /// </summary>
    public int IsQuery { get; set; }

    /// <summary>
    /// 是否生成新增相关（0=否，1=是，由 GenFunction 解析）
    /// </summary>
    public int IsCreate { get; set; }

    /// <summary>
    /// 是否生成更新相关（0=否，1=是，由 GenFunction 解析）
    /// </summary>
    public int IsUpdate { get; set; }

    /// <summary>
    /// 是否生成删除相关（0=否，1=是，由 GenFunction 解析）
    /// </summary>
    public int IsDelete { get; set; }

    /// <summary>
    /// 是否生成状态相关（0=否，1=是，由 GenFunction 解析）
    /// </summary>
    public int IsStatus { get; set; }

    /// <summary>
    /// 是否生成排序相关（0=否，1=是，由 GenFunction 解析）
    /// </summary>
    public int IsSort { get; set; }

    /// <summary>
    /// 是否生成模板相关（0=否，1=是，由 GenFunction 解析）
    /// </summary>
    public int IsTemplate { get; set; }

    /// <summary>
    /// 是否生成导入相关（0=否，1=是，由 GenFunction 解析）
    /// </summary>
    public int IsImport { get; set; }

    /// <summary>
    /// 是否生成导出相关（0=否，1=是，由 GenFunction 解析）
    /// </summary>
    public int IsExport { get; set; }

    /// <summary>
    /// 生成代码方式（0=zip 压缩包，1=自定义路径，2=当前项目）
    /// </summary>
    public int GenMethod { get; set; } = 0;

    /// <summary>
    /// 是否生成仓储层（1=是，0=否）
    /// </summary>
    public int IsRepository { get; set; } = 1;

    /// <summary>
    /// 生成路径（默认为项目根目录）
    /// </summary>
    public string GenPath { get; set; } = "/";

    /// <summary>
    /// 上级菜单 ID
    /// </summary>
    public long ParentMenuId { get; set; }

    /// <summary>
    /// 是否生成菜单（1=是，0=否）
    /// </summary>
    public int IsGenMenu { get; set; }

    /// <summary>
    /// 是否生成翻译（1=是，0=否）
    /// </summary>
    public int IsGenTranslation { get; set; }

    /// <summary>
    /// 排序字段（实体列名，帕斯卡）
    /// </summary>
    public string SortField { get; set; } = string.Empty;

    /// <summary>
    /// 排序方向（asc=升序，desc=降序）
    /// </summary>
    public string SortType { get; set; } = "asc";

    /// <summary>
    /// 排序字段驼峰（供前端 API 使用，如 CreatedAt→createdAt）
    /// </summary>
    public string TsSortField { get; set; } = string.Empty;

    /// <summary>
    /// 与 TaktGenTable.PermsPrefix 一致，来自生成表配置（数据库原样）。
    /// </summary>
    public string PermsPrefix { get; set; } = string.Empty;

    /// <summary>
    /// 由库表列 <c>perms_prefix</c>（同模型中的 <c>PermsPrefix</c> 属性）规范化得到的三段基点：领域:目录:实体（小写、冒号分隔）。模板中完整权限码一律为「本属性 + 冒号 + key」（如再拼 <c>:list</c>、<c>:update</c>）。
    /// 计算顺序：<b>先处理 PermsPrefix</b>；仅当其为空或未解析出任何段时，才用 GenModuleName 与实体类名推导；非空时只规范化前缀字符串，不以模块名覆盖用户已填段。
    /// </summary>
    public string PermsPrefixCanonical { get; set; } = string.Empty;

    /// <summary>
    /// 菜单权限组
    /// </summary>
    public string? MenuButtonGroup { get; set; }

    /// <summary>
    /// 前端UI框架（1=element plus，2=ant design vue）
    /// </summary>
    public int FrontUi { get; set; } = 1;

    /// <summary>
    /// 前端表单布局（12=一行一列，24=一行两列）
    /// </summary>
    public int FrontFormLayout { get; set; } = 24;

    /// <summary>
    /// 前端操作按钮样式（0=文本，1=标准）
    /// </summary>
    public int FrontBtnStyle { get; set; } = 1;

    /// <summary>
    /// 是否生成代码（1=是，0=否）
    /// </summary>
    public int IsGenCode { get; set; } = 1;

    /// <summary>
    /// 代码生成次数（每次生成成功后自增）
    /// </summary>
    public int GenCodeCount { get; set; } = 0;

    /// <summary>
    /// 是否使用 tabs（1=是，0=否）
    /// </summary>
    public int IsUseTabs { get; set; } = 1;

    /// <summary>
    /// tabs 标签中字段的数量
    /// </summary>
    public int TabsFieldCount { get; set; } = 10;

    /// <summary>
    /// 作者
    /// </summary>
    public string GenAuthor { get; set; } = string.Empty;

    /// <summary>
    /// SQL 创建人（生成菜单/翻译 SQL 时写入 create_by，如 admin、user01；由生成时当前登录用户名或 GenAuthor 填充）
    /// </summary>
    public string SqlCreateBy { get; set; } = "admin";

    /// <summary>
    /// 菜单 SQL 的雪花 ID（IsGenMenu=1 时由 SnowFlakeSingle.Instance.NextId() 生成，供 INSERT id 列）
    /// </summary>
    public long? SqlMenuId { get; set; }

    /// <summary>
    /// 翻译 SQL 行列表（IsGenTranslation=1 时按模板顺序预生成，每行含雪花 Id、culture、resource_key、translation_value、sort_order）
    /// </summary>
    public List<TaktSqlTranslationRowItem> SqlTranslationRows { get; set; } = new();

    /// <summary>
    /// 按钮菜单 SQL 行列表
    /// </summary>
    public List<TaktSqlMenuButtonRowItem> SqlMenuButtonRows { get; set; } = new();

    /// <summary>
    /// 其他生成选项（JSON 格式）
    /// </summary>
    public string? OtherGenOptions { get; set; }

    /// <summary>
    /// 实体基类层级（tenant/company/approval，来自 OtherGenOptions.entityBaseTier）
    /// </summary>
    public string EntityBaseTier { get; set; } = TaktGenEntityBaseProfile.TierTenant;

    /// <summary>
    /// C# 实体基类名（如 TaktTenantEntityBase）
    /// </summary>
    public string EntityBaseClass { get; set; } = "TaktTenantEntityBase";

    /// <summary>
    /// 响应 Dto 基类名（如 TaktTenantDtoBase）
    /// </summary>
    public string DtoEntityBaseClass { get; set; } = "TaktTenantDtoBase";

    /// <summary>
    /// 前端 TS 实体基类接口名（如 TaktTenantEntityBase）
    /// </summary>
    public string TsEntityBaseInterface { get; set; } = "TaktTenantEntityBase";

    /// <summary>
    /// 仓储接口名（ITaktTenantRepository / ITaktCompanyRepository / ITaktApprovalRepository）
    /// </summary>
    public string RepositoryInterfaceName { get; set; } = "ITaktTenantRepository";

    /// <summary>
    /// ApiModule 枚举成员名（如 HumanResource、Statistics）
    /// </summary>
    public string ApiModuleEnum { get; set; } = "Routine";

    /// <summary>
    /// 控制器 Action 列表，由 GenFunction 解析生成，模板仅循环渲染
    /// </summary>
    public List<TaktGenControllerActionDescriptor> ControllerActions { get; set; } = new();

    /// <summary>
    /// 服务方法列表（接口+实现），由 GenFunction 解析生成，模板仅循环渲染
    /// </summary>
    public List<TaktGenServiceMethodDescriptor> ServiceMethods { get; set; } = new();

    /// <summary>
    /// 私有方法列表（如 QueryExpression），由 GenFunction 解析生成
    /// </summary>
    public List<TaktGenServiceMethodDescriptor> ServicePrivateMethods { get; set; } = new();

    /// <summary>
    /// 将 DtoCategory 的 JSON 转为描述符列表。支持：JSON 对象 {"主传输对象":"Dto","查询传输对象":"QueryDto",...}（取 value 为 DTO 类后缀）；JSON 数组 ["Dto","QueryDto",...] 或 [{"Name":"x","BodyKind":"NoId"},...]；逗号分隔。相对 BaseClass（不含.）解析为 EntityClassName+BaseClass。TsInterfaceName 由 entityNamePascal 与 Name 推导。
    /// </summary>
    /// <param name="entityClassName">实体类名（用于解析相对基类）</param>
    /// <param name="dtoCategory">DtoCategory 原始字符串（JSON 对象/数组或逗号分隔）</param>
    /// <param name="entityNamePascal">前端实体帕斯卡名（用于 TsInterfaceName，如 Dept）</param>
    /// <returns>DTO 类别描述符列表，供模板循环生成</returns>
    private static List<TaktDtoCategoryDescriptor> BuildDtoCategoryDescriptors(string entityClassName, string? dtoCategory, string entityNamePascal)
    {
        if (string.IsNullOrWhiteSpace(dtoCategory))
            return new List<TaktDtoCategoryDescriptor>();

        var trimmed = dtoCategory.Trim();

        if (trimmed.StartsWith('{'))
        {
            try
            {
                var jobj = JObject.Parse(trimmed);
                var list = new List<TaktDtoCategoryDescriptor>();
                foreach (var prop in jobj.Properties())
                {
                    var name = prop.Value?.Type == JTokenType.String ? prop.Value.ToString() : prop.Value?.ToString() ?? "";
                    if (string.IsNullOrWhiteSpace(name)) continue;
                    var d = InferDescriptor(name.Trim(), entityClassName);
                    d.TsInterfaceName = GetTsInterfaceName(name.Trim(), entityNamePascal);
                    d.TsExtendsName = GetTsExtendsName(d.BodyKind, entityNamePascal);
                    list.Add(d);
                }
                return list;
            }
            catch { }
        }

        if (!trimmed.StartsWith('['))
        {
            var names = trimmed.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Where(s => s.Length > 0).ToList();
            return names.Select(name =>
            {
                var d = InferDescriptor(name, entityClassName);
                d.TsInterfaceName = GetTsInterfaceName(name, entityNamePascal);
                d.TsExtendsName = GetTsExtendsName(d.BodyKind, entityNamePascal);
                return d;
            }).ToList();
        }

        try
        {
            var arr = JArray.Parse(trimmed);
            var list = new List<TaktDtoCategoryDescriptor>();
            foreach (var el in arr)
            {
                if (el?.Type == JTokenType.String)
                {
                    var name = el.ToString();
                    if (string.IsNullOrEmpty(name)) continue;
                    var d = InferDescriptor(name, entityClassName);
                    d.TsInterfaceName = GetTsInterfaceName(name, entityNamePascal);
                    d.TsExtendsName = GetTsExtendsName(d.BodyKind, entityNamePascal);
                    list.Add(d);
                }
                else if (el is JObject jo)
                {
                    var name = jo["Name"]?.ToString() ?? "";
                    if (string.IsNullOrEmpty(name)) continue;
                    var bodyKind = jo["BodyKind"]?.ToString() ?? "";
                    var baseClass = jo["BaseClass"]?.ToString();
                    if (!string.IsNullOrEmpty(bodyKind) || baseClass != null)
                    {
                        if (!string.IsNullOrEmpty(baseClass) && !baseClass.Contains('.'))
                            baseClass = entityClassName + baseClass;
                        var tsName = GetTsInterfaceName(name, entityNamePascal);
                        var tsExt = GetTsExtendsName(bodyKind, entityNamePascal);
                        list.Add(new TaktDtoCategoryDescriptor { Name = name, BodyKind = bodyKind, BaseClass = baseClass, TsInterfaceName = tsName, TsExtendsName = tsExt });
                    }
                    else
                    {
                        var d = InferDescriptor(name, entityClassName);
                        d.TsInterfaceName = GetTsInterfaceName(name, entityNamePascal);
                        d.TsExtendsName = GetTsExtendsName(d.BodyKind, entityNamePascal);
                        list.Add(d);
                    }
                }
            }
            return list;
        }
        catch
        {
            var names = trimmed.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Where(s => s.Length > 0).ToList();
            return names.Select(name =>
            {
                var d = InferDescriptor(name, entityClassName);
                d.TsInterfaceName = GetTsInterfaceName(name, entityNamePascal);
                d.TsExtendsName = GetTsExtendsName(d.BodyKind, entityNamePascal);
                return d;
            }).ToList();
        }
    }

    /// <summary>
    /// 根据 BodyKind 返回前端 extends 类型名。
    /// </summary>
    private static string GetTsExtendsName(
        string bodyKind,
        string entityNamePascal,
        string tsEntityBaseInterface = "TaktTenantEntityBase",
        string dtoName = "")
    {
        if (string.Equals(dtoName, "UpdateDto", StringComparison.Ordinal) && bodyKind == "OnlyId")
        {
            return entityNamePascal + "Create";
        }
        return bodyKind switch
        {
            "Full" => tsEntityBaseInterface,
            "Query" => "TaktPagedQuery",
            _ => string.Empty
        };
    }

    /// <summary>
    /// DTO 类别名转前端 TypeScript 接口名（如 Dto→Dept，QueryDto→DeptQuery）。
    /// </summary>
    private static string GetTsInterfaceName(string dtoName, string entityNamePascal)
    {
        if (string.IsNullOrEmpty(dtoName)) return entityNamePascal;
        return dtoName switch
        {
            "Dto" => entityNamePascal,
            "QueryDto" => entityNamePascal + "Query",
            "CreateDto" => entityNamePascal + "Create",
            "UpdateDto" => entityNamePascal + "Update",
            "TemplateDto" => entityNamePascal + "Template",
            "ImportDto" => entityNamePascal + "Import",
            "ExportDto" => entityNamePascal + "Export",
            "StatusDto" => entityNamePascal + "Status",
            "SortDto" => entityNamePascal + "Sort",
            _ => entityNamePascal + dtoName.Replace("Dto", "", StringComparison.OrdinalIgnoreCase)
        };
    }

    /// <summary>
    /// 仅当数组项为字符串时用于推断 DTO 形状。IsCreate/IsUpdate/IsExport/IsQuery/IsUnique 与前端一致；仅 IsList 影响前端。
    /// </summary>
    /// <param name="name">DTO 类别名（如 Dto、QueryDto、CreateDto、UpdateDto）</param>
    /// <param name="entityClassName">实体类名（用于解析 UpdateDto 基类等）</param>
    /// <returns>含 Name、BodyKind、BaseClass 的描述符</returns>
    private static TaktDtoCategoryDescriptor InferDescriptor(string name, string entityClassName)
    {
        var (bodyKind, baseClass) = name switch
        {
            "Dto" => ("Full", (string?)null),
            "QueryDto" => ("Query", "TaktPagedQuery"),
            "CreateDto" => ("NoId", (string?)null),
            "UpdateDto" => ("OnlyId", entityClassName + "CreateDto"),
            "TemplateDto" => ("NoId", (string?)null),
            "ImportDto" => ("NoId", (string?)null),
            "ExportDto" => ("Export", (string?)null),
            "StatusDto" => ("NoId", (string?)null),
            "SortDto" => ("NoId", (string?)null),
            _ when name.Contains("Template", StringComparison.OrdinalIgnoreCase) => ("NoId", (string?)null),
            _ => ("Full", (string?)null)
        };
        return new TaktDtoCategoryDescriptor { Name = name, BodyKind = bodyKind, BaseClass = baseClass };
    }

    /// <summary>
    /// 实体类名转前端帕斯卡名（去掉 Takt 前缀，如 TaktDept→Dept）。
    /// </summary>
    private static string ToEntityNamePascal(string? entityClassName)
    {
        if (string.IsNullOrEmpty(entityClassName)) return string.Empty;
        if (entityClassName.StartsWith("Takt", StringComparison.Ordinal) && entityClassName.Length > 4)
            return entityClassName[4..];
        return entityClassName;
    }

    /// <summary>
    /// 帕斯卡名转驼峰（首字母小写，如 Dept→dept）。
    /// </summary>
    private static string ToEntityNameCamel(string pascal)
    {
        if (string.IsNullOrEmpty(pascal)) return string.Empty;
        return char.ToLowerInvariant(pascal[0]) + pascal[1..];
    }

    /// <summary>
    /// 根据 GenModuleName 生成 API 中 import request 的相对路径（api/{modulePath}/xx.ts 需若干 ../ 回到 api 再取 request）。
    /// </summary>
    private static string BuildRequestImportPath(string? genModuleName)
    {
        if (string.IsNullOrWhiteSpace(genModuleName)) return "../request";
        var segmentCount = genModuleName!.Trim().Split(new[] { '_', '/', ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
        return string.Concat(System.Linq.Enumerable.Repeat("../", segmentCount)) + "request";
    }

    /// <summary>
    /// 帕斯卡名转 kebab（如 StandardWageRate→standard-wage-rate，用于前端表单组件文件名）。
    /// </summary>
    private static string ToEntityNameKebab(string? pascal)
    {
        if (string.IsNullOrWhiteSpace(pascal)) return "entity";
        var s = pascal!.Trim();
        if (s.Length == 0) return "entity";
        var sb = new System.Text.StringBuilder();
        for (var i = 0; i < s.Length; i++)
        {
            var c = s[i];
            if (char.IsUpper(c))
            {
                if (i > 0) sb.Append('-');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
                sb.Append(c);
        }
        return sb.ToString();
    }

    /// <summary>
    /// 排序字段帕斯卡转驼峰（供前端 API，如 CreatedAt→createdAt）。
    /// </summary>
    private static string ToTsSortField(string? sortField)
    {
        if (string.IsNullOrEmpty(sortField)) return string.Empty;
        return char.ToLowerInvariant(sortField[0]) + sortField[1..];
    }

    /// <summary>
    /// 从 TaktGenTable.PermsPrefix 解析时，若误将第四段 key 写入前缀则剥离用。
    /// </summary>
    private static readonly HashSet<string> KnownPermissionKeySuffixes = new(StringComparer.OrdinalIgnoreCase)
    {
        "list", "query", "create", "update", "delete", "import", "export", "page",
        "status", "info", "resetpwd", "changepwd", "reset", "change", "unlock"
    };

    private static bool IsKnownPermissionKeySuffix(string segment) =>
        !string.IsNullOrEmpty(segment) && KnownPermissionKeySuffixes.Contains(segment);

    /// <summary>
    /// 由表配置 <paramref name="permsPrefix"/> 得到 PermsPrefixCanonical（三段、小写、冒号分隔）。<b>非空时仅解析本参数</b>；仅当其为空或未得到任何段时，才用 <paramref name="genModuleName"/> 与 <paramref name="entityClassName"/> 推导。完整权限码 = 返回值 + ":" + key。
    /// </summary>
    private static string BuildPermsPrefixCanonical(string? permsPrefix, string? genModuleName, string? entityClassName)
    {
        // 权限组合：完整码 = 本方法返回值（模板字段 perms_prefix_canonical / PermsPrefixCanonical）+ ":" + key。
        // 必须先处理库表 perms_prefix（实参 permsPrefix）：trim 后非空则只解析、规范化该串，不以 genModuleName 覆盖用户已填各段；仅当其为空或未解析出任何段时，才用 genModuleName + 实体名推导三段。
        var ep = ToEntityNamePascal(entityClassName);
        var entitySeg = string.IsNullOrEmpty(ep) ? "entity" : ToEntityNameCamel(ep).ToLowerInvariant();

        var raw = (permsPrefix ?? string.Empty).Trim();
        // 无下划线时：按冒号分段，尊重用户已配置的三段（或误填四段时去掉末尾已知 key 后取前三段）
        if (!string.IsNullOrEmpty(raw) && !raw.Contains('_'))
        {
            var colonParts = raw.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => s.ToLowerInvariant())
                .Where(s => s.Length > 0)
                .ToList();
            while (colonParts.Count >= 3 && IsKnownPermissionKeySuffix(colonParts[^1]))
                colonParts.RemoveAt(colonParts.Count - 1);
            if (colonParts.Count >= 3)
                return string.Join(':', colonParts.Take(3));
        }

        var segs = new List<string>();
        if (!string.IsNullOrEmpty(raw))
        {
            if (raw.Contains(':'))
            {
                foreach (var block in raw.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    foreach (var part in block.Split(new[] { '_', '.', '/', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                        segs.Add(part.ToLowerInvariant());
                }
            }
            else if (!raw.Contains('_') && raw.Contains('.'))
            {
                foreach (var part in raw.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    segs.Add(part.ToLowerInvariant());
            }
            else
            {
                foreach (var part in raw.Split(new[] { '_', '.', ':', '/', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    segs.Add(part.ToLowerInvariant());
            }
        }

        if (segs.Count == 0)
        {
            foreach (var part in (genModuleName ?? string.Empty).Trim().Split(new[] { '_', '.', '/', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                segs.Add(part.ToLowerInvariant());
        }

        if (segs.Count == 0)
            segs.Add("app");

        if (segs.Count == 1)
        {
            segs.Add("data");
            segs.Add(entitySeg);
        }
        else if (segs.Count == 2)
        {
            if (string.Equals(segs[1], entitySeg, StringComparison.Ordinal))
                segs.Insert(1, "core");
            else
                segs.Add(entitySeg);
        }

        if (segs.Count > 3)
            segs = segs.Take(3).ToList();

        while (segs.Count < 3)
            segs.Add("data");

        return string.Join(':', segs);
    }

    /// <summary>
    /// 与属性 PermsPrefixCanonical 的计算规则同源（私有 <c>BuildPermsPrefixCanonical</c>）。
    /// 供导入表等工作流在 TaktGenTable.PermsPrefix 为空时写入推荐的三段基点（领域:目录:实体，小写冒号）。
    /// </summary>
    /// <param name="permsPrefix">与库表 <c>perms_prefix</c> 一致；传 <see langword="null"/> 或空白表示仅按模块名与实体类名推导。</param>
    /// <param name="genModuleName">生成模块名（如 <c>logistics_materials</c>）。</param>
    /// <param name="entityClassName">实体类名（如 <c>TaktPlant</c>）。</param>
    /// <returns>规范化三段、小写、冒号分隔。</returns>
    public static string ResolvePermsPrefixCanonical(string? permsPrefix, string? genModuleName, string? entityClassName)
        => BuildPermsPrefixCanonical(permsPrefix, genModuleName, entityClassName);

    /// <summary>
    /// 控制器类名转 API 基础路径（去掉 Controller 后缀，如 TaktDeptsController→/api/TaktDepts）。
    /// </summary>
    private static string ToApiBasePath(string? controllerClassName)
    {
        if (string.IsNullOrEmpty(controllerClassName)) return "/api";
        var name = controllerClassName.EndsWith("Controller", StringComparison.Ordinal) && controllerClassName.Length >= 10
            ? controllerClassName[..^10]
            : controllerClassName;
        return "/api/" + name;
    }

    private static readonly Dictionary<string, string> ApiModuleTopLevelNames = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Accounting", "会计核算" },
        { "Identity", "身份认证" },
        { "Logistics", "后勤管理" },
        { "Generator", "代码管理" },
        { "Routine", "日常事务" },
        { "Tenant", "租户管理" },
        { "HumanResource", "人力资源" },
        { "Statistics", "统计看板" },
        { "Workflow", "工作流程" },
        { "Controlling", "控制" }
    };

    /// <summary>
    /// 从 GenModuleName 解析 [ApiModule] 的顶级键与名称（首段 PascalCase + 预定义中文名）。
    /// </summary>
    private static (string Key, string Name) GetApiModuleTopLevel(string? genModuleName)
    {
        if (string.IsNullOrWhiteSpace(genModuleName))
            return ("Default", "通用");
        var segment = genModuleName.Trim().Split('_', '.')[0];
        if (string.IsNullOrEmpty(segment))
            return ("Default", "通用");
        var key = char.ToUpperInvariant(segment[0]) + segment[1..].ToLowerInvariant();
        var name = ApiModuleTopLevelNames.TryGetValue(key, out var n) ? n : "通用";
        return (key, name);
    }

    /// <summary>
    /// 从 GenModuleName 解析 TaktModule 枚举成员名
    /// </summary>
    private static string ResolveApiModuleEnum(string? genModuleName)
    {
        var (key, _) = GetApiModuleTopLevel(genModuleName);
        return key.ToLowerInvariant() switch
        {
            "humanresource" => "HumanResource",
            "statistics" => "Statistics",
            "identity" => "Identity",
            "logistics" => "Logistics",
            "routine" => "Routine",
            "accounting" => "Accounting",
            "workflow" => "Workflow",
            "foundation" => "Foundation",
            "dashboard" => "Dashboard",
            "generator" or "code" => "Code",
            "tenant" => "Identity",
            _ => "Routine"
        };
    }

    /// <summary>
    /// GenFunction 别名 → 模板引擎标准键（PascalCase，与 BuildControllerActions / DtoCategoryDescriptors 一致）。
    /// </summary>
    private static readonly Dictionary<string, string> GenFunctionKeyAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["query"] = "Query",
        ["view"] = "View",
        ["查询"] = "Query",
        ["查看"] = "View",
        ["create"] = "Create",
        ["新增"] = "Create",
        ["update"] = "Update",
        ["更新"] = "Update",
        ["delete"] = "Delete",
        ["删除"] = "Delete",
        ["status"] = "Status",
        ["状态"] = "Status",
        ["sort"] = "Sort",
        ["排序"] = "Sort",
        ["template"] = "Template",
        ["模板"] = "Template",
        ["import"] = "Import",
        ["导入"] = "Import",
        ["export"] = "Export",
        ["导出"] = "Export",
    };

    /// <summary>
    /// 将字典 value（小写 query/create）、中文标签或 PascalCase 统一为标准功能键。
    /// </summary>
    /// <param name="raw">原始功能键</param>
    /// <returns>标准键 Query/Create/…；无法识别时返回 trim 后原值</returns>
    private static string CanonicalizeGenFunctionKey(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return string.Empty;
        var trimmed = raw.Trim();
        return GenFunctionKeyAliases.TryGetValue(trimmed, out var canonical) ? canonical : trimmed;
    }

    /// <summary>
    /// 去重并规范 GenFunction 键列表。
    /// </summary>
    private static List<string> NormalizeGenFunctionKeys(IEnumerable<string> rawKeys) =>
        rawKeys
            .Select(CanonicalizeGenFunctionKey)
            .Where(k => k.Length > 0)
            .Distinct(StringComparer.Ordinal)
            .ToList();

    /// <summary>
    /// 解析 GenFunction 为功能键列表。支持：JSON 对象（取 key 与 string value）；JSON 数组；逗号分隔。字典 code_generator_function 的 value 为小写 query/create，解析后统一为 Query/Create。
    /// </summary>
    /// <param name="genFunction">GenFunction 原始字符串（JSON 对象/数组或逗号分隔）</param>
    /// <returns>标准功能键列表（Query、Create、Update、Delete 等）</returns>
    private static List<string> ParseGenFunctionKeys(string? genFunction)
    {
        if (string.IsNullOrWhiteSpace(genFunction))
            return new List<string>();
        var trimmed = genFunction.Trim();

        if (trimmed.StartsWith('{'))
        {
            try
            {
                var jobj = JObject.Parse(trimmed);
                var raw = jobj.Properties().SelectMany(p =>
                {
                    var name = p.Name ?? string.Empty;
                    if (p.Value?.Type == JTokenType.String)
                        return new[] { name, p.Value.ToString() };
                    return new[] { name };
                });
                return NormalizeGenFunctionKeys(raw);
            }
            catch { }
        }

        if (trimmed.StartsWith('['))
        {
            try
            {
                var arr = JArray.Parse(trimmed);
                var raw = arr
                    .Where(el => el?.Type == JTokenType.String)
                    .Select(el => el!.ToString());
                return NormalizeGenFunctionKeys(raw);
            }
            catch { }
        }
        var split = trimmed.Split(new[] { ',', '，', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => s.Length > 0);
        return NormalizeGenFunctionKeys(split);
    }

    /// <summary>
    /// 功能键列表中是否包含"Query"或"View"，用于生成查询相关 Action/方法。
    /// </summary>
    /// <param name="keys">GenFunction 解析得到的功能键列表</param>
    /// <returns>包含查询/查看时返回 true</returns>
    private static bool HasQueryKey(List<string> keys) =>
        keys.Exists(k => k.Equals("Query", StringComparison.Ordinal) || k.Equals("View", StringComparison.Ordinal));

    /// <summary>
    /// 根据 GenFunction 功能键列表，向表模型追加控制器 Action 描述符（GetList、GetById、Create、Update、Delete 等）。
    /// </summary>
    /// <param name="m">表级模板模型</param>
    /// <param name="keys">功能键列表（如 查询、新增、更新、删除）</param>
    private static void BuildControllerActions(TaktGenTableTemplateModel m, List<string> keys)
    {
        var permBase = m.PermsPrefixCanonical;
        var entity = m.EntityClassName;
        var funcName = m.GenFunctionName ?? m.GenBusinessName ?? string.Empty;
        var ep = m.EntityNamePascal;
        var ec = m.EntityNameCamel;
        if (HasQueryKey(keys))
        {
            m.ControllerActions.Add(new TaktGenControllerActionDescriptor
            {
                Summary = $"获取{funcName}列表（分页）",
                HttpMethod = "HttpGet",
                Route = "list",
                PermissionKey = $"{permBase}:list",
                PermissionName = $"查询{funcName}列表",
                MethodName = $"Get{ep}ListAsync",
                FrontendMethodName = $"get{ep}List",
                FrontendSignature = $"params: {ep}Query",
                FrontendReturnType = $"Promise<TaktPagedResult<{ep}>>",
                FrontendRequestKey = "params",
                Signature = $"[FromQuery] {entity}QueryDto queryDto",
                ResponseType = "IActionResult",
                Body = "try\n        {\n            var result = await _" + ec + "Service.Get" + ep + "ListAsync(queryDto);\n            return Success(result.Data, result.Total, result.PageIndex, result.PageSize, \"查询成功\");\n        }\n        catch (Exception ex)\n        {\n            return HandleException(ex);\n        }"
            });
            m.ControllerActions.Add(new TaktGenControllerActionDescriptor
            {
                Summary = $"根据ID获取{funcName}",
                HttpMethod = "HttpGet",
                Route = "{id}",
                PermissionKey = $"{permBase}:query",
                PermissionName = $"查询{funcName}详情",
                MethodName = $"Get{ep}ByIdAsync",
                FrontendMethodName = $"get{ep}ById",
                FrontendSignature = "id: string",
                FrontendReturnType = $"Promise<{ep}>",
                FrontendRequestKey = "",
                Signature = "long id",
                ResponseType = "IActionResult",
                Body = "try\n        {\n            var result = await _" + ec + "Service.Get" + ep + "ByIdAsync(id);\n            if (result == null)\n            {\n                return NotFound(\"" + funcName + "不存在\");\n            }\n            return Success(result, \"查询成功\");\n        }\n        catch (Exception ex)\n        {\n            return HandleException(ex);\n        }"
            });
            m.ControllerActions.Add(new TaktGenControllerActionDescriptor
            {
                Summary = $"获取{funcName}下拉选项",
                HttpMethod = "HttpGet",
                Route = "options",
                PermissionKey = $"{permBase}:query",
                PermissionName = $"查询{funcName}下拉选项",
                MethodName = $"Get{ep}OptionsAsync",
                FrontendMethodName = $"get{ep}Options",
                FrontendSignature = string.Empty,
                FrontendReturnType = "Promise<TaktSelectOption[]>",
                FrontendRequestKey = string.Empty,
                Signature = string.Empty,
                ResponseType = "IActionResult",
                Body = "try\n        {\n            var result = await _" + ec + "Service.Get" + ep + "OptionsAsync();\n            return Success(result, \"查询成功\");\n        }\n        catch (Exception ex)\n        {\n            return HandleException(ex);\n        }"
            });
        }
        if (keys.Any(k => k.Equals("Create", StringComparison.Ordinal)))
        {
            m.ControllerActions.Add(new TaktGenControllerActionDescriptor
            {
                Summary = $"创建{funcName}",
                HttpMethod = "HttpPost",
                Route = "",
                PermissionKey = $"{permBase}:create",
                PermissionName = $"创建{funcName}",
                MethodName = $"Create{ep}Async",
                FrontendMethodName = $"create{ep}",
                FrontendSignature = $"data: {ep}Create",
                FrontendReturnType = $"Promise<{ep}>",
                FrontendRequestKey = "data",
                Signature = $"[FromBody] {entity}CreateDto dto",
                ResponseType = "IActionResult",
                Body = "try\n        {\n            var result = await _" + ec + "Service.Create" + ep + "Async(dto);\n            return Success(result, \"创建成功\");\n        }\n        catch (Exception ex)\n        {\n            return HandleException(ex);\n        }"
            });
        }
        if (keys.Any(k => k.Equals("Update", StringComparison.Ordinal)))
        {
            m.ControllerActions.Add(new TaktGenControllerActionDescriptor
            {
                Summary = $"更新{funcName}",
                HttpMethod = "HttpPut",
                Route = "{id}",
                PermissionKey = $"{permBase}:update",
                PermissionName = $"更新{funcName}",
                MethodName = $"Update{ep}Async",
                FrontendMethodName = $"update{ep}",
                FrontendSignature = $"id: string, data: {ep}Update",
                FrontendReturnType = $"Promise<{ep}>",
                FrontendRequestKey = "data",
                Signature = $"long id, [FromBody] {entity}UpdateDto dto",
                ResponseType = "IActionResult",
                Body = "try\n        {\n            var result = await _" + ec + "Service.Update" + ep + "Async(id, dto);\n            return Success(result, \"更新成功\");\n        }\n        catch (Exception ex)\n        {\n            return HandleException(ex);\n        }"
            });
        }
        if (keys.Any(k => k.Equals("Delete", StringComparison.Ordinal)))
        {
            m.ControllerActions.Add(new TaktGenControllerActionDescriptor
            {
                Summary = $"删除{funcName}",
                HttpMethod = "HttpDelete",
                Route = "{id}",
                PermissionKey = $"{permBase}:delete",
                PermissionName = $"删除{funcName}",
                MethodName = $"Delete{ep}ByIdAsync",
                FrontendMethodName = $"delete{ep}ById",
                FrontendSignature = "id: string",
                FrontendReturnType = "Promise<void>",
                FrontendRequestKey = "",
                Signature = "long id",
                ResponseType = "IActionResult",
                Body = "try\n        {\n            await _" + ec + "Service.Delete" + ep + "ByIdAsync(id);\n            return Success(\"删除成功\");\n        }\n        catch (Exception ex)\n        {\n            return HandleException(ex);\n        }"
            });
            m.ControllerActions.Add(new TaktGenControllerActionDescriptor
            {
                Summary = $"批量删除{funcName}",
                HttpMethod = "HttpDelete",
                Route = "batch",
                PermissionKey = $"{permBase}:delete",
                PermissionName = $"批量删除{funcName}",
                MethodName = $"Delete{ep}BatchAsync",
                FrontendMethodName = $"delete{ep}Batch",
                FrontendSignature = "ids: string[]",
                FrontendReturnType = "Promise<void>",
                FrontendRequestKey = "data: ids",
                Signature = "[FromBody] IEnumerable<long> ids",
                ResponseType = "IActionResult",
                Body = "try\n        {\n            await _" + ec + "Service.Delete" + ep + "BatchAsync(ids);\n            return Success(\"删除成功\");\n        }\n        catch (Exception ex)\n        {\n            return HandleException(ex);\n        }"
            });
        }
        if (keys.Any(k => k.Equals("Status", StringComparison.Ordinal)))
        {
            m.ControllerActions.Add(new TaktGenControllerActionDescriptor
            {
                Summary = $"更新{funcName}状态",
                HttpMethod = "HttpPut",
                Route = "status",
                PermissionKey = $"{permBase}:status",
                PermissionName = $"更新{funcName}状态",
                MethodName = $"Update{ep}StatusAsync",
                FrontendMethodName = $"update{ep}Status",
                FrontendSignature = "id: string, status: number",
                FrontendReturnType = "Promise<void>",
                FrontendRequestKey = "data: { id, status }",
                Signature = $"long id, [FromBody] {entity}StatusDto dto",
                ResponseType = "IActionResult",
                Body = "try\n        {\n            await _" + ec + "Service.Update" + ep + "StatusAsync(id, dto);\n            return Success(\"更新成功\");\n        }\n        catch (Exception ex)\n        {\n            return HandleException(ex);\n        }"
            });
        }
        if (keys.Any(k => k.Equals("Sort", StringComparison.Ordinal)))
        {
            m.ControllerActions.Add(new TaktGenControllerActionDescriptor
            {
                Summary = $"更新{funcName}排序",
                HttpMethod = "HttpPut",
                Route = "sort",
                PermissionKey = $"{permBase}:sort",
                PermissionName = $"更新{funcName}排序",
                MethodName = $"Update{ep}SortAsync",
                FrontendMethodName = $"update{ep}Sort",
                FrontendSignature = "id: string, sort: number",
                FrontendReturnType = "Promise<void>",
                FrontendRequestKey = "data: { id, sort }",
                Signature = $"long id, [FromBody] {entity}SortDto dto",
                ResponseType = "IActionResult",
                Body = "try\n        {\n            await _" + ec + "Service.Update" + ep + "SortAsync(id, dto);\n            return Success(\"更新成功\");\n        }\n        catch (Exception ex)\n        {\n            return HandleException(ex);\n        }"
            });
        }
        if (keys.Any(k => k.Equals("Template", StringComparison.Ordinal)))
        {
            m.ControllerActions.Add(new TaktGenControllerActionDescriptor
            {
                Summary = "获取导入模板",
                HttpMethod = "HttpGet",
                Route = "template",
                PermissionKey = $"{permBase}:import",
                PermissionName = "获取导入模板",
                MethodName = $"Get{ep}TemplateAsync",
                FrontendMethodName = $"get{ep}Template",
                FrontendSignature = "sheetName?: string, fileName?: string",
                FrontendReturnType = "Promise<Blob>",
                FrontendRequestKey = "params",
                Signature = "[FromQuery] string? sheetName = null, [FromQuery] string? fileName = null",
                ResponseType = "IActionResult",
                Body = "try\n        {\n            var (resultFileName, content) = await _" + ec + "Service.Get" + ep + "TemplateAsync(sheetName, fileName);\n            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);\n        }\n        catch (Exception ex)\n        {\n            return HandleException(ex);\n        }"
            });
        }
        if (keys.Any(k => k.Equals("Import", StringComparison.Ordinal)))
        {
            m.ControllerActions.Add(new TaktGenControllerActionDescriptor
            {
                Summary = $"导入{funcName}",
                HttpMethod = "HttpPost",
                Route = "import",
                PermissionKey = $"{permBase}:import",
                PermissionName = $"导入{funcName}",
                MethodName = $"Import{ep}Async",
                FrontendMethodName = $"import{ep}Data",
                FrontendSignature = "file: File, sheetName?: string",
                FrontendReturnType = "Promise<{ success: number, fail: number, errors: string[] }>",
                FrontendRequestKey = "formData",
                Signature = "IFormFile file, [FromQuery] string? sheetName = null",
                ResponseType = "IActionResult",
                Body = "try\n        {\n            if (file == null || file.Length == 0)\n            {\n                return BadRequest(\"请选择要导入的文件\");\n            }\n            await using var stream = file.OpenReadStream();\n            var (success, fail, errors) = await _" + ec + "Service.Import" + ep + "Async(stream, sheetName);\n            return Success(new { SuccessCount = success, FailCount = fail, Errors = errors }, $\"导入完成：成功{success}条，失败{fail}条\");\n        }\n        catch (Exception ex)\n        {\n            return HandleException(ex);\n        }"
            });
        }
        if (keys.Any(k => k.Equals("Export", StringComparison.Ordinal)))
        {
            m.ControllerActions.Add(new TaktGenControllerActionDescriptor
            {
                Summary = $"导出{funcName}",
                HttpMethod = "HttpGet",
                Route = "export",
                PermissionKey = $"{permBase}:export",
                PermissionName = $"导出{funcName}",
                MethodName = $"Export{ep}Async",
                FrontendMethodName = $"export{ep}Data",
                FrontendSignature = $"query?: {ep}Query, sheetName?: string, fileName?: string",
                FrontendReturnType = "Promise<Blob>",
                FrontendRequestKey = "params",
                Signature = $"[FromQuery] {entity}QueryDto? query = null, [FromQuery] string? sheetName = null, [FromQuery] string? fileName = null",
                ResponseType = "IActionResult",
                Body = "try\n        {\n            var (resultFileName, content) = await _" + ec + "Service.Export" + ep + "Async(query, sheetName, fileName);\n            return File(content, TaktExcelHelper.ExcelContentType, resultFileName);\n        }\n        catch (Exception ex)\n        {\n            return HandleException(ex);\n        }"
            });
        }
    }

    /// <summary>
    /// 根据 GenFunction 功能键列表，向表模型追加服务方法描述符（接口+实现）及私有方法（如 QueryExpression）。
    /// </summary>
    /// <param name="m">表级模板模型</param>
    /// <param name="keys">功能键列表（如 查询、新增、更新、删除）</param>
    private static void BuildServiceMethods(TaktGenTableTemplateModel m, List<string> keys)
    {
        var entity = m.EntityClassName;
        var funcName = m.GenFunctionName ?? m.GenBusinessName ?? string.Empty;
        var ep = m.EntityNamePascal;
        var ec = m.EntityNameCamel;
        var repo = $"_{ec}Repository";
        var tier = m.EntityBaseTier;
        var getByIdGuard = tier is TaktGenEntityBaseProfile.TierCompany or TaktGenEntityBaseProfile.TierApproval
            ? "if (entity == null || entity.TenantCode != CurrentTenantCode || entity.CompanyCode != CurrentCompanyCode)"
            : "if (entity == null || entity.TenantCode != CurrentTenantCode)";
        if (HasQueryKey(keys))
        {
            m.ServiceMethods.Add(new TaktGenServiceMethodDescriptor
            {
                Summary = $"获取{funcName}列表（分页）",
                ParamsXml = "<param name=\"queryDto\">查询DTO</param>\n    /// <returns>分页结果</returns>",
                MethodName = $"Get{ep}ListAsync",
                Signature = $"{entity}QueryDto queryDto",
                ReturnType = $"Task<TaktPagedResult<{entity}Dto>>",
                Body = "var predicate = QueryExpression(queryDto);\n        var (data, total) = await " + repo + ".GetPagedAsync(queryDto.PageIndex, queryDto.PageSize, predicate);\n        return TaktPagedResult<" + entity + "Dto>.Create(\n            data.Adapt<List<" + entity + "Dto>>(),\n            total,\n            queryDto.PageIndex,\n            queryDto.PageSize);"
            });
            m.ServiceMethods.Add(new TaktGenServiceMethodDescriptor
            {
                Summary = $"根据ID获取{funcName}",
                ParamsXml = $"<param name=\"id\">{funcName}ID</param>\n    /// <returns>{funcName}DTO</returns>",
                MethodName = $"Get{ep}ByIdAsync",
                Signature = "long id",
                ReturnType = $"Task<{entity}Dto?>",
                Body = "var entity = await " + repo + ".GetByIdAsync(id);\n        " + getByIdGuard + "\n        {\n            return null;\n        }\n        return entity.Adapt<" + entity + "Dto>();"
            });
            m.ServicePrivateMethods.Add(new TaktGenServiceMethodDescriptor
            {
                MethodName = "QueryExpression",
                Signature = $"{entity}QueryDto queryDto",
                ReturnType = $"System.Linq.Expressions.Expression<Func<{entity}, bool>>",
                Body = "var exp = Expressionable.Create<" + entity + ">();\n        if (!string.IsNullOrEmpty(queryDto?.KeyWords))\n        {\n            exp = exp.And(x => x.Remark != null && x.Remark.Contains(queryDto.KeyWords));\n        }\n        return exp.ToExpression();",
                IsPrivate = true
            });
        }
        if (keys.Any(k => k.Equals("Create", StringComparison.Ordinal)))
        {
            m.ServiceMethods.Add(new TaktGenServiceMethodDescriptor
            {
                Summary = $"创建{funcName}",
                ParamsXml = $"<param name=\"dto\">创建{funcName}DTO</param>\n    /// <returns>{funcName}DTO</returns>",
                MethodName = $"Create{ep}Async",
                Signature = $"{entity}CreateDto dto",
                ReturnType = $"Task<{entity}Dto>",
                Body = "var entity = dto.Adapt<" + entity + ">();\n        entity = await " + repo + ".CreateAsync(entity);\n        return await Get" + ep + "ByIdAsync(entity.Id) ?? entity.Adapt<" + entity + "Dto>();"
            });
        }
        if (keys.Any(k => k.Equals("Update", StringComparison.Ordinal)))
        {
            m.ServiceMethods.Add(new TaktGenServiceMethodDescriptor
            {
                Summary = $"更新{funcName}",
                ParamsXml = $"<param name=\"id\">{funcName}ID</param>\n    /// <param name=\"dto\">更新{funcName}DTO</param>\n    /// <returns>{funcName}DTO</returns>",
                MethodName = $"Update{ep}Async",
                Signature = $"long id, {entity}UpdateDto dto",
                ReturnType = $"Task<{entity}Dto>",
                Body = "var entity = await " + repo + ".GetByIdAsync(id);\n        if (entity == null)\n            throw new TaktBusinessException(\"" + funcName + "不存在\");\n        dto.Adapt(entity);\n        await " + repo + ".UpdateAsync(entity);\n        return await Get" + ep + "ByIdAsync(id) ?? throw new TaktBusinessException(\"" + funcName + "不存在\");"
            });
        }
        if (keys.Any(k => k.Equals("Delete", StringComparison.Ordinal)))
        {
            m.ServiceMethods.Add(new TaktGenServiceMethodDescriptor
            {
                Summary = $"删除{funcName}",
                ParamsXml = $"<param name=\"id\">{funcName}ID</param>\n    /// <returns>任务</returns>",
                MethodName = $"Delete{ep}ByIdAsync",
                Signature = "long id",
                ReturnType = "Task",
                Body = "var deleted = await " + repo + ".DeleteAsync(id);\n        if (!deleted)\n        {\n            throw new TaktBusinessException(\"" + funcName + "不存在或已删除\");\n        }"
            });
            m.ServiceMethods.Add(new TaktGenServiceMethodDescriptor
            {
                Summary = $"批量删除{funcName}",
                ParamsXml = $"<param name=\"ids\">{funcName}ID列表</param>\n    /// <returns>任务</returns>",
                MethodName = $"Delete{ep}BatchAsync",
                Signature = "IEnumerable<long> ids",
                ReturnType = "Task",
                Body = "var idList = ids?.Distinct().ToList() ?? new List<long>();\n        if (idList.Count == 0) return;\n        foreach (var id in idList)\n        {\n            await Delete" + ep + "ByIdAsync(id);\n        }"
            });
        }
        if (keys.Any(k => k.Equals("Status", StringComparison.Ordinal)))
        {
            m.ServiceMethods.Add(new TaktGenServiceMethodDescriptor
            {
                Summary = $"更新{funcName}状态",
                ParamsXml = $"<param name=\"id\">{funcName}ID</param>\n    /// <param name=\"dto\">状态DTO</param>\n    /// <returns>任务</returns>",
                MethodName = $"Update{ep}StatusAsync",
                Signature = $"long id, {entity}StatusDto dto",
                ReturnType = "Task",
                Body = "var entity = await " + repo + ".GetByIdAsync(id);\n        if (entity == null)\n            throw new TaktBusinessException(\"" + funcName + "不存在\");\n        dto.Adapt(entity);\n        await " + repo + ".UpdateAsync(entity);"
            });
        }
        if (keys.Any(k => k.Equals("Sort", StringComparison.Ordinal)))
        {
            m.ServiceMethods.Add(new TaktGenServiceMethodDescriptor
            {
                Summary = $"更新{funcName}排序",
                ParamsXml = $"<param name=\"id\">{funcName}ID</param>\n    /// <param name=\"dto\">排序DTO</param>\n    /// <returns>任务</returns>",
                MethodName = $"Update{ep}SortAsync",
                Signature = $"long id, {entity}SortDto dto",
                ReturnType = "Task",
                Body = "var entity = await " + repo + ".GetByIdAsync(id);\n        if (entity == null)\n            throw new TaktBusinessException(\"" + funcName + "不存在\");\n        dto.Adapt(entity);\n        await " + repo + ".UpdateAsync(entity);"
            });
        }
        if (keys.Any(k => k.Equals("Template", StringComparison.Ordinal)))
        {
            m.ServiceMethods.Add(new TaktGenServiceMethodDescriptor
            {
                Summary = $"获取导入模板",
                ParamsXml = "<param name=\"sheetName\">工作表名称</param>\n    /// <param name=\"fileName\">文件名</param>\n    /// <returns>Excel模板文件信息（文件名和内容）</returns>",
                MethodName = $"Get{ep}TemplateAsync",
                Signature = "string? sheetName, string? fileName",
                ReturnType = "Task<(string fileName, byte[] content)>",
                Body = "return await TaktExcelHelper.GenerateTemplateAsync<" + entity + "TemplateDto>(\n            sheetName ?? \"" + funcName + "导入模板\",\n            fileName ?? \"" + funcName + "导入模板.xlsx\"\n        );"
            });
        }
        if (keys.Any(k => k.Equals("Import", StringComparison.Ordinal)))
        {
            m.ServiceMethods.Add(new TaktGenServiceMethodDescriptor
            {
                Summary = $"导入{funcName}",
                ParamsXml = "<param name=\"fileStream\">Excel文件流</param>\n    /// <param name=\"sheetName\">工作表名称</param>\n    /// <returns>导入结果（成功数量、失败数量、错误信息列表）</returns>",
                MethodName = $"Import{ep}Async",
                Signature = "Stream fileStream, string? sheetName",
                ReturnType = "Task<(int success, int fail, List<string> errors)>",
                Body = "var errors = new List<string>();\n        int success = 0;\n        int fail = 0;\n        var excelSheet = ResolveExcelSheetName(sheetName, nameof(" + entity + "));\n        var importData = await TaktExcelHelper.ImportAsync<" + entity + "ImportDto>(fileStream, excelSheet);\n        if (importData == null || importData.Count == 0)\n        {\n            AddImportError(errors, \"validation.importExcelNoData\");\n            return (0, 0, errors);\n        }\n        var rowIndex = 0;\n        foreach (var item in importData)\n        {\n            rowIndex++;\n            try\n            {\n                var e = item.Adapt<" + entity + ">();\n                await " + repo + ".CreateAsync(e);\n                success++;\n            }\n            catch (Exception ex)\n            {\n                fail++;\n                AddImportError(errors, \"validation.importRowUnhandledException\", rowIndex, GetLocalizedExceptionMessage(ex));\n            }\n        }\n        return (success, fail, errors);"
            });
        }
        if (keys.Any(k => k.Equals("Export", StringComparison.Ordinal)))
        {
            m.ServiceMethods.Add(new TaktGenServiceMethodDescriptor
            {
                Summary = $"导出{funcName}",
                ParamsXml = $"<param name=\"query\">{funcName}查询DTO（包含查询条件）</param>\n    /// <param name=\"sheetName\">工作表名称</param>\n    /// <param name=\"fileName\">文件名</param>\n    /// <returns>Excel文件信息（文件名和内容）</returns>",
                MethodName = $"Export{ep}Async",
                Signature = $"{entity}QueryDto? query, string? sheetName, string? fileName",
                ReturnType = "Task<(string fileName, byte[] content)>",
                Body = "var predicate = QueryExpression(query ?? new " + entity + "QueryDto());\n        var list = await " + repo + ".GetListForExportAsync(predicate);\n        if (list == null || list.Count == 0)\n        {\n            return await TaktExcelHelper.ExportAsync(\n                new List<" + entity + "ExportDto>(),\n                sheetName ?? \"" + funcName + "数据\",\n                fileName ?? \"" + funcName + "导出.xlsx\");\n        }\n        var exportData = list.Adapt<List<" + entity + "ExportDto>();\n        return await TaktExcelHelper.ExportAsync(\n            exportData,\n            sheetName ?? \"" + funcName + "数据\",\n            fileName ?? \"" + funcName + "导出.xlsx\");"
            });
        }
    }

    /// <summary>
    /// 从 TaktGenTable 实体构建表级模板模型。
    /// </summary>
    public static TaktGenTableTemplateModel From(TaktGenTable table)
    {
        if (table == null)
            return new TaktGenTableTemplateModel();
        var m = new TaktGenTableTemplateModel
        {
            DataSource = table.DataSource,
            TableName = table.TableName,
            TableComment = table.TableComment,
            SubTableName = table.SubTableName,
            SubTableFkName = table.SubTableFkName,
            TreeCode = table.TreeCode,
            TreeParentCode = table.TreeParentCode,
            TreeName = table.TreeName,
            InDatabase = table.InDatabase,
            GenTemplateCategory = table.GenTemplateCategory,
            NamePrefix = table.NamePrefix,
            EntityNamespace = table.EntityNamespace ?? "Takt.Domain.Entities",
            EntityClassName = table.EntityClassName,
            EntityNamePascal = ToEntityNamePascal(table.EntityClassName),
            EntityNameCamel = ToEntityNameCamel(ToEntityNamePascal(table.EntityClassName)),
            EntityNameKebab = ToEntityNameKebab(ToEntityNamePascal(table.EntityClassName)),
            ApiBasePath = ToApiBasePath(table.ControllerClassName),
            DtoNamespace = table.DtoNamespace ?? "Takt.Application.Dtos",
            DtoClassName = table.DtoClassName,
            // 从 GenFunction 自动推断 DTO 类别，不再依赖 DtoCategory 字段
            DtoCategories = new List<string>(),
            DtoCategoryDescriptors = new List<TaktDtoCategoryDescriptor>(),
            ServiceNamespace = table.ServiceNamespace ?? "Takt.Application.Services",
            IServiceClassName = table.IServiceClassName,
            ServiceClassName = table.ServiceClassName,
            ControllerNamespace = table.ControllerNamespace ?? "Takt.WebApi.Controllers",
            ControllerClassName = table.ControllerClassName,
            RepositoryInterfaceNamespace = table.RepositoryInterfaceNamespace,
            IRepositoryClassName = table.IRepositoryClassName,
            RepositoryNamespace = table.RepositoryNamespace,
            RepositoryClassName = table.RepositoryClassName,
            GenModuleName = table.GenModuleName,
            ApiModuleKey = GetApiModuleTopLevel(table.GenModuleName).Key,
            ApiModuleName = GetApiModuleTopLevel(table.GenModuleName).Name,
            FrontendModulePath = string.IsNullOrWhiteSpace(table.GenModuleName) ? "module" : table.GenModuleName.Trim().ToLowerInvariant().Replace('_', '/').Replace(".", "/"),
            RequestImportPath = BuildRequestImportPath(table.GenModuleName),
            GenBusinessName = table.GenBusinessName,
            GenFunctionName = table.GenFunctionName,
            GenFunction = table.GenFunction,
            GenMethod = table.GenMethod,
            IsRepository = table.IsRepository,
            GenPath = table.GenPath,
            ParentMenuId = table.ParentMenuId,
            IsGenMenu = table.IsGenMenu,
            IsGenTranslation = table.IsGenTranslation,
            SortField = table.SortField,
            SortType = table.SortType,
            TsSortField = ToTsSortField(table.SortField),
            PermsPrefix = table.PermsPrefix,
            PermsPrefixCanonical = BuildPermsPrefixCanonical(table.PermsPrefix, table.GenModuleName, table.EntityClassName),
            MenuButtonGroup = table.MenuButtonGroup,
            FrontUi = table.FrontUi,
            FrontFormLayout = table.FrontFormLayout,
            FrontBtnStyle = table.FrontBtnStyle,
            IsGenCode = table.IsGenCode,
            GenCodeCount = table.GenCodeCount,
            IsUseTabs = table.IsUseTabs,
            TabsFieldCount = table.TabsFieldCount,
            GenAuthor = table.GenAuthor ?? "Takt365",
            OtherGenOptions = table.OtherGenOptions
        };
        var baseProfile = TaktGenEntityBaseProfile.Resolve(table.OtherGenOptions);
        m.EntityBaseTier = baseProfile.Tier;
        m.EntityBaseClass = baseProfile.EntityBaseClass;
        m.DtoEntityBaseClass = baseProfile.DtoEntityBaseClass;
        m.TsEntityBaseInterface = baseProfile.TsEntityBaseInterface;
        m.RepositoryInterfaceName = baseProfile.RepositoryInterfaceName;
        m.ApiModuleEnum = ResolveApiModuleEnum(table.GenModuleName);
        var keys = ParseGenFunctionKeys(table.GenFunction);
        m.IsQuery = HasQueryKey(keys) ? 1 : 0;
        m.IsCreate = keys.Any(k => k.Equals("Create", StringComparison.Ordinal)) ? 1 : 0;
        m.IsUpdate = keys.Any(k => k.Equals("Update", StringComparison.Ordinal)) ? 1 : 0;
        m.IsDelete = keys.Any(k => k.Equals("Delete", StringComparison.Ordinal)) ? 1 : 0;
        m.IsStatus = keys.Any(k => k.Equals("Status", StringComparison.Ordinal)) ? 1 : 0;
        m.IsSort = keys.Any(k => k.Equals("Sort", StringComparison.Ordinal)) ? 1 : 0;
        m.IsTemplate = keys.Any(k => k.Equals("Template", StringComparison.Ordinal)) ? 1 : 0;
        m.IsImport = keys.Any(k => k.Equals("Import", StringComparison.Ordinal)) ? 1 : 0;
        m.IsExport = keys.Any(k => k.Equals("Export", StringComparison.Ordinal)) ? 1 : 0;
        
        // 根据 GenFunction 自动推断 DtoCategoryDescriptors
        var entityNamePascal = m.EntityNamePascal;
        var descriptors = new List<TaktDtoCategoryDescriptor>();
        
        // 所有功能都生成基础 Dto（Full）
        descriptors.Add(new TaktDtoCategoryDescriptor
        {
            Name = "Dto",
            BodyKind = "Full",
            BaseClass = baseProfile.DtoEntityBaseClass,
            TsInterfaceName = entityNamePascal,
            TsExtendsName = baseProfile.TsEntityBaseInterface
        });
        
        // Query → QueryDto
        if (m.IsQuery == 1)
        {
            descriptors.Add(new TaktDtoCategoryDescriptor
            {
                Name = "QueryDto",
                BodyKind = "Query",
                BaseClass = "TaktPagedQuery",
                TsInterfaceName = entityNamePascal + "Query",
                TsExtendsName = "TaktPagedQuery"
            });
        }
        
        // Create → CreateDto
        if (m.IsCreate == 1)
        {
            descriptors.Add(new TaktDtoCategoryDescriptor
            {
                Name = "CreateDto",
                BodyKind = "NoId",
                BaseClass = null,
                TsInterfaceName = entityNamePascal + "Create",
                TsExtendsName = string.Empty
            });
        }
        
        // Update → UpdateDto
        if (m.IsUpdate == 1)
        {
            descriptors.Add(new TaktDtoCategoryDescriptor
            {
                Name = "UpdateDto",
                BodyKind = "OnlyId",
                BaseClass = table.EntityClassName + "CreateDto",
                TsInterfaceName = entityNamePascal + "Update",
                TsExtendsName = m.IsCreate == 1 ? entityNamePascal + "Create" : string.Empty
            });
        }
        
        // Status → StatusDto
        if (m.IsStatus == 1)
        {
            descriptors.Add(new TaktDtoCategoryDescriptor
            {
                Name = "StatusDto",
                BodyKind = "OnlyId",
                BaseClass = null,
                TsInterfaceName = entityNamePascal + "Status",
                TsExtendsName = string.Empty
            });
        }
        
        // Sort → SortDto
        if (m.IsSort == 1)
        {
            descriptors.Add(new TaktDtoCategoryDescriptor
            {
                Name = "SortDto",
                BodyKind = "OnlyId",
                BaseClass = null,
                TsInterfaceName = entityNamePascal + "Sort",
                TsExtendsName = string.Empty
            });
        }
        
        // Import → TemplateDto + ImportDto
        if (m.IsImport == 1)
        {
            descriptors.Add(new TaktDtoCategoryDescriptor
            {
                Name = "TemplateDto",
                BodyKind = "NoId",
                BaseClass = null,
                TsInterfaceName = entityNamePascal + "Template",
                TsExtendsName = string.Empty
            });
            descriptors.Add(new TaktDtoCategoryDescriptor
            {
                Name = "ImportDto",
                BodyKind = "NoId",
                BaseClass = null,
                TsInterfaceName = entityNamePascal + "Import",
                TsExtendsName = string.Empty
            });
        }
        
        // Export → ExportDto
        if (m.IsExport == 1)
        {
            descriptors.Add(new TaktDtoCategoryDescriptor
            {
                Name = "ExportDto",
                BodyKind = "NoId",
                BaseClass = null,
                TsInterfaceName = entityNamePascal + "Export",
                TsExtendsName = string.Empty
            });
        }
        
        m.DtoCategoryDescriptors = descriptors;
        m.DtoCategories = descriptors.Select(d => d.Name).ToList();
        BuildControllerActions(m, keys);
        BuildServiceMethods(m, keys);
        return m;
    }
}

/// <summary>
/// 列级模板模型，与 TaktGenTableColumn 实体字段一一对应，供 Scriban 列循环使用并充分利用实体全部属性。
/// </summary>
public class TaktGenColumnTemplateModel
{
    /// <summary>
    /// 数据库列名称（snake_case）
    /// </summary>
    public string DatabaseColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 列描述（字段注释）
    /// </summary>
    public string? ColumnComment { get; set; }

    /// <summary>
    /// 列注释展示值（ColumnComment ?? CsharpColumnName，模板中直接用 col.Comment）
    /// </summary>
    public string Comment => string.IsNullOrWhiteSpace(ColumnComment) ? (CsharpColumnName ?? string.Empty) : ColumnComment;

    /// <summary>
    /// 数据库数据类型（如 varchar、int、datetime）
    /// </summary>
    public string DatabaseDataType { get; set; } = "nvarchar";

    /// <summary>
    /// C# 类型（如 string、int、DateTime、decimal）
    /// </summary>
    public string CsharpDataType { get; set; } = "string";

    /// <summary>
    /// C# 列名（帕斯卡命名）
    /// </summary>
    public string CsharpColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 前端列名（驼峰命名，供 TypeScript/API 使用）
    /// </summary>
    public string TsColumnName { get; set; } = string.Empty;

    /// <summary>
    /// 前端类型（string、number、boolean，供 TypeScript 使用）
    /// </summary>
    public string TsDataType { get; set; } = "string";

    /// <summary>
    /// C# 长度（字符串长度或数值整数位）
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// C# 小数位数
    /// </summary>
    public int DecimalDigits { get; set; }

    /// <summary>
    /// 是否主键（0=否，1=是）
    /// </summary>
    public int IsPk { get; set; }

    /// <summary>
    /// 是否自增（0=否，1=是）
    /// </summary>
    public int IsIncrement { get; set; }

    /// <summary>
    /// 是否必填（0=否，1=是）
    /// </summary>
    public int IsRequired { get; set; }

    /// <summary>
    /// 是否为新增字段（0=否，1=是）
    /// </summary>
    public int IsCreate { get; set; }

    /// <summary>
    /// 是否更新字段（0=否，1=是）
    /// </summary>
    public int IsUpdate { get; set; }

    /// <summary>
    /// 是否查重字段（0=否，1=是）
    /// </summary>
    public int IsUnique { get; set; }

    /// <summary>
    /// 是否列表字段（0=否，1=是）
    /// </summary>
    public int IsList { get; set; }

    /// <summary>
    /// 是否导出字段（0=否，1=是）
    /// </summary>
    public int IsExport { get; set; }

    /// <summary>
    /// 是否排序字段（0=否，1=是）
    /// </summary>
    public int IsSort { get; set; }

    /// <summary>
    /// 是否查询字段（0=否，1=是）
    /// </summary>
    public int IsQuery { get; set; }

    /// <summary>
    /// 查询方式（EQ=等于，NE=不等于，GT=大于，LT=小于，LIKE=模糊，BETWEEN=范围）
    /// </summary>
    public string QueryType { get; set; } = "LIKE";

    /// <summary>
    /// 显示类型（input、textarea、select、date、switch 等）
    /// </summary>
    public string HtmlType { get; set; } = "input";

    /// <summary>
    /// 字典类型（关联数据字典）
    /// </summary>
    public string? DictType { get; set; }

    /// <summary>
    /// 行号（项号/序号，固定步长=10）
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// 帕斯卡转驼峰（供前端列名使用）。
    /// </summary>
    private static string ToTsColumnName(string pascal)
    {
        if (string.IsNullOrEmpty(pascal)) return string.Empty;
        return char.ToLowerInvariant(pascal[0]) + pascal[1..];
    }

    /// <summary>
    /// C# 类型转 TypeScript 类型（string、number、boolean）。
    /// </summary>
    private static string ToTsDataType(string csharpType)
    {
        if (string.IsNullOrEmpty(csharpType)) return "string";
        var t = csharpType.Trim();
        if (t.Equals("bool", StringComparison.OrdinalIgnoreCase)) return "boolean";
        if (t.Equals("int", StringComparison.OrdinalIgnoreCase) || t.Equals("long", StringComparison.OrdinalIgnoreCase)
            || t.Equals("decimal", StringComparison.OrdinalIgnoreCase) || t.Equals("float", StringComparison.OrdinalIgnoreCase)
            || t.Equals("double", StringComparison.OrdinalIgnoreCase)) return "number";
        return "string";
    }

    /// <summary>
    /// 从 TaktGenTableColumn 实体构建列级模板模型。
    /// </summary>
    /// <param name="column">代码生成字段配置实体</param>
    /// <returns>列级模板模型，供 Scriban 列循环使用</returns>
    public static TaktGenColumnTemplateModel From(TaktGenTableColumn column)
    {
        return new TaktGenColumnTemplateModel
        {
            DatabaseColumnName = column.DatabaseColumnName,
            ColumnComment = column.ColumnComment,
            DatabaseDataType = column.DatabaseDataType,
            CsharpDataType = column.CsharpDataType,
            CsharpColumnName = column.CsharpColumnName,
            TsColumnName = ToTsColumnName(column.CsharpColumnName),
            TsDataType = ToTsDataType(column.CsharpDataType),
            Length = column.Length,
            DecimalDigits = column.DecimalDigits,
            IsPk = column.IsPk,
            IsIncrement = column.IsIncrement,
            IsRequired = column.IsRequired,
            IsCreate = column.IsCreate,
            IsUpdate = column.IsUpdate,
            IsUnique = column.IsUnique,
            IsList = column.IsList,
            IsExport = column.IsExport,
            IsSort = column.IsSort,
            IsQuery = column.IsQuery,
            QueryType = column.QueryType,
            HtmlType = column.HtmlType,
            DictType = column.DictType,
            LineNumber = column.LineNumber
        };
    }
}

/// <summary>
/// 代码生成实体/DTO 基类层级解析（对齐 TaktTenantEntityBase / TaktCompanyEntityBase / TaktApprovalEntityBase）
/// </summary>
public static class TaktGenEntityBaseProfile
{
    /// <summary>
    /// 租户级
    /// </summary>
    public const string TierTenant = "tenant";

    /// <summary>
    /// 公司级
    /// </summary>
    public const string TierCompany = "company";

    /// <summary>
    /// 审批级
    /// </summary>
    public const string TierApproval = "approval";

    private static readonly string[] TenantImportColumns =
    {
        "id", "tenant_code", "ext_field", "remark",
        "created_by", "created_at", "updated_by", "updated_at",
        "is_deleted", "deleted_by", "deleted_at"
    };

    private static readonly string[] CompanyImportColumns = { "company_code" };

    private static readonly string[] ApprovalImportColumns =
    {
        "approval_status", "initiator_id", "initiated_at", "approval_opinion", "approved_by", "approved_at"
    };

    /// <summary>
    /// 导入表时排除的全部基类列（三档基类并集 + 历史别名列）
    /// </summary>
    public static IReadOnlyCollection<string> AllImportColumnNames { get; } = TenantImportColumns
        .Concat(CompanyImportColumns)
        .Concat(ApprovalImportColumns)
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 从 OtherGenOptions JSON 解析 entityBaseTier，默认 tenant
    /// </summary>
    /// <param name="otherGenOptions">OtherGenOptions 原始 JSON</param>
    /// <returns>基类配置</returns>
    public static TaktGenEntityBaseProfileResult Resolve(string? otherGenOptions)
    {
        var tier = TierTenant;
        if (!string.IsNullOrWhiteSpace(otherGenOptions))
        {
            try
            {
                var jobj = JObject.Parse(otherGenOptions.Trim());
                var raw = jobj["entityBaseTier"]?.ToString()?.Trim().ToLowerInvariant();
                if (raw is TierCompany or TierApproval or TierTenant)
                {
                    tier = raw;
                }
            }
            catch
            {
                // 保持默认 tenant
            }
        }
        return tier switch
        {
            TierCompany => new TaktGenEntityBaseProfileResult(
                TierCompany,
                "TaktCompanyEntityBase",
                "TaktCompanyDtoBase",
                "TaktCompanyEntityBase",
                "ITaktCompanyRepository"),
            TierApproval => new TaktGenEntityBaseProfileResult(
                TierApproval,
                "TaktApprovalEntityBase",
                "TaktApprovalDtoBase",
                "TaktApprovalEntityBase",
                "ITaktApprovalRepository"),
            _ => new TaktGenEntityBaseProfileResult(
                TierTenant,
                "TaktTenantEntityBase",
                "TaktTenantDtoBase",
                "TaktTenantEntityBase",
                "ITaktTenantRepository")
        };
    }
}

/// <summary>
/// 实体基类层级解析结果
/// </summary>
/// <param name="Tier">tenant/company/approval</param>
/// <param name="EntityBaseClass">C# 实体基类</param>
/// <param name="DtoEntityBaseClass">C# Dto 基类</param>
/// <param name="TsEntityBaseInterface">前端 TS 基类接口</param>
/// <param name="RepositoryInterfaceName">仓储接口名</param>
public record TaktGenEntityBaseProfileResult(
    string Tier,
    string EntityBaseClass,
    string DtoEntityBaseClass,
    string TsEntityBaseInterface,
    string RepositoryInterfaceName);
