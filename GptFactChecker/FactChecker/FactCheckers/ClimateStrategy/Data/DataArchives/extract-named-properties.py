import json
import os

# Load the JSON file
json_file_path = r"C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\Data\climateArgumentData.json"
with open(json_file_path, 'r', encoding='utf-8') as file:
    json_content = json.load(file)

# Properties to extract
properties_to_extract = ["Id", "ArgumentText"]

# List to store the new objects
new_objects = []

# Process each object in the JSON array
for obj in json_content:
    # Extract properties and save to new_objects
    extracted_obj = {prop: obj[prop] for prop in properties_to_extract}
    new_objects.append(extracted_obj)

# Save the new objects as JSON
original_file_folder, original_file_name = os.path.split(json_file_path)
original_file_name_without_ext = os.path.splitext(original_file_name)[0]
new_file_name = f"{original_file_name_without_ext}_extracted_" + "_".join(properties_to_extract) + ".json"

output_json_file_path = os.path.join(original_file_folder, new_file_name)
with open(output_json_file_path, 'w', encoding='utf-8') as file:
    json.dump(new_objects, file, indent=4)  # Use indent=4 for pretty-print
