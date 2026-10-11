// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/error
// 文件名称：zh-HK.ts
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
      reload: "刷新頁麵",
      goback: "返回上一頁",
      gologin: "前往登錄",
      gohome: "返回首頁",
    },
    notfound: {
      title: "頁麵不存在",
      subtitle: "抱歉，您訪問的頁麵不存在",
    },
    forbidden: {
      title: "無權限訪問",
      subtitle: "抱歉，您沒有權限訪問此頁麵",
    },
    unauthorized: {
      title: "未授權",
      subtitle: "登錄已過期或未授權，請重新登錄",
    },
    servererror: {
      title: "服務器錯誤",
      subtitle: "服務器內部錯誤，請稍後重試",
    },
    serviceunavailable: {
      title: "服務不可用",
      subtitle: "服務暫不可用，請稍後重試",
    },
    comingsoon: {
      title: "即將推出",
      subtitle: "功能即將推出，敬請期待",
    },
    http: {
      network: "無法連接後端服務，請確認 API 已啓動",
      timeout: "請求超時，請稍後重試",
      config: "請求配置錯誤",
      default: {
        with: {
          status: "請求失敗（HTTP {status}）",
        },
      },
      login: {
        failed: "登錄失敗，請檢查賬號、密碼與租戶",
      },
      tenant: {
        database: {
          hint: "租戶業務庫不可用，請檢查資料庫是否已建立或聯繫管理員執行 InitDb。",
        },
      },
      status: {
        '400': "請求參數錯誤",
        '401': "登錄已過期，請重新登錄",
        '403': "無權限訪問該資源",
        '404': "請求的資源不存在",
        '405': "請求方法不允許",
        '408': "請求超時，請稍後重試",
        '409': "數據衝突，請刷新頁麵後重試",
        '413': "請求數據過大",
        '415': "不支持的媒體類型",
        '422': "請求無法處理，請檢查輸入內容",
        '429': "請求過於頻繁，請稍後再試",
        '500': "服務器內部錯誤，請稍後重試",
        '501': "服務器未實現該功能",
        '502': "網關錯誤，無法連接後端服務，請確認 API 已啓動",
        '503': "服務暫不可用，請稍後重試",
        '504': "網關超時，請稍後重試或確認 API 已啓動",
      },
    },
  },
};
