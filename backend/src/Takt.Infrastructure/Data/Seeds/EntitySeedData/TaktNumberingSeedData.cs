// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Infrastructure.Data.Seeds.EntitySeedData
// 文件名称：TaktNumberingSeedData.cs
// 创建时间：2026-06-14
// 创建人：Takt365(Cursor AI)
// 功能描述：内置业务编码规则种子（基础/日常/人事/财务/后勤等模块；按 Database:CompanyCodes 各公司写入）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Takt.Domain.Entities.Accounting.Financial;
using Takt.Domain.Entities.Foundation;
using Takt.Domain.Entities.Identity;
using Takt.Domain.Interfaces;
using Takt.Domain.Repositories;
using Takt.Shared.Constants;
using Takt.Shared.Helpers;
using Takt.Shared.Models;
using Takt.Shared.Options;

namespace Takt.Infrastructure.Data.Seeds.EntitySeedData;

/// <summary>
/// 内置编码规则种子（按 Database:CompanyCodes 各公司写入；幂等：更新段配置并刷新 StartingNumber 为首号样例）
/// </summary>
public class TaktNumberingSeedData : ITaktSeedDataCoordinator
{
    private const int StatusEnabled = 1;
    private const int IsBuiltInYes = 1;
    private const string SegmentsWithDepartment =
        "segments:CompanyCode,DeptCode,PrefixCode,DateSequence";
    private const string SegmentsCompanyLevel =
        "segments:CompanyCode,PrefixCode,DateSequence";
    private const string SegmentsCompanyNoDate =
        "segments:CompanyCode,PrefixCode,Sequence";
    private const string SegmentsGenderEmployeeCode =
        "segments:PrefixCode,Sequence";
    private const string DefaultSeparator = "-";

    // DocumentType 存 TaktMenu.MenuName（表单 tree-options?valueBy=name）；模板用 MenuCode，种子时解析
    private const string MenuQuartzTask = "FOUNDATION_QUARTZ_TASK";
    private const string MenuFile = "FOUNDATION_FILE";
    private const string MenuEmployee = "HUMAN_RESOURCE_PERSONNEL_EMPLOYEE";
    private const string MenuAnnouncement = "ROUTINE_ANNOUNCEMENT";
    private const string MenuMeeting = "ROUTINE_MEETING_CENTER_MEETING";
    private const string MenuDocument = "ROUTINE_DOCUMENT_CENTER";
    private const string MenuNews = "ROUTINE_NEWS_CENTER";
    private const string MenuHelpDeskTicket = "ROUTINE_HELP_DESK_TICKET";
    private const string MenuConfigurableQuickQuery = "STATISTICS_QUICK_QUERY_CONFIGURABLE";
    private const string MenuFlowForm = "WORKFLOW_FORM";
    private const string MenuAsset = "ACCOUNTING_FINANCIAL_ASSET";
    private const string MenuServiceContract = "LOGISTICS_CUSTOMER_SERVICE_CONTRACT";
    private const string MenuServiceOrder = "LOGISTICS_CUSTOMER_SERVICE_ORDER";
    private const string MenuServiceRequest = "LOGISTICS_CUSTOMER_SERVICE_REQUEST";
    private const string MenuServiceTicket = "LOGISTICS_CUSTOMER_SERVICE_TICKET";
    private const string MenuEquipment = "LOGISTICS_MAINTENANCE_EQUIPMENT";
    private const string MenuSalesInvoice = "LOGISTICS_SALES_INVOICE";
    private const string MenuSalesOrder = "LOGISTICS_SALES_ORDER";
    private const string MenuSalesCondition = "LOGISTICS_SALES_CONDITION";
    private const string MenuSalesQuotation = "LOGISTICS_SALES_QUOTATION";
    private const string MenuPurchaseOrder = "LOGISTICS_PROCUREMENT_PURCHASE_ORDER";
    private const string MenuPurchaseInfo = "LOGISTICS_PROCUREMENT_PURCHASE_INFO";
    private const string MenuPurchaseRequest = "LOGISTICS_PROCUREMENT_PURCHASE_REQUEST";
    private const string MenuSupplier = "LOGISTICS_PROCUREMENT_SUPPLIER";
    private const string MenuVendor = "LOGISTICS_PROCUREMENT_VENDOR";
    private const string MenuClient = "LOGISTICS_SALES_CLIENT";
    private const string MenuCustomer = "LOGISTICS_SALES_CUSTOMER";
    private const string MenuEcNotification = "LOGISTICS_MANUFACTURING_ENGINEERING_CHANGE_NOTIFICATION";

