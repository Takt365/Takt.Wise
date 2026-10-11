// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/identity
// 文件名称：userFormView.types.ts
// 创建时间：2026-05-28
// 创建人：Takt365(Cursor AI)
// 功能描述：用户表单视图模型（手工维护；勿写入自动生成的 user.d.ts）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import type { User } from '@/types/identity/user.types';

/**
 * 父组件传入 `user-form` 的编辑数据：API `User` 字段 + 列表行别名 + 权限回填（可选）
 */
export type UserFormDataInput = Partial<User> & {
  /** 登录名别名（`index.vue` 的 `toUserAssignRecord` 由 `userName` 补齐） */
  userName?: string;
  /** 昵称别名（由 `nickName` 补齐） */
  nickName?: string;
  /** 备注 */
  remark?: string;
  /** 角色 ID 列表（RBAC 回填） */
  roleIds?: Array<string | number>;
};

/**
 * 用户表单「员工 / 用户」标签页字段（a-form 绑定，camelCase 与表单项 name 一致）
 */
export interface UserFormModel {
  /**
   * 关联员工 ID
   */
  employeeId: string;

  /**
   * 登录用户名（表单字段 userName，提交时映射为 API userName）
   */
  userName: string;

  /**
   * 昵称（表单字段 nickName，提交时映射为 API nickName）
   */
  nickName: string;

  /**
   * 用户类型（字典 sys_user_type）
   */
  userType: number;

  /**
   * 登录密码（仅新增；提交时映射为 passwordHash）
   */
  password: string;

  /**
   * 用户状态（字典 sys_normal_disable）
   */
  userStatus: number;

  /**
   * 备注
   */
  remark: string;
}

/**
 * 用户表单「权限」标签页字段
 */
export interface UserFormPermissionModel {
  /**
   * 角色 ID 列表
   */
  roleIds: string[];
}

/**
 * `UserForm.vue` defineExpose.getValues() 返回值（合并基础信息与权限）
 */
export type UserFormValues = UserFormModel & UserFormPermissionModel;
