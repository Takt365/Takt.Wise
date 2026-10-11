// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Generator.GenEngine
// 文件名称：TaktGenTemplateLoader.cs
// 创建时间：2026-06-20
// 创建人：Takt365(Cursor AI)
// 功能描述：从 wwwroot/Generator 加载 Scriban 模板，并将模板键解析为工程内目标相对路径
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.Code.Generator;
using Takt.Shared.Helpers;

namespace Takt.Application.Services.Code.Generator.GenEngine;

/// <summary>
/// 代码生成 Scriban 模板加载与输出路径解析（wwwroot/Generator）。
/// </summary>
public static class TaktGenTemplateLoader
{
    private const string GeneratorSegment = "Generator";

    /// <summary>
    /// 解析 wwwroot/Generator 根目录。
    /// </summary>
    /// <param name="contentRootPath">Web 内容根路径（可空，空则按 AppContext.BaseDirectory 向上查找 wwwroot）</param>
    /// <returns>Generator 目录绝对路径</returns>
    public static string GetGeneratorRootPath(string? contentRootPath = null)
    {
        return Path.Combine(TaktFileHelper.GetWwwRootPath(contentRootPath), GeneratorSegment);
    }

    /// <summary>
    /// 按生成模板类型从磁盘加载模板字典；模板键为相对 Generator 的路径（不含 .sbn 后缀，如 Backend/Crud/Csharp/Entity.cs）。
    /// </summary>
    /// <param name="genTemplateCategory">生成模板类型（crud/tree/sub）</param>
    /// <param name="contentRootPath">Web 内容根路径（可空）</param>
    /// <returns>模板键 → Scriban 模板内容</returns>
    /// <exception cref="InvalidOperationException">Generator 目录不存在或未找到任何模板文件</exception>
    public static IReadOnlyDictionary<string, string> LoadTemplates(string genTemplateCategory, string? contentRootPath = null)
    {
        var generatorRoot = GetGeneratorRootPath(contentRootPath);
        if (!Directory.Exists(generatorRoot))
            throw new InvalidOperationException($"未找到代码生成模板目录：{generatorRoot}");

        var backendCategory = NormalizeBackendCategory(genTemplateCategory);
        var templates = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var csharpDir = Path.Combine(generatorRoot, "Backend", backendCategory, "Csharp");
        CollectTemplateFiles(csharpDir, $"Backend/{backendCategory}/Csharp", templates);

        var sqlDir = Path.Combine(generatorRoot, "Backend", "Sql");
        CollectTemplateFiles(sqlDir, "Backend/Sql", templates);

        if (ShouldLoadFrontendTemplates(genTemplateCategory))
        {
            var frontDir = Path.Combine(generatorRoot, "Frontend", "Antdv", "crud");
            CollectTemplateFiles(frontDir, "Frontend/Antdv/crud", templates);
        }

        if (templates.Count == 0)
            throw new InvalidOperationException($"模板目录 {generatorRoot} 下未找到可用的 .sbn 模板（类型={genTemplateCategory}）。");

        return templates;
    }

