// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/login
// 文件名称：zh-HK.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：login 页面静态文案；引用键 login.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    tenant: {
      code: "租戶",
    },
    company: {
      code: "公司",
    },
    forgot: {
      password: "忘記密碼？",
      title: "忘記密碼",
      subtitle: "請輸入郵箱地址，我們將向您發送密碼重置鏈接",
      submit: "重置密碼",
      backtologin: "返回登錄",
      fail: "發送重置郵件失敗，請檢查郵箱地址",
      emailnotregistered: "該郵箱地址未註冊，請確認！",
      email: {
        sent: "重置郵件已發送，請查收郵箱",
      },
      reset: {
        unavailable: "該賬號不支持自助重置密碼，請聯繫管理員",
      },
      steps: {
        email: "輸入郵箱",
        captcha: "安全驗證",
        done: "發送完成",
      },
    },
    field: {
      tenant: {
        code: {
          placeholder: "請輸入租戶編碼",
        },
      },
      userName: {
        label: "用戶名",
        placeholder: "請輸入用戶名",
        or: {
          email: {
            label: "用戶名或郵箱",
            placeholder: "請輸入用戶名或郵箱",
          },
        },
      },
      password: {
        label: "密碼",
        placeholder: "請輸入密碼",
      },
      culture: {
        label: "語言",
        placeholder: "請先輸入有效租戶編碼",
      },
      email: {
        label: "郵箱",
        placeholder: "請輸入郵箱",
      },
      phone: {
        label: "手機號",
        placeholder: "請輸入手機號",
      },
    },
    validate: {
      tenant: {
        required: "請輸入租戶編碼",
        invalid: "租戶編碼須為 3 位數字",
      },
      userName: {
        required: "請輸入用戶名",
        invalid: "用戶名格式不正確",
        or: {
          email: {
            required: "請輸入用戶名或郵箱",
            invalid: "用戶名或郵箱格式不正確",
            too: {
              short: "用戶名或郵箱長度不能少於 {min} 個字符",
              long: "用戶名或郵箱長度不能超過 {max} 個字符",
            },
          },
        },
      },
      password: {
        required: "請輸入密碼",
        weak: "密碼強度不足",
      },
      email: {
        required: "請輸入郵箱",
        invalid: "郵箱格式不正確",
        too: {
          short: "郵箱長度不能少於 {min} 個字符",
          long: "郵箱長度不能超過 {max} 個字符",
        },
      },
      phone: {
        required: "請輸入手機號",
        invalid: "手機號格式不正確",
      },
      captcha: {
        required: "請完成安全驗證",
      },
    },
    login: {
      title: "歡迎",
      rememberme: "記住我",
      login: "登錄",
      logout: "退出登錄",
      noaccountregister: "初識節拍365？{register}",
    },
    sign: {
      title: "註冊",
      subtitle: "創建您的賬號以開始使用",
      hasaccount: "已有賬號？去登錄",
      success: "註冊成功，請登錄",
      successinitialpassword: "註冊成功，初始密碼請查收郵件",
      fail: "註冊失敗",
      steps: {
        info: "填寫資料",
        captcha: "安全驗證",
        done: "完成註冊",
      },
    },
    message: {
      fail: "登錄失敗",
      credentials: {
        incorrect: "用戶名或密碼錯誤",
      },
      account: {
        locked: "賬戶已鎖定，請稍後再試",
      },
      tenant: {
        no: {
          access: "當前用戶無權登錄所選租戶",
        },
        not: {
          found: "租戶不存在",
        },
        validate: {
          fail: "租戶校驗失敗，請稍後重試",
        },
        database: {
          missing: "租戶 {tenantCode} 的業務資料庫不存在（庫名：{databaseName}）。請將 Init.InitDb 設為 true 後重啟後端建庫。",
          login: {
            failed: "租戶 {tenantCode} 無法登入 SQL Server（庫名：{databaseName}）。請檢查連線字串中的伺服器與 sa 密碼。",
          },
        },
        table: {
          missing: "租戶 {tenantCode} 的資料庫 {databaseName} 已連線，但缺少業務資料表。請開啟 Init.InitDb 建表並視需要 SeedData。",
        },
        options: {
          fail: "加載租戶列表失敗，請稍後重試",
        },
      },
      user: {
        not: {
          found: "用戶不存在或已禁用",
        },
      },
      default: {
        company: {
          not: {
            found: "未配置默認登錄公司",
          },
        },
      },
      public: {
        key: {
          missing: "無法獲取登錄加密公鑰，請刷新後重試",
        },
      },
      encrypt: {
        fail: "密碼加密失敗，請重試",
      },
    },
    log: {
      sign: {
        in: {
          session: {
            start: "開始建立 Cookie 會話",
            success: "Cookie 會話已建立，準備 OAuth 授權",
          },
        },
      },
      verify: {
        password: {
          start: "開始校驗密碼",
          success: "密碼校驗通過",
          captcha: {
            required: "密碼校驗通過，需驗證碼",
          },
        },
      },
    },
    captcha: {
      title: "安全驗證",
      drag: {
        hint: "拖動滑塊完成拼圖",
      },
      behavior: {
        hint: "拖動滑塊至正確位置",
      },
      slide: {
        to: {
          target: "請拖動滑塊至目標位置 {position}%",
        },
      },
      success: "驗證通過",
      cancel: "取消",
      confirm: "確認",
      typerequired: "服務端未返回驗證碼類型，請檢查後端配置",
    },
    callback: {
      title: "登錄回調",
      processing: "正在完成登錄...",
      missing: {
        code: "缺少授權碼",
      },
      state: {
        mismatch: "state 校驗失敗",
      },
      pkce: {
        missing: "PKCE 校驗參數丟失",
      },
      fail: "登錄失敗",
      log: {
        oauth: {
          error: "授權回調錯誤：{description}",
        },
        exchange: {
          start: "開始用授權碼換取令牌",
          success: "授權碼換令牌成功，用戶資料已加載",
        },
        rbac: {
          ready: "OAuth 回調後 RBAC 已就緒",
        },
      },
    },
    showcase: {
      split: {
        practical: "實用",
        simple: "簡潔",
        flexible: "靈活",
      },
    },
  },
};
