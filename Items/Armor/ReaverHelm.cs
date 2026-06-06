using CalRD.CalPlayer;
using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaverHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Reaver Helm");
/*
            Tooltip.SetDefault("15% increased melee damage, 10% increased melee speed, and 5% increased melee critical strike chance\n" +
                "10% increased movement speed and can move freely through liquids");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.value = Item.buyPrice(0, 30, 0, 0);
            Item.rare = 7;
            Item.defense = 25; //58
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
            player.thorns += 0.33f;
            modPlayer.reaverBlast = true;
            player.setBonus = "5% increased melee damage\n" +
                "Melee projectiles explode on hit\n" +
                "Reaver thorns\n" +
                "Rage activates when you are damaged";
            player.GetDamage(DamageClass.Melee) += 0.05f;
        }

        public override void UpdateEquip(Player player)
        {
            player.ignoreWater = true;
            player.GetDamage(DamageClass.Melee) += 0.15f;
            player.GetCritChance(DamageClass.Melee) += 5;
            player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
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
