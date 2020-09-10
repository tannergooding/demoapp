// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace Mathematics
{
    public readonly struct Quaternion
    {
        public static readonly Quaternion Identity = new Quaternion(Vector4.UnitW);

        public readonly Vector4 Value;

        public Quaternion(Vector4 value)
        {
            Value = value;
        }

        public Quaternion(Vector3 value, float w)
        {
            Value = new Vector4(value, w);
        }

        public Quaternion(float x, float y, float z, float w)
        {
            Value = new Vector4(x, y, z, w);
        }

        public Quaternion Conjugate => new Quaternion(-X, -Y, -Z, W);

        public float X => Value.X;

        public float Y => Value.Y;

        public float Z => Value.Z;

        public float W => Value.W;

        public static Quaternion CreateFrom(float pitch, float yaw, float roll)
        {
            var halfPitch = pitch * 0.5f;
            var sp = MathF.Sin(halfPitch);
            var cp = MathF.Cos(halfPitch);

            var halfYaw = yaw * 0.5f;
            var sy = MathF.Sin(halfYaw);
            var cy = MathF.Cos(halfYaw);

            var halfRoll = roll * 0.5f;
            var sr = MathF.Sin(halfRoll);
            var cr = MathF.Cos(halfRoll);

            return new Quaternion((cy * sp * cr) + (sy * cp * sr),
                                  (sy * cp * cr) - (cy * sp * sr),
                                  (cy * cp * sr) - (sy * sp * cr),
                                  (cy * cp * cr) + (sy * sp * sr));
        }

        public static Quaternion CreateFrom(Matrix3x3 value)
        {
            var trace = value.X.X + value.Y.Y + value.Z.Z;

            if (trace > 0.0f)
            {
                var s = MathF.Sqrt(trace + 1.0f);
                var invS = 0.5f / s;

                return new Quaternion((value.Y.Z - value.Z.Y) * invS,
                                      (value.Z.X - value.X.Z) * invS,
                                      (value.X.Y - value.Y.X) * invS,
                                      s * 0.5f);
            }
            else if ((value.X.X >= value.Y.Y) && (value.X.X >= value.Z.Z))
            {
                var s = MathF.Sqrt(1.0f + value.X.X - value.Y.Y - value.Z.Z);
                var invS = 0.5f / s;

                return new Quaternion(0.5f * s,
                                      (value.X.Y + value.Y.X) * invS,
                                      (value.X.Z + value.Z.X) * invS,
                                      (value.Y.Z - value.Z.Y) * invS);
            }
            else if (value.Y.Y > value.Z.Z)
            {
                var s = MathF.Sqrt(1.0f + value.Y.Y - value.X.X - value.Z.Z);
                var invS = 0.5f / s;

                return new Quaternion((value.Y.X + value.X.Y) * invS,
                                      0.5f * s,
                                      (value.Z.Y + value.Y.Z) * invS,
                                      (value.Z.X - value.X.Z) * invS);
            }
            else
            {
                var s = MathF.Sqrt(1.0f + value.Z.Z - value.X.X - value.Y.Y);
                var invS = 0.5f / s;

                return new Quaternion((value.Z.X + value.X.Z) * invS,
                                      (value.Z.Y + value.Y.Z) * invS,
                                      0.5f * s,
                                      (value.X.Y - value.Y.X) * invS);
            }
        }

        public Quaternion Normalize()
        {
            var value = Value.Normalize();
            return new Quaternion(value);
        }
    }
}
