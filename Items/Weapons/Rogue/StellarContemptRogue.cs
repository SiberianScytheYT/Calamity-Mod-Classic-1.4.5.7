using CalRD.Projectiles.Hybrid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class StellarContemptRogue : RogueWeapon
    {
        public static int BaseDamage = 325;
        public static float Speed = 18f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Stellar Contempt");
/*
            Tooltip.SetDefault("Lunar flares rain down on enemy hits");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 66;
            Item.height = 64;
            Item.damage = BaseDamage;
            Item.knockBack = 9f;
            Item.useTime = 13;
            Item.useAnimation = 13;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Turquoise;
            Item.value = Item.buyPrice(1, 20, 0, 0);

            Item.Calamity().rogue = true;
            Item.shoot = ModContent.ProjectileType<StellarContemptHammer>();
            Item.shootSpeed = Speed;
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
            Recipe r = CreateRecipe();
            r.AddIngredient(ModContent.ItemType<TruePaladinsHammer>());
            r.AddIngredient(ItemID.LunarBar, 5);
            r.AddIngredient(ItemID.FragmentSolar, 10);
            r.AddIngredient(ItemID.FragmentNebula, 10);
            r.AddTile(TileID.LunarCraftingStation);
            r.Register();
        }
    }
}
