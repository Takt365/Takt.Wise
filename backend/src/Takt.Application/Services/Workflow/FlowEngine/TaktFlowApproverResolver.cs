// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Workflow.FlowEngine
// 文件名称：TaktFlowApproverResolverService.cs
// 创建时间：2026-06-03
// 创建人：Takt365(Cursor AI)
// 功能描述：解析流程节点审批人（setType 与 nodeApproveList）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Domain.Entities.HumanResource.Organization;
using Takt.Domain.Entities.HumanResource.Personnel;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Repositories;
using Takt.Shared.Enums;

namespace Takt.Application.Services.Workflow.FlowEngine;

/// <summary>
/// 审批人解析器
/// </summary>
public class TaktFlowApproverResolverService
{
    private readonly ITaktCompanyRepository<TaktEmployee> _employeeRepository;
    private readonly ITaktApprovalRepository<TaktEmployeeJoined> _employeeJoinedRepository;
    private readonly ITaktCompanyRepository<TaktEmployeeDept> _employeeDeptRepository;
    private readonly ITaktCompanyRepository<TaktDept> _deptRepository;
    private readonly ITaktTenantRepository<TaktUser> _userRepository;
    private readonly ITaktCompanyRepository<TaktUserRole> _userRoleRepository;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="employeeRepository">员工仓储</param>
    /// <param name="employeeJoinedRepository">上岗仓储</param>
    /// <param name="employeeDeptRepository">员工部门仓储</param>
    /// <param name="deptRepository">部门仓储</param>
    /// <param name="userRepository">用户仓储</param>
    /// <param name="userRoleRepository">用户角色仓储</param>
    public TaktFlowApproverResolverService(
        ITaktCompanyRepository<TaktEmployee> employeeRepository,
        ITaktApprovalRepository<TaktEmployeeJoined> employeeJoinedRepository,
        ITaktCompanyRepository<TaktEmployeeDept> employeeDeptRepository,
        ITaktCompanyRepository<TaktDept> deptRepository,
        ITaktTenantRepository<TaktUser> userRepository,
        ITaktCompanyRepository<TaktUserRole> userRoleRepository)
    {
        _employeeRepository = employeeRepository;
        _employeeJoinedRepository = employeeJoinedRepository;
        _employeeDeptRepository = employeeDeptRepository;
        _deptRepository = deptRepository;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
    }

    /// <summary>
    /// 解析审批节点办理人
    /// </summary>
    /// <param name="node">审批节点</param>
    /// <param name="startUserId">发起人用户 ID</param>
    /// <param name="tenantCode">租户</param>
    /// <param name="companyCode">公司</param>
    /// <returns>办理人列表</returns>
    public async Task<List<TaktFlowResolvedApprover>> ResolveApproversAsync(
        TaktFlowTreeNode node,
        long startUserId,
        string tenantCode,
        string companyCode)
    {
        var setType = node.SetType ?? 1;
        return setType switch
        {
            5 => await ResolveStarterAsync(startUserId, tenantCode),
            2 => await ResolveDirectorAsync(startUserId, node.DirectorLevel ?? 1, tenantCode, companyCode, singleLevel: true),
            6 => await ResolveDirectorAsync(startUserId, node.DirectorLevel ?? 1, tenantCode, companyCode, singleLevel: false),
            3 => await ResolveRoleUsersAsync(node, tenantCode, companyCode),
            4 => await ResolveDeptUsersAsync(node, tenantCode, companyCode),
            _ => await ResolveSpecifiedUsersAsync(node, tenantCode)
        };
    }

