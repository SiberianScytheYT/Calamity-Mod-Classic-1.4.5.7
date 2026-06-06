using CalRD.Items.Materials;
using CalRD.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Ammo
{
    public class VanquisherArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Vanquisher Arrow");
/*
            Tooltip.SetDefault("Pierces through tiles\n" +
                "Spawns extra homing arrows as it travels");
*/
        }

        public override void SetDefaults()
        {
            Item.damage = 33;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 22;
            Item.height = 46;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 3.5f;
            Item.value = 2250;
            Item.shoot = ModContent.ProjectileType<VanquisherArrowMain>();
            Item.shootSpeed = 10f;
            Item.ammo = AmmoID.Arrow;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Ammo/VanquisherArrowGlow").Value);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>());
            recipe.AddRecipeGroup("NForEE");
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
