using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace mqtt_service
{
	internal class HomeAssistantListener
	{
		#region MQTT

		private MqttClient client;
		private CoreAudioDevice defaultPlaybackDevice;
		private double initial_volume;
		private Logger _log;

		public Action<object, MqttMsgPublishEventArgs> MqttMsgPublishReceived { get; private set; }

		internal HomeAssistantListener(Logger log)
		{
			_log = log;
			defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;

			client = new MqttClient(secrets.ip, secrets.port, false, null, null, MqttSslProtocols.None);
			client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
			string clientId = Guid.NewGuid().ToString();
			client.Connect(clientId, secrets.username, secrets.password);
			client.Subscribe(new string[] { secrets.topic_desktop }, new byte[] { 0 });
			client.Subscribe(new string[] { secrets.topic_test }, new byte[] { 0 });
		}

		private void Client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
		{
			switch (e.Topic)
			{
				case secrets.topic_desktop:
					string msg = Encoding.Default.GetString(e.Message);
					handleDesktopMessage(msg);
					break;

				case secrets.topic_test:
				default:
					_log.logReceivedMessage(e);
					break;
			}
		}

		private void handleDesktopMessage(string message)
		{
			switch (message)
			{
				case "mute":
					initial_volume = defaultPlaybackDevice.Volume;
					defaultPlaybackDevice.Mute(true);
					break;

				case "unmute":
					defaultPlaybackDevice.Mute(false);
					break;


				case "shutdown":
					var psi = new ProcessStartInfo("shutdown", "/s /t 0");
					psi.CreateNoWindow = true;
					psi.UseShellExecute = false;
					Process.Start(psi);
					break;

				case "cmd":
					break;
			}
		}

		internal void ExecuteCommand(string Command)
		{
			ProcessStartInfo ProcessInfo;
			Process Process;

			ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + Command);
			ProcessInfo.CreateNoWindow = true;
			ProcessInfo.UseShellExecute = true;

			Process = Process.Start(ProcessInfo);
		}

		internal void Close()
		{
			client.Disconnect();
		}

		#endregion
	}
}
