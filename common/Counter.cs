using System;
using Baseline.ImTools;
using Marten.Events;
using Marten.Events.Aggregation;

namespace common
{

    public class Counter
    {
        public Guid Id { get; set; }
        public int Total { get; set; }
        
        public override string ToString()
        {
            return $"Total is {Total}";
        }
    }
    
    public class CounterProjection : AggregateProjection<Counter>
    {

        public void Apply(AnEvent @event, Counter counter)
        {
            counter.Total++;
            Console.WriteLine($"{@event}[{@event}]: {counter.Total}");
        }
        
        public Counter Create(AnEvent started)
        {
            return new Counter();
        }
    }
}