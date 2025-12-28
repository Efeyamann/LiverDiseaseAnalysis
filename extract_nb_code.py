import json

with open('KaraciğerHastalığıTespiti.ipynb', 'r', encoding='utf-8') as f:
    nb = json.load(f)

for cell in nb['cells']:
    if cell['cell_type'] == 'code':
        print("".join(cell['source']))
        print("\n" + "#" * 20 + "\n")
