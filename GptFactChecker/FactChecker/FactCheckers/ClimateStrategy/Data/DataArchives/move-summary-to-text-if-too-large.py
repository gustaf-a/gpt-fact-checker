import json

# Load the JSON file
json_file_path = "C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\Data\climateArgumentData.json"
with open(json_file_path, 'r', encoding='utf-8') as file:
    json_content = json.load(file)

# List to store the new objects
new_objects = []

# Process each object in the JSON array
for obj in json_content:
    # Get the properties from the current object
    new_obj = obj.copy()  # copy all properties

    # If the CounterArgumentSummary is longer than 180 chars,
    # then move the value to CounterArgumentText
    # and leave CounterArgumentSummary set to empty string.
    if len(new_obj.get('CounterArgumentSummary', '')) > 180:
        new_obj['CounterArgumentText'] = new_obj['CounterArgumentSummary']
        new_obj['CounterArgumentSummary'] = ''

    # Add the new object to the list
    new_objects.append(new_obj)

# Save the new objects as JSON
output_json_file_path = "C:\PrivateRepos\gpt-fact-checker\GptFactChecker\FactChecker\FactCheckers\ClimateStrategy\Data\climateArgumentData_modified.json"
with open(output_json_file_path, 'w', encoding='utf-8') as file:
    json.dump(new_objects, file, indent=4)  # Use indent=4 for pretty-print
