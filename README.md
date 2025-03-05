# GD_Sirial

このプロジェクトは、Godot.mono(C#対応版)とpicoSDK間をシリアル通信でつなぐためのものです。現状、Godot→RP2040に対応しています

## 必要なパッケージ

このプロジェクトを実行するためには、以下のパッケージが必要です：

- `System.IO.Ports`

## パッケージのインストール

1. ターミナル等で使用するゲームのプロジェクトに `System.IO.Ports` パッケージを追加します。以下のコマンドを使用してください：

    ```sh
    dotnet add package System.IO.Ports
    ```

## 使用方法

1. ``pico``フォルダ内を開き、cmakeを用いてビルドを行ってください。(ビルドにはpicoSDKがPCにダウンロードされている必要があります。)または、``pico/build``フォルダ内の``pico.uf2``をRP2040にインストールしてください
2. GodotのシーンにNodeを追加し、``cs``フォルダ内の``SerialConnecter.cs``をアタッチしてください
3. SerialConnecterに各種関数があるので、コメントにしたがって利用してください。


## `SerialConnecter` クラスの説明

### 変数一覧

csharp
| 変数名              | 型          | 説明 |
|---------------------|------------|----------------------------------------------------|
| `defaultPortName`  | `string`   | シリアル通信に使用するポートの名前（GodotEditorで変更可、デフォルト: `"COM3"`） |
| `defaultBaudRate`  | `int`      | シリアル通信に使用するボーレート（GodotEditorで変更可、デフォルト: `9600`） |
| `last_serial_data` | `string`   | 最後に取得したシリアル通信の内容 |
| `serialPort`       | `SerialPort` | シリアル通信のポートを管理するオブジェクト |
| `serialThread`     | `Thread`   | シリアル通信の受信を処理するスレッド |
| `isRunning`        | `bool`     | シリアル通信の受信スレッドを動作させるかどうかを管理するフラグ |

### 関数一覧

| 関数名                        | 戻り値    | 引数                                                | 説明 |
|--------------------------------|----------|----------------------------------------------------|------------------------------------------------|
| `_ExitTree()`                  | `void`   |                                                | ノードが削除されたときにシリアルポートを閉じる処理を実行 |
| `SendMessage(string message)`  | `void`   | `message: string`（送信するメッセージ）           | シリアルポートを通じてメッセージを送信 |
| `SerialReadLoop()`             | `void`   |                                                | シリアル通信の受信ループ（別スレッドで実行） |
| `GetPortNames()`               | `string[]` |                                               | 利用可能なシリアルポートの名前の配列を取得 |
| `GetRecieveMode()`             | `string` |                                                | デバイスの現在のモードを取得 |
| `SetNone()`                    | `void`   |                                                | `None` モードに切り替え |
| `ResetIO()`                    | `void`   |                                                | `stdio_init_all();` を実行（IOリセット） |
| `GpioPut(int[] gpio, int[] value)` | `void` | `gpio: int[]`（設定するGPIOピンの番号）、`value: int[]`（それぞれのピンに設定する値、0 or 1） | `gpio_put` を実行（GPIO出力設定） |
| `GpioSetIn(int[] gpio)`        | `void`   | `gpio: int[]`（設定するGPIOピンの番号）            | `gpio_set_in` を実行（GPIO入力設定） |
| `GpioSetOut(int[] gpio)`       | `void`   | `gpio: int[]`（設定するGPIOピンの番号）            | `gpio_set_out` を実行（GPIO出力設定） |
| `OpenSerialPort()`             | `bool`   |                                                | シリアルポートを開く（成功なら `true`、失敗なら `false`） |
| `CloseSerialPort()`            | `void`   |                                                | シリアルポートを閉じる |



## ライセンス

このプロジェクトはMITライセンスの下で公開されています。詳細はLICENSEファイルを参照してください。

## クレジット・謝辞
Raspberry Pi財団のサポートとリソースに感謝します。
本プロジェクトはpicoSDKおよび、VScode拡張機能「Raspberry Pi Pico」を利用しています。

また、Godot財団およびコミュニティーに感謝します。

## 貢献

バグ報告や機能リクエストは、Issueトラッカーを使用してください。プルリクエストも歓迎します。
