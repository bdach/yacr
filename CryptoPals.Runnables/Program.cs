using System;
using System.Linq;
using System.Reflection;

namespace CryptoPals.Runnables;

public static class Program
{
    public static void Main(string[] args)
    {
        if (!int.TryParse(args[0], out int runnableId))
            throw new ArgumentException($"{args[0]} is not a valid runnable ID");

        var runnableType = typeof(Program)
            .Assembly
            .GetTypes()
            .SingleOrDefault(t => t.GetCustomAttribute(typeof(RunnableAttribute))?.Match(runnableId) == true);

        if (runnableType == null)
            throw new ArgumentException($"{args[0]} is not a known runnable ID");

        var runnableInstance = Activator.CreateInstance(runnableType);
        if (runnableInstance is not IRunnable runnable)
            throw new InvalidOperationException($"{runnableType} is not an {typeof(IRunnable)}");

        runnable.Run(args[1..]);
    }
}