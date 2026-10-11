// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Logistics.Manufacturing.EngineeringChange
// 文件名称：ITaktEcExecMasterQueryService.cs
// 创建时间：2026-08-27
// 创建人：Takt365(Cursor AI)
// 功能描述：执行部门左栏主表分页查询（TaktEcDetail；各部门执行表 OneToOne 挂在明细上）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.Logistics.Manufacturing.EngineeringChange;
using Takt.Shared.Models;

namespace Takt.Application.Services.Logistics.Manufacturing.EngineeringChange;

/// <summary>
/// 执行部门左栏主表查询（TaktEcDetail）。权限由各部门 Controller 的 list 码校验。
/// </summary>
public interface ITaktEcExecMasterQueryService
{
    /// <summary>
    /// 分页查询设变明细（执行部门主表；不含部门执行导航，避免 Adapt/JSON 环）。
    /// </summary>
    /// <param name="queryDto">查询条件；为空时按默认分页。</param>
    /// <param name="execDeptCode">执行部门编码；生管/采购/受检/部管/制二/制一/品管/制技附加各自可见明细条件。</param>
    /// <returns>分页结果。</returns>
    Task<TaktPagedResult<TaktEcDetailDto>> GetEcDetailMasterListAsync(
        TaktEcDetailQueryDto? queryDto,
        string? execDeptCode = null);
}
