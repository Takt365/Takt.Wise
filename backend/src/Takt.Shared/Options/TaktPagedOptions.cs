// ========================================

// 项目名称：节拍智控·TaktWise

// 命名空间：Takt.Shared.Options

// 文件名称：TaktPagedOptions.cs

// 创建时间：2026-06-14

// 创建人：Takt365(Cursor AI)

// 功能描述：列表分页全局配置（appsettings Paged 节为唯一运维入口）

//

// 版权信息：Copyright (c) 2025 Takt  All rights reserved.

// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。

// ========================================



namespace Takt.Shared.Options;



/// <summary>

/// 分页配置（<c>appsettings:Paged</c> 覆盖本类默认值；前后端均由此节驱动）

/// </summary>

public class TaktPagedOptions

{

    /// <summary>

    /// appsettings 配置节名称

    /// </summary>

    public const string SectionName = "Paged";



    /// <summary>

    /// 代码硬上限：单页最大条数

    /// </summary>

    public const int HardMaxPageSize = 500;



    /// <summary>

    /// 默认页码（从 1 开始）

    /// </summary>

    public int DefaultPageIndex { get; set; } = 1;



    /// <summary>

    /// 默认每页条数

    /// </summary>

    public int DefaultPageSize { get; set; } = 20;



    /// <summary>

    /// 列表接口允许的 pageSize 上限

    /// </summary>

    public int MaxPageSize { get; set; } = 100;



    /// <summary>

    /// 前端 TaktPagination 可选每页条数（勿设属性默认值：Bind 会与 appsettings 数组合并导致重复）

    /// </summary>

    public int[] PageSizeOptions { get; set; } = [];



    /// <summary>

    /// 校验分页配置（防 0、防误配超大值、默认项须在可选项内）

    /// </summary>

    public void Validate()

    {

        if (DefaultPageIndex < 1)

        {

            throw new InvalidOperationException($"{SectionName}:{nameof(DefaultPageIndex)} 必须 >= 1");

        }



        if (DefaultPageSize < 1)

        {

            throw new InvalidOperationException($"{SectionName}:{nameof(DefaultPageSize)} 必须 >= 1");

        }



        if (MaxPageSize < 1 || MaxPageSize > HardMaxPageSize)

        {

            throw new InvalidOperationException(

                $"{SectionName}:{nameof(MaxPageSize)} 必须在 1～{HardMaxPageSize} 之间");

        }



        if (DefaultPageSize > MaxPageSize)

        {

            throw new InvalidOperationException(

                $"{SectionName}:{nameof(DefaultPageSize)} 不能大于 {nameof(MaxPageSize)}");

        }



        if (PageSizeOptions == null || PageSizeOptions.Length == 0)

        {

            PageSizeOptions = [10, 20, 50, 100];

        }



        var distinct = PageSizeOptions.Distinct().ToArray();

        if (distinct.Length != PageSizeOptions.Length)

        {

            throw new InvalidOperationException($"{SectionName}:{nameof(PageSizeOptions)} 不能包含重复值");

        }



        foreach (var size in PageSizeOptions)

        {

            if (size < 1 || size > MaxPageSize)

            {

                throw new InvalidOperationException(

                    $"{SectionName}:{nameof(PageSizeOptions)} 每项必须在 1～{MaxPageSize} 之间");

            }

        }



        if (!PageSizeOptions.Contains(DefaultPageSize))

        {

            throw new InvalidOperationException(

                $"{SectionName}:{nameof(PageSizeOptions)} 必须包含 {nameof(DefaultPageSize)}={DefaultPageSize}");

        }

    }

}

