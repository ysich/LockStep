using System;

namespace LockStep
{
    /// <summary>
    /// 定点数 使用Int64实现
    /// </summary>
    [Serializable]
    public struct FixedNumberBit
    {
        /// <summary>
        /// 小数占用位数
        /// </summary>
        public const int k_Fracbits = 16;
        public const int k_FracNumber = 1 << 16;
        /// <summary>
        /// 0
        /// </summary>
        public static FixedNumberBit Zero = new FixedNumberBit(0);
        internal Int64 m_Bits;
        
        public FixedNumberBit(float value)
        {
            m_Bits = (Int64)(value * k_FracNumber);
        }
        public FixedNumberBit(int value):this((long)value){}
        public FixedNumberBit(Int64 value)
        {
            m_Bits = value * k_FracNumber;
        }
        
        public Int64 GetBits()
        {
            return m_Bits;
        }
        public FixedNumberBit SetBits(Int64 i)
        {
            m_Bits = i;
            return this;
        }

        #region operator

         public static FixedNumberBit operator +(FixedNumberBit p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = p1.m_Bits + p2.m_Bits;
            return tmp;
        }
        public static FixedNumberBit operator +(FixedNumberBit p1, int p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = p1.m_Bits + (Int64)(p2 << k_Fracbits);
            return tmp;
        }
        public static FixedNumberBit operator +(int p1, FixedNumberBit p2)
        {
            return p2 + p1;
        }
        public static FixedNumberBit operator +(FixedNumberBit p1, Int64 p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = p1.m_Bits + p2 << k_Fracbits;
            return tmp;
        }
        public static FixedNumberBit operator +(Int64 p1, FixedNumberBit p2)
        {
            return p2 + p1;
        }

        public static FixedNumberBit operator +(FixedNumberBit p1, float p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = p1.m_Bits + (Int64)(p2 * k_FracNumber);
            return tmp;
        }
        public static FixedNumberBit operator +(float p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp = p2 + p1;
            return tmp;
        }
        //*******************  -  **************************
        public static FixedNumberBit operator -(FixedNumberBit p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = p1.m_Bits - p2.m_Bits;
            return tmp;
        }

        public static FixedNumberBit operator -(FixedNumberBit p1, int p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = p1.m_Bits - (Int64)(p2 << k_Fracbits);
            return tmp;
        }

        public static FixedNumberBit operator -(int p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = (p1 << k_Fracbits) - p2.m_Bits;
            return tmp;
        }
        public static FixedNumberBit operator -(FixedNumberBit p1, Int64 p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = p1.m_Bits - (p2 << k_Fracbits);
            return tmp;
        }
        public static FixedNumberBit operator -(Int64 p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = (p1 << k_Fracbits) - p2.m_Bits;
            return tmp;
        }

        public static FixedNumberBit operator -(float p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = (Int64)(p1 * k_FracNumber) - p2.m_Bits;
            return tmp;
        }
        public static FixedNumberBit operator -(FixedNumberBit p1, float p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = p1.m_Bits - (Int64)(p2 * k_FracNumber);
            return tmp;
        }

        //******************* * **************************
        public static FixedNumberBit operator *(FixedNumberBit p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = ((p1.m_Bits) * (p2.m_Bits)) >> (k_Fracbits);
            return tmp;
        }

