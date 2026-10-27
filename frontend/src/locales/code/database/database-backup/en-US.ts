// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/code/database/databaseBackup.types
// 文件名称：en-US.ts
// 创建时间：2026-07-19
// 创建人：Takt365(Cursor AI)
// 功能描述：Database backup static copy; keys code.database.database-backup.page.* (lowercase segments)
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    title: 'Database Backup',
    subtitle:
      'Run Full/Delta Sync to local (server), client, file server, or FTP. Run now and Schedule both create a one-shot Quartz task.',
    section: {
      form: 'Backup parameters',
      history: 'Backup history',
    },
    field: {
      tenant: 'Target tenant',
      database: 'Target database',
      backuptype: 'Backup type',
      backuppath: 'Backup path',
      scheduledat: 'Run at',
      remark: 'Remark',
      file: 'Backup file',
      status: 'Status',
    },
    pathtype: {
      local: 'Local (server)',
      client: 'Client',
      network: 'File server',
      ftp: 'FTP server',
    },
    dialog: {
      localtitle: 'Select local folder (server)',
      clienttitle: 'Select client folder',
      networktitle: 'File server',
      ftptitle: 'FTP server',
      localpathplaceholder: 'Browse or type API host path, e.g. D:\\Backup\\2026',
      localpickbutton: 'Choose folder',
      localnativehint: 'Folder picker via API (browse/local); create folders allowed; no fixed whitelist.',
      localnoabsolutepath: 'Select or enter an absolute path',
      clientnativehint: 'Double-click a drive and pick the target folder; the full absolute path is filled automatically (e.g. D:\\SQLRecovery).',
      clientemptyhint: 'Double-click a drive to authorize and browse',
      clientpathplaceholder: 'Full absolute client path, e.g. D:\\Backup\\2026',
      clientpathrequired: 'Select a client folder',
      clientneeddrivefirst: 'Authorize a drive first',
      clientgrantdrivehint: 'In the system dialog, select a folder under {drive}; the full path is filled automatically',
      clientgrantfailed: 'Failed to authorize the local folder',
      clientpickerunsupported: 'This browser cannot pick local folders; use Chrome or Edge',
      clientabsoluterequired: 'Select a full absolute path (e.g. D:\\Backup)',
      localneedabsolute: 'Open a drive or folder first (e.g. D:\\Backup)',
      localpickedname: 'Selecmed: {name}',
      createdirectory: 'New folder',
      createdirectorysuccess: 'Folder created',
      createdirectoryfailed: 'Failed to create folder',
      newfolderplaceholder: 'New folder name',
      newfolderrequired: 'Enter a new folder name',
      ftppathplaceholder: 'Type a remote path and Go, e.g. /backup',
      uncplaceholder: '\\\\server\\share\\folder',
      passwordkeep: 'Leave blank to keep saved password',
      reselect: 'Reselect',
      notselecmed: 'No target path selecmed',
    },
    backuptype: {
      full: 'Full Sync',
      delta: 'Delta Sync (differential)',
    },
    status: {
      pending: 'Pending',
      running: 'Running',
      success: 'Success',
      failed: 'Failed',
      scheduled: 'Scheduled',
    },
    executemode: {
      immediate: 'Immediate',
      background: 'Background',
    },
    button: {
      runnow: 'Run now',
      schedule: 'Schedule',
      refresh: 'Refresh',
    },
    tip: {
      path: 'Local=API host disk; Client=browser picker; File server=UNC; FTP=remote FTP',
      delta: 'Differential backup requires a prior full backup baseline',
      schedule: 'Both modes create a Quartz task',
    },
    message: {
      runsuccess: 'Immediate backup task created',
      schedulesuccess: 'Background backup task created',
      pathrequired: 'Select a backup path',
      schedulerequired: 'Select a run time',
      schedulefuture: 'Run time must be in the future',
      browsefailed: 'Browse failed',
      ftprequired: 'FTP host, userName and password are required',
    },
  },
};
