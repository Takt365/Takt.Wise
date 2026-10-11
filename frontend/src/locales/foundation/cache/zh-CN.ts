// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/cache
// 文件名称：zh-CN.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/cache 页面静态文案；引用键 foundation.cache.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "缓存管理",
    description: "查看运行时缓存配置与统计，并按键检查或移除缓存项",
    section: {
      config: "缓存配置",
      statistics: "缓存统计",
      key: {
        ops: "按键操作",
      },
    },
    field: {
      provider: "缓存提供者",
      default: {
        expiration: {
          minutes: "默认过期（分钟）",
        },
      },
      enable: {
        sliding: {
          expiration: "滑动过期",
        },
        multi: {
          level: {
            cache: "多级缓存",
          },
        },
      },
      redis: {
        instance: {
          name: "Redis 实例前缀",
        },
      },
      note: "说明",
      current: {
        entry: {
          count: "当前条目数",
        },
      },
      total: {
        hits: "命中次数",
        misses: "未命中次数",
      },
      hit: {
        rate: "命中率",
      },
      estimated: {
        size: {
          bytes: "估算占用（字节）",
        },
      },
      cache: {
        key: "缓存键",
      },
    },
    placeholder: {
      cache: {
        key: "请输入完整缓存键",
      },
    },
    button: {
      check: {
        exists: "检查存在",
      },
      remove: "移除键",
    },
    rule: {
      cache: {
        key: {
          required: "请输入缓存键",
        },
      },
    },
    message: {
      load: {
        fail: "加载缓存信息失败",
      },
      check: {
        fail: "检查缓存键失败",
      },
      remove: {
        success: "缓存键已移除",
        fail: "移除缓存键失败",
      },
      loading: {
        hint: "加载中…",
      },
    },
    alert: {
      key: {
        exists: "缓存键存在",
        not: {
          exists: "缓存键不存在",
        },
      },
    },
  },
};
