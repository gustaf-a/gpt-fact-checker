import json

# Load the JSON file
json_file_path = "C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\Data\climateArgumentData_modified.json"
with open(json_file_path, 'r', encoding='utf-8') as file:
    json_content = json.load(file)

# List to store the new objects
new_objects = []

# Process each object in the JSON array
for obj in json_content:
    # If the CounterArgumentSummary is empty
    if not obj.get('CounterArgumentSummary'):
        # Create a new object with Id, CounterArgumentText and CounterArgumentSummary
        new_obj = {
            'Id': obj.get('Id'), 
            'CounterArgumentText': obj.get('CounterArgumentText', '<add value here>')
        }

        # Add the new object to the list
        new_objects.append(new_obj)

# Save the new objects as JSON
output_json_file_path = "C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\Data\climateArgumentData_withoutSummary.json"
with open(output_json_file_path, 'w', encoding='utf-8') as file:
    json.dump(new_objects, file, indent=4)  # Use indent=4 for pretty-print
