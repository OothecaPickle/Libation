using Avalonia.Collections;
using Avalonia.Controls;
using Dinah.Core;
using FileManager;
using LibationAvalonia.ViewModels.Settings;
using LibationFileManager;
using LibationUiBase;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibationAvalonia.Dialogs
{
	public partial class SettingsDialog : DialogWindow
	{
		private SettingsVM settingsDisp;

		private readonly Configuration config = Configuration.Instance;
		public SettingsDialog()
		{
			if (Design.IsDesignMode)
				_ = Configuration.Instance.LibationFiles;
			InitializeComponent();

			DataContext = settingsDisp = new(config);
		}

		protected override async Task SaveAndCloseAsync()
		{
			#region validation

			if (string.IsNullOrWhiteSpace(settingsDisp.ImportantSettings.BooksDirectory))
			{
				await MessageBox.Show(this.GetParentWindow(), "Cannot set Books Location to blank", "Location is blank", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			#endregion

			settingsDisp.SaveSettings(config);

			await MessageBox.VerboseLoggingWarning_ShowIfTrue();
			await base.SaveAndCloseAsync();
		}

		public async void SaveButton_Clicked(object sender, Avalonia.Interactivity.RoutedEventArgs e)
			=> await SaveAndCloseAsync();

		public void OpenLogFolderButton_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			Go.To.Folder(((LongPath)Configuration.Instance.LibationFiles).ShortPathName);
		}
	}
}
