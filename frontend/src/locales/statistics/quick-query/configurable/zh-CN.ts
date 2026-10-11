// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/statistics/quick-query/configurable
// 文件名称：zh-CN.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：statistics/quick-query/configurable 页面静态文案；引用键 statistics.quickquery.configurable.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    run: "执行定制报表",
    selectionscreen: "筛选条件",
    query: "查询",
    exportdata: "导出数据",
    valueto: "结束值",
    enablenullcheck: "启用条件",
    resulttitle: "查询结果",
    maxrowshint: "最多返回 {max} 行",
    rowlimit: "最大命中数",
    runpage: {
      backtolist: "返回列表",
      resultempty: "请先填写筛选条件并点击查询",
      col: {
        label: "标签",
        operator: "选项",
        input: "输入值",
      },
    },
    field: {
      tenant: "租户",
      database: "数据库",
      table: "数据表",
    },
    operator: {
      eq: "等于",
      ne: "不等于",
      gt: "大于",
      gte: "大于等于",
      lt: "小于",
      lte: "小于等于",
      like: "包含",
      between: "区间",
      in: "在列表中",
      isnull: "为空",
      isnotnull: "不为空",
    },
    designer: {
      design: "定制报表设计",
      createstep: "定制报表 {code}：基本信息",
      tablestep: "选择数据表与连接",
      quickview: "定制报表编码",
      titlelabel: "标题",
      section: {
        datasource: "数据源",
        tabledata: "表/数据库视图中的数据",
      },
      sourcetype: {
        table: "表",
        join: "表连接",
        clearedjoin: "已切换为单表模式，表连接配置已清除",
        locked: "已进入后续步骤，不能再更改「表 / 表连接」类型",
      },
      tableview: "表格/视图",
      mode: {
        basic: "基本模式",
        layout: "布局模式",
      },
      confirm: "确定",
      datasource: "数据源（主表）",
      outputfields: "输出字段",
      applyprimary: "设为主表",
      importcolumns: "导入全部列",
      quickdept: "示例：部门表",
      addselection: "添加筛选项",
      output: "输出",
      technical: "技术名",
      importsuccess: "已导入全部列",
      novisiblefield: "请至少勾选一个输出字段",
      prevstep: "上一步",
      nextstep: "下一步",
      execute: "执行 (F8)",
      steps: {
        basicinfo: "基本信息",
        selecttables: "选择数据表",
        selecttable: "选择数据表",
        selectjoin: "配置表连接",
        joindesign: "表连接设计",
        datalist: "数据列清单",
        fieldpick: "字段选择",
        outputfields: "输出字段",
        selection: "筛选条件",
        advanced: "高级设置",
      },
      fieldtree: {
        root: "表/视图/结构",
        datafield: "字段清单（描述）",
        outputlist: "清单字段",
        selectionfield: "选择字段",
        fieldname: "字段名称",
        empty: "请先在上一步选择数据表",
        sourcenotready: "无法写入数据源，请确认已选择租户与数据表",
      },
      jointype: {
        inner: "内连接",
        left: "左连接",
        right: "右连接",
        full: "全连接",
      },
      join: {
        primarytable: "主表",
        jointable: "连接表",
        condition: "关联条件",
        apply: "应用表连接",
        importcolumns: "导入两表全部列",
        importsuccess: "已导入两表全部列",
        incomplete: "请完整选择主表与连接表",
        conditionrequired: "请选择关联条件列",
        samealias: "主表与连接表别名不能相同",
        summaryreadonly: "表连接已在「表连接设计」步骤配置，此处为只读预览",
      },
    },
  },
};
