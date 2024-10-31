using MauiShakeDetector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShakingLight.Services;

namespace ShakingLight.Services
{
    public class ShakingDetectorService
    {
        private readonly FlashlightService flashlightService;

        public bool IsListening { get; private set; }

        public ShakingDetectorService(FlashlightService flashlightService)
        {
            this.flashlightService = flashlightService;
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

        private async void Detector_ShakeDetected(object? sender, ShakeDetectedEventArgs e)
        {
            if (e.NoOfShakes >= 3)
            {
                await flashlightService.ToggleFlashlightAsync();
            }
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
