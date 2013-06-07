using System;
using System.Globalization;

namespace ServiceStack.Text
{
    public class ParseAssistant
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s")]
        public static bool TryParse(string s, out bool result)
        {
            var retVal = false;
            try
            {
                result = Convert.ToBoolean(s);
                retVal = true;
            }
            catch (FormatException) { result = false; }
            catch (InvalidCastException) { result = false; }
            return retVal;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s")]
        public static bool TryParse(string s, out long result)
        {
            var retVal = false;
            try
            {
                result = Convert.ToInt64(s);
                retVal = true;
            }
            catch (FormatException) { result = 0; }
            catch (InvalidCastException) { result = 0; }
            return retVal;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s")]
        public static bool TryParse(string s, out int result)
        {
            var retVal = false;
            try
            {
                result = Convert.ToInt32(s);
                retVal = true;
            }
            catch (FormatException) { result = 0; }
            catch (InvalidCastException) { result = 0; }
            return retVal;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s")]
        public static bool TryParse(string s, out decimal result)
        {
            bool retVal = false;
            try
            {
                result = Convert.ToDecimal(s);
                retVal = true;
            }
            catch (FormatException) { result = 0; }
            catch (InvalidCastException) { result = 0; }
            return retVal;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s")]
        public static bool TryParse(string s, NumberStyles ns, CultureInfo ci, out decimal result)
        {
            bool retVal = false;
            try
            {
                result = Convert.ToDecimal(s, ci);
                retVal = true;
            }
            catch (FormatException) { result = 0; }
            catch (InvalidCastException) { result = 0; }
            return retVal;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s")]
        public static bool TryParse(string s, out float result)
        {
            bool retVal = false;
            try
            {
                result = (float)Convert.ToDecimal(s);
                retVal = true;
            }
            catch (FormatException) { result = 0; }
            catch (InvalidCastException) { result = 0; }
            return retVal;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s")]
        public static bool TryParse(string s, NumberStyles ns, CultureInfo ci, out float result)
        {
            bool retVal = false;
            try
            {
                result = (float)Convert.ToDecimal(s, ci);
                retVal = true;
            }
            catch (FormatException) { result = 0; }
            catch (InvalidCastException) { result = 0; }
            return retVal;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s")]
        public static bool TryParse(string s, out double result)
        {
            bool retVal = false;
            try
            {
                result = Convert.ToDouble(s);
                retVal = true;
            }
            catch (FormatException) { result = 0; }
            catch (InvalidCastException) { result = 0; }
            return retVal;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s")]
        public static bool TryParse(string s, NumberStyles ns, CultureInfo ci, out double result)
        {
            bool retVal = false;
            try
            {
                result = Convert.ToDouble(s, ci);
                retVal = true;
            }
            catch (FormatException) { result = 0; }
            catch (InvalidCastException) { result = 0; }
            return retVal;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "s")]
        public static bool TryParse(string s, out char result)
        {
            var retVal = false;
            try
            {
                result = Convert.ToChar(s);
                retVal = true;
            }
            catch (FormatException) { result = '\0'; }
            catch (InvalidCastException) { result = '\0'; }
            return retVal;
        }


    }
}