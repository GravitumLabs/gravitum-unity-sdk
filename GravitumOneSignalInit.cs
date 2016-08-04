using UnityEngine;
using System.Collections;
using System.Collections.Generic;  

//Attach this script to an object in the first scene that is loaded when your game is opened.
public class GravitumOneSignalInit : MonoBehaviour {


	//Initialize OneSignal & Request OneSignal User ID
    //See: https://documentation.onesignal.com/docs/onesignal-unity-sdk-api#Init
	void Start () {
        
        //OneSignal Init
        string appId = "########";//Insert your OneSignal app ID (from the onesignal dashboard) here. E.g. "79a1aa3c-69a4-444c-9d2a-194d16175848"
        string googleProjectNumber = "#######";// Insert your Google project number(also called SenderID) from the GoogleDeveloper Dashboard / Google Services Zizard here. E.g. "178716085905"
		OneSignal.Init(appId, googleProjectNumber, HandleNotification);
		
        //Request the  OneSignal userID
		OneSignal.GetIdsAvailable(IdsAvailable);
	}

	// This Handler is called when the player opens the notification received.
    //See: https://documentation.onesignal.com/docs/onesignal-unity-sdk-api#HandleNotification
	private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive) {
        
        /* If you are currently using OneSignal and have an existing OneSignal notification handler. 
        Please add the following code to it.*/
		//Start Gravitum Code insert-----------------
		object temp = null;
		string msgid=null;
		if(additionalData.TryGetValue("msgid", out temp)) {
			msgid=additionalData["msgid"].ToString();
		}
		Dictionary<string, object> pushEventData = new Dictionary<string, object> ();
		pushEventData.Add ("messageID", msgid);
		pushEventData.Add ("Status", "Opened");
		Gravitum.Analytics.SendEvent ("pushEvent", pushEventData);
        //End of Gravitum Code insert----------------
	}
	

	//OneSignal Handler function for when the OneSignal PlayerID is available.
    //See:https://documentation.onesignal.com/docs/onesignal-unity-sdk-api#IdsAvailable
	private void IdsAvailable(string OneSigPlayerID, string pushToken) {

		//Setup Gravitum optional methods below
		/*
        Gravitum.Analytics.SetUserId(user_id);
        Gravitum.Analytics.SetUserName(user_name);
        Gravitum.Analytics.SetFacebookId(facebookId);
        Gravitum.Analytics.SetGender(Gravitum.Analytics.Gender.Male);
        Gravitum.Analytics.SetBirthday(new System.DateTime(1985, 5, 16, 0, 0, 0, 0, System.DateTimeKind.Utc));
        */
        
        //These two are COMPULSORY for Gravitum + OneSignal to work
		Gravitum.Analytics.SetDevicePushToken (OneSigPlayerID);
		Gravitum.Analytics.Init ();
	}
}
