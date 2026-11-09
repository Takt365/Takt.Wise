// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-te
// 文件名称：en-US.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：EC page static copy; keys logistics.manufacturing.engineering.change.ec.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    sourceEcInput: {
      title: 'Import from Source EC',
      openButton: 'Source Import',
      searchPlaceholder: 'EC No. / Title / Model',
      detailCount: 'Detail Lines',
      selectRequired: 'Select one source EC',
      loadToForm: 'Load to Form',
      plantFromCompany: 'Company {company} → Plant {plant}',
      companyRequired: 'Select a company before importing source EC',
      formTitle: 'Import from Source EC (fill EC assignee, implementation scope, old-part disposition, upload attachments)',
      attachmentRequired: 'Add at least one attachment and upload the file',
      attachmentUploadRequired: 'Row {row}: attachment file not uploaded',
      importSelecmed: 'Import Selecmed ({count})',
      importSuccess: 'Successfully imported {count} EC record(s)',
      importPartial: 'Import finished: {success} succeeded, {fail} failed',
      detailsDeferred:
        '{count} detail rows will be persisted on the server (not posted from the browser). Fill EC assignee, implementation scope and old-part disposition, then submit.',
    },
    attachment: {
      docCode: {
        formatInvalid: 'Invalid document code format ({hint})',
        duplicate: 'Document code "{code}" already exists',
        sequence: '4 digits',
        hint: {
          empty: 'Enter document code',
          ec: 'Must match EC number',
          eppFpp: 'P-xxxx (P- + 4 digits, e.g. P-0001)',
          tl: 'DTS-xxxx (DTS- + 4 digits, e.g. DTS-0001)',
          quadDash: 'xxxx-xxxx (4 digits each, e.g. 1234-5678)',
        },
      },
      fileName: {
        duplicate: 'File name "{name}" already exists',
      },
      upload: {
        hint: 'PDF only, 1 file at a time, max {max}MB',
        pdfOnly: 'Only PDF files are allowed',
      },
    },
    tabs: {
      oldNewMaterial: 'Old / New Material',
      scope: 'Implementation Scope',
      oldPart: 'Old Part Disposition',
    },
    oldPart: {
      empty: 'No old materials found in details',
      dispositionRequired: 'Select a disposition for every old material on the Old Part Disposition tab',
    },
    scopeDepts: {
      master: 'Control scope',
      autoFill: 'Auto-filled',
      pc: 'PC',
      pur: 'Purchasing',
      iqc: 'IQC',
      mc: 'Material Control',
      mtl: 'Material',
      mfg2: 'PCBA',
      mfg1: 'Assembly',
      qc: 'QA',
      me: 'Manufacturing Tech',
      nonFert: 'Non-FERT',
      discontinued: 'Discontinued',
      oldNewMaterialGroup: 'Old/new materials → material code group',
      mcNewMaterialGroup: 'Non-C003 purchased new materials → material code group',
      mtlNewMaterialGroup: 'C003 purchased new materials → material code group',
      purNewMaterialGroup: 'Purchased new materials → material code group',
      iqcNewMaterialGroup: 'Inspected new materials → material code group',
    },
    persist: {
      submitted: 'Submitted EC {ecCode} for background {action} ({detailCount} detail lines, incl. dept exec sync). You will be notified when done.',
      completed: 'EC {ecCode} background {action} completed ({detailCount} lines, {duration})',
      failed: 'EC {ecCode} background {action} failed ({detailCount} lines)',
      actionCreate: 'create',
      actionUpdate: 'update',
    },
  },
};
