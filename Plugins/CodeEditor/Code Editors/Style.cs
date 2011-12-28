using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.ComponentModel;
using System.Drawing;
using Alsing.SourceCode;
namespace CodeEditor
{

	public class Style : Component
	{

		private Container _components;
		public string Name = "";

		public TextStyle TextStyle;
		public Style(IContainer container)
		{
			container.Add(this);
			InitializeComponent();
		}

		public Style()
		{
			InitializeComponent();
		}

		[Category("Color")]
		public Color ForeColor {
			get { return TextStyle.ForeColor; }
			set { TextStyle.ForeColor = value; }
		}

		[Category("Color")]
		public Color BackColor {
			get { return TextStyle.BackColor; }
			set { TextStyle.BackColor = value; }
		}

		[Category("Font")]
		public bool FontBold {
			get { return TextStyle.Bold; }

			set { TextStyle.Bold = value; }
		}

		[Category("Font")]
		public bool FontItalic {
			get { return TextStyle.Italic; }
			set { TextStyle.Italic = value; }
		}

		[Category("Font")]
		public bool FontUnderline {
			get { return TextStyle.Underline; }
			set { TextStyle.Underline = value; }
		}

		public Container Components {
			get { return _components; }
		}


		public override string ToString()
		{
			return Name;
		}

		#region " Component Designer generated code "

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			_components = new System.ComponentModel.Container();
		}

		#endregion
	}
}
