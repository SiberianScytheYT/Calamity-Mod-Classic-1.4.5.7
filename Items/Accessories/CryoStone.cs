using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class CryoStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Cryo Stone");
/*
            Tooltip.SetDefault("One of the ancient relics\n" +
                "Increases damage reduction by 5% and all damage by 3%");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 4));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.value = CalamityGlobalItem.Rarity5BuyPrice;
            Item.rare = 5;
            Item.defense = 6;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Lighting.AddLight((int)player.Center.X / 16, (int)player.Center.Y / 16, 0f, 0.25f, 0.6f);
            player.endurance += 0.05f;
            player.GetDamage(DamageClass.Generic) += 0.03f;
        }
    }
}
