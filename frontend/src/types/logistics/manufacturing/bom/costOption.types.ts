// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/logistics/manufacturing/bom
// 文件名称：costOption.types.ts
// 创建时间：2026-08-21
// 创建人：Takt365(Cursor AI)
// 功能描述：BOM 成本查询栏共用选项（对齐 TaktBomCostOptionDto / TaktBomCostOptions）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * BOM 成本查询栏选项查询（工厂 + 期间；物料类型/机种/关键字按接口选用）
 */
export interface BomCostOptionQuery {
  /** 工厂代码（必填） */
  plantCode: string
  /** 期间起 yyyy-MM（必填；单月时与止相同） */
  periodStart?: string
  /** 期间止 yyyy-MM（可空=与起相同） */
  periodEnd?: string
  /** 物料类型（本表 MaterialType；空=不过滤） */
  materialType?: string
  /** 机种编码（产品/物料选项可空过滤；空=不过滤） */
  modelCode?: string
  /** 机种编码多选（逗号分隔；物料选项可空过滤；空=不过滤） */
  modelCodes?: string
  /** 产品编码（物料选项可空过滤；空=不过滤） */
  productCode?: string
  /** 远程搜索关键字（物料选项） */
  keyword?: string
}
