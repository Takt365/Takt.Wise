// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：Takt.Shared.Helpers
// 文件名称：TaktRegexHelper.cs
// 功能描述：通用正则与格式验证（与前端 regex.ts 对齐）。
// ========================================

using System.Globalization;
using System.Text.RegularExpressions;
using Takt.Shared.Constants;

namespace Takt.Shared.Helpers;

/// <summary>
/// Takt 通用正则与格式验证帮助类（后端）。
/// 规则命名与前端 regex.ts 对齐，便于前后端一致。
/// </summary>
/// <remarks>
/// 仅含编译期不可变的正则常量，无运行时可变状态。
/// </remarks>
public static class TaktRegexHelper
{
    /// <summary>
    /// 统一正则选项：预编译提升服务端高频校验性能。
    /// </summary>
    private const RegexOptions Opt = RegexOptions.Compiled;

    /// <summary>
    /// 邮箱最小长度（RFC 5322 最短合法地址如 a@b.co）。
    /// </summary>
    public const int EmailMinLength = 6;

    /// <summary>
    /// 邮箱最大长度（与 TaktEmployee.email varchar(100) 一致）。
    /// </summary>
    public const int EmailMaxLength = 100;

    /// <summary>
    /// 用户昵称最小长度。
    /// </summary>
    public const int NickNameMinLength = 2;

    /// <summary>
    /// 用户昵称最大长度（与 TaktUser.NickName nvarchar(20) 一致）。
    /// </summary>
    public const int NickNameMaxLength = 20;

    /// <summary>
    /// 员工姓名最小长度。
    /// </summary>
    public const int EmployeeNameMinLength = 2;

    /// <summary>
    /// 员工姓名最大长度（与 TaktEmployee.EmployeeName nvarchar(80) 一致）。
    /// </summary>
    public const int EmployeeNameMaxLength = 80;

    /// <summary>
    /// 中国大陆手机号位数。
    /// </summary>
    public const int PhoneCnLength = 11;

    /// <summary>
    /// 中国香港手机号位数。
    /// </summary>
    public const int PhoneHkLength = 8;

    /// <summary>
    /// 美国手机号位数。
    /// </summary>
    public const int PhoneUsLength = 10;

    /// <summary>
    /// 日本手机号位数。
    /// </summary>
    public const int PhoneJpLength = 11;

    /// <summary>
    /// 公司默认文化 zh-CN → 手机号格式说明 I18n 键（common.tip.phone.*，表单提示用）。
    /// </summary>
    public const string PhoneTipI18nKeyCn = "common.tip.phone.cn";

    /// <summary>
    /// 公司默认文化 zh-HK → 手机号格式说明 I18n 键。
    /// </summary>
    public const string PhoneTipI18nKeyHk = "common.tip.phone.hk";

    /// <summary>
    /// 公司默认文化 en-US → 手机号格式说明 I18n 键。
    /// </summary>
    public const string PhoneTipI18nKeyUs = "common.tip.phone.us";

