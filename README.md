# RyujuEngine for Unity
This repository contains the scripts for Ryuju Orchestra Unity Applications.

>このリポジトリは龍獣楽団の Unity アプリケーションのためのスクリプトを含みます。

## Get started
Add this repository from Unity Package Manager Window
or add the following line into `Packages/manifest.json`

>Unity Package Manager を用いてリポジトリを追加してください。
>または、次の 1 行を `Packages/manifest.json` に追加してください。

```
"com.ryujuorchestra.ryujuengine": "https://github.com/RyujuOrchestra/RyujuEngine.git#v0.2.0-preview"
```

See [RyujuEngine.md](Documentation~/RyujuEngine) for more usage.

>詳しい使用方法は[RyujuEngine.md](Documentation~/RyujuEngine)を参照してください。

## Development
For development, prepare the following items:
>開発には次のものを準備します。

- Unity Editor 2019.3
- New empty project for development
  (開発用の新しい空のプロジェクト)
- Your new development repository that forked this repository
  (このリポジトリを fork した開発リポジトリ)

Open Unity Package Manager on the above project, enable `show preview package` on its `Advanced` button,
and then install the following packages into the project:
>上記のプロジェクトにて Unity Package Manager を開き、`show preview package` を `Advanced` ボタンから有効化し、
>その上で次のパッケージをインストールしてください。

- Package Development

Finally, git-clone your forked repository on the `path/to/project/Packages` directory.
Well done! This library is installed on `Packages/RyujuEngine` that is visible on the `Project` view.
It is editable and testable.
>最後に、fork したリポジトリを `このプロジェクトのパス/Packages` ディレクトリの中で git clone します。
>すると、RyujuEngine ライブラリが`プロジェクト`ビューから見える `Packages/RyujuEngine` にインストールされます。
>これは編集やテストが可能です。

## License
Licensed under either of

- Apache License, Version 2.0
  ([LICENSE.md#apache](LICENSE.md#apache-license-version-20) or http://www.apache.org/licenses/LICENSE-2.0)
- MIT License
  ([LICENSE.md#mit](LICENSE.md#mit-license) or http://opensource.org/licenses/MIT)

at your option.

>このプログラムを利用する人は上記 2 つのライセンスから好きな方を選んで利用できます。
>(訳注：この日本語は意訳であり、実際のライセンスは必ず英文に従います。)

## Contribution
:heart_eyes_cat: Your contribution is always welcome :bangbang:

>:heart_eyes_cat: いつでもコントリビューションを歓迎します :bangbang:

Unless you explicitly state otherwise, any contribution intentionally submitted
for inclusion in the work by you, as defined in the Apache-2.0 license, shall be
dual licensed as above, without any additional terms or conditions.

>特に明記がない限り、あなたの成果物の意図的な提出による任意のコントリビューションは、
>Apache License, Version 2.0 に定めるところにより、
>追加の条項や条件なしに上記 2 つのライセンス (訳注: Apache License, Version 2.0 及び MIT License)
>によってデュアルライセンスのもとに頒布されるでしょう。
>(訳注：この日本語は意訳であり、実際のコントリビューションに関わる契約は必ず英文に従います。)
