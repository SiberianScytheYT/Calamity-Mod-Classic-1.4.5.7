using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
	public class ShockGrenade : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Shock Grenade");
/*
            Tooltip.SetDefault("Throws a grenade that explodes into a burst of lightning\n" +
                "Stealth strikes cause the grenade to leave an electrifying aura when it explodes");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 14;
            Item.damage = 90;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.consumable = true;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.knockBack = 1f;
            Item.autoReuse = true;
            Item.height = 30;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<ShockGrenadeProjectile>();
            Item.shootSpeed = 7.5f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int p = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[p].Calamity().stealthStrike = true;
                return false;
            }
            return true;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Item.DrawItemGlowmaskSingleFrame(spriteBatch, rotation, ModContent.Request<Texture2D>("CalRD/Items/Weapons/Rogue/ShockGrenadeGlow").Value);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(10);
            recipe.AddIngredient(ItemID.Grenade, 10);
            recipe.AddIngredient(ItemID.MartianConduitPlating, 2);
            recipe.AddIngredient(ItemID.Nanites, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
