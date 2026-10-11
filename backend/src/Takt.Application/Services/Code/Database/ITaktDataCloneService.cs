// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Code.Database
// 文件名称：ITaktDataCloneService.cs
// 创建时间：2026-06-09
// 创建人：Takt365(Cursor AI)
// 功能描述：公司级数据克隆应用服务接口（备份预览 + 执行克隆；库表元数据见 ITaktDatabaseInfoService）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Code.Database;

namespace Takt.Application.Services.Code.Database;

/// <summary>
/// 公司级数据克隆应用服务
/// </summary>
public interface ITaktDataCloneService
{
    /// <summary>
    /// 单次请求允许的源/目标公司数量（各 1 个）
    /// </summary>
    const int MaxCompanyCountPerRequest = 1;

    /// <summary>
    /// 单次请求允许的源/目标表数量（各 1 张）
    /// </summary>
    const int MaxTableCountPerRequest = 1;

    /// <summary>
    /// 获取公司级数据克隆备份预览（备份窗口）
    /// </summary>
    /// <param name="dto">克隆请求</param>
    /// <returns>目标公司备份与清空预览</returns>
    Task<TaktDataClonePreviewDto> GetDataClonePreviewAsync(TaktDataCloneDto dto);

    /// <summary>
    /// 按公司范围克隆数据（一次仅一个源公司、一张源表 → 一个目标公司、一张目标表；须先确认备份窗口）
    /// </summary>
    /// <param name="dto">克隆请求</param>
    /// <returns>克隆结果</returns>
    Task<TaktDataCloneResultDto> CloneDataAsync(TaktDataCloneDto dto);
}
