using CalRD.Items.Materials;
using CalRD.Projectiles.Hybrid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class FlameScythe : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Subduction Slicer");
/*
            Tooltip.SetDefault("Throws a scythe that explodes on enemy hits");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 50;
			Item.height = 48;
            Item.damage = 90;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 20;
            Item.useAnimation = 20;
            Item.knockBack = 8.5f;
            Item.UseSound = SoundID.Item1;
            Item.value = Item.buyPrice(gold: 80);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<FlameScytheProjectile>();
            Item.shootSpeed = 16f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI);
            Main.projectile[proj].Calamity().forceRogue = true;
			Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 9);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
