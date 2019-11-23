using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SolitaireiOS.Lang
{
    public static class IdManager
    {
        private static int Id = 0;

        public static int GenerateId() => Id++;
    }
}