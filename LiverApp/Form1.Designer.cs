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
            this.ClientSize = new System.Drawing.Size(800, 650);
            this.Text = "Karaciğer Hastalığı Tespiti (AI Destekli)";
            
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.ColumnCount = 2;
            this.mainLayout.RowCount = 12;
            this.mainLayout.Padding = new System.Windows.Forms.Padding(20);
            this.mainLayout.AutoSize = true;

            // Define column styles
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));

            string[] labels = { 
                "Yaş (4-90):", "Cinsiyet:", "Toplam Bilirubin (0.4-75.0):", "Direkt Bilirubin (0.1-19.7):", 
                "Alkalin Fosfataz (63-2110):", "Alanin Aminotransferaz (10-2000):", "Aspartat Aminotransferaz (10-4929):", 
                "Toplam Proteinler (2.7-9.6):", "Albümin (0.9-5.5):", "Albümin/Globulin Oranı (0.3-2.8):" 
            };
            
            // Generate Controls
            this.inputs = new System.Windows.Forms.Control[10];
            
            for (int i = 0; i < 10; i++)
            {
                System.Windows.Forms.Label lbl = new System.Windows.Forms.Label();
                lbl.Text = labels[i];
                lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                lbl.Dock = System.Windows.Forms.DockStyle.Fill;
                
                System.Windows.Forms.Control input;
                if (i == 1) // Cinsiyet
                {
                    System.Windows.Forms.ComboBox cmb = new System.Windows.Forms.ComboBox();
                    cmb.Items.AddRange(new object[] { "Erkek", "Kadın" });
                    cmb.SelectedIndex = 0;
                    cmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                    input = cmb;
                }
                else
                {
                    System.Windows.Forms.TextBox txt = new System.Windows.Forms.TextBox();
                    txt.Text = ""; // Default empty
                    input = txt;
                }
                input.Dock = System.Windows.Forms.DockStyle.Fill;
                
                this.mainLayout.Controls.Add(lbl, 0, i);
                this.mainLayout.Controls.Add(input, 1, i);
                this.inputs[i] = input;
            }

            // Button
            this.btnPredict = new System.Windows.Forms.Button();
            this.btnPredict.Text = "Analiz Et";
            this.btnPredict.Size = new System.Drawing.Size(100, 40);
            this.btnPredict.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPredict.Click += new System.EventHandler(this.btnPredict_Click);
            
            // Result Label
            this.lblResult = new System.Windows.Forms.Label();
            this.lblResult.Text = "Sonuç bekleniyor...";
            this.lblResult.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResult.AutoSize = true;

            this.mainLayout.Controls.Add(this.btnPredict, 1, 10);
            this.mainLayout.Controls.Add(this.lblResult, 0, 11);
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
