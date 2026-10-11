// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Foundation
// 文件名称：ITaktCurrentEmployeeCodeService.cs
// 创建时间：2026-08-31
// 创建人：Takt365(Cursor AI)
// 功能描述：解析当前登录用户关联的人事员工编码（EmployeeCode）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Application.Services.Foundation;

/// <summary>
/// 当前登录用户员工编码解析服务
/// </summary>
public interface ITaktCurrentEmployeeCodeService
{
    /// <summary>
    /// 获取当前登录用户对应的 EmployeeCode；未登录或未关联员工时返回 null
    /// </summary>
    /// <returns>员工编码</returns>
    Task<string?> GetCurrentEmployeeCodeAsync();
}
