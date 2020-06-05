using Microsoft.UI.Xaml.Controls;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Fluent_WebView_Browser {
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page {
		public MainPage() {
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			base.OnNavigatedTo(e);

		}

		private void TabView_Loaded(object sender, RoutedEventArgs e) {
			(sender as TabView).TabItems.Add(CreateNewTab());
			//CreateNewTab().content
		}

		private void TabView_AddTabButtonClick(TabView sender, object args) {
			sender.TabItems.Add(CreateNewTab());
		}

		private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args) {
			sender.TabItems.Remove(args.Tab);
		}

		private TabViewItem CreateNewTab() {
			TabViewItem newItem = new TabViewItem() {
				Header = "New Tab",
				IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document },
				Content = new TabContentControl()
			};

			return newItem;
		}

		private void TabView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		}
	}
}
