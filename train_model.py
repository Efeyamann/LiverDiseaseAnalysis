import pandas as pd
import numpy as np
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
# Fill missing values for all numeric columns
numeric_cols = df.select_dtypes(include=['float64', 'int64']).columns
for col in numeric_cols:
    df[col] = df[col].fillna(df[col].median())

# Fill missing values for categorical column (Cinsiyet)
if df['Cinsiyet'].isnull().any():
    df['Cinsiyet'] = df['Cinsiyet'].fillna(df['Cinsiyet'].mode()[0])

df['Cinsiyet'] = df['Cinsiyet'].map({'Male': 1, 'Female': 0})
df['Sonuç'] = df['Sonuç'].map({1: 1, 2: 0}) # 1: Disease, 0: No Disease (mapped from 2)

# Features and Target
X = df.drop('Sonuç', axis=1)
y = df['Sonuç']

# Train Models
models = {}

# 1. Random Forest
from sklearn.ensemble import RandomForestClassifier
rf = RandomForestClassifier(n_estimators=500, max_depth=10, random_state=42)
rf.fit(X, y)
models['Random Forest'] = rf

# 2. XGBoost
from xgboost import XGBClassifier
xgb = XGBClassifier(
    n_estimators=500,
    max_depth=6,
    learning_rate=0.05,
    eval_metric='logloss',
    random_state=42
)
xgb.fit(X, y)
models['XGBoost'] = xgb

# 3. Logistic Regression
from sklearn.linear_model import LogisticRegression
lr = LogisticRegression(max_iter=1000, random_state=42)
lr.fit(X, y)
models['Logistic Regression'] = lr

# 4. SVC
from sklearn.svm import SVC
svc = SVC(probability=True, random_state=42)
svc.fit(X, y)
models['SVC'] = svc

# 5. KNN
from sklearn.neighbors import KNeighborsClassifier
knn = KNeighborsClassifier(n_neighbors=5)
knn.fit(X, y)
models['KNN'] = knn

# Save Models
import joblib
joblib.dump(models, 'liver_models.joblib')
print("All 5 models saved to liver_models.joblib")
