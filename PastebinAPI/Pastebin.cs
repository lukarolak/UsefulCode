/* PASTEBIN WIKI INFO
 	FORMAT		
	 	use "text" for text, check the website for anything else - https://pastebin.com/api#6
	EXPIRE		
		N = Never
		10M = 10 Minutes
    	1H = 1 Hour
    	1D = 1 Day
    	1W = 1 Week
    	2W = 2 Weeks
    	1M = 1 Month
    	6M = 6 Months
    	1Y = 1 Year
	PRIVATE
		0 = Public
   	 	1 = Unlisted (Not linked in google)
    	2 = Private (only allowed in combination with api_user_key, as you have to be logged into your account to access the paste)
		NOTE FOR FREE USERS:
			Public pastes:		unlimited
			Unlisted pastes:	25
			Private pastes:		0
*/
//Code modified from 
		/*
		https://pastebin.com/pzvNjarK
		https://github.com/nikibobi/pastebin-csharp
		*/
	
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class Pastebin : MonoBehaviour {
	
	private string ILoginURL = 	"http://pastebin.com/api/api_login.php";
    private string IPostURL  = 	"http://pastebin.com/api/api_post.php";
    private string IDevKey 	 = 	; //Your dev key here <--
    private string IUserKey  = 	; //Use GetUserKey() to get this key

	public enum IExpireDateEnum{ //This is so I don't have to pass string, This is a much safer way to handle passing of fixed amount of arg!
		eN,e10M,e1H,e1D,e1W,e2W,e1M,e6M,e1Y
	}
	public enum IPrivateEnum{
		e0,e1,e2
	}
	public enum IOptionEnum{
		ePaste,eList
	}

	public String GetUserKey(){
		NameValueCollection IQuery = new NameValueCollection();

		IQuery.Add("api_dev_key", IDevKey);
		IQuery.Add("api_user_name","");			//Your pastebin username here
		IQuery.Add("api_user_password","");		//Your pastebin password here //NOTE: after use of this function delete data from this query
													//(you do not want to leave unencrypted passwords in your code)
		using (WebClient wc = new WebClient())
            {
                byte[] respBytes = wc.UploadValues(ILoginURL, IQuery);
                string resp = Encoding.UTF8.GetString(respBytes);

                if (resp.Contains("Bad API request"))
                {
                    throw new WebException(resp, WebExceptionStatus.SendFailure);
                }
                return resp;
            }
	}
	public void Send(string IBody = "", string ISubj = "", string IFormat = "text", IExpireDateEnum IExpireDate = IExpireDateEnum.eN, 
		IPrivateEnum IPrivate = IPrivateEnum.e0, IOptionEnum IOption = IOptionEnum.ePaste){
		//If body and/or subject isn't provided in formats that require them this function will not execute
		if(IOption != IOptionEnum.eList){
			if (string.IsNullOrEmpty(IBody.Trim())) { return; }
        	if (string.IsNullOrEmpty(ISubj.Trim())) { return; }
		}

		//ValueCollection I will be sending to pastebin
		NameValueCollection IQuery = new NameValueCollection();
		if(IOption == IOptionEnum.eList){
			IQuery.Add("api_dev_key", IDevKey);
			IQuery.Add("api_user_key",IUserKey);
			IQuery.Add("api_results_limit","1000"); //default 50, min 1, max 1000
			IQuery.Add("api_option", ReturnOptionString(IOption));
			print(SendData(IPostURL,IQuery));
		}else if( IOption == IOptionEnum.ePaste){ 
			IQuery.Add("api_dev_key", IDevKey);
        	IQuery.Add("api_option", ReturnOptionString(IOption));
        	IQuery.Add("api_paste_code", IBody);
        	IQuery.Add("api_paste_private", ReturnPrivateStatus(IPrivate));
        	IQuery.Add("api_paste_name", ISubj);
        	IQuery.Add("api_paste_expire_date", ReturnExpireDate(IExpireDate));
        	IQuery.Add("api_paste_format", IFormat);
        	if(IUserKey!=null)
				IQuery.Add("api_user_key", IUserKey);
			print(SendData(IPostURL,IQuery));
		}
	}
	string SendData(string PastebinURL,NameValueCollection IQuery){
		using (WebClient IClient = new WebClient()){
                string IResponse = Encoding.UTF8.GetString(IClient.UploadValues(PastebinURL, IQuery));
				return IResponse;
            }
	}
	string ReturnExpireDate(IExpireDateEnum ExpireDate){
		switch(ExpireDate){
			case IExpireDateEnum.eN:
				return "N";
			case IExpireDateEnum.e1H:
				return "1H";
			case IExpireDateEnum.e1D:
				return "1D";
			case IExpireDateEnum.e1W:
				return "1W";
			case IExpireDateEnum.e2W:
				return "2W";
			case IExpireDateEnum.e1M:
				return "1M";
			case IExpireDateEnum.e6M:
				return "6M";
			case IExpireDateEnum.e1Y:
				return "1Y";
			default: //For now this is redundant 
				return "N";
		}
	}
	string ReturnPrivateStatus(IPrivateEnum IPrivate){
		switch(IPrivate){
			case IPrivateEnum.e2:
				return "2";
			case IPrivateEnum.e1:
				return "1";
			default:
				return "0";
		}
	}
	string ReturnOptionString(IOptionEnum IOption){
		switch (IOption){
			case(IOptionEnum.eList):
				return "list";
			case(IOptionEnum.ePaste):
				return "paste";
			default:
				return "paste";
		}


	}


	// Use this for initialization
	void Start () {
		//IUserKey = GetUserKey(); Fills IUserKey with user key

		//Sends a paste
		//Send("Body","Subject",IExpireDate: IExpireDateEnum.e10M);
		
		//Get's all posts made by user
		//Send(IOption: IOptionEnum.eList);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
