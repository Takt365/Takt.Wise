// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Domain.Interfaces
// 文件名称：ITaktHasRelatedPlant.cs
// 创建时间：2026-08-12
// 创建人：Takt365(Cursor AI)
// 功能描述：租户组合 1/3 关联工厂契约（FillHelper 仅对此类型补齐 RelatedPlant）
//
// 版权信息：Copyright (c) 2026 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

namespace Takt.Domain.Interfaces;

/// <summary>
/// 具备关联工厂字段的租户实体（组合 1、组合 3）
/// </summary>
public interface ITaktHasRelatedPlant
{
    /// <summary>
    /// 关联工厂（选项 TaktPlants/options；DictValue=PlantCode）
    /// </summary>
    string RelatedPlant { get; set; }
}
