// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds
// 文件名称：TaktRoleSeedData.cs
// 创建时间：2025-01-20
// 创建人：Takt365(Cursor AI)
// 功能描述：角色种子数据初始化
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;
using Takt.Shared.Helpers;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 角色种子数据初始化
/// 幂等性操作：存在则更新，不存在则创建
/// 注意：每个租户数据库只初始化自己的角色数据
/// Program.cs 会为每个租户数据库调用此方法，因此只需为当前租户初始化角色
/// </summary>
public class TaktRoleSeedData : ITaktSeedDataCoordinator
{
    /// <summary>
    /// 执行顺序（在用户之后，部门之前）
    /// </summary>
    public int Order => 25;

    /// <summary>
    /// 初始化角色种子数据
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(IServiceProvider serviceProvider, string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化角色种子数据...");

        // 参数验证
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过角色种子数据初始化");
            return (0, 0);
        }

        var repository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktRole>>();

        int insertCount = 0;
        int updateCount = 0;

        TaktLogger.Information("正在为租户 {TenantCode} 初始化角色数据...", tenantCode);

        // 为当前租户初始化标准角色
        var roles = GetStandardRoles(tenantCode);
        
        foreach (var roleData in roles)
        {
            var (role, i, u) = await CreateOrUpdateRoleAsync(
                repository,
                tenantCode,
                roleData.RoleCode,
                roleData.RoleName,
                roleData.DataScope,
                roleData.SortOrder);
            
            insertCount += i;
            updateCount += u;
        }

        TaktLogger.Information("角色种子数据初始化完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条", insertCount, updateCount);

        return (insertCount, updateCount);
    }

    /// <summary>
    /// 部门管理员（短码与 TaktDept.DeptShortName 对齐；DeptCode 为 DTA 一/二/三级部门）
    /// </summary>
    internal static readonly (string RoleCode, string RoleName, string DeptCode, string ShortName)[] DeptAdminRoles =
    {
        // 一级
        ("ROLE_GM_DEPT_ADMIN", "总经理室", "D1000", "GM"),
        ("ROLE_GA_DEPT_ADMIN", "总务部", "D0100", "GA"),
        ("ROLE_FIN_DEPT_ADMIN", "财务部", "D0200", "FIN"),
        ("ROLE_IT_DEPT_ADMIN", "IT部", "D0300", "IT"),
        ("ROLE_ADM_DEPT_ADMIN", "管理部", "D0400", "ADM"),
        ("ROLE_MAT_DEPT_ADMIN", "资材部", "D0500", "MAT"),
        ("ROLE_PROD_DEPT_ADMIN", "生产部", "D0600", "PROD"),
        ("ROLE_ENG_DEPT_ADMIN", "技术部", "D0700", "ENG"),
        ("ROLE_QA_DEPT_ADMIN", "品保部", "D0800", "QA"),
        ("ROLE_OEM_DEPT_ADMIN", "OEM部", "D0900", "OEM"),
        // 二级 / 制造2课下属
        ("ROLE_GAS_DEPT_ADMIN", "总务课", "D0110", "GAS"),
        ("ROLE_FAC_DEPT_ADMIN", "财务课", "D0210", "FAC"),
        ("ROLE_MIS_DEPT_ADMIN", "电脑课", "D0310", "MIS"),
        ("ROLE_PUR_DEPT_ADMIN", "采购课", "D0510", "PUR"),
        ("ROLE_MFG1_DEPT_ADMIN", "制造1课", "D0610", "MFG1"),
        ("ROLE_MFG2_DEPT_ADMIN", "制造2课", "D0620", "MFG2"),
        ("ROLE_MTE_DEPT_ADMIN", "制造技术课", "D0630", "MTE"),
        ("ROLE_SMT_DEPT_ADMIN", "SMT", "D0621", "SMT"),
        ("ROLE_AI_DEPT_ADMIN", "自插", "D0622", "AI"),
        ("ROLE_REW_DEPT_ADMIN", "修正", "D0623", "REW"),
        ("ROLE_MI_DEPT_ADMIN", "手插", "D0624", "MI"),
        ("ROLE_MTL_DEPT_ADMIN", "物料", "D0625", "MTL"),
        ("ROLE_MFG2I_DEPT_ADMIN", "制造2课-间接", "D0626", "MFG2I"),
        ("ROLE_TEC_DEPT_ADMIN", "技术课", "D0710", "TEC"),
        ("ROLE_IQC_DEPT_ADMIN", "受检课", "D0810", "IQC"),
        ("ROLE_QC_DEPT_ADMIN", "品管课", "D0820", "QC"),
        ("ROLE_OEMQA_DEPT_ADMIN", "OEM QA课", "D0910", "OEMQA"),
        ("ROLE_OEMADM_DEPT_ADMIN", "OEM管理课", "D0920", "OEMADM"),
    };

    /// <summary>
    /// 获取当前租户的标准角色列表
    /// </summary>
    private static List<(string RoleCode, string RoleName, int DataScope, int SortOrder)> GetStandardRoles(string tenantCode)
    {
        var list = new List<(string, string, int, int)>
        {
            ("ROLE_SUPER_ADMIN", "超级管理员", 1, 1),
        };
        var sort = 2;
        foreach (var d in DeptAdminRoles)
        {
            // DataScope=3：本部门数据范围
            list.Add((d.RoleCode, d.RoleName, 3, sort++));
        }
        list.Add(("ROLE_EMPLOYEE", "普通员工", 4, sort++));
        list.Add(("ROLE_DEMO", "演示访客", 4, sort));
        return list;
    }

    /// <summary>
    /// 创建或更新角色
    /// </summary>
    private static async Task<(TaktRole Role, int InsertCount, int UpdateCount)> CreateOrUpdateRoleAsync(
        ITaktTenantSeedRepository<TaktRole> repository,
        string tenantCode,
        string roleCode,
        string roleName,
        int dataScope,
        int sortOrder)
    {
        var role = await repository.FirstAsync(r => r.TenantCode == tenantCode && r.RoleCode == roleCode);
        
        if (role == null)
        {
            // 不存在：创建新记录（仓储会自动生成雪花ID和审计字段）
            role = new TaktRole
            {
                TenantCode = tenantCode,
                RoleCode = roleCode,
                RoleName = roleName,
                DataScope = dataScope,
                IsBuiltIn = 1,
                RoleStatus = 1,
                SortOrder = sortOrder
            };
            role = await repository.CreateAsync(role);
            return (role, 1, 0);
        }
        else
        {
            // 存在：更新记录
            role.RoleName = roleName;
            role.DataScope = dataScope;
            role.IsBuiltIn = 1;
            role.RoleStatus = 1;
            role.SortOrder = sortOrder;

            await repository.UpdateAsync(role);
            return (role, 0, 1);
        }
    }
}
