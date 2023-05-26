import type ClaimCheck from "./ClaimCheck";

class Claim {
    public id: string;
    public claimSummarized: string;
    public claimRawText: string;
    public claimChecks: ClaimCheck[];
  
    constructor() {
      this.id = "";
      this.claimSummarized = "";
      this.claimRawText = "";
      this.claimChecks = [];
    }
  }

export default Claim;