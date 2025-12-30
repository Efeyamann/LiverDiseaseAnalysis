import pandas as pd
import numpy as np
from sklearn.model_selection import train_test_split
from sklearn.metrics import accuracy_score
from imblearn.over_sampling import SMOTE
import joblib

# Load dataset
try:
    df = pd.read_csv('Liver Patient Dataset (LPD)_train.csv', encoding='latin1')
except FileNotFoundError:
    # Fallback if local file not found
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

# Split data FIRST validation
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42, stratify=y)

# Balance training dataset using SMOTE
print(f"Original train class distribution:\n{y_train.value_counts()}")
smote = SMOTE(random_state=42)
X_train_res, y_train_res = smote.fit_resample(X_train, y_train)
print(f"Resampled train class distribution:\n{y_train_res.value_counts()}")

# Models dictionary
models = {}
accuracies = {}

# 1. Random Forest
from sklearn.ensemble import RandomForestClassifier
rf = RandomForestClassifier(n_estimators=500, max_depth=10, random_state=42)
rf.fit(X_train_res, y_train_res)
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
xgb.fit(X_train_res, y_train_res)
models['XGBoost'] = xgb

# 3. Logistic Regression
from sklearn.linear_model import LogisticRegression
lr = LogisticRegression(max_iter=1000, random_state=42)
lr.fit(X_train_res, y_train_res)
models['Logistic Regression'] = lr

# 4. SVC
from sklearn.svm import SVC
svc = SVC(probability=True, random_state=42)
svc.fit(X_train_res, y_train_res)
models['SVC'] = svc

# 5. KNN
from sklearn.neighbors import KNeighborsClassifier
knn = KNeighborsClassifier(n_neighbors=5)
knn.fit(X_train_res, y_train_res)
models['KNN'] = knn

# Evaluate and find best model
print("\nModel Evaluation:")
best_model_name = ""
best_acc = 0.0

for name, model in models.items():
    y_pred = model.predict(X_test)
    acc = accuracy_score(y_test, y_pred)
    accuracies[name] = acc
    print(f"{name}: {acc:.4f}")
    
    if acc > best_acc:
        best_acc = acc
        best_model_name = name

print(f"\nBest Model: {best_model_name} with Accuracy: {best_acc:.4f}")

# Save Models and Metadata
# We save a dictionary containing the models and the best model name
save_data = {
    'models': models,
    'best_model_name': best_model_name,
    'accuracies': accuracies
}

joblib.dump(save_data, 'liver_models.joblib')
print("Models and metadata saved to liver_models.joblib")
