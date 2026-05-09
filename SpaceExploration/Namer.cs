using System;
using System.Formats.Asn1;

namespace SpaceExploration
{
    class Namer
    {
        private const int MinStemLen = 3;
        private const int MaxStemLen = 9;
        private static readonly char[] _vowels = ['a', 'e', 'i', 'o', 'u', 'y'];
        private static readonly char[] _hardConsonants = ['b', 'c', 'd', 'f', 'g', 'j', 'k', 'm', 'n', 'p', 'q', 't', 'v', 'w', 'x', 'z'];
        private static readonly char[] _softConsonants = ['h', 'l', 'r', 's'];

        public static string BuildName(int type, string name, int instancePos)
        {
            string fullName = name;

            if (type == 1)
                fullName += " " + (char)('@' + instancePos);
            else if (type == 2)
                fullName += " " + (char)('0' + instancePos);

            return fullName;
        }

        public static string CreateStem()
        {
            int totalChar = Program.Rand.Next(MinStemLen, MaxStemLen + 1);
            int vowelChance = 25;
            int consonantChance = 50; // All consonants below or equal to threshold, soft consonants after
            string Stem = "";
            char currentChar = ' ';
            int currentCharType = 0;
            int prevCharType = 0;

            for (int i = 0; i < totalChar; i++)
            {
                    if (i == 1 && currentCharType > 1)
                        (vowelChance, consonantChance) = (100, 0);
                    else if (i == totalChar - 1)
                    {
                        if (prevCharType == 1 && currentCharType == 1)
                            (vowelChance, consonantChance) = (0, 50);
                        else if (prevCharType == 1 && currentCharType == 2)
                            (vowelChance, consonantChance) = (100, 0);
                        else if (prevCharType == 1 && currentCharType == 3)
                            (vowelChance, consonantChance) = (50, 50);
                        else if (prevCharType == 3 && currentCharType > 1)
                            (vowelChance, consonantChance) = (100, 0);
                        else if (prevCharType > 1 && currentCharType == 1)
                            (vowelChance, consonantChance) = (33, 34);
                        else if (prevCharType == 2 && currentCharType > 1)
                            (vowelChance, consonantChance) = (100, 0);
                        else
                            (vowelChance, consonantChance) = (100, 0);
                    }
                    else if (i > 0 && i < totalChar - 1)
                    {
                        if (currentCharType == 1 && prevCharType == 1)
                            (vowelChance, consonantChance) = (0, 100);
                        else if (currentCharType == 1 && prevCharType > 1)
                            (vowelChance, consonantChance) = (25, 50);
                        else if (currentCharType > 1 && prevCharType == 1)
                            (vowelChance, consonantChance) = (50, 25);
                        else if (currentCharType > 1 && prevCharType > 1)
                            (vowelChance, consonantChance) = (100, 0);
                        else
                            (vowelChance, consonantChance) = (50, 50);
                    }

                    (currentChar, int newCharType) =
                        ChooseLetterType(vowelChance, consonantChance, currentChar == ' ');

                    Stem += currentChar;

                    prevCharType = currentCharType;
                    currentCharType = newCharType;
                }

            return Stem;
        }

        private static Tuple<char, int> ChooseLetterType(int vowelChance, int consonantChance, bool capitalize)
        {
            int letterType = Program.Rand.Next(1, 101);
            int lowerCase = capitalize ? 32 : 0;

            if (letterType <= vowelChance)
                return Tuple.Create((char)(_vowels[Program.Rand.Next(_vowels.Length)] - lowerCase), 1);

            if (letterType <= vowelChance + consonantChance)
                return Tuple.Create((char)(_hardConsonants[Program.Rand.Next(_hardConsonants.Length)] - lowerCase), 2);

            return Tuple.Create((char)(_softConsonants[Program.Rand.Next(_softConsonants.Length)] - lowerCase), 3);
        }
    }
}