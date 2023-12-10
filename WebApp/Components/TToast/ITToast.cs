namespace WebApp.Components.TToast
{
    public interface ITToast
    {
        Task Show( string title, string message, string? type = "");
    }
}
