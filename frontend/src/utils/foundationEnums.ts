// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：foundationEnums.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：Foundation 域运行时枚举（与后端字典/Shared 数值对齐；设备/浏览器/操作系统见 sys_* 字典 DictValue）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 在线用户状态（与后端 sys_online_status 字典 DictValue 一致：0=在线，1=离线，2=离开）
 */
export enum TaktOnlineStatus {
  /** 在线 */
  Online = 0,
  /** 离线 */
  Offline = 1,
  /** 离开（含强退） */
  Away = 2,
}
