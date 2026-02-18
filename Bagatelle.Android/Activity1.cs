using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Bagatelle.Shared;
using Microsoft.Xna.Framework;
using System;

namespace Bagatelle.Android
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : AndroidGameActivity
    {
        private Game1 _game;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Catch unhandled exceptions to see what crashes in release
            AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
            {
                var msg = args.Exception?.ToString() ?? "Unknown error";
                System.Diagnostics.Debug.WriteLine($"BAGATELLE CRASH: {msg}");
                try
                {
                    // Write crash log to app storage
                    var path = System.IO.Path.Combine(
                        ApplicationContext.GetExternalFilesDir(null)?.AbsolutePath ?? FilesDir.AbsolutePath,
                        "crash.log");
                    System.IO.File.WriteAllText(path, $"{DateTime.Now}\n{msg}");
                }
                catch { }
            };

            try
            {
                // Enable rendering behind display cutouts (notches/camera holes)
                if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
                {
                    Window.Attributes.LayoutInDisplayCutoutMode = LayoutInDisplayCutoutMode.ShortEdges;
                }

                // Hide navigation and status bars for true fullscreen
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(
                    SystemUiFlags.LayoutStable |
                    SystemUiFlags.LayoutHideNavigation |
                    SystemUiFlags.LayoutFullscreen |
                    SystemUiFlags.HideNavigation |
                    SystemUiFlags.Fullscreen |
                    SystemUiFlags.ImmersiveSticky
                );

                _game = new Game1();
                _view = _game.Services.GetService(typeof(View)) as View;

                SetContentView(_view);
                _game.Run();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, $"Crash: {ex.Message}", ToastLength.Long)?.Show();
                System.Diagnostics.Debug.WriteLine($"BAGATELLE CRASH: {ex}");
                try
                {
                    var path = System.IO.Path.Combine(
                        ApplicationContext.GetExternalFilesDir(null)?.AbsolutePath ?? FilesDir.AbsolutePath,
                        "crash.log");
                    System.IO.File.WriteAllText(path, $"{DateTime.Now}\n{ex}");
                }
                catch { }
            }
        }
    }
}
