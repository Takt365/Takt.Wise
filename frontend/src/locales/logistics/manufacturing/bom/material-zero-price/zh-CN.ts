// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/bom/material-zero-price
// 文件名称：zh-CN.ts
// 创建时间：2026-08-13
// 创建人：Takt365(Cursor AI)
// 功能描述：引用键 logistics.manufacturing.bom.material-zero-price.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    plantCode: '工厂',
    selectPlantRequired: '请选择工厂',
    costingMonth: '核算月',
    costingMonthPlaceholder: '选择核算月',
    costNeedMonth: '请选择核算月',
    modelCode: '机种',
    modelCodesOptional: '机种（可选，可多选；空=全部机种）',
    modelCodePlaceholder: '可选；空=全部机种',
    hint: '{month} · 产品 {productCount} · 零价组件 {componentCount}（仅 FERT · IsDeleted=0 · ProductionRelated=X · PcbSectIndicator 空 · PurchaseType=F · 移动平均价=0，按组件合并产品；建议代替=末尾版本字母逆推如 E02597400C→B→A，取移动价格表同期间或以前最近有价÷PriceUnit）',
    productCodes: '共用产品',
    productCount: '产品数',
    suggestedComponentCode: '建议代替组件',
    suggestedMovingPrice: '建议移动价格',
    exportSuccess: 'BOM零价格导出成功',
    exportFailed: 'BOM零价格导出失败',
    costSum: '计算成本',
    costRecalculate: '重算成本',
    costAverage: '计算平均成本',
    purchaseInfoBackfill: '回填采购价',
    purchaseInfoBackfillSuccess:
      '{month} 回填采购价完成：扫描 {scanned} 行，更新 {updated}，无价格 {skipped}，未变化 {unchanged}',
    purchaseInfoBackfillFailed: '回填采购价失败',
    movingPriceBackfill: '回填移动价格',
    movingPriceBackfillBatch: '批量回填移动价格',
    movingPriceBackfillRow: '回填移动价格',
    movingPriceBackfillNoSuggested: '无建议代替组件，无法回填移动价格',
    movingPriceBackfillConfirmTitle: '确认回填移动价格？',
    movingPriceBackfillConfirmContent:
      '将按当前工厂与核算月，把组件 {component} 的零价明细回填为建议代替 {suggested} 的移动平均价/单位/货币，并写入 ExtField 履历，同时更新各机种产品月成本与机种月成本。',
    movingPriceBackfillBatchConfirmTitle: '确认批量回填移动价格？',
    movingPriceBackfillBatchConfirmContent:
      '将先标记 PCB SECT，再按当前工厂与核算月 {month}（及机种条件）对全部有建议代替的零价组件回填移动平均价/单位/货币（排除 PcbSectIndicator 非空），写入 ExtField 履历，并更新各机种产品月成本与机种月成本。',
    movingPriceBackfillSuccess:
      '{month} 回填移动价格完成：明细扫描 {scanned}、更新 {updated}、无价格 {skipped}、未变化 {unchanged}；产品月成本 {productCost}、机种月成本 {modelAverage}；{priceInfo}',
    movingPriceBackfillBatchSuccess:
      '{month} 批量回填移动价格完成：组件 {components}、明细扫描 {scanned}、更新 {updated}、无价格 {skipped}、未变化 {unchanged}；产品月成本 {productCost}、机种月成本 {modelAverage}',
    movingPriceBackfillFailed: '回填移动价格失败',
    movingPriceManualRow: '手工更新价格',
    movingPriceManualTitle: '手工替换更新移动价格',
    movingPriceManualHint:
      '将新组件的移动价格、价格单位、币种回填到原组件明细（工厂+核算月下该组件全部产品行），并同步更新各机种主表产品月成本与机种月成本，ExtField 记录完整履历。',
    movingPriceManualOriginal: '原组件',
    movingPriceManualReplace: '替换',
    movingPriceManualSourceComponent: '新组件',
    movingPriceManualSourceRequired: '请输入替换新组件编码',
    movingPriceManualPrice: '移动价格',
    movingPriceManualPriceRequired: '请输入大于 0 的移动价格',
    movingPriceManualUnit: '价格单位',
    movingPriceManualCurrency: '币种',
    movingPriceManualSuccess:
      '{month} 手工替换完成：原组件 {component} ← 新组件 {source}，明细扫描 {scanned}、更新 {updated}、未变化 {unchanged}；产品月成本 {productCost}、机种月成本 {modelAverage}；{priceInfo}',
    movingPriceManualFailed: '手工替换更新移动价格失败',
    latestPurchaseCost: '计算最近采购成本',
    latestPurchaseCostSuccess:
      '{month} 计算最近采购成本完成：扫描 {scanned} 行，刷新 {refreshed} 组，跳过 {skipped} 组',
    latestPurchaseCostFailed: '计算最近采购成本失败',
    costSumSubmitted: '已提交 {month} 后台计算成本（全部物料类型），完成后将通知您',
    costRecalculateSubmitted: '已提交 {month} 后台重算（全部物料类型；先归零再汇总），完成后将通知您',
    costRecalculateCompleted: '{month} 处理完成（耗时 {duration}，刷新 {refreshed} 组，跳过 {skipped} 组）',
    costRecalculateFailed: '成本处理失败',
    costRecalculateConfirmTitle: '确认重算成本？',
    costRecalculateConfirmContent: '将把旧成本写入扩展字段后，按该核算月全部物料类型重算成本。',
    costAverageSuccess:
      '{month} 计算平均成本完成：扫描 {scanned} 行（产品月成本>0共 {positiveCostRows}），机种更新 {modelUpdated}，类型更新 {typeUpdated}，月均更新 {averageUpdated}（有成本组 {groupsWithCost}/{groups}，无成本组 {groupsNoCost}）',
    costAverageFailed: '计算平均成本失败',
    pcbSectMark: '标记 PCB SECT',
    pcbSectMarkConfirmTitle: '确认标记 PCB SECT 整树？',
    pcbSectMarkConfirmContent:
      '将按当前工厂与核算月 {month}（及机种条件）识别组件描述含「PCB SECT」的节点及其子层级整树，在明细 pcb_sect_indicator 写入 X（已有标识跳过）。',
    pcbSectMarkSuccess:
      '{month} PCB SECT 打标完成：扫描 {scanned}，整树 {pcbSect}，新标 {updated}，已有 {unchanged}',
    pcbSectMarkFailed: 'PCB SECT 打标失败',
    stripLeadingZeros: '去前导0',
    stripLeadingZerosConfirmTitle: '确认去掉前导 0？',
    stripLeadingZerosConfirmContent:
      '将当前工厂 {plant} 的成本汇总表产品编码，以及明细表（含各年分表）的产品编码、组件编码中，纯数字且带前导 0 的值去掉前导 0。例如 000000001918610130 变为 1918610130。含字母的编码（如 0919MH8G93）不改。去掉后与已有唯一键冲突的行会跳过。',
    stripLeadingZerosSuccess:
      '去前导 0 完成：汇总产品 {headerProduct}，明细产品 {itemProduct}，明细组件 {itemComponent}，冲突跳过 {skipped}',
    stripLeadingZerosFailed: '去前导 0 失败',
  },
}