    /// <summary>
    /// 公司默认文化 ja-JP → 手机号格式说明 I18n 键。
    /// </summary>
    public const string PhoneTipI18nKeyJp = "common.tip.phone.jp";
    /// <summary>
    /// 邮箱地址（RFC 5322 常用子集；长度 6–100 由 IsValidEmail 校验）。
    /// </summary>
    public static readonly Regex Email = new(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)+$", Opt);
    /// <summary>
    /// 手机号（中国大陆，与前端 RegexPatterns.PHONE_CN 对齐）。
    /// </summary>
    public static readonly Regex PhoneCn = new(@"^1[3-9]\d{9}$", Opt);
    /// <summary>
    /// 手机号（中国台湾，与前端 RegexPatterns.PHONE_TW 对齐）。
    /// </summary>
    public static readonly Regex PhoneTw = new(@"^09\d{8}$", Opt);
    /// <summary>
    /// 手机号（中国香港，与前端 RegexPatterns.PHONE_HK 对齐）。
    /// </summary>
    public static readonly Regex PhoneHk = new(@"^(5|6|8|9)\d{7}$", Opt);
    /// <summary>
    /// 手机号（美国 10 位，与前端 RegexPatterns.PHONE_US 对齐）。
    /// </summary>
    public static readonly Regex PhoneUs = new(@"^[2-9]\d{2}[2-9]\d{6}$", Opt);
    /// <summary>
    /// 手机号（日本 11 位，与前端 RegexPatterns.PHONE_JP 对齐）。
    /// </summary>
    public static readonly Regex PhoneJp = new(@"^0\d{10}$", Opt);
    /// <summary>
    /// 固定电话（中国大陆，与前端 RegexPatterns.TEL_CN 对齐）。
    /// </summary>
    public static readonly Regex TelCn = new(@"^0\d{2,3}-?\d{7,8}$", Opt);
    /// <summary>
    /// 身份证号（大陆18位，与前端 RegexPatterns.ID_CARD_18 对齐）。
    /// </summary>
    public static readonly Regex IdCard18 = new(@"^[1-9]\d{5}(18|19|20)\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{3}[\dXx]$", Opt);
    /// <summary>
    /// 身份证号（大陆15位，与前端 RegexPatterns.ID_CARD_15 对齐）。
    /// </summary>
    public static readonly Regex IdCard15 = new(@"^[1-9]\d{5}\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{3}$", Opt);
    /// <summary>
    /// 身份证号（中国台湾，与前端 RegexPatterns.ID_CARD_TW 对齐）。
    /// </summary>
    public static readonly Regex IdCardTw = new(@"^[A-Z][12]\d{8}$", Opt);
    /// <summary>
    /// 身份证号（中国香港，与前端 RegexPatterns.ID_CARD_HK 对齐）。
    /// </summary>
    public static readonly Regex IdCardHk = new(@"^[A-Z]{1,2}\d{6}\([0-9A]\)$", Opt);
    /// <summary>
    /// 身份证号（美国 SSN，与前端 RegexPatterns.ID_CARD_US 对齐）。
    /// </summary>
    public static readonly Regex IdCardUs = new(@"^(?!000|666|9\d\d)\d{3}[- ]?(?!00)\d{2}[- ]?(?!0000)\d{4}$", Opt);
    /// <summary>
    /// 身份证号（日本个人番号，与前端 RegexPatterns.ID_CARD_JP 对齐）。
    /// </summary>
    public static readonly Regex IdCardJp = new(@"^\d{12}$", Opt);
    /// <summary>
    /// 统一社会信用代码（与前端 RegexPatterns.UNIFIED_SOCIAL_CREDIT_CODE 对齐）。
    /// </summary>
    public static readonly Regex UnifiedSocialCreditCode = new(@"^[0-9A-HJ-NPQRTUWXY]{2}\d{6}[0-9A-HJ-NPQRTUWXY]{10}$", Opt);
    /// <summary>
    /// 邮政编码（中国大陆，与前端 RegexPatterns.POSTAL_CODE_CN 对齐）。
    /// </summary>
    public static readonly Regex PostalCodeCn = new(@"^[1-9]\d{5}$", Opt);
    /// <summary>
    /// URL 地址（与前端 RegexPatterns.URL 对齐）。
    /// </summary>
    public static readonly Regex Url = new(@"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$", Opt);
    /// <summary>
    /// IPv4 地址（与前端 RegexPatterns.IPV4 对齐）。
    /// </summary>
    public static readonly Regex Ipv4 = new(@"^((25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(25[0-5]|2[0-4]\d|[01]?\d\d?)$", Opt);
    /// <summary>
    /// IPv6 地址（与前端 RegexPatterns.IPV6 对齐）。
    /// </summary>
    public static readonly Regex Ipv6 = new(@"^([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}$", Opt);
    /// <summary>
    /// 用户名（小写字母开头，允许小写字母和数字，4-20位）。
    /// </summary>
    public static readonly Regex UserName = new(@"^[a-z][a-z0-9]{3,19}$", Opt);
    /// <summary>
    /// 登录账号（小写字母开头，4-20 位，与 UserName / 前端 RegexPatterns.LOGIN_USER_NAME / 种子用户 demo 对齐）。
    /// </summary>
    public static readonly Regex LoginUserName = new(@"^[a-z][a-z0-9]{3,19}$", Opt);
    /// <summary>
    /// 租户编码（3 位数字，与前端 RegexPatterns.TENANT_CODE 对齐）。
    /// </summary>
    public static readonly Regex TenantCode = new(@"^\d{3}$", Opt);
    /// <summary>
    /// 真实姓名（中文姓名，2-50个汉字，与前端 RegexPatterns.REAL_NAME 对齐）。
    /// </summary>
    public static readonly Regex RealName = new(@"^[\u4e00-\u9fa5]{2,50}$", Opt);
    /// <summary>
    /// 全名（中文、英文、数字、空格、点、横线，2-100位）。
    /// </summary>
    public static readonly Regex FullName = new(@"^[\u4e00-\u9fa5a-zA-Z0-9\s.\-]{2,100}$", Opt);
    /// <summary>
    /// 昵称（中文、英文、数字、下划线、横线、点，2-20 位）。
    /// </summary>
    public static readonly Regex NickName = new(@"^[\u4e00-\u9fa5a-zA-Z0-9_.-]{2,20}$", Opt);
    /// <summary>
    /// 员工姓名（中文、英文、数字、空格、点、横线，2-80 位；与 TaktEmployee.EmployeeName 对齐）。
    /// </summary>
    public static readonly Regex EmployeeName = new(@"^[\u4e00-\u9fa5a-zA-Z0-9\s.\-]{2,80}$", Opt);
    /// <summary>
    /// 英文名（字母开头结尾，可含数字、空格、点、横线、单引号）。
    /// </summary>
    public static readonly Regex EnglishName = new(@"^[a-zA-Z]([a-zA-Z0-9\s.\-']{0,98}[a-zA-Z])?$", Opt);
    /// <summary>
    /// 姓/名英文规则（首字母大写，可含字母数字，1-100位）。
    /// </summary>
    public static readonly Regex NameEn = new(@"^[A-Z][A-Za-z0-9]{0,99}$", Opt);
    /// <summary>
    /// 姓（last_name）规则：
    /// 支持中文姓氏（1-20位）或英文姓氏（字母开头，可含空格/单引号/短横线，1-50位）。
    /// </summary>
    public static readonly Regex LastName = new(@"^(?:[\u4e00-\u9fa5]{1,20}|[A-Za-z][A-Za-z\s'\-]{0,49})$", Opt);
    /// <summary>
    /// 名（first_name）规则：
    /// 支持中文名字（1-30位）或英文名字（字母开头，可含空格/单引号/短横线，1-50位）。
    /// </summary>
    public static readonly Regex FirstName = new(@"^(?:[\u4e00-\u9fa5]{1,30}|[A-Za-z][A-Za-z\s'\-]{0,49})$", Opt);
    /// <summary>
    /// 密码（强密码）：
    /// 8-20位，必须包含大写字母、小写字母、数字、特殊字符。
    /// </summary>
    public static readonly Regex Password = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$", Opt);
    /// <summary>
    /// 强密码（与 Password 保持一致，兼容既有调用）。
    /// </summary>
    public static readonly Regex StrongPassword = Password;
    /// <summary>
    /// 中文字符（至少1个）。
    /// </summary>
    public static readonly Regex Chinese = new(@"^[\u4e00-\u9fa5]+$", Opt);
    /// <summary>
    /// 英文字母（至少1个）。
    /// </summary>
    public static readonly Regex English = new(@"^[a-zA-Z]+$", Opt);
    /// <summary>
    /// 数字（至少1位）。
    /// </summary>
    public static readonly Regex Number = new(@"^\d+$", Opt);
    /// <summary>
    /// 整数（可负数）。
    /// </summary>
    public static readonly Regex Integer = new(@"^-?\d+$", Opt);
    /// <summary>
    /// 正整数（不含0）。
    /// </summary>
    public static readonly Regex PositiveInteger = new(@"^[1-9]\d*$", Opt);
    /// <summary>
    /// 非负整数（含0）。
    /// </summary>
    public static readonly Regex NonNegativeInteger = new(@"^\d+$", Opt);
    /// <summary>
    /// 浮点数（可负）。
    /// </summary>
    public static readonly Regex Float = new(@"^-?\d+\.\d+$", Opt);
    /// <summary>
    /// 正浮点数。
    /// </summary>
    public static readonly Regex PositiveFloat = new(@"^[1-9]\d*\.\d+|0\.\d*[1-9]\d*$", Opt);
    /// <summary>
    /// 非负浮点数（含0）。
    /// </summary>
    public static readonly Regex NonNegativeFloat = new(@"^\d+\.\d+|0$", Opt);
    /// <summary>
    /// 日期（YYYY-MM-DD）。
    /// </summary>
    public static readonly Regex Date = new(@"^\d{4}-\d{2}-\d{2}$", Opt);
    /// <summary>
    /// 时间（HH:mm:ss）。
    /// </summary>
    public static readonly Regex Time = new(@"^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$", Opt);
    /// <summary>
    /// 日期时间（YYYY-MM-DD HH:mm:ss）。
    /// </summary>
    public static readonly Regex DateTimePattern = new(@"^\d{4}-\d{2}-\d{2}\s+([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$", Opt);
    /// <summary>
    /// 银行卡号（16-19位数字）。
    /// </summary>
    public static readonly Regex BankCard = new(@"^\d{16,19}$", Opt);
    /// <summary>
    /// QQ 号（5-11位数字）。
    /// </summary>
    public static readonly Regex Qq = new(@"^[1-9]\d{4,10}$", Opt);
    /// <summary>
    /// 微信号（6-20位，字母数字下划线减号）。
    /// </summary>
    public static readonly Regex Wechat = new(@"^[a-zA-Z0-9_-]{6,20}$", Opt);
    /// <summary>
    /// 中国大陆车牌号。
    /// </summary>
    public static readonly Regex LicensePlateCn = new(@"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领][A-Z][A-HJ-NP-Z0-9]{4,5}[A-HJ-NP-Z0-9挂学警港澳]$", Opt);
    /// <summary>
    /// 美国车牌号（常见格式）：
    /// 1-7位字母数字，可选连接符后缀。
    /// </summary>
    public static readonly Regex LicensePlateUs = new(@"^[A-Z0-9]{1,7}(?:[-\s][A-Z0-9]{1,7})?$", Opt);
    /// <summary>
    /// 日本车牌号（常见简化格式）：
    /// 支持数字+假名+数字，或字母数字组合。
    /// </summary>
    public static readonly Regex LicensePlateJp = new(@"^[0-9]{2,3}[ぁ-んァ-ヶ][0-9]{4}$|^[A-Z0-9]{2,4}[-\s]?[A-Z0-9]{2,4}$", Opt);
    /// <summary>
    /// 中国台湾车牌号（常见格式）：
    /// 字母在前或数字在前，允许连接符。
    /// </summary>
    public static readonly Regex LicensePlateTw = new(@"^[A-Z]{2,3}[-\s]?[0-9]{3,4}$|^[0-9]{3,4}[-\s]?[A-Z]{2,3}$", Opt);
    /// <summary>
    /// 中国香港车牌号（常见格式）：
    /// 1-2位字母 + 1-4位数字。
    /// </summary>
    public static readonly Regex LicensePlateHk = new(@"^[A-Z]{1,2}\d{1,4}$", Opt);
    /// <summary>
    /// MAC 地址。
    /// </summary>
    public static readonly Regex MacAddress = new(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$", Opt);
    /// <summary>
    /// 十六进制颜色值。
    /// </summary>
    public static readonly Regex HexColor = new(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", Opt);
    /// <summary>
    /// 版本号（如 1.0.0）。
    /// </summary>
    public static readonly Regex Version = new(@"^\d+\.\d+\.\d+$", Opt);
    /// <summary>
    /// 文件扩展名（含点）。
    /// </summary>
    public static readonly Regex FileExtension = new(@"\.([a-zA-Z0-9]+)$", Opt);
    /// <summary>
    /// 通用编码（字母数字下划线横线，3-50位）。
    /// </summary>
    public static readonly Regex Code = new(@"^[a-zA-Z0-9_-]{3,50}$", Opt);
    /// <summary>
    /// 物料编码（字母数字下划线横线点，3-50位）。
    /// </summary>
    public static readonly Regex MaterialCode = new(@"^[a-zA-Z0-9_.-]{3,50}$", Opt);
    /// <summary>
    /// 文档编码（字母数字下划线横线，3-50位）。
    /// </summary>
    public static readonly Regex DocumentCode = new(@"^[a-zA-Z0-9_-]{3,50}$", Opt);
    /// <summary>
    /// 订单编码（字母开头，3-50位）。
    /// </summary>
    public static readonly Regex OrderCode = new(@"^[a-zA-Z][a-zA-Z0-9-]{2,49}$", Opt);
    /// <summary>
    /// 批次编码（字母开头，3-50位）。
    /// </summary>
    public static readonly Regex BatchCode = new(@"^[a-zA-Z][a-zA-Z0-9-]{2,49}$", Opt);
    /// <summary>
    /// 序列号（字母数字，3-50位）。
    /// </summary>
    public static readonly Regex SerialNumber = new(@"^[a-zA-Z0-9]{3,50}$", Opt);
    /// <summary>
    /// 带日期编码（YYYYMMDD + 后缀）。
    /// </summary>
    public static readonly Regex CodeWithDate = new(@"^\d{8}[a-zA-Z0-9]{1,20}$", Opt);
    /// <summary>
    /// 带前缀编码（前缀-序号）。
    /// </summary>
    public static readonly Regex CodeWithPrefix = new(@"^[a-zA-Z]{2,10}-[a-zA-Z0-9]{1,30}$", Opt);
    /// <summary>
    /// 带分隔符编码（下划线或横线分隔多段）。
    /// </summary>
    public static readonly Regex CodeWithSeparator = new(@"^[a-zA-Z0-9]+([_-][a-zA-Z0-9]+){1,9}$", Opt);
    /// <summary>
    /// 数字编码（3-20位）。
    /// </summary>
    public static readonly Regex NumericCode = new(@"^\d{3,20}$", Opt);
    /// <summary>
    /// 字母编码（3-20位）。
    /// </summary>
    public static readonly Regex AlphabeticCode = new(@"^[a-zA-Z]{3,20}$", Opt);
    /// <summary>
    /// 大写字母编码（3-20位）。
    /// </summary>
    public static readonly Regex UppercaseCode = new(@"^[A-Z]{3,20}$", Opt);
    /// <summary>
    /// 小写字母编码（3-20位）。
    /// </summary>
    public static readonly Regex LowercaseCode = new(@"^[a-z]{3,20}$", Opt);
    /// <summary>
    /// 菜单编码（字母开头，3-200位）。
    /// </summary>
    public static readonly Regex MenuCode = new(@"^[a-zA-Z][a-zA-Z0-9_-]{2,199}$", Opt);
    /// <summary>
    /// 国际化键（小写字母开头，点分格式）。
    /// </summary>
    public static readonly Regex L10nKey = new(@"^[a-z][a-z0-9.]{1,198}[a-z0-9]$", Opt);
    /// <summary>
    /// 角色编码（字母开头，3-50位）。
    /// </summary>
    public static readonly Regex RoleCode = new(@"^[a-zA-Z][a-zA-Z0-9_-]{2,49}$", Opt);
    /// <summary>
    /// 部门编码（字母开头，3-50位）。
    /// </summary>
    public static readonly Regex DeptCode = new(@"^[a-zA-Z][a-zA-Z0-9_-]{2,49}$", Opt);
    /// <summary>
    /// 岗位编码（字母开头，3-50位）。
    /// </summary>
    public static readonly Regex PostCode = new(@"^[a-zA-Z][a-zA-Z0-9_-]{2,49}$", Opt);
    /// <summary>
    /// 工厂代码（4位，第1位大写字母或数字，第2位数字，第3-4位固定00）。
    /// </summary>
    public static readonly Regex PlantCode = new(@"^[A-Z0-9]\d00$", Opt);
    /// <summary>
    /// 公司代码（4位，数字或大写字母开头，形如 2300）。
    /// </summary>
    public static readonly Regex CompanyCode = new(@"^\d{2}00$", Opt);
    /// <summary>
    /// 利润中心编码（数值型字符，最大8位）。
    /// </summary>
    public static readonly Regex ProfitCenterCode = new(@"^.{1,4}$", Opt);
    /// <summary>
    /// 成本要素编码（数值型字符，最大8位）。
    /// </summary>
    public static readonly Regex CostElementCode = new(@"^\d{1,8}$", Opt);
    /// <summary>
    /// 成本中心编码（数值型字符，最大8位）。
    /// </summary>
    public static readonly Regex CostCenterCode = new(@"^\d{1,8}$", Opt);
    /// <summary>
    /// 会计科目编码（数值型字符，最大8位）。
    /// </summary>
    public static readonly Regex TitleCode = new(@"^\d{1,8}$", Opt);
    /// <summary>
    /// 资产编码（数值型字符，最大8位）。
    /// </summary>
    public static readonly Regex AssetCode = new(@"^\d{1,8}$", Opt);
    /// <summary>
    /// 权限标识（module:resource:action）。
    /// </summary>
    public static readonly Regex Permission = new(@"^[a-z][a-z0-9]*:[a-z0-9]+:[a-z0-9]+$", Opt);
    /// <summary>
    /// 空白字符（全空或空字符串）。
    /// </summary>
    public static readonly Regex WhiteSpace = new(@"^\s*$", Opt);
    /// <summary>
    /// 非空白字符（至少包含一个非空白字符）。
    /// </summary>
    public static readonly Regex NonWhiteSpace = new(@"\S", Opt);
    /// <summary>
    /// 全角字符：全角空格、全角英数字、全角标点、全角货币符。保存时命中即拒绝。
    /// </summary>
    public static readonly Regex FullWidth = new(@"[\u3000\uFF01-\uFF60\uFFE0-\uFFE6]", Opt);

    /// <summary>
    /// 保存文本是否含全角字符。空值视为不含。
    /// </summary>
    /// <param name="value">待保存文本</param>
    /// <returns>含全角字符时为 true</returns>
    public static bool ContainsFullWidth(string? value)
    {
        return !string.IsNullOrEmpty(value) && FullWidth.IsMatch(value);
    }

    /// <summary>
    /// 通用匹配入口：空值返回 false，其他值按指定正则进行 Trim 后匹配。
    /// </summary>
    public static bool IsMatch(Regex regex, string? value)
    {
        ArgumentNullException.ThrowIfNull(regex);
        return !string.IsNullOrWhiteSpace(value) && regex.IsMatch(value.Trim());
    }

    /// <summary>
    /// 是否为有效邮箱（RFC 5322 常用子集，6–100 位）。
    /// </summary>
    public static bool IsValidEmail(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var trimmed = value.Trim();
        if (trimmed.Length < EmailMinLength || trimmed.Length > EmailMaxLength)
        {
            return false;
        }

        return IsMatch(Email, trimmed);
    }

    /// <summary>
    /// 是否为有效中国大陆手机号。
    /// </summary>
    public static bool IsValidPhoneCn(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var trimmed = value.Trim();
        return trimmed.Length == PhoneCnLength && IsMatch(PhoneCn, trimmed);
    }

    /// <summary>
    /// 是否为有效中国台湾手机号。
    /// </summary>
    public static bool IsValidPhoneTw(string? value) => IsMatch(PhoneTw, value);

    /// <summary>
    /// 是否为有效中国香港手机号（8 位）。
    /// </summary>
    public static bool IsValidPhoneHk(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }
        var trimmed = value.Trim();
        return trimmed.Length == PhoneHkLength && IsMatch(PhoneHk, trimmed);
    }

    /// <summary>
    /// 是否为有效美国手机号（10 位）。
    /// </summary>
    public static bool IsValidPhoneUs(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }
        var trimmed = value.Trim();
        return trimmed.Length == PhoneUsLength && IsMatch(PhoneUs, trimmed);
    }

