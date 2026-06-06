using CalRD.Items.Accessories;
using CalRD.Items.Accessories.Wings;
using CalRD.Items.Materials;
using CalRD.Items.Weapons.Magic;
using CalRD.Items.Weapons.Melee;
using CalRD.Items.Weapons.Ranged;
using CalRD.Projectiles.Hybrid;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class KelvinCatalyst : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Kelvin Catalyst");
/*
            Tooltip.SetDefault("Throws an icy blade that splits into multiple ice stars on enemy hits");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 20;
            Item.damage = 70;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.height = 20;
            Item.value = Item.buyPrice(0, 48, 0, 0);
            Item.rare = 6;
            Item.shoot = ModContent.ProjectileType<KelvinCatalystBoomerang>();
            Item.shootSpeed = 8f;
            Item.Calamity().rogue = true;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 1f);
			Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<IceStar>(), 200);
            recipe.AddIngredient(ItemID.FrozenKey);
            recipe.AddIngredient(ModContent.ItemType<CryoBar>(), 20);
            recipe.AddIngredient(ModContent.ItemType<EssenceofEleum>(), 10);
            recipe.AddIngredient(ModContent.ItemType<Avalanche>(), 2);
            recipe.AddIngredient(ModContent.ItemType<BittercoldStaff>(), 2);
            recipe.AddIngredient(ModContent.ItemType<EffluviumBow>(), 2);
            recipe.AddIngredient(ModContent.ItemType<GlacialCrusher>(), 2);
            recipe.AddIngredient(ModContent.ItemType<Icebreaker>(), 2);
            recipe.AddIngredient(ModContent.ItemType<SnowstormStaff>(), 2);
            recipe.AddIngredient(ModContent.ItemType<SoulofCryogen>(), 2);
            recipe.AddIngredient(ModContent.ItemType<FrostFlare>(), 2);
            recipe.AddIngredient(ItemID.FrostCore, 2);
            recipe.AddIngredient(ModContent.ItemType<CryoStone>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
