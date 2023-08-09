# gpt-fact-checker: FactFriend
FactFriend is a tool designed to streamline and simplify the fact-checking process. 
With FactFriend, users can easily identify claims from various sources, and either create their own fact checks or browse existing ones.

![Solution Flowchart](SourceDetailsScreenShot.JPG)

![Source details page screenshot](overview.png)

# How It Functions

## 1. Sources & Claims:

Sources encompass a broad range of mediums, including videos, news articles, podcasts, and more.
Each source is represented with its individual claims, itemized for clarity.

## 2. Claim Extraction:

Users have the flexibility to manually input claims or rely on an automated extraction from the raw text.
FactFriend leverages the OpenAI GPT API to automatically extract claims from a given source.
Certain supported sources can automatically have their raw text extracted.

## 3. Fact-Checking Process:

Upon the listing of a claim, users have the opportunity to attach their own fact checks to that specific claim.
FactFriend's integration with the OpenAI GPT API facilitates automated fact-checking, utilizing topics and predefined references.

### 3.1 Topic-Based Fact Checking:

In the backend, the process begins by trying to find a match between the claim's tags and a specific topic.
If there's a match, keywords from all associated references for that topic are retrieved.
These keywords are then submitted to the GPT API to determine their relevance to the claim in question.
Finally, each individual claim is cross-referenced with the GPT API using summaries of the relevant references.

# Technology Stack

Frontend: Vue 3 with TypeScript, connected to SupaBase for user login
Backend: C#
External Services: OpenAI GPT API, OpenAI Whisper API

# Future development

There's significant potential for enhancing and expanding the utility of this project:

- Implementing an intuitive interface to browse through various sources.
- Refining and augmenting the efficiency of automated fact-checking.
- Streamlining the process for registering new topics and references.
- Implementing proper a proper databases

# License

This project is licensed under the MIT License.
