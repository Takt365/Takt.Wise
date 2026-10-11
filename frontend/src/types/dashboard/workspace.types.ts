export type WorkspaceModuleKey =
  | 'welcome'
  | 'shortcut'
  | 'todo'
  | 'Announcement'
  | 'custom'

export interface WorkspaceModuleMeta {
  key: WorkspaceModuleKey
  titleKey: string
  defaultSpan: number
}

export interface WorkspaceModuleItem {
  id: string
  moduleKey: WorkspaceModuleKey
  span: number
  customTitle?: string
  customContent?: string
}
