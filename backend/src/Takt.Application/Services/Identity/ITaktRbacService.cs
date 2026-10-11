// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Services.Identity
// 文件名称：ITaktRbacService.cs
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：RBAC 八张关联表统一分配服务（TaktUserRole/TaktUserTenant/TaktUserCompany/TaktRoleMenu/TaktRoleCompany/TaktRoleDept/TaktEmployeeDept/TaktEmployeePost）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Takt.Application.Dtos.HumanResource.Organization;
using Takt.Application.Dtos.Identity;

namespace Takt.Application.Services.Identity;

/// <summary>
/// RBAC 关联分配应用服务接口（八张关联表：列表查询 + 全量覆盖分配，非关联表 CRUD）
/// </summary>
public interface ITaktRbacService
{
    #region 用户角色授权（TaktUserRole）

    /// <summary>
    /// 获取用户角色关联列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>用户-角色关联列表</returns>
    Task<List<TaktUserRoleDto>> GetUserRoleIdsAsync(long userId);

    /// <summary>
    /// 批量获取用户角色名称（导出用；UserId → 逗号分隔角色名）
    /// </summary>
    /// <param name="userIds">用户 ID 集合</param>
    /// <returns>用户 ID 到角色名称的映射</returns>
    Task<IReadOnlyDictionary<long, string>> GetUserRoleNamesMapAsync(IEnumerable<long> userIds);

    /// <summary>
    /// 分配用户角色（全量覆盖）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="roleIds">角色ID列表</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignUserRolesAsync(long userId, long[] roleIds);

    #endregion

    #region 用户租户与公司范围（TaktUserTenant、TaktUserCompany）

    /// <summary>
    /// 获取用户租户关联列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>用户-租户关联列表</returns>
    Task<List<TaktUserTenantDto>> GetUserTenantIdsAsync(long userId);

    /// <summary>
    /// 分配用户可访问租户（全量覆盖，TenantCode 列表）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="tenantCodes">租户编码列表</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignUserTenantsAsync(long userId, string[] tenantCodes);

    /// <summary>
    /// 获取租户用户关联列表
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <returns>用户-租户关联列表</returns>
    Task<List<TaktUserTenantDto>> GetTenantUserIdsAsync(string tenantCode);

    /// <summary>
    /// 分配租户用户（全量覆盖）
    /// </summary>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="userIds">用户ID列表</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignTenantUsersAsync(string tenantCode, long[] userIds);

    /// <summary>
    /// 获取用户公司关联列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>用户-公司关联列表</returns>
    Task<List<TaktUserCompanyDto>> GetUserCompanyIdsAsync(long userId);

    /// <summary>
    /// 分配用户可访问公司（全量覆盖，CompanyCode 列表）
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="companyCodes">公司编码列表</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignUserCompaniesAsync(long userId, string[] companyCodes);

    #endregion

    #region 角色菜单权限（TaktRoleMenu）

    /// <summary>
    /// 获取角色菜单关联列表
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>角色-菜单关联列表</returns>
    Task<List<TaktRoleMenuDto>> GetRoleMenuIdsAsync(long roleId);

    /// <summary>
    /// 分配角色菜单（全量覆盖）
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <param name="menuIds">菜单ID列表</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignRoleMenusAsync(long roleId, long[] menuIds);

    /// <summary>
    /// 获取菜单角色关联列表
    /// </summary>
    /// <param name="menuId">菜单ID</param>
    /// <returns>角色-菜单关联列表</returns>
    Task<List<TaktRoleMenuDto>> GetMenuRoleIdsAsync(long menuId);

    /// <summary>
    /// 分配菜单角色（全量覆盖）
    /// </summary>
    /// <param name="menuId">菜单ID</param>
    /// <param name="roleIds">角色ID列表</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignMenuRolesAsync(long menuId, long[] roleIds);

    #endregion

    #region 角色数据权限范围（TaktRoleCompany、TaktRoleDept）

    /// <summary>
    /// 获取角色公司关联列表
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>角色-公司关联列表</returns>
    Task<List<TaktRoleCompanyDto>> GetRoleCompanyIdsAsync(long roleId);

    /// <summary>
    /// 分配角色可访问公司（全量覆盖，CompanyCode 列表）
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <param name="companyCodes">公司编码列表</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignRoleCompaniesAsync(long roleId, string[] companyCodes);

    /// <summary>
    /// 获取角色部门关联列表（数据权限自定义范围）
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <returns>角色-部门关联列表</returns>
    Task<List<TaktRoleDeptDto>> GetRoleDeptIdsAsync(long roleId);

    /// <summary>
    /// 分配角色部门（全量覆盖，部门 ID 列表；TaktRbacsController 等 API 使用）
    /// </summary>
    /// <param name="roleId">角色ID</param>
    /// <param name="deptIds">部门ID列表</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignRoleDeptsAsync(long roleId, long[] deptIds);

    #endregion

    #region 员工组织关系（TaktEmployeeDept、TaktEmployeePost）

    /// <summary>
    /// 获取员工部门关联列表
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <returns>员工-部门关联列表</returns>
    Task<List<TaktEmployeeDeptDto>> GetEmployeeDeptIdsAsync(long employeeId);

    /// <summary>
    /// 分配员工部门（全量覆盖）
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <param name="deptIds">部门ID列表</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignEmployeeDeptsAsync(long employeeId, long[] deptIds);

    /// <summary>
    /// 获取部门员工关联列表
    /// </summary>
    /// <param name="deptId">部门ID</param>
    /// <returns>员工-部门关联列表</returns>
    Task<List<TaktEmployeeDeptDto>> GetDeptEmployeeIdsAsync(long deptId);

    /// <summary>
    /// 分配部门员工（全量覆盖）
    /// </summary>
    /// <param name="deptId">部门ID</param>
    /// <param name="employeeIds">员工ID列表</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignDeptEmployeesAsync(long deptId, long[] employeeIds);

    /// <summary>
    /// 获取员工岗位关联列表
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <returns>员工-岗位关联列表</returns>
    Task<List<TaktEmployeePostDto>> GetEmployeePostIdsAsync(long employeeId);

    /// <summary>
    /// 分配员工岗位（全量覆盖）
    /// </summary>
    /// <param name="employeeId">员工ID</param>
    /// <param name="postIds">岗位ID列表</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignEmployeePostsAsync(long employeeId, long[] postIds);

    /// <summary>
    /// 获取岗位员工关联列表
    /// </summary>
    /// <param name="postId">岗位ID</param>
    /// <returns>员工-岗位关联列表</returns>
    Task<List<TaktEmployeePostDto>> GetPostEmployeeIdsAsync(long postId);

    /// <summary>
    /// 分配岗位员工（全量覆盖）
    /// </summary>
    /// <param name="postId">岗位ID</param>
    /// <param name="employeeIds">员工ID列表</param>
    /// <returns>是否成功</returns>
    Task<bool> AssignPostEmployeesAsync(long postId, long[] employeeIds);

    #endregion
}
