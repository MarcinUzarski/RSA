
using System.ComponentModel;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Channels;

List<int> primesNumbers = new(GeneratePrimesNaive(30));
List<int> listOfE = new List<int>();
List<int> pow = new List<int>();

Random random = new();

int e = 0, d= 0;

int index = random.Next(0, primesNumbers.Count-15);
int index2 = random.Next(16, primesNumbers.Count);
int p = primesNumbers[index];
int q = primesNumbers[index2];
int n = (p * q);
int phiN = ((p - 1) * (q - 1));

////// ZNALEZENIE MOŻLIWYCH E
for (int i = 3; i < phiN; i += 2)
{
    if (gcd(i, phiN) == 1)
    {
        listOfE.Add(i);
        if (listOfE.Count == 10)
        {
            e = listOfE[random.Next(listOfE.Count)];
            break;
        }
    }
}
////// WYLICZANIE "D"
int k = 0;
while (true)
{
    int x = 1 + (k * phiN);
    if (x % e == 0)
    {
        d = x / e;
        break;
    }
    k++;
}
////////////////////////////////////////////////////////////////////////////////////
/////////////////////              SZYFROWANIE           //////////////////////////
//////////////////////////////////////////////////////////////////////////////////

Console.Write(@"Wiadomość do zaszyfrowania: ");
string plaintext = Console.ReadLine();
byte[] plainTextByte = Encoding.ASCII.GetBytes(plaintext);
BigInteger [] cypherText = new BigInteger[plainTextByte.Length];

for (int i = 0; i < plainTextByte.Length; i++)
{
    cypherText[i] = (BigInteger.Pow(plainTextByte[i], e) % n);
}

string cypherTextString ="";
for (int i = 0; i <= plainTextByte.Length-1; i++)
{
    cypherTextString +=
        cypherText[i]
            .ToString("X");
}

Console.WriteLine(@"Wiadomość zaszyfrowana : {0}", cypherTextString);

////////////////////////////////////////////////////////////////////////////////////
/////////////////////              ROZSZYFROWANIE           ///////////////////////
///////////////////////////////////////////////////////////////////////////////////

BigInteger[] decode = new BigInteger[cypherText.Length];

for (int i = 0; d != 0; i++)
{
    if (d % 2 == 1)
    {
        pow.Add((int)Math.Pow(2, i));
    }
    d /= 2;
}

for (int j = 0; j < plainTextByte.Length; j++)
{
    for (int i = 0; i < pow.Count; i++)
    {
        if (i == 0)
            decode[j] = BigInteger.Pow(cypherText[j], pow[i]) % n;
        else
            decode[j] *= BigInteger.Pow(cypherText[j], pow[i]) % n;
    }

    decode[j] %= n;
}

byte[] toAscii = new byte[decode.Length];

for (int i = 0; i < decode.Length; i++)
{
    toAscii[i] = (byte)decode[i];
}

Console.WriteLine(@"Wiadomość odszyfrowana: {0}", Encoding.ASCII.GetString(toAscii));

    static int gcd(int a, int b)
{
    while (a != 0 && b != 0)
    {
        if (a > b)
            a %= b;
        else
            b %= a;
    }

    return a | b;
}


static List<int> GeneratePrimesNaive(int n)
{
    List<int> primes = new List<int>();
    primes.Add(2);
    int nextPrime = 3;
    while (primes.Count < n)
    {
        ulong sqrt = (ulong)Math.Sqrt(nextPrime);
        bool isPrime = true;
        for (int i = 0; (ulong)primes[i] <= sqrt; i++)
        {
            if (nextPrime % primes[i] == 0)
            {
                isPrime = false;
                break;
            }
        }
        if (isPrime)
        {
            primes.Add(nextPrime);
        }
        nextPrime += 2;
    }
    return primes;


}