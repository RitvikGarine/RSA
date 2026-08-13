namespace RSA;

abstract class Program
{
	static void Main()
	{
		RsaAlgorithm test = new RsaAlgorithm();
		test.GenerateKeys();
		//string cT = test.Encrypt("hello world");
		Console.Write("Enter the text to be encrypted: ");
		string cT = test.Encrypt(Console.ReadLine());
		string pT = test.Decrypt(cT);
		Console.WriteLine(cT);
		Console.WriteLine(pT);
	}
}