using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_1_PEis
{
    public class Vedomost
    {
        public List<string> data { set; get; }
        public List<int> number { set; get; }
        public int summ { set; get; }
        public bool otg { set; get; }

        Vedomost() {
            List<object> List = ClassSupport.selectValueOthet("Select Data from Journal");
            foreach (object dat in List) {
                data.Add(dat.ToString());
            }
            List = ClassSupport.selectValueOthet("Select Application_id from Journal");
            foreach (object dat in List)
            {
                number.Add(Convert.ToInt32(dat));
            }
            List = ClassSupport.selectValueOthet("Select Application_id from Journal");
            foreach (object dat in List)
            {
                number.Add(Convert.ToInt32(dat));
            }
            List = ClassSupport.selectValueOthet("Select Application_id from Journal");
            foreach (object dat in List)
            {
                number.Add(Convert.ToInt32(dat));
            }
        }


    }

}
