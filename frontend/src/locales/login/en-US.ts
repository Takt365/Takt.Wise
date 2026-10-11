// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/login
// 文件名称：en-US.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：login page static copy; keys login.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    tenant: {
      code: "Tenant",
    },
    company: {
      code: "Company",
    },
    forgot: {
      password: "Forgot password?",
      title: "Forgot Password",
      subtitle: "Enter your email address and we will send you a password reset link",
      submit: "Reset Password",
      backtologin: "Back to Sign In",
      fail: "Failed to send reset email. Please check your email address",
      emailnotregistered: "This email address is not registered. Please confirm!",
      email: {
        sent: "Reset email sent. Please check your inbox",
      },
      reset: {
        unavailable: "Self-service password reset is not available for this account. Please contact an administrator",
      },
      steps: {
        email: "Enter Email",
        captcha: "Security Check",
        done: "Email Sent",
      },
    },
    field: {
      tenant: {
        code: {
          placeholder: "Enter tenant code",
        },
      },
      userName: {
        label: "UserName",
        placeholder: "Enter UserName",
        or: {
          email: {
            label: "UserName or email",
            placeholder: "Enter UserName or email",
          },
        },
      },
      password: {
        label: "Password",
        placeholder: "Enter password",
      },
      culture: {
        label: "Language",
        placeholder: "Enter a valid tenant code first",
      },
      email: {
        label: "Email",
        placeholder: "Enter email",
      },
      phone: {
        label: "Phone",
        placeholder: "Enter phone number",
      },
    },
    validate: {
      tenant: {
        required: "Please enter tenant code",
        invalid: "Tenant code must be 3 digits",
      },
      userName: {
        required: "Please enter UserName",
        invalid: "Invalid UserName format",
        or: {
          email: {
            required: "Please enter UserName or email",
            invalid: "Invalid UserName or email format",
            too: {
              short: "UserName or email must be at least {min} characters",
              long: "UserName or email must not exceed {max} characters",
            },
          },
        },
      },
      password: {
        required: "Please enter password",
        weak: "Password is too weak",
      },
      email: {
        required: "Please enter email",
        invalid: "Invalid email format",
        too: {
          short: "Email must be at least {min} characters",
          long: "Email must not exceed {max} characters",
        },
      },
      phone: {
        required: "Please enter phone number",
        invalid: "Invalid phone number format",
      },
      captcha: {
        required: "Please complete security verification",
      },
    },
    login: {
      title: "Welcome",
      rememberme: "Remember Me",
      login: "Sign In",
      logout: "Sign Out",
      noaccountregister: "New to Takt365? {register}",
    },
    sign: {
      title: "Sign up",
      subtitle: "Create your account to get started",
      hasaccount: "Already have an account? Sign in",
      success: "Registration successful. Please sign in",
      successinitialpassword: "Registration successful. Please check your email for the initial password.",
      fail: "Registration failed",
      steps: {
        info: "Account Info",
        captcha: "Security Check",
        done: "Complete",
      },
    },
    message: {
      fail: "Login failed",
      credentials: {
        incorrect: "Incorrect UserName or password",
      },
      account: {
        locked: "This account is locked. Please try again later",
      },
      tenant: {
        no: {
          access: "You do not have permission to sign in to the selected tenant",
        },
        not: {
          found: "Tenant does not exist",
        },
        validate: {
          fail: "Failed to validate tenant. Please try again later.",
        },
        database: {
          missing: "Tenant {tenantCode} database does not exist ({databaseName}). Set Init.InitDb to true and restart the backend.",
          login: {
            failed: "Cannot log in to SQL Server for tenant {tenantCode} ({databaseName}). Check the connection string and sa password.",
          },
        },
        table: {
          missing: "Tenant {tenantCode} database {databaseName} is reachable but business tables are missing. Enable Init.InitDb and SeedData as needed.",
        },
        options: {
          fail: "Failed to load tenant list. Please try again later.",
        },
      },
      user: {
        not: {
          found: "User does not exist or is disabled",
        },
      },
      default: {
        company: {
          not: {
            found: "No default login company configured",
          },
        },
      },
      public: {
        key: {
          missing: "Unable to load login encryption key. Please refresh and try again.",
        },
      },
      encrypt: {
        fail: "Failed to encrypt password. Please try again.",
      },
    },
    log: {
      sign: {
        in: {
          session: {
            start: "Establishing cookie session",
            success: "Cookie session ready; starting OAuth authorization",
          },
        },
      },
      verify: {
        password: {
          start: "Verifying password",
          success: "Password verified",
          captcha: {
            required: "Password verified; captcha required",
          },
        },
      },
    },
    captcha: {
      title: "Security Verification",
      drag: {
        hint: "Drag the slider to complete the puzzle",
      },
      behavior: {
        hint: "Drag the slider to the correct position",
      },
      slide: {
        to: {
          target: "Drag the slider to target position {position}%",
        },
      },
      success: "Verified",
      cancel: "Cancel",
      confirm: "Confirm",
      typerequired: "Captcha type not returned from server, please check backend configuration",
    },
    callback: {
      title: "Login Callback",
      processing: "Completing sign-in...",
      missing: {
        code: "Authorization code is missing",
      },
      state: {
        mismatch: "State validation failed",
      },
      pkce: {
        missing: "PKCE verifier is missing",
      },
      fail: "Login failed",
      log: {
        oauth: {
          error: "Authorization callback error: {description}",
        },
        exchange: {
          start: "Exchanging authorization code for tokens",
          success: "Tokens received; user profile loaded",
        },
        rbac: {
          ready: "RBAC ready after OAuth callback",
        },
      },
    },
    showcase: {
      split: {
        practical: "Practical",
        simple: "Simple",
        flexible: "Flexible",
      },
    },
  },
};
