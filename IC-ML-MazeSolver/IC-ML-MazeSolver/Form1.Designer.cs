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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.rbtnSarsa = new System.Windows.Forms.RadioButton();
            this.rbtnQLearning = new System.Windows.Forms.RadioButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnFile = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.chkAnimateGUI = new System.Windows.Forms.CheckBox();
            this.txtReductionConstant = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtEpisodesToRun = new System.Windows.Forms.TextBox();
            this.tlpMaze = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(5, 310);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(180, 40);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.rbtnSarsa);
            this.panel1.Controls.Add(this.rbtnQLearning);
            this.panel1.Location = new System.Drawing.Point(5, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(180, 100);
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
            this.btnFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFile.Location = new System.Drawing.Point(65, 30);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(120, 25);
            this.btnFile.TabIndex = 12;
            this.btnFile.Text = "Select File";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(5, 5);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(180, 20);
            this.txtFile.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.chkAnimateGUI);
            this.panel2.Controls.Add(this.txtReductionConstant);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtEpisodesToRun);
            this.panel2.Location = new System.Drawing.Point(5, 165);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(180, 108);
            this.panel2.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(80, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Every X Episodes";
            // 
            // chkAnimateGUI
            // 
            this.chkAnimateGUI.AutoSize = true;
            this.chkAnimateGUI.Location = new System.Drawing.Point(42, 79);
            this.chkAnimateGUI.Name = "chkAnimateGUI";
            this.chkAnimateGUI.Size = new System.Drawing.Size(92, 17);
            this.chkAnimateGUI.TabIndex = 21;
            this.chkAnimateGUI.Text = "Animated GUI";
            this.chkAnimateGUI.UseVisualStyleBackColor = true;
            // 
            // txtReductionConstant
            // 
            this.txtReductionConstant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReductionConstant.Location = new System.Drawing.Point(3, 28);
            this.txtReductionConstant.Name = "txtReductionConstant";
            this.txtReductionConstant.Size = new System.Drawing.Size(71, 20);
            this.txtReductionConstant.TabIndex = 22;
            this.txtReductionConstant.Text = "10";
            this.txtReductionConstant.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Parameters";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(80, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Max Episodes";
            // 
            // txtEpisodesToRun
            // 
            this.txtEpisodesToRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEpisodesToRun.Location = new System.Drawing.Point(3, 53);
            this.txtEpisodesToRun.Name = "txtEpisodesToRun";
            this.txtEpisodesToRun.Size = new System.Drawing.Size(71, 20);
            this.txtEpisodesToRun.TabIndex = 19;
            this.txtEpisodesToRun.Text = "2000";
            this.txtEpisodesToRun.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            this.tlpMaze.Size = new System.Drawing.Size(1036, 885);
            this.tlpMaze.TabIndex = 15;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.btnReset);
            this.panel3.Controls.Add(this.txtFile);
            this.panel3.Controls.Add(this.btnStart);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.btnFile);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Location = new System.Drawing.Point(1040, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(190, 885);
            this.panel3.TabIndex = 22;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(5, 355);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(180, 40);
            this.btnReset.TabIndex = 19;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // frmMazeSolver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 886);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.tlpMaze);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "frmMazeSolver";
            this.Text = "MazeSolver";
            this.ResizeEnd += new System.EventHandler(this.frmMazeSolver_ResizeEnd);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbtnSarsa;
        private System.Windows.Forms.RadioButton rbtnQLearning;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tlpMaze;
        private System.Windows.Forms.TextBox txtEpisodesToRun;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkAnimateGUI;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtReductionConstant;
        private System.Windows.Forms.Button btnReset;
    }
}

