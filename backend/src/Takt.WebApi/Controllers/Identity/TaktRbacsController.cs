// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.WebApi.Controllers.Identity
// 文件名称：TaktRbacsController.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：RBAC 八张关联表统一分配控制器
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.AspNetCore.Mvc;
using Takt.Application.Services.Identity;
using Takt.Shared.Constants;

namespace Takt.WebApi.Controllers.Identity;

/// <summary>
/// RBAC 关联分配控制器
/// </summary>
[ApiModule(1, "身份认证")]
[Route("api/[controller]", Name = "RBAC关联")]
public class TaktRbacsController : TaktControllerBase
{
    private readonly ITaktRbacService _rbacService;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="rbacService">RBAC 服务</param>
    public TaktRbacsController(ITaktRbacService rbacService)
    {
        _rbacService = rbacService;
    }

    #region 用户角色授权（TaktUserRole）

    /// <summary>
    /// 获取用户角色列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>用户角色列表</returns>
    [TaktPermission("identity:user:query", "获取用户角色")]
    [HttpGet("users/{userId}/roles")]
    public async Task<IActionResult> GetUserRoleIdsAsync(long userId)
    {
        try
        {
            var result = await _rbacService.GetUserRoleIdsAsync(userId);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分配用户角色
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="roleIds">角色ID列表</param>
    /// <returns>是否成功</returns>
    [TaktPermission("identity:user:update", "分配用户角色")]
    [HttpPut("users/{userId}/roles")]
    public async Task<IActionResult> AssignUserRolesAsync(long userId, [FromBody] long[] roleIds)
    {
        try
        {
            var result = await _rbacService.AssignUserRolesAsync(userId, roleIds);
            return Success(result ? "角色分配成功" : "角色分配失败");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    #region 用户租户与公司范围（TaktUserTenant、TaktUserCompany）

    /// <summary>
    /// 获取用户可访问租户关联列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>用户-租户关联列表</returns>
    [TaktPermission("identity:user:query", "获取用户租户")]
    [HttpGet("users/{userId}/tenants")]
    public async Task<IActionResult> GetUserTenantIdsAsync(long userId)
    {
        try
        {
            var result = await _rbacService.GetUserTenantIdsAsync(userId);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分配用户可访问租户
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="tenantCodes">租户编码列表</param>
    /// <returns>是否成功</returns>
    [TaktPermission("identity:user:update", "分配用户租户")]
    [HttpPut("users/{userId}/tenants")]
    public async Task<IActionResult> AssignUserTenantsAsync(long userId, [FromBody] string[] tenantCodes)
    {
        try
        {
            var result = await _rbacService.AssignUserTenantsAsync(userId, tenantCodes);
            return Success(result ? "租户分配成功" : "租户分配失败");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取用户可访问公司关联列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>用户-公司关联列表</returns>
    [TaktPermission("identity:user:query", "获取用户公司")]
    [HttpGet("users/{userId}/companies")]
    public async Task<IActionResult> GetUserCompanyIdsAsync(long userId)
    {
        try
        {
            var result = await _rbacService.GetUserCompanyIdsAsync(userId);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分配用户可访问公司
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="companyCodes">公司编码列表</param>
    /// <returns>是否成功</returns>
    [TaktPermission("identity:user:update", "分配用户公司")]
    [HttpPut("users/{userId}/companies")]
    public async Task<IActionResult> AssignUserCompaniesAsync(long userId, [FromBody] string[] companyCodes)
    {
        try
        {
            var result = await _rbacService.AssignUserCompaniesAsync(userId, companyCodes);
            return Success(result ? "公司分配成功" : "公司分配失败");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    #region 租户用户（TaktUserTenant 反向查询）

    /// <summary>
    /// 获取租户用户关联列表
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>用户-租户关联列表</returns>
    [TaktPermission("identity:tenant:query", "获取租户用户")]
    [HttpGet("tenants/{tenantCode}/users")]
    public async Task<IActionResult> GetTenantUserIdsAsync(string tenantCode)
    {
        try
        {
            var result = await _rbacService.GetTenantUserIdsAsync(tenantCode);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分配租户用户
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="userIds">用户ID列表</param>
    /// <returns>是否成功</returns>
    [TaktPermission("identity:tenant:update", "分配租户用户")]
    [HttpPut("tenants/{tenantCode}/users")]
    public async Task<IActionResult> AssignTenantUsersAsync(string tenantCode, [FromBody] long[] userIds)
    {
        try
        {
            var result = await _rbacService.AssignTenantUsersAsync(tenantCode, userIds);
            return Success(result ? "用户分配成功" : "用户分配失败");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    #region 角色菜单权限（TaktRoleMenu）

    /// <summary>
    /// 获取角色菜单关联列表
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>角色-菜单关联列表</returns>
    [TaktPermission("identity:role:query", "获取角色菜单")]
    [HttpGet("roles/{roleId}/menus")]
    public async Task<IActionResult> GetRoleMenuIdsAsync(long roleId)
    {
        try
        {
            var result = await _rbacService.GetRoleMenuIdsAsync(roleId);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分配角色菜单
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <param name="menuIds">菜单ID列表</param>
    /// <returns>是否成功</returns>
    [TaktPermission("identity:role:update", "分配角色菜单")]
    [HttpPut("roles/{roleId}/menus")]
    public async Task<IActionResult> AssignRoleMenusAsync(long roleId, [FromBody] long[] menuIds)
    {
        try
        {
            var result = await _rbacService.AssignRoleMenusAsync(roleId, menuIds);
            return Success(result ? "菜单分配成功" : "菜单分配失败");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    #region 角色数据权限范围（TaktRoleCompany、TaktRoleDept）

    /// <summary>
    /// 获取角色可访问公司关联列表
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>角色-公司关联列表</returns>
    [TaktPermission("identity:role:query", "获取角色公司")]
    [HttpGet("roles/{roleId}/companies")]
    public async Task<IActionResult> GetRoleCompanyIdsAsync(long roleId)
    {
        try
        {
            var result = await _rbacService.GetRoleCompanyIdsAsync(roleId);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分配角色可访问公司
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <param name="companyCodes">公司编码列表</param>
    /// <returns>是否成功</returns>
    [TaktPermission("identity:role:update", "分配角色公司")]
    [HttpPut("roles/{roleId}/companies")]
    public async Task<IActionResult> AssignRoleCompaniesAsync(long roleId, [FromBody] string[] companyCodes)
    {
        try
        {
            var result = await _rbacService.AssignRoleCompaniesAsync(roleId, companyCodes);
            return Success(result ? "公司分配成功" : "公司分配失败");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取角色部门关联列表
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>角色-部门关联列表</returns>
    [TaktPermission("identity:role:query", "获取角色部门")]
    [HttpGet("roles/{roleId}/depts")]
    public async Task<IActionResult> GetRoleDeptIdsAsync(long roleId)
    {
        try
        {
            var result = await _rbacService.GetRoleDeptIdsAsync(roleId);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分配角色部门
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <param name="deptIds">部门ID列表</param>
    /// <returns>是否成功</returns>
    [TaktPermission("identity:role:update", "分配角色部门")]
    [HttpPut("roles/{roleId}/depts")]
    public async Task<IActionResult> AssignRoleDeptsAsync(long roleId, [FromBody] long[] deptIds)
    {
        try
        {
            var result = await _rbacService.AssignRoleDeptsAsync(roleId, deptIds);
            return Success(result ? "部门分配成功" : "部门分配失败");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    #region 员工组织关系（TaktEmployeeDept、TaktEmployeePost）

    /// <summary>
    /// 获取员工部门关联列表
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <returns>员工-部门关联列表</returns>
    [TaktPermission("human:resource:personnel:employee:query", "获取员工部门")]
    [HttpGet("employees/{employeeId}/depts")]
    public async Task<IActionResult> GetEmployeeDeptIdsAsync(long employeeId)
    {
        try
        {
            var result = await _rbacService.GetEmployeeDeptIdsAsync(employeeId);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分配员工部门
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <param name="deptIds">部门ID列表</param>
    /// <returns>是否成功</returns>
    [TaktPermission("human:resource:personnel:employee:update", "分配员工部门")]
    [HttpPut("employees/{employeeId}/depts")]
    public async Task<IActionResult> AssignEmployeeDeptsAsync(long employeeId, [FromBody] long[] deptIds)
    {
        try
        {
            var result = await _rbacService.AssignEmployeeDeptsAsync(employeeId, deptIds);
            return Success(result ? "部门分配成功" : "部门分配失败");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取员工岗位关联列表
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <returns>员工-岗位关联列表</returns>
    [TaktPermission("human:resource:personnel:employee:query", "获取员工岗位")]
    [HttpGet("employees/{employeeId}/posts")]
    public async Task<IActionResult> GetEmployeePostIdsAsync(long employeeId)
    {
        try
        {
            var result = await _rbacService.GetEmployeePostIdsAsync(employeeId);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分配员工岗位
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <param name="postIds">岗位ID列表</param>
    /// <returns>是否成功</returns>
    [TaktPermission("human:resource:personnel:employee:update", "分配员工岗位")]
    [HttpPut("employees/{employeeId}/posts")]
    public async Task<IActionResult> AssignEmployeePostsAsync(long employeeId, [FromBody] long[] postIds)
    {
        try
        {
            var result = await _rbacService.AssignEmployeePostsAsync(employeeId, postIds);
            return Success(result ? "岗位分配成功" : "岗位分配失败");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取岗位员工关联列表
    /// </summary>
    /// <param name="postId">岗位ID</param>
    /// <returns>员工-岗位关联列表</returns>
    [TaktPermission("human:resource:organization:post:update", "获取岗位员工")]
    [HttpGet("posts/{postId}/employees")]
    public async Task<IActionResult> GetPostEmployeeIdsAsync(long postId)
    {
        try
        {
            var result = await _rbacService.GetPostEmployeeIdsAsync(postId);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分配岗位员工
    /// </summary>
    /// <param name="postId">岗位ID</param>
    /// <param name="employeeIds">员工ID列表</param>
    /// <returns>是否成功</returns>
    [TaktPermission("human:resource:organization:post:update", "分配岗位员工")]
    [HttpPut("posts/{postId}/employees")]
    public async Task<IActionResult> AssignPostEmployeesAsync(long postId, [FromBody] long[] employeeIds)
    {
        try
        {
            var result = await _rbacService.AssignPostEmployeesAsync(postId, employeeIds);
            return Success(result ? "员工分配成功" : "员工分配失败");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 获取部门员工关联列表
    /// </summary>
    /// <param name="deptId">部门ID</param>
    /// <returns>员工-部门关联列表</returns>
    [TaktPermission("human:resource:organization:dept:update", "获取部门员工")]
    [HttpGet("depts/{deptId}/employees")]
    public async Task<IActionResult> GetDeptEmployeeIdsAsync(long deptId)
    {
        try
        {
            var result = await _rbacService.GetDeptEmployeeIdsAsync(deptId);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分配部门员工
    /// </summary>
    /// <param name="deptId">部门ID</param>
    /// <param name="employeeIds">员工ID列表</param>
    /// <returns>是否成功</returns>
    [TaktPermission("human:resource:organization:dept:update", "分配部门员工")]
    [HttpPut("depts/{deptId}/employees")]
    public async Task<IActionResult> AssignDeptEmployeesAsync(long deptId, [FromBody] long[] employeeIds)
    {
        try
        {
            var result = await _rbacService.AssignDeptEmployeesAsync(deptId, employeeIds);
            return Success(result ? "员工分配成功" : "员工分配失败");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion

    #region 菜单角色（TaktRoleMenu 反向查询）

    /// <summary>
    /// 获取菜单角色关联列表
    /// </summary>
    /// <param name="menuId">菜单ID</param>
    /// <returns>角色-菜单关联列表</returns>
    [TaktPermission("identity:menu:update", "获取菜单角色")]
    [HttpGet("menus/{menuId}/roles")]
    public async Task<IActionResult> GetMenuRoleIdsAsync(long menuId)
    {
        try
        {
            var result = await _rbacService.GetMenuRoleIdsAsync(menuId);
            return Success(result, "查询成功");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    /// <summary>
    /// 分配菜单角色
    /// </summary>
    /// <param name="menuId">菜单ID</param>
    /// <param name="roleIds">角色ID列表</param>
    /// <returns>是否成功</returns>
    [TaktPermission("identity:menu:update", "分配菜单角色")]
    [HttpPut("menus/{menuId}/roles")]
    public async Task<IActionResult> AssignMenuRolesAsync(long menuId, [FromBody] long[] roleIds)
    {
        try
        {
            var result = await _rbacService.AssignMenuRolesAsync(menuId, roleIds);
            return Success(result ? "角色分配成功" : "角色分配失败");
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    #endregion
}
