using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using System.Windows.Shapes;

namespace Snippet.Extention
{
    internal class HighlightCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0102;
        private const string Cursor = "<Cursor>";
        private const string HighlightStart = "<Highlight>";
        private const string HighlightEnd = "</Highlight>";
        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("913c5552-9527-4aef-bf96-cdee352aa658");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;
        private readonly OleMenuCommandService oleCommandService;
        private readonly CommandID menuCommandID;
        private readonly DTE2 dteObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>        
        private HighlightCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            oleCommandService = commandService;
            menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.ExecuteAsync, menuCommandID);
            commandService.AddCommand(menuItem);
            //***************************************
            dteObject = (DTE2)(ServiceProvider.GetServiceAsync(typeof(DTE)).Result);
        }
        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static HighlightCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in KeyCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new HighlightCommand(package, commandService);

        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void ExecuteAsync(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            string title = "Highlight Command";

            //*********************************************************
            if (dteObject.ActiveDocument == null)
                return;
            var textDoc = dteObject.ActiveDocument.Object("TextDocument") as TextDocument;
            if (textDoc == null)
                return;
            var editpoint = textDoc.StartPoint.CreateEditPoint();
            string txt = editpoint.GetText(textDoc.EndPoint);
            
            
            if (!string.IsNullOrEmpty(txt) && (txt.Contains(Cursor) || txt.Contains(HighlightStart) || txt.Contains(HighlightEnd)))
                return;
            if (!Snippethelper.ValidateQuery(txt))
            {
                VsShellUtilities.ShowMessageBox(
                       this.package, "Enter a valid tsql query", title,
                       OLEMSGICON.OLEMSGICON_INFO,
                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                return;
            }

            TextSelection ts = (TextSelection)dteObject.ActiveDocument.Selection;
            if (string.IsNullOrEmpty(ts.Text))
            {
                VsShellUtilities.ShowMessageBox(
                                       this.package, "Highlite a text or sentence ", title,
                                       OLEMSGICON.OLEMSGICON_INFO,
                                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
                                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                return;
            }
            ts.ReplacePattern(ts.Text, $"{HighlightStart}{ts.Text}{HighlightEnd}");


            ////*********************************************************
            VsShellUtilities.ShowMessageBox(
                 this.package, "Highlite is added", title,
                 OLEMSGICON.OLEMSGICON_INFO,
                 OLEMSGBUTTON.OLEMSGBUTTON_OK,
                 OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

    }
}