    // 与字典 sys_numbering_dept_code 的 DictValue 一致
    private const string IsoR = "R"; // 总务部 D0100
    private const string IsoF = "F"; // 财务部 D0200
    private const string IsoD = "D"; // IT部 D0300
    private const string IsoS = "S"; // 生管课 D0420
    private const string IsoB = "B"; // 部管课 D0430
    private const string IsoC = "C"; // 资材部 D0500
    private const string IsoZ = "Z"; // 制造部 D0600

    /// <summary>
    /// 执行顺序（在字典与公司基础数据之后、Quartz 示例之前）
    /// </summary>
    public int Order => 48;

    /// <summary>
    /// 初始化内置编码规则种子
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <param name="tenantCode">租户编码（由协调器传入）</param>
    /// <returns>返回插入和更新的记录数（插入数, 更新数）</returns>
    public async Task<(int InsertCount, int UpdateCount)> SeedAsync(
        IServiceProvider serviceProvider,
        string? tenantCode = null)
    {
        TaktLogger.Information("开始初始化内置编码规则种子数据...");
        if (string.IsNullOrEmpty(tenantCode))
        {
            TaktLogger.Warning("租户编码为空，跳过编码规则种子数据初始化");
            return (0, 0);
        }
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var database = configuration.RequireDatabase();
        var repository = serviceProvider.GetRequiredService<ITaktCompanySeedRepository<TaktNumbering>>();
        var companyRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktCompany>>();
        var menuRepository = serviceProvider.GetRequiredService<ITaktTenantSeedRepository<TaktMenu>>();
        var companies = await companyRepository.GetListAsync(
            c => c.TenantCode == tenantCode && c.CompanyStatus == 1);
        if (companies == null || companies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到启用的公司，跳过编码规则种子", tenantCode);
            return (0, 0);
        }
        var orderedCompanies = TaktDatabaseOptions.OrderByConfiguredCodes(
            database.CompanyCodes,
            companies,
            c => c.CompanyCode);
        if (orderedCompanies.Count == 0)
        {
            TaktLogger.Warning("租户 {TenantCode} 未找到 Database:CompanyCodes 对应的公司，跳过编码规则种子", tenantCode);
            return (0, 0);
        }
        var menus = await menuRepository.GetListAsync(m => m.TenantCode == tenantCode && m.IsDeleted == 0);
        var menuNameByCode = (menus ?? [])
            .Where(m => !string.IsNullOrWhiteSpace(m.MenuCode) && !string.IsNullOrWhiteSpace(m.MenuName))
            .GroupBy(m => m.MenuCode.Trim(), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First().MenuName.Trim(), StringComparer.OrdinalIgnoreCase);
        var templates = GetBuiltInRuleTemplates();
        var insertCount = 0;
        var updateCount = 0;
        TaktLogger.Information("正在为租户 {TenantCode} 初始化内置编码规则...", tenantCode);
        foreach (var company in orderedCompanies)
        {
            foreach (var template in templates)
            {
                if (!menuNameByCode.TryGetValue(template.MenuCode, out var documentType)
                    || string.IsNullOrWhiteSpace(documentType))
                {
                    TaktLogger.Warning(
                        "编码规则 {RuleCode} 跳过：未找到菜单 MenuCode={MenuCode}（DocumentType 须为对应菜单 MenuName）",
                        template.RuleCode,
                        template.MenuCode);
                    continue;
                }
                var (_, inserted, updated) = await CreateOrUpdateNumberingAsync(
                    repository,
                    tenantCode,
                    company.CompanyCode,
                    company.CompanyShortName,
                    database.GetPlantCodeForCompanyCode(company.CompanyCode),
                    company.CultureCode,
                    template,
                    documentType);
                insertCount += inserted;
                updateCount += updated;
            }
        }
        TaktLogger.Information(
            "内置编码规则种子完成: 插入 {InsertCount} 条，更新 {UpdateCount} 条",
            insertCount,
            updateCount);
        return (insertCount, updateCount);
    }

