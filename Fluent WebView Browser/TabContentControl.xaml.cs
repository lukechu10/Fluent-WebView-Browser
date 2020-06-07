using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Fluent_WebView_Browser {
	public sealed partial class TabContentControl : UserControl {
		public delegate void DocumentTitleChangedHandler(object sender, string title);
		public event DocumentTitleChangedHandler DocumentTitleChangedEvent;

		public delegate void NewWindowRequestedHandler(WebView sender, WebViewNewWindowRequestedEventArgs args);
		// WebView sent a new window request, create new tab
		public event NewWindowRequestedHandler NewWindowRequested;

		public TabContentControl(Uri uri) {
			this.InitializeComponent();
			WebViewContent.Source = uri;
		}

		private void HrefLocationTextBox_KeyDown(object sender, KeyRoutedEventArgs e) {
			if (e.Key == Windows.System.VirtualKey.Enter)
			// enter key pressed, load new page
			{
				try {
					if (Uri.IsWellFormedUriString(HrefLocationTextBox.Text, UriKind.RelativeOrAbsolute) &&
						/* make sure there is a '.' between domain name and top level domain  (e.g. ".com") */
						HrefLocationTextBox.Text.Contains('.')) {
						// construct new URI
						Uri uri = new UriBuilder(HrefLocationTextBox.Text).Uri;
						// load page
						WebViewContent.Navigate(uri);
						// remove focus from TextBox and set focus on WebView
						WebViewContent.Focus(FocusState.Programmatic);
					}
					else {
						// url is not valid, search on Google instead
						Uri uri = new Uri($"https://www.google.com/search?q={Uri.EscapeDataString(HrefLocationTextBox.Text)}");
						// load page
						WebViewContent.Navigate(uri);
						// remove focus from TextBox and set focus on WebView
						WebViewContent.Focus(FocusState.Programmatic);
					}
				}
				catch (Exception err) {
					// reset href location text box
					HrefLocationTextBox.Text = "";
				}
			}
		}

		private void HrefLocationTextBox_FocusEngaged(object sender, RoutedEventArgs e) {
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

		private void RefreshButton_Click(object sender, RoutedEventArgs e) {
			// determine action based on current symbol icon
			if (RefreshSymbol.Symbol == Symbol.Refresh)
				WebViewContent.Refresh();
			else if (RefreshSymbol.Symbol == Symbol.Cancel)
				WebViewContent.Stop();
		}

		private void WebViewContent_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args) {
			// show stop loading button instead of refresh button
			RefreshSymbol.Symbol = Symbol.Cancel;

			string host = args.Uri.Host;
			string uri = args.Uri.ToString();
			// get start and end of host string in uri
			int hostStartIndex = uri.IndexOf(host);
			int hostEndIndex = hostStartIndex + host.Length;

			string beforeHost = uri.Substring(0, hostStartIndex);
			string afterHost = uri.Substring(hostEndIndex); // get string until the end

			Run beforeHostRun = new Run() { Text = beforeHost };
			Run hostRun = new Run() { Text = host };
			Run afterHostRun = new Run() { Text = afterHost };

			// update HrefLocationTextBox
			HrefLocationTextBox.Text = beforeHost + host + afterHost;
		}

		private void WebViewContent_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args) {
			// show refresh button
			RefreshSymbol.Symbol = Symbol.Refresh;

			//string functionString = "new MutationObserver(function () { window.external.notify(document.title); }).observe(document.querySelector('title'), { childList: true })";
			//await WebViewContent.InvokeScriptAsync("eval", new string[] { functionString });
			DocumentTitleChangedEvent?.Invoke(this, WebViewContent.DocumentTitle);
		}

		private void WebViewContent_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args) {
			// forward event to NewWindowRequested event
			NewWindowRequested?.Invoke(sender, args);
		}
	}
}
