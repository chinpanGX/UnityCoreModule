# Unityサードパーティーライブラリの導入手順

# 1
Edit > ProjectSetting > Package Manager > "Scoped Registries" に、以下の内容を各項目に記載する

    name  :
        OpenUPM
    URL   :
        https://package.openupm.com
    Scope(s) :
        org.nuget
        com.cysharp
        jp.hadashikick.vcontainer


# 2
Windows > Package Managerから必要なライブラリをインストール

    UniTask
    R3
    R3(NuGet)
    MessagePipe,
    MessagePipe.VContainer
    VContainer
    UnityScreenSystem
    ZString


# 3
CoreModule/Packages/以下のライブラリについて

    １．利用するプロジェクトへコピー
    ２．manifest.jsonへ記載

    サンプル
    {
        "personalpackage": "file:../../Packages/PersonalPackage",
    }


