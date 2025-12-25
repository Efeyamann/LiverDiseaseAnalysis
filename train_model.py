import pandas as pd
import numpy as np
from xgboost import XGBClassifier
import joblib
import os

# Load dataset
try:
    df = pd.read_csv('Liver Patient Dataset (LPD)_train.csv', encoding='latin1')
except FileNotFoundError:
    # Fallback if local file not found (though it should be)
    url = "https://raw.githubusercontent.com/Efeyamann/LiverDiseaseAnalysis/main/Liver%20Patient%20Dataset%20(LPD)_train.csv"
    df = pd.read_csv(url, encoding='latin1')

# Rename columns to match notebook
df.columns = [
    'Yaş', 'Cinsiyet', 'Toplam_Bilirubin', 'Direkt_Bilirubin',
    'Alkali_Fosfataz', 'Alanin_Aminotransferaz', 'Aspartat_Aminotransferaz',
    'Toplam_Proteinler', 'Albümin', 'Albumin_Globulin_Orani', 'Sonuç'
]

# Preprocessing
df['Albumin_Globulin_Orani'] = df['Albumin_Globulin_Orani'].fillna(df['Albumin_Globulin_Orani'].median())
df['Cinsiyet'] = df['Cinsiyet'].map({'Male': 1, 'Female': 0})
df['Sonuç'] = df['Sonuç'].map({1: 1, 2: 0}) # 1: Disease, 0: No Disease (mapped from 2)

# Features and Target
X = df.drop('Sonuç', axis=1)
y = df['Sonuç']

# Train Model
# Using parameters from notebook
model = XGBClassifier(
    n_estimators=500,
    max_depth=6,
    learning_rate=0.05,
    eval_metric='logloss',
    random_state=42
)
model.fit(X, y)

# Save Model
model.save_model('liver_model.json')
print("Model saved to liver_model.json")