    /// <summary>
    /// 将模板键解析为仓库内目标相对路径（用于预览与 zip 下载文件名）。
    /// </summary>
    /// <param name="table">代码生成表配置实体</param>
    /// <param name="templateKey">模板键（如 Backend/Crud/Csharp/Entity.cs）</param>
    /// <returns>目标相对路径；无法解析时返回 null</returns>
    public static string? ResolveOutputRelativePath(TaktGenTable table, string templateKey)
    {
        ArgumentNullException.ThrowIfNull(table);
        if (string.IsNullOrWhiteSpace(templateKey))
            return null;

        var model = TaktGenTableTemplateModel.From(table);
        var key = templateKey.Replace('\\', '/').Trim();

        if (key.StartsWith("Backend/Sql/", StringComparison.OrdinalIgnoreCase))
        {
            var sqlName = Path.GetFileName(key);
            if (string.IsNullOrEmpty(sqlName))
                return null;
            var tableSnake = string.IsNullOrWhiteSpace(table.TableName) ? "gen_table" : table.TableName.Trim();
            return Path.Combine("backend", "sql", "seed", $"{tableSnake}_{sqlName}").Replace('\\', '/');
        }

        if (key.Contains("/Csharp/", StringComparison.OrdinalIgnoreCase))
        {
            var filePart = Path.GetFileName(key);
            return filePart switch
            {
                "Entity.cs" => NamespaceToSrcRelativePath(table.EntityNamespace, $"{table.EntityClassName}.cs"),
                "Dto.cs" => NamespaceToSrcRelativePath(table.DtoNamespace, $"{table.EntityClassName}Dtos.cs"),
                "IService.cs" => NamespaceToSrcRelativePath(table.ServiceNamespace, table.IServiceClassName + ".cs"),
                "Service.cs" => NamespaceToSrcRelativePath(table.ServiceNamespace, table.ServiceClassName + ".cs"),
                "Controller.cs" => NamespaceToSrcRelativePath(table.ControllerNamespace, table.ControllerClassName + ".cs"),
                "Validators.cs" => NamespaceToSrcRelativePath(ResolveValidatorsNamespace(table.DtoNamespace), $"{table.EntityClassName}Validators.cs"),
                _ => null
            };
        }

        if (key.StartsWith("Frontend/Antdv/crud/", StringComparison.OrdinalIgnoreCase))
        {
            if (key.EndsWith("/api/api.ts", StringComparison.OrdinalIgnoreCase))
                return $"frontend/src/api/{model.FrontendModulePath}/{model.EntityNameKebab}.ts".Replace('\\', '/');
            if (key.EndsWith("/types/types.d.ts", StringComparison.OrdinalIgnoreCase))
                return $"frontend/src/types/{model.FrontendModulePath}/{model.EntityNameKebab}.d.ts".Replace('\\', '/');
            if (key.EndsWith("/views/index.vue", StringComparison.OrdinalIgnoreCase))
                return $"frontend/src/views/{model.FrontendModulePath}/{model.EntityNameKebab}/index.vue".Replace('\\', '/');
            if (key.EndsWith("/views/components/form.vue", StringComparison.OrdinalIgnoreCase))
                return $"frontend/src/views/{model.FrontendModulePath}/{model.EntityNameKebab}/components/{model.EntityNameKebab}-form.vue".Replace('\\', '/');
            if (key.Contains("/locales/", StringComparison.OrdinalIgnoreCase))
            {
                var localeFile = Path.GetFileName(key);
                if (string.IsNullOrEmpty(localeFile))
                    return null;
                return $"frontend/src/locales/{model.FrontendModulePath}/{model.EntityNameKebab}/{localeFile}".Replace('\\', '/');
            }
        }

        return null;
    }

    /// <summary>
    /// 递归收集目录下 .sbn 文件并写入模板字典。
    /// </summary>
    private static void CollectTemplateFiles(string directory, string keyPrefix, Dictionary<string, string> templates)
    {
        if (!Directory.Exists(directory))
            return;

        foreach (var file in Directory.EnumerateFiles(directory, "*.sbn", SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(directory, file).Replace('\\', '/');
            if (!relative.EndsWith(".sbn", StringComparison.OrdinalIgnoreCase))
                continue;
            var templateKey = $"{keyPrefix}/{relative[..^4]}";
            templates[templateKey] = File.ReadAllText(file);
        }
    }

    /// <summary>
    /// 将 code_generator_template_type 字典值规范为 Backend 子目录名（Crud/Tree/Sub）。
    /// </summary>
    private static string NormalizeBackendCategory(string? genTemplateCategory)
    {
        var category = (genTemplateCategory ?? "crud").Trim().ToLowerInvariant();
        return category switch
        {
            "tree" => "Tree",
            "sub" => "Sub",
            _ => "Crud"
        };
    }

    /// <summary>
    /// tree 模板暂无独立前端目录，仅 crud/sub 加载 Frontend/Antdv/crud。
    /// </summary>
    private static bool ShouldLoadFrontendTemplates(string? genTemplateCategory)
    {
        var category = (genTemplateCategory ?? "crud").Trim().ToLowerInvariant();
        return category is "crud" or "sub";
    }

    /// <summary>
    /// C# 命名空间转为 backend/src 下相对路径并拼接文件名。
    /// </summary>
    private static string? NamespaceToSrcRelativePath(string? namespaceName, string fileName)
    {
        if (string.IsNullOrWhiteSpace(namespaceName) || string.IsNullOrWhiteSpace(fileName))
            return null;
        var path = namespaceName.Trim().Replace('.', Path.DirectorySeparatorChar);
        return Path.Combine("backend", "src", path, fileName).Replace('\\', '/');
    }

    /// <summary>
    /// 由 Dto 命名空间推导 Validators 命名空间。
    /// </summary>
    private static string? ResolveValidatorsNamespace(string? dtoNamespace)
    {
        if (string.IsNullOrWhiteSpace(dtoNamespace))
            return null;
        return dtoNamespace.Replace("Takt.Application.Dtos", "Takt.Application.Validators", StringComparison.Ordinal);
    }
}
