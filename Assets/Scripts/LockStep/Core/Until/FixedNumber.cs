/*---------------------------------------------------------------------------------------
-- 负责人: onemt
-- 创建时间: 2024-01-17 16:14:30
-- 概述:
---------------------------------------------------------------------------------------*/

namespace LockStep
{
    public struct FixedNumber
    {
        public const int k_DecimalLenght = 10000;
        public long value;

        public float floatValue
        {
            get { return integerValue + decimalValue / k_DecimalLenght; }
        }
        
        /// <summary>
        /// 整数部分
        /// </summary>
        public int integerValue
        {
            get { return (int)(value /k_DecimalLenght); }
        }
        /// <summary>
        /// 小数部分
        /// </summary>
        public int decimalValue
        {
            get{ return (int)(value %k_DecimalLenght);}
        }

        public FixedNumber(float value)
        {
            long decimalValue = (long)(value * k_DecimalLenght % k_DecimalLenght);
            long intergerValue = (long)value;
            this.value = intergerValue * k_DecimalLenght + decimalValue;
        }
        public FixedNumber(int value):this((long)value){}
        public FixedNumber(long value)
        {
            this.value = value;
        }
        
        public static FixedNumber operator +(FixedNumber left,FixedNumber right)
        {
            long value = left.value;
            value += right.value;
            FixedNumber newNumber = new FixedNumber(value);
            return newNumber;
        }

        public static bool operator ==(FixedNumber left, FixedNumber right)
        {
            return left.value == right.value;
        }

        public static bool operator !=(FixedNumber left, FixedNumber right)
        {
            return left.value != right.value;
        }
    }
}