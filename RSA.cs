using System.Numerics;
using System.Text;
namespace RSA;
using static PrimeGeneration;

public class RsaAlgorithm
{
	BigInteger _n;
	int _e;
	BigInteger _d;
	
	BigInteger Lcm(BigInteger a, BigInteger b)
	{
		return (a / BigInteger.GreatestCommonDivisor(a, b)) * b;
	}
	
	static BigInteger ModInverse(BigInteger a, BigInteger m)
	{
		BigInteger m0 = m;
		BigInteger y = 0, x = 1;
		
		while (a > 1)
		{
			BigInteger q = a / m;
			BigInteger t = m;

			m = a % m;
			a = t;
			t = y;

			y = x - q * y;
			x = t;
		}

		x += m0;
		return x;
	}
	
	public (BigInteger, int, BigInteger) GenerateKeys()
	{
		byte[] p = PrimeGenerator();
		byte[] q = PrimeGenerator();
		BigInteger pInt = new BigInteger(p, isUnsigned: true, isBigEndian: true);
		BigInteger qInt = new BigInteger(q, isUnsigned: true, isBigEndian: true);
		BigInteger n = pInt * qInt;
		BigInteger totientN = Lcm(pInt - 1, qInt - 1);

		int e = 65537;
		BigInteger d = ModInverse(e, totientN);
		_n = n;
		_e = e;
		_d = d;
		return (n, e, d);
	}

	public string Encrypt(string message)
	{
		char[] messageArray = message.ToCharArray();
		byte[] messageBytes = messageArray.Select(x => (byte)x).ToArray();
		BigInteger messageInt = new BigInteger(messageBytes);
		BigInteger cipherText = BigInteger.ModPow(messageInt, _e, _n);
		return cipherText.ToString();
	}

	public string Decrypt(string message)
	{
		BigInteger cipherText = BigInteger.Parse(message);
		BigInteger plainText = BigInteger.ModPow(cipherText, _d, _n);
		return Encoding.UTF8.GetString(plainText.ToByteArray());
	}
}