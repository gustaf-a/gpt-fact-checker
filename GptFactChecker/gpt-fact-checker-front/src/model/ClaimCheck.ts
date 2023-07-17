import type ClaimCheckReaction from "./ClaimCheckReaction";

class ClaimCheck {
    public id: string;
    public userId: string;
    public label: string;
    public claimCheckText: string;
    public dateCreated: string;
    public references?: string[];

    public claimCheckReactions?: ClaimCheckReaction[];

    constructor() {
        this.id = '';
        this.userId = '';
        this.label = '';
        this.claimCheckText = '';
        this.dateCreated = new Date().toISOString(); //YYYY-MM-DDTHH:mm:ss.sssZ
    }
}

export default ClaimCheck;