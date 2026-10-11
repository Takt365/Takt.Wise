// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types
// 文件名称：virtual-modules.d.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：Vite 虚拟模块类型声明
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

declare module 'virtual:app-info' {
  const appInfo: {
    pkg: {
      name: string;
      version: string;
      dependencies: Record<string, string>;
      devDependencies: Record<string, string>;
    };
    lastBuildTime: string;
  };

  export default appInfo;
}

declare module '@form-create/antd-designer/locale/zh-cn.js' {
  const locale: Record<string, unknown>;
  export default locale;
}

declare module '@form-create/antd-designer/locale/en.js' {
  const locale: Record<string, unknown>;
  export default locale;
}

declare module '@form-create/antd-designer' {
  import type { Plugin } from 'vue';

  const FcDesigner: Plugin & { formCreate: Plugin };
  export default FcDesigner;
}
