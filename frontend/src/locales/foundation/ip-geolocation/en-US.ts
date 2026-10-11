// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/ip-geolocation
// 文件名称：en-US.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/ip-geolocation page copy; keys foundation.ip-geolocation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'IP Geolocation',
    description: 'Lookup IPv4 / IPv6 geolocation via offline ip2region databases',
    section: {
      query: 'Query',
      result: 'Result',
    },
    field: {
      ip: 'IP address',
      country: 'Country',
      region: 'Region',
      province: 'Province',
      city: 'City',
      isp: 'ISP',
      full: {
        address: 'Full address',
      },
      formatted: {
        address: 'Formatted address',
      },
    },
    placeholder: {
      ip: 'Enter IPv4 or IPv6, e.g. 8.8.8.8',
    },
    button: {
      search: 'Search',
      client: 'My client IP',
    },
    rule: {
      ip: {
        required: 'Please enter an IP address',
      },
    },
    message: {
      search: {
        fail: 'IP geolocation lookup failed',
      },
      client: {
        fail: 'Client IP geolocation lookup failed',
      },
      not: {
        found: 'No geolocation found for this IP',
      },
    },
    alert: {
      found: 'Lookup succeeded',
      not: {
        found: 'No matching geolocation data',
      },
    },
  },
}
