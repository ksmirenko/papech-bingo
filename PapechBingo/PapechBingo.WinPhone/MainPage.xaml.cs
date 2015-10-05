using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace PapechBingo.WinPhone {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        private string GRID_STATE_SETTING_NAME = "GridState";
        private ToggleButton[] mainButtons;
        private ResourceLoader resLoader = new ResourceLoader();
        private ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;

        public MainPage() {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            SetupButtonGrid();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            object gridState = settings.Values[GRID_STATE_SETTING_NAME];
            if (gridState != null) {
                GridLogic.Instance.FillData((string)gridState);
            }
            else {
                GridLogic.Instance.Reset();
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e) {
            settings.Values[GRID_STATE_SETTING_NAME] = GridLogic.Instance.ExtractData();
        }

        private void SetupButtonGrid() {
            var gridSize = GridLogic.GRID_SIZE;
            // calculating button height
            var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            var buttonSize = Window.Current.Bounds.Width * scaleFactor / gridSize;
            // setting up rows and columns
            for (var i = 0; i < gridSize; i++) {
                var colDef = new ColumnDefinition();
                colDef.Width = new GridLength(1, GridUnitType.Star);
                mainButtonsGrid.ColumnDefinitions.Add(colDef);
            }
            for (var i = 0; i < gridSize; i++) {
                var rowDef = new RowDefinition();
                rowDef.Height = new GridLength(1, GridUnitType.Star);
                mainButtonsGrid.RowDefinitions.Add(rowDef);
            }
            // obtaining buttons content - a shameful workaround
            var buttonStrings = resLoader.GetString("MainButtonsContent").Split(';');
            // creating and adding buttons
            mainButtons = new ToggleButton[gridSize * gridSize];
            for (var i = 0; i < gridSize; i++) {
                for (var j = 0; j < gridSize; j++) {
                    var bIndex = i * gridSize + j;
                    var b = new ToggleButton();
                    b.HorizontalAlignment = HorizontalAlignment.Stretch;
                    b.VerticalAlignment = VerticalAlignment.Stretch;
                    b.Height = buttonSize;
                    b.Width = buttonSize;
                    b.SetValue(Grid.RowProperty, i);
                    b.SetValue(Grid.ColumnProperty, j);
                    b.Padding = new Thickness(0);
                    b.Margin = new Thickness(0, -11, 0, -11);
                    // setting button content
                    var textBlock = new TextBlock();
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    textBlock.FontSize = 12;
                    textBlock.FontStretch = Windows.UI.Text.FontStretch.ExtraCondensed;
                    textBlock.Text = buttonStrings[bIndex];
                    b.HorizontalContentAlignment = HorizontalAlignment.Left;
                    b.Padding = new Thickness(4, 1, 1, 1);
                    b.Content = textBlock;
                    // adding OnClickListener
                    b.Tag = bIndex;
                    b.Click += mainButton_Click;
                    // saving the button and adding to grid
                    mainButtons[bIndex] = b;
                    mainButtonsGrid.Children.Add(b);
                }
            }
        }

        private void buttonReset_Click(object sender, RoutedEventArgs e) {
            for (var i = 0; i < GridLogic.GRID_SIZE * GridLogic.GRID_SIZE; i++) {
                mainButtons[i].IsChecked = false;
            }
            GridLogic.Instance.Reset();
        }
        private async void buttonInfo_Click(object sender, RoutedEventArgs e) {
            MessageDialog msgbox = new MessageDialog(resLoader.GetString("InfoMessage"));
            await msgbox.ShowAsync();
        }
        private async void mainButton_Click(object sender, RoutedEventArgs e) {
            var buttonIndex = (int)((ToggleButton)sender).Tag;
            if (GridLogic.Instance.ToggleButton(buttonIndex)) {
                MessageDialog msgbox = new MessageDialog(resLoader.GetString("BingoMessage"));
                await msgbox.ShowAsync();
            }
        }
    }
}
