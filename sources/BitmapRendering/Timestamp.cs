// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace BitmapRendering;

public readonly struct Timestamp(long ticks)
{
    public readonly long Ticks = ticks;

    public static TimeSpan operator +(Timestamp left, Timestamp right) => new TimeSpan(left.Ticks + right.Ticks);

    public static TimeSpan operator -(Timestamp left, Timestamp right) => new TimeSpan(left.Ticks - right.Ticks);
}
