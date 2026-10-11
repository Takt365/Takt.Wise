export type DataBoardModuleKey =
  | 'overview'
  | 'change'
  | 'online'
  | 'salesInvoice'
  | 'salesOrder'
  | 'assyOph'
  | 'pcbaOph'
  | 'assyDefect'
  | 'pcba'
  | 'qualityCost'
  | 'qualityOperation'
  | 'qualityComplaint'
  | 'helpDesk'
  | 'meetingNotice'
  | 'customerService'
  | 'purchaseInvoice'
  | 'purchaseOrder'
  | 'purchaseRequest'
  | 'inventory'
  | 'custom'

export interface DataBoardModuleMeta {
  key: DataBoardModuleKey
  titleKey: string
  defaultSpan: number
}

export interface DataBoardModuleItem {
  id: string
  moduleKey: DataBoardModuleKey
  span: number
}
