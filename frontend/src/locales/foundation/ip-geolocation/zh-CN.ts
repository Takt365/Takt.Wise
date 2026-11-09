// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/ip-geolocation
// 文件名称：zh-CN.ts
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/ip-geolocation 页面静态文案；引用键 foundation.ip-geolocation.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'IP归属',
    description: '基于 ip2region 离线库查询 IPv4 / IPv6 归属地',
    section: {
      query: '查询',
      result: '归属结果',
    },
    field: {
      ip: 'IP 地址',
      country: '国家',
      region: '区域',
      province: '省份',
      city: '城市',
      isp: 'ISP',
      active: {
        host: {
          ips: '已激活网卡',
        },
      },
      full: {
        address: '完整地址',
      },
      formatted: {
        address: '格式化地址',
      },
    },
    placeholder: {
      ip: '请输入 IPv4 或 IPv6，例如 8.8.8.8',
    },
    button: {
      search: '查询',
      client: '查询本机 IP',
    },
    rule: {
      ip: {
        required: '请输入 IP 地址',
      },
    },
    visitor: {
      loading: '正在获取您的公网 IP…',
      line: '您的IP：{ip},位置：{location}',
      unknown: {
        location: '未知',
      },
    },
    message: {
      search: {
        fail: 'IP 归属查询失败',
      },
      client: {
        fail: '本机网卡 IP 归属查询失败',
      },
      visitor: {
        fail: '无法自动获取公网 IP，请手动输入后查询',
      },
      not: {
        found: '未找到该 IP 的归属信息',
      },
    },
    alert: {
      found: '查询成功',
      not: {
        found: '未命中归属数据',
      },
    },
  },
}
