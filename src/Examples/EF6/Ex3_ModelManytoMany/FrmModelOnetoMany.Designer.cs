﻿
namespace Ex3_ModelManytoMany
{
    partial class FrmModelOnetoMany
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnection = new System.Windows.Forms.TextBox();
            this.btnTestOne2One = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(211, 135);
            this.txtDebug.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(710, 266);
            this.txtDebug.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(207, 49);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Sql Server Connection";
            // 
            // txtConnection
            // 
            this.txtConnection.Location = new System.Drawing.Point(211, 74);
            this.txtConnection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConnection.Name = "txtConnection";
            this.txtConnection.Size = new System.Drawing.Size(710, 26);
            this.txtConnection.TabIndex = 10;
            this.txtConnection.Text = "...";
            // 
            // btnTestOne2One
            // 
            this.btnTestOne2One.Location = new System.Drawing.Point(33, 135);
            this.btnTestOne2One.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTestOne2One.Name = "btnTestOne2One";
            this.btnTestOne2One.Size = new System.Drawing.Size(170, 63);
            this.btnTestOne2One.TabIndex = 9;
            this.btnTestOne2One.Text = "Test One to Many";
            this.btnTestOne2One.UseVisualStyleBackColor = true;
            this.btnTestOne2One.Click += new System.EventHandler(this.btnMany2Many_Click);
            // 
            // FrmModelOnetoMany
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 450);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtConnection);
            this.Controls.Add(this.btnTestOne2One);
            this.Name = "FrmModelOnetoMany";
            this.Text = "Ex 3: One 2 Many - Authors & Books";
            this.Load += new System.EventHandler(this.FrmMany2Many_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      #endregion

      private System.Windows.Forms.TextBox txtDebug;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtConnection;
      private System.Windows.Forms.Button btnTestOne2One;
   }
}