using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class ExecutionersBlade : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Executioner's Blade");
/*
            Tooltip.SetDefault("Throws a stream of homing blades\n" +
                "Stealth strikes summon a guillotine of blades on hit");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 64;
            Item.damage = 420;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTime = 3;
            Item.useAnimation = 9;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6.75f;
            Item.UseSound = SoundID.Item73;
            Item.autoReuse = true;
            Item.height = 64;
            Item.value = Item.buyPrice(1, 80, 0, 0);
            Item.rare = 10;
            Item.shoot = ModContent.ProjectileType<ExecutionersBladeProj>();
            Item.shootSpeed = 26f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.DarkBlue;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Rogue/ExecutionersBladeGlow").Value);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmiliteBar>(), 11);
            recipe.AddRecipeGroup("NForEE", 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int stealth = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI);
                Main.projectile[stealth].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }
    }
}
