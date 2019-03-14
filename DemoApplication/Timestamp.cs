using System;

namespace DemoApplication
{
    public readonly struct Timestamp
    {
        private readonly long _ticks;

        public Timestamp(long ticks)
        {
            _ticks = ticks;
        }

        public long Ticks
        {
            get
            {
                return _ticks;
            }
        }

        public static TimeSpan operator +(Timestamp left, Timestamp right)
        {
            return new TimeSpan(left._ticks + right._ticks);
        }

        public static TimeSpan operator -(Timestamp left, Timestamp right)
        {
            return new TimeSpan(left._ticks - right._ticks);
        }
    }
}
