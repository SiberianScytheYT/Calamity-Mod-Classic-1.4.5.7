using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class MetalMonstrosity : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Metal Monstrosity");
/*
            Tooltip.SetDefault("This has to hurt\n" +
							   "Hurls a heavy metal ball that shatters on impact\n" +
                               "Stealth strikes cause the ball to release spikes as it travels");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item1;
            Item.rare = 3;
            Item.value = Item.buyPrice(0, 4, 0, 0);

            Item.damage = 30;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.knockBack = 7f;
            Item.shoot = ModContent.ProjectileType<MetalChunk>();
            Item.shootSpeed = 7f;

            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), ModContent.ProjectileType<MetalChunk>(), damage, Item.knockBack, player.whoAmI, 0f, 0f);
            if (player.Calamity().StealthStrikeAvailable())
                Main.projectile[proj].Calamity().stealthStrike = true;
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpikyBall, 500);
            recipe.AddIngredient(ItemID.Spike, 80);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
