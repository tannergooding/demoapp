// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

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

        public static TimeSpan operator +(Timestamp left, Timestamp right) => new TimeSpan(left.Ticks + right.Ticks);

        public static TimeSpan operator -(Timestamp left, Timestamp right) => new TimeSpan(left.Ticks - right.Ticks);
    }
}
