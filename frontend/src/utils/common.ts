// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/utils
// 文件名称：common.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：通用运行时枚举、常量（与后端 Shared 对齐；Pinia Store 等统一引用）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * API 结果代码（与后端 Takt.Shared.Enums.TaktResultCode 数值一致）
 */
export enum TaktResultCode {
  /** 成功（200 OK） */
  Success = 200,
  /** 错误请求（400） */
  BadRequest = 400,
  /** 未授权（401） */
  Unauthorized = 401,
  /** 禁止访问（403） */
  Forbidden = 403,
  /** 未找到（404） */
  NotFound = 404,
  /** 服务器内部错误（500） */
  InternalServerError = 500,
  /** 请求超时（408） */
  RequestTimeout = 408,
  /** 请求过于频繁（429） */
  TooManyRequests = 429,
  /** 租户不存在 */
  TenantNotFound = 1001,
  /** 公司不存在 */
  CompanyNotFound = 1002,
  /** 用户不存在 */
  UserNotFound = 1003,
  /** 用户名或密码错误 */
  InvalidCredentials = 1004,
  /** 用户已锁定 */
  UserLocked = 1005,
  /** 账号已禁用 */
  AccountDisabled = 1006,
  /** 数据不存在 */
  DataNotFound = 1007,
  /** 数据已存在 */
  DataAlreadyExists = 1008,
  /** 操作失败 */
  OperationFailed = 1009,
  /** 权限不足 */
  InsufficientPermission = 1010,
  /** 验证码错误 */
  InvalidCaptcha = 1011,
  /** 验证码已过期 */
  CaptchaExpired = 1012,
}

/**
 * Remix 图标 CSS 类（与 global.css @remixicon/vue 尺寸约定一致）
 */
export const TAKT_REMIX_ICON_CLASS = 'takt-remix-icon' as const;

/** Remix 图标 12px */
export const TAKT_REMIX_ICON_SM_CLASS = 'takt-remix-icon-sm' as const;

/** Remix 图标 24px */
export const TAKT_REMIX_ICON_LG_CLASS = 'takt-remix-icon-lg' as const;

/** Remix 图标 32px */
export const TAKT_REMIX_ICON_XL_CLASS = 'takt-remix-icon-xl' as const;

/**
 * 菜单类型（字典 sys_menu_type：0=目录，1=菜单，2=按钮）
 */
export enum TaktMenuType {
  /** 目录（侧栏分组） */
  Directory = 0,
  /** 页面菜单 */
  Menu = 1,
  /** 按钮权限 */
  Button = 2,
}

/**
 * 数据权限范围（与后端 Takt.Shared.Enums.TaktDataScope 一致）
 */
export enum TaktDataScope {
  /** 全部数据 */
  All = 1,
  /** 本公司数据 */
  Company = 2,
  /** 本部门数据 */
  Department = 3,
  /** 仅本人数据 */
  Self = 4,
  /** 自定义（部门范围见 TaktRoleDept 关联表） */
  Custom = 5,
}

/**
 * 已读状态（与后端 TaktReadStatus 一致；消息、新闻、邮件等通用）
 */
export enum TaktReadStatus {
  /** 未读 */
  Unread = 0,
  /** 已读 */
  Read = 1,
}

/** 菜单启用状态（与后端 TaktCommonStatus.Enabled = 1 一致） */
export const TAKT_MENU_STATUS_ENABLED = 1;

/** 菜单可见（与后端可见标志 1 一致） */
export const TAKT_MENU_VISIBLE_YES = 1;

/** 内联侧栏/混合菜单每层级缩进（px），对应 a-menu inlineIndent */
export const TAKT_MENU_INLINE_INDENT = 4;

/** OAuth 访问令牌 localStorage 键 */
export const TAKT_ACCESS_TOKEN_STORAGE_KEY = 'takt.access_token';

/** OAuth 刷新令牌 localStorage 键 */
export const TAKT_REFRESH_TOKEN_STORAGE_KEY = 'takt.refresh_token';

/** 访问令牌过期时间 localStorage 键 */
export const TAKT_TOKEN_EXPIRES_STORAGE_KEY = 'takt.token_expires_at';

