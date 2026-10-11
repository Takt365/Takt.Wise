// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/api/foundation
// 文件名称：ipGeolocation.api.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：IP 归属查询 API（对应 TaktIpGeolocationsController）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import request from '@/api/request'
import type { IpGeolocation } from '@/types/foundation/ipGeolocation.types'

export type { IpGeolocation }

/**
 * API 路径前缀（相对 request baseURL，对应后端 TaktIpGeolocationsController）
 * @description TaktIpGeolocations
 */
const IP_GEOLOCATION_API_BASE = 'TaktIpGeolocations'

/**
 * 按 IP 查询归属地
 * @param {string} ip IPv4 或 IPv6
 * @returns {Promise<IpGeolocation>} 归属结果
 */
export function searchIpGeolocation(ip: string): Promise<IpGeolocation> {
  return request<IpGeolocation>({
    url: `${IP_GEOLOCATION_API_BASE}/search`,
    method: 'get',
    params: { ip },
  })
}

/**
 * 查询当前请求客户端 IP 归属地
 * @returns {Promise<IpGeolocation>} 归属结果
 */
export function searchClientIpGeolocation(): Promise<IpGeolocation> {
  return request<IpGeolocation>({
    url: `${IP_GEOLOCATION_API_BASE}/client`,
    method: 'get',
  })
}
