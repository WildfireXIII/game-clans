﻿//*************************************************************
//  File: Master.cs
//  Date created: 11/28/2016
//  Date edited: 12/30/2016
//  Author: Nathan Martindale
//  Copyright © 2016 Digital Warrior Labs
//  Description: Superclass of static functions and properties
//*************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DWL.Utility;

namespace GameClansServer
{
	public class Master
	{
		// constants
		public const string MSGTYPE_DEFAULT = "Default"; // message only
		public const string MSGTYPE_DATA = "Data"; // data only
		public const string MSGTYPE_BOTH = "Both"; // data and message
		public const string MSGTYPE_ERROR = "Error"; // error message (can include data)

		public const string SERVER_VERSION = "1.1.0";
		public const string APP_VERSION = "1.1.0"; // this is the version of app required in order to work with the current server version
		public const string CLIENT_VERSION = "1.1.0";

		// methods

		public static string EncodeXML(string sXML) { return sXML.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;"); }

		public static string BuildUserPartitionKey(string sClanName) { return sClanName + "|USER"; }
		public static string BuildUserNotifPartitionKey(string sClanName, string sUserName) { return sClanName + "|" + sUserName + "|NOTIF"; }
		public static string BuildGamePartitionKey(string sClanName) { return sClanName + "|GAME"; }
		
		public static string MessagifySimple(string sMsg) { return Messagify(sMsg, MSGTYPE_DEFAULT, ""); } // why is this necessary?
		public static string MessagifyError(string sMsg) { return Messagify(sMsg, MSGTYPE_ERROR, ""); }
		public static string MessagifyError(string sMsg, string sData) { return Messagify(sMsg, MSGTYPE_ERROR, sData); }
		public static string MessagifyData(string sData) { return Messagify("", MSGTYPE_DATA, sData); }
		public static string Messagify(string sMsg, string sType, string sData) { return "<Message Type='" + sType + "'><Text>" + sMsg + "</Text><Data>" + sData + "</Data></Message>"; }

		public static string GenerateKey()
		{
			Guid pKey = Guid.NewGuid();
			string sKey = pKey.ToString().Replace("-", "");

			string sHashedKey = Security.Sha256Hash(sKey);
			return sHashedKey;
		}
	}

}
