using System.Numerics;
using System.Security.Cryptography;
namespace RSA;

public static class PrimeGeneration
{
	static Byte[] CandidateGenerator(int length)
	{
		byte[] number = new byte[length / 8];
		RandomNumberGenerator rand = RandomNumberGenerator.Create();
		rand.GetBytes(number);
		number[0] |= 128;
		number[(length / 8) - 1] |= 1;
		return number;
	}
	
	static bool CompareWithNPrimes(int max, byte[] number)
	{
		BigInteger n = new BigInteger(number, isUnsigned: true, isBigEndian: true);
		for (int i = 2; i <= max; i++) if (n % i == 0)
		{
			return false;
		}
		return true;
	}

	static bool FermatLTheorem(byte[] number)
	{
		BigInteger n = new BigInteger(number, isUnsigned: true, isBigEndian: true);
		if (BigInteger.ModPow(2, n - 1, n) == 1) return true;
		return false;
	}

	static bool MillRabTest(byte[] number, int repeats = 5)
	{
		BigInteger n = new BigInteger(number, isUnsigned: true, isBigEndian: true);
		int s = 0;
		BigInteger d = n - 1;
		while (d.IsEven)
		{
			d /= 2;
			s++;
		}
		Random rand = new Random();

		for (int i = 0; i < repeats; i++)
		{
			BigInteger a = rand.Next(2, 1000000000);
			BigInteger x = BigInteger.ModPow(a, d, n);
			BigInteger y = 0;
			
			for (int j = 0; j < s; j++)	
			{
				y = (x*x) % n;
				if (y == 1 && x != 1 && x != n -1) return false;
				x = y;
			}

			if (y != 1) return false;
		}
		return true;
	}
	
	public static byte[] PrimeGenerator()
	{
		byte[] candidate;
		while (true)
		{
			candidate = CandidateGenerator(1024);
			if (!CompareWithNPrimes(5000, candidate)) continue;
			if (!FermatLTheorem(candidate)) continue;
			if(!MillRabTest(candidate)) continue;
			break;
		}
		return candidate;
	}
}