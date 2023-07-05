import json

# Load JSON file 1
json_file1_path = "C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\Data\climateArgumentData_modified.json"
with open(json_file1_path, 'r', encoding='utf-8') as file1:
    json_content1 = json.load(file1)
    
# Load JSON file 2
json_file2_path = "C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\Data\climateArgumentData_generated_Summaries.json"
with open(json_file2_path, 'r', encoding='utf-8') as file2:
    json_content2 = json.load(file2)

# List to store the new objects
merged_objects = []

# Convert json_content2 into a dictionary for easy lookups
json_content2_dict = {item['Id']: item for item in json_content2}

# Add properties found in file2 to the objects with the same ID in file1
for item in json_content1:
    # Check if item exists in json_content2_dict
    if item['Id'] in json_content2_dict:
        # Merge the properties
        item.update(json_content2_dict[item['Id']])
    
    # Add to the list of new objects
    merged_objects.append(item)

# Save the new objects as JSON
output_json_file_path = "C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\Data\climateArgumentData_merged.json"
with open(output_json_file_path, 'w', encoding='utf-8') as file:
    json.dump(merged_objects, file, indent=4)  # Use indent=4 for pretty-print

