import type Source from "./Source";

class ClaimWithSource {
	public id?: string;
	public claimSummarized: string;
	public claimRawText: string;
	public tags: string[];
	
	public source?: Source;

	constructor() {
		this.claimSummarized = "";
		this.claimRawText = "";
		this.tags = [];

		this.source = undefined;
	}
}

export default ClaimWithSource;
