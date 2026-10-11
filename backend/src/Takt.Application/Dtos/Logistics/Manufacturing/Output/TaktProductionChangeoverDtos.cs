// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Application.Dtos.Logistics.Manufacturing.Output
// 文件名称：TaktProductionChangeoverDtos.cs
// 创建时间：2026-08-11
// 创建人：Takt365(Auto Generated)
// 功能描述：ProductionChangeover 模块 DTO（由 generate-dtos-from-entity.cjs 根据 TaktProductionChangeover 生成，请按需审阅）
// 
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

using System.ComponentModel.DataAnnotations;
using Mapster;
using Takt.Shared.Helpers;
using Takt.Shared.Models;

namespace Takt.Application.Dtos.Logistics.Manufacturing.Output;

// ========================================
// ProductionChangeover 响应 DTO
// ========================================

/// <summary>
/// 生产切换记录实体
/// 对应前端 TaktProductionChangeoverDto
/// 继承 TaktCompanyDtoBase
/// </summary>
public class TaktProductionChangeoverDto : TaktCompanyDtoBase
{
    /// <summary>
    /// ProductionChangeoverID（适配实体 Id，序列化为 string 以避免 Javascript 精度问题）
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionChangeoverId { get; set; }


    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 切换类别（字典 logistics_manufacturing_changeover_category；存 DictValue：ASSY/PCBA）
    /// </summary>
    public string ChangeoverCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    public string CurrentProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前机种（回填：随工单）
    /// </summary>
    public string CurrentModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    public string ChangeoverProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后机种（回填：随工单）
    /// </summary>
    public string ChangeoverModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换次数
    /// </summary>
    public int ChangeoverCount { get; set; } = 0;

    /// <summary>
    /// 切换时间（单次，单位：分钟）
    /// </summary>
    public int ChangeoverTime { get; set; } = 0;

    /// <summary>
    /// 仪设时间（仪器/设备设置耗时，单位：分钟）
    /// </summary>
    public int InstrumentSetupTime { get; set; } = 0;

    /// <summary>
    /// 切换总时间（单位：分钟）
    /// </summary>
    public int TotalChangeoverTime { get; set; } = 0;

    /// <summary>
    /// 读取SOP时间（单位：分钟）
    /// </summary>
    public int ReadSopTime { get; set; } = 0;

    /// <summary>
    /// 学习时间（切换学习/培训耗时，单位：分钟）
    /// </summary>
    public int LearningTime { get; set; } = 0;

    /// <summary>
    /// 人数（参与切换人数）
    /// </summary>
    public int PersonCount { get; set; } = 0;

    /// <summary>
    /// 学习总时间（单位：分钟）
    /// </summary>
    public int TotalLearningTime { get; set; } = 0;

    /// <summary>
    /// SOP总时间（单位：分钟）
    /// </summary>
    public int TotalSopTime { get; set; } = 0;

}

// ========================================
// ProductionChangeover 查询 DTO
// ========================================

/// <summary>
/// ProductionChangeover 分页查询 DTO
/// 继承 TaktPagedQuery
/// </summary>
public class TaktProductionChangeoverQueryDto : TaktPagedQuery
{
    /// <summary>
    /// 租户编码
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司代码
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工厂（回填：随工单）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 切换类别（字典 logistics_manufacturing_changeover_category；存 DictValue：ASSY/PCBA）
    /// </summary>
    public string? ChangeoverCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期（范围查询-开始）
    /// </summary>
    public DateTime? ProdDateStart { get; set; }

