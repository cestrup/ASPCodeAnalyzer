using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace AspCodeAnalyzer {
  /// <summary>
  /// Summary description for OptionsDlg.
  /// </summary>
  public class OptionsDlg : System.Windows.Forms.Form {
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private Preferences _preferences;
    private System.Windows.Forms.TextBox txtEditorPath;
    private System.Windows.Forms.TextBox txtFilePattern;
    private System.Windows.Forms.Label label3;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public OptionsDlg( Preferences pPreferences) {
      _preferences = pPreferences;
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      txtEditorPath.Text = _preferences.EditorPath;
      txtFilePattern.Text = _preferences.FilePattern;
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing ) {
      if( disposing ) {
        if(components != null) {
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
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OptionsDlg));
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.txtFilePattern = new System.Windows.Forms.TextBox();
      this.txtEditorPath = new System.Windows.Forms.TextBox();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(16, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(48, 23);
      this.label1.TabIndex = 0;
      this.label1.Text = "Editor:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(16, 56);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(440, 23);
      this.label2.TabIndex = 1;
      this.label2.Text = "Editor Arguments: {0} = Filename, {1} = Line Number";
      // 
      // txtFilePattern
      // 
      this.txtFilePattern.Location = new System.Drawing.Point(16, 80);
      this.txtFilePattern.Name = "txtFilePattern";
      this.txtFilePattern.Size = new System.Drawing.Size(440, 20);
      this.txtFilePattern.TabIndex = 2;
      this.txtFilePattern.Text = "";
      // 
      // txtEditorPath
      // 
      this.txtEditorPath.Location = new System.Drawing.Point(72, 16);
      this.txtEditorPath.Name = "txtEditorPath";
      this.txtEditorPath.Size = new System.Drawing.Size(384, 20);
      this.txtEditorPath.TabIndex = 1;
      this.txtEditorPath.Text = "";
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(296, 120);
      this.btnOK.Name = "btnOK";
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(384, 120);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      // 
      // label3
      // 
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.label3.Location = new System.Drawing.Point(24, 120);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(152, 23);
      this.label3.TabIndex = 5;
      this.label3.Text = "schudel@evolootion.net";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // OptionsDlg
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(472, 157);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.txtEditorPath);
      this.Controls.Add(this.txtFilePattern);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.btnCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "OptionsDlg";
      this.Text = "ASP Code Analyzer: Options";
      this.ResumeLayout(false);

    }
    #endregion

    private void btnOK_Click(object sender, System.EventArgs e) {
      _preferences.EditorPath = txtEditorPath.Text;
      _preferences.FilePattern = txtFilePattern.Text;
      _preferences.SavePreferences();
      Close();
    
    }
  }
}
