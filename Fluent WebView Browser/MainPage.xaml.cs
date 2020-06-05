using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
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

			var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
			coreTitleBar.ExtendViewIntoTitleBar = true;
			coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
			
			var titleBar = ApplicationView.GetForCurrentView().TitleBar;
			titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
			titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;

			Window.Current.SetTitleBar(CustomDragRegion);
		}

		private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args) {
			// To ensure that the tabs in the titlebar are not occluded by shell
			// content, we must ensure that we account for left and right overlays.
			// In LTR layouts, the right inset includes the caption buttons and the
			// drag region, which is flipped in RTL. 

			// The SystemOverlayLeftInset and SystemOverlayRightInset values are
			// in terms of physical left and right. Therefore, we need to flip
			// then when our flow direction is RTL.
			if (FlowDirection == FlowDirection.LeftToRight) {
				CustomDragRegion.MinWidth = sender.SystemOverlayRightInset;
				ShellTitlebarInset.MinWidth = sender.SystemOverlayLeftInset;
			}
			else {
				CustomDragRegion.MinWidth = sender.SystemOverlayLeftInset;
				ShellTitlebarInset.MinWidth = sender.SystemOverlayRightInset;
			}

			// Ensure that the height of the custom regions are the same as the titlebar.
			CustomDragRegion.Height = ShellTitlebarInset.Height = sender.Height;
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
