namespace tjenamannen.Services.Rimmaskin
{
	public interface IRimmaskinService
	{
		public IList<string> GetRhymingWords(string word);
		public void UploadWordsFromJson(string word);
		public void UploadWordsShittyWay();
	}
}
