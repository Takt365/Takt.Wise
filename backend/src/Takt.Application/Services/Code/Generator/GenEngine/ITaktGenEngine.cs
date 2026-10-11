// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Generator.GenEngine
// 文件名称：ITaktGenEngine.cs
// 创建时间：2025-02-02
// 创建人：Takt365(Cursor AI)
// 功能描述：代码生成核心引擎接口，基于 Scriban 模板渲染
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================
namespace Takt.Application.Services.Code.Generator.GenEngine;

/// <summary>
/// 代码生成核心引擎接口：使用 Scriban 解析并渲染模板，生成代码文本。
/// </summary>
public interface ITaktGenEngine
{
    /// <summary>
    /// 使用给定模板内容与上下文模型渲染，返回生成后的文本。
    /// </summary>
    /// <param name="templateContent">Scriban 模板内容（如 {{ Table.EntityClassName }}、{{ for col in Columns }} 等）</param>
    /// <param name="model">模板绑定的模型（可为 TaktGenTemplateContext 或任意可序列化对象）</param>
    /// <returns>渲染后的字符串</returns>
    /// <exception cref="TaktGenEngineException">模板解析或渲染失败时抛出</exception>
    Task<string> RenderAsync(string templateContent, object model);

    /// <summary>
    /// 使用给定模板内容与强类型上下文渲染，返回生成后的文本。
    /// </summary>
    /// <param name="templateContent">Scriban 模板内容</param>
    /// <param name="context">代码生成模板上下文（表 + 列）</param>
    /// <returns>渲染后的字符串</returns>
    Task<string> RenderAsync(string templateContent, TaktGenTemplateContext context);
}
