// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/logistics/manufacturing/engineering-change/ec-gijutsu
// 文件名称：zh-CN.ts
// 创建时间：2026-06-29
// 创建人：Takt365(Cursor AI)
// 功能描述：设变主页面静态文案；引用键 logistics.manufacturing.engineering-change.ec-gijutsu.page.*
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    sourceEcInput: {
      title: '来源设变录入',
      openButton: '来源录入',
      searchPlaceholder: '设变号码 / 标题 / 机种',
      detailCount: '明细行数',
      selectRequired: '请选择一条来源设变',
      loadToForm: '加载到表单',
      plantFromCompany: '公司 {company} → 工厂 {plant}',
      companyRequired: '请先选择公司后再录入来源设变',
      formTitle: '来源设变导入（请填写设变担当、实施范围并上传附件）',
      attachmentRequired: '请至少添加一条附件并上传文件',
      attachmentUploadRequired: '第 {row} 行附件尚未上传文件',
      importSelected: '导入选中（{count}）',
      importSuccess: '已成功导入 {count} 条设变',
      importPartial: '导入完成：成功 {success} 条，失败 {fail} 条',
      detailsDeferred: '明细共 {count} 行，已改由服务端落库（不经浏览器回传），请填写设变担当与实施范围后提交',
    },
    attachment: {
      docCode: {
        formatInvalid: '文件编码格式不正确（{hint}）',
        duplicate: '文件编码「{code}」已存在，不可重复',
        hint: {
          empty: '请输入文件编码',
          ec: '与设变单号一致',
          eppFpp: 'P-xxxx（P- + 4 位数字，如 P-0001）',
          tl: 'DTS-xxxx（DTS- + 4 位数字，如 DTS-0001）',
          quadDash: 'xxxx-xxxx（各 4 位数字，如 1234-5678）',
        },
      },
      fileName: {
        duplicate: '文件名称「{name}」已存在，不可重复',
      },
      upload: {
        hint: '仅支持 PDF，每次 1 个文件，不超过 {max}MB',
        pdfOnly: '仅允许上传 PDF 文件',
      },
    },
    tabs: {
      oldNewMaterial: '旧新物料',
      scope: '实施策略',
    },
    scopeDepts: {
      master: '部门实施范围',
      autoFill: '自动填写',
      pmc: '生管',
      mp: '采购',
      iqc: '受检',
      mc: '部管',
      smt: 'SMT',
      pcba: '制二',
      assy: '制一',
      qa: '品管',
      te: '制技',
      nonFert: '非FERT',
      discontinued: '停产',
    },
    persist: {
      submitted: '已提交设变 {ecCode} 后台{action}（明细 {detailCount} 行，含各部门执行派生），完成后将通知您',
      completed: '设变 {ecCode} 后台{action}完成（明细 {detailCount} 行，耗时 {duration}）',
      failed: '设变 {ecCode} 后台{action}失败（明细 {detailCount} 行）',
      actionCreate: '新增',
      actionUpdate: '更新',
    },
  },
};
