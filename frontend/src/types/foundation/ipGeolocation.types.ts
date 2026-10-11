// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/foundation
// 文件名称：ipGeolocation.types.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：IP 归属查询类型（对应后端 TaktIpGeolocationDto）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * IP 归属查询结果
 * @description 对应后端 TaktIpGeolocationDto
 */
export interface IpGeolocation {
  /**
   * 查询的 IP 地址
   */
  ip: string

  /**
   * 是否命中定位结果（含内网占位结果）
   */
  found: boolean

  /**
   * 国家
   */
  country: string

  /**
   * 区域（省/州）
   */
  region: string

  /**
   * 省份
   */
  province: string

  /**
   * 城市
   */
  city: string

  /**
   * ISP（互联网服务提供商）
   */
  isp: string

  /**
   * 完整地址信息（国家|区域|省份|城市|ISP）
   */
  fullAddress: string

  /**
   * 格式化地址（用于显示）
   */
  formattedAddress: string
}
