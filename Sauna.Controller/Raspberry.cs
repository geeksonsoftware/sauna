using System;
using System.Collections.Generic;
using System.Text;

namespace Sauna.Controller
{
    public class Raspberry
    {
        int _externalTemperature;

        Random _random;

        public Raspberry()
        {
            _random = new Random();
        }

        public int ReadInternalTemperature()
        {
            return (_externalTemperature++) % 100;
        }

        public int ReadExternalTemperature()
        {
            return _random.Next() % 3 + 15;
        }
    }
}
