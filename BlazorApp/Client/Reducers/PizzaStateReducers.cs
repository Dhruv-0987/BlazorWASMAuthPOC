using Fluxor;
using wasmwithids.Client.Actions;
using wasmwithids.Client.States;

namespace wasmwithids.Client.Reducers;

public static class PizzaStateReducers
{
    [ReducerMethod]
    public static PizzaState ReduceRemovePizzaAction(PizzaState state, RemovePizzaAction action) =>
        new PizzaState(numberOfPizzas: state.NumberOfPizzas - 1);
}