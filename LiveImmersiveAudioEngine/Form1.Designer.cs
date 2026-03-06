namespace LiveImmersiveAudioEngine
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblLeftRightPan = new Label();
            btnAddInput = new Button();
            txtSelectedChannel = new TextBox();
            SuspendLayout();
            // 
            // lblLeftRightPan
            // 
            lblLeftRightPan.AutoSize = true;
            lblLeftRightPan.Location = new Point(18, 423);
            lblLeftRightPan.Name = "lblLeftRightPan";
            lblLeftRightPan.Size = new Size(0, 15);
            lblLeftRightPan.TabIndex = 2;
            // 
            // btnAddInput
            // 
            btnAddInput.Location = new Point(12, 12);
            btnAddInput.Name = "btnAddInput";
            btnAddInput.Size = new Size(75, 23);
            btnAddInput.TabIndex = 4;
            btnAddInput.Text = "Add Input";
            btnAddInput.UseVisualStyleBackColor = true;
            btnAddInput.Click += btnAddInput_Click;
            // 
            // txtSelectedChannel
            // 
            txtSelectedChannel.Location = new Point(698, 12);
            txtSelectedChannel.Name = "txtSelectedChannel";
            txtSelectedChannel.Size = new Size(90, 23);
            txtSelectedChannel.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtSelectedChannel);
            Controls.Add(btnAddInput);
            Controls.Add(lblLeftRightPan);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblLeftRightPan;
        private Button btnAddInput;
        private TextBox txtSelectedChannel;
    }
}
