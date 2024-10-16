using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using Microsoft.AspNetCore.Components;
using ShakingLight.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShakingLight.Platforms.Android.Services
{
    [Service]
    public class ForegroundShakingLightService : Service
    {
        [Inject]
        public FlashlightService FlashlightService { get; set; } = default!;

        public ForegroundShakingLightService()
        {

        }

        private string NOTIFICATION_CHANNEL_ID = "8888";
        private int NOTIFICATION_ID = 1;
        private string NOTIFICATION_CHANNEL_NAME = "notification";

        private void createNotificationChannel(NotificationManager notificationMnaManager)
        {
            var channel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, NOTIFICATION_CHANNEL_NAME,
            NotificationImportance.Low);
            notificationMnaManager.CreateNotificationChannel(channel);
        }

        private void startForegroundService()
        {
            var notifcationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                createNotificationChannel(notifcationManager);
            }

            var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID);
            notification.SetAutoCancel(false);
            notification.SetOngoing(true);
            notification.SetSmallIcon(Resource.Mipmap.appicon);
            notification.SetContentTitle("ShakingLight");
            notification.SetContentText("Shaking Light Service is running");
            StartForeground(NOTIFICATION_ID, notification.Build());
        }


        public bool IsServiceRunning()
        {
            ActivityManager manager = (ActivityManager)GetSystemService(ActivityService);
            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (service.Service?.ShortClassName?.Contains(nameof(ForegroundShakingLightService)) == true)
                {
                    return true;
                }
            }
            return false;
        }
        public override ComponentName? StartService(Intent? service)
        {
            try
            {
                FlashlightService.ToggleFlashlightAsync().RunSynchronously();
            }
            catch (Exception ex)
            {
                if (App.Current != null && App.Current.MainPage != null)
                {
                    App.Current.MainPage.DisplayAlert("Error", ex.Message, "Cancel");
                }
            }
            return base.StartService(service);
        }

        public override bool StopService(Intent? name)
        {
            try
            {
                FlashlightService.ToggleFlashlightAsync().RunSynchronously();
            }
            catch (Exception ex)
            {
                if (App.Current != null && App.Current.MainPage != null)
                {
                    App.Current.MainPage.DisplayAlert("Error", ex.Message, "Cancel");
                }
            }
            return base.StopService(name);
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return base.OnStartCommand(intent, flags, startId);
        }

        public override IBinder? OnBind(Intent? intent)
        {
            return null;
        }
    }
}
