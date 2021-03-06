﻿/*
 * Created by SharpDevelop.
 * User: elijah
 * Date: 12/15/2011
 * Time: 12:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using IExtendFramework.Controls.AdvancedMessageBox;
using IExtendFramework.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace NotepadX
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        public static MainForm Instance;
        public static readonly Plugins.PluginService PluginManager = new NotepadX.Plugins.PluginService();
        
        public MainForm()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            Instance = this;
            
            // load plugins
            PluginManager.FindPlugins();
            
            // set up default extensions - TODO: move to plugin
            IExtendFramework.Text.FileExtensionManager.AddEditor(new DefaultExtensions.TXTEditor());
            IExtendFramework.Text.FileExtensionManager.AddEditor(new DefaultExtensions.NXMEditor());
            IExtendFramework.Text.FileExtensionManager.AddEditor(new DefaultExtensions.RTFEditor());
        }
        
        public void AddForm(DockContent dc, DockState ds)
        {
            if (dc == null)
                return;
            //FIXME: set 'dc.ShowHint = ds' without failing from no active content
            dc.ShowHint = DockState.Document;
            dc.MdiParent = this;
            dc.Show(dockPanel1);
        }
        
        public IDockContent CurrentDocument
        {
            get
            {
                return dockPanel1.ActiveDocument;
            }
        }
        
        public delegate void ProcessParametersDelegate(object sender, string[] args);
        
        public void ProcessParameters(object sender, string[] args)
        {
            if (args.Length > 1)
            {
                ITextEditor e = null;
                try {
                    e = IExtendFramework.Text.FileExtensionManager.OpenDocument(args[1]);
                } catch (IExtendFramework.Text.InvalidFileTypeException) {
                    if (MessageBox.Show("No editor registered for file extension '" + System.IO.Path.GetExtension(args[1]) + "'. Open as a text file?", "Notepad X", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.Yes)
                    {
                        DefaultExtensions.TXTEditor ed = new NotepadX.DefaultExtensions.TXTEditor();
                        ed.Open(args[1]);
                        AddForm(ed as DockContent, DockState.Document);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot open file '" + args[1] + "'!\n" + ex.ToString());
                }
                if (e != null)
                    AddForm(e as DockContent, DockState.Document);
            }
            this.BringToFront();
        }
        
        void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.NewFileForm nff = new Forms.NewFileForm();
            if (nff.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(nff.Filename))
                { // create file
                    sw.Close();
                }
                nff.Result.Open(nff.Filename);
                AddForm(nff.Result as DockContent, DockState.Document);
            }
        }
        
        void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = FileFilterSettings.ToFilter();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ITextEditor e2 = null;
                try {
                    e2 = IExtendFramework.Text.FileExtensionManager.OpenDocument(ofd.FileName);
                } catch (IExtendFramework.Text.InvalidFileTypeException) {
                    if (MessageBox.Show("No editor registered for file extension '" + System.IO.Path.GetExtension(ofd.FileName) + "'. Open as a text file?", "Notepad X", MessageBoxButtons.YesNo, MessageBoxIcon.None) == DialogResult.Yes)
                    {
                        DefaultExtensions.TXTEditor ed = new NotepadX.DefaultExtensions.TXTEditor();
                        ed.Open(ofd.FileName);
                        AddForm(ed as DockContent, DockState.Document);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot open file '" + ofd.FileName + "!\n" + ex.ToString());
                }
                if (e2 != null)
                {
                    FileFilterSettings.AddAHit(e2.Extension);
                    AddForm(e2 as WeifenLuo.WinFormsUI.Docking.DockContent, DockState.Document);
                }
            }
        }
        
        void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.Save();
            } catch (Exception) {
            }
        }
        
        void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.SaveAs();
            } catch (Exception) {
            }
        }
        
        void PrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.Print();
            } catch (Exception) {
            }
        }
        
        void PrintPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.PrintPreview();
            } catch (Exception) {
            }
        }
        
        void PrintSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.PrintSetup();
            } catch (Exception) {
            }
        }
        
        void CloseCurrentWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.Dispose();
                i = null;
            } catch (Exception) {
            }
        }
        
        void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.Undo();
            } catch (Exception) {
            }
        }
        
        void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.Redo();
            } catch (Exception) {
            }
        }
        
        void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.Cut();
            } catch (Exception) {
            }
        }
        
        void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.Copy();
            } catch (Exception) {
            }
        }
        
        void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.Paste();
            } catch (Exception) {
            }
        }
        
        void InsertDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.Insert(i.DocumentText.Length, DateTime.Now.ToString());
            } catch (Exception) {
            }
        }
        
        void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.SelectAll();
            } catch (Exception) {
            }
        }
        
        void FontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                try {
                    ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                    i.ChangeFont(fd.Font);
                } catch (Exception) {
                }
            }
        }
        
        void ColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                try {
                    ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                    i.ChangeColor(cd.Color);
                } catch (Exception) {
                }
            }
        }
        
        void Timer1_Tick(object sender, EventArgs e)
        {
            try {
                if (dockPanel1.ActiveContent == null)
                    sepMenuItem.Visible = false;
                else
                    sepMenuItem.Visible = true;
                
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                this.Text = Path.GetFileName(i.Filename) + " - Notepad X v" + Application.ProductVersion;
                this.undobufferStatusLabel.Text = "Undo Buffer: " + i.UndoBuffer;
            } catch (Exception) {
                this.Text = "Notepad X v" + Application.ProductVersion;
            }
        }
        
        void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                dockPanel1.ActiveDocument.DockHandler.Close();
            } catch (Exception ex) {
                MessageBox.Show("Error closing document: " + ex.Message);
            }
        }
        
        void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                foreach (IDockContent dp in dockPanel1.Documents)
                    dp.DockHandler.Close();
            } catch (Exception ex) {
                MessageBox.Show("Error closing document: " + ex.Message);
            }
        }
        
        void SaveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try {
                ITextEditor i = dockPanel1.ActiveDocument as ITextEditor;
                i.Save();
            } catch (Exception) {
            }
        }
        
        void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                foreach (IDockContent dp in dockPanel1.Documents)
                    (dp as ITextEditor).Save();
            } catch (Exception ex) {
                MessageBox.Show("Error closing document: " + ex.Message);
            }
        }
        
        void RunDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try {
                Macros.Parser p = new NotepadX.Macros.Parser((CurrentDocument as ITextEditor).DocumentText);
                Macros.Environment env = NotepadX.Macros.Environment.LastEnvironment==null ? new Macros.Environment() : Macros.Environment.LastEnvironment;
                MessageBox.Show(env.Run(p.Parsed.ToArray()).ToString());
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex.ToString());
            }
        }
        
        public void AddMenuItem(ToolStripMenuItem item, string path, int index)
        {
            PluginManager.GetMenuItemFromString(path, index, item);
        }
        
        public object RunMacro(string macro)
        {
            Macros.Parser p = new NotepadX.Macros.Parser(macro);
            Macros.Environment env = NotepadX.Macros.Environment.LastEnvironment==null ? new Macros.Environment() : Macros.Environment.LastEnvironment;
            return env.Run(p.Parsed.ToArray());
        }
        
        void MainForm_Load(object sender, EventArgs e)
        {
            ProcessParameters(null, Environment.GetCommandLineArgs());
        }
        
        void TextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefaultExtensions.TXTEditor e2 = new NotepadX.DefaultExtensions.TXTEditor().Create("") as DefaultExtensions.TXTEditor;
            AddForm(e2, DockState.Document);
        }
        
        void PluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Plugins.PluginsForm().ShowDialog();
        }
        
        void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Forms.OptionsForm().ShowDialog();
        }
        
        void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Forms.AboutForm().ShowDialog();
        }
        
        void HelpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new Forms.HelpForm().ShowDialog();
        }
        
        void UserGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskDialogOptions o = new TaskDialogOptions();
            o.Content = "Sorry, this is unavailable";
            o.MainInstruction = "Unavailable!";
            o.Title = "Unavailable";
            o.ExpandedInfo = "This is coming soon";
            TaskDialog.Show(o);
        }
        
        void MacroGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskDialogOptions o = new TaskDialogOptions();
            o.Content = "Sorry, this is unavailable";
            o.MainInstruction = "Unavailable!";
            o.Title = "Unavailable";
            o.ExpandedInfo = "This is coming soon";
            TaskDialog.Show(o);
        }
        
        void PluginGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskDialogOptions o = new TaskDialogOptions();
            o.Content = "Sorry, this is unavailable";
            o.MainInstruction = "Unavailable!";
            o.Title = "Unavailable";
            o.ExpandedInfo = "This is coming soon";
            TaskDialog.Show(o);
        }
    }
}
