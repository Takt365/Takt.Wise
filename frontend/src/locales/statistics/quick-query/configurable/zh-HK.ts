// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/statistics/quick-query/configurable
// 文件名称：zh-HK.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：statistics/quick-query/configurable 页面静态文案；引用键 statistics.quickquery.configurable.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    run: "執行定制報表",
    selectionscreen: "篩選條件",
    query: "查詢",
    exportdata: "導出數據",
    valueto: "結束值",
    enablenullcheck: "啟用條件",
    resulttitle: "查詢結果",
    maxrowshint: "最多返回 {max} 行",
    rowlimit: "最大命中數",
    runpage: {
      backtolist: "返回列表",
      resultempty: "請先填寫篩選條件並點擊查詢",
      col: {
        label: "標籤",
        operator: "選項",
        input: "輸入值",
      },
    },
    field: {
      tenant: "租戶",
      database: "數據庫",
      table: "數據表",
    },
    operator: {
      eq: "等於",
      ne: "不等於",
      gt: "大於",
      gte: "大於等於",
      lt: "小於",
      lte: "小於等於",
      like: "包含",
      between: "區間",
      in: "在列表中",
      isnull: "為空",
      isnotnull: "不為空",
    },
    designer: {
      design: "報表設計",
      createstep: "定制报表 {code}：基本資訊",
      tablestep: "選擇數據表與連接",
      quickview: "編碼",
      titlelabel: "標題",
      section: {
        datasource: "數據源",
        tabledata: "表/數據庫視圖中的數據",
      },
      sourcetype: {
        table: "表",
        join: "表連接",
        clearedjoin: "已切換為單表模式，表連接配置已清除",
        locked: "已進入後續步驟，不能再更改「表 / 表連接」類型",
      },
      tableview: "表格/視圖",
      mode: {
        basic: "基本模式",
        layout: "佈局模式",
      },
      confirm: "確定",
      datasource: "數據源（主表）",
      outputfields: "輸出字段",
      applyprimary: "設為主表",
      importcolumns: "導入全部列",
      quickdept: "示例：部門表",
      addselection: "添加篩選項",
      output: "輸出",
      technical: "技術名",
      importsuccess: "已導入全部列",
      novisiblefield: "請至少勾選一個輸出字段",
      prevstep: "上一步",
      nextstep: "下一步",
      execute: "執行 (F8)",
      steps: {
        basicinfo: "基本資訊",
        selecttables: "選擇數據表",
        selecttable: "選擇數據表",
        selectjoin: "配置表連接",
        joindesign: "表連接設計",
        datalist: "數據列清單",
        fieldpick: "字段選擇",
        outputfields: "輸出字段",
        selection: "篩選條件",
        advanced: "高級設置",
      },
      fieldtree: {
        root: "表/視圖/結構",
        datafield: "字段清單（描述）",
        outputlist: "清單字段",
        selectionfield: "選擇字段",
        fieldname: "字段名稱",
        empty: "請先在上一步選擇數據表",
        sourcenotready: "無法寫入數據源，請確認已選擇租戶與數據表",
      },
      jointype: {
        inner: "內連接",
        left: "左連接",
        right: "右連接",
        full: "全連接",
      },
      join: {
        primarytable: "主表",
        jointable: "連接表",
        condition: "關聯條件",
        apply: "應用表連接",
        importcolumns: "導入兩表全部列",
        importsuccess: "已導入兩表全部列",
        incomplete: "請完整選擇主表與連接表",
        conditionrequired: "請選擇關聯條件列",
        samealias: "主表與連接表別名不能相同",
        summaryreadonly: "表連接已在「表連接設計」步驟配置，此處為只讀預覽",
      },
    },
  },
};
