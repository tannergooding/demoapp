using System;

namespace DemoApplication
{
    public readonly struct Timestamp
    {
        public readonly long Ticks;

        public Timestamp(long ticks)
        {
            Ticks = ticks;
        }

        public static TimeSpan operator +(Timestamp left, Timestamp right)
        {
            return new TimeSpan(left.Ticks + right.Ticks);
        }

        public static TimeSpan operator -(Timestamp left, Timestamp right)
        {
            return new TimeSpan(left.Ticks - right.Ticks);
        }
    }
}
