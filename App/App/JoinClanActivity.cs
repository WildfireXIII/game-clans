//*************************************************************
//  File: JoinClanActivity.cs
//  Date created: 12/11/2016
//  Date edited: 12/31/2016
//  Author: Nathan Martindale
//  Copyright � 2016 Digital Warrior Labs
//  Description: 
//*************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using DWL.Utility;

namespace App
{
	[Activity(Label = "Join Clan")]
	public class JoinClanActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.JoinGroup);

			EditText pClan = FindViewById<EditText>(Resource.Id.txtClanName);
			EditText pPass = FindViewById<EditText>(Resource.Id.txtClanPassword);
			EditText pUser = FindViewById<EditText>(Resource.Id.txtUserName);
			Button pJoin = FindViewById<Button>(Resource.Id.btnSubmitJoin);

			pJoin.Click += delegate
			{
				// TODO: SANITIZATION!
				_hidden.InitializeWeb();
				//string sUserPass = File.ReadAllText(Master.GetBaseDir() + "_key.dat");
				string sUserPass = Master.GetKey();
				string sBody = "<params><param name='sEmail'>" + Master.GetEmail() + "</param><param name='sClanName'>" + pClan.Text.ToString() + "</param><param name='sClanPassPhrase'>" + pPass.Text.ToString() + "</param><param name='sUserName'>" + pUser.Text.ToString() + "</param><param name='sUserPassPhrase'>" + sUserPass +  "</param></params>";
				//string sResponse = WebCommunications.SendPostRequest("http://dwlapi.azurewebsites.net/api/reflection/GameClansServer/GameClansServer/ClanServer/JoinClan", sBody, true);
				string sResponse = WebCommunications.SendPostRequest(Master.GetBaseURL() + Master.GetServerURL() + "JoinClan", sBody, true);

				XElement pResponse = Master.ReadResponse(sResponse);
				
				//Master.Popup(this, pResponse.Element("Text").Value);
				string sResponseMessage = pResponse.Element("Text").Value;


				// TODO: even if user is already part of clan, need to check local clans file and make sure the data is there (and add it if not)

				

				var pBuilder = new AlertDialog.Builder(this);
				pBuilder.SetMessage(sResponseMessage);

				if (pResponse.Attribute("Type").Value == "Error" || pResponse.Element("Data").Element("ClanStub") == null)
				{
					pBuilder.SetPositiveButton("Ok", (e, s) => { return; });
				}
				else
				{
					pBuilder.SetPositiveButton("Ok", (e, s) =>
					{
						XElement pClanStub = pResponse.Element("Data").Element("ClanStub");
						string sClanName = pClanStub.Attribute("ClanName").Value;
						string sUserName = pClanStub.Attribute("UserName").Value;

						File.AppendAllLines(Master.GetBaseDir() + "_clans.dat", new List<string>() { sClanName + "|" + sUserName });
						this.SetResult(Result.Ok);
						this.Finish();
					});
				}
				pBuilder.Create().Show();
			};
		}
	}
}