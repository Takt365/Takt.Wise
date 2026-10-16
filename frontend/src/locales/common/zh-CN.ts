// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/common
// 文件名称：zh-CN.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：common 页面静态文案；引用键 common.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    app: {
      title: "节拍智控",
      name: "节拍智控·TaktWise",
      productcode: "TP-MES-PRO",
      slogan: "让产线，踩准客户的节拍。",//"节拍驱动智造",
      tagline: "算节拍 · 看平衡 · 调产线",//"实用 · 简洁 · 灵活",
    },
    api: {
      connect: {
        fail: "无法连接服务器",
        description: "请检查网络连接或稍后重试；若持续失败请联系管理员。",
      },
    },
    signalr: {
      connect: {
        fail: "实时消息连接失败",
      },
      online: {
        notify: "实时消息连接已恢复",
      },
      new: {
        message: "新消息",
      },
    },
    theme: {
      switch: {
        label: "切换主题",
        to: {
          light: "切换为浅色模式",
          dark: "切换为深色模式",
        },
      },
      light: "浅色",
      dark: "深色",
      system: "跟随系统",
    },
    locale: {
      switch: "切换语言",
    },
    tenant: {
      switch: "切换租户",
    },
    company: {
      switch: "切换公司",
    },
    color: {
      title: "主题色",
      switch: "切换主题色",
      custom: "自定义",
      mars: {
        green: "马尔斯绿",
      },
      tiffany: {
        blue: "蒂芙尼蓝",
      },
      chinese: {
        red: "中国红",
      },
      titian: {
        red: "提香红",
      },
      burgundy: {
        red: "勃艮第酒红",
      },
      bordeaux: {
        red: "波尔多红",
      },
      klein: {
        blue: "克莱因蓝",
      },
      vandyke: {
        brown: "凡戴克棕",
      },
      prussian: {
        blue: "普鲁士蓝",
      },
      sennelier: {
        yellow: "申布伦黄",
      },
      memorial: {
        gray: "纪念灰",
      },
    },
    layout: {
      switch: "切换登录布局",
      position: {
        left: "左对齐",
        center: "居中",
        right: "右对齐",
      },
    },
    entity: {
      culturelist: "语言列表",
      menulist: "菜单",
      tenantlist: "租户列表",
    },
    button: {
      ok: "确定",
      cancel: "取消",
      logout: "退出登录",
      profile: "个人中心",
      checkall: "全选",
    },
    icon: {
      picker: {
        title: "选择图标",
        placeholder: "请选择图标",
        search: "搜索图标名称（如 grid、user）",
        selected: "已选：",
        clear: "清空选择",
        empty: "无匹配图标",
        total: "共 {total} 个",
        variant: {
          line: "线性",
          fill: "填充",
          all: "全部",
        },
      },
    },
    form: {
      numberingRule: "编码规则",
      numberingCodePreview: "选择编码规则后自动生成",
      numberingRuleRequired: "请选择编码规则",
      periodRangeMaxMonths: "查询期间不能超过 {max} 个月",
    },
  },
};
