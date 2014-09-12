namespace Almirante.Editor.Plugins.Behavior
{
    partial class BehaviorEditorControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.hello = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hello
            // 
            this.hello.AutoSize = true;
            this.hello.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hello.Location = new System.Drawing.Point(14, 15);
            this.hello.Name = "hello";
            this.hello.Size = new System.Drawing.Size(204, 37);
            this.hello.TabIndex = 0;
            this.hello.Text = "Hello World!";
            // 
            // BehaviorEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hello);
            this.Name = "BehaviorEditorControl";
            this.Size = new System.Drawing.Size(803, 397);
            this.Load += new System.EventHandler(this.BehaviorEditorControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label hello;
    }
}
