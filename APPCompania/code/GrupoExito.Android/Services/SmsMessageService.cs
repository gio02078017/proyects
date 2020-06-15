using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Telephony;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Utilities.Resources;
using Java.Lang;
using Microsoft.AppCenter.Crashes;
using System.Collections.Generic;

namespace GrupoExito.Android.Services
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { Telephony.Sms.Intents.SmsReceivedAction })]
    public class SmsMessageService : BroadcastReceiver
    {
        private static ISmsListener _smsListener;

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                string messageBody = string.Empty;
                Bundle data = intent.Extras;
                Object[] pdus = data.Get("pdus").ToArray<Object>();
                SmsMessage smsMessage = GetIncomingMessage(pdus[0], data);

                if (!string.IsNullOrEmpty(smsMessage.DisplayOriginatingAddress) && smsMessage.DisplayOriginatingAddress.Equals(AppConfigurations.SmsOriginator))
                {
                    if (!string.IsNullOrEmpty(smsMessage.MessageBody))
                    {
                        string[] messagesBody = smsMessage.MessageBody.Split(':');
                        messageBody = messagesBody[1];
                        _smsListener.MessageReceived(messageBody.TrimEnd().TrimStart());
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SmsMessageService, ConstantMethodName.OnReceive } };
                Crashes.TrackError(exception, properties);
            }
        }

        private SmsMessage GetIncomingMessage(Object aObject, Bundle bundle)
        {
            SmsMessage currentSMS;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                string format = bundle.GetString("format");
                currentSMS = SmsMessage.CreateFromPdu((byte[])aObject, format);
            }
            else
            {
                currentSMS = SmsMessage.CreateFromPdu((byte[])aObject);
            }

            return currentSMS;
        }


        public void BindListener(ISmsListener listener)
        {
            _smsListener = listener;
        }
    }
}