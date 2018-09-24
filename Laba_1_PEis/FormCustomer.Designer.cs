namespace Laba_1_PEis
{
    partial class FormCustomer
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
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.del = new System.Windows.Forms.Button();
            this.Add = new System.Windows.Forms.Button();
            this.redaction = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(842, 34);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(221, 22);
            this.textBoxName.TabIndex = 12;
            this.textBoxName.MaxLength = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(768, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "ФИО";
            // 
            // del
            // 
            this.del.Location = new System.Drawing.Point(797, 151);
            this.del.Name = "del";
            this.del.Size = new System.Drawing.Size(120, 23);
            this.del.TabIndex = 3;
            this.del.Text = "Удалить";
            this.del.UseVisualStyleBackColor = true;
            this.del.Click += new System.EventHandler(this.del_Click);
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(691, 151);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(75, 23);
            this.Add.TabIndex = 2;
            this.Add.Text = "Добавить";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // redaction
            // 
            this.redaction.Location = new System.Drawing.Point(943, 151);
            this.redaction.Name = "redaction";
            this.redaction.Size = new System.Drawing.Size(75, 23);
            this.redaction.TabIndex = 1;
            this.redaction.Text = "Изменить";
            this.redaction.UseVisualStyleBackColor = true;
            this.redaction.Click += new System.EventHandler(this.redaction_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(654, 346);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.AllowUserToAddRows = false; 
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellContentClick);
            // 
            // FormCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1075, 404);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.del);
            this.Controls.Add(this.Add);
            this.Controls.Add(this.redaction);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormCustomer";
            this.Text = "Покупатели";
            this.Load += new System.EventHandler(this.FormCustomer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button del;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Button redaction;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}