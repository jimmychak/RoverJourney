using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace RoverJourney
{
    public class Rover
    {
        public int Position { get; set; }

        private int _angle;

        public int Angle
        {
            get { return _angle; }
            set { _angle = GetFixedAngle(value); }
        }

        private readonly int MaxCommands; // maximum no of commands user can enter

        private readonly int GridLength; // length of the grid in exploration area 

        public Rover()
        {
            //default values
            this.Position = 1;
            this.Angle = 90; // 0-East, 90-South, 180-West, 270-North
            this.MaxCommands = 5;
            this.GridLength = 100;
        }

        public Rover(int startPosition, int startAngle, int gridLength, int maxCommands)
        {
            this.Position = startPosition;
            this.Angle = startAngle;
            this.GridLength = gridLength;
            this.MaxCommands = maxCommands;
        }

        public void Start()
        {
            Console.WriteLine("Rover is on mission now.");
            Console.WriteLine($"Rover's initial position is at {this.Position} facing {GetPointing()}.");
            Console.WriteLine($"You can enter up to {this.MaxCommands} commands to control the movement of the rover");
            Console.WriteLine("Please follow this format:");
            Console.WriteLine("50m Left 23m Left 4m");
            Console.WriteLine("or");
            Console.WriteLine("Left 5m Right 10m");

            while (true)
            {
                Console.WriteLine($"Please enter commands (enter Q to quit)");
                string commands = Console.ReadLine();

                commands = commands.Trim();

                if (commands.Equals("Q", StringComparison.OrdinalIgnoreCase))
                    break;

                string[] arrCommands = commands.Split(' ');

                if (!IsCommandsValid(arrCommands))
                {
                    Console.WriteLine($"Please check the format of commands again.");
                    continue;
                }

                if (arrCommands.Length > this.MaxCommands)
                {
                    Console.WriteLine($"Please enter up to {this.MaxCommands} commands.");
                    continue;
                }

                Queue<string> qCommands = new Queue<string>(arrCommands);

                while (qCommands.Count > 0)
                {
                    string command = qCommands.Peek();

                    if (command.Equals("Left", StringComparison.OrdinalIgnoreCase))
                        this.Angle -= 90;
                    else if (command.Equals("Right", StringComparison.OrdinalIgnoreCase))
                        this.Angle += 90;
                    else
                    {
                        string strDistance = command.Substring(0, command.Length - 1);
                        int distance = int.Parse(strDistance);
                        int newPosition = GetNewPosition(distance);
                        if (!IsOutOfBounds(newPosition))
                            this.Position = newPosition;
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.AppendJoin(' ', qCommands);
                            Console.WriteLine($"The following commands: {sb} will not be executed because it will be out of bounds.");
                            break;
                        }
                    }

                    qCommands.Dequeue();
                }

                Console.WriteLine($"Rover's current position is at {this.Position} facing {GetPointing()}.");
            }
        }

        private int GetNewPosition(int distance)
        {
            int newPosition;

            switch (this.Angle)
            {
                case 90:
                    newPosition = this.Position + distance * this.GridLength;
                    break;
                case 180:
                    newPosition = this.Position - distance;
                    break;
                case 270:
                    newPosition = this.Position - distance * this.GridLength;
                    break;
                default:
                    newPosition = this.Position + distance;
                    break;
            }

            return newPosition;
        }

        private string GetPointing()
        {
            string pointing;

            switch (this.Angle)
            {
                case 90:
                    pointing = "South";
                    break;
                case 180:
                    pointing = "West";
                    break;
                case 270:
                    pointing = "North";
                    break;
                default:
                    pointing = "East";
                    break;
            }

            return pointing;
        }

        //Get angle after fixing
        private int GetFixedAngle(int angle)
        {
            if (angle < 0)
                angle += 360;

            angle %= 360;

            return angle;
        }

        //Check if the format of commands is valid
        private bool IsCommandsValid(string[] arrCommands)
        {
            bool isValid = true;

            foreach (string command in arrCommands)
            {
                string curr = command;
                curr = curr.Trim();

                string regex = @"^[0-9]+m$"; // format: (x)m, x is any integer

                if (!curr.Equals("Left", StringComparison.OrdinalIgnoreCase) && !curr.Equals("Right", StringComparison.OrdinalIgnoreCase) && !Regex.Match(curr, regex).Success)
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }

        //Check if the new position is out of bounds
        private bool IsOutOfBounds(int newPosition)
        {
            if (IsHorizontal())
            {
                decimal upperBound = Math.Ceiling((decimal)this.Position / this.GridLength) * this.GridLength;
                decimal lowerBound = upperBound - this.GridLength;

                return newPosition > upperBound || newPosition <= lowerBound;

            }
            else
                return newPosition < 1 || newPosition > (this.GridLength * this.GridLength);
        }

        // Check the pointing is horizontal or verticle
        private bool IsHorizontal()
        {
            if (this.Angle % 180 == 0)
                return true;
            else
                return false;
        }
    }
}
