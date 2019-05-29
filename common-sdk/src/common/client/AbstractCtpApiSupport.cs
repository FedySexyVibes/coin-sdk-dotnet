using System.Security.Cryptography;
using Coin.Common.Crypto;

namespace Coin.Common.Client
{
  public abstract class AbstractCtpApiSupport
  {
    protected readonly HMACSHA256 Signer;
    protected readonly RSA PrivateKey;
    protected readonly string ConsumerName;
    protected readonly int ValidPeriodInSeconds;
    protected readonly CtpApiClientUtil.HmacSignatureType HmacSignatureType;

    protected AbstractCtpApiSupport(
      string consumerName,
      HMACSHA256 signer,
      RSA privateKey,
      CtpApiClientUtil.HmacSignatureType hmacSignatureType = CtpApiClientUtil.HmacSignatureType.XDateAndDigest,
      int validPeriodInSeconds = CtpApiClientUtil.DefaultValidPeriodInSecs)
    {
      Signer = signer;
      PrivateKey = privateKey;
      ConsumerName = consumerName;
      ValidPeriodInSeconds = validPeriodInSeconds;
      HmacSignatureType = hmacSignatureType;
    }
  }
}
