using Fluxor;

namespace wasmwithids.Client.States;

[FeatureState]
public class PizzaState
{
    public int NumberOfPizzas { get; }

    private PizzaState() {}
    
    public PizzaState(int numberOfPizzas)
    {
        NumberOfPizzas = numberOfPizzas;
    }
}