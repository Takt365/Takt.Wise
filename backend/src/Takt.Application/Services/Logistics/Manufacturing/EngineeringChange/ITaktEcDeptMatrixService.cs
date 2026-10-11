// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcDeptMatrixService.cs
// 创建时间：2026-07-01
// 创建人：Takt365(Cursor AI)
// 功能描述：设变部门执行矩阵视图服务接口（跨 8 张部门表转置/统计，无 EcExec 实体 CRUD）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 设变部门执行矩阵视图服务接口（技术监控：转置列表、部门行统计）
/// </summary>
public interface ITaktEcDeptMatrixService
{
    /// <summary>
    /// 统计部门执行行数（8 张部门表聚合；可选按是否实施筛选）
    /// </summary>
    /// <param name="isImplemented">是否实施（0=否 1=是；空=全部）</param>
    /// <returns>部门执行行数量</returns>
    Task<int> CountDeptExecutionRowsAsync(int? isImplemented = null);

    /// <summary>
    /// 获取设变部门执行统计（设变单数 + 明细数 + 部门行数）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>设变部门执行统计</returns>
    Task<TaktEcExecStatDto> GetEcDeptExecutionStatAsync(TaktEcExecStatQueryDto queryDto);

    /// <summary>
    /// 获取设变部门执行转置列表（分页；行=设变明细，列=各部门实施状态）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>转置分页结果</returns>
    Task<TaktEcExecTransposedResultDto> GetEcDeptTransposedListAsync(TaktEcExecTransposedQueryDto queryDto);

    /// <summary>
    /// 获取设变批次转置列表（分页；行=设变明细，列=各阶段日期+批次）
    /// </summary>
    /// <param name="queryDto">查询 DTO</param>
    /// <returns>批次转置分页结果</returns>
    Task<TaktEcExecBatchTransposedResultDto> GetEcBatchTransposedListAsync(TaktEcExecBatchTransposedQueryDto queryDto);
}
