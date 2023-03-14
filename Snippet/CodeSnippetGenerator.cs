using Microsoft.SqlServer.TransactSql.ScriptDom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Snippet.Extention.Snippet;
using Snippet.Extention.Definitions;

namespace Snippet.Extention
{
    internal class Snippethelper
    {
        private static string xmlSnippet = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<CodeSnippets xmlns=""http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet"">
<_locDefinition xmlns=""urn:locstudio"">
    <_locDefault _loc=""locNone"" />
    <_locTag _loc=""locData"">Title</_locTag>
    <_locTag _loc=""locData"">Description</_locTag>
    <_locTag _loc=""locData"">Author</_locTag>
    <_locTag _loc=""locData"">ToolTip</_locTag>
</_locDefinition>
	<CodeSnippet Format=""1.0.0"">
	<Header>
	<Title>Create Table</Title>
        <Shortcut></Shortcut>
	<Description>Creates a table.</Description>
	<Author>Microsoft Corporation</Author>
	<SnippetTypes>
		<SnippetType>Expansion</SnippetType>
	</SnippetTypes>
	</Header>
	<Snippet>
		<Declarations>
			
		</Declarations>
		<Code Language=""SQL"">
			
		</Code>
	</Snippet>
	</CodeSnippet>
</CodeSnippets>

";
        public static void MakeSnippet(string query, string path, string name, string descipriotn)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            XmlSerializer serializer = new XmlSerializer(typeof(CodeSnippets));
            StringReader rdr = new StringReader(xmlSnippet);
            var snipet = (CodeSnippets)serializer.Deserialize(rdr);
            ParsQuery(snipet, query);
            snipet.CodeSnippet.Header.Title = name;
            snipet.CodeSnippet.Header.Description = descipriotn;
            var sww = new StringWriter();
            var writer = XmlWriter.Create(sww);
            serializer.Serialize(writer, snipet);
            var xml = sww.ToString().Replace("&lt;", "<").Replace("&gt;", ">");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Save($@"{path}\{name}.snippet");

        }

        public static string LoadSnippet(string name)
        {

            string xml = File.ReadAllText(name);
            var stReader = new StringReader(xml);
            XmlSerializer serializer = new XmlSerializer(typeof(CodeSnippets));
            var snipet = (CodeSnippets)serializer.Deserialize(stReader);
            var commnad = snipet.CodeSnippet.Snippet.Code.Value.Replace(";$end$", "");
            foreach (var item in snipet.CodeSnippet.Snippet.Declarations)
                commnad = commnad.Replace(item.ID, item.Default);

            commnad = commnad.Replace("$", "");
            return commnad;
        }
        public static List<FileInfo> GetAllCustomeSnippets()
        {
            var path = Variables.Storedpath;
            var dir = new DirectoryInfo(path);
            if (!dir.Exists)
                dir.Create();
            return dir.GetFiles("*.snippet").ToList();
        }
        private static void ParsQuery(CodeSnippets snipet, string query)
        {

            using (var rdr = new StringReader(query))
            {
                IList<ParseError> errors = null;
                var parser = new TSql150Parser(true, SqlEngineType.All);
                var tree = parser.Parse(rdr, out errors);
                var cdata = new StringBuilder();
                int indetifier = 0;
                if (!tree.ScriptTokenStream.Any(x => x.IsKeyword()))
                    throw new Exception("Query does not have a valid keywords");
                foreach (var item in tree.ScriptTokenStream)
                {
                    if (item.IsKeyword())
                        cdata.Append(item.Text);
                    else if (item.TokenType == TSqlTokenType.QuotedIdentifier)
                    {
                        var text = makeIdentifier(item.Text, ++indetifier);
                        cdata.Append(text);
                        snipet.CodeSnippet.Snippet.Declarations.Add(new CodeSnippetsCodeSnippetSnippetLiteral()
                        {
                            ID = text.Replace("$", ""),
                            Default = item.Text.Replace("[", "").Replace("]", ""),
                            ToolTip = item.Text.Replace("[", "").Replace("]", ""),
                        });
                    }
                    else if (item.TokenType == TSqlTokenType.Identifier)
                    {
                        var text = makeIdentifier(item.Text, ++indetifier);
                        cdata.Append(text);
                        snipet.CodeSnippet.Snippet.Declarations.Add(new CodeSnippetsCodeSnippetSnippetLiteral()
                        {
                            ID = text.Replace("$", ""),
                            Default = item.Text.Replace("[", "").Replace("]", ""),
                            ToolTip = item.Text.Replace("[", "").Replace("]", ""),
                        });
                    }
                    else
                        cdata.Append(item.Text);
                }
                var data = cdata.ToString();
                snipet.CodeSnippet.Snippet.Code.Value = $@"<![CDATA[{data};$end$]]>";
            }
        }
        public static bool ValidateQuery(string query)
        {
            using (var rdr = new StringReader(query))
            {
                IList<ParseError> errors = null;
                var parser = new TSql150Parser(true, SqlEngineType.All);
                var tree = parser.Parse(rdr, out errors);
                var cdata = new StringBuilder();
                int indetifier = 0;
                if (!tree.ScriptTokenStream.Any(x => x.IsKeyword()))
                    return false;
                return true;
            }
        }
        private static string makeIdentifier(string text, int index)
        {
            if (text == " ")
                return " ";
            return $"$identifier{index}$";
        }
    }
}
