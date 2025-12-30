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
    
    # Load models
    model_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'liver_models.joblib')
    saved_data = joblib.load(model_path)
    
    # Check if we have the new structure or old
    if isinstance(saved_data, dict) and 'best_model_name' in saved_data:
        models = saved_data['models']
        best_model_name = saved_data['best_model_name']
        
        # We only want to predict using the best model
        # Create a new dictionary with just the best model for the loop below (or just simplify logic)
        target_models = {best_model_name: models[best_model_name]}
    else:
        # Fallback for old format if someone runs this without retraining
        models = saved_data
        target_models = models
    
    results = {}
    
    for name, model in target_models.items():
        prediction = model.predict(df)[0]
        try:
            probability = model.predict_proba(df)[0][1] # Probability of class 1 (Disease)
        except AttributeError:
             # Some models might not support predict_proba or need calibration, 
             # but we configured SVC with probability=True and others support it.
             # If resizing issues occur (e.g. KNN), this might fail, but for single sample it should be fine.
             probability = float(prediction) # Fallback

        results[name] = {
            "Prediction": int(prediction),
            "Probability": float(probability)
        }
    
    print(json.dumps(results))

if __name__ == "__main__":
    predict(sys.argv[1:])
