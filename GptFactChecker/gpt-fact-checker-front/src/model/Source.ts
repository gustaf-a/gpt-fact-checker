import type Claim from "./Claim";

class Source {
	public id: string;
	public name: string;
	public language: string;
	public description: string;
	public coverImageUrl: string;

	public tags: string[];
	
	public sourceType: string;
	public sourcePerson: string;
	public sourceContext: string;
	public sourceUrl: string;
	public sourceRawText: string;
	public sourceImportedDate?: string;
	public sourceCreatedDate?: string;

	public claimsFirstExtractedDate: string;
	public claimsUpdatedDate: string;
	public claims?: Claim[];

	constructor() {
		this.id = "";
		this.name = "";
		this.language = "";
		this.description = "";
		this.tags = [];
		this.sourceType = "";
		this.sourcePerson = "";
		this.sourceContext = "";
		this.sourceUrl = "";
		this.sourceImportedDate = "";
		this.sourceCreatedDate = "";
		this.claimsFirstExtractedDate = "";
		this.claimsUpdatedDate = "";
		this.coverImageUrl = "";
		this.sourceRawText = "";
	}
}

export default Source;
