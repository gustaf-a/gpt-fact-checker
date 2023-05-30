import type ClaimCheck from "./ClaimCheck";

class Claim {
	public id?: string;
	public claimSummarized: string;
	public claimRawText: string;
	public tags: string[];
	public claimChecks?: ClaimCheck[];

	constructor() {
		this.claimSummarized = "";
		this.claimRawText = "";
		this.tags = [];
	}
}

export default Claim;
