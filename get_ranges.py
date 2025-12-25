import pandas as pd
pd.set_option('display.max_columns', None)
pd.set_option('display.width', 1000)

try:
    df = pd.read_csv('Liver Patient Dataset (LPD)_train.csv', encoding='latin1')
    
    # Rename columns to match what we use
    df.columns = [
        'Age', 'Gender', 'Total_Bilirubin', 'Direct_Bilirubin',
        'Alkaline_Phosphotase', 'Alamine_Aminotransferase', 'Aspartate_Aminotransferase',
        'Total_Protiens', 'Albumin', 'Albumin_and_Globulin_Ratio', 'Result'
    ]
    
    stats = df.describe().loc[['min', 'max']]
    print(stats)
except Exception as e:
    print(e)
