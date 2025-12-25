namespace LiverApp
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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 750);
            this.Text = "Karaciğer Hastalığı Tespiti (AI Destekli)";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245); // Light Gray/Blue background
            this.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.ColumnCount = 2;
            this.mainLayout.RowCount = 13; // Added header row
            this.mainLayout.Padding = new System.Windows.Forms.Padding(40);
            this.mainLayout.AutoSize = true;

            // Define column styles
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F)); // Labels
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F)); // Inputs

            // Header
            System.Windows.Forms.Label lblHeader = new System.Windows.Forms.Label();
            lblHeader.Text = "Karaciğer Hastalığı Risk Analizi";
            lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblHeader.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41);
            lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            lblHeader.AutoSize = true;
            this.mainLayout.Controls.Add(lblHeader, 0, 0);
            this.mainLayout.SetColumnSpan(lblHeader, 2);

            string[] labels = { 
                "Yaş (4-90):", "Cinsiyet:", "Toplam Bilirubin (0.1 - 1.2):", "Direkt Bilirubin (0.0 - 0.3):", 
                "Alkalin Fosfataz (44 - 147):", "Alanin Aminotransferaz (7 - 55):", "Aspartat Aminotransferaz (8 - 48):", 
                "Toplam Proteinler (6.0 - 8.3):", "Albümin (3.4 - 5.4):", "Albümin/Globulin Oranı (1.1 - 2.5):" 
            };
            
            // Generate Controls
            this.inputs = new System.Windows.Forms.Control[10];
            
            for (int i = 0; i < 10; i++)
            {
                int rowIndex = i + 1; // Start from row 1 (0 is header)

                System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();
                lbl.Text = labels[i];
                lbl.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                lbl.ForeColor = System.Drawing.Color.FromArgb(73, 80, 87);
                lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                lbl.Dock = System.Windows.Forms.DockStyle.Fill;
                lbl.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10); // Vertical spacing
                
                System.Windows.Forms.Control input;
                if (i == 1) // Cinsiyet
                {
                    System.Windows.Forms.ComboBox cmb = new System.Windows.Forms.ComboBox();
                    cmb.Items.AddRange(new object[] { "Erkek", "Kadın" });
                    cmb.SelectedIndex = 0;
                    cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                    cmb.Font = new System.Drawing.Font("Segoe UI", 10F);
                    cmb.BackColor = System.Drawing.Color.White;
                    input = cmb;
                }
                else
                {
                    System.Windows.Forms.TextBox txt = new System.Windows.Forms.TextBox();
                    txt.Text = ""; 
                    txt.Font = new System.Drawing.Font("Segoe UI", 10F);
                    txt.BackColor = System.Drawing.Color.White;
                    txt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    input = txt;
                }
                input.Dock = System.Windows.Forms.DockStyle.Fill;
                input.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
                
                this.mainLayout.Controls.Add(lbl, 0, rowIndex);
                this.mainLayout.Controls.Add(input, 1, rowIndex);
                this.inputs[i] = input;
            }

            // Button
            this.btnPredict = new System.Windows.Forms.Button();
            this.btnPredict.Text = "ANALİZ ET";
            this.btnPredict.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnPredict.Size = new System.Drawing.Size(150, 50);
            this.btnPredict.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPredict.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPredict.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPredict.FlatAppearance.BorderSize = 0;
            this.btnPredict.BackColor = System.Drawing.Color.FromArgb(0, 123, 255); // Blue
            this.btnPredict.ForeColor = System.Drawing.Color.White;
            this.btnPredict.Margin = new System.Windows.Forms.Padding(0, 20, 0, 20);
            this.btnPredict.Click += new System.EventHandler(this.btnPredict_Click);
            
            // Result Label
            this.lblResult = new System.Windows.Forms.Label();
            this.lblResult.Text = "Sonuç Bekleniyor...";
            this.lblResult.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult.AutoSize = true;
            this.lblResult.Padding = new System.Windows.Forms.Padding(10);
            this.lblResult.BackColor = System.Drawing.Color.White;
            this.lblResult.BorderStyle = System.Windows.Forms.BorderStyle.None; // Cleaner look

            this.mainLayout.Controls.Add(this.btnPredict, 1, 11);
            this.mainLayout.Controls.Add(this.lblResult, 0, 12);
            this.mainLayout.SetColumnSpan(this.lblResult, 2);

            this.Controls.Add(this.mainLayout);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Control[] inputs;
        private System.Windows.Forms.Button btnPredict;
        private System.Windows.Forms.Label lblResult;
    }
}
