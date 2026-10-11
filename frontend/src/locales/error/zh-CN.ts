// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/error
// 文件名称：zh-CN.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：error 页面静态文案；引用键 error.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title404: "404",
    title403: "403",
    common: {
      reload: "刷新页面",
      goback: "返回上一页",
      gologin: "前往登录",
      gohome: "返回首页",
    },
    notfound: {
      title: "页面不存在",
      subtitle: "抱歉，您访问的页面不存在",
    },
    forbidden: {
      title: "无权限访问",
      subtitle: "抱歉，您没有权限访问此页面",
    },
    unauthorized: {
      title: "未授权",
      subtitle: "登录已过期或未授权，请重新登录",
    },
    servererror: {
      title: "服务器错误",
      subtitle: "服务器内部错误，请稍后重试",
    },
    serviceunavailable: {
      title: "服务不可用",
      subtitle: "服务暂不可用，请稍后重试",
    },
    comingsoon: {
      title: "即将推出",
      subtitle: "功能即将推出，敬请期待",
    },
    http: {
      network: "无法连接后端服务，请确认 API 已启动",
      timeout: "请求超时，请稍后重试",
      config: "请求配置错误",
      default: {
        with: {
          status: "请求失败（HTTP {status}）",
        },
      },
      login: {
        failed: "登录失败，请检查账号、密码与租户",
      },
      tenant: {
        database: {
          hint: "租户业务库不可用，请检查数据库是否已创建或联系管理员执行 InitDb。",
        },
      },
      status: {
        '400': "请求参数错误",
        '401': "登录已过期，请重新登录",
        '403': "无权限访问该资源",
        '404': "请求的资源不存在",
        '405': "请求方法不允许",
        '408': "请求超时，请稍后重试",
        '409': "数据冲突，请刷新页面后重试",
        '413': "请求数据过大",
        '415': "不支持的媒体类型",
        '422': "请求无法处理，请检查输入内容",
        '429': "请求过于频繁，请稍后再试",
        '500': "服务器内部错误，请稍后重试",
        '501': "服务器未实现该功能",
        '502': "网关错误，无法连接后端服务，请确认 API 已启动",
        '503': "服务暂不可用，请稍后重试",
        '504': "网关超时，请稍后重试或确认 API 已启动",
      },
    },
  },
};
