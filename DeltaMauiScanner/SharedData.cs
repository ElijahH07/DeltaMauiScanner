using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaMauiScanner
{
    public class SharedData
    {
        private SharedData(){ }
        private static SharedData _instance;
        public int ID { get; private set; }
        public int Points { get; private set; }


        private static readonly object _lock= new object();
        public static SharedData Instance(int id, int points)
        {
            if(_instance == null)
            {

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SharedData();
                        _instance.ID= id;
                        _instance.Points= points;
                    }
                }

                _instance = new SharedData();
            }

            return _instance;
        }

        public void ChangePoints(int points)
        {
            Points += points;
        }
    }
    
}
