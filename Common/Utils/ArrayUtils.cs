namespace Common.Utils
{
    public class ArrayUtils
    {

        public bool IsEqual(byte[] source, byte[] destination)
        {
            if (source == null && destination == null)
            {
                return true;
            }

            if ((source != null && destination == null) || (source == null && destination != null))
            {
                return false;
            }

            if (source.Length != destination.Length)
            {
                return false;
            }
            
            for (int i = 0;i < source.Length; i++)
            {
                if (source[i] != destination[i])
                {
                    return false;
                }
            }
            return true;
        }

    }
}
