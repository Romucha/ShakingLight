using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShakingLight.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShakingLight.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly FlashlightService flashlightService;
#if ANDROID
        private readonly Platforms.Android.Services.ShakingDetectorService shakingService;
#endif

        private bool isOn;

        public bool IsOn
        {
            get => isOn;
            set => SetProperty(ref isOn, value);
        }

        public IAsyncRelayCommand ToggleFlashlightCommand => new AsyncRelayCommand(ToggleFlashlight);
        
        public MainViewModel(
#if ANDROID
            Platforms.Android.Services.ShakingDetectorService shakingService,
#endif
            FlashlightService flashlightService
            )
        {
            this.flashlightService = flashlightService;
#if ANDROID
            this.shakingService = shakingService;
#endif
        }

        private async Task ToggleFlashlight()
        {
            try
            {
#if ANDROID
                if (shakingService.IsListening)
                {
                    await shakingService.StopListening();
                }
                else
                {
                    await shakingService.StartListening();
                }
                IsOn = shakingService.IsListening;
#elif WINDOWS
                if (IsOn)
                {
                    await Flashlight.TurnOffAsync();
                    IsOn = false;
                }
                else
                {
                    await Flashlight.TurnOnAsync();
                    IsOn = true;
                }
#endif
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
