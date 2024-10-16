using Android.Content;
using Android.App;
using MauiShakeDetector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application = Android.App.Application;

namespace ShakingLight.Platforms.Android.Services
{
    public class ShakingDetectorService
    {
        private readonly ForegroundShakingLightService foregroundShakingLightService;

        public bool IsListening { get; private set; }

        public ShakingDetectorService(ForegroundShakingLightService foregroundShakingLightService)
        {
            this.foregroundShakingLightService = foregroundShakingLightService;
        }

        public async Task StartListening()
        {
            try
            {
                ShakeDetector.Default.StartListening();
                ShakeDetector.Default.ShakeDetected += Detector_ShakeDetected;
                IsListening = true;
            }
            catch (Exception ex)
            {
                if (App.Current != null && App.Current.MainPage != null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Cancel");
                }
            }
        }

        private void Detector_ShakeDetected(object? sender, ShakeDetectedEventArgs e)
        {
#if ANDROID
            Intent intent = new Intent(Application.Context, typeof(ForegroundShakingLightService));
            if (foregroundShakingLightService.IsServiceRunning())
            {
                foregroundShakingLightService.StopService(intent);
            }
            else
            {
                foregroundShakingLightService.StartService(intent);
            }
#endif
        }

        public async Task StopListening()
        {
            try
            {
                if (ShakeDetector.Default.IsMonitoring)
                {
                    ShakeDetector.Default.StopListening();
                    ShakeDetector.Default.ShakeDetected -= Detector_ShakeDetected;
                    IsListening = false;
                }
            }
            catch (Exception ex)
            {
                if (App.Current != null && App.Current.MainPage != null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Cancel");
                }
            }
        }
    }
}
