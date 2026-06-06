using CalRD.Projectiles.Hybrid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class Pwnagehammer : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Pwnagehammer");
/*
            Tooltip.SetDefault("Throws a heavy, gravity-affected hammer that creates a loud blast of hallowed energy when it hits something\n" +
			"There is a 20 percent chance for the hammer to home in on a target\n" +
			"Homing hammers summon an additional spectral hammer on hit and are guaranteed to land a critical hit");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 68;
            Item.damage = 210;
			Item.crit = 10;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = Item.useTime = 48;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 10f;
            Item.UseSound = SoundID.Item1;
            Item.Calamity().rogue = true;
            Item.height = 68;
            Item.value = Item.buyPrice(0, 48, 0, 0);
            Item.rare = 6;
            Item.shoot = ModContent.ProjectileType<PwnagehammerProj>();
            Item.shootSpeed = 24.4f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 speed = new Vector2(velocity.X, velocity.Y);
			Vector2 yeetOffset = Vector2.Normalize(speed) * 40f;
			if (Collision.CanHit(position, 0, 0, position + yeetOffset, 0, 0))
			{
				position += yeetOffset;
			}
            int proj = Projectile.NewProjectile(source, position, speed, type, damage, Item.knockBack, player.whoAmI, Main.rand.NextBool(5) ? 1f : -1f);
            Main.projectile[proj].Calamity().forceRogue = true;
			Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Pwnhammer);
            recipe.AddIngredient(ItemID.HallowedBar, 7);
            recipe.AddIngredient(ItemID.SoulofMight, 3);
            recipe.AddIngredient(ItemID.SoulofSight, 3);
            recipe.AddIngredient(ItemID.SoulofFright, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
