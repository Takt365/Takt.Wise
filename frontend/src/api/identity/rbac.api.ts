// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/identity
// 文件名称：rbac.api.ts
// 创建时间：2026-05-27
// 创建人：Takt365(Cursor AI)
// 功能描述：RBAC 八张关联表统一分配 API（对应 TaktRbacsController）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request';
import type { EmployeeDept } from '@/types/human-resource/organization/employeeDept.types';
import type { EmployeePost } from '@/types/human-resource/organization/employeePost.types';
import type { RoleDept } from '@/types/human-resource/organization/roleDept.types';
import type { RoleCompany } from '@/types/identity/roleCompany.types';
import type { RoleMenu } from '@/types/identity/roleMenu.types';
import type { UserCompany } from '@/types/identity/userCompany.types';
import type { UserRole } from '@/types/identity/userRole.types';
import type { UserTenant } from '@/types/identity/userTenant.types';

/**
 * API 路径前缀（相对 request baseURL，对应后端 [controller]）
 * @description TaktRbacs
 */
const RBAC_API_BASE = 'TaktRbacs';

// ========================================
// 用户角色授权（TaktUserRole）
// ========================================

/**
 * 获取用户角色列表
 * @param {string} userId 用户ID
 * @returns {Promise<UserRole[]>} 用户角色列表
 */
export function getUserRoleIds(userId: string): Promise<UserRole[]> {
  return request<UserRole[]>({
    url: `${RBAC_API_BASE}/users/${userId}/roles`,
    method: 'get',
  });
}

/**
 * 分配用户角色
 * @param {string} userId 用户ID
 * @param {string[]} roleIds 角色ID列表
 * @returns {Promise<boolean>} 是否成功
 */
export function assignUserRoles(userId: string, roleIds: string[]): Promise<boolean> {
  return request<boolean>({
    url: `${RBAC_API_BASE}/users/${userId}/roles`,
    method: 'put',
    data: roleIds,
  });
}

// ========================================
// 用户租户与公司范围（TaktUserTenant、TaktUserCompany）
// ========================================

/**
 * 获取用户可访问租户关联列表
 * @param {string} userId 用户ID
 * @returns {Promise<UserTenant[]>} 用户-租户关联列表
 */
export function getUserTenantIds(userId: string): Promise<UserTenant[]> {
  return request<UserTenant[]>({
    url: `${RBAC_API_BASE}/users/${userId}/tenants`,
    method: 'get',
  });
}

/**
 * 分配用户可访问租户
 * @param {string} userId 用户ID
 * @param {string[]} tenantCodes 租户编码列表
 * @returns {Promise<boolean>} 是否成功
 */
export function assignUserTenants(userId: string, tenantCodes: string[]): Promise<boolean> {
  return request<boolean>({
    url: `${RBAC_API_BASE}/users/${userId}/tenants`,
    method: 'put',
    data: tenantCodes,
  });
}

/**
 * 获取用户可访问公司关联列表
 * @param {string} userId 用户ID
 * @returns {Promise<UserCompany[]>} 用户-公司关联列表
 */
export function getUserCompanyIds(userId: string): Promise<UserCompany[]> {
  return request<UserCompany[]>({
    url: `${RBAC_API_BASE}/users/${userId}/companies`,
    method: 'get',
  });
}

/**
 * 分配用户可访问公司
 * @param {string} userId 用户ID
 * @param {string[]} companyCodes 公司编码列表
 * @returns {Promise<boolean>} 是否成功
 */
export function assignUserCompanies(userId: string, companyCodes: string[]): Promise<boolean> {
  return request<boolean>({
    url: `${RBAC_API_BASE}/users/${userId}/companies`,
    method: 'put',
    data: companyCodes,
  });
}

// ========================================
// 租户用户（TaktUserTenant 反向查询）
// ========================================

/**
 * 获取租户用户关联列表
 * @param {string} tenantCode 租户编码
 * @returns {Promise<UserTenant[]>} 用户-租户关联列表
 */