/** 空闲会话上次真实活动时间 sessionStorage 键（毫秒时间戳） */
export const TAKT_IDLE_LAST_ACTIVITY_STORAGE_KEY = 'takt.idle.last_activity_at';

/** 当前租户编码 localStorage 键 */
export const TAKT_TENANT_CODE_STORAGE_KEY = 'tenantCode';

/** 当前公司编码 localStorage 键 */
export const TAKT_COMPANY_CODE_STORAGE_KEY = 'companyCode';

/** 当前会话内用户手动选择的公司编码 sessionStorage 键（刷新页时恢复；登出/OAuth 前清除） */
export const TAKT_COMPANY_USER_PICKED_STORAGE_KEY = 'takt.company.user_picked';

/** OAuth 跳转前暂存租户编码 localStorage 键 */
export const TAKT_OAUTH_PENDING_TENANT_STORAGE_KEY = 'takt.oauth.tenant_code';

/** 工作台快捷方式路径列表 localStorage 键 */
export const TAKT_WORKSPACE_SHORTCUT_STORAGE_KEY = 'takt-workspace-shortcuts';

/** 工作台快捷方式数量上限（默认两行 × 每行 8 个） */
export const TAKT_WORKSPACE_MAX_SHORTCUTS = 16;

/** 用户语言偏好 localStorage 键 */
export const TAKT_LOCALE_STORAGE_KEY = 'locale';

/** 系统默认语言（与后端 Localization:DefaultCulture 对齐） */
export const TAKT_DEFAULT_LOCALE = 'en-US';

/** 主题模式 localStorage 键 */
export const TAKT_THEME_STORAGE_KEY = 'takt.theme.mode';

/** 主题色预设 localStorage 键 */
export const TAKT_THEME_COLOR_STORAGE_KEY = 'takt.theme.color.preset';

/** 登录表单位置 localStorage 键 */
export const TAKT_LOGIN_LAYOUT_STORAGE_KEY = 'takt.login.layout.position';

/** 租户流水位数（与 Database:TenantCodes 一致） */
export const TAKT_TENANT_CODE_LENGTH = 3;

/** 登录租户远程校验防抖（毫秒） */
export const TAKT_LOGIN_TENANT_VALIDATE_DEBOUNCE_MS = 300;

/** 登录预览防抖（毫秒） */
export const TAKT_LOGIN_PREVIEW_DEBOUNCE_MS = 300;

/** 空闲自动登出默认超时时长（分钟） */
export const TAKT_AUTH_IDLE_DEFAULT_TIMEOUT_MINUTES = 30;

/** 空闲登出预警默认时长（分钟；到期前弹窗，用户可点「继续使用」续期） */
export const TAKT_AUTH_IDLE_DEFAULT_WARNING_MINUTES = 5;

/**
 * 空闲会话视为用户活动的 DOM 事件
 * @description 仅显式交互；不含 mousemove / wheel / pointerdown（触控板与指针微动易误重置）
 */
export const TAKT_IDLE_ACTIVITY_EVENTS = [
  'keydown',
  'click',
  'touchstart',
] as const;

/** 空闲活动节流间隔（毫秒） */
export const TAKT_IDLE_ACTIVITY_THROTTLE_MS = 1000;

/** 行为验证码滑轨设计高度（像素） */
export const TAKT_CAPTCHA_BEHAVIOR_TRACK_HEIGHT = 40;

/** 滑块拼图验证码滑轨高度（像素） */
export const TAKT_CAPTCHA_SLIDER_TRACK_HEIGHT = 40;

/** 行为验证码拖到目标附近允许误差（百分比） */
export const TAKT_CAPTCHA_POSITION_TOLERANCE = 5;

/** 后端「验证码未启用」资源键及常见本地化文案片段 */
export const TAKT_CAPTCHA_DISABLED_HINTS = [
  'common.system.feature.disabled',
  '验证码未启用',
  'Captcha is not enabled',
  '認証コードは有効になっていません',
] as const;

/** Vite 开发服务器需打印的代理路径前缀 */
export const TAKT_DEV_LOG_PATH_PREFIXES = ['/api', '/connect', '/hubs'] as const;
