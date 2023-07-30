import type Source from "./Source";

class SourceExtractionResponse {
	public isSuccess: boolean = false;
	public messages: string[] = [];
	public sourceId: string = "";
	public collectedSource?: Source;
}

export default SourceExtractionResponse;
