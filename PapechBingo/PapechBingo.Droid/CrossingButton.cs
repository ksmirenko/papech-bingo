using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Widget;

namespace PapechBingo.Droid {
    sealed class CrossingButton : Button {
        private readonly Drawable _bgOff;
        private readonly Drawable _bgOn;
        private bool _state;
        
        public CrossingButton(Context context) : base(context) {
            SetTextColor(Color.Black);
            SetTextSize(ComplexUnitType.Dip, 14);
            SetIncludeFontPadding(false);
            SetPadding(0, 0, 0, 0);
            Click += (sender, args) => {
                _state = !_state;
                UpdateBackground();
            };

            _state = false;
            _bgOff = Resources.GetDrawable(Resource.Drawable.crossed_button_off);
            _bgOn = Resources.GetDrawable(Resource.Drawable.crossed_button_on);
            UpdateBackground();
            
        }

        public bool State {
            get { return _state; }
            set { _state = value; UpdateBackground(); }
        }

        private void UpdateBackground() {
            Background = _state ? _bgOn : _bgOff;
        }
    }
}