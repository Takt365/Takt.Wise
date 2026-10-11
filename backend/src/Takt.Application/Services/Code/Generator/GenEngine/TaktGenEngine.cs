// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Generator.GenEngine
// 文件名称：TaktGenEngine.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成核心引擎实现，使用 Scriban 解析并渲染模板
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Scriban;
using Scriban.Runtime;
using Takt.Shared.Helpers;
using System.Text.RegularExpressions;

namespace Takt.Application.Services.Code.Generator.GenEngine;

/// <summary>
/// 代码生成核心引擎：基于 Scriban 的模板解析与渲染。
/// 规范：模板使用小写 table/columns，属性名 snake_case（entity_namespace、perms_prefix_canonical、comment 等），与常见模板习惯一致。
/// </summary>
public class TaktGenEngine : ITaktGenEngine
{
    /// <summary>
    /// 使用给定模板内容与上下文模型渲染，返回生成后的文本。
    /// 全局变量：table、columns；表属性：entity_namespace、entity_class_name、comment、gen_author、table_name 等（snake_case）；默认值在引擎内设置，模板直接写 {{ table.entity_namespace }} 即可。
    /// </summary>
    public async Task<string> RenderAsync(string templateContent, object model)
    {
        if (string.IsNullOrWhiteSpace(templateContent))
            throw new ArgumentException("模板内容不能为空。", nameof(templateContent));

        TaktLogger.Debug("[CodeEngine] 开始渲染模板，内容长度={ContentLength}, 模型类型={ModelType}", templateContent.Length, model?.GetType().Name ?? "null");
        var template = Template.Parse(templateContent);
        if (template.HasErrors)
        {
            var messages = string.Join("; ", template.Messages.Select(m => m.Message));
            TaktLogger.Warning("[CodeEngine] 模板解析失败: {Messages}", messages);
            throw new TaktGenEngineException($"模板解析失败：{messages}");
        }

        var scriptContext = new TemplateContext { MemberRenamer = member => member.Name };
        if (model is TaktGenTemplateContext genContext)
        {
            var global = new ScriptObject();
            var tableModel = genContext.Table ?? new TaktGenTableTemplateModel();
            var cols = genContext.Columns ?? [];
            var baseProfile = TaktGenEntityBaseProfile.Resolve(tableModel.OtherGenOptions);

            // table / columns：小写 + snake_case，模板写 {{ table.entity_namespace }}、{{ col.comment }}
            var tableScript = new ScriptObject();
            tableScript.Import(tableModel, null, m => ToSnakeCase(m.Name));
            tableScript["entity_base_tier"] = baseProfile.Tier;
            tableScript["entity_base_class"] = baseProfile.EntityBaseClass;
            tableScript["dtos_entity_base_class"] = baseProfile.DtoEntityBaseClass;
            tableScript["ts_entity_base_interface"] = baseProfile.TsEntityBaseInterface;
            tableScript["repository_interface_name"] = baseProfile.RepositoryInterfaceName;
            tableScript["api_module_enum"] = tableModel.ApiModuleEnum ?? "Routine";
            tableScript["entity_namespace"] = tableModel.EntityNamespace ?? "Takt.Domain.Entities";
            tableScript["controller_namespace"] = tableModel.ControllerNamespace ?? "Takt.WebApi.Controllers";
            tableScript["dto_namespace"] = tableModel.DtoNamespace ?? "Takt.Application.Dtos";
            tableScript["service_namespace"] = tableModel.ServiceNamespace ?? "Takt.Application.Services";
            tableScript["gen_author"] = tableModel.GenAuthor ?? "Takt365";
            tableScript["comment"] = tableModel.Comment ?? string.Empty;
            tableScript["data_source"] = tableModel.DataSource ?? string.Empty;
            tableScript["table_name"] = tableModel.TableName ?? string.Empty;
            tableScript["entity_class_name"] = tableModel.EntityClassName ?? string.Empty;
            tableScript["entity_name_pascal"] = tableModel.EntityNamePascal ?? string.Empty;
            tableScript["entity_name_camel"] = tableModel.EntityNameCamel ?? string.Empty;
            tableScript["entity_name_kebab"] = tableModel.EntityNameKebab ?? string.Empty;
            tableScript["frontend_module_path"] = tableModel.FrontendModulePath ?? string.Empty;
            tableScript["gen_module_name"] = tableModel.GenModuleName ?? string.Empty;
            tableScript["perms_prefix_canonical"] = tableModel.PermsPrefixCanonical ?? string.Empty;
            tableScript["is_gen_menu"] = tableModel.IsGenMenu;
            tableScript["is_gen_translation"] = tableModel.IsGenTranslation;
            tableScript["gen_business_name"] = tableModel.GenBusinessName ?? string.Empty;
            tableScript["table_comment"] = tableModel.TableComment ?? string.Empty;
            tableScript["is_query"] = tableModel.IsQuery;
            tableScript["is_create"] = tableModel.IsCreate;
            tableScript["is_update"] = tableModel.IsUpdate;
            tableScript["is_delete"] = tableModel.IsDelete;
            tableScript["is_template"] = tableModel.IsTemplate;
            tableScript["is_import"] = tableModel.IsImport;
            tableScript["is_export"] = tableModel.IsExport;
            tableScript["parent_menu_id"] = tableModel.ParentMenuId;
            tableScript["sql_create_by"] = tableModel.SqlCreateBy ?? "admin";
            tableScript["sql_menu_id"] = tableModel.SqlMenuId;
            var translationRowsArray = new ScriptArray();
            foreach (var row in tableModel.SqlTranslationRows ?? [])
            {
                var rowScript = new ScriptObject();
                rowScript["id"] = row.Id;
                rowScript["culture"] = row.Culture ?? string.Empty;
                rowScript["resource_key"] = row.ResourceKey ?? string.Empty;
                rowScript["translation_value"] = row.TranslationValue ?? string.Empty;
                rowScript["resource_group"] = row.ResourceGroup ?? "page";
                rowScript["sort_order"] = row.SortOrder;
                translationRowsArray.Add(rowScript);
            }
            tableScript["sql_translation_rows"] = translationRowsArray;

            var menuButtonRowsArray = new ScriptArray();
            foreach (var row in tableModel.SqlMenuButtonRows ?? [])
            {
                var btnRow = new ScriptObject();
                btnRow["id"] = row.Id;
                btnRow["menu_code"] = row.MenuCode ?? string.Empty;
                btnRow["menu_name"] = row.MenuName ?? string.Empty;
                btnRow["permission"] = row.Permission ?? string.Empty;
                btnRow["menu_l10n_key"] = row.MenuL10nKey ?? string.Empty;
                btnRow["sort_order"] = row.SortOrder;
                btnRow["order_num"] = row.SortOrder;
                menuButtonRowsArray.Add(btnRow);
            }
            tableScript["sql_menu_button_rows"] = menuButtonRowsArray;

            global["table"] = tableScript;

            var columnsArray = new ScriptArray();
            foreach (var col in cols)
            {
                var colScript = new ScriptObject();
                colScript.Import(col, null, m => ToSnakeCase(m.Name));
                colScript["comment"] = col.Comment;
                colScript["csharp_column_name"] = col.CsharpColumnName ?? string.Empty;
                colScript["database_column_name"] = col.DatabaseColumnName ?? string.Empty;
                colScript["database_data_type"] = col.DatabaseDataType ?? "nvarchar";
                colScript["csharp_data_type"] = col.CsharpDataType ?? "string";
                columnsArray.Add(colScript);
            }
            global["columns"] = columnsArray;

            scriptContext.PushGlobal(global);
        }
        else
        {
            scriptContext.PushGlobal(ScriptObject.From(model));
        }

        try
        {
            var result = await template.RenderAsync(scriptContext);
            var output = NormalizeGeneratedOutput(result ?? string.Empty);
            TaktLogger.Debug("[CodeEngine] 模板渲染完成，输出长度={OutputLength}", output.Length);
            return output;
        }
        catch (Exception ex)
        {
            TaktLogger.Error(ex, "[CodeEngine] 模板渲染失败: {Message}", ex.Message);
            throw new TaktGenEngineException($"模板渲染失败：{ex.Message}", ex);
        }
    }

