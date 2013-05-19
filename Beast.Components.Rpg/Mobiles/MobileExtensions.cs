using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Mobiles
{
    public static class MobileExtensions
    {
        #region A
        public static string A(this IMobile mobile)
        {
            return mobile.Name.A();
        }
        public static string A(this IMobile mobile, int quantity)
        {
            return mobile.Name.A(quantity);
        }
        public static string A(this IMobile mobile, bool isSentenceStart)
        {
            return mobile.Name.A(false, isSentenceStart);
        }
        public static string A(this IMobile mobile, bool isSentenceStart, int quantity)
        {
            return mobile.Name.A(false, isSentenceStart, quantity);
        }
        public static string A(this ICharacter mobile)
        {
            return mobile.Name.A(true);
        }
        public static string A(this ICharacter mobile, int quantity)
        {
            return mobile.Name.A(true, quantity);
        }
        public static string A(this ICharacter mobile, bool isSentenceStart)
        {
            return mobile.Name.A(true, isSentenceStart);
        }
        public static string A(this ICharacter mobile, bool isSentenceStart, int quantity)
        {
            return mobile.Name.A(true, isSentenceStart, quantity);
        }
        #endregion


        #region The
        public static string The(this IMobile mobile)
        {
            return mobile.Name.The();
        }
        public static string The(this IMobile mobile, int quantity)
        {
            return mobile.Name.The(quantity);
        }
        public static string The(this IMobile mobile, bool isSentenceStart)
        {
            return mobile.Name.The(false, isSentenceStart);
        }
        public static string The(this IMobile mobile, bool isSentenceStart, int quantity)
        {
            return mobile.Name.The(false, isSentenceStart, quantity);
        }
        public static string The(this ICharacter mobile)
        {
            return mobile.Name.The(true);
        }
        public static string The(this ICharacter mobile, int quantity)
        {
            return mobile.Name.The(true, quantity);
        }
        public static string The(this ICharacter mobile, bool isSentenceStart)
        {
            return mobile.Name.The(true, isSentenceStart);
        }
        public static string The(this ICharacter mobile, bool isSentenceStart, int quantity)
        {
            return mobile.Name.The(true, isSentenceStart, quantity);
        }
        #endregion


        #region Your
        public static string Your(this IMobile mobile)
        {
            return mobile.Name.Your();
        }
        public static string Your(this IMobile mobile, int quantity)
        {
            return mobile.Name.Your(quantity);
        }
        public static string Your(this IMobile mobile, bool isSentenceStart)
        {
            return mobile.Name.Your(false, isSentenceStart);
        }
        public static string Your(this IMobile mobile, bool isSentenceStart, int quantity)
        {
            return mobile.Name.Your(false, isSentenceStart, quantity);
        }
        public static string Your(this ICharacter mobile)
        {
            return mobile.Name.Your(true);
        }
        public static string Your(this ICharacter mobile, int quantity)
        {
            return mobile.Name.Your(true, quantity);
        }
        public static string Your(this ICharacter mobile, bool isSentenceStart)
        {
            return mobile.Name.Your(true, isSentenceStart);
        }
        public static string Your(this ICharacter mobile, bool isSentenceStart, int quantity)
        {
            return mobile.Name.Your(true, isSentenceStart, quantity);
        }
        #endregion
    }
}
