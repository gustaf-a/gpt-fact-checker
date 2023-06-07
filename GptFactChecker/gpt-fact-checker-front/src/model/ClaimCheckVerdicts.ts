export interface Verdict {
	name: string;
	color: string;
	description: string;
	image: string;
}

export const claimCheckVerdicts: Verdict[] = [
	{
		name: "True",
		color: "#008000", // Green
		description:
			"The claim is factually correct and backed by credible sources",
		image: "true.png",
	},
	{
		name: "Misleading",
		color: "#FFA500", // Orange
		description: "The claim is misleading",
		image: "misleading.png",
	},
	{
		name: "Out of Context",
		color: "#FFA500", // Orange
		description:
			"The information is true, but it has been presented in a misleading way because essential context is missing",
		image: "outofcontext.png",
	},
	{
		name: "Exaggerated",
		color: "#FFFF00", // Yellow
		description:
			"The claim contains truth but overstates or overemphasizes certain aspects",
		image: "exaggerated.png",
	},
	{
		name: "Understated",
		color: "#FFFF00", // Yellow
		description:
			"The claim downplays or understates important details or impacts",
		image: "understated.png",
	},
	{
		name: "Opinion",
		color: "#FFFF00", // Yellow
		description:
			"The claim expresses a personal belief or value judgement rather than stating a fact",
		image: "opinion.png",
	},
	{
		name: "False",
		color: "#FF0000", // Red
		description: "The claim is incorrect based on available evidence",
		image: "false.png",
	},
	{
		name: "Correct Attribution",
		color: "#008000", // Green
		description: "The claim accurately attributes a statement to someone",
		image: "correctattribution.png",
	},
	{
		name: "False Attribution",
		color: "#FF0000", // Red
		description: "The claim attributes a false statement to someone",
		image: "falseattribution.png",
	},
	{
		name: "Unverifiable",
		color: "#808080", // Grey
		description: "The claim is unverifiable",
		image: "unverifiable.png",
	},
	{
		name: "Unsupported",
		color: "#808080", // Grey
		description: "The claim is not supported by available evidence",
		image: "unsupported.png",
	}
];