        public static FixedNumberBit operator *(int p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = p1 * p2.m_Bits;
            return tmp;
        }
        public static FixedNumberBit operator *(FixedNumberBit p1, int p2)
        {
            return p2 * p1;
        }
        public static FixedNumberBit operator *(FixedNumberBit p1, float p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = (Int64)(p1.m_Bits * p2);
            return tmp;
        }
        public static FixedNumberBit operator *(float p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = (Int64)(p1 * p2.m_Bits);
            return tmp;
        }
        //******************* / **************************
        public static FixedNumberBit operator /(FixedNumberBit p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp;
            if (p2 == FixedNumberBit.Zero)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                tmp.m_Bits = (p1.m_Bits) * k_FracNumber / (p2.m_Bits);
            }
            return tmp;
        }
        public static FixedNumberBit operator /(FixedNumberBit p1, int p2)
        {
            FixedNumberBit tmp;
            if (p2 == 0)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                tmp.m_Bits = p1.m_Bits / (p2);
            }
            return tmp;
        }
        public static FixedNumberBit operator %(FixedNumberBit p1, int p2)
        {
            FixedNumberBit tmp;
            if (p2 == 0)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                tmp.m_Bits =( p1.m_Bits % (p2 << k_Fracbits));
            }
            return tmp;
        }
        public static FixedNumberBit operator /(int p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp;
            if (p2 == Zero)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                Int64 tmp2 = ((Int64)p1 << k_Fracbits << k_Fracbits);
                tmp.m_Bits = tmp2 / (p2.m_Bits);
            }
            return tmp;
        }
        public static FixedNumberBit operator /(FixedNumberBit p1, Int64 p2)
        {
            FixedNumberBit tmp;
            if (p2 == 0)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                tmp.m_Bits = p1.m_Bits / (p2);
            }
            return tmp;
        }
        public static FixedNumberBit operator /(Int64 p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp;
            if (p2 == Zero)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                if (p1 > Int32.MaxValue || p1 < Int32.MinValue)
                {
                    tmp.m_Bits = 0;
                    return tmp;
                }
                tmp.m_Bits = (p1 << k_Fracbits) / (p2.m_Bits);
            }
            return tmp;
        }
        public static FixedNumberBit operator /(float p1, FixedNumberBit p2)
        {
            FixedNumberBit tmp;
            if (p2 == Zero)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                Int64 tmp1 = (Int64)p1 * ((Int64)1 << k_Fracbits << k_Fracbits);
                tmp.m_Bits = (tmp1) / (p2.m_Bits);
            }
            return tmp;
        }
        public static FixedNumberBit operator /(FixedNumberBit p1, float p2)
        {
            FixedNumberBit tmp;
            if (p2 > -0.000001f && p2 < 0.000001f)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                tmp.m_Bits = (p1.m_Bits << k_Fracbits) / ((Int64)(p2 * k_FracNumber));
            }
            return tmp;
        }
        //*******************  -  **************************
        public static FixedNumberBit Sqrt(FixedNumberBit p1)
        {
            FixedNumberBit tmp;
            Int64 ltmp = p1.m_Bits * k_FracNumber;
            tmp.m_Bits = (Int64)Math.Sqrt(ltmp);
            return tmp;
        }
        public static bool operator >(FixedNumberBit p1, FixedNumberBit p2)
        {
            return (p1.m_Bits > p2.m_Bits) ? true : false;
        }
        public static bool operator <(FixedNumberBit p1, FixedNumberBit p2)
        {
            return (p1.m_Bits < p2.m_Bits) ? true : false;
        }
        public static bool operator <=(FixedNumberBit p1, FixedNumberBit p2)
        {
            return (p1.m_Bits <= p2.m_Bits) ? true : false;
        }
        public static bool operator >=(FixedNumberBit p1, FixedNumberBit p2)
        {
            return (p1.m_Bits >= p2.m_Bits) ? true : false;
        }
        public static bool operator !=(FixedNumberBit p1, FixedNumberBit p2)
        {
            return (p1.m_Bits != p2.m_Bits) ? true : false;
        }
        public static bool operator ==(FixedNumberBit p1, FixedNumberBit p2)
        {
            return (p1.m_Bits == p2.m_Bits) ? true : false;
        }

        public static bool Equals(FixedNumberBit p1, FixedNumberBit p2)
        {
            return (p1.m_Bits == p2.m_Bits) ? true : false;
        }

        public bool Equals(FixedNumberBit right)
        {
            if (m_Bits == right.m_Bits)
            {
                return true;
            }
            return false;
        }

        public static bool operator >(FixedNumberBit p1, float p2)
        {
            return (p1.m_Bits > (p2 * k_FracNumber)) ? true : false;
        }
        public static bool operator <(FixedNumberBit p1, float p2)
        {
            return (p1.m_Bits < (p2 * k_FracNumber)) ? true : false;
        }
        public static bool operator <=(FixedNumberBit p1, float p2)
        {
            return (p1.m_Bits <= p2 * k_FracNumber) ? true : false;
        }
        public static bool operator >=(FixedNumberBit p1, float p2)
        {
            return (p1.m_Bits >= p2 * k_FracNumber) ? true : false;
        }
        public static bool operator !=(FixedNumberBit p1, float p2)
        {
            return (p1.m_Bits != p2 * k_FracNumber) ? true : false;
        }
        public static bool operator ==(FixedNumberBit p1, float p2)
        {
            return (p1.m_Bits == p2 * k_FracNumber) ? true : false;
        }

        public static FixedNumberBit Max()
        {
            FixedNumberBit tmp;
            tmp.m_Bits = Int64.MaxValue;
            return tmp;
        }

        public static FixedNumberBit Max(FixedNumberBit p1, FixedNumberBit p2)
        {
            return p1.m_Bits > p2.m_Bits ? p1 : p2;
        }
        public static FixedNumberBit Min(FixedNumberBit p1, FixedNumberBit p2)
        {
            return p1.m_Bits < p2.m_Bits ? p1 : p2;
        }

        public static FixedNumberBit Precision()
        {
            FixedNumberBit tmp;
            tmp.m_Bits = 1;
            return tmp;
        }

        public static FixedNumberBit MaxValue()
        {
            FixedNumberBit tmp;
            tmp.m_Bits = Int64.MaxValue;
            return tmp;
        }
        public static FixedNumberBit Abs(FixedNumberBit P1)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = Math.Abs(P1.m_Bits);
            return tmp;
        }
        public static FixedNumberBit operator -(FixedNumberBit p1)
        {
            FixedNumberBit tmp;
            tmp.m_Bits = -p1.m_Bits;
            return tmp;
        }

        #endregion

        public float ToFloat()
        {
            return m_Bits / (float)(k_FracNumber);
        }
        public int ToInt()
        {
            return (int)(m_Bits >> (k_Fracbits));
        }
    }
}
