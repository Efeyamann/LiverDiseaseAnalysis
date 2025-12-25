import sys
import pandas as pd
import joblib
from xgboost import XGBClassifier
import json
import os

def predict(args):
    # Expected order: Yaş, Cinsiyet, Toplam_Bilirubin, Direkt_Bilirubin, Alkali_Fosfataz, 
    # Alanin_Aminotransferaz, Aspartat_Aminotransferaz, Toplam_Proteinler, Albümin, Albumin_Globulin_Orani
    
    if len(args) != 10:
        print("Error: Expected 10 arguments")
        return

    features = [
        float(args[0]), # Yaş
        int(args[1]),   # Cinsiyet (1 for Male, 0 for Female)
        float(args[2]), # Toplam_Bilirubin
        float(args[3]), # Direkt_Bilirubin
        float(args[4]), # Alkali_Fosfataz
        float(args[5]), # Alanin_Aminotransferaz
        float(args[6]), # Aspartat_Aminotransferaz
        float(args[7]), # Toplam_Proteinler
        float(args[8]), # Albümin
        float(args[9])  # Albumin_Globulin_Orani
    ]

    # Create DataFrame to ensure feature names match (XGBoost can be picky about feature names if saved with them)
    feature_names = [
        'Yaş', 'Cinsiyet', 'Toplam_Bilirubin', 'Direkt_Bilirubin',
        'Alkali_Fosfataz', 'Alanin_Aminotransferaz', 'Aspartat_Aminotransferaz',
        'Toplam_Proteinler', 'Albümin', 'Albumin_Globulin_Orani'
    ]
    
    df = pd.DataFrame([features], columns=feature_names)
    
    # Load model
    model = XGBClassifier()
    model_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'liver_model.json')
    model.load_model(model_path)
    
    prediction = model.predict(df)[0]
    probability = model.predict_proba(df)[0][1] # Probability of class 1 (Disease)
    
    result = {
        "Prediction": int(prediction),
        "Probability": float(probability)
    }
    
    print(json.dumps(result))

if __name__ == "__main__":
    predict(sys.argv[1:])
