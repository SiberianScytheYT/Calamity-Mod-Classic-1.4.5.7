using CalRD.Items.Materials;
using CalRD.Items.Weapons.Melee;
using CalRD.Projectiles.Hybrid;
using CalRD.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class NanoblackReaperRogue : RogueWeapon
    {
        public static int BaseDamage = 500;
        public static float Knockback = 9f;
        public static float Speed = 16f;

        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Nanoblack Reaper");
/*
            Tooltip.SetDefault("Unleashes a storm of nanoblack energy blades\n"
                               +"Blades target bosses whenever possible\n"
                               +"'She smothered them in Her hatred'");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 78;
            Item.height = 64;
            Item.damage = BaseDamage;
            Item.knockBack = Knockback;
            Item.useTime = 6;
            Item.useAnimation = 6;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item18;

            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.ItemSpecific;
            Item.value = Item.buyPrice(5, 0, 0, 0);

            Item.Calamity().rogue = true;
            Item.shoot = ModContent.ProjectileType<NanoblackMain>();
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
            r.AddTile(ModContent.TileType<DraedonsForge>());
            r.AddIngredient(ModContent.ItemType<GhoulishGouger>());
            r.AddIngredient(ModContent.ItemType<SoulHarvester>());
            r.AddIngredient(ModContent.ItemType<EssenceFlayer>());
            r.AddIngredient(ModContent.ItemType<ShadowspecBar>(), 5);
            r.AddIngredient(ModContent.ItemType<EndothermicEnergy>(), 40);
            r.AddIngredient(ModContent.ItemType<DarkPlasma>(), 10);
            r.AddIngredient(ItemID.Nanites, 400);
            r.Register();
        }
    }
}
