# Bu kodu not defterindeki 10. hücreye (veri temizleme hücresi) yapıştırın.

# Eksik verileri doldurma (Tüm sütunlar için)
# Sayısal sütunlar için medyan kullan
numeric_cols = df.select_dtypes(include=['float64', 'int64']).columns
for col in numeric_cols:
    df[col] = df[col].fillna(df[col].median())

# Kategorik sütun (Cinsiyet) için en sık geçen değeri (mode) kullan
if df['Cinsiyet'].isnull().any():
    df['Cinsiyet'] = df['Cinsiyet'].fillna(df['Cinsiyet'].mode()[0])

# Mapping işlemleri
df['Cinsiyet'] = df['Cinsiyet'].map({'Male': 1, 'Female': 0})
df['Sonuç'] = df['Sonuç'].map({1: 1, 2: 0})

# Eksik veri kontrolü
print("Eksik veri sayısı:")
print(df.isnull().sum())

X = df.drop('Sonuç', axis=1)
y = df['Sonuç']

X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.2, random_state=42)

print("Veri temizlendi ve eğitime hazır hale getirildi.")
