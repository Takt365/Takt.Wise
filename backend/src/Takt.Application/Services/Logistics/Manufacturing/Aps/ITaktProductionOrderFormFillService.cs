// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.Aps
// 文件名称：ITaktProductionOrderFormFillService.cs
// 创建时间：2026-08-30
// 创建人：Takt365(Cursor AI)
// 功能描述：生产工单表单回填服务接口（独立非实体 CRUD；generate-services 不会覆盖）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.Aps;

namespace Takt.Application.Services.Logistics.Manufacturing.Aps;

/// <summary>
/// 生产工单表单回填服务（组立/PCBA 等选工单后自动赋值）
/// </summary>
public interface ITaktProductionOrderFormFillService
{
    /// <summary>
    /// 按工单号解析表单回填字段（工单类别/机种/物料/批次/数量/序号/标准工时等）
    /// </summary>
    /// <param name="queryDto">查询参数</param>
    /// <returns>回填 DTO；工单不存在时返回 null</returns>
    Task<TaktProductionOrderFormFillDto?> GetProductionOrderFormFillAsync(TaktProductionOrderFormFillQueryDto queryDto);
}
