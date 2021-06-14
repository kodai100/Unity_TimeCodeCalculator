using UnityEngine;

namespace kodai100.TimeCodeCalculation
{
    public class TimeCodeCalculator
    {
        public static TimeCode FrameNumberToTimeCode(int frameNumber, FrameRateInfo info)
        {
            var frameNum = frameNumber;

            if (info.DropFrame)
            {
                var tc = FrameNumberToDropFrameTimeCode(frameNum, info.FrameRate);
                return tc;
            }
            else
            {
                var tc = FrameNumberToNonDropFrameTimeCode(frameNum, (int) info.FrameRate);
                return tc;
            }
        }

        public static int TimeCodeToNumber(TimeCode timeCode, FrameRateInfo info)
        {
            if (timeCode.DropFrame)
            {
                var number = DropFrameTimeCodeToFrameNumber(timeCode, info.FrameRate);
                return number;
            }
            else
            {
                var number = NonDropFrameTimeCodeToFrameNumber(timeCode, (int) info.FrameRate);
                return number;
            }
        }

        private static TimeCode FrameNumberToDropFrameTimeCode(int frameNumber, float framerate = 29.97f)
        {
            var dropFrames = Mathf.RoundToInt(framerate * 0.066666f);
            var framesPerHour = Mathf.RoundToInt(framerate * 60 * 60);
            var framesPer24Hours = framesPerHour * 24;
            var framesPer10Minutes = Mathf.RoundToInt(framerate * 60 * 10);
            var framesPerMinute = (Mathf.RoundToInt(framerate) * 60) - dropFrames;

            while (frameNumber < 0)
            {
                frameNumber = framesPer24Hours + frameNumber;
            }


            frameNumber = frameNumber % framesPer24Hours;

            var d = frameNumber / framesPer10Minutes; // int division
            var m = frameNumber % framesPer10Minutes;


            if (m > dropFrames)
            {
                frameNumber = frameNumber + (dropFrames * 9 * d) + dropFrames * ((m - dropFrames) / framesPerMinute);
            }
            else
            {
                frameNumber = frameNumber + dropFrames * 9 * d;
            }

            var frRound = Mathf.RoundToInt(framerate);
            var frames = frameNumber % frRound;
            var seconds = (frameNumber / frRound) % 60; // int division
            var minutes = ((frameNumber / frRound) / 60) % 60; // int division
            var hours = (((frameNumber / frRound) / 60) / 60); // int division

            return new TimeCode {DropFrame = true, Hour = hours, Minute = minutes, Second = seconds, Frame = frames};
        }

        private static int DropFrameTimeCodeToFrameNumber(TimeCode timeCode, float framerate = 29.97f)
        {
            var dropFrames = Mathf.RoundToInt(framerate * 0.066666f);
            var timeBase = Mathf.RoundToInt(framerate);

            var hourFrames = timeBase * 60 * 60;
            var minuteFrames = timeBase * 60;
            var totalMinutes = (60 * timeCode.Hour) + timeCode.Minute;
            var frameNumber =
                ((hourFrames * timeCode.Hour) + (minuteFrames * timeCode.Minute) + (timeBase * timeCode.Second) +
                 timeCode.Frame) - (dropFrames * (totalMinutes - (totalMinutes / 10)));
            return frameNumber;
        }

        private static TimeCode FrameNumberToNonDropFrameTimeCode(int frameNumber, int frameRate = 30)
        {
            var timeBase = frameRate;

            var framesPerHour = timeBase * 60 * 60;
            var framesPer24Hours = framesPerHour * 24;

            while (frameNumber < 0)
            {
                frameNumber = frameNumber + framesPer24Hours;
            }

            frameNumber = frameNumber % framesPer24Hours;

            var remainingFrames = frameNumber;

            var hourFrames = timeBase * 60 * 60;
            var minuteFrames = timeBase * 60;


            var hours = remainingFrames / hourFrames;
            remainingFrames = remainingFrames - (hours * hourFrames);

            var minutes = remainingFrames / minuteFrames;
            remainingFrames = remainingFrames - (minutes * minuteFrames);

            var seconds = remainingFrames / timeBase;
            var frames = remainingFrames - (seconds * timeBase);

            return new TimeCode {DropFrame = false, Hour = hours, Minute = minutes, Second = seconds, Frame = frames};
        }

        private static int NonDropFrameTimeCodeToFrameNumber(TimeCode timeCode, int frameRate = 30)
        {
            var timeBase = frameRate;

            var hourFrames = timeBase * 60 * 60;
            var minuteFrames = timeBase * 60;

            var frameNumber = (hourFrames * timeCode.Hour) + (minuteFrames * timeCode.Minute) +
                              (timeBase * timeCode.Second) + timeCode.Frame;

            return frameNumber;
        }
    }
}