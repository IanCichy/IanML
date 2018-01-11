namespace IC_ML_MazeSolver
{
    partial class frmMazeSolver
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
            this.btnStart = new System.Windows.Forms.Button();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rbtnGUI = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.rbtnSarsaElg = new System.Windows.Forms.RadioButton();
            this.rbtnSarsa = new System.Windows.Forms.RadioButton();
            this.rbtnQLearning = new System.Windows.Forms.RadioButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnFile = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.rbtnFast = new System.Windows.Forms.RadioButton();
            this.rbtnNormal = new System.Windows.Forms.RadioButton();
            this.rbtnSlow = new System.Windows.Forms.RadioButton();
            this.tlpMaze = new System.Windows.Forms.TableLayoutPanel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnStart.Location = new System.Drawing.Point(1256, 398);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(149, 50);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtHeight
            // 
            this.txtHeight.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtHeight.Location = new System.Drawing.Point(1256, 122);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(77, 20);
            this.txtHeight.TabIndex = 6;
            // 
            // txtWidth
            // 
            this.txtWidth.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtWidth.Location = new System.Drawing.Point(1256, 96);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(77, 20);
            this.txtWidth.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1344, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Height";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1344, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Width";
            // 
            // rbtnGUI
            // 
            this.rbtnGUI.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.rbtnGUI.AutoSize = true;
            this.rbtnGUI.Location = new System.Drawing.Point(1256, 148);
            this.rbtnGUI.Name = "rbtnGUI";
            this.rbtnGUI.Size = new System.Drawing.Size(85, 17);
            this.rbtnGUI.TabIndex = 10;
            this.rbtnGUI.TabStop = true;
            this.rbtnGUI.Text = "Animate GUI";
            this.rbtnGUI.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.rbtnSarsaElg);
            this.panel1.Controls.Add(this.rbtnSarsa);
            this.panel1.Controls.Add(this.rbtnQLearning);
            this.panel1.Location = new System.Drawing.Point(1241, 180);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(178, 103);
            this.panel1.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Algorithm";
            // 
            // rbtnSarsaElg
            // 
            this.rbtnSarsaElg.AutoSize = true;
            this.rbtnSarsaElg.Location = new System.Drawing.Point(15, 67);
            this.rbtnSarsaElg.Name = "rbtnSarsaElg";
            this.rbtnSarsaElg.Size = new System.Drawing.Size(155, 17);
            this.rbtnSarsaElg.TabIndex = 2;
            this.rbtnSarsaElg.TabStop = true;
            this.rbtnSarsaElg.Text = "SARSA w/ Eligibility Traces";
            this.rbtnSarsaElg.UseVisualStyleBackColor = true;
            // 
            // rbtnSarsa
            // 
            this.rbtnSarsa.AutoSize = true;
            this.rbtnSarsa.Location = new System.Drawing.Point(15, 44);
            this.rbtnSarsa.Name = "rbtnSarsa";
            this.rbtnSarsa.Size = new System.Drawing.Size(61, 17);
            this.rbtnSarsa.TabIndex = 1;
            this.rbtnSarsa.TabStop = true;
            this.rbtnSarsa.Text = "SARSA";
            this.rbtnSarsa.UseVisualStyleBackColor = true;
            // 
            // rbtnQLearning
            // 
            this.rbtnQLearning.AutoSize = true;
            this.rbtnQLearning.Location = new System.Drawing.Point(15, 21);
            this.rbtnQLearning.Name = "rbtnQLearning";
            this.rbtnQLearning.Size = new System.Drawing.Size(77, 17);
            this.rbtnQLearning.TabIndex = 0;
            this.rbtnQLearning.TabStop = true;
            this.rbtnQLearning.Text = "Q-Learning";
            this.rbtnQLearning.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnFile
            // 
            this.btnFile.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnFile.Location = new System.Drawing.Point(1282, 45);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(123, 26);
            this.btnFile.TabIndex = 12;
            this.btnFile.Text = "Select File";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // txtFile
            // 
            this.txtFile.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtFile.Location = new System.Drawing.Point(1247, 19);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(163, 20);
            this.txtFile.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.rbtnFast);
            this.panel2.Controls.Add(this.rbtnNormal);
            this.panel2.Controls.Add(this.rbtnSlow);
            this.panel2.Location = new System.Drawing.Point(1242, 289);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(178, 103);
            this.panel2.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Reduction Type";
            // 
            // rbtnFast
            // 
            this.rbtnFast.AutoSize = true;
            this.rbtnFast.Location = new System.Drawing.Point(15, 67);
            this.rbtnFast.Name = "rbtnFast";
            this.rbtnFast.Size = new System.Drawing.Size(45, 17);
            this.rbtnFast.TabIndex = 2;
            this.rbtnFast.TabStop = true;
            this.rbtnFast.Text = "Fast";
            this.rbtnFast.UseVisualStyleBackColor = true;
            // 
            // rbtnNormal
            // 
            this.rbtnNormal.AutoSize = true;
            this.rbtnNormal.Location = new System.Drawing.Point(15, 44);
            this.rbtnNormal.Name = "rbtnNormal";
            this.rbtnNormal.Size = new System.Drawing.Size(58, 17);
            this.rbtnNormal.TabIndex = 1;
            this.rbtnNormal.TabStop = true;
            this.rbtnNormal.Text = "Normal";
            this.rbtnNormal.UseVisualStyleBackColor = true;
            // 
            // rbtnSlow
            // 
            this.rbtnSlow.AutoSize = true;
            this.rbtnSlow.Location = new System.Drawing.Point(15, 21);
            this.rbtnSlow.Name = "rbtnSlow";
            this.rbtnSlow.Size = new System.Drawing.Size(48, 17);
            this.rbtnSlow.TabIndex = 0;
            this.rbtnSlow.TabStop = true;
            this.rbtnSlow.Text = "Slow";
            this.rbtnSlow.UseVisualStyleBackColor = true;
            // 
            // tlpMaze
            // 
            this.tlpMaze.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMaze.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMaze.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.tlpMaze.ColumnCount = 2;
            this.tlpMaze.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMaze.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMaze.Location = new System.Drawing.Point(0, 0);
            this.tlpMaze.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMaze.Name = "tlpMaze";
            this.tlpMaze.RowCount = 2;
            this.tlpMaze.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMaze.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMaze.Size = new System.Drawing.Size(1234, 880);
            this.tlpMaze.TabIndex = 15;
            // 
            // frmMazeSolver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1432, 882);
            this.Controls.Add(this.tlpMaze);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rbtnGUI);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.btnStart);
            this.Name = "frmMazeSolver";
            this.Text = "MazeSolver";
            this.ResizeEnd += new System.EventHandler(this.frmMazeSolver_ResizeEnd);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbtnGUI;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbtnSarsaElg;
        private System.Windows.Forms.RadioButton rbtnSarsa;
        private System.Windows.Forms.RadioButton rbtnQLearning;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbtnFast;
        private System.Windows.Forms.RadioButton rbtnNormal;
        private System.Windows.Forms.RadioButton rbtnSlow;
        private System.Windows.Forms.TableLayoutPanel tlpMaze;
    }
}

