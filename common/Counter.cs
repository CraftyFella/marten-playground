using System;
using Marten.Events;

namespace common
{
    public class Counter
    {
        public int Total { get; set; }

        public void Apply(Event<AnEvent> @event)
        {
            Total++;
            Console.WriteLine($"{@event.StreamId}[{@event.Version}]: {Total}");
        }

        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"Total is {Total}";
        }
    }
}