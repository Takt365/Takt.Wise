// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types
// 文件名称：setting.types.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：全局应用偏好设置类型（布局、主题、导航、页签等）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 布局模式
 */
export type LayoutMode = 'side' | 'top' | 'mix' | 'content';

/**
 * 主题模式（应用偏好，不含 system）
 */
export type ThemeMode = 'light' | 'dark';

/**
 * 主题色预设键（AppSetting 短键；色值权威源见 utils/theme.ts）
 */
export type ThemeColor =
  | 'green'
  | 'cyan'
  | 'red'
  | 'orange'
  | 'purple'
  | 'pink'
  | 'blue'
  | 'brown'
  | 'indigo'
  | 'yellow'
  | 'gray'
  | 'custom';

/**
 * 主题色配置
 */
export interface ThemeColorConfig {
  /**
   * 预设类型或自定义
   */
  type: ThemeColor;

  /**
   * 自定义色值（type 为 custom 时有效）
   */
  customColor?: string;
}

/**
 * 页签样式
 */
export type TabStyle = 'card' | 'google';

/**
 * 菜单样式
 */
export type MenuStyle = 'plain' | 'rounded';

/**
 * 内容区宽度模式
 */
export type ContentWidthMode = 'fluid' | 'fixed';

/**
 * 全局应用设置
 */
export interface AppSetting {
  /**
   * 布局模式
   */
  layout: LayoutMode;

  /**
   * 浅色 / 深色主题
   */
  theme: ThemeMode;

  /**
   * 主题色
   */
  themeColor: ThemeColorConfig;

  /**
   * 用户是否手动改过主题明暗或主色；为 true 时假日主题色不覆盖
   */
  appearanceUserOverride?: boolean;

  /**
   * 圆角（px）
   */
  borderRadius: number;

  /**
   * 基础字号（px，15–22）
   */
  fontSize: number;

  /**
   * 色弱模式
   */
  colorWeak: boolean;

  /**
   * 灰色模式
   */
  grayscale: boolean;

  /**
   * 固定顶栏
   */
  fixedHeader: boolean;

  /**
   * 固定侧栏
   */
  fixedSider: boolean;

  /**
   * 显示 Logo
   */
  showLogo: boolean;

  /**
   * 侧栏展开宽度（px）
   */
  siderWidth: number;

  /**
   * 侧栏折叠宽度（px）
   */
  siderCollapsedWidth: number;

  /**
   * 显示面包屑
   */
  showBreadcrumb: boolean;

  /**
   * 面包屑显示图标
   */
  breadcrumbIcon: boolean;

  /**
   * 显示多页签栏
   */
  showTabs: boolean;

  /**
   * 页签样式
   */
  tabStyle: TabStyle;

  /**
   * 刷新后恢复页签
   */
  persistTabs: boolean;

  /**
   * 最大页签数量
   */
  maxTabs: number;

  /**
   * 显示页脚
   */
  showFooter: boolean;

  /**
   * 页脚版权文案
   */
  copyright: string;

  /**
   * 内容区宽度
   */
  contentWidth: ContentWidthMode;

  /**
   * 启用多页签
   */
  multiTab: boolean;

  /**
   * 显示水印
   */
  watermark: boolean;

  /**
   * 水印文案
   */
  watermarkContent: string;

  /**
   * 演示模式（true=全站只读并强制启用水印，禁止增删改/导入等写操作；false=正常）
   */
  demo: boolean;

  /**
   * 手风琴菜单
   */
  menuAccordion: boolean;

  /**
   * 菜单样式
   */
  menuStyle: MenuStyle;

  /**
   * Logo 资源路径（@/ 或相对 src）
   */
  logo: string;

  /**
   * Logo 旁标题
   */
  logoText: string;

  /**
   * 侧栏折叠时 Logo 短标题
   */
  logoCollapsedText: string;

  /**
   * 登录页显示忘记密码
   */
  showForgotPassword: boolean;

  /**
   * 登录页显示注册
   */
  showRegister: boolean;
}
