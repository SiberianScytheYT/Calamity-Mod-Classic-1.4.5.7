using CalRD.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Tools
{
    public class AerialHamaxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Aerial Hamaxe");
        }

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 44;
            Item.height = 44;
            Item.useTime = 18;
            Item.useAnimation = 26;
            Item.useTurn = true;
            Item.axe = 25;
            Item.hammer = 65;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7f;
            Item.value = Item.buyPrice(0, 4, 0, 0);
            Item.rare = 3;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 6);
            recipe.AddIngredient(ItemID.SunplateBlock, 5);
            recipe.AddTile(TileID.SkyMill);
            recipe.Register();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 59);
            }
        }
    }
}
