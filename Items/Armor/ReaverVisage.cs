using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverVisage : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Reaver Visage");
/*
            Tooltip.SetDefault("15% increased ranged damage, 20% decreased ammo usage, and 5% increased ranged critical strike chance\n" +
                "10% increased movement speed and can move freely through liquids");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 7;
            Item.defense = 13; //46
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ReaverScaleMail>() && legs.type == ModContent.ItemType<ReaverCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer modPlayer = player.Calamity();
            modPlayer.reaverDoubleTap = true;
            player.setBonus = "5% increased ranged damage\n" +
                "While using a ranged weapon you have a 10% chance to fire a powerful rocket";
            player.GetDamage(DamageClass.Ranged) += 0.05f;
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.GetDamage(DamageClass.Ranged) += 0.15f;
            player.GetCritChance(DamageClass.Ranged) += 5;
            player.ammoCost80 = true;
            player.moveSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DraedonBar>(), 8);
			recipe.AddIngredient(ItemID.JungleSpores, 6);
			recipe.AddIngredient(ModContent.ItemType<EssenceofCinder>());
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
