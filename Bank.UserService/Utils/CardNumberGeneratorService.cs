namespace Bank.UserService.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

public static class CardNumberGeneratorService
{
    private static readonly Dictionary<string, string> _cardPrefixes = new()
    {
        { "Visa Debit", "4" },
        { "Visa Business", "4" },
        { "MasterCard Gold", "52" },
        { "DinaCard Standard", "9891" },
        { "American Express Platinum", "37" }
    };

    private static readonly Dictionary<string, int> _cardLengths = new()
    {
        { "Visa Debit", 16 },
        { "Visa Business", 16 },
        { "MasterCard Gold", 16 },
        { "DinaCard Standard", 16 },
        { "American Express Platinum", 15 }
    };

    private static readonly Random _random = new Random();

    public static (string CardNumber, string Cvv) GenerateCardDetails(string cardTypeName)
    {
        string cardNumber = GenerateCardNumber(cardTypeName);
        string cvv = GenerateCvv(cardTypeName);
        return (cardNumber, cvv);
    }

    public static string GenerateCardNumber(string cardTypeName)
    {
        if (!_cardPrefixes.TryGetValue(cardTypeName, out string prefix))
        {
            throw new ArgumentException($"Unknown card type: {cardTypeName}");
        }

        int cardLength = _cardLengths[cardTypeName];

        // Generate the middle digits randomly
        string partialNumber = prefix;
        int digitsToGenerate = cardLength - prefix.Length - 1;

        for (int i = 0; i < digitsToGenerate; i++)
        {
            partialNumber += _random.Next(0, 10).ToString();
        }

        // Calculate the Luhn check digit
        int checkDigit = CalculateLuhnCheckDigit(partialNumber);
        string cardNumber = partialNumber + checkDigit;

        if (!ValidateLuhn(cardNumber))
        {
            return GenerateCardNumber(cardTypeName);
        }

        return cardNumber;
    }

    public static string GenerateCvv(string cardTypeName)
    {
        int cvvLength = cardTypeName == "American Express Platinum" ? 4 : 3;
        using var rng = RandomNumberGenerator.Create();
        byte[] data = new byte[cvvLength];
        rng.GetBytes(data);

        return string.Concat(data.Select(b => (b % 10).ToString()));
    }

    private static int CalculateLuhnCheckDigit(string partialCardNumber)
    {
        int sum = 0;
        bool isSecondDigit = true;

        for (int i = partialCardNumber.Length - 1; i >= 0; i--)
        {
            int digit = int.Parse(partialCardNumber[i].ToString());

            if (isSecondDigit)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
            isSecondDigit = !isSecondDigit;
        }

        return (10 - (sum % 10)) % 10;
    }

    public static bool ValidateLuhn(string cardNumber)
    {
        int sum = 0;
        bool isSecondDigit = false;

        for (int i = cardNumber.Length - 1; i >= 0; i--)
        {
            int digit = int.Parse(cardNumber[i].ToString());

            if (isSecondDigit)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
            isSecondDigit = !isSecondDigit;
        }

        return sum % 10 == 0;
    }
}