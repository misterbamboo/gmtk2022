namespace DiceGame.Utils
{
    public class TrackValueChange<T>
    {
        public bool HasChanged { get; private set; }

        public T LastValue { get; set; }

        internal void Reset(T newValue)
        {
            HasChanged = false;
            if (!LastValue.Equals(newValue))
            {
                HasChanged = true;
            }
            LastValue = newValue;
        }
    }

    public class TrackValueChange<T1, T2>
    {
        public bool HasChanged { get; private set; }

        public T1 LastValue1 { get; set; }

        public T2 LastValue2 { get; set; }

        internal void Reset(T1 newValue1, T2 newValue2)
        {
            HasChanged = false;
            if (!LastValue1.Equals(newValue1) || !LastValue1.Equals(newValue2))
            {
                HasChanged = true;
            }
            LastValue1 = newValue1;
            LastValue2 = newValue2;
        }
    }
}