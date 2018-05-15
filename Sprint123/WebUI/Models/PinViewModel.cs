using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class PinViewModel
    {
        private String pinValue;        
        public string IncorrectPinValue { get; set; }

        public String PinValue
        {
            get { return pinValue; }
            set
            {
                if (value.Length > 4)
                {
                    pinValue = value.Remove(4);
                }
                else
                {
                    pinValue = value;
                }
            }
        }

        public void PinRemoveNumber()
        {
            if (PinValue.Length > 0)
            {
                PinValue = PinValue.Remove(PinValue.Length - 1);
            }
        }
        public long reservationID;
    }
}