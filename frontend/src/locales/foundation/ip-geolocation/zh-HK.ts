// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/ip-geolocation
// 文件名称：zh-HK.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/ip-geolocation 頁面靜態文案；引用鍵 foundation.ip-geolocation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'IP歸屬查詢',
    description: '基於 ip2region 離線庫查詢 IPv4 / IPv6 歸屬地',
    section: {
      query: '查詢',
      result: '歸屬結果',
    },
    field: {
      ip: 'IP 地址',
      country: '國家',
      region: '區域',
      province: '省份',
      city: '城市',
      isp: 'ISP',
      full: {
        address: '完整地址',
      },
      formatted: {
        address: '格式化地址',
      },
    },
    placeholder: {
      ip: '請輸入 IPv4 或 IPv6，例如 8.8.8.8',
    },
    button: {
      search: '查詢',
      client: '查詢本機 IP',
    },
    rule: {
      ip: {
        required: '請輸入 IP 地址',
      },
    },
    message: {
      search: {
        fail: 'IP 歸屬查詢失敗',
      },
      client: {
        fail: '客戶端 IP 歸屬查詢失敗',
      },
      not: {
        found: '未找到該 IP 的歸屬資訊',
      },
    },
    alert: {
      found: '查詢成功',
      not: {
        found: '未命中歸屬資料',
      },
    },
  },
}
