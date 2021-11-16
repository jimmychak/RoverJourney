using System;

namespace RoverJourney
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Rover rover = new Rover();
                //custom initial values
                //Rover rover = new Rover(1,0,1000,10);
				//test

                rover.Start();

                Console.WriteLine("The journey is over.");
            }
            catch
            {
                Console.WriteLine("Opsss! We may have encountered some problems.");
            }
        }
    }
}
