using System;

using Android.App;
using Android.Content;
using Android.Views;
using Android.OS;
using Android.Widget;

namespace PapechBingo.Droid {
    [Activity(Label = "PapechBingo", MainLauncher = true,
        Icon = "@drawable/ic_launcher")]
    public class MainActivity : Activity {
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Set button onClickListeners
            var buttonReset = FindViewById<Button>(Resource.Id.ButtonReset);
            buttonReset.Click += delegate { };
            var buttonInfo = FindViewById<Button>(Resource.Id.ButtonInfo);
            buttonInfo.Click += delegate { ShowAlertMessage(Resources.GetString(Resource.String.info_message)); };
        }

        private void ShowAlertMessage(string msg) {
            // Build the dialog.
            var builder = new AlertDialog.Builder(this);
            //builder.SetTitle(msg);
            builder.SetMessage(msg);
            builder.SetCancelable(false);

            // Create empty event handlers, we will override them manually instead of letting the builder handling the clicks.
            builder.SetPositiveButton("OK", (EventHandler<DialogClickEventArgs>)null);
            var dialog = builder.Create();

            // Show the dialog. This is important to do before accessing the buttons.
            dialog.Show();

            // Get the button.
            var yesBtn = dialog.GetButton((int) DialogButtonType.Positive);

            // Assign our handlers.
            yesBtn.Click += (sender, args) => { dialog.Dismiss(); };
        }
    }
}


