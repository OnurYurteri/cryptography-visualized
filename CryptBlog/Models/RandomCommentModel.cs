using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CryptBlog.Models
{
    public class RandomCommentModel
    {
        private int commentSize=100;
        private string characters = "abcdefghijklmnopqrstuvwxyz";
        public string generatedRandom;
        public Dictionary<char, int> freq = new Dictionary<char, int>();

        public RandomCommentModel()
        {
            Create();
            ComputeFreq(generatedRandom);
        }

        private void Create()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[4];
            StringBuilder result = new StringBuilder(commentSize);
            int index;
            for (int i = 0; i < commentSize; i++)
            {
                provider.GetBytes(byteArray);
                index = (Math.Abs(BitConverter.ToInt32(byteArray, 0)) % 26);
                result.Append(characters[index]);
            }
            generatedRandom= result.ToString();
        }

        private void ComputeFreq(string random)
        {
            char[] chars = random.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (!freq.ContainsKey(chars[i]))
                {
                    freq.Add(chars[i], 1);
                }
                else
                {
                    freq[chars[i]]++;
                }
            }
        }
    }
}