    /// <summary>
    /// 生产日期（范围查询-结束）
    /// </summary>
    public DateTime? ProdDateEnd { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    public string? CurrentProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前机种（回填：随工单）
    /// </summary>
    public string? CurrentModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    public string? ChangeoverProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后机种（回填：随工单）
    /// </summary>
    public string? ChangeoverModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换次数
    /// </summary>
    public int? ChangeoverCount { get; set; }

    /// <summary>
    /// 切换时间（单次，单位：分钟）
    /// </summary>
    public int? ChangeoverTime { get; set; }

    /// <summary>
    /// 仪设时间（仪器/设备设置耗时，单位：分钟）
    /// </summary>
    public int? InstrumentSetupTime { get; set; }

    /// <summary>
    /// 切换总时间（单位：分钟）
    /// </summary>
    public int? TotalChangeoverTime { get; set; }

    /// <summary>
    /// 读取SOP时间（单位：分钟）
    /// </summary>
    public int? ReadSopTime { get; set; }

    /// <summary>
    /// 学习时间（切换学习/培训耗时，单位：分钟）
    /// </summary>
    public int? LearningTime { get; set; }

    /// <summary>
    /// 人数（参与切换人数）
    /// </summary>
    public int? PersonCount { get; set; }

    /// <summary>
    /// 学习总时间（单位：分钟）
    /// </summary>
    public int? TotalLearningTime { get; set; }

    /// <summary>
    /// SOP总时间（单位：分钟）
    /// </summary>
    public int? TotalSopTime { get; set; }

    /// <summary>
    /// 创建时间（范围查询-开始）
    /// </summary>
    public DateTime? CreatedAtStart { get; set; }

    /// <summary>
    /// 创建时间（范围查询-结束）
    /// </summary>
    public DateTime? CreatedAtEnd { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注（模糊查询）
    /// </summary>
    public string? Remark { get; set; }
}

// ========================================
// 创建ProductionChangeover DTO
// ========================================

/// <summary>
/// 创建ProductionChangeover DTO
/// </summary>
public class TaktProductionChangeoverCreateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工厂（回填：随工单）
    /// </summary>
    [Required(ErrorMessage = "生产工厂（回填：随工单）不能为空")]
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 切换类别（字典 logistics_manufacturing_changeover_category；存 DictValue：ASSY/PCBA）
    /// </summary>
    [Required(ErrorMessage = "切换类别（字典 logistics_manufacturing_changeover_category；存 DictValue：ASSY/PCBA）不能为空")]
    public string ChangeoverCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    [Required(ErrorMessage = "当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）不能为空")]
    public string CurrentProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前机种（回填：随工单）
    /// </summary>
    [Required(ErrorMessage = "当前机种（回填：随工单）不能为空")]
    public string CurrentModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    [Required(ErrorMessage = "切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）不能为空")]
    public string ChangeoverProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后机种（回填：随工单）
    /// </summary>
    [Required(ErrorMessage = "切换后机种（回填：随工单）不能为空")]
    public string ChangeoverModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换次数
    /// </summary>
    public int ChangeoverCount { get; set; } = 0;

    /// <summary>
    /// 切换时间（单次，单位：分钟）
    /// </summary>
    public int ChangeoverTime { get; set; } = 0;

    /// <summary>
    /// 仪设时间（仪器/设备设置耗时，单位：分钟）
    /// </summary>
    public int InstrumentSetupTime { get; set; } = 0;

    /// <summary>
    /// 切换总时间（单位：分钟）
    /// </summary>
    public int TotalChangeoverTime { get; set; } = 0;

    /// <summary>
    /// 读取SOP时间（单位：分钟）
    /// </summary>
    public int ReadSopTime { get; set; } = 0;

    /// <summary>
    /// 学习时间（切换学习/培训耗时，单位：分钟）
    /// </summary>
    public int LearningTime { get; set; } = 0;

    /// <summary>
    /// 人数（参与切换人数）
    /// </summary>
    public int PersonCount { get; set; } = 0;

    /// <summary>
    /// 学习总时间（单位：分钟）
    /// </summary>
    public int TotalLearningTime { get; set; } = 0;

    /// <summary>
    /// SOP总时间（单位：分钟）
    /// </summary>
    public int TotalSopTime { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 更新ProductionChangeover DTO
// ========================================

/// <summary>
/// 更新ProductionChangeover DTO
/// 继承 TaktProductionChangeoverCreateDto，添加 ProductionChangeoverId 字段
/// </summary>
public class TaktProductionChangeoverUpdateDto : TaktProductionChangeoverCreateDto
{
    /// <summary>
    /// ProductionChangeoverID（标识要更新的实体）
    /// </summary>
    [Required(ErrorMessage = "ID不能为空")]
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionChangeoverId { get; set; }

}

// ========================================
// 导入 DTO
// ========================================

/// <summary>
/// ProductionChangeover 导入模板行 DTO
/// </summary>
public class TaktProductionChangeoverTemplateDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工厂（回填：随工单）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 切换类别（字典 logistics_manufacturing_changeover_category；存 DictValue：ASSY/PCBA）
    /// </summary>
    public string? ChangeoverCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    public string? CurrentProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前机种（回填：随工单）
    /// </summary>
    public string? CurrentModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    public string? ChangeoverProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后机种（回填：随工单）
    /// </summary>
    public string? ChangeoverModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换次数
    /// </summary>
    public int? ChangeoverCount { get; set; }

    /// <summary>
    /// 切换时间（单次，单位：分钟）
    /// </summary>
    public int? ChangeoverTime { get; set; }

    /// <summary>
    /// 仪设时间（仪器/设备设置耗时，单位：分钟）
    /// </summary>
    public int? InstrumentSetupTime { get; set; }

    /// <summary>
    /// 切换总时间（单位：分钟）
    /// </summary>
    public int? TotalChangeoverTime { get; set; }

    /// <summary>
    /// 读取SOP时间（单位：分钟）
    /// </summary>
    public int? ReadSopTime { get; set; }

    /// <summary>
    /// 学习时间（切换学习/培训耗时，单位：分钟）
    /// </summary>
    public int? LearningTime { get; set; }

    /// <summary>
    /// 人数（参与切换人数）
    /// </summary>
    public int? PersonCount { get; set; }

    /// <summary>
    /// 学习总时间（单位：分钟）
    /// </summary>
    public int? TotalLearningTime { get; set; }

    /// <summary>
    /// SOP总时间（单位：分钟）
    /// </summary>
    public int? TotalSopTime { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

/// <summary>
/// ProductionChangeover 导入 DTO（独立实现，不继承 TemplateDto）
/// </summary>
public class TaktProductionChangeoverImportDto
{
    /// <summary>
    /// 租户编码（登录上下文注入，对应请求头 X-Tenant-Code）
    /// </summary>
    public string? TenantCode { get; set; } = string.Empty;

    /// <summary>
    /// 公司（选项 TaktCompanies/options；DictValue=CompanyCode）
    /// </summary>
    public string? CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 区域文化编码（业务字段；字典 sys_culture_code；BCP47 如 zh-CN、en-US、ja-JP；DictData 另可用 mul=多种语言内容）
    /// </summary>
    public string? CultureCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工厂（回填：随工单）
    /// </summary>
    public string? PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 切换类别（字典 logistics_manufacturing_changeover_category；存 DictValue：ASSY/PCBA）
    /// </summary>
    public string? ChangeoverCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    public string? CurrentProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前机种（回填：随工单）
    /// </summary>
    public string? CurrentModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    public string? ChangeoverProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后机种（回填：随工单）
    /// </summary>
    public string? ChangeoverModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换次数
    /// </summary>
    public int? ChangeoverCount { get; set; }

    /// <summary>
    /// 切换时间（单次，单位：分钟）
    /// </summary>
    public int? ChangeoverTime { get; set; }

    /// <summary>
    /// 仪设时间（仪器/设备设置耗时，单位：分钟）
    /// </summary>
    public int? InstrumentSetupTime { get; set; }

    /// <summary>
    /// 切换总时间（单位：分钟）
    /// </summary>
    public int? TotalChangeoverTime { get; set; }

    /// <summary>
    /// 读取SOP时间（单位：分钟）
    /// </summary>
    public int? ReadSopTime { get; set; }

    /// <summary>
    /// 学习时间（切换学习/培训耗时，单位：分钟）
    /// </summary>
    public int? LearningTime { get; set; }

    /// <summary>
    /// 人数（参与切换人数）
    /// </summary>
    public int? PersonCount { get; set; }

    /// <summary>
    /// 学习总时间（单位：分钟）
    /// </summary>
    public int? TotalLearningTime { get; set; }

    /// <summary>
    /// SOP总时间（单位：分钟）
    /// </summary>
    public int? TotalSopTime { get; set; }

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

}

// ========================================
// 导出 DTO
// ========================================

/// <summary>
/// ProductionChangeover 导出 DTO（独立实现，不继承响应 Dto）
/// </summary>
public class TaktProductionChangeoverExportDto
{
    /// <summary>
    /// ProductionChangeoverID
    /// </summary>
    [AdaptMember("Id")]
    [JsonConverter(typeof(ValueToStringConverter))]
    public long ProductionChangeoverId { get; set; }

    /// <summary>
    /// 公司代码
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产工厂（回填：随工单）
    /// </summary>
    public string PlantCode { get; set; } = string.Empty;

    /// <summary>
    /// 生产类别（字典 logistics_manufacturing_prod_category；存 DictValue：EPP/FPP/RWP/MDP/CPP）
    /// </summary>
    public string? ProdCategory { get; set; } = string.Empty;

    /// <summary>
    /// 切换类别（字典 logistics_manufacturing_changeover_category；存 DictValue：ASSY/PCBA）
    /// </summary>
    public string ChangeoverCategory { get; set; } = string.Empty;

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime ProdDate { get; set; }

    /// <summary>
    /// 生产班组（选项 TaktProductionTeams/options，存 TeamCode，ExtValue=PlantCode 按工厂过滤）
    /// </summary>
    public string? TeamCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前工单（切换前工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    public string CurrentProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 当前机种（回填：随工单）
    /// </summary>
    public string CurrentModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后工单（切换目标工单号，选项 TaktProductionOrders/options，按 PlantCode 过滤）
    /// </summary>
    public string ChangeoverProdOrderCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换后机种（回填：随工单）
    /// </summary>
    public string ChangeoverModelCode { get; set; } = string.Empty;

    /// <summary>
    /// 切换次数
    /// </summary>
    public int ChangeoverCount { get; set; } = 0;

    /// <summary>
    /// 切换时间（单次，单位：分钟）
    /// </summary>
    public int ChangeoverTime { get; set; } = 0;

    /// <summary>
    /// 仪设时间（仪器/设备设置耗时，单位：分钟）
    /// </summary>
    public int InstrumentSetupTime { get; set; } = 0;

    /// <summary>
    /// 切换总时间（单位：分钟）
    /// </summary>
    public int TotalChangeoverTime { get; set; } = 0;

    /// <summary>
    /// 读取SOP时间（单位：分钟）
    /// </summary>
    public int ReadSopTime { get; set; } = 0;

    /// <summary>
    /// 学习时间（切换学习/培训耗时，单位：分钟）
    /// </summary>
    public int LearningTime { get; set; } = 0;

    /// <summary>
    /// 人数（参与切换人数）
    /// </summary>
    public int PersonCount { get; set; } = 0;

    /// <summary>
    /// 学习总时间（单位：分钟）
    /// </summary>
    public int TotalLearningTime { get; set; } = 0;

    /// <summary>
    /// SOP总时间（单位：分钟）
    /// </summary>
    public int TotalSopTime { get; set; } = 0;

    /// <summary>
    /// 扩展字段JSON
    /// </summary>
    public string? ExtField { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
