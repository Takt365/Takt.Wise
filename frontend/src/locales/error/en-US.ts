// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/error
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：error page static copy; keys error.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title404: "404",
    title403: "403",
    common: {
      reload: "Reload page",
      goback: "Go back",
      gologin: "Sign in",
      gohome: "Back to Home",
    },
    notfound: {
      title: "Page not found",
      subtitle: "Sorry, the page you visited does not exist",
    },
    forbidden: {
      title: "Access denied",
      subtitle: "Sorry, you are not authorized to access this page",
    },
    unauthorized: {
      title: "Unauthorized",
      subtitle: "Your session has expired or you are not signed in. Please sign in again.",
    },
    servererror: {
      title: "Server error",
      subtitle: "Internal server error. Please try again later.",
    },
    serviceunavailable: {
      title: "Service unavailable",
      subtitle: "Service temporarily unavailable. Please try again later.",
    },
    comingsoon: {
      title: "Coming soon",
      subtitle: "This feature is coming soon. Please stay tuned.",
    },
    http: {
      network: "Cannot connect to the backend. Please ensure the API is running.",
      timeout: "Request timed out. Please try again later.",
      config: "Request configuration error",
      default: {
        with: {
          status: "Request failed (HTTP {status})",
        },
      },
      login: {
        failed: "Sign-in failed. Check your account, password, and tenant.",
      },
      tenant: {
        database: {
          hint: "Tenant business database is unavailable. Create the database or run InitDb.",
        },
      },
      status: {
        '400': "Invalid request parameters",
        '401': "Your session has expired. Please sign in again.",
        '403': "You do not have permission to access this resource",
        '404': "The requested resource was not found",
        '405': "HTTP method not allowed",
        '408': "Request timed out. Please try again later.",
        '409': "Data conflict. Please refresh the page and try again.",
        '413': "Request payload too large",
        '415': "Unsupported media type",
        '422': "The request could not be processed. Please check your input.",
        '429': "Too many requests. Please try again later.",
        '500': "Internal server error. Please try again later.",
        '501': "Not implemented on the server",
        '502': "Bad gateway. Cannot connect to the backend. Please ensure the API is running.",
        '503': "Service temporarily unavailable. Please try again later.",
        '504': "Gateway timeout. Please try again later or ensure the API is running.",
      },
    },
  },
};
