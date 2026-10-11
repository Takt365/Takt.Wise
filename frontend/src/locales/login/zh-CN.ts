// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/login
// 文件名称：zh-CN.ts
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
      code: "租户",
    },
    company: {
      code: "公司",
    },
    forgot: {
      password: "忘记密码？",
      title: "忘记密码",
      subtitle: "请输入邮箱地址，我们将向您发送密码重置链接",
      submit: "重置密码",
      backtologin: "返回登录",
      fail: "发送重置邮件失败，请检查邮箱地址",
      emailnotregistered: "该邮箱地址未注册，请确认！",
      email: {
        sent: "重置邮件已发送，请查收邮箱",
      },
      reset: {
        unavailable: "该账号不支持自助重置密码，请联系管理员",
      },
      steps: {
        email: "输入邮箱",
        captcha: "安全验证",
        done: "发送完成",
      },
    },
    field: {
      tenant: {
        code: {
          placeholder: "请输入租户编码",
        },
      },
      userName: {
        label: "用户名",
        placeholder: "请输入用户名",
        or: {
          email: {
            label: "用户名或邮箱",
            placeholder: "请输入用户名或邮箱",
          },
        },
      },
      password: {
        label: "密码",
        placeholder: "请输入密码",
      },
      culture: {
        label: "语言",
        placeholder: "请先输入有效租户编码",
      },
      email: {
        label: "邮箱",
        placeholder: "请输入邮箱",
      },
      phone: {
        label: "手机号",
        placeholder: "请输入手机号",
      },
    },
    validate: {
      tenant: {
        required: "请输入租户编码",
        invalid: "租户编码须为 3 位数字",
      },
      userName: {
        required: "请输入用户名",
        invalid: "用户名格式不正确",
        or: {
          email: {
            required: "请输入用户名或邮箱",
            invalid: "用户名或邮箱格式不正确",
            too: {
              short: "用户名或邮箱长度不能少于 {min} 个字符",
              long: "用户名或邮箱长度不能超过 {max} 个字符",
            },
          },
        },
      },
      password: {
        required: "请输入密码",
        weak: "密码强度不足",
      },
      email: {
        required: "请输入邮箱",
        invalid: "邮箱格式不正确",
        too: {
          short: "邮箱长度不能少于 {min} 个字符",
          long: "邮箱长度不能超过 {max} 个字符",
        },
      },
      phone: {
        required: "请输入手机号",
        invalid: "手机号格式不正确",
      },
      captcha: {
        required: "请完成安全验证",
      },
    },
    login: {
      title: "欢迎",
      rememberme: "记住我",
      login: "登录",
      logout: "退出登录",
      noaccountregister: "初识节拍365？{register}",
    },
    sign: {
      title: "注册",
      subtitle: "创建您的账号以开始使用",
      hasaccount: "已有账号？去登录",
      success: "注册成功，请登录",
      successinitialpassword: "注册成功，初始密码请查收邮件",
      fail: "注册失败",
      steps: {
        info: "填写资料",
        captcha: "安全验证",
        done: "完成注册",
      },
    },
    message: {
      fail: "登录失败",
      credentials: {
        incorrect: "用户名或密码错误",
      },
      account: {
        locked: "账户已锁定，请稍后再试",
      },
      tenant: {
        no: {
          access: "当前用户无权登录所选租户",
        },
        not: {
          found: "租户不存在",
        },
        validate: {
          fail: "租户校验失败，请稍后重试",
        },
        database: {
          missing: "租户 {tenantCode} 的业务数据库不存在（库名：{databaseName}）。请将 Init.InitDb 设为 true 后重启后端建库。",
          login: {
            failed: "租户 {tenantCode} 无法登录 SQL Server（库名：{databaseName}）。请检查连接字符串中的服务器与 sa 密码。",
          },
        },
        table: {
          missing: "租户 {tenantCode} 的业务数据库 {databaseName} 已连接，但缺少业务数据表。请开启 Init.InitDb 建表并视需要 SeedData。",
        },
        options: {
          fail: "加载租户列表失败，请稍后重试",
        },
      },
      user: {
        not: {
          found: "用户不存在或已禁用",
        },
      },
      default: {
        company: {
          not: {
            found: "未配置默认登录公司",
          },
        },
      },
      public: {
        key: {
          missing: "无法获取登录加密公钥，请刷新后重试",
        },
      },
      encrypt: {
        fail: "密码加密失败，请重试",
      },
    },
    log: {
      sign: {
        in: {
          session: {
            start: "开始建立 Cookie 会话",
            success: "Cookie 会话已建立，准备 OAuth 授权",
          },
        },
      },
      verify: {
        password: {
          start: "开始校验密码",
          success: "密码校验通过",
          captcha: {
            required: "密码校验通过，需验证码",
          },
        },
      },
    },
    captcha: {
      title: "安全验证",
      drag: {
        hint: "拖动滑块完成拼图",
      },
      behavior: {
        hint: "拖动滑块至正确位置",
      },
      slide: {
        to: {
          target: "请拖动滑块至目标位置 {position}%",
        },
      },
      success: "验证通过",
      cancel: "取消",
      confirm: "确认",
      typerequired: "服务端未返回验证码类型，请检查后端配置",
    },
    callback: {
      title: "登录回调",
      processing: "正在完成登录...",
      missing: {
        code: "缺少授权码",
      },
      state: {
        mismatch: "state 校验失败",
      },
      pkce: {
        missing: "PKCE 校验参数丢失",
      },
      fail: "登录失败",
      log: {
        oauth: {
          error: "授权回调错误：{description}",
        },
        exchange: {
          start: "开始用授权码换取令牌",
          success: "授权码换令牌成功，用户资料已加载",
        },
        rbac: {
          ready: "OAuth 回调后 RBAC 已就绪",
        },
      },
    },
    showcase: {
      split: {
        practical: "实用",
        simple: "简洁",
        flexible: "灵活",
      },
    },
  },
};
