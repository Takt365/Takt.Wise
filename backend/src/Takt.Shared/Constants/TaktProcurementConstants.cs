// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Constants
// 文件名称：TaktProcurementChainConstants.cs
// 创建时间：2026-06-24
// 创建人：Takt365(Cursor AI)
// 功能描述：采购全链路编排常量（三套方案、会签 BusinessType/BusinessKey）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Shared.Constants;

/// <summary>
/// 采购全链路编排常量
/// </summary>
public static class TaktProcurementConstants
{
    /// <summary>
    /// 询价价格（字典 logistics_procurement_price_type=3）
    /// </summary>
    public const int PriceTypeInquiry = 3;

    /// <summary>
    /// 费用类型：月结供应商货款（字典 accounting_financial_expense_type=2）
    /// </summary>
    public const int ExpenseTypeSupplierPayment = 2;

    /// <summary>
    /// 费用类型：杂项购置（员工报销场景字典 accounting_financial_expense_type=3）
    /// </summary>
    public const int ExpenseTypeMiscPurchase = 3;

    /// <summary>
    /// 方案一：询价→PR→人工PO决策→报销
    /// </summary>
    public const int ChainSchemeWithExpense = 1;

    /// <summary>
    /// 方案二：询价→PR→自动PO（无报销）
    /// </summary>
    public const int ChainSchemePoOnly = 2;

    /// <summary>
    /// PO 决策：暂不生成 PO，直接报销
    /// </summary>
    public const int PoDecisionSkipPo = 0;

    /// <summary>
    /// PO 决策：生成 PO 后报销
    /// </summary>
    public const int PoDecisionGeneratePo = 1;

    /// <summary>
    /// 会签业务类型：询价审批（字典 accounting_financial_countersign_business_type）
    /// </summary>
    public const string BusinessTypeInquiry = "inquiry";

    /// <summary>
    /// 会签业务类型：采购申请审批
    /// </summary>
    public const string BusinessTypePurchaseRequest = "pr";

    /// <summary>
    /// 会签业务类型：费用报销审批
    /// </summary>
    public const string BusinessTypeExpense = "expense";

    /// <summary>
    /// 会签业务类型：独立会签（方案三入口）
    /// </summary>
    public const string BusinessTypeStandalone = "standalone";

    /// <summary>
    /// 付款方式：供应商付款（字典 logistics_procurement_payment_mode）
    /// </summary>
    public const string PaymentModeVendorPay = "vendorpay";

    /// <summary>
    /// 付款方式：员工报销
    /// </summary>
    public const string PaymentModeEmployeeReimburse = "employeereimburse";

    /// <summary>
    /// 会签单物理表名
    /// </summary>
    public const string CountersignTableName = "takt_accounting_financial_countersign";

    /// <summary>
    /// 费用单物理表名
    /// </summary>
    public const string ExpenseTableName = "takt_accounting_financial_expense";

    /// <summary>
    /// 会签步骤：询价审批
    /// </summary>
    public const int StepNoInquiry = 1;

    /// <summary>
    /// 会签步骤：采购申请审批
    /// </summary>
    public const int StepNoPurchaseRequest = 2;

    /// <summary>
    /// 会签步骤：费用报销审批
    /// </summary>
    public const int StepNoExpense = 3;

    /// <summary>
    /// 会签流程键
    /// </summary>
    public const string ProcessKeyCountersign = "Countersign";

    /// <summary>
    /// 会签明细 ItemDescription 前缀：存放来源物料编码
    /// </summary>
    public const string CountersignMaterialCodePrefix = "MAT:";
}
