namespace Common.Utils
{
    /// <summary>The generic validation.</summary>
    public static class GenericValidation
    {
        /// <summary>The validate basic string.</summary>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentNullException">generic string validation</exception>
        public static void ValidateBasicString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>The validate basic string.</summary>
        /// <param name="value">The value.</param>
        /// <param name="attributeName">The attribute Name.</param>
        /// <exception cref="ArgumentNullException">generic string validation</exception>
        public static void ValidateBasicString(string value, string attributeName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(attributeName);
            }
        }

        /// <summary>The validate null or empty string.</summary>
        /// <param name="value">The value.</param>
        /// <param name="fieldName">The field Name.</param>
        public static void ValidateNullOrEmptyString(string value, string fieldName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception($"The value of the  field: '{fieldName}' has a null value.");
            }
        }
    }
}
