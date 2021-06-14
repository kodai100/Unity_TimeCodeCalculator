namespace kodai100.TimeCodeCalculation
{
    public class TimeCode
    {
        public bool DropFrame = false;
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public int Frame { get; set; }

        public override string ToString()
        {
            return DropFrame
                ? $"{Hour:D2}:{Minute:D2}:{Second:D2};{Frame:D2}"
                : $"{Hour:D2}:{Minute:D2}:{Second:D2}:{Frame:D2}";
        }
    }
}