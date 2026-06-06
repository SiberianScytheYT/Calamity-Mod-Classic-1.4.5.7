using CalRD.Items.Materials;
using CalRD.Projectiles.Hybrid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class TruePaladinsHammer : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Fallen Paladin's Hammer");
/*
            Tooltip.SetDefault("Explodes on enemy hits");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 14;
            Item.damage = 87;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 13;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 13;
            Item.knockBack = 20f;
            Item.UseSound = SoundID.Item1;
            Item.height = 28;
            Item.value = Item.buyPrice(0, 80, 0, 0);
            Item.rare = 8;
            Item.shoot = ModContent.ProjectileType<OPHammer>();
            Item.shootSpeed = 14f;
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
            recipe.AddIngredient(ItemID.PaladinsHammer);
            recipe.AddIngredient(ModContent.ItemType<Pwnagehammer>());
            recipe.AddIngredient(ModContent.ItemType<CalamityDust>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CoreofChaos>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CruptixBar>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
