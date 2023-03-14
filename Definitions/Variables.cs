using Microsoft.SqlServer.Dac.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snippet.Extention.Definitions
{
    internal class Variables
    {
        public static string Storedpath => Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "customSnippet"); //"C:\\Program Files (x86)\\Microsoft SQL Server Management Studio 19\\Common7\\IDE\\SQL\\Snippets\\1033\\CustomeSnippet";

    }
}
