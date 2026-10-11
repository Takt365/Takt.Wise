// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/foundation/cache
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：foundation/cache page static copy; keys foundation.cache.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "Cache Management",
    description: "Inspect runtime cache configuration and statistics; check or remove entries by key",
    section: {
      config: "Configuration",
      statistics: "Statistics",
      key: {
        ops: "Key Operations",
      },
    },
    field: {
      provider: "Provider",
      default: {
        expiration: {
          minutes: "Default expiration (minutes)",
        },
      },
      enable: {
        sliding: {
          expiration: "Sliding expiration",
        },
        multi: {
          level: {
            cache: "Multi-level cache",
          },
        },
      },
      redis: {
        instance: {
          name: "Redis instance prefix",
        },
      },
      note: "Note",
      current: {
        entry: {
          count: "Current entries",
        },
      },
      total: {
        hits: "Hits",
        misses: "Misses",
      },
      hit: {
        rate: "Hit rate",
      },
      estimated: {
        size: {
          bytes: "Estimated size (bytes)",
        },
      },
      cache: {
        key: "Cache key",
      },
    },
    placeholder: {
      cache: {
        key: "Enter the full cache key",
      },
    },
    button: {
      check: {
        exists: "Check exists",
      },
      remove: "Remove key",
    },
    rule: {
      cache: {
        key: {
          required: "Cache key is required",
        },
      },
    },
    message: {
      load: {
        fail: "Failed to load cache information",
      },
      check: {
        fail: "Failed to check cache key",
      },
      remove: {
        success: "Cache key removed",
        fail: "Failed to remove cache key",
      },
      loading: {
        hint: "Loading…",
      },
    },
    alert: {
      key: {
        exists: "Cache key exists",
        not: {
          exists: "Cache key does not exist",
        },
      },
    },
  },
};
