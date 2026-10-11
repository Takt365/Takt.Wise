// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types
// 文件名称：umoEditor.types.ts
// 创建时间：2026-08-24
// 创建人：Takt365(Cursor AI)
// 功能描述：@umoteam/editor 模块声明（官方包未导出完整 d.ts）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

declare module '@umoteam/editor' {
  import type { DefineComponent, Plugin } from 'vue';

  /** Umo Editor 组件实例方法（常用子集，与官方 expose 对齐） */
  export interface UmoEditorInstance {
    getHTML: () => string;
    setContent: (content: string, options?: { emitUpdate?: boolean }) => void;
    setReadOnly: (readonly: boolean) => void;
    setLocale: (locale: string) => void;
    setTheme: (theme: string) => void;
    setToolbar?: (toolbar: { mode?: string; show?: boolean }) => void;
  }

  export const UmoEditor: DefineComponent<Record<string, unknown>>;
  export const useUmoEditor: Plugin;
}

declare module '@umoteam/editor/style';
