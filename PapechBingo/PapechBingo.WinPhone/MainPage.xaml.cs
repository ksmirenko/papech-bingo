using System;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

namespace PapechBingo.WinPhone {
    public sealed partial class MainPage {
        private const string GridStateSettingName = "GridState";
        private ToggleButton[] _mainButtons;
        private readonly ResourceLoader _resLoader = new ResourceLoader();

        public MainPage() {
            InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Required;
            SetupButtonGrid();
            RestoreOrResetGrid();

            Application.Current.Suspending += (sender, e) => {
                var def = e.SuspendingOperation.GetDeferral();
                ApplicationData.Current.LocalSettings.Values[GridStateSettingName] = GridLogic.Instance.ExtractData();
                def.Complete();
            };
            Application.Current.Resuming += (sender, e) => { RestoreOrResetGrid(); };
        }

        private void SetupButtonGrid() {
            var gridSize = GridLogic.GridSize;
            // calculating button height
            var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            var buttonSize = Window.Current.Bounds.Width * scaleFactor / gridSize;
            // setting up rows and columns
            for (var i = 0; i < gridSize; i++) {
                var colDef = new ColumnDefinition {
                    Width = new GridLength(1, GridUnitType.Star)
                };
                MainButtonsGrid.ColumnDefinitions.Add(colDef);
            }
            for (var i = 0; i < gridSize; i++) {
                var rowDef = new RowDefinition {
                    Height = new GridLength(1, GridUnitType.Star)
                };
                MainButtonsGrid.RowDefinitions.Add(rowDef);
            }
            // obtaining buttons content - a shameful workaround
            var buttonStrings = _resLoader.GetString("MainButtonsContent").Split(';');
            // creating and adding buttons
            _mainButtons = new ToggleButton[gridSize * gridSize];
            for (var i = 0; i < gridSize; i++) {
                for (var j = 0; j < gridSize; j++) {
                    var bIndex = i * gridSize + j;
                    var b = new ToggleButton {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Height = buttonSize,
                        Width = buttonSize
                    };
                    b.SetValue(Grid.RowProperty, i);
                    b.SetValue(Grid.ColumnProperty, j);
                    b.Padding = new Thickness(0);
                    b.Margin = new Thickness(0, -11, 0, -11);
                    // setting button content
                    var textBlock = new TextBlock {
                        TextWrapping = TextWrapping.Wrap,
                        FontSize = 14,
                        FontStretch = Windows.UI.Text.FontStretch.ExtraCondensed,
                        Text = buttonStrings[bIndex]
                    };
                    b.HorizontalContentAlignment = HorizontalAlignment.Left;
                    b.Padding = new Thickness(4, 1, 1, 1);
                    b.Content = textBlock;
                    // adding OnClickListener
                    b.Tag = bIndex;
                    b.Click += mainButton_Click;
                    // saving the button and adding to grid
                    _mainButtons[bIndex] = b;
                    MainButtonsGrid.Children.Add(b);
                }
            }
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e) {
            ResetGrid();
        }
        private async void buttonInfo_Click(object sender, RoutedEventArgs e) {
            var msgbox = new MessageDialog(_resLoader.GetString("InfoMessage"));
            await msgbox.ShowAsync();
        }
        private async void mainButton_Click(object sender, RoutedEventArgs e) {
            var buttonIndex = (int)((ToggleButton)sender).Tag;
            if (!GridLogic.Instance.ToggleButton(buttonIndex)) return;
            var msgbox = new MessageDialog(_resLoader.GetString("BingoMessage"));
            await msgbox.ShowAsync();
        }

        private void RestoreOrResetGrid() {
            var gridState = ApplicationData.Current.LocalSettings.Values[GridStateSettingName];
            if (gridState != null) {
                var stringGridState = (string)gridState;
                GridLogic.Instance.FillData(stringGridState);
                for (var i = 0; i < _mainButtons.Length; i++) {
                    _mainButtons[i].IsChecked = stringGridState[i] == '1';
                }
            }
            else {
                ResetGrid();
            }
        }

        private void ResetGrid() {
            GridLogic.Instance.Reset();
            ApplicationData.Current.LocalSettings.Values[GridStateSettingName] = GridLogic.Instance.ExtractData();
            foreach (var tb in _mainButtons) {
                tb.IsChecked = false;
            }
        }
    }
}
