// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/databaseBackup.types
// 文件名称：zh-CN.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：数据库备份页静态文案；引用键 code.database.database-backup.page.*（段内全小写）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: '数据库备份',
    subtitle:
      '对租户库执行 Full Sync / Delta Sync。目标可为本地（服务器端）、客户端、文件服务器或 FTP；立即执行与后台执行均创建定时任务。',
    section: {
      form: '备份参数',
      history: '备份记录',
    },
    field: {
      tenant: '目标租户',
      database: '目标数据库',
      backuptype: '备份类型',
      backuppath: '备份路径',
      scheduledat: '执行时间',
      remark: '备注',
      file: '备份文件',
      status: '状态',
    },
    pathtype: {
      local: '本地（服务器端）',
      client: '客户端',
      network: '文件服务器',
      ftp: 'FTP 服务器',
    },
    dialog: {
      localtitle: '选择本地文件夹（服务器端）',
      clienttitle: '选择客户端文件夹',
      networktitle: '文件服务器',
      ftptitle: 'FTP 服务器',
      localpathplaceholder: '输入或浏览 API 服务器路径，例如 D:\\Backup\\2026',
      localpickbutton: '选择文件夹',
      localnativehint: '弹出目录选择器，数据来自 API（browse/local）；可新建目录，无固定白名单。',
      localnoabsolutepath: '请选择或输入绝对路径',
      clientnativehint: '双击盘符，在系统对话框中选择目标文件夹；系统自动回填完整绝对路径（如 D:\\SQLRecovery），无需手补。',
      clientemptyhint: '双击盘符以授权并浏览本机目录',
      clientpathplaceholder: '本机完整绝对路径，例如 D:\\Backup\\2026',
      clientpathrequired: '请选择客户端目录',
      clientneeddrivefirst: '请先双击盘符并授权访问后再操作',
      clientgrantdrivehint: '请选择 {drive} 下的目标文件夹，确定后系统自动回填完整路径',
      clientgrantfailed: '授权本机目录失败',
      clientpickerunsupported: '当前浏览器不支持本机目录选择，请换用 Chrome / Edge',
      clientabsoluterequired: '请选择完整绝对路径（如 D:\\Backup）',
      localneedabsolute: '请先进入盘符或文件夹后再确定（如 D:\\Backup）',
      localpickedname: '已选：{name}',
      createdirectory: '新建文件夹',
      createdirectorysuccess: '目录已创建',
      createdirectoryfailed: '创建目录失败',
      newfolderplaceholder: '新文件夹名称',
      newfolderrequired: '请输入新文件夹名称',
      ftppathplaceholder: '输入远程路径后跳转，例如 /backup',
      uncplaceholder: '\\\\server\\share\\folder',
      passwordkeep: '留空表示不修改已保存密码',
      reselect: '重新选择',
      notselecmed: '尚未选择目标路径',
    },
    backuptype: {
      full: 'Full Sync（完整备份）',
      delta: 'Delta Sync（差异备份）',
    },
    status: {
      pending: '待执行',
      running: '执行中',
      success: '成功',
      failed: '失败',
      scheduled: '已调度',
    },
    executemode: {
      immediate: '立即',
      background: '后台',
    },
    button: {
      runnow: '立即执行',
      schedule: '后台执行',
      refresh: '刷新',
    },
    tip: {
      path: '路径类型：本地=API 服务器磁盘；客户端=本机选择器；文件服务器=UNC；FTP=远程 FTP',
      delta: '差异备份须存在同库的完整备份基线，否则 SQL Server 会失败',
      schedule: '立即执行与后台执行都会自动创建定时任务记录',
    },
    message: {
      runsuccess: '已创建立即备份任务',
      schedulesuccess: '已创建后台备份任务',
      pathrequired: '请选择备份路径',
      schedulerequired: '请选择执行时间',
      schedulefuture: '执行时间须晚于当前时间',
      browsefailed: '目录浏览失败',
      ftprequired: '请填写 FTP 服务器、用户名与密码',
    },
  },
};