    /// <summary>
    /// 解析指定成员（setType=1）
    /// </summary>
    /// <param name="node">节点</param>
    /// <param name="tenantCode">租户</param>
    /// <returns>办理人</returns>
    private async Task<List<TaktFlowResolvedApprover>> ResolveSpecifiedUsersAsync(TaktFlowTreeNode node, string tenantCode)
    {
        var list = new List<TaktFlowResolvedApprover>();
        foreach (var item in node.NodeApproveList ?? new List<TaktFlowNodeApproveItem>())
        {
            if (!long.TryParse(item.TargetId, out var userId) || userId <= 0)
            {
                continue;
            }
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.TenantCode != tenantCode || user.UserStatus != 1)
            {
                continue;
            }
            list.Add(new TaktFlowResolvedApprover
            {
                UserId = user.Id,
                UserName = user.NickName ?? user.UserName
            });
        }
        return list;
    }

    /// <summary>
    /// 发起人自己
    /// </summary>
    /// <param name="startUserId">发起人</param>
    /// <param name="tenantCode">租户</param>
    /// <returns>办理人</returns>
    private async Task<List<TaktFlowResolvedApprover>> ResolveStarterAsync(long startUserId, string tenantCode)
    {
        var user = await _userRepository.GetByIdAsync(startUserId);
        if (user == null || user.TenantCode != tenantCode)
        {
            return new List<TaktFlowResolvedApprover>();
        }
        return new List<TaktFlowResolvedApprover>
        {
            new()
            {
                UserId = user.Id,
                UserName = user.NickName ?? user.UserName
            }
        };
    }

    /// <summary>
    /// 主管 / 层层审批
    /// </summary>
    /// <param name="startUserId">发起人</param>
    /// <param name="level">层级</param>
    /// <param name="tenantCode">租户</param>
    /// <param name="companyCode">公司</param>
    /// <param name="singleLevel">是否仅一级</param>
    /// <returns>办理人</returns>
    private async Task<List<TaktFlowResolvedApprover>> ResolveDirectorAsync(
        long startUserId,
        int level,
        string tenantCode,
        string companyCode,
        bool singleLevel)
    {
        var result = new List<TaktFlowResolvedApprover>();
        var employeeId = await GetEmployeeIdByUserAsync(startUserId, tenantCode);
        if (employeeId == null)
        {
            return result;
        }
        var currentEmployeeId = employeeId.Value;
        var maxLevel = singleLevel ? 1 : Math.Max(1, level);
        for (var i = 0; i < maxLevel; i++)
        {
            var managerEmployeeId = await GetDirectManagerEmployeeIdAsync(currentEmployeeId, tenantCode, companyCode);
            if (managerEmployeeId == null || managerEmployeeId == currentEmployeeId)
            {
                break;
            }
            var managerUser = await GetUserByEmployeeIdAsync(managerEmployeeId.Value, tenantCode);
            if (managerUser == null)
            {
                break;
            }
            result.Add(new TaktFlowResolvedApprover
            {
                UserId = managerUser.Id,
                UserName = managerUser.NickName ?? managerUser.UserName
            });
            currentEmployeeId = managerEmployeeId.Value;
        }
        return result;
    }

    /// <summary>
    /// 角色成员
    /// </summary>
    /// <param name="node">节点</param>
    /// <param name="tenantCode">租户</param>
    /// <param name="companyCode">公司</param>
    /// <returns>办理人</returns>
    private async Task<List<TaktFlowResolvedApprover>> ResolveRoleUsersAsync(
        TaktFlowTreeNode node,
        string tenantCode,
        string companyCode)
    {
        var roleIds = (node.NodeApproveList ?? new List<TaktFlowNodeApproveItem>())
            .Select(x => long.TryParse(x.TargetId, out var id) ? id : 0)
            .Where(x => x > 0)
            .ToList();
        if (roleIds.Count == 0)
        {
            return new List<TaktFlowResolvedApprover>();
        }
        var userRoles = await _userRoleRepository.GetListAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && roleIds.Contains(x.RoleId));
        var userIds = userRoles.Select(x => x.UserId).Distinct().ToList();
        var result = new List<TaktFlowResolvedApprover>();
        foreach (var userId in userIds)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.UserStatus != 1)
            {
                continue;
            }
            result.Add(new TaktFlowResolvedApprover
            {
                UserId = user.Id,
                UserName = user.NickName ?? user.UserName
            });
        }
        return result;
    }

    /// <summary>
    /// 部门成员（负责人优先，否则部门内员工对应用户）
    /// </summary>
    /// <param name="node">节点</param>
    /// <param name="tenantCode">租户</param>
    /// <param name="companyCode">公司</param>
    /// <returns>办理人</returns>
    private async Task<List<TaktFlowResolvedApprover>> ResolveDeptUsersAsync(
        TaktFlowTreeNode node,
        string tenantCode,
        string companyCode)
    {
        var result = new List<TaktFlowResolvedApprover>();
        foreach (var item in node.NodeApproveList ?? new List<TaktFlowNodeApproveItem>())
        {
            if (!long.TryParse(item.TargetId, out var deptId) || deptId <= 0)
            {
                continue;
            }
            var dept = await _deptRepository.GetByIdAsync(deptId);
            if (dept == null || dept.TenantCode != tenantCode || dept.CompanyCode != companyCode)
            {
                continue;
            }
            if (dept.HeadUserId > 0)
            {
                var head = await _userRepository.GetByIdAsync(dept.HeadUserId);
                if (head != null && head.UserStatus == 1)
                {
                    result.Add(new TaktFlowResolvedApprover
                    {
                        UserId = head.Id,
                        UserName = head.NickName ?? head.UserName
                    });
                    continue;
                }
            }
            var empDepts = await _employeeDeptRepository.GetListAsync(x =>
                x.TenantCode == tenantCode && x.CompanyCode == companyCode && x.DeptId == deptId);
            foreach (var rel in empDepts)
            {
                var user = await GetUserByEmployeeIdAsync(rel.EmployeeId, tenantCode);
                if (user == null)
                {
                    continue;
                }
                if (result.Any(x => x.UserId == user.Id))
                {
                    continue;
                }
                result.Add(new TaktFlowResolvedApprover
                {
                    UserId = user.Id,
                    UserName = user.NickName ?? user.UserName
                });
            }
        }
        return result;
    }

    /// <summary>
    /// 获取用户关联员工 ID
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="tenantCode">租户</param>
    /// <returns>员工 ID</returns>
    private async Task<long?> GetEmployeeIdByUserAsync(long userId, string tenantCode)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || user.TenantCode != tenantCode || user.EmployeeId <= 0)
        {
            return null;
        }
        return user.EmployeeId;
    }

    /// <summary>
    /// 获取员工直属上级员工 ID
    /// </summary>
    /// <param name="employeeId">员工 ID</param>
    /// <param name="tenantCode">租户</param>
    /// <param name="companyCode">公司</param>
    /// <returns>上级员工 ID</returns>
    private async Task<long?> GetDirectManagerEmployeeIdAsync(long employeeId, string tenantCode, string companyCode)
    {
        var joinedList = await _employeeJoinedRepository.GetListAsync(
            x => x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.EmployeeId == employeeId
                && x.ApprovalStatus == 2,
            x => x.CreatedAt,
            true);
        var latest = joinedList.FirstOrDefault();
        if (latest?.DirectManagerId is > 0)
        {
            return latest.DirectManagerId;
        }
        var employeeDepts = await _employeeDeptRepository.GetListAsync(
            x => x.TenantCode == tenantCode
                && x.CompanyCode == companyCode
                && x.EmployeeId == employeeId,
            x => x.CreatedAt,
            false);
        var primaryDeptId = employeeDepts.FirstOrDefault(x => x.DeptId > 0)?.DeptId ?? 0;
        if (primaryDeptId <= 0)
        {
            return null;
        }
        var dept = await _deptRepository.GetByIdAsync(primaryDeptId);
        if (dept?.HeadUserId > 0)
        {
            var headUser = await _userRepository.GetByIdAsync(dept.HeadUserId);
            if (headUser?.EmployeeId > 0)
            {
                return headUser.EmployeeId;
            }
        }
        return null;
    }

    /// <summary>
    /// 按员工 ID 查用户
    /// </summary>
    /// <param name="employeeId">员工 ID</param>
    /// <param name="tenantCode">租户</param>
    /// <returns>用户</returns>
    private async Task<TaktUser?> GetUserByEmployeeIdAsync(long employeeId, string tenantCode)
    {
        return await _userRepository.FirstAsync(x =>
            x.TenantCode == tenantCode && x.EmployeeId == employeeId && x.UserStatus == 1);
    }
}
