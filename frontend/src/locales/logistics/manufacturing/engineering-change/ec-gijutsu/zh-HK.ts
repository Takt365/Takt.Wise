// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-gijutsu
// 文件名称：zh-HK.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：設變主頁面靜態文案；引用鍵 logistics.manufacturing.engineering.change.ec.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    sourceEcInput: {
      title: '來源設變錄入',
      openButton: '來源錄入',
      searchPlaceholder: '設變號碼 / 標題 / 機種',
      detailCount: '明細行數',
      selectRequired: '請選擇一條來源設變',
      loadToForm: '載入至表單',
      plantFromCompany: '公司 {company} → 工廠 {plant}',
      companyRequired: '請先選擇公司後再錄入來源設變',
      formTitle: '來源設變導入（請填寫負責人、管理區分並上傳附件）',
      attachmentRequired: '請至少添加一條附件並上傳文件',
      attachmentUploadRequired: '第 {row} 行附件尚未上傳文件',
      importSelected: '導入選中（{count}）',
      importSuccess: '已成功導入 {count} 條設變',
      importPartial: '導入完成：成功 {success} 條，失敗 {fail} 條',
      detailsDeferred: '明細共 {count} 行，已改由服務端落庫（不經瀏覽器回傳），請填寫負責人與管理區分後提交',
    },
    attachment: {
      docCode: {
        formatInvalid: '文件編碼格式不正確（{hint}）',
        duplicate: '文件編碼「{code}」已存在，不可重複',
        hint: {
          empty: '請輸入文件編碼',
          ec: '與設變單號一致',
          eppFpp: 'P-xxxx（P- + 4 位數字，如 P-0001）',
          tl: 'DTS-xxxx（DTS- + 4 位數字，如 DTS-0001）',
          quadDash: 'xxxx-xxxx（各 4 位數字，如 1234-5678）',
        },
      },
      fileName: {
        duplicate: '文件名稱「{name}」已存在，不可重複',
      },
      upload: {
        hint: '僅支持 PDF，每次 1 個文件，不超過 {max}MB',
        pdfOnly: '僅允許上傳 PDF 文件',
      },
    },
    tabs: {
      oldNewMaterial: '舊新物料',
      scope: '實施範圍',
    },
    scopeDepts: {
      master: '部門實施範圍',
      autoFill: '自動填寫',
      pmc: '生管',
      mp: '採購',
      iqc: '受檢',
      mc: '部管',
      smt: 'SMT',
      pcba: '製二',
      assy: '製一',
      qa: '品管',
      te: '製技',
    },
    persist: {
      submitted: '已提交設變 {ecCode} 後台{action}（明細 {detailCount} 行，含各部門執行派生），完成後將通知您',
      completed: '設變 {ecCode} 後台{action}完成（明細 {detailCount} 行，耗時 {duration}）',
      failed: '設變 {ecCode} 後台{action}失敗（明細 {detailCount} 行）',
      actionCreate: '新增',
      actionUpdate: '更新',
    },
  },
};
