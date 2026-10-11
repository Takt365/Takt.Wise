// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Repositories
// 文件名称：ITaktEmployeeProfileResolver.cs
// 创建时间：2026-09-12
// 创建人：Takt365(Cursor AI)
// 功能描述：按用户 EmployeeId 解析当前会话公司下的员工档案（同 EmployeeCode 跨公司映射）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.HumanResource.Personnel;

namespace Takt.Domain.Repositories;

/// <summary>
/// 员工档案资料解析（个人中心 /me：User.EmployeeId 可能指向其他公司同码员工行）
/// </summary>
public interface ITaktEmployeeProfileResolver
{
    /// <summary>
    /// 解析指定租户、公司下与用户绑定员工对应的档案行
    /// </summary>
    /// <param name="employeeId">用户表 EmployeeId</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">当前会话公司编码</param>
    /// <returns>当前公司员工档案；找不到返回 null</returns>
    Task<TaktEmployee?> ResolveForCompanyAsync(long employeeId, string tenantCode, string companyCode);
}
