using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShakingLight.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private bool isOn;
        public bool IsOn
        {
            get => isOn;
            set => SetProperty(ref isOn, value);
        }

        public IAsyncRelayCommand ToggleFlashlightCommand => new AsyncRelayCommand(ToggleFlashlight);
        
        public MainViewModel()
        {
            
        }

        private async Task ToggleFlashlight()
        {
            try
            {
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
            }
            catch (Exception ex)
            {

            }
        }
    }
}
