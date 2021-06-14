using System;

namespace kodai100.TimeCodeCalculation
{
    public enum FrameRateType
    {
        F_30,
        F_29_97,
        F_60,
        F_59_97
    }

    public class FrameRateInfo
    {
        public FrameRateType FrameRateType { get; }
        public bool DropFrame { get; }

        public FrameRateInfo(FrameRateType type)
        {
            FrameRateType = type;
            DropFrame = ((int) type) % 2 != 0;
        }

        public float FrameRate
        {
            get
            {
                return FrameRateType switch
                {
                    FrameRateType.F_30 => 30,
                    FrameRateType.F_29_97 => 29.97f,
                    FrameRateType.F_60 => 60,
                    FrameRateType.F_59_97 => 59.97f,
                    _ => throw new InvalidOperationException("Invalid Frame Rate")
                };
            }
        }
    }
}