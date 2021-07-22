using System;
using Marten.Events;

namespace common
{
    public class Counter
    {
        private int _total;

        public void Apply(Event<AnEvent> @event)
        {
            _total++;
            Console.WriteLine($"{@event.StreamId}[{@event.Version}]: {_total}");
        }

        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"Total is {_total}";
        }
    }
}