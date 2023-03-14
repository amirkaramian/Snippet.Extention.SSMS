using EnvDTE;
using EnvDTE80;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.Settings.Internal;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using System.Xml.Serialization;
using Snippet.Extention.Snippet;
using Task = System.Threading.Tasks.Task;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using Formatting = Newtonsoft.Json.Formatting;
using Microsoft.VisualStudio.Debugger.Symbols;
using System.Linq;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualBasic;

using System.Runtime.Remoting;
using Snippet.Extention.Definitions;
using System.Windows;
using Community.VisualStudio.Toolkit;
using System.Windows.Interop;

namespace Snippet.Extention
{
    /// <summary>
    /// Command handler
    /// </summary>

    internal sealed class KeyCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Cursor pointer
        /// </summary>
        private const string Cursor = "<Cursor>";

        /// <summary>
        /// Shortkey
        /// </summary>
        private const string ShortKey = "xxx";

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("913c5552-9527-4aef-bf96-cdee352aa656");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;
        private readonly OleMenuCommandService oleCommandService;
        private readonly CommandID menuCommandID;
        private readonly DTE2 dteObject;
        private TextEditorEvents textEditorEvents;
        private EnvDTE.DocumentEvents windowEvents;
        private KeyboardHook hook;
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>        
        private KeyCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            oleCommandService = commandService;
            menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.ExecuteAsync, menuCommandID);
            commandService.AddCommand(menuItem);
            //**************************************           
            dteObject = (DTE2)(ServiceProvider.GetServiceAsync(typeof(DTE)).Result);
            textEditorEvents = dteObject.Events.TextEditorEvents;
            textEditorEvents.LineChanged += TextEditorEvents_LineChanged;
            windowEvents = dteObject.Events.DocumentEvents;
            windowEvents.DocumentOpened += WindowEvents_DocumentOpened; ;
            //************
          
        }
        string key = string.Empty;
        private void Keymgr_KeyDown(object sender, KeyDownEventArgs e)
        {
            var textDoc = dteObject.ActiveDocument?.Object("TextDocument") as TextDocument;
            if (textDoc == null)
                return;
            var ptr = KeyboarManager.FindWindow(textDoc.Parent.Name);
            var activeHandler = KeyboarManager.GetForegroundWindow();
            if (activeHandler != (sender as KeyboarManager)._window.Handle) return;
            key += e.Key;
            if (key.ToLower().Trim().EndsWith(ShortKey) )
            {
                FetchSnippet(key.ToLower());
                
            }
        }

        private async void WindowEvents_DocumentOpened(Document document)
        {
            key = string.Empty;
            var keymgr = new KeyboarManager(KeyboarManager.GetForegroundWindow());
            keymgr.KeyDown += new EventHandler<KeyDownEventArgs>(Keymgr_KeyDown);
            keymgr._window.Start();
       
        }



     


        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static KeyCommand Instance
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
            Instance = new KeyCommand(package, commandService);

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
            string title = "Snippet";

            //*********************************************************
            try
            {

                TextSelection ts = (TextSelection)dteObject.ActiveDocument.Selection;
                string query = ts.Text;
                if (string.IsNullOrEmpty(query))
                {
                    VsShellUtilities.ShowMessageBox(
                       this.package, "select query for creating snippet", title,
                       OLEMSGICON.OLEMSGICON_INFO,
                       OLEMSGBUTTON.OLEMSGBUTTON_OK,
                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                    return;
                }
                if (!Snippethelper.ValidateQuery(query))
                {
                    VsShellUtilities.ShowMessageBox(
                           this.package, "enter a valid tsql query", title,
                           OLEMSGICON.OLEMSGICON_INFO,
                           OLEMSGBUTTON.OLEMSGBUTTON_OK,
                           OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                    return;
                }
                using (PromptForm prp = new PromptForm())
                {
                    if (prp.ShowDialog() != DialogResult.OK)
                        return;
                    Snippethelper.MakeSnippet(query, Variables.Storedpath, prp.Name, prp.Description);

                    VsShellUtilities.ShowMessageBox(
                         this.package, "snippet is created successfully ", title,
                         OLEMSGICON.OLEMSGICON_INFO,
                         OLEMSGBUTTON.OLEMSGBUTTON_OK,
                         OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                }

            }
            catch (Exception ex)
            {
                VsShellUtilities.ShowMessageBox(
                        this.package, ex.Message, title,
                        OLEMSGICON.OLEMSGICON_INFO,
                        OLEMSGBUTTON.OLEMSGBUTTON_OK,
                        OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }

        }

        /// <summary>
        /// Editor event
        /// </summary>
        /// <param name="StartPoint"></param>
        /// <param name="EndPoint"></param>
        /// <param name="Hint"></param>
        private void TextEditorEvents_LineChanged(TextPoint StartPoint, TextPoint EndPoint, int Hint)
        {
            //FetchSnippet();
        }

        /// <summary>
        /// Fetch Snippet
        /// </summary>
        public void FetchSnippet()
        {
            if (dteObject.ActiveDocument == null)
                return;
            var textDoc = dteObject.ActiveDocument.Object("TextDocument") as TextDocument;
            if (textDoc == null)
                return;
            //Check keyCode
            var editpoint = textDoc.StartPoint.CreateEditPoint();
            string txtSelected = editpoint.GetText(textDoc.EndPoint);

            var line = textDoc.Selection.ActivePoint.Line;
            var offset = textDoc.Selection.ActivePoint.LineCharOffset;
            var lines = txtSelected.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var txt = string.Empty; //TODO:Resolve offset
            if (lines.Length > 0 && lines[line - 1].Length > 2 && offset > ShortKey.Length)
                txt = lines[line - 1].Substring(offset - ShortKey.Length - 1, ShortKey.Length).Trim();

            if (!string.IsNullOrEmpty(txt) && txt.TrimEnd().Contains(ShortKey))
            {
                using (var snForm = new SnippetListForm())
                {
                    if (snForm.ShowDialog() != DialogResult.OK)
                        return;
                    var snipetText = Snippethelper.LoadSnippet(snForm.CurrentSnippet);
                    if (string.IsNullOrEmpty(snipetText))
                        return;
                    Task.Run(() =>
                    {
                        textDoc.ReplacePattern(ShortKey, snipetText);
                        ChekCursor();
                    });

                }
            }
        }

        public void FetchSnippet(string txt)
        {
            if (dteObject.ActiveDocument == null)
                return;
            var textDoc = dteObject.ActiveDocument.Object("TextDocument") as TextDocument;
            if (textDoc == null)
                return;
            ////Check keyCode
            var editpoint = textDoc.StartPoint.CreateEditPoint();
            txt = editpoint.GetText(textDoc.EndPoint);

            //var line = textDoc.Selection.ActivePoint.Line;
            //var offset = textDoc.Selection.ActivePoint.LineCharOffset;
            //var lines = txtSelected.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            //var txt = string.Empty; //TODO:Resolve offset
            //if (lines.Length > 0 && lines[line - 1].Length > 2 && offset > ShortKey.Length)
            //    txt = lines[line - 1].Substring(offset - ShortKey.Length - 1, ShortKey.Length).Trim();

            if (!string.IsNullOrEmpty(txt) && txt.TrimEnd().Contains(ShortKey))
            {
                key = string.Empty;
                using (var snForm = new SnippetListForm())
                {
                    if (snForm.ShowDialog() != DialogResult.OK)
                        return;
                    var snipetText = Snippethelper.LoadSnippet(snForm.CurrentSnippet);
                    if (string.IsNullOrEmpty(snipetText))
                        return;
                    Task.Run(() =>
                    {
                        textDoc.ReplacePattern(ShortKey, snipetText);
                        ChekCursor();
                    });

                }
            }
        }
        public void ChekCursor()
        {
            TextSelection objSel = dteObject.ActiveDocument.Selection as TextSelection;

            objSel.StartOfDocument();
            if (objSel.FindPattern(Cursor))
            {
                int lStartLine = objSel.TopPoint.Line;
                int lStartColumn = objSel.TopPoint.LineCharOffset;
                objSel.MoveToLineAndOffset(lStartLine, lStartColumn, true);
                objSel.Delete(Cursor.Length);
            }
        }

    }
}
