using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.IO
{
    public class NotificationOutput : BasicOutput
    {
        public NotificationOutput(string inputId, int type, string text)
            : base(inputId)
        {
            Command = "notification";
            Data = new
            {
                Type = type,
                Text = text
            };
        }
    }
}
