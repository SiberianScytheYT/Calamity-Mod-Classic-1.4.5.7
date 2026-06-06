using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class AcidicRainBarrel : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Acidic Rain Barrel");
/*
            Tooltip.SetDefault("Throws a rolling barrel that explodes on wall collision\n" +
                               "Stealth strikes make it rain on collision");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 48;
            Item.height = 48;
            Item.damage = 43;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 22;
            Item.useTime = 22;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 36, 0, 0);
            Item.rare = 5;
            Item.shoot = ModContent.ProjectileType<GreenDonkeyKongReference>();
            Item.shootSpeed = 14f;
            Item.Calamity().rogue = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            velocity.Y *= 0.667f;
            Vector2 initialVelocity = new Vector2(velocity.X, velocity.Y);

            int p = Projectile.NewProjectile(source, position - Vector2.UnitY * 12f, initialVelocity, type, damage, Item.knockBack, player.whoAmI);
            Main.projectile[p].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BlastBarrel>());
            recipe.AddIngredient(ModContent.ItemType<ContaminatedBile>());
            recipe.AddIngredient(ModContent.ItemType<CorrodedFossil>(), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
