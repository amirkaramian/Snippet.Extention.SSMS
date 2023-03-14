using EnvDTE;
using EnvDTE80;
using Microsoft.VisualBasic;
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

namespace Snippet.Extention
{
    internal sealed class PointerCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0101;
        private const string Cursor = "<Cursor>";
        private const string HighlightStart = "<Highlight>";
        private const string HighlightEnd = "</Highlight>";
        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("913c5552-9527-4aef-bf96-cdee352aa657");

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
        private PointerCommand(AsyncPackage package, OleMenuCommandService commandService)
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
        public static PointerCommand Instance
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
            Instance = new PointerCommand(package, commandService);

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
            string title = "PointerCommand";

            //*********************************************************
            if (dteObject.ActiveDocument == null)
                return;
            var textDoc = dteObject.ActiveDocument.Object("TextDocument") as TextDocument;
            if (textDoc == null)
                return;
            var editpoint = textDoc.StartPoint.CreateEditPoint();
            string txt = editpoint.GetText(textDoc.EndPoint);

            if (string.IsNullOrEmpty(txt) || (!string.IsNullOrEmpty(txt) && (txt.Contains(Cursor) || txt.Contains(HighlightStart) || txt.Contains(HighlightEnd))))
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
            var line = textDoc.Selection.ActivePoint.Line;
            var offset = textDoc.Selection.ActivePoint.LineCharOffset;

            textDoc.Selection.MoveToLineAndOffset(line, offset);
            editpoint = textDoc.Selection.ActivePoint.CreateEditPoint();
            editpoint.Insert(Cursor);


            ////*********************************************************
            VsShellUtilities.ShowMessageBox(
                 this.package, "Cursor is added", title,
                 OLEMSGICON.OLEMSGICON_INFO,
                 OLEMSGBUTTON.OLEMSGBUTTON_OK,
                 OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

        }

    }
}
