using CalRD.Items.Materials;
using CalRD.Projectiles.Melee.Spears;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Melee
{
    public class StreamGouge : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stream Gouge");
/*
            Tooltip.SetDefault("Fires an essence spear clone\n" +
                "Ignores immunity frames");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 100;
            Item.damage = 350;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 19;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 19;
            Item.knockBack = 9.75f;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
            Item.height = 100;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<StreamGougeProj>();
            Item.shootSpeed = 15f;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Melee/StreamGougeGlow").Value);
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] <= 0;

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 14);
            recipe.AddRecipeGroup("NForEE", 7);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
