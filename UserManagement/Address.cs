using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement
{
    
  public  class Address
    {
      public string Residenza  { get; set; }
      public string Via{ get; set;}
      public string Num { get;  set; }

      public Address( string residenza,string via, string num)
        {
          this.Via = via;
          this.Residenza = residenza;
          this.Num = num;
        }
    }
}
