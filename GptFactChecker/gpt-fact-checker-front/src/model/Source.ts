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
	public sourceImportedDate?: Date;
	public sourceCreatedDate?: Date;
	public sourceRawText: string;

	public claimsFirstExtractedDate?: Date;
	public claimsUpdatedDate?: Date;
	public claims: Claim[];

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
		this.sourceImportedDate = undefined;
		this.sourceCreatedDate = undefined;
		this.claimsFirstExtractedDate = undefined;
		this.claimsUpdatedDate = undefined;
		this.coverImageUrl = "";
		this.claims = [];
		this.sourceRawText = "";
	}
}

export default Source;
