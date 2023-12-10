using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace WebApp.Components.TToast
{

    /***
    * Using Bootstrap 5
    * Import in _import.razor
    * Register <ITToast,TToastProvider> in Ioc
    * Add TToast tag in upper component, MainLayout.razor
    * Usage: TToast.Show("title", "message", "success")
    ****/

    public class TToastProvider : ITToast
    {
        private IJSRuntime JSRuntime { get; set; }

        public TToastProvider(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
        }

        

        public async Task Show(string title, string message, string? type = "")
        {
            await JSRuntime.InvokeVoidAsync("window.TToast.show", title, message, type);
        }
    }
}
