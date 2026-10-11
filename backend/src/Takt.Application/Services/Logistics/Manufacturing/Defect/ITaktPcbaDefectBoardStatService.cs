// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：ITaktPcbaDefectBoardStatService.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA 不良看板统计服务接口（检查数 + 修理数；与 CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Defect;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA 不良看板统计服务（SMT 检查数 + 修理数；与 CRUD 分离）
/// </summary>
public interface ITaktPcbaDefectBoardStatService
{
    /// <summary>
    /// 获取 PCBA 不良看板统计（检查：明细当日完成数量；修理：明细不良数量）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>PCBA 不良看板统计</returns>
    Task<TaktPcbaDefectBoardStatDto> GetPcbaDefectBoardStatAsync(TaktDefectStatQueryDto queryDto);
}
