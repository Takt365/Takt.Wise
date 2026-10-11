// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Defect
// 文件名称：ITaktPcbaRepairStatService.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：PCBA 改修不良看板统计服务接口（与 TaktPcbaRepair CRUD 分离）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Defect;

namespace Takt.Application.Services.Logistics.Manufacturing.Defect;

/// <summary>
/// PCBA 改修不良看板统计服务（与 TaktPcbaRepairService CRUD 分离）
/// </summary>
public interface ITaktPcbaRepairStatService
{
    /// <summary>
    /// 获取 PCBA 改修不良统计（数据看板 defect-stat；按生产日期、生产班组）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>PCBA 改修统计</returns>
    Task<TaktPcbaRepairStatDto> GetPcbaRepairStatAsync(TaktDefectStatQueryDto queryDto);
}
