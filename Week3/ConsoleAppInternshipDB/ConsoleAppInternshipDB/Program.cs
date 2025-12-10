using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SIS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Coordinator c = new Coordinator("John", "Doe", "john@example.com", "1234");
            c.ShowMenu();
        }
    }
}