
namespace Beast.IO
{
	public class FileRepository : IRepository
	{
		public string DirectoryPath { get; set; }

		public FileRepository(string path)
		{
			DirectoryPath = path;
		}

		public override string ToString()
		{
			return string.Format("FileRepository: PATH={0}", DirectoryPath);
		}
	}
}
