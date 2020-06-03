using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Fluent_WebView_Browser {
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page {
		public MainPage() {
			this.InitializeComponent();
		}

		private void TabView_Loaded(object sender, RoutedEventArgs e) {
			(sender as TabView).TabItems.Add(CreateNewTab());
		}

		private void TabView_AddTabButtonClick(TabView sender, object args) {
			sender.TabItems.Add(CreateNewTab());
		}

		private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args) {
			sender.TabItems.Remove(args.Tab);
		}

		private TabViewItem CreateNewTab() {
			TabViewItem newItem = new TabViewItem() {
				Header = "New Tab"
			};

			return newItem;
		}

		private void HrefLocationTextBox_KeyDown(object sender, KeyRoutedEventArgs e) {
			if (e.Key == VirtualKey.Enter)
			// enter key pressed, load new page
			{
				try {
					// construct new URI
					Uri uri = new UriBuilder(HrefLocationTextBox.Text).Uri;
					// load page
					WebViewContent.Source = uri;
				}
				catch (Exception err) {
					// reset href location text box
					HrefLocationTextBox.Text = "";
				}
			}
		}

		private void NavigationBackward(object sender, RoutedEventArgs e) {
			if (WebViewContent.CanGoBack)
				WebViewContent.GoBack();
		}

		private void NavigationForeward(object sender, RoutedEventArgs e) {
			if (WebViewContent.CanGoForward)
				WebViewContent.GoForward();
		}
	}
}
