namespace GptFactCheckerApi.Model
{
    public class ClaimCheckResultsDto
    {
        public ClaimDto Claim { get; set; }
        public ClaimCheckDto? ClaimCheck { get; set; }
        public string AuthorUserId { get; set; }
        public bool IsFactChecked { get; set; } = false;
        public bool IsStored { get; set; } = false;
        public List<string> Messages { get; set; } = new();
    }
}
