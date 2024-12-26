<img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/title.png">
<br>

# KeyakiStudio Kit

[English version here (英語版はこちら)](locals/README_en.md)

このプロジェクトは**ケヤキスタジオ**向けに3Dモデルや3Dシーンをエクスポートするためのプロジェクトです。

*こちらのunitypackageをご利用ください。*
<br>
[Unity Package]
https://github.com/keyaki-kaihatsu/avatar_studio-kit/releases

<img src="https://skillicons.dev/icons?i=unity">

# ケヤキスタジオ

ケヤキスタジオはアバターを使って3Dアニメ制作ができるアプリです。全ての登場人物の演技・3D舞台の編集・撮影など、アニメ制作に必要な工程をアプリで作業できます。
<br>

*App Store*
<img src="https://skillicons.dev/icons?i=apple">
<br>
https://apps.apple.com/app/keyaki-studio/id6447358128

*Google Play*
<img src="https://skillicons.dev/icons?i=androidstudio">
<br>
https://play.google.com/store/apps/details?id=com.keyakikaihatsu.keyakistudio

*Windows & Mac*
<img src="https://skillicons.dev/icons?i=windows,apple">
<br>
[英語版]
https://keyaki-kaihatsu.booth.pm/items/5428965
<br>
[日本語版]
https://keyaki-kaihatsu.booth.pm/items/5381402　

# 概要

このプロジェクトを使うと、UnityのAssetBundle機能を使ってUnityのプレハブをケヤキスタジオにインポートすることができます。

# 環境

**Unity**
```bash
 Version   >>>  Unity 6 (6000.0.25f1)
 Platform  >>>  iOS, Android, Windows, macOS
```

**VRM**

VRMファイル（アバター用のファイル）をAssetBundle化すると、VRMファイルを利用するよりもパフォーマンスが上がります。VRMファイルをAssetBundle化するには**VRM1.0**のunitypackageをインポートしてください。
<br>
https://github.com/vrm-c/UniVRM/releases

# 利用方法

1. Unityプロジェクトを新規作成し、unitypackageをインポートしてください。

2. Projectウィンドウから任意のプレハブを右クリックしてください。メニューから「KeyakiStudio > Asset Build (From Prefab)」を選択してください。

<img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/feature-05.png">
<br>
<br>

* Asset Build (From Prefab)
<br>
<small>
    ∟ プレハブをAssetBundle化します。アプリのassetsフォルダにコピーしてください。
</small>
* Avatar Asset Build (From VRM)
<br>
<small>
    ∟ VRMをAssetBundle化します。アプリのvrm_filesフォルダにコピーしてください。
    <br>
    ∟ VRoidHub連携やVRMファイルを利用したときに、パフォーマンスが悪い場合にご活用ください。
</small>
* Animation Asset Build (From VRM Prefab + Animation)
<br>
<small>
    ∟ アニメーション付きVRMをAssetBundle化します。アプリのanimationsフォルダにコピーしてください。
    <br>
    ∟ AnimatorのAnimationControllerのデフォルト状態に任意のアニメーションを設定し、Motion Timeに「MotionTime」パラメータを設定してください。
    <br>
    ∟ アニメーションファイル（FBX・GLB）を利用したときに、パフォーマンスが悪い場合にご活用ください。
</small>
<br>
<img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/feature-07.png">
<br>
* Export Animation > Create Animator Controller
<br>
<small>
    ∟ この機能はAssetBuildではありません。
    <br>
    ∟ アプリ > 編集画面 > アバターパネル > AnimationClipデータの出力 > トレースボタン（長押し）で出力したアバターのアニメーションファイル（JSON）をAnimatoinClipおよびAnimation.Controllerに変換します。
    <br>
    ∟ AnimatorのAnimationControllerのデフォルト状態に任意のアニメーションを設定し、Motion Timeに「MotionTime」パラメータを設定してください。
    <br>
    ∟ アニメーションファイル（FBX・GLB）を利用したときに、パフォーマンスが悪い場合にご活用ください。
</small>

3. ダイアログの入力をしてください。特に設定の必要がなければそのまま「Build」ボタンを押下してください。
<br>

<img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/feature-06.png">
<br>
<br>

* ビルド出力先を入力してください。
* アセット名を入力してください。(任意)
* アセットを展開したいデバイスOSを選択してください。(任意)
* ZIP圧縮が必要な場合は選択してください。展開先ではZIP解答してください。(任意)

4. プレハブをAssetBuild化すると、ダイアログの#1のフォルダに出力されます。出力されたフォルダごとAirDropやGoogleDrive等でケヤキスタジオアプリをインストール済みのデバイスへ送信してください。「Keyaki Studio > assetsフォルダ」にフォルダごとコピーしてください。
<br>
参考 >>> https://keyaki-kaihatsu.fanbox.cc/posts/7841803
<br>

# 注意

* スクリプトやシェーダーをAssetBuild化することはできません。
* 同名のアセット名を使うことはできません。

# 運営

* Yuta Ueda / ケヤキ開発
* https://twitter.com/keyaki_kaihatsu

# ライセンス

"KeyakiStudio Kit" is under [MIT license](https://en.wikipedia.org/wiki/MIT_License).