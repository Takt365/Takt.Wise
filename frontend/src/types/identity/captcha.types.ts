// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/types/identity
// 文件名称：captcha.types.ts
// 创建时间：2026-05-25
// 创建人：Takt365(Cursor AI)
// 功能描述：登录验证码类型定义
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

/**
 * 验证码类型字面量（与 Captcha:Type、utils/taktCaptchaType 一致）
 */
export type TaktCaptchaTypeValue = 'Slider' | 'Behavior';

/**
 * 验证码提交给登录接口的载荷
 */
export interface TaktCaptchaSubmitPayload {
  /**
   * 验证码 ID
   */
  captchaId: string;

  /**
   * 验证数据 JSON 字符串（position、timeSpent、mouseTrajectory）
   */
  captchaCode: string;
}

/**
 * 验证码轨迹点（mouseTrajectory 数组元素）
 */
export interface TaktCaptchaTrajectoryPointDto {
  /**
   * 横向坐标（相对滑轨，像素）
   */
  x: number;

  /**
   * 纵向坐标（相对滑轨，像素）
   */
  y: number;

  /**
   * 相对按下时刻的耗时（毫秒，Behavior 轨迹评分可选）
   */
  t?: number;
}

/**
 * 验证码提交载荷（与后端 Verify UserInput JSON 一致）
 */
export interface TaktCaptchaSubmissionDto {
  /**
   * 滑块位置（百分比 0–100）
   */
  position: number;

  /**
   * 完成耗时（秒）
   */
  timeSpent: number;

  /**
   * 鼠标轨迹（RequireBehaviorData / RequireTrajectory 时提交）
   */
  mouseTrajectory?: TaktCaptchaTrajectoryPointDto[];
}

/**
 * 验证码挑战 DTO（GET session/captcha 响应）
 */
export interface TaktCaptchaChallengeDto {
  /**
   * 验证码 ID
   */
  captchaId: string;

  /**
   * 验证码类型：Slider | Behavior
   */
  captchaType: string;

  /**
   * 拼图区域宽度（像素）
   */
  width: number;

  /**
   * 拼图区域高度（像素）
   */
  height: number;

  /**
   * 滑块宽度（像素）
   */
  sliderWidth: number;

  /**
   * 滑块高度（像素）
   */
  sliderHeight: number;

  /**
   * 是否要求提交 timeSpent、mouseTrajectory 等行为数据
   */
  requireBehaviorData: boolean;

  /**
   * 背景图 data URL 或 Base64（仅 Slider）
   */
  backgroundImage?: string;

  /**
   * 滑块图 data URL 或 Base64（仅 Slider）
   */
  sliderImage?: string;

  /**
   * 目标位置百分比 0–100（仅 Behavior，用于前端目标指示）
   */
  targetPosition?: number;
}

