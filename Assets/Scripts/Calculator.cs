using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace kodai100.TimeCodeCalculation
{
    public class Calculator : MonoBehaviour
    {
        [SerializeField] private Dropdown frameRateDropDown;

        [SerializeField] private InputField hourText;
        [SerializeField] private InputField minText;
        [SerializeField] private InputField secText;
        [SerializeField] private InputField frameText;

        [SerializeField] private Button tcToNum;

        [SerializeField] private InputField frameNumText;

        [SerializeField] private Button numToTc;

        [SerializeField] private Text resultText;

        private void Start()
        {
            numToTc.onClick.AddListener(NumberToTimeCode);
            tcToNum.onClick.AddListener(TimeCodeToNumber);

            frameRateDropDown.options = new List<Dropdown.OptionData>()
            {
                new Dropdown.OptionData("30"),
                new Dropdown.OptionData("29.97"), new Dropdown.OptionData("60"), new Dropdown.OptionData("59.97")
            };
        }

        private void NumberToTimeCode()
        {
            var frameNum = int.Parse(frameNumText.text);

            var frame = (FrameRateType) Enum.ToObject(typeof(FrameRateType), frameRateDropDown.value);
            var frameRateInfo = new FrameRateInfo(frame);

            resultText.text = TimeCodeCalculator.FrameNumberToTimeCode(frameNum, frameRateInfo).ToString();
        }


        private void TimeCodeToNumber()
        {
            var frame = (FrameRateType) Enum.ToObject(typeof(FrameRateType), frameRateDropDown.value);
            var frameRateInfo = new FrameRateInfo(frame);

            var timeCode = new TimeCode
            {
                DropFrame = frameRateInfo.DropFrame, Hour = int.Parse(hourText.text), Minute = int.Parse(minText.text),
                Second = int.Parse(secText.text), Frame = int.Parse(frameText.text)
            };

            resultText.text = TimeCodeCalculator.TimeCodeToNumber(timeCode, frameRateInfo).ToString();
        }
    }
}