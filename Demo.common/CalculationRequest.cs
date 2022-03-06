using Demo.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Common
{
    public  class CalculationRequest
    {
        public int Num1 { get; set; }
        public int Num2 { get; set; }

        public OperationType Operation { get; set; }

        public CalculationRequest()
        {

        }

        public CalculationRequest(int num1, int num2, OperationType operation)
        {
            this.Num1 = num1;
            this.Num2 = num2;
            this.Operation = operation;
        }

        public override string ToString()
        {
            return Num1+(Operation == OperationType.Add ? "+" : "-" + Num2);
        }
    }
}