export function getTenantUserIds(tenantCode: string): Promise<UserTenant[]> {
  return request<UserTenant[]>({
    url: `${RBAC_API_BASE}/tenants/${encodeURIComponent(tenantCode)}/users`,
    method: 'get',
  });
}

/**
 * 分配租户用户
 * @param {string} tenantCode 租户编码
 * @param {string[]} userIds 用户ID列表
 * @returns {Promise<boolean>} 是否成功
 */
export function assignTenantUsers(tenantCode: string, userIds: string[]): Promise<boolean> {
  return request<boolean>({
    url: `${RBAC_API_BASE}/tenants/${encodeURIComponent(tenantCode)}/users`,
    method: 'put',
    data: userIds,
  });
}

// ========================================
// 角色菜单权限（TaktRoleMenu）
// ========================================

/**
 * 获取角色菜单关联列表
 * @param {string} roleId 角色ID
 * @returns {Promise<RoleMenu[]>} 角色-菜单关联列表
 */
export function getRoleMenuIds(roleId: string): Promise<RoleMenu[]> {
  return request<RoleMenu[]>({
    url: `${RBAC_API_BASE}/roles/${roleId}/menus`,
    method: 'get',
  });
}

/**
 * 分配角色菜单
 * @param {string} roleId 角色ID
 * @param {string[]} menuIds 菜单ID列表
 * @returns {Promise<boolean>} 是否成功
 */
export function assignRoleMenus(roleId: string, menuIds: string[]): Promise<boolean> {
  return request<boolean>({
    url: `${RBAC_API_BASE}/roles/${roleId}/menus`,
    method: 'put',
    data: menuIds,
  });
}

/**
 * 获取菜单角色关联列表
 * @param {string} menuId 菜单ID
 * @returns {Promise<RoleMenu[]>} 角色-菜单关联列表
 */
export function getMenuRoleIds(menuId: string): Promise<RoleMenu[]> {
  return request<RoleMenu[]>({
    url: `${RBAC_API_BASE}/menus/${menuId}/roles`,
    method: 'get',
  });
}

/**
 * 分配菜单角色
 * @param {string} menuId 菜单ID
 * @param {string[]} roleIds 角色ID列表
 * @returns {Promise<boolean>} 是否成功
 */
export function assignMenuRoles(menuId: string, roleIds: string[]): Promise<boolean> {
  return request<boolean>({
    url: `${RBAC_API_BASE}/menus/${menuId}/roles`,
    method: 'put',
    data: roleIds,
  });
}

// ========================================
// 角色数据权限范围（TaktRoleCompany、TaktRoleDept）
// ========================================

/**
 * 获取角色可访问公司关联列表
 * @param {string} roleId 角色ID
 * @returns {Promise<RoleCompany[]>} 角色-公司关联列表
 */
export function getRoleCompanyIds(roleId: string): Promise<RoleCompany[]> {
  return request<RoleCompany[]>({
    url: `${RBAC_API_BASE}/roles/${roleId}/companies`,
    method: 'get',
  });
}

/**
 * 分配角色可访问公司
 * @param {string} roleId 角色ID
 * @param {string[]} companyCodes 公司编码列表
 * @returns {Promise<boolean>} 是否成功
 */
export function assignRoleCompanies(roleId: string, companyCodes: string[]): Promise<boolean> {
  return request<boolean>({
    url: `${RBAC_API_BASE}/roles/${roleId}/companies`,
    method: 'put',
    data: companyCodes,
  });
}

/**
 * 获取角色部门关联列表
 * @param {string} roleId 角色ID
 * @returns {Promise<RoleDept[]>} 角色-部门关联列表
 */
export function getRoleDeptIds(roleId: string): Promise<RoleDept[]> {
  return request<RoleDept[]>({
    url: `${RBAC_API_BASE}/roles/${roleId}/depts`,
    method: 'get',
  });
}

