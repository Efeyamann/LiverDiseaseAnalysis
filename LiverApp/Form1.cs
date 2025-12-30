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

                if (!double.TryParse(age, out vAge))
                {
                    MessageBox.Show("Geçerli bir yaş giriniz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(totBil, out vTotBil))
                {
                    MessageBox.Show("Toplam Bilirubin geçerli bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(dirBil, out vDirBil))
                {
                    MessageBox.Show("Direkt Bilirubin geçerli bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(alkPhos, out vAlkPhos))
                {
                    MessageBox.Show("Alkalin Fosfataz geçerli bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(alaAmino, out vAlaAmino))
                {
                    MessageBox.Show("Alanin Aminotransferaz geçerli bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(aspAmino, out vAspAmino))
                {
                    MessageBox.Show("Aspartat Aminotransferaz geçerli bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(totProt, out vTotProt))
                {
                    MessageBox.Show("Toplam Proteinler geçerli bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(albumin, out vAlbumin))
                {
                    MessageBox.Show("Albümin geçerli bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(agRatio, out vAgRatio))
                {
                    MessageBox.Show("Albümin/Globulin Oranı geçerli bir sayı olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                
                                // Deserialize as Dictionary
                                var predictionResults = JsonSerializer.Deserialize<Dictionary<string, PredictionResult>>(result, options);

                                string outputText = "Sonuçlar:\n";
                                int highRiskCount = 0;

                                foreach (var kvp in predictionResults)
                                {
                                    string modelName = kvp.Key;
                                    var pred = kvp.Value;
                                    string status = pred.Prediction == 1 ? "RİSK VAR" : "SAĞLIKLI";
                                    double prob = pred.Prediction == 1 ? pred.Probability : (1 - pred.Probability);
                                    
                                    outputText += $"{modelName}: {status} (%{prob * 100:F1})\n";
                                    
                                    if (pred.Prediction == 1) highRiskCount++;
                                }

                                this.lblResult.Text = outputText;
                                
                                // Set color based on result (now only 1 model)
                                if (highRiskCount > 0)
                                {
                                    this.lblResult.ForeColor = Color.Red;
                                }
                                else
                                {
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
