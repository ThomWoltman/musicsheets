namespace DPA_Musicsheets.Models
{
	public class LilypondToken
    {
        public LilypondTokenKind TokenKind { get; set; }
        public string Value { get; set; }

        public LilypondToken NextToken { get; set; }

        public LilypondToken PreviousToken { get; set; }
    }
}
