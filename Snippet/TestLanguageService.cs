using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace Snippet.Extention.Snippet
{
   public class TestLanguageService //: LanguageService
    {
        

        public void GetSnippets(Guid languageGuid, ref ArrayList expansionsList)
        {
            IVsTextManager textManager = (IVsTextManager)Package.GetGlobalService(typeof(SVsTextManager));
            if (textManager != null)
            {
                IVsTextManager2 textManager2 = (IVsTextManager2)textManager;
                if (textManager2 != null)
                {
                    IVsExpansionManager expansionManager = null;
                    textManager2.GetExpansionManager(out expansionManager);
                    if (expansionManager != null)
                    {
                        // Tell the environment to fetch all of our snippets.
                        IVsExpansionEnumeration expansionEnumerator = null;
                        
                        expansionManager.EnumerateExpansions(languageGuid,
                        0,     // return all info
                        null,    // return all types
                        0,     // return all types
                        1,     // include snippets without types
                        0,     // do not include duplicates
                        out expansionEnumerator);
                        if (expansionEnumerator != null)
                        {
                            // Cache our expansions in a VsExpansion array
                            uint count = 0;
                            uint fetched = 0;
                            VsExpansion expansionInfo = new VsExpansion();
                            IntPtr[] pExpansionInfo = new IntPtr[1];

                            // Allocate enough memory for one VSExpansion structure. This memory is filled in by the Next method.
                            pExpansionInfo[0] = Marshal.AllocCoTaskMem(Marshal.SizeOf(expansionInfo));

                            expansionEnumerator.GetCount(out count);
                            for (uint i = 0; i < count; i++)
                            {
                                expansionEnumerator.Next(1, pExpansionInfo, out fetched);
                                if (fetched > 0)
                                {
                                    // Convert the returned blob of data into a structure that can be read in managed code.
                                    expansionInfo = (VsExpansion)Marshal.PtrToStructure(pExpansionInfo[0], typeof(VsExpansion));

                                    if (!String.IsNullOrEmpty(expansionInfo.shortcut))
                                    {
                                        expansionsList.Add(expansionInfo);
                                    }
                                }
                            }
                            Marshal.FreeCoTaskMem(pExpansionInfo[0]);
                        }
                    }
                }
            }
        }
    }
}
