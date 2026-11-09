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
 * 查询本机已激活网卡 IP 归属（服务端枚举 Up 网卡，非 HTTP ::1）
 * @returns {Promise<IpGeolocation>} 优先激活 IP 归属；含 activeHostIps
 */
export function searchClientIpGeolocation(): Promise<IpGeolocation> {
  return request<IpGeolocation>({
    url: `${IP_GEOLOCATION_API_BASE}/client`,
    method: 'get',
  })
}

/**
 * 查询访问者公网 IP 归属（后端：请求公网地址或 API 出口；前端优先用浏览器 echo）
 * @returns {Promise<IpGeolocation>} 公网归属结果
 */
export function searchVisitorPublicIpGeolocation(): Promise<IpGeolocation> {
  return request<IpGeolocation>({
    url: `${IP_GEOLOCATION_API_BASE}/public`,
    method: 'get',
  })
}
