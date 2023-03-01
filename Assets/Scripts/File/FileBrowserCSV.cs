using UnityEngine;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using Enums;

public class FileBrowserCSV : MonoBehaviour
{
	// Warning: paths returned by FileBrowser dialogs do not contain a trailing '\' character
	// Warning: FileBrowser can only show 1 dialog at a time

	public Trajectory trajectoryType = Trajectory.Target;
	public string targetFileName = "";
	public string predictedFileName = "";
	
	
	#region Events

	public delegate void FileBrowserEvent(BrowserEvent eventType, string filePathOrigin, string fileName, string filePathPersist);
	public static event FileBrowserEvent OnFileBrowserEvent;

	#endregion
	
	void Start()
	{
		// Set filters (optional)
		// It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
		// if all the dialogs will be using the same filters
		FileBrowser.SetFilters( true, new FileBrowser.Filter( "Images", ".jpg", ".png" ), new FileBrowser.Filter( "Text Files", ".txt", ".pdf" ) );

		// Set default filter that is selected when the dialog is shown (optional)
		// Returns true if the default filter is set successfully
		// In this case, set Images filter as the default filter
		// FileBrowser.SetDefaultFilter( ".jpg" );
		FileBrowser.SetDefaultFilter( ".csv" );

		// Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
		// Note that when you use this function, .lnk and .tmp extensions will no longer be
		// excluded unless you explicitly add them as parameters to the function
		FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );

		// Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
		// It is sufficient to add a quick link just once
		// Name: Users
		// Path: C:\Users
		// Icon: default (folder icon)
		FileBrowser.AddQuickLink( "Users", "C:\\Users", null );

		// Show a save file dialog 
		// onSuccess event: not registered (which means this dialog is pretty useless)
		// onCancel event: not registered
		// Save file/folder: file, Allow multiple selection: false
		// Initial path: "C:\", Initial filename: "Screenshot.png"
		// Title: "Save As", Submit button text: "Save"
		// FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "Save As", "Save" );

		// Show a select folder dialog 
		// onSuccess event: print the selected folder's path
		// onCancel event: print "Canceled"
		// Load file/folder: folder, Allow multiple selection: false
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Select Folder", Submit button text: "Select"
		// FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
		//						   () => { Debug.Log( "Canceled" ); },
		//						   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );

		// Coroutine example
		//StartCoroutine( ShowLoadDialogCoroutine() );
	}

	//browser 1=target 2=predicted
	public void OpenBrowser(int browser){
		if (browser == 1){
			trajectoryType = Trajectory.Target;
		}
		if (browser == 2){
			trajectoryType = Trajectory.Predicted;
		}
		StartCoroutine( ShowLoadDialogCoroutine() );
	}
	IEnumerator ShowLoadDialogCoroutine()
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: both, Allow multiple selection: true
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Load File", Submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );

		// Dialog is closed
		// Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
		Debug.Log( FileBrowser.Success );
		
		if (OnFileBrowserEvent != null){
			OnFileBrowserEvent(BrowserEvent.Opened, "", "", "");
		}

		if( FileBrowser.Success )
		{
			// Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
			for (int i = 0; i < FileBrowser.Result.Length; i++){
				Debug.Log( FileBrowser.Result[i] );
			}

			// Read the bytes of the first file via FileBrowserHelpers
			// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
			// byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( FileBrowser.Result[0] );

			// Or, copy the first file to persistentDataPath
			string destinationPath = Path.Combine( Application.persistentDataPath, FileBrowserHelpers.GetFilename( FileBrowser.Result[0] ) );
			
			//check if the file is correct extension
			if (FileUtilities.CheckFileExtension(destinationPath, ".csv")){
				Debug.Log("CSV Selected: "+FileUtilities.CheckFileExtension(destinationPath, ".csv"));
				
				if (trajectoryType == Trajectory.Target){
					//use a utility to get the filename and extension only from the file and path provided by the filebrowser
					// targetFileName = TransformUtilities.GetFileName(FileBrowser.Result[0], "csv");
					targetFileName = FileUtilities.GetFileName(FileBrowser.Result[0], "csv");
				
					if (targetFileName != null){
						DAO.instance.TargetFilePath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
						FileBrowserHelpers.CopyFile( FileBrowser.Result[0], destinationPath );

						yield return new WaitForFixedUpdate();
						//send a load event
						if (OnFileBrowserEvent != null){
							OnFileBrowserEvent(BrowserEvent.LoadedTarget, FileBrowser.Result[0], targetFileName, 
								Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0])));
						}
					}
				}
			
				if (trajectoryType == Trajectory.Predicted){
					//use a utility to get the filename and extension only from the file and path provided by the filebrowser
					predictedFileName = FileUtilities.GetFileName(FileBrowser.Result[0], "csv");
				
					if (predictedFileName != null){
						DAO.instance.PredictedFilePath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
						FileBrowserHelpers.CopyFile( FileBrowser.Result[0], destinationPath );
					
						yield return new WaitForFixedUpdate();
						//send a load event
						if (OnFileBrowserEvent != null){
							OnFileBrowserEvent(BrowserEvent.LoadedPredicted, FileBrowser.Result[0], predictedFileName, 
								Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0])));
						}
					}
				}
				
			}
			else{
				Debug.Log("Select a CSV File please");
				
				//maybe send something to alert the file is wrong
			}
			
			


		}
	}
}
