﻿//*************************************************************
//  File: Password.xaml.cs
//  Date created: 12/15/2016
//  Date edited: 12/30/2016
//  Author: Nathan Martindale
//  Copyright © 2016 Digital Warrior Labs
//  Description: 
//*************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

using System.IO;

using DWL.Utility;

namespace Client
{
	/// <summary>
	/// Interaction logic for Password.xaml
	/// </summary>
	public partial class Password : Window
	{
		public Password()
		{
			InitializeComponent();
		}

		private void btnSubmitPassword_MouseLeave(object sender, MouseEventArgs e) { btnSubmitPassword.Background = Master.BUTTON_NORMAL; }
		private void btnSubmitPassword_MouseEnter(object sender, MouseEventArgs e) { btnSubmitPassword.Background = Master.BUTTON_HOVER; }

		private void btnSubmitPassword_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (txtEmail.Text == "") { MessageBox.Show("Your email must not be empty.", "Empty email", MessageBoxButton.OK, MessageBoxImage.Error); return; }
			if (txtPass.Password == "") { MessageBox.Show("Your password cannot be an empty string.", "Empty password", MessageBoxButton.OK, MessageBoxImage.Error); return; }
			if (txtPass.Password != txtPass2.Password) { MessageBox.Show("The passwords do not match.", "Inconsistent password", MessageBoxButton.OK, MessageBoxImage.Error); return; }


			string sBody = "<params><param name='sEmail'>" + txtEmail.Text + "</param><param name='sPassword'>" + Security.Sha256Hash(txtPass.Password) + "</param></params>";
			string sResponse = WebCommunications.SendPostRequest(Master.GetBaseURL() + Master.GetServerURL() + "RegisterUser", sBody, true);
			XElement pResponse = Master.ReadResponse(sResponse);

			string sResponseMessage = pResponse.Element("Text").Value;
			
			if (pResponse.Attribute("Type").Value == "Error")
			{
				MessageBox.Show(sResponseMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
			else
			{
				MessageBox.Show(sResponseMessage);
				Master.HandleUserRegistrationData(pResponse);
				this.Close();
			}
		}
	}
}
