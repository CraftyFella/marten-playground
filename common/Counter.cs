using System;

namespace common
{
    public class Counter
    {
        private int _total;

        public void Apply(AnEvent @event)
        {
            Console.WriteLine($"Applying Event {@event}");
            _total++;
        }

        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"Total is {_total}";
        }
    }
}