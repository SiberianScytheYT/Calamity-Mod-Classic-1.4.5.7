using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace CalRD.Items.Weapons.Rogue
{
    public class OrichalcumSpikedGemstone : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Orichalcum Spiked Gemstone");
/*
            Tooltip.SetDefault("Stealth strikes last longer and summon petals on enemy hits");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 14;
            Item.damage = 37;
            Item.noMelee = true;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.knockBack = 2f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 24;
            Item.shoot = ProjectileID.StarAnise;
            Item.maxStack = 999;
            Item.value = 1200;
            Item.rare = 4;
            Item.shoot = ModContent.ProjectileType<OrichalcumSpikedGemstoneProjectile>();
            Item.shootSpeed = 12f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if (player.Calamity().StealthStrikeAvailable())
			{
				int gemstone = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
				Main.projectile[gemstone].Calamity().stealthStrike = true;
				Main.projectile[gemstone].usesLocalNPCImmunity = true;
				Main.projectile[gemstone].timeLeft = 900;
				Main.projectile[gemstone].penetrate = -1;
				return false;
			}
			return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(50);
            recipe.AddIngredient(ItemID.OrichalcumBar);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
