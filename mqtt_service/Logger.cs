using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace mqtt_service
{
	internal class Logger
	{
		private EventLog eventLog1;
		private const string log = "Application";

		internal Logger(string source)
		{
			eventLog1 = new EventLog();
			if (!EventLog.SourceExists(source))
			{
				EventLog.CreateEventSource(source, log);
			}
			eventLog1.Source = source;
			eventLog1.Log = log;
		}

		internal void WriteEntry(string Entry)
		{
			eventLog1.WriteEntry(Entry);
		}

		internal void logReceivedMessage(MqttMsgPublishEventArgs e)
		{
			string topic = "Topic: " + e.Topic + "\r";
			string msg = "Message: " + System.Text.Encoding.Default.GetString(e.Message) + "\r";
			string qos = "QoS: " + e.QosLevel.ToString() + "\r";
			string logMessage = "--------------------\rMessage Received\r--------------------\rTimestamp: " + DateTime.Now.ToString() + "\r" + topic + msg + qos + "--------------------\r\r";
			eventLog1.WriteEntry(logMessage);
		}
	}
}
