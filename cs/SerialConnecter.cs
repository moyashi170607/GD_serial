using Godot;
using System;
using System.IO.Ports;
using System.Threading;

[GlobalClass]
public partial class SerialConnecter : Node
{
	/// <summary>
	/// シリアル通信に使用するポートの名前
	/// GodotEditorで変更可
	/// </summary>
	[Export]
	public string defaultPortName = "COM3";

	/// <summary>
	/// シリアル通信に使用するレート
	/// GodotEditorで変更可
	/// </summary>
	[Export]
	public int defaultBaudRate = 9600;

	/// <summary>
	/// 最後に取得したシリアル通信の内容
	/// </summary>
	public string last_serial_data = "";

	private SerialPort serialPort;
	private Thread serialThread;
	private bool isRunning = true;


	// Called whens the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	/// <summary>
	/// ノードが削除されたとき自動的にポートを閉じる
	/// </summary>
	public override void _ExitTree()
	{
		base._ExitTree();

		if(serialPort != null && serialPort.IsOpen)
		{
			CloseSerialPort();
		}

	}

	/// <summary>
	/// シリアル通信　送信
	/// </summary>
	/// <param name="message">
	/// 送信するメッセージ
	/// </param>
	public void SendMessage(string message)
	{
		if(serialPort != null && serialPort.IsOpen)
		{
			serialPort.WriteLine(message);
			GD.Print("送信" + message);
		}
	}

	/// <summary>
	/// シリアル通信　受信
	/// </summary>
	private void SerialReadLoop()
	{
		while (isRunning && serialPort != null && serialPort.IsOpen)
		{
			try
			{
				if (serialPort.BytesToRead > 0)
				{
					string received = serialPort.ReadLine();

					last_serial_data = received;

					GD.Print("受信しました"+received);
				}

			}
			catch (TimeoutException) { }
			catch (Exception ex)
			{
				GD.PrintErr("シリアル受信エラー: " + ex.Message);
				isRunning = false;
			}
		}
	}

	/// <summary>
	/// ポートの名前の配列を取得
	/// </summary>
	/// <returns>
	/// string[] ポートの名前の配列
	/// </returns>
	public string[] GetPortNames(){
		return SerialPort.GetPortNames();
	}

	public string GetRecieveMode(){
		if(serialPort != null && serialPort.IsOpen)
		{
			SendMessage("@GET_MODE");

			while (!last_serial_data.Contains("MODE:"))
			{
				Thread.Sleep(100);
			}

		}

		return last_serial_data;
	}

	/// <summary>
	/// Noneモードに切り替え
	/// </summary>
	public void SetNone(){
		SendMessage("@None");
	}

	/// <summary>
	/// stdio_init_all();を実行
	/// </summary>
	public void ResetIO(){
		SendMessage("@RESET");
	}

	/// <summary>
	/// PUTモードに切り替え
	/// gpio_putを実行
	/// </summary>
	/// <param name="gpio">uint[] putするgpioの番号</param>
	/// <param name="value">uint[] それぞれのピンに設定する値 0 or 1 </param>
	public void GpioPut(int[] gpio, int[] value){
		SendMessage("@PUT");
		for(int i=0; i<gpio.Length; i++){
			SendMessage(gpio[i] + "," + value[i]);
		}
	}

	/// <summary>
	/// SET_INモードに切り替え
	/// gpioをGPIO_INに設定する
	/// </summary>
	/// <param name="gpio">uint[] 設定するピンの番号</param>
	public void GpioSetIn(int[] gpio){
		SendMessage("@SET_IN");
		foreach(uint num in gpio){
			SendMessage(num.ToString());
		}
	}

	/// <summary>
	/// SET_OUTモードに切り替え
	/// gpioをGPIO_OUTに設定する
	/// </summary>
	/// <param name="gpio">uint[] 設定するピンの番号</param>
	public void GpioSetOut(int[] gpio){
		SendMessage("@SET_OUT");
		foreach(uint num in gpio){
			SendMessage(num.ToString());
		}
	}

	/// <summary>
	/// シリアル通信を開く
	/// </summary>
	/// <returns>
	/// bool シリアル通信の接続に成功したかどうか
	/// </returns>
	public bool OpenSerialPort()
	{
		try
		{

			serialPort = new SerialPort(defaultPortName,defaultBaudRate);
			serialPort.DtrEnable = true;
			serialPort.ReadTimeout = 500;
			serialPort.Open();

			serialThread = new Thread(SerialReadLoop);
			serialThread.Start();

			GD.Print("接続成功");

			return true;

		}
		catch (Exception ex)
		{
			GD.PrintErr("エラー" + ex.Message);
			return false;
		}
	}

	/// <summary>
	/// シリアル通信を閉じる
	/// </summary>
	public void CloseSerialPort()
		{
			if(serialPort != null && serialPort.IsOpen)
			{
				isRunning = false;
				serialPort.Close();
			}

		}

}
