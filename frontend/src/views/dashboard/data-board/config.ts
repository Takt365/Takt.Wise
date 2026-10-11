/**
 * 数据看板统计模块默认配置与存储 key（与工作台一致的可添加模块方式）
 */
import type { DataBoardModuleItem, DataBoardModuleMeta } from '@/types/dashboard/dataBoard.types'

export const DATA_BOARD_STORAGE_KEY = 'takt-data-board-modules'

/** 可添加的统计模块类型列表 */
export const DATA_BOARD_AVAILABLE_MODULES: DataBoardModuleMeta[] = [
  { key: 'overview', titleKey: 'dashboard.data-board.page.modules.overview', defaultSpan: 12 },
  { key: 'change', titleKey: 'dashboard.data-board.page.modules.change', defaultSpan: 12 },
  { key: 'online', titleKey: 'dashboard.data-board.page.modules.online', defaultSpan: 12 },
  { key: 'salesInvoice', titleKey: 'dashboard.data-board.page.modules.salesinvoice', defaultSpan: 12 },
  { key: 'salesOrder', titleKey: 'dashboard.data-board.page.modules.salesorder', defaultSpan: 12 },
  { key: 'assyOph', titleKey: 'dashboard.data-board.page.modules.assyoph', defaultSpan: 12 },
  { key: 'pcbaOph', titleKey: 'dashboard.data-board.page.modules.pcbaoph', defaultSpan: 12 },
  { key: 'assyDefect', titleKey: 'dashboard.data-board.page.modules.assydefect', defaultSpan: 12 },
  { key: 'pcba', titleKey: 'dashboard.data-board.page.modules.pcba', defaultSpan: 12 },
  { key: 'qualityCost', titleKey: 'dashboard.data-board.page.modules.qualitycost', defaultSpan: 12 },
  { key: 'qualityOperation', titleKey: 'dashboard.data-board.page.modules.qualityoperation', defaultSpan: 12 },
  { key: 'qualityComplaint', titleKey: 'dashboard.data-board.page.modules.qualitycomplaint', defaultSpan: 12 },
  { key: 'helpDesk', titleKey: 'dashboard.data-board.page.modules.helpdesk', defaultSpan: 12 },
  { key: 'meetingNotice', titleKey: 'dashboard.data-board.page.modules.meetingnotice', defaultSpan: 12 },
  { key: 'customerService', titleKey: 'dashboard.data-board.page.modules.customerservice', defaultSpan: 12 },
  { key: 'purchaseInvoice', titleKey: 'dashboard.data-board.page.modules.purchaseinvoice', defaultSpan: 12 },
  { key: 'purchaseOrder', titleKey: 'dashboard.data-board.page.modules.purchaseorder', defaultSpan: 12 },
  { key: 'purchaseRequest', titleKey: 'dashboard.data-board.page.modules.purchaserequest', defaultSpan: 12 },
  { key: 'inventory', titleKey: 'dashboard.data-board.page.modules.inventory', defaultSpan: 12 },
  { key: 'custom', titleKey: 'dashboard.data-board.page.modules.custom', defaultSpan: 24 }
]

/** 默认数据看板模块（首次进入或未持久化时） */
export function getDefaultDataBoardModules(): DataBoardModuleItem[] {
  return [
    { id: 'overview-1', moduleKey: 'overview', span: 24 }
  ]
}

export function generateModuleId(moduleKey: string): string {
  return `${moduleKey}-${Date.now()}`
}
