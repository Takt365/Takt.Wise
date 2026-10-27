// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/statistics/quick-query/configurable
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：statistics/quick-query/configurable page static copy; keys statistics.quickquery.configurable.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    run: "Run Custom Report",
    selectionscreen: "Selection Criteria",
    query: "Query",
    exportdata: "Export Data",
    valueto: "End Value",
    enablenullcheck: "Apply",
    resulttitle: "Query Result",
    maxrowshint: "Up to {max} rows",
    rowlimit: "Maximum hits",
    runpage: {
      backtolist: "Back to list",
      resultempty: "Set selection criteria and run query first",
      col: {
        label: "Label",
        operator: "Operator",
        input: "Value",
      },
    },
    field: {
      tenant: "Tenant",
      database: "Database",
      table: "Table",
    },
    operator: {
      eq: "Equals",
      ne: "Not equal",
      gt: "Greater than",
      gte: "Greater or equal",
      lt: "Less than",
      lte: "Less or equal",
      like: "Contains",
      between: "Between",
      in: "In list",
      isnull: "Is null",
      isnotnull: "Is not null",
    },
    designer: {
      design: "Report Design",
      createstep: "定制报表 {code}: Basic Info",
      tablestep: "Select Tables & Join",
      quickview: "Code",
      titlelabel: "Title",
      section: {
        datasource: "Data Source",
        tabledata: "Data in Table/Database View",
      },
      sourcetype: {
        table: "Table",
        join: "Table Join",
        clearedjoin: "Switched to single-table mode; join configuration cleared",
        locked: "Source type is locked after you proceed; table vs join cannot be changed",
      },
      tableview: "Table/View",
      mode: {
        basic: "Basic Mode",
        layout: "Layout Mode",
      },
      confirm: "OK",
      datasource: "Data Source (Primary Table)",
      outputfields: "Output Fields",
      applyprimary: "Set as Primary",
      importcolumns: "Import All Columns",
      quickdept: "Example: Department",
      addselection: "Add Filter",
      output: "Output",
      technical: "Technical Name",
      importsuccess: "All columns imported",
      novisiblefield: "Select at least one output field",
      prevstep: "Previous",
      nextstep: "Next",
      execute: "Execute (F8)",
      steps: {
        basicinfo: "Basic Info",
        selecttables: "Select Tables",
        selecttable: "Select Table",
        selectjoin: "Configure Table Join",
        joindesign: "Table Join Design",
        datalist: "Column List",
        fieldpick: "Field Selection",
        outputfields: "Output Fields",
        selection: "Selection Criteria",
        advanced: "Advanced",
      },
      fieldtree: {
        root: "Table/View/Structure",
        datafield: "Field List (ConfigurableDescription)",
        outputlist: "List Fields",
        selectionfield: "Selection Fields",
        fieldname: "Field Name",
        empty: "Select a table in the previous step first",
        sourcenotready: "Could not apply the data source. Confirm tenant and table are selecmed.",
      },
      jointype: {
        inner: "Inner Join",
        left: "Left Join",
        right: "Right Join",
        full: "Full Join",
      },
      join: {
        primarytable: "Primary Table",
        jointable: "Joined Table",
        condition: "Join Condition",
        apply: "Apply Table Join",
        importcolumns: "Import All Columns (Both Tables)",
        importsuccess: "All columns from both tables imported",
        incomplete: "Select primary and joined tables",
        conditionrequired: "Select join condition columns",
        samealias: "Primary and joined table aliases must differ",
        summaryreadonly: "Join is configured in the Table Join Design step; read-only preview here",
      },
    },
  },
};
