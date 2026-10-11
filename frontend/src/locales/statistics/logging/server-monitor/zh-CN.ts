// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/statistics/logging/server-monitor
// 文件名称：zh-CN.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：statistics/logging/server-monitor 页面静态文案；引用键 statistics.logging.server-monitor.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "服务监控",
    description: "查看应用运行状态与服务器硬件信息（CPU、内存、显卡、磁盘、网络）",
    tabs: {
      app: "应用",
      system: "系统",
      cpu: "CPU",
      memory: "内存",
      gpu: "显卡",
      drive: "磁盘",
      network: "网络",
    },
    section: {
      app: {
        status: "应用状态",
      },
      os: {
        language: "操作系统与语言",
      },
      motherboard: "主板",
      cpu: "CPU 信息",
      memory: "内存信息",
      gpu: "显卡信息",
      drive: "磁盘信息",
      network: "网络适配器",
    },
    field: {
      application: {
        name: "应用名称",
        version: "应用版本",
      },
      environment: "运行环境",
      machine: {
        name: "机器名",
      },
      dot: {
        net: {
          version: ".NET 版本",
        },
      },
      process: {
        architecture: "进程架构",
      },
      processor: {
        count: "处理器数量",
      },
      start: {
        time: "启动时间",
      },
      uptime: "运行时长",
      working: {
        set: "工作集内存",
      },
      operating: {
        system: "操作系统",
      },
      os: {
        version: "系统版本",
      },
      current: {
        culture: "当前区域文化",
        ui: {
          culture: "当前 UI 文化",
        },
      },
      system: {
        type: "系统架构",
        type32: "32 位",
        type64: "64 位",
      },
      motherboard: {
        manufacturer: "主板厂商",
        product: "主板型号 / ID",
        serial: {
          number: "主板序列号",
        },
        version: "主板版本",
        uuid: "机器 UUID",
      },
      cpu: {
        name: "名称",
        manufacturer: "制造商",
        cores: "核心数",
        logical: {
          processors: "逻辑处理器",
          core: {
            name: "逻辑核心",
          },
        },
        usage: {
          percent: "CPU 使用率",
        },
        model: "CPU 型号",
        socket: "插槽",
        processor: {
          id: "处理器 ID",
        },
      },
      usage: {
        used: "已用",
        idle: "空闲",
      },
      memory: {
        total: {
          physical: "物理内存总量",
        },
        used: {
          physical: "已用物理内存",
        },
        available: "可用",
        usage: {
          percent: "内存使用率",
        },
        type: {
          physical: "物理内存",
          virtual: "虚拟内存",
        },
        bank: {
          label: "插槽",
        },
        manufacturer: "制造商",
        capacity: "容量",
        speed: "频率",
        part: {
          number: "部件号",
        },
        serial: {
          number: "序列号",
        },
      },
      gpu: {
        name: "名称",
        manufacturer: "制造商",
        adapter: {
          ram: "显存",
        },
        driver: {
          version: "驱动版本",
        },
      },
      drive: {
        name: "驱动器",
        type: "类型",
        file: {
          system: "文件系统",
        },
        total: {
          size: "总容量",
        },
        free: {
          space: "可用空间",
        },
        used: {
          space: "已用空间",
        },
        usage: {
          percent: "使用率",
        },
      },
      network: {
        name: "名称",
        description: "描述",
        mac: {
          address: "MAC 地址",
        },
        ip: {
          address: "IP 地址",
        },
        speed: "速度",
        status: {
          online: "已联网",
          no: {
            internet: "无外网",
          },
          dns: {
            fault: "DNS 异常",
          },
          up: "已连接",
          down: "已断开",
          enabled: "已启用",
          disabled: "已禁用",
          unknown: "未知",
        },
      },
    },
    unit: {
      core: "核",
      thread: "线程",
      day: "天",
      hour: "小时",
      minute: "分钟",
    },
    button: {
      refresh: {
        cache: "刷新硬件缓存",
      },
    },
    message: {
      load: {
        fail: "加载监控数据失败",
      },
      refresh: {
        success: "硬件信息缓存已刷新",
      },
    },
  },
};
