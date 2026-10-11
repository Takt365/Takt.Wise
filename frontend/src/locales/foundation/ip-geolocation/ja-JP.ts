// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/ip-geolocation
// 文件名称：ja-JP.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/ip-geolocation ページ文言；キー foundation.ip-geolocation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'IP位置照会',
    description: 'ip2region オフラインDBで IPv4 / IPv6 の位置を照会します',
    section: {
      query: '照会',
      result: '結果',
    },
    field: {
      ip: 'IPアドレス',
      country: '国',
      region: '地域',
      province: '省/州',
      city: '都市',
      isp: 'ISP',
      full: {
        address: '完全住所',
      },
      formatted: {
        address: '表示用住所',
      },
    },
    placeholder: {
      ip: 'IPv4 または IPv6 を入力（例: 8.8.8.8）',
    },
    button: {
      search: '照会',
      client: 'クライアントIPを照会',
    },
    rule: {
      ip: {
        required: 'IPアドレスを入力してください',
      },
    },
    message: {
      search: {
        fail: 'IP位置照会に失敗しました',
      },
      client: {
        fail: 'クライアントIPの位置照会に失敗しました',
      },
      not: {
        found: 'このIPの位置情報が見つかりません',
      },
    },
    alert: {
      found: '照会成功',
      not: {
        found: '位置データに一致しません',
      },
    },
  },
}
