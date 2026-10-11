// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktApiModuleAttribute.cs
// 创建时间：2026-05-20
// 创建人：Takt365(Cursor AI)
// 功能描述：API 模块特性（Constants 层 Attribute；模块类型引用 Enums.TaktModule，禁止在本目录定义 enum）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Shared.Enums;

namespace Takt.Shared.Constants;

/// <summary>
/// API 模块特性
/// 用于标记控制器所属的业务模块，支持 OpenAPI 文档自动分组
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ApiModuleAttribute : Attribute
{
    /// <summary>
    /// 获取模块类型
    /// </summary>
    public TaktModule Module { get; }

    /// <summary>
    /// 获取模块显示名称
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// 获取 API 文档名称（用于 OpenAPI 文档分组）
    /// </summary>
    public string DocumentName { get; }

    /// <summary>
    /// 初始化 API 模块特性（模块码与 <see cref="TaktModule"/> 数值一致）
    /// </summary>
    /// <param name="module">模块类型码</param>
    /// <param name="displayName">模块显示名称</param>
    public ApiModuleAttribute(int module, string displayName)
        : this((TaktModule)module, displayName)
    {
    }

    /// <summary>
    /// 初始化 API 模块特性
    /// </summary>
    /// <param name="module">模块类型</param>
    /// <param name="displayName">模块显示名称</param>
    public ApiModuleAttribute(TaktModule module, string displayName)
    {
        Module = module;
        DisplayName = displayName;
        DocumentName = module.ToString().ToLowerInvariant();
    }
}