    /// <summary>
    /// 是否为有效日本手机号（11 位）。
    /// </summary>
    public static bool IsValidPhoneJp(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }
        var trimmed = value.Trim();
        return trimmed.Length == PhoneJpLength && IsMatch(PhoneJp, trimmed);
    }

    /// <summary>
    /// 按公司默认文化 BCP47 解析手机号校验所用文化（未知时回退 en-US）。
    /// </summary>
    public static string ResolvePhoneCultureCode(string? cultureCode)
    {
        var normalized = (cultureCode ?? string.Empty).Trim();
        return normalized switch
        {
            "zh-CN" or "zh-HK" or "en-US" or "ja-JP" => normalized,
            _ => "en-US"
        };
    }

    /// <summary>
    /// 按公司默认文化返回手机号格式说明 I18n 键（common.tip.phone.*，表单下方提示）。
    /// </summary>
    public static string GetPhoneTipI18nKeyByCulture(string? cultureCode)
    {
        return ResolvePhoneCultureCode(cultureCode) switch
        {
            "zh-CN" => PhoneTipI18nKeyCn,
            "zh-HK" => PhoneTipI18nKeyHk,
            "ja-JP" => PhoneTipI18nKeyJp,
            _ => PhoneTipI18nKeyUs
        };
    }

    /// <summary>
    /// 按公司默认文化返回手机号最大长度。
    /// </summary>
    public static int GetPhoneMaxLengthByCulture(string? cultureCode)
    {
        return ResolvePhoneCultureCode(cultureCode) switch
        {
            "zh-CN" => PhoneCnLength,
            "zh-HK" => PhoneHkLength,
            "ja-JP" => PhoneJpLength,
            _ => PhoneUsLength
        };
    }

    /// <summary>
    /// 按公司默认文化校验手机号（zh-CN 11 / zh-HK 8 / en-US 10 / ja-JP 11）。
    /// </summary>
    public static bool IsValidPhoneByCulture(string? value, string? cultureCode)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }
        return ResolvePhoneCultureCode(cultureCode) switch
        {
            "zh-CN" => IsValidPhoneCn(value),
            "zh-HK" => IsValidPhoneHk(value),
            "ja-JP" => IsValidPhoneJp(value),
            _ => IsValidPhoneUs(value)
        };
    }

    /// <summary>
    /// 是否为有效手机号（按公司默认文化；未传文化时回退 en-US）。
    /// </summary>
    public static bool IsValidPhone(string? value, string? cultureCode = null)
        => IsValidPhoneByCulture(value, cultureCode);

    /// <summary>
    /// 是否为有效登录账号（4-20 位小写字母开头）。
    /// </summary>
    public static bool IsValidLoginUserName(string? value) => IsMatch(LoginUserName, value);

    /// <summary>
    /// 是否为有效租户编码（3 位数字）。
    /// </summary>
    public static bool IsValidTenantCode(string? value) => IsMatch(TenantCode, value);

    /// <summary>
    /// 是否为有效用户昵称（可为空；非空时 2–20 位且符合 NickName）。
    /// </summary>
    public static bool IsValidUserNickName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        return IsMatch(NickName, value);
    }

    /// <summary>
    /// 是否为有效员工姓名（非空时 2–80 位且符合 EmployeeName）。
    /// </summary>
    public static bool IsValidEmployeeName(string? value) => IsMatch(EmployeeName, value);

    /// <summary>
    /// 是否为有效用户名。
    /// </summary>
    public static bool IsValidUserName(string? value) => IsMatch(UserName, value);

    /// <summary>
    /// 是否为有效密码（强密码规则）。
    /// </summary>
    public static bool IsValidPassword(string? value) => IsMatch(Password, value);

    /// <summary>
    /// 是否为有效强密码。
    /// </summary>
    public static bool IsValidStrongPassword(string? value) => IsMatch(StrongPassword, value);

    /// <summary>
    /// 是否为有效全名。
    /// </summary>
    public static bool IsValidFullName(string? value) => IsMatch(FullName, value);

    /// <summary>
    /// 是否为有效英文名。
    /// </summary>
    public static bool IsValidEnglishName(string? value) => IsMatch(EnglishName, value);

    /// <summary>
    /// 是否为有效姓（last_name）。
    /// </summary>
    public static bool IsValidLastName(string? value) => IsMatch(LastName, value);

    /// <summary>
    /// 是否为有效名（first_name）。
    /// </summary>
    public static bool IsValidFirstName(string? value) => IsMatch(FirstName, value);

    /// <summary>
    /// 是否为有效中国大陆二代身份证号（18位）：格式 + 出生日期 + 校验码。
    /// </summary>
    public static bool IsValidChineseIdCard(string? idCard)
    {
        if (string.IsNullOrWhiteSpace(idCard))
            return false;

        var code = idCard.Trim().ToUpperInvariant();
        if (!IdCard18.IsMatch(code))
            return false;

        var birthText = code.Substring(6, 8);
        if (!DateTime.TryParseExact(
                birthText,
                "yyyyMMdd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _))
        {
            return false;
        }

        var weights = new[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        var checkCodes = new[] { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };
        var sum = 0;
        for (var i = 0; i < 17; i++)
        {
            if (!char.IsDigit(code[i]))
                return false;
            sum += (code[i] - '0') * weights[i];
        }
        return code[17] == checkCodes[sum % 11];
    }

    /// <summary>
    /// 是否为有效中国台湾身份证号。
    /// </summary>
    public static bool IsValidTaiwanIdCard(string? value) => !string.IsNullOrWhiteSpace(value) && IdCardTw.IsMatch(value.Trim().ToUpperInvariant());

    /// <summary>
    /// 是否为有效中国香港身份证号。
    /// </summary>
    public static bool IsValidHongKongIdCard(string? value) => !string.IsNullOrWhiteSpace(value) && IdCardHk.IsMatch(value.Trim().ToUpperInvariant());

    /// <summary>
    /// 是否为有效美国身份证号（SSN）。
    /// </summary>
    public static bool IsValidUsIdCard(string? value) => IsMatch(IdCardUs, value);

    /// <summary>
    /// 是否为有效日本身份证号（个人番号12位）。
    /// </summary>
    public static bool IsValidJapanIdCard(string? value) => IsMatch(IdCardJp, value);

    /// <summary>
    /// 是否为有效身份证号（大陆/台湾/香港/美国/日本）。
    /// </summary>
    public static bool IsValidIdCard(string? value) =>
        IsValidChineseIdCard(value) ||
        (!string.IsNullOrWhiteSpace(value) && IdCard15.IsMatch(value.Trim())) ||
        IsValidTaiwanIdCard(value) ||
        IsValidHongKongIdCard(value) ||
        IsValidUsIdCard(value) ||
        IsValidJapanIdCard(value);

    /// <summary>
    /// 是否为有效中国大陆车牌号。
    /// </summary>
    public static bool IsValidLicensePlateCn(string? value) => IsMatch(LicensePlateCn, value);

    /// <summary>
    /// 是否为有效美国车牌号。
    /// </summary>
    public static bool IsValidLicensePlateUs(string? value) => IsMatch(LicensePlateUs, value);

    /// <summary>
    /// 是否为有效日本车牌号。
    /// </summary>
    public static bool IsValidLicensePlateJp(string? value) => IsMatch(LicensePlateJp, value);

    /// <summary>
    /// 是否为有效中国台湾车牌号。
    /// </summary>
    public static bool IsValidLicensePlateTw(string? value) => IsMatch(LicensePlateTw, value);

    /// <summary>
    /// 是否为有效中国香港车牌号。
    /// </summary>
    public static bool IsValidLicensePlateHk(string? value) => IsMatch(LicensePlateHk, value);

    /// <summary>
    /// 是否为有效车牌号（大陆/美国/日本/台湾/香港）。
    /// </summary>
    public static bool IsValidLicensePlate(string? value) =>
        IsValidLicensePlateCn(value) ||
        IsValidLicensePlateUs(value) ||
        IsValidLicensePlateJp(value) ||
        IsValidLicensePlateTw(value) ||
        IsValidLicensePlateHk(value);

    /// <summary>
    /// 是否为有效工厂代码（4位，数字或大写字母开头）。
    /// </summary>
    public static bool IsValidPlantCode(string? value) => IsMatch(PlantCode, value);

    /// <summary>
    /// 是否为有效公司代码（4位，数字或大写字母开头）。
    /// </summary>
    public static bool IsValidCompanyCode(string? value) => IsMatch(CompanyCode, value);

    /// <summary>
    /// 是否为有效利润中心编码（数值型字符，最大8位）。
    /// </summary>
    public static bool IsValidProfitCenterCode(string? value) => IsMatch(ProfitCenterCode, value);

    /// <summary>
    /// 是否为有效成本要素编码（数值型字符，最大8位）。
    /// </summary>
    public static bool IsValidCostElementCode(string? value) => IsMatch(CostElementCode, value);

    /// <summary>
    /// 是否为有效成本中心编码（数值型字符，最大8位）。
    /// </summary>
    public static bool IsValidCostCenterCode(string? value) => IsMatch(CostCenterCode, value);

    /// <summary>
    /// 是否为有效会计科目编码（数值型字符，最大8位）。
    /// </summary>
    public static bool IsValidTitleCode(string? value) => IsMatch(TitleCode, value);

    /// <summary>
    /// 是否为有效资产编码（数值型字符，最大8位）。
    /// </summary>
    public static bool IsValidAssetCode(string? value) => IsMatch(AssetCode, value);
}
