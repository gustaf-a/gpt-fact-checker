import type Claim from "./Claim";

class FactExtractionResponse {
  public sourceId: string = '';
  public isStored: boolean = false;
  public extractedClaims?: Claim[];

  constructor() {
  }
}

export default FactExtractionResponse;