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
        //private const string GridStateSettingName = "GridState";
        private CrossingButton[] _mainButtons;

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Set button onClickListeners
            var buttonReset = FindViewById<Button>(Resource.Id.ButtonReset);
            buttonReset.Click += delegate { };
            var buttonInfo = FindViewById<Button>(Resource.Id.ButtonInfo);
            buttonInfo.Click += delegate { ShowAlertMessage(Resources.GetString(Resource.String.info_message)); };

            // Initializing main buttons
            InitButtons();
        }

        private void InitButtons() {
            const int gridSize = GridLogic.GridSize;
            var strings =
                Resources.GetStringArray(Resource.Array.main_buttons_content);
            var gridLayout = FindViewById<LinearLayout>(Resource.Id.MainButtonsGrid);
            _mainButtons = new CrossingButton[gridSize * gridSize];
            var buttonSize = (int)((Resources.DisplayMetrics.WidthPixels) / Resources.DisplayMetrics.Density / gridSize);
            for (var i = 0; i < gridSize; i++) {
                var row = new LinearLayout(ApplicationContext) {
                    LayoutParameters =
                        new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0, 1.0f),
                    WeightSum = 5.0f
                };
                for (var j = 0; j < gridSize; j++) {
                    var bIndex = i * gridSize + j;
                    var b = new CrossingButton(ApplicationContext) {
                        LayoutParameters =
                            new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.MatchParent, 1.0f),
                        Text = strings[bIndex],
                        Tag = bIndex
                    };
                    b.Click += (sender, args) => {
                        var buttonIndex = (int)((CrossingButton)sender).Tag;
                        if (!GridLogic.Instance.ToggleButton(buttonIndex))
                            return;
                        ShowAlertMessage(
                            Resources.GetString(Resource.String.bingo_message));
                    };
                    b.SetMaxHeight(buttonSize);
                    b.SetMinHeight(buttonSize);
                    _mainButtons[bIndex] = b;
                    row.AddView(b);
                }
                gridLayout.AddView(row);
            }
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
            var yesBtn = dialog.GetButton((int)DialogButtonType.Positive);

            // Assign our handlers.
            yesBtn.Click += (sender, args) => { dialog.Dismiss(); };
        }
    }
}


