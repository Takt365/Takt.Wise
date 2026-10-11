// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktEcExecModelRootMaterialRow.cs
// 创建时间：2026-09-09
// 创建人：Takt365(Cursor AI)
// 功能描述：生管/制一/品管/制技/制二执行行：设变+机种+根物料编码自去重所需冗余列
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Domain.Interfaces;

/// <summary>
/// 设变部门执行行：按设变单号+机种+根物料编码去重时使用的冗余字段
/// </summary>
public interface ITaktEcExecModelRootMaterialRow : ITaktEcDeptExecEntity
{
    /// <summary>
    /// 租户编码
    /// </summary>
    string TenantCode { get; }

    /// <summary>
    /// 公司编码
    /// </summary>
    string CompanyCode { get; }

    /// <summary>
    /// 是否删除
    /// </summary>
    int IsDeleted { get; }

    /// <summary>
    /// 机种编码
    /// </summary>
    string EcModelCode { get; set; }

    /// <summary>
    /// 根物料编码组（同去重键下的根物料编码，分号分隔）
    /// </summary>
    string EcRootMaterialCodeGroup { get; set; }
}
