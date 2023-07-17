import type Claim from "./Claim";
import type ClaimCheck from "./ClaimCheck";
import type Author from "./Author";

class ClaimCheckResult {
  public claim?: Claim;
  
  public isFactChecked: boolean = false;
  public isStored: boolean = false;
  public claimCheck?: ClaimCheck;
  public author?: Author;

  public messages?: string[];

  constructor() {
  }
}

export default ClaimCheckResult;