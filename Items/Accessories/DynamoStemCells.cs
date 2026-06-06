using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
	public class DynamoStemCells : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Dynamo Stem Cells");
/*
            Tooltip.SetDefault("15% increased movement speed\n"
                               +"Ranged weapons have a chance to fire mini swarmers");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 5));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.accessory = true;
            Item.expert = true;
            Item.rare = 10;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.Calamity().dynamoStemCells = true;
            player.moveSpeed += 0.15f;
        }
    }
}
