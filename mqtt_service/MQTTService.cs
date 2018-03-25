using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher.AudioApi.CoreAudio;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace mqtt_service
{
	public partial class MQTTService : ServiceBase
	{
		private Logger log;
		private HomeAssistantListener listener;

		public MQTTService()
		{
			InitializeComponent();
			log = new Logger("MQTT Client");
			listener = new HomeAssistantListener(log);
		}

		protected override void OnStart(string[] args)
		{
			log.WriteEntry("Start: " + DateTime.Now.ToString());
		}

		protected override void OnStop()
		{
			listener.Close();
			log.WriteEntry("Exit: " + DateTime.Now.ToString());
		}

	
	}
}
