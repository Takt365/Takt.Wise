// ========================================
// 项目名称：节拍智控·TaktWise
// 命名空间：frontend/src/locales/login
// 文件名称：ja-JP.ts
// 创建时间：2026-06-16
// 创建人：Takt365(Cursor AI)
// 功能描述：login 页面静态文案；引用键 login.page.*
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

export default {
  page: {
    tenant: {
      code: "テナント",
    },
    company: {
      code: "会社",
    },
    forgot: {
      password: "パスワードをお忘れですか？",
      title: "パスワードをお忘れの方",
      subtitle: "メールアドレスを入力してください。パスワード再設定リンクをお送りします",
      submit: "パスワードをリセット",
      backtologin: "ログインに戻る",
      fail: "リセットメールの送信に失敗しました。メールアドレスを確認してください",
      emailnotregistered: "このメールアドレスは登録されていません。ご確認ください",
      email: {
        sent: "リセットメールを送信しました。受信トレイをご確認ください",
      },
      reset: {
        unavailable: "このアカウントは自助パスワードリセットに対応していません。管理者にお問い合わせください",
      },
      steps: {
        email: "メール入力",
        captcha: "セキュリティ確認",
        done: "送信完了",
      },
    },
    field: {
      tenant: {
        code: {
          placeholder: "テナントコードを入力",
        },
      },
      userName: {
        label: "ユーザー名",
        placeholder: "ユーザー名を入力",
        or: {
          email: {
            label: "ユーザー名またはメールアドレス",
            placeholder: "ユーザー名またはメールアドレスを入力",
          },
        },
      },
      password: {
        label: "パスワード",
        placeholder: "パスワードを入力",
      },
      culture: {
        label: "言語",
        placeholder: "有効なテナントコードを先に入力してください",
      },
      email: {
        label: "メールアドレス",
        placeholder: "メールアドレスを入力",
      },
      phone: {
        label: "携帯電話番号",
        placeholder: "携帯電話番号を入力",
      },
    },
    validate: {
      tenant: {
        required: "テナントコードを入力してください",
        invalid: "テナントコードは3桁の数字である必要があります",
      },
      userName: {
        required: "ユーザー名を入力してください",
        invalid: "ユーザー名の形式が正しくありません",
        or: {
          email: {
            required: "ユーザー名またはメールアドレスを入力してください",
            invalid: "ユーザー名またはメールアドレスの形式が正しくありません",
            too: {
              short: "ユーザー名またはメールアドレスは {min} 文字以上である必要があります",
              long: "ユーザー名またはメールアドレスは {max} 文字以内である必要があります",
            },
          },
        },
      },
      password: {
        required: "パスワードを入力してください",
        weak: "パスワードの強度が不足しています",
      },
      email: {
        required: "メールアドレスを入力してください",
        invalid: "メールアドレスの形式が正しくありません",
        too: {
          short: "メールアドレスは {min} 文字以上である必要があります",
          long: "メールアドレスは {max} 文字以内である必要があります",
        },
      },
      phone: {
        required: "携帯電話番号を入力してください",
        invalid: "携帯電話番号の形式が正しくありません",
      },
      captcha: {
        required: "セキュリティ認証を完了してください",
      },
    },
    login: {
      title: "ようこそ",
      rememberme: "ログイン状態を保持",
      login: "ログイン",
      logout: "ログアウト",
      noaccountregister: "Takt365 は初めてですか？{register}",
    },
    sign: {
      title: "新規登録",
      subtitle: "アカウントを作成して始めましょう",
      hasaccount: "すでにアカウントをお持ちですか？ログイン",
      success: "登録が完了しました。ログインしてください",
      successinitialpassword: "登録が完了しました。初期パスワードはメールをご確認ください",
      fail: "登録に失敗しました",
      steps: {
        info: "情報入力",
        captcha: "セキュリティ確認",
        done: "登録完了",
      },
    },
    message: {
      fail: "ログインに失敗しました",
      credentials: {
        incorrect: "ユーザー名またはパスワードが正しくありません",
      },
      account: {
        locked: "アカウントがロックされています。しばらくしてから再試行してください",
      },
      tenant: {
        no: {
          access: "選択したテナントにログインする権限がありません",
        },
        not: {
          found: "テナントが存在しません",
        },
        validate: {
          fail: "テナントの検証に失敗しました。しばらくしてから再試行してください",
        },
        database: {
          missing: "テナント {tenantCode} の業務 DB が存在しません（{databaseName}）。Init.InitDb を true にして再起動してください。",
          login: {
            failed: "テナント {tenantCode} で SQL Server にログインできません（{databaseName}）。接続文字列と sa パスワードを確認してください。",
          },
        },
        table: {
          missing: "テナント {tenantCode} の DB {databaseName} には接続できますが、業務テーブルがありません。Init.InitDb / SeedData を有効にしてください。",
        },
        options: {
          fail: "テナント一覧の読み込みに失敗しました。しばらくしてから再試行してください",
        },
      },
      user: {
        not: {
          found: "ユーザーが存在しないか、無効になっています",
        },
      },
      default: {
        company: {
          not: {
            found: "デフォルトのログイン会社が設定されていません",
          },
        },
      },
      public: {
        key: {
          missing: "ログイン暗号化公開鍵を取得できません。ページを更新してください",
        },
      },
      encrypt: {
        fail: "パスワードの暗号化に失敗しました。再試行してください",
      },
    },
    log: {
      sign: {
        in: {
          session: {
            start: "Cookie セッションの確立を開始",
            success: "Cookie セッション確立完了。OAuth 認可を開始",
          },
        },
      },
      verify: {
        password: {
          start: "パスワード検証を開始",
          success: "パスワード検証成功",
          captcha: {
            required: "パスワード検証成功。認証コードが必要",
          },
        },
      },
    },
    captcha: {
      title: "セキュリティ確認",
      drag: {
        hint: "スライダーをドラッグしてパズルを完成",
      },
      behavior: {
        hint: "スライダーを正しい位置までドラッグ",
      },
      slide: {
        to: {
          target: "スライダーを目標位置 {position}% までドラッグしてください",
        },
      },
      success: "確認完了",
      cancel: "キャンセル",
      confirm: "確認",
      typerequired: "サーバーから認証コードタイプが返されませんでした。バックエンド設定を確認してください",
    },
    callback: {
      title: "ログインコールバック",
      processing: "ログインを完了しています...",
      missing: {
        code: "認可コードがありません",
      },
      state: {
        mismatch: "state の検証に失敗しました",
      },
      pkce: {
        missing: "PKCE 検証パラメータが失われました",
      },
      fail: "ログインに失敗しました",
      log: {
        oauth: {
          error: "認可コールバックエラー：{description}",
        },
        exchange: {
          start: "認可コードをトークンに交換開始",
          success: "トークン取得完了。ユーザープロファイルを読み込み",
        },
        rbac: {
          ready: "OAuth コールバック後 RBAC 準備完了",
        },
      },
    },
    showcase: {
      split: {
        practical: "実践的",
        simple: "シンプル",
        flexible: "柔軟",
      },
    },
  },
};
