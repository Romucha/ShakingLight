using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShakingLight.Services
{
    public class FlashlightService
    {
        public bool IsOn { get; private set; }

        public FlashlightService()
        {
            
        }

        public async Task ToggleFlashlightAsync()
        {
            try
            {
                if (await Flashlight.Default.IsSupportedAsync())
                {
                    if (!IsOn)
                    {
                        await Flashlight.Default.TurnOnAsync();
                    }
                    else
                    {
                        await Flashlight.Default.TurnOffAsync();
                    }
                    IsOn = !IsOn;
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
