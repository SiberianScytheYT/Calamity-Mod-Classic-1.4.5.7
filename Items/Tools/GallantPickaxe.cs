using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools
{
    public class GallantPickaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Genesis Pickaxe");
/*
            Tooltip.SetDefault("Can mine Uelibloom Ore");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 80;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 84;
            Item.height = 80;
            Item.useTime = 7;
            Item.useAnimation = 14;
            Item.useTurn = true;
            Item.pick = 225;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5.5f;
            Item.value = Item.buyPrice(1, 0, 0, 0);
            Item.rare = 10;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.tileBoost += 6;
			Item.useTurn = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MeldiateBar>(), 9);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>());
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(5))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 62);
            }
        }
    }
}
