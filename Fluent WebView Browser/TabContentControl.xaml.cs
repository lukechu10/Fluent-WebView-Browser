using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fluent_WebView_Browser {
	public sealed partial class TabContentControl : UserControl {
		public TabContentControl() {
			this.InitializeComponent();
		}

		private void HrefLocationTextBox_KeyDown(object sender, KeyRoutedEventArgs e) {
			if (e.Key == Windows.System.VirtualKey.Enter)
			// enter key pressed, load new page
			{
				try {
					// construct new URI
					Uri uri = new UriBuilder(HrefLocationTextBox.Text).Uri;
					// load page
					WebViewContent.Navigate(uri);
					// remove focus from TextBox and set focus on WebView
					WebViewContent.Focus(FocusState.Programmatic);
				}
				catch (Exception err) {
					// reset href location text box
					HrefLocationTextBox.Text = "";
				}
			}
		}

		private void HrefLocationTextBox_FocusEngaged(Control sender, FocusEngagedEventArgs args) {
			// select all text in text box
			(sender as TextBox).SelectAll();
		}

		private void NavigationBackward(object sender, RoutedEventArgs e) {
			if (WebViewContent.CanGoBack)
				WebViewContent.GoBack();
		}

		private void NavigationForward(object sender, RoutedEventArgs e) {
			if (WebViewContent.CanGoForward)
				WebViewContent.GoForward();
		}

		private void WebViewContent_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args) {
			// update HrefLocationTextBox
			HrefLocationTextBox.Text = args.Uri.ToString();
		}
	}
}
