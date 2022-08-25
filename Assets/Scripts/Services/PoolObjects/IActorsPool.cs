using Data.Types;

namespace Services.PoolObjects
{
    public interface IActorsPool
    {
        Actor GetActorFromPool(EActorType actorType);
    }
}