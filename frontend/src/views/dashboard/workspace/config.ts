/**
 * 工作台模块默认配置与存储 key
 */
import type { WorkspaceModuleItem, WorkspaceModuleMeta } from '@/types/dashboard/workspace.types'

export const WORKSPACE_STORAGE_KEY = 'takt-workspace-modules'

/** 可添加的模块类型列表（用于“添加模块”选择） */
export const WORKSPACE_AVAILABLE_MODULES: WorkspaceModuleMeta[] = [
  { key: 'welcome', titleKey: 'dashboard.workspace.page.modules.welcome', defaultSpan: 12 },
  { key: 'shortcut', titleKey: 'dashboard.workspace.page.modules.shortcut', defaultSpan: 12 },
  { key: 'todo', titleKey: 'dashboard.workspace.page.modules.todo', defaultSpan: 12 },
  { key: 'Announcement', titleKey: 'dashboard.workspace.page.modules.announcement', defaultSpan: 12 },
  { key: 'custom', titleKey: 'dashboard.workspace.page.modules.custom', defaultSpan: 24 }
]

/** 默认工作台模块（首次进入或未持久化时）：欢迎问候 + 快捷入口 */
export function getDefaultWorkspaceModules(): WorkspaceModuleItem[] {
  return [
    { id: 'welcome-1', moduleKey: 'welcome', span: 12 },
    { id: 'shortcut-1', moduleKey: 'shortcut', span: 12 }
  ]
}

export function generateModuleId(moduleKey: string): string {
  return `${moduleKey}-${Date.now()}`
}