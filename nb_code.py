import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
import io
import warnings
warnings.filterwarnings('ignore')
from google.colab import files
from sklearn.model_selection import train_test_split
from sklearn.ensemble import RandomForestClassifier
from xgboost import XGBClassifier
from sklearn.linear_model import LogisticRegression
from sklearn.svm import SVC
from sklearn.neighbors import KNeighborsClassifier
from sklearn.metrics import accuracy_score, confusion_matrix, classification_report

print("Kütüphaneler baþarýyla yüklendi.")

####################


url = "https://raw.githubusercontent.com/Efeyamann/LiverDiseaseAnalysis/main/Liver%20Patient%20Dataset%20(LPD)_train.csv"
try:
    df = pd.read_csv(url, encoding='latin1')
    print("Veri baþarýyla çekildi (latin1 kullanýldý).")
except Exception as e:
    print(f"Hata devam ediyor: {e}")
print("Veri GitHub üzerinden baþarýyla çekildi.")

####################

turkce_basliklar = [
    'Yaþ', 'Cinsiyet', 'Toplam_Bilirubin', 'Direkt_Bilirubin',
    'Alkali_Fosfataz', 'Alanin_Aminotransferaz', 'Aspartat_Aminotransferaz',
    'Toplam_Proteinler', 'Albümin', 'Albumin_Globulin_Orani', 'Sonuç'
]

df.columns = turkce_basliklar

print("--- VERÝ SETÝNÝN ÝLK 5 SATIRI (TÜRKÇE) ---")
display(df.head())

print("\n--- VERÝ SETÝ ÖZETÝ ---")
df.info()

####################

df['Albumin_Globulin_Orani'] = df['Albumin_Globulin_Orani'].fillna(df['Albumin_Globulin_Orani'].median())
df['Cinsiyet'] = df['Cinsiyet'].map({'Male': 1, 'Female': 0})
df['Sonuç'] = df['Sonuç'].map({1: 1, 2: 0})

X = df.drop('Sonuç', axis=1)
y = df['Sonuç']

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

print("Veri temizlendi ve eðitime hazýr hale getirildi.")

####################

for col in X_train.columns:
    if X_train[col].isnull().any():
        median_val = X_train[col].median()
        X_train[col] = X_train[col].fillna(median_val)
        X_test[col] = X_test[col].fillna(median_val)

# Random Forest
rf_model = RandomForestClassifier(n_estimators=500, max_depth=10, random_state=42)
rf_model.fit(X_train, y_train)
rf_preds = rf_model.predict(X_test)

# XGBoost
xgb_model = XGBClassifier(
    n_estimators=500,
    max_depth=6,
    learning_rate=0.05,
    eval_metric='logloss',
    random_state=42
)
xgb_model.fit(X_train, y_train)
xgb_preds = xgb_model.predict(X_test)

# Logistic Regression
lr_model = LogisticRegression(max_iter=1000, random_state=42)
lr_model.fit(X_train, y_train)
lr_preds = lr_model.predict(X_test)

# SVC
svc_model = SVC(random_state=42)
svc_model.fit(X_train, y_train)
svc_preds = svc_model.predict(X_test)

# KNN
knn_model = KNeighborsClassifier(n_neighbors=5)
knn_model.fit(X_train, y_train)
knn_preds = knn_model.predict(X_test)

print("Beþ model de baþarýyla eðitildi.")

####################

# Doðruluk
rf_acc = accuracy_score(y_test, rf_preds)
xgb_acc = accuracy_score(y_test, xgb_preds)
lr_acc = accuracy_score(y_test, lr_preds)
svc_acc = accuracy_score(y_test, svc_preds)
knn_acc = accuracy_score(y_test, knn_preds)

print(f"Random Forest Baþarýsý:      %{rf_acc*100:.2f}")
print(f"XGBoost Baþarýsý:            %{xgb_acc*100:.2f}")
print(f"Logistic Regression Baþarýsý:%{lr_acc*100:.2f}")
print(f"SVC Baþarýsý:                %{svc_acc*100:.2f}")
print(f"KNN Baþarýsý:                %{knn_acc*100:.2f}")

plt.figure(figsize=(15, 6))
sns.set_style("white")

plt.subplot(1, 2, 1)
models = ['Random Forest', 'XGBoost', 'Logistic Reg.', 'SVC', 'KNN']
accuracies = [rf_acc, xgb_acc, lr_acc, svc_acc, knn_acc]
colors = ["#4A90E2", "#50E3C2", "#FFC107", "#FF5722", "#9C27B0"]

ax = sns.barplot(x=models, y=accuracies, palette=colors)
plt.title('Model Performansý', fontsize=12, pad=15)
plt.ylim(0, 1)
for i, v in enumerate(accuracies):
    ax.text(i, v + 0.02, f"%{v*100:.1f}", ha='center', fontsize=10)
sns.despine()

plt.subplot(1, 2, 2)
# Random Forest konfüzyon matrisini göstermeye devam edelim
sns.heatmap(confusion_matrix(y_test, rf_preds), annot=True, fmt='d', cmap='Greys', cbar=False)
plt.title('Hata Daðýlýmý (Random Forest)', fontsize=12, pad=15)
plt.xlabel('Tahmin Edilen')
plt.ylabel('Gerçek Deðer')

plt.tight_layout()
plt.show();

####################

corr_matrix = df.corr()

plt.figure(figsize=(12, 10))
sns.set_theme(style="white")


heatmap = sns.heatmap(
    corr_matrix,
    annot=True,
    fmt=".2f",
    cmap='RdBu_r',
    vmin=-1, vmax=1,
    center=0,
    linewidths=.5,
    cbar_kws={"shrink": .8}
)

plt.title('Karaciðer Veri Seti - Deðiþkenler Arasý Korelasyon Matrisi', fontsize=16, pad=20)
plt.xticks(rotation=45, ha='right')
plt.yticks(rotation=0)
plt.show()

####################