    /// <summary>
    /// 获取内置编码规则模板
    /// </summary>
    /// <returns>规则模板列表</returns>
    private static List<NumberingSeedTemplate> GetBuiltInRuleTemplates()
    {
        return
        [
            new NumberingSeedTemplate(
                "FD-TASK",
                "任务编码",
                MenuQuartzTask,
                IsoD,
                "TASK",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：定时任务 TaktQuartzTask.TaskCode"),
            new NumberingSeedTemplate(
                "FD-FDOC",
                "文档文件编码",
                MenuFile,
                IsoD,
                "FDOC",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktFile.FileCode；MIME→FileCategory.Document(0)"),
            new NumberingSeedTemplate(
                "FD-FIMG",
                "图片文件编码",
                MenuFile,
                IsoD,
                "FIMG",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktFile.FileCode；MIME image/*→FileCategory.Image(1)"),
            new NumberingSeedTemplate(
                "FD-FVID",
                "视频文件编码",
                MenuFile,
                IsoD,
                "FVID",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktFile.FileCode；MIME video/*→FileCategory.Video(2)"),
            new NumberingSeedTemplate(
                "FD-FAUD",
                "音频文件编码",
                MenuFile,
                IsoD,
                "FAUD",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktFile.FileCode；MIME audio/*→FileCategory.Audio(3)"),
            new NumberingSeedTemplate(
                "FD-FARC",
                "压缩包文件编码",
                MenuFile,
                IsoD,
                "FARC",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktFile.FileCode；MIME zip/rar/7z 等→FileCategory.Archive(4)"),
            new NumberingSeedTemplate(
                "FD-FOTH",
                "其他文件编码",
                MenuFile,
                IsoD,
                "FOTH",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktFile.FileCode；未匹配 MIME→FileCategory.Other(5)"),
            new NumberingSeedTemplate(
                "HR-EMPM",
                "男员工编码",
                MenuEmployee,
                IsoR,
                "M",
                "none",
                "None",
                SegmentsGenderEmployeeCode,
                "内置：TaktEmployee.EmployeeCode；Gender=1男（字典 sys_user_gender）",
                SequenceLength: 5,
                Separator: string.Empty),
            new NumberingSeedTemplate(
                "HR-EMPF",
                "女员工编码",
                MenuEmployee,
                IsoR,
                "F",
                "none",
                "None",
                SegmentsGenderEmployeeCode,
                "内置：TaktEmployee.EmployeeCode；Gender=2女（字典 sys_user_gender）",
                SequenceLength: 5,
                Separator: string.Empty),
            new NumberingSeedTemplate(
                "HR-EMPU",
                "未知性别员工编码",
                MenuEmployee,
                IsoR,
                "U",
                "none",
                "None",
                SegmentsGenderEmployeeCode,
                "内置：TaktEmployee.EmployeeCode；Gender=0未知（字典 sys_user_gender）",
                SequenceLength: 5,
                Separator: string.Empty),
            new NumberingSeedTemplate(
                "RT-ANN1",
                "紧急通知编码",
                MenuAnnouncement,
                IsoR,
                "URGN",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktAnnouncement.AnnouncementCode；AnnouncementType=1（字典 sys_announcement_category）"),
            new NumberingSeedTemplate(
                "RT-ANN2",
                "公告编码",
                MenuAnnouncement,
                IsoR,
                "ANNC",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktAnnouncement.AnnouncementCode；AnnouncementType=2（字典 sys_announcement_category）"),
            new NumberingSeedTemplate(
                "RT-ANN3",
                "通知编码",
                MenuAnnouncement,
                IsoR,
                "NOTF",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktAnnouncement.AnnouncementCode；AnnouncementType=3（字典 sys_announcement_category）"),
            new NumberingSeedTemplate(
                "RT-ANN4",
                "决议编码",
                MenuAnnouncement,
                IsoR,
                "RESL",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktAnnouncement.AnnouncementCode；AnnouncementType=4（字典 sys_announcement_category）"),
            new NumberingSeedTemplate(
                "RT-ANN5",
                "活动编码",
                MenuAnnouncement,
                IsoR,
                "ACTV",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktAnnouncement.AnnouncementCode；AnnouncementType=5（字典 sys_announcement_category）"),
            new NumberingSeedTemplate(
                "RT-ANN6",
                "安全通告编码",
                MenuAnnouncement,
                IsoR,
                "SAFE",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktAnnouncement.AnnouncementCode；AnnouncementType=6（字典 sys_announcement_category）"),
            new NumberingSeedTemplate(
                "RT-ANN7",
                "运维通知编码",
                MenuAnnouncement,
                IsoR,
                "OPSN",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktAnnouncement.AnnouncementCode；AnnouncementType=7（字典 sys_announcement_category）"),
            new NumberingSeedTemplate(
                "RT-ANN8",
                "系统公告编码",
                MenuAnnouncement,
                IsoR,
                "SYSA",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktAnnouncement.AnnouncementCode；AnnouncementType=8（字典 sys_announcement_category）"),
            new NumberingSeedTemplate(
                "RT-CONF0",
                "内部会议编码",
                MenuMeeting,
                IsoR,
                "INTN",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktMeeting.MeetingCode；MeetingType=0（字典 routine_meeting_center_type）"),
            new NumberingSeedTemplate(
                "RT-CONF1",
                "外部会议编码",
                MenuMeeting,
                IsoR,
                "EXTR",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktMeeting.MeetingCode；MeetingType=1（字典 routine_meeting_center_type）"),
            new NumberingSeedTemplate(
                "RT-CONF2",
                "视频会议编码",
                MenuMeeting,
                IsoR,
                "VIDO",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktMeeting.MeetingCode；MeetingType=2（字典 routine_meeting_center_type）"),
            new NumberingSeedTemplate(
                "RT-CONF3",
                "混合会议编码",
                MenuMeeting,
                IsoR,
                "HYBR",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktMeeting.MeetingCode；MeetingType=3（字典 routine_meeting_center_type）"),
            new NumberingSeedTemplate(
                "RT-DOC0",
                "制度文档编码",
                MenuDocument,
                IsoR,
                "REGL",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktDocument.DocumentCode；DocumentCategory=0（字典 routine_document_center_category）"),
            new NumberingSeedTemplate(
                "RT-DOC1",
                "流程文档编码",
                MenuDocument,
                IsoR,
                "PROC",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktDocument.DocumentCode；DocumentCategory=1（字典 routine_document_center_category）"),
            new NumberingSeedTemplate(
                "RT-DOC2",
                "模板文档编码",
                MenuDocument,
                IsoR,
                "TMPL",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktDocument.DocumentCode；DocumentCategory=2（字典 routine_document_center_category）"),
            new NumberingSeedTemplate(
                "RT-DOC3",
                "规范文档编码",
                MenuDocument,
                IsoR,
                "SPEC",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktDocument.DocumentCode；DocumentCategory=3（字典 routine_document_center_category）"),
            new NumberingSeedTemplate(
                "RT-DOC4",
                "其他文档编码",
                MenuDocument,
                IsoR,
                "DOOT",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktDocument.DocumentCode；DocumentCategory=4（字典 routine_document_center_category）"),
            new NumberingSeedTemplate(
                "RT-NEWS0",
                "公司新闻编码",
                MenuNews,
                IsoR,
                "CORP",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktNews.NewsCode；NewsCategory=0（字典 sys_news_type）"),
            new NumberingSeedTemplate(
                "RT-NEWS1",
                "行业动态编码",
                MenuNews,
                IsoR,
                "INDY",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktNews.NewsCode；NewsCategory=1（字典 sys_news_type）"),
            new NumberingSeedTemplate(
                "RT-NEWS2",
                "技术分享编码",
                MenuNews,
                IsoR,
                "TECH",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktNews.NewsCode；NewsCategory=2（字典 sys_news_type）"),
            new NumberingSeedTemplate(
                "RT-NEWS3",
                "产品发布编码",
                MenuNews,
                IsoR,
                "PROD",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktNews.NewsCode；NewsCategory=3（字典 sys_news_type）"),
            new NumberingSeedTemplate(
                "RT-NEWS4",
                "活动资讯编码",
                MenuNews,
                IsoR,
                "EVNT",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktNews.NewsCode；NewsCategory=4（字典 sys_news_type）"),
            new NumberingSeedTemplate(
                "RT-NEWS5",
                "其他新闻编码",
                MenuNews,
                IsoR,
                "NWOT",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktNews.NewsCode；NewsCategory=5（字典 sys_news_type）"),
            new NumberingSeedTemplate(
                "HD-TICKET",
                "工单编码",
                MenuHelpDeskTicket,
                IsoD,
                "TKT",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：服务台 TaktTicket.TicketCode"),
            new NumberingSeedTemplate(
                "ST-RPT",
                "自定义报表编码",
                MenuConfigurableQuickQuery,
                IsoD,
                "RPT",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktConfigurable.ConfigurableCode；前端表单选择编码规则后取号"),
            new NumberingSeedTemplate(
                "WF-FORM0",
                "通用表单编码",
                MenuFlowForm,
                IsoD,
                "FRMG",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktFlowForm.FormCode；FormCategory=0（字典 sys_form_category）"),
            new NumberingSeedTemplate(
                "WF-FORM1",
                "业务表单编码",
                MenuFlowForm,
                IsoD,
                "FRMB",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktFlowForm.FormCode；FormCategory=1（字典 sys_form_category）"),
            new NumberingSeedTemplate(
                "WF-FORM2",
                "系统表单编码",
                MenuFlowForm,
                IsoD,
                "FRMS",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：TaktFlowForm.FormCode；FormCategory=2（字典 sys_form_category）"),
            new NumberingSeedTemplate(
                "AC-ASSET",
                "资产编码",
                MenuAsset,
                IsoF,
                "AST",
                "yyyy",
                "Annually",
                SegmentsWithDepartment,
                "内置：财务资产 TaktAsset.AssetCode"),
            new NumberingSeedTemplate(
                "LG-SVC-CON",
                "服务合同编码",
                MenuServiceContract,
                IsoB,
                "SVCN",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktCustomerServiceContract.ServiceContractCode"),
            new NumberingSeedTemplate(
                "LG-SVC-ORD",
                "服务订单编码",
                MenuServiceOrder,
                IsoB,
                "SVCO",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktCustomerServiceOrder.ServiceOrderCode"),
            new NumberingSeedTemplate(
                "LG-SVC-REQ",
                "服务请求单号",
                MenuServiceRequest,
                IsoB,
                "SVCR",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktCustomerServiceRequest.ServiceRequestCode"),
            new NumberingSeedTemplate(
                "LG-SVC-TKT",
                "服务工单编码",
                MenuServiceTicket,
                IsoB,
                "SVCT",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktCustomerServiceTicket.ServiceTicketCode"),
            new NumberingSeedTemplate(
                "LG-EQP",
                "设备编码",
                MenuEquipment,
                IsoZ,
                "EQP",
                "none",
                "None",
                SegmentsWithDepartment,
                "内置：TaktEquipment.EquipCode"),
            new NumberingSeedTemplate(
                "LG-SLS-INV",
                "销售发票编码",
                MenuSalesInvoice,
                IsoS,
                "SINV",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktSalesInvoice.AccountingDocumentCode"),
            new NumberingSeedTemplate(
                "LG-SLS-ORD",
                "销售订单编码",
                MenuSalesOrder,
                IsoS,
                "SORD",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktSalesOrder.SalesOrderCode"),
            new NumberingSeedTemplate(
                "LG-SLS-PRC",
                "销售价格编码",
                MenuSalesCondition,
                IsoS,
                "SPRC",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktSalesCondition.SalesConditionCode"),
            new NumberingSeedTemplate(
                "LG-SLS-QUO",
                "销售报价编码",
                MenuSalesQuotation,
                IsoS,
                "SQUO",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktSalesQuotation.SalesQuotationCode"),
            new NumberingSeedTemplate(
                "LG-PUR-ORD",
                "采购订单编码",
                MenuPurchaseOrder,
                IsoC,
                "PUR",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktPurchaseOrder.PurchaseOrderCode"),
            new NumberingSeedTemplate(
                "LG-PUR-PRC",
                "采购价格编码",
                MenuPurchaseInfo,
                IsoC,
                "PRC",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktPurchaseInfo.PurchaseInfoCode"),
            new NumberingSeedTemplate(
                "LG-PUR-REQ",
                "采购申请编码",
                MenuPurchaseRequest,
                IsoC,
                "REQ",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktPurchaseRequest.PurchaseRequestCode"),
            new NumberingSeedTemplate(
                "LG-SUP",
                "供货商编码",
                MenuSupplier,
                IsoC,
                "SUP",
                "none",
                "None",
                SegmentsCompanyNoDate,
                "内置：TaktSupplier.SupplierCode"),
            new NumberingSeedTemplate(
                "LG-VND",
                "经销商编码",
                MenuVendor,
                IsoC,
                "VND",
                "none",
                "None",
                SegmentsCompanyNoDate,
                "内置：TaktVendor.VendorCode"),
            new NumberingSeedTemplate(
                "LG-CLT",
                "客户端编码",
                MenuClient,
                IsoB,
                "CLT",
                "none",
                "None",
                SegmentsCompanyNoDate,
                "内置：TaktClient.ClientCode"),
            new NumberingSeedTemplate(
                "LG-CUS",
                "客户编码",
                MenuCustomer,
                IsoB,
                "CUS",
                "none",
                "None",
                SegmentsCompanyNoDate,
                "内置：TaktCustomer.CustomerCode"),
            new NumberingSeedTemplate(
                TaktEcFlowConstants.NotificationRuleSystem,
                "设变系统通知编码",
                MenuEcNotification,
                IsoZ,
                "SYS",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktEcNotification.EcNotificationCode；EcNotificationMethod=1系统通知"),
            new NumberingSeedTemplate(
                TaktEcFlowConstants.NotificationRuleEmail,
                "设变邮件通知编码",
                MenuEcNotification,
                IsoZ,
                "MAIL",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktEcNotification.EcNotificationCode；EcNotificationMethod=2邮件"),
            new NumberingSeedTemplate(
                TaktEcFlowConstants.NotificationRulePaper,
                "设变纸质通知编码",
                MenuEcNotification,
                IsoZ,
                "PAPR",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktEcNotification.EcNotificationCode；EcNotificationMethod=3纸质"),
            new NumberingSeedTemplate(
                TaktEcFlowConstants.NotificationRuleMeeting,
                "设变会议通知编码",
                MenuEcNotification,
                IsoZ,
                "MEET",
                "yyyy",
                "Annually",
                SegmentsCompanyLevel,
                "内置：TaktEcNotification.EcNotificationCode；EcNotificationMethod=4会议"),
        ];
    }

    /// <summary>
    /// 创建或更新编码规则（按最新段配置重算 StartingNumber 为首号样例；实际流水以业务表为准）
    /// </summary>
    /// <param name="repository">编码规则仓储</param>
    /// <param name="tenantCode">租户编码</param>
    /// <param name="companyCode">公司编码</param>
    /// <param name="companyShortName">公司简称（写入业务编码公司段）</param>
    /// <param name="plantCode">工厂编码</param>
    /// <param name="cultureCode">区域文化</param>
    /// <param name="template">规则模板</param>
    /// <param name="documentType">单据类型（已解析的 TaktMenu.MenuName）</param>
    /// <returns>实体与插入/更新计数</returns>
    private static async Task<(TaktNumbering Rule, int InsertCount, int UpdateCount)> CreateOrUpdateNumberingAsync(
        ITaktCompanySeedRepository<TaktNumbering> repository,
        string tenantCode,
        string companyCode,
        string companyShortName,
        string plantCode,
        string cultureCode,
        NumberingSeedTemplate template,
        string documentType)
    {
        var entity = await repository.FirstAsync(x =>
            x.TenantCode == tenantCode
            && x.CompanyCode == companyCode
            && x.RuleCode == template.RuleCode);
        if (entity == null)
        {
            entity = new TaktNumbering
            {
                TenantCode = tenantCode,
                CompanyCode = companyCode,
                RuleCode = template.RuleCode,
                RuleName = template.RuleName,
                DocumentType = documentType,
                DeptCode = template.DeptCode,
                PrefixCode = template.PrefixCode,
                DateFormat = template.DateFormat,
                SequenceLength = template.SequenceLength,
                SequenceStep = template.SequenceStep,
                SuffixCode = template.SuffixCode,
                ResetPeriod = template.ResetPeriod,
                Separator = template.Separator,
                NumberingDescription = template.SegmentsDescription,
                IsBuiltIn = IsBuiltInYes,
                NumberingStatus = StatusEnabled,
                Remark = template.Remark,
                PlantCode = plantCode,
                CultureCode = cultureCode
            };
            ApplyDateFormatResetPeriodAlignment(entity);
            entity.StartingNumber = BuildInitialStartingNumber(entity, companyShortName, DateTime.Now);
            entity = await repository.CreateAsync(entity);
            return (entity, 1, 0);
        }
        entity.RuleName = template.RuleName;
        entity.DocumentType = documentType;
        entity.DeptCode = template.DeptCode;
        entity.PrefixCode = template.PrefixCode;
        entity.DateFormat = template.DateFormat;
        entity.SequenceLength = template.SequenceLength;
        entity.SequenceStep = template.SequenceStep;
        entity.SuffixCode = template.SuffixCode;
        entity.ResetPeriod = template.ResetPeriod;
        entity.Separator = template.Separator;
        entity.NumberingDescription = template.SegmentsDescription;
        entity.IsBuiltIn = IsBuiltInYes;
        entity.NumberingStatus = StatusEnabled;
        entity.Remark = template.Remark;
        entity.PlantCode = plantCode;
        entity.CultureCode = cultureCode;
        ApplyDateFormatResetPeriodAlignment(entity);
        entity.StartingNumber = BuildInitialStartingNumber(entity, companyShortName, DateTime.Now);
        await repository.UpdateAsync(entity);
        return (entity, 0, 1);
    }

    /// <summary>
    /// 按默认段顺序生成初始起始编码样例（公司段用 CompanyShortName；与 TaktNumberingHelper 一致）
    /// </summary>
    /// <param name="rule">编码规则</param>
    /// <param name="companyShortName">公司简称</param>
    /// <param name="referenceTime">参考时间</param>
    /// <returns>起始编码样例</returns>
    private static string BuildInitialStartingNumber(
        TaktNumbering rule,
        string companyShortName,
        DateTime referenceTime)
    {
        var step = rule.SequenceStep <= 0 ? 1 : rule.SequenceStep;
        var model = new TaktNumberingModel
        {
            RuleCode = rule.RuleCode,
            BusinessCode = rule.StartingNumber,
            CompanyCode = rule.CompanyCode,
            CompanyShortName = string.IsNullOrWhiteSpace(companyShortName) ? rule.CompanyCode : companyShortName.Trim(),
            DeptCode = rule.DeptCode,
            PrefixCode = rule.PrefixCode,
            DateFormat = rule.DateFormat,
            SequenceLength = rule.SequenceLength,
            SequenceStep = rule.SequenceStep,
            SuffixCode = rule.SuffixCode,
            ResetPeriod = rule.ResetPeriod,
            Separator = rule.Separator,
            Description = rule.NumberingDescription,
            DocumentType = rule.DocumentType,
            UpdatedAt = rule.UpdatedAt,
        };
        return TaktNumberingHelper.FormatBusinessCode(model, step, referenceTime);
    }

    /// <summary>
    /// 按 DateFormat 强制对齐 ResetPeriod（与 TaktNumberingHelper.NormalizeNumberingModel 一致）
    /// </summary>
    /// <param name="entity">编码规则</param>
    private static void ApplyDateFormatResetPeriodAlignment(TaktNumbering entity)
    {
        if (string.Equals(entity.DateFormat?.Trim(), "none", StringComparison.OrdinalIgnoreCase))
        {
            entity.DateFormat = null;
        }
        else
        {
            entity.DateFormat = TaktNumberingHelper.NormalizeSupportedDateFormat(entity.DateFormat);
        }
        entity.ResetPeriod = TaktNumberingHelper.ResolveRequiredResetPeriod(entity.DateFormat);
        // DocumentType 已为菜单名称，勿再走旧版领域码 NormalizeDocumentType
    }

    /// <summary>
    /// 按 Description 段配置拼接业务编码（与 TaktNumberingService 一致）
    /// </summary>
    /// <param name="rule">编码规则</param>
    /// <param name="sequence">流水号</param>
    /// <param name="referenceTime">参考时间</param>
    /// <returns>业务编码</returns>
    private static string FormatBusinessCode(TaktNumbering rule, int sequence, DateTime referenceTime)
    {
        var length = rule.SequenceLength <= 0 ? 6 : rule.SequenceLength;
        var separator = string.IsNullOrWhiteSpace(rule.Separator) ? DefaultSeparator : rule.Separator.Trim();
        var effectiveDescription = ResolveEffectiveSegmentDescription(rule.NumberingDescription);
        var parts = new List<string>();
        foreach (var segmentKey in ParseSegmentKeys(effectiveDescription))
        {
            var part = ResolveSegmentValue(rule, segmentKey, sequence, length, referenceTime);
            if (!string.IsNullOrWhiteSpace(part))
            {
                parts.Add(part);
            }
        }
        var code = string.IsNullOrEmpty(separator)
            ? string.Concat(parts)
            : string.Join(separator, parts);
        var suffixCode = rule.SuffixCode?.Trim();
        if (!string.IsNullOrWhiteSpace(suffixCode))
        {
            code += suffixCode;
        }
        return code;
    }

    /// <summary>
    /// 从 segments 配置解析段键列表
    /// </summary>
    /// <param name="segmentsDescription">segments: 配置全文</param>
    /// <returns>段键列表</returns>
    private static IReadOnlyList<string> ParseSegmentKeys(string segmentsDescription)
    {
        return TryParseSegmentKeysFromDescription(segmentsDescription) ?? new[]
        {
            nameof(TaktNumbering.CompanyCode),
            nameof(TaktNumbering.DeptCode),
            nameof(TaktNumbering.PrefixCode),
            "DateSequence",
        };
    }

    /// <summary>
    /// 将旧版 segments 解析为有效段配置（与 TaktNumberingService 一致）
    /// </summary>
    /// <param name="description">原始 Description</param>
    /// <returns>有效段配置</returns>
    private static string ResolveEffectiveSegmentDescription(string? description)
    {
        var text = description?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(text)
            || !text.StartsWith("segments:", StringComparison.OrdinalIgnoreCase))
        {
            return SegmentsWithDepartment;
        }
        var body = text["segments:".Length..];
        var isLegacy = body.Contains("DocumentType", StringComparison.OrdinalIgnoreCase)
            || body.Contains("DateFormat,Sequence", StringComparison.OrdinalIgnoreCase);
        if (!isLegacy)
        {
            return text;
        }
        var hasDepartment = body.Contains("DeptCode", StringComparison.OrdinalIgnoreCase)
            || body.Contains("DepartmentCode", StringComparison.OrdinalIgnoreCase);
        var hasDate = body.Contains("DateFormat", StringComparison.OrdinalIgnoreCase)
            || body.Contains("DateSequence", StringComparison.OrdinalIgnoreCase);
        if (hasDepartment)
        {
            return hasDate ? SegmentsWithDepartment : "segments:CompanyCode,DeptCode,PrefixCode,Sequence";
        }
        return hasDate ? SegmentsCompanyLevel : SegmentsCompanyNoDate;
    }

    /// <summary>
    /// 解析单个编码段文本
    /// </summary>
    private static string? ResolveSegmentValue(
        TaktNumbering rule,
        string segmentKey,
        int sequence,
        int sequenceLength,
        DateTime time)
    {
        if (segmentKey.Equals("Sequence", StringComparison.OrdinalIgnoreCase))
        {
            return sequence.ToString().PadLeft(sequenceLength, '0');
        }
        if (segmentKey.Equals("DateSequence", StringComparison.OrdinalIgnoreCase))
        {
            var datePart = FormatDateSegment(rule.DateFormat, time);
            var seqPart = sequence.ToString().PadLeft(sequenceLength, '0');
            var combined = string.Concat(datePart, seqPart);
            return string.IsNullOrWhiteSpace(combined) ? null : combined;
        }
        if (segmentKey.Equals(nameof(TaktNumbering.DateFormat), StringComparison.OrdinalIgnoreCase))
        {
            return FormatDateSegment(rule.DateFormat, time);
        }
        if (segmentKey.Equals(nameof(TaktNumbering.PrefixCode), StringComparison.OrdinalIgnoreCase)
            || segmentKey.Equals("Prefix", StringComparison.OrdinalIgnoreCase))
        {
            var prefixCode = rule.PrefixCode?.Trim();
            return string.IsNullOrWhiteSpace(prefixCode) ? null : prefixCode.TrimEnd('-', '_', '/', ' ');
        }
        if (segmentKey.Equals(nameof(TaktNumbering.CompanyCode), StringComparison.OrdinalIgnoreCase))
        {
            return string.IsNullOrWhiteSpace(rule.CompanyCode) ? null : rule.CompanyCode.Trim();
        }
        if (segmentKey.Equals(nameof(TaktNumbering.DeptCode), StringComparison.OrdinalIgnoreCase)
            || segmentKey.Equals("DepartmentCode", StringComparison.OrdinalIgnoreCase))
        {
            return string.IsNullOrWhiteSpace(rule.DeptCode) ? null : rule.DeptCode.Trim();
        }
        if (segmentKey.Equals(nameof(TaktNumbering.DocumentType), StringComparison.OrdinalIgnoreCase))
        {
            return string.IsNullOrWhiteSpace(rule.DocumentType) ? null : rule.DocumentType.Trim();
        }
        return null;
    }

    /// <summary>
    /// 从 Description 解析 segments: 段配置
    /// </summary>
    private static string[]? TryParseSegmentKeysFromDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return null;
        }
        var text = description.Trim();
        const string prefix = "segments:";
        if (!text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
        var body = text[prefix.Length..].Trim();
        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }
        var keys = body.Split(new[] { ',', '|', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return keys.Length == 0 ? null : keys;
    }

    /// <summary>
    /// 按 DateFormat 生成日期段
    /// </summary>
    /// <param name="dateFormat">日期格式</param>
    /// <param name="time">参考时间</param>
    /// <returns>日期段文本；不使用日期时返回空串</returns>
    private static string FormatDateSegment(string? dateFormat, DateTime time)
    {
        if (string.IsNullOrWhiteSpace(dateFormat)
            || dateFormat.Trim().Equals("none", StringComparison.OrdinalIgnoreCase))
        {
            return string.Empty;
        }
        return dateFormat.Trim() switch
        {
            "yyyy" => time.ToString("yyyy"),
            "yyyyMM" => time.ToString("yyyyMM"),
            "yyyyMMdd" => time.ToString("yyyyMMdd"),
            "yyyyMMddHH" => time.ToString("yyyyMMddHH"),
            _ => time.ToString(dateFormat.Trim()),
        };
    }

    /// <summary>
    /// 编码规则种子模板
    /// </summary>
    /// <param name="RuleCode">规则编码</param>
    /// <param name="RuleName">规则名称</param>
    /// <param name="MenuCode">关联菜单 MenuCode（种子解析为 TaktMenu.MenuName 写入 DocumentType）</param>
    /// <param name="DeptCode">部门编码（字典 sys_numbering_dept_code；DictValue=部门短码如 R/F/D）</param>
    /// <param name="PrefixCode">前缀编码</param>
    /// <param name="DateFormat">日期格式</param>
    /// <param name="ResetPeriod">重置周期</param>
    /// <param name="SegmentsDescription">编码段配置（segments:…）</param>
    /// <param name="Remark">备注</param>
    /// <param name="SequenceLength">流水位数</param>
    /// <param name="SequenceStep">流水步长</param>
    /// <param name="SuffixCode">后缀编码</param>
    /// <param name="Separator">段间分隔符</param>
    private sealed record NumberingSeedTemplate(
        string RuleCode,
        string RuleName,
        string MenuCode,
        string DeptCode,
        string PrefixCode,
        string DateFormat,
        string ResetPeriod,
        string SegmentsDescription,
        string Remark,
        int SequenceLength = 6,
        int SequenceStep = 1,
        string? SuffixCode = null,
        string Separator = DefaultSeparator);
}
