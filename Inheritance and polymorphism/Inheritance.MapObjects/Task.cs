using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
    public interface IGrab
    {
        int Owner { get; set; }
    }

    public interface IFightArmy
    {
        Army Army { get; set; }
    }

    public interface ICollectTreasure
    {
        Treasure Treasure { get; set; }
    }

    public class Dwelling : IGrab
    {
        public int Owner { get; set; }
    }

    public class Mine : IGrab, IFightArmy, ICollectTreasure
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : IFightArmy, ICollectTreasure
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolves : IFightArmy
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : ICollectTreasure
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object obj)
        {
            if (!(obj is IFightArmy) || player.CanBeat(((IFightArmy)obj).Army))
            {
                if (obj is IGrab grab)
                    grab.Owner = player.Id;
                if (obj is ICollectTreasure treasure)
                    player.Consume(treasure.Treasure);
            }
            else player.Die();
        }
    }
}