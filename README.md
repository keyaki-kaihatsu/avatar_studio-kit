<img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/title.png">
<br>

# AvatarStudio Kit

This project is for export your 3D models or VRM avatars to **Keyaki Studio** (iOS App).

<img src="https://img.shields.io/badge/-Unity-000000.svg?logo=unity&style=plastic">

# Keyaki Studio

'Keyaki Studio' is an editor that allows you to create animations using avatars.
Create dance videos and dramas by performing the three roles of acting, editing, and filming.
<br>

*App Store*
<br>
https://apps.apple.com/app/keyaki-studio/id6447358128

# Features

Using this project, you can export prefabs to 'Keyaki Studio' using Unity's **Asset Bundle** function.

# Requirement

**Unity**
```bash
 Version   >>>  2021.3.22f1
 Platform  >>>  iOS
```

**For VRM avatars**
<br>
Please import unitypackage for VRM1.0.
<br>
https://github.com/vrm-c/UniVRM/releases

# Usage

1. You right-click on any prefab and click on 'Asset Build' from the menu. (*)
<br>
**For VRM avatars**
<br>
(*) 'VRM Build' button.

<img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/feature-01.png">
<br>
<br>

2. Please fill in the input for this window.
<br>

<img src="https://avatar-studio.s3.ap-northeast-1.amazonaws.com/avatar_studio-kit/readme/feature-03.png">
<br>
<br>

* Please input the destination for the build output.
* Please input the asset name (Any).

3. You can export the folder (asset name) created with '2' using AirDrop or similar methods to any iOS device. Then, place it in Keyaki Studio > assets using the Files app. (*)
<br>
**For VRM avatars**
<br>
(*) Keyaki Studio > vrm_files
<br>

# Note

* You cannot add scripts or shaders.
* You cannot use asset names that are the same.

# Author

* Yuta Ueda / Keyaki Kaihatsu
* https://twitter.com/keyaki_kaihatsu

# License

"AvatarStudio Kit" is under [MIT license](https://en.wikipedia.org/wiki/MIT_License).
