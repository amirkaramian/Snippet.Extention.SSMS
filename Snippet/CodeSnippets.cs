using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Snippet.Extention
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet", IsNullable = false)]
    public partial class CodeSnippets
    {

        private _locDefinition _locDefinitionField;

        private CodeSnippetsCodeSnippet codeSnippetField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn:locstudio")]
        public _locDefinition _locDefinition
        {
            get
            {
                return this._locDefinitionField;
            }
            set
            {
                this._locDefinitionField = value;
            }
        }

        /// <remarks/>
        public CodeSnippetsCodeSnippet CodeSnippet
        {
            get
            {
                return this.codeSnippetField;
            }
            set
            {
                this.codeSnippetField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:locstudio")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:locstudio", IsNullable = false)]
    public partial class _locDefinition
    {

        private _locDefinition_locDefault _locDefaultField;

        private _locDefinition_locTag[] _locTagField;

        /// <remarks/>
        public _locDefinition_locDefault _locDefault
        {
            get
            {
                return this._locDefaultField;
            }
            set
            {
                this._locDefaultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("_locTag")]
        public _locDefinition_locTag[] _locTag
        {
            get
            {
                return this._locTagField;
            }
            set
            {
                this._locTagField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:locstudio")]
    public partial class _locDefinition_locDefault
    {

        private string _locField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string _loc
        {
            get
            {
                return this._locField;
            }
            set
            {
                this._locField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:locstudio")]
    public partial class _locDefinition_locTag
    {

        private string _locField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string _loc
        {
            get
            {
                return this._locField;
            }
            set
            {
                this._locField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
    public partial class CodeSnippetsCodeSnippet
    {

        private CodeSnippetsCodeSnippetHeader headerField;

        private CodeSnippetsCodeSnippetSnippet snippetField;

        private string formatField;

        /// <remarks/>
        public CodeSnippetsCodeSnippetHeader Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        public CodeSnippetsCodeSnippetSnippet Snippet
        {
            get
            {
                return this.snippetField;
            }
            set
            {
                this.snippetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
    public partial class CodeSnippetsCodeSnippetHeader
    {

        private string titleField;

        private object shortcutField;

        private string descriptionField;

        private string authorField;

        private CodeSnippetsCodeSnippetHeaderSnippetTypes snippetTypesField;

        /// <remarks/>
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public object Shortcut
        {
            get
            {
                return this.shortcutField;
            }
            set
            {
                this.shortcutField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string Author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        public CodeSnippetsCodeSnippetHeaderSnippetTypes SnippetTypes
        {
            get
            {
                return this.snippetTypesField;
            }
            set
            {
                this.snippetTypesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
    public partial class CodeSnippetsCodeSnippetHeaderSnippetTypes
    {

        private string snippetTypeField;

        /// <remarks/>
        public string SnippetType
        {
            get
            {
                return this.snippetTypeField;
            }
            set
            {
                this.snippetTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
    public partial class CodeSnippetsCodeSnippetSnippet
    {

        private List<CodeSnippetsCodeSnippetSnippetLiteral> declarationsField;

        private CodeSnippetsCodeSnippetSnippetCode codeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Literal", IsNullable = false)]
        public List<CodeSnippetsCodeSnippetSnippetLiteral> Declarations
        {
            get
            {
                return this.declarationsField;
            }
            set
            {
                this.declarationsField = value;
            }
        }

        /// <remarks/>
        public CodeSnippetsCodeSnippetSnippetCode Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
    public partial class CodeSnippetsCodeSnippetSnippetLiteral
    {

        private string idField;

        private string toolTipField;

        private string defaultField;

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string ToolTip
        {
            get
            {
                return this.toolTipField;
            }
            set
            {
                this.toolTipField = value;
            }
        }

        /// <remarks/>
        public string Default
        {
            get
            {
                return this.defaultField;
            }
            set
            {
                this.defaultField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet")]
    public partial class CodeSnippetsCodeSnippetSnippetCode
    {

        private string languageField;
        [XmlText]
        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlTextAttribute()]
        [XmlText]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }


}
