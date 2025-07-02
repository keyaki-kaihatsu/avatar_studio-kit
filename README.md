<img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/title.png">
<br>

# KeyakiStudio Kit

[English version here (英語版はこちら)](locals/README_en.md)

このプロジェクトは**ケヤキスタジオ**向けに 3D モデルや 3D シーンをエクスポートするためのプロジェクトです。

_こちらの unitypackage をご利用ください。_
<br>
[Unity Package]
https://github.com/keyaki-kaihatsu/avatar_studio-kit/releases

<img src="https://skillicons.dev/icons?i=unity">

# ケヤキスタジオ

ケヤキスタジオはアバターを使って 3D アニメ制作ができるアプリです。全ての登場人物の演技・3D 舞台の編集・撮影など、アニメ制作に必要な工程をアプリで作業できます。
<br>

_App Store_
<img src="https://skillicons.dev/icons?i=apple">
<br>
https://apps.apple.com/app/keyaki-studio/id6447358128

_Google Play_
<img src="https://skillicons.dev/icons?i=androidstudio">
<br>
https://play.google.com/store/apps/details?id=com.keyakikaihatsu.keyakistudio

_Windows & Mac_
<img src="https://skillicons.dev/icons?i=windows,apple">
<br>
[英語版]
https://keyaki-kaihatsu.booth.pm/items/5428965
<br>
[日本語版]
https://keyaki-kaihatsu.booth.pm/items/5381402

# 概要

このプロジェクトを使うと、Unity の AssetBundle 機能を使って Unity のプレハブをケヤキスタジオにインポートすることができます。

# 環境

**Unity**

```bash
 Version   >>>  Unity 2022.3.61f1
 Platform  >>>  iOS, Android, Windows, macOS
```

**VRM**

VRM ファイル（アバター用のファイル）を AssetBundle 化すると、VRM ファイルを利用するよりもパフォーマンスが上がります。VRM ファイルを AssetBundle 化するには**VRM1.0**の unitypackage をインポートしてください。
<br>
https://github.com/vrm-c/UniVRM/releases

# 利用方法

1. Unity プロジェクトを新規作成し、unitypackage をインポートしてください。

2. Project ウィンドウから任意のプレハブを右クリックしてください。メニューから「KeyakiStudio > Asset Build (From Prefab)」を選択してください。

<img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/feature-08.png">
<br>
・ Asset Build (From Prefab)
<br>
<small>
    ∟ プレハブをAssetBundle化します。出力されたフォルダ全体をアプリのassetsフォルダにコピーしてください。
</small>
<br>
<br>
・ Avatar Asset Build (From VRM)
<br>
<small>
    ∟ VRMをAssetBundle化します。出力されたフォルダ全体をアプリのvrm_filesフォルダにコピーしてください。
    <br>
    ∟ VRoidHubやVRMファイルの読み込みのパフォーマンスが悪いと感じた場合にご活用ください。
</small>
<br>
<br>
・ Animation Asset Build (From VRM Prefab + Animation)
<br>
<small>
    ∟ アニメーション付きVRMをAssetBundle化します。出力されたフォルダ全体をアプリのanimationsフォルダにコピーしてください。
    <br>
    ∟ アニメーションファイル（FBX・GLB）の読み込みのパフォーマンスが悪いと感じた場合にご活用ください。
    <br>
    ∟ AnimatorのAnimationControllerのデフォルト状態に任意のアニメーションを設定し、Motion Timeに「MotionTime」パラメータを設定してください。
</small>
<br>
<br>
<img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/feature-07.png">
<br>
・ Convert Animation (From .txt)
<br>
<small>
    ∟ この機能はAssetBuildではありません。アプリで生成したトレースファイル（.txt）をアニメーションに変換する機能です。
    <br>
    ∟ アプリ > 編集画面 > アバターパネル > AnimationClipデータの出力 > トレースボタン（長押し）で生成したトレースファイルをAnimatoinClipおよびAnimation.Controllerに出力します。
</small>
<br>
<br>
<br>
3. ダイアログの入力をしてください。特に設定の必要がなければそのまま「Build」ボタンを押下してください。
<br>
<br>
<div style="display: flex; justify-content: center;">
  <img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/feature-06.png" style="display: block; width: 40%;">
</div>
<br>
・ ビルド出力先を入力してください。
<br>
・ アセット名を入力してください。(任意)
<br>
・ アセットを展開したいデバイスOSを選択してください。(任意)
<br>
・ ZIP圧縮が必要な場合は選択してください。展開先ではZIP解答してください。(任意)
<br>

## アップロード

### フォルダにコピー

プレハブを AssetBuild すると、ダイアログの#1 のフォルダに出力されます。
<br>
出力されたフォルダを AirDrop や GoogleDrive 等でケヤキスタジオアプリをインストール済みのデバイスへ送信してください。
<br>
「Keyaki Studio > assets フォルダ」にフォルダごとコピーしてください。
<br>
参考 → <a href="https://keyaki-kaihatsu.fanbox.cc/posts/7841803" target="_blank">https://keyaki-kaihatsu.fanbox.cc/posts/7841803</a>
<br>
<br>

### マイアセットを利用

マイアセットとはクラウド経由でアプリにアセットをダウンロードできる WEB サイトです。マイアセットサイトにログインしてアクセスコード（\*1）を取得して入力してください。
<br>

<div style="display: flex; justify-content: center;">
  <img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/feature-10.png" style="display: block; width: 40%;">
</div>

<br>
（\*1）マイアセット画面から「XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXX」のアクセスコードを発行してください。
<br>
マイアセット → <a href="https://keyaki-studio.onrender.com/" target="_blank">https://keyaki-studio.onrender.com/</a>
<br>

<div style="border: 1px solid #ccc; border-radius: 12px; overflow: hidden; display: inline-block; padding: 12px 0px;">
  <img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/feature-11.png" style="display: block; border-radius: 12px;">
</div>
<br>
<br>

# 注意

- スクリプトやシェーダーを AssetBuild 化することはできません。
- 同名のアセット名を使うことはできません。

<br>

# KEYAKI STUDIO - My Assets

KEYAKI STUDI MyAssets は PC で AssetBuild した 3D コンテンツを iPhone/Android のアプリにインポートするときに便利な Web サイトです。このサイトを経由して 3D コンテンツを PC からアプリにトランスポートすることができます。
<br>
<br>
<img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/feature-09.png">
<br>
※ サイトにアクセスする方法
<br>
アプリの TOP 画面の"マイアセット"でマイアセット一覧を開き、"マイアページ"ボタンを押下してください。

# 運営

- Yuta Ueda / ケヤキ開発
- https://twitter.com/keyaki_kaihatsu

# ライセンス

"KeyakiStudio Kit" is under [MIT license](https://en.wikipedia.org/wiki/MIT_License).
