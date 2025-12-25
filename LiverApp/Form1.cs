using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;
using System.Collections.Generic;

namespace LiverApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPredict_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs
                string age = this.inputs[0].Text;
                string gender = ((ComboBox)this.inputs[1]).SelectedItem.ToString() == "Erkek" ? "1" : "0";
                string totBil = this.inputs[2].Text;
                string dirBil = this.inputs[3].Text;
                string alkPhos = this.inputs[4].Text;
                string alaAmino = this.inputs[5].Text;
                string aspAmino = this.inputs[6].Text;
                string totProt = this.inputs[7].Text;
                string albumin = this.inputs[8].Text;
                string agRatio = this.inputs[9].Text;

                // Validation
                double vAge, vTotBil, vDirBil, vAlkPhos, vAlaAmino, vAspAmino, vTotProt, vAlbumin, vAgRatio;

                if (!double.TryParse(age, out vAge) || vAge < 4 || vAge > 90)
                {
                    MessageBox.Show("Geçerli bir yaş giriniz (4-90)", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(totBil, out vTotBil) || vTotBil < 0.4 || vTotBil > 75.0)
                {
                    MessageBox.Show("Toplam Bilirubin 0.4 - 75.0 arasında olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(dirBil, out vDirBil) || vDirBil < 0.1 || vDirBil > 19.7)
                {
                    MessageBox.Show("Direkt Bilirubin 0.1 - 19.7 arasında olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(alkPhos, out vAlkPhos) || vAlkPhos < 63 || vAlkPhos > 2110)
                {
                    MessageBox.Show("Alkalin Fosfataz 63 - 2110 arasında olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(alaAmino, out vAlaAmino) || vAlaAmino < 10 || vAlaAmino > 2000)
                {
                    MessageBox.Show("Alanin Aminotransferaz 10 - 2000 arasında olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(aspAmino, out vAspAmino) || vAspAmino < 10 || vAspAmino > 4929)
                {
                    MessageBox.Show("Aspartat Aminotransferaz 10 - 4929 arasında olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(totProt, out vTotProt) || vTotProt < 2.7 || vTotProt > 9.6)
                {
                    MessageBox.Show("Toplam Proteinler 2.7 - 9.6 arasında olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(albumin, out vAlbumin) || vAlbumin < 0.9 || vAlbumin > 5.5)
                {
                    MessageBox.Show("Albümin 0.9 - 5.5 arasında olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(agRatio, out vAgRatio) || vAgRatio < 0.3 || vAgRatio > 2.8)
                {
                    MessageBox.Show("Albümin/Globulin Oranı 0.3 - 2.8 arasında olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Prepare arguments
                string args = $"{age} {gender} {totBil} {dirBil} {alkPhos} {alaAmino} {aspAmino} {totProt} {albumin} {agRatio}";

                // Call Python script
                string pythonPath = "python"; // Assume python is in PATH
                string scriptPath = @"..\predict.py"; // Relative path to predict.py

                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = pythonPath;
                start.Arguments = $"\"{scriptPath}\" {args}";
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.RedirectStandardError = true;
                start.CreateNoWindow = true;

                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        string stderr = process.StandardError.ReadToEnd();
                        
                        if (!string.IsNullOrEmpty(stderr))
                        {
                            // Ignore some warnings if result is present, otherwise show error
                            if (string.IsNullOrWhiteSpace(result)) 
                            {
                                MessageBox.Show($"Python Hatası: {stderr}", "Hata");
                                return;
                            }
                        }

                        // Parse JSON result
                        try 
                        {
                             // Clean result string (sometimes python prints other stuff)
                            result = result.Trim();
                            int jsonStart = result.IndexOf('{');
                            int jsonEnd = result.LastIndexOf('}');
                            if (jsonStart >= 0 && jsonEnd > jsonStart)
                            {
                                result = result.Substring(jsonStart, jsonEnd - jsonStart + 1);
                                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                                var predictionResult = JsonSerializer.Deserialize<PredictionResult>(result, options);

                                if (predictionResult.Prediction == 1)
                                {
                                    this.lblResult.Text = $"Sonuç: HASTALIK RİSKİ VAR\n(Olasılık: %{predictionResult.Probability * 100:F2})";
                                    this.lblResult.ForeColor = Color.Red;
                                }
                                else
                                {
                                    this.lblResult.Text = $"Sonuç: SAĞLIKLI GÖRÜNÜYOR\n(Olasılık: %{(1 - predictionResult.Probability) * 100:F2})";
                                    this.lblResult.ForeColor = Color.Green;
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Beklenmeyen çıktı: {result}", "Hata");
                            }

                        }
                        catch (Exception ex)
                        {
                             MessageBox.Show($"Sonuç işleme hatası: {ex.Message}\nÇıktı: {result}", "Hata");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata");
            }
        }

        public class PredictionResult
        {
            public int Prediction { get; set; }
            public double Probability { get; set; }
        }
    }
}
