using CalRD.Buffs.Pets;
using CalRD.Projectiles.Pets;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Pets
{
    public class SparksSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Enchanted Butterfly");
/*
            Tooltip.SetDefault("Feed him butterflies to keep him strong!\n" +
                "Summons a mysterious dragonfly light pet\n" +
                "Provides a small amount of light in the abyss");
*/
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WispinaBottle);
            Item.shoot = ModContent.ProjectileType<Sparks>();
            Item.buffType = ModContent.BuffType<SparksBuff>();
            Item.value = Item.sellPrice(gold: 1);
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600, true);
            }
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldButterfly);
            recipe.AddIngredient(ItemID.MonarchButterfly);
            recipe.AddIngredient(ItemID.PurpleEmperorButterfly);
            recipe.AddIngredient(ItemID.RedAdmiralButterfly);
            recipe.AddIngredient(ItemID.UlyssesButterfly);
            recipe.AddIngredient(ItemID.SulphurButterfly);
            recipe.AddIngredient(ItemID.TreeNymphButterfly);
            recipe.AddIngredient(ItemID.ZebraSwallowtailButterfly);
            recipe.AddIngredient(ItemID.JuliaButterfly);
            recipe.AddTile(TileID.CrystalBall);
            recipe.Register();
        }
    }
}