/**
 * 分配角色部门
 * @param {string} roleId 角色ID
 * @param {string[]} deptIds 部门ID列表
 * @returns {Promise<boolean>} 是否成功
 */
export function assignRoleDepts(roleId: string, deptIds: string[]): Promise<boolean> {
  return request<boolean>({
    url: `${RBAC_API_BASE}/roles/${roleId}/depts`,
    method: 'put',
    data: deptIds,
  });
}

// ========================================
// 员工组织关系（TaktEmployeeDept、TaktEmployeePost）
// ========================================

/**
 * 获取员工部门关联列表
 * @param {string} employeeId 员工ID
 * @returns {Promise<EmployeeDept[]>} 员工-部门关联列表
 */
export function getEmployeeDeptIds(employeeId: string): Promise<EmployeeDept[]> {
  return request<EmployeeDept[]>({
    url: `${RBAC_API_BASE}/employees/${employeeId}/depts`,
    method: 'get',
  });
}

/**
 * 分配员工部门
 * @param {string} employeeId 员工ID
 * @param {string[]} deptIds 部门ID列表
 * @returns {Promise<boolean>} 是否成功
 */
export function assignEmployeeDepts(employeeId: string, deptIds: string[]): Promise<boolean> {
  return request<boolean>({
    url: `${RBAC_API_BASE}/employees/${employeeId}/depts`,
    method: 'put',
    data: deptIds,
  });
}

/**
 * 获取员工岗位关联列表
 * @param {string} employeeId 员工ID
 * @returns {Promise<EmployeePost[]>} 员工-岗位关联列表
 */
export function getEmployeePostIds(employeeId: string): Promise<EmployeePost[]> {
  return request<EmployeePost[]>({
    url: `${RBAC_API_BASE}/employees/${employeeId}/posts`,
    method: 'get',
  });
}

/**
 * 分配员工岗位
 * @param {string} employeeId 员工ID
 * @param {string[]} postIds 岗位ID列表
 * @returns {Promise<boolean>} 是否成功
 */
export function assignEmployeePosts(employeeId: string, postIds: string[]): Promise<boolean> {
  return request<boolean>({
    url: `${RBAC_API_BASE}/employees/${employeeId}/posts`,
    method: 'put',
    data: postIds,
  });
}

/**
 * 获取岗位员工关联列表
 * @param {string} postId 岗位ID
 * @returns {Promise<EmployeePost[]>} 员工-岗位关联列表
 */
export function getPostEmployeeIds(postId: string): Promise<EmployeePost[]> {
  return request<EmployeePost[]>({
    url: `${RBAC_API_BASE}/posts/${postId}/employees`,
    method: 'get',
  });
}

/**
 * 分配岗位员工
 * @param {string} postId 岗位ID
 * @param {string[]} employeeIds 员工ID列表
 * @returns {Promise<boolean>} 是否成功
 */
export function assignPostEmployees(postId: string, employeeIds: string[]): Promise<boolean> {
  return request<boolean>({
    url: `${RBAC_API_BASE}/posts/${postId}/employees`,
    method: 'put',
    data: employeeIds,
  });
}

/**
 * 获取部门员工关联列表
 * @param {string} deptId 部门ID
 * @returns {Promise<EmployeeDept[]>} 员工-部门关联列表
 */
export function getDeptEmployeeIds(deptId: string): Promise<EmployeeDept[]> {
  return request<EmployeeDept[]>({
    url: `${RBAC_API_BASE}/depts/${deptId}/employees`,
    method: 'get',
  });
}

/**
 * 分配部门员工
 * @param {string} deptId 部门ID
 * @param {string[]} employeeIds 员工ID列表
 * @returns {Promise<boolean>} 是否成功
 */
export function assignDeptEmployees(deptId: string, employeeIds: string[]): Promise<boolean> {
  return request<boolean>({
    url: `${RBAC_API_BASE}/depts/${deptId}/employees`,
    method: 'put',
    data: employeeIds,
  });
}