    /// <summary>
    /// 统一规整模板输出，避免生成代码出现大量连续空行。
    /// 保留单个空行用于语义分段，将 3 行及以上连续空白压缩为 1 个空行。
    /// </summary>
    /// <param name="text">模板渲染后的原始文本</param>
    /// <returns>规整后的文本</returns>
    private static string NormalizeGeneratedOutput(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        // 先统一换行，避免 CRLF/LF 混用导致空行压缩失效
        var normalized = text.Replace("\r\n", "\n").Replace('\r', '\n');

        // 去除行尾多余空白（保留行内空格）
        normalized = Regex.Replace(normalized, @"[ \t]+\n", "\n");

        // 将 3 行及以上连续空白压缩为 2 个换行（即最多保留 1 个空行）
        normalized = Regex.Replace(normalized, @"\n{3,}", "\n\n");

        // 输出统一为 CRLF，符合当前仓库 C# 文件习惯
        return normalized.Replace("\n", "\r\n");
    }

    /// <summary>
    /// 使用给定模板内容与强类型上下文渲染，返回生成后的文本。
    /// </summary>
    /// <param name="templateContent">Scriban 模板内容</param>
    /// <param name="context">代码生成模板上下文（表 + 列）</param>
    /// <returns>渲染后的字符串</returns>
    /// <exception cref="ArgumentNullException">context 为 null 时抛出</exception>
    /// <exception cref="TaktGenEngineException">模板解析或渲染失败时抛出</exception>
    public Task<string> RenderAsync(string templateContent, TaktGenTemplateContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        return RenderAsync(templateContent, (object)context);
    }

    /// <summary>
    /// PascalCase 转 snake_case，如 EntityNamespace → entity_namespace。
    /// </summary>
    private static string ToSnakeCase(string? pascal)
    {
        if (string.IsNullOrEmpty(pascal)) return string.Empty;
        var sb = new System.Text.StringBuilder(pascal.Length + 4);
        for (var i = 0; i < pascal.Length; i++)
        {
            var c = pascal[i];
            if (char.IsUpper(c))
            {
                if (i > 0) sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
                sb.Append(c);
        }
        return sb.ToString();
    }
}
