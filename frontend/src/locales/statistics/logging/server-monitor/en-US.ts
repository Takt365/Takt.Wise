// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/statistics/logging/server-monitor
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：statistics/logging/server-monitor page static copy; keys statistics.logging.server-monitor.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: "Server Monitor",
    description: "Application runtime status and server hardware (CPU, memory, GPU, disks, network)",
    tabs: {
      app: "Application",
      system: "System",
      cpu: "CPU",
      memory: "Memory",
      gpu: "GPU",
      drive: "Disks",
      network: "Network",
    },
    section: {
      app: {
        status: "Application Status",
      },
      os: {
        language: "OS & Language",
      },
      motherboard: "Motherboard",
      cpu: "CPU",
      memory: "Memory",
      gpu: "Graphics",
      drive: "Disks",
      network: "Network Adapters",
    },
    field: {
      application: {
        name: "Application",
        version: "Version",
      },
      environment: "Environment",
      machine: {
        name: "Machine name",
      },
      dot: {
        net: {
          version: ".NET version",
        },
      },
      process: {
        architecture: "Process architecture",
      },
      processor: {
        count: "Processor count",
      },
      start: {
        time: "Start time",
      },
      uptime: "Uptime",
      working: {
        set: "Working set",
      },
      operating: {
        system: "Operating system",
      },
      os: {
        version: "OS version",
      },
      current: {
        culture: "Current culture",
        ui: {
          culture: "Current UI culture",
        },
      },
      system: {
        type: "OS architecture",
        type32: "32-bit",
        type64: "64-bit",
      },
      motherboard: {
        manufacturer: "Board manufacturer",
        product: "Board model / ID",
        serial: {
          number: "Board serial number",
        },
        version: "Board version",
        uuid: "Machine UUID",
      },
      cpu: {
        name: "Name",
        manufacturer: "Manufacturer",
        cores: "Cores",
        logical: {
          processors: "Logical processors",
          core: {
            name: "Logical core",
          },
        },
        usage: {
          percent: "CPU usage",
        },
        model: "CPU model",
        socket: "Socket",
        processor: {
          id: "Processor ID",
        },
      },
      usage: {
        used: "Used",
        idle: "Idle",
      },
      memory: {
        total: {
          physical: "Total physical memory",
        },
        used: {
          physical: "Used physical memory",
        },
        available: "Available",
        usage: {
          percent: "Memory usage",
        },
        type: {
          physical: "Physical memory",
          virtual: "Virtual memory",
        },
        bank: {
          label: "Bank",
        },
        manufacturer: "Manufacturer",
        capacity: "Capacity",
        speed: "Speed",
        part: {
          number: "Part number",
        },
        serial: {
          number: "Serial number",
        },
      },
      gpu: {
        name: "Name",
        manufacturer: "Manufacturer",
        adapter: {
          ram: "Video memory",
        },
        driver: {
          version: "Driver version",
        },
      },
      drive: {
        name: "Drive",
        type: "Type",
        file: {
          system: "File system",
        },
        total: {
          size: "Total size",
        },
        free: {
          space: "Free space",
        },
        used: {
          space: "Used space",
        },
        usage: {
          percent: "Usage",
        },
      },
      network: {
        name: "Name",
        description: "Description",
        mac: {
          address: "MAC address",
        },
        ip: {
          address: "IP address",
        },
        speed: "Speed",
        status: {
          online: "Online",
          no: {
            internet: "No internet",
          },
          dns: {
            fault: "DNS fault",
          },
          up: "Connected",
          down: "Disconnected",
          enabled: "Enabled",
          disabled: "Disabled",
          unknown: "Unknown",
        },
      },
    },
    unit: {
      core: "cores",
      thread: "threads",
      day: "d",
      hour: "h",
      minute: "m",
    },
    button: {
      refresh: {
        cache: "Refresh hardware cache",
      },
    },
    message: {
      load: {
        fail: "Failed to load monitor data",
      },
      refresh: {
        success: "Hardware cache refreshed",
      },
    },
  },
};
