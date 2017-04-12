using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;

namespace AspCodeAnalyzer {

  public class AnalyzerGui : System.Windows.Forms.Form {
    public System.Windows.Forms.ListBox lbResult;
    public System.Windows.Forms.Button btnGo;
    private System.Windows.Forms.TextBox txtPath;
    private System.Windows.Forms.Button btnDirectory;
    private System.Windows.Forms.Label label1;
    public System.Windows.Forms.Label lblAnalyzedFiles;
    public System.Windows.Forms.Label lblElapsedTime;
    private System.Windows.Forms.Label label3;
    public System.Windows.Forms.Label lblFindings;
    private System.Windows.Forms.Label label4;
    private Thread _thread = null;
    private System.Windows.Forms.Label label2;
    public System.Windows.Forms.Label lblCurrentAction;
    public System.Windows.Forms.ListBox lbError;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.MainMenu mainMenu1;
    private System.Windows.Forms.MenuItem menuItem1;
    private System.Windows.Forms.MenuItem menuItem3;
    private Preferences _preferences = new Preferences();
    private System.Windows.Forms.MenuItem menOptions;
    private System.Windows.Forms.MenuItem menExit;


    private System.ComponentModel.Container components = null;

    public AnalyzerGui() {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      InitGui();
    }


    private void InitGui() {
      _preferences.LoadPreferences();
      txtPath.Text = _preferences.LastUsedPath;
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing ) {
      if( disposing ) {
        if (components != null) {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AnalyzerGui));
      this.lbResult = new System.Windows.Forms.ListBox();
      this.btnGo = new System.Windows.Forms.Button();
      this.btnDirectory = new System.Windows.Forms.Button();
      this.txtPath = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.lblAnalyzedFiles = new System.Windows.Forms.Label();
      this.lblElapsedTime = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.lblFindings = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.lblCurrentAction = new System.Windows.Forms.Label();
      this.lbError = new System.Windows.Forms.ListBox();
      this.label5 = new System.Windows.Forms.Label();
      this.mainMenu1 = new System.Windows.Forms.MainMenu();
      this.menuItem1 = new System.Windows.Forms.MenuItem();
      this.menOptions = new System.Windows.Forms.MenuItem();
      this.menuItem3 = new System.Windows.Forms.MenuItem();
      this.menExit = new System.Windows.Forms.MenuItem();
      this.SuspendLayout();
      // 
      // lbResult
      // 
      this.lbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.lbResult.Location = new System.Drawing.Point(8, 48);
      this.lbResult.Name = "lbResult";
      this.lbResult.Size = new System.Drawing.Size(608, 290);
      this.lbResult.TabIndex = 8;
      this.lbResult.DoubleClick += new System.EventHandler(this.lbResult_DoubleClick);
      // 
      // btnGo
      // 
      this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnGo.Location = new System.Drawing.Point(536, 8);
      this.btnGo.Name = "btnGo";
      this.btnGo.TabIndex = 7;
      this.btnGo.Text = "Go";
      this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
      // 
      // btnDirectory
      // 
      this.btnDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnDirectory.Location = new System.Drawing.Point(448, 8);
      this.btnDirectory.Name = "btnDirectory";
      this.btnDirectory.TabIndex = 6;
      this.btnDirectory.Text = "Browse";
      this.btnDirectory.Click += new System.EventHandler(this.btnDirectory_Click);
      // 
      // txtPath
      // 
      this.txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.txtPath.Location = new System.Drawing.Point(8, 8);
      this.txtPath.Name = "txtPath";
      this.txtPath.Size = new System.Drawing.Size(424, 20);
      this.txtPath.TabIndex = 5;
      this.txtPath.Text = "C:\\";
      this.txtPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPath_KeyPress);
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label1.Location = new System.Drawing.Point(8, 392);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(100, 16);
      this.label1.TabIndex = 9;
      this.label1.Text = "Analyzed files:";
      // 
      // lblAnalyzedFiles
      // 
      this.lblAnalyzedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblAnalyzedFiles.Location = new System.Drawing.Point(112, 392);
      this.lblAnalyzedFiles.Name = "lblAnalyzedFiles";
      this.lblAnalyzedFiles.Size = new System.Drawing.Size(72, 16);
      this.lblAnalyzedFiles.TabIndex = 10;
      // 
      // lblElapsedTime
      // 
      this.lblElapsedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblElapsedTime.Location = new System.Drawing.Point(512, 392);
      this.lblElapsedTime.Name = "lblElapsedTime";
      this.lblElapsedTime.Size = new System.Drawing.Size(100, 16);
      this.lblElapsedTime.TabIndex = 10;
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label3.Location = new System.Drawing.Point(408, 392);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(100, 16);
      this.label3.TabIndex = 9;
      this.label3.Text = "Elapsed Time:";
      // 
      // lblFindings
      // 
      this.lblFindings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblFindings.Location = new System.Drawing.Point(312, 392);
      this.lblFindings.Name = "lblFindings";
      this.lblFindings.Size = new System.Drawing.Size(40, 16);
      this.lblFindings.TabIndex = 10;
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label4.Location = new System.Drawing.Point(208, 392);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(100, 16);
      this.label4.TabIndex = 9;
      this.label4.Text = "Findings:";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label2.Location = new System.Drawing.Point(8, 344);
      this.label2.Name = "label2";
      this.label2.TabIndex = 11;
      this.label2.Text = "Current Action:";
      // 
      // lblCurrentAction
      // 
      this.lblCurrentAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.lblCurrentAction.Location = new System.Drawing.Point(120, 344);
      this.lblCurrentAction.Name = "lblCurrentAction";
      this.lblCurrentAction.Size = new System.Drawing.Size(488, 40);
      this.lblCurrentAction.TabIndex = 12;
      // 
      // lbError
      // 
      this.lbError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
      this.lbError.ForeColor = System.Drawing.Color.Red;
      this.lbError.Location = new System.Drawing.Point(8, 440);
      this.lbError.Name = "lbError";
      this.lbError.Size = new System.Drawing.Size(608, 69);
      this.lbError.TabIndex = 13;
      // 
      // label5
      // 
      this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label5.Location = new System.Drawing.Point(8, 416);
      this.label5.Name = "label5";
      this.label5.TabIndex = 14;
      this.label5.Text = "Errors:";
      // 
      // mainMenu1
      // 
      this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                              this.menuItem1});
      // 
      // menuItem1
      // 
      this.menuItem1.Index = 0;
      this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                              this.menOptions,
                                                                              this.menuItem3,
                                                                              this.menExit});
      this.menuItem1.Text = "&File";
      // 
      // menOptions
      // 
      this.menOptions.Index = 0;
      this.menOptions.Text = "&Options";
      this.menOptions.Click += new System.EventHandler(this.menOptions_Click);
      // 
      // menuItem3
      // 
      this.menuItem3.Index = 1;
      this.menuItem3.Text = "-";
      // 
      // menExit
      // 
      this.menExit.Index = 2;
      this.menExit.Text = "E&xit";
      this.menExit.Click += new System.EventHandler(this.menExit_Click);
      // 
      // AnalyzerGui
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(624, 517);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.lbError);
      this.Controls.Add(this.lblCurrentAction);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.lblAnalyzedFiles);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.lbResult);
      this.Controls.Add(this.btnGo);
      this.Controls.Add(this.btnDirectory);
      this.Controls.Add(this.txtPath);
      this.Controls.Add(this.lblElapsedTime);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.lblFindings);
      this.Controls.Add(this.label4);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Menu = this.mainMenu1;
      this.Name = "AnalyzerGui";
      this.Text = "ASP Code Analyzer";
      this.ResumeLayout(false);

    }
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
      Application.Run(new AnalyzerGui());
    }

    private void btnDirectory_Click(object sender, System.EventArgs e) {
      FolderBrowserDialog dialog = new FolderBrowserDialog();
      if ( txtPath.Text != "" ) {
        dialog.SelectedPath = txtPath.Text;
      }
      DialogResult res = dialog.ShowDialog();
      if ( res != DialogResult.OK ) {
        return;
      }
      txtPath.Text = dialog.SelectedPath;
    }

    private void btnGo_Click(object sender, System.EventArgs e) {

      if ( _thread != null && _thread.IsAlive ) {
        _thread.Abort();
        _thread = null;
        btnGo.Text = "Go";
        return;
      }
      btnGo.Text = "Stop";

      lbResult.Items.Clear();
      lbError.Items.Clear();
      lblAnalyzedFiles.Text = "";
      lblFindings.Text = "";
      lblElapsedTime.Text = "";

       _preferences.LastUsedPath = txtPath.Text;
      _preferences.SavePreferences();


      _thread = new Thread( new ThreadStart( new Analyzer( this, txtPath.Text).Analyze));
      _thread.Start();
    }

    private void lbResult_DoubleClick(object sender, System.EventArgs e) {
      if ( lbResult.SelectedItem == null ) {
        return;
      }
      try {
        Result curResult = (Result) lbResult.SelectedItem;
        System.Diagnostics.Process.Start( _preferences.EditorPath, String.Format( _preferences.FilePattern, curResult.Filename, curResult.Row));
      } catch ( Exception ex) {
        MessageBox.Show( ex.Message);
      }
    
    }

    private void txtPath_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
      if(e.KeyChar == (char)13) { 
        btnGo_Click(null,null);
        e.Handled = true; 
      }

    
    }

    private void menExit_Click(object sender, System.EventArgs e) {
      Environment.Exit( 0);    
    }

    private void menOptions_Click(object sender, System.EventArgs e) {
      OptionsDlg dlg = new OptionsDlg( _preferences);
      dlg.ShowDialog();
      dlg.Dispose();
    }


  }
}
