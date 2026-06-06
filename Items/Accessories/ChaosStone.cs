using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Accessories
{
    public class ChaosStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Chaos Stone");
/*
            Tooltip.SetDefault("One of the ancient relics\n" +
                "Increases max mana by 50, all damage by 3%, and reduces mana usage by 5%");
*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 7));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.value = CalamityGlobalItem.Rarity7BuyPrice;
            Item.rare = 7;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Lighting.AddLight((int)player.Center.X / 16, (int)player.Center.Y / 16, 0.85f, 0f, 0f);
            player.statManaMax2 += 50;
            player.manaCost *= 0.95f;
            player.GetDamage(DamageClass.Generic) += 0.03f;
        }
    }
}
