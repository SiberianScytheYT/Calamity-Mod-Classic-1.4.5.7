using CalRD.Items.Materials;
using CalRD.Projectiles.Rogue;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.Rogue
{
    public class TimeBolt : RogueWeapon
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Time Bolt");
/*
            Tooltip.SetDefault("There should be no boundary to human endeavor.\n" +
			"Stealth strikes can hit more enemies and create a larger time field");
*/
        }

        public override void SafeSetDefaults()
        {
            Item.width = 24;
            Item.height = 46;
            Item.damage = 720;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.knockBack = 4f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(1, 40, 0, 0);
            Item.rare = 10;
            Item.Calamity().customRarity = CalamityRarity.Dedicated;
            Item.shoot = ModContent.ProjectileType<TimeBoltKnife>();
            Item.shootSpeed = 16f;
            Item.Calamity().rogue = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, Item.knockBack, player.whoAmI, 0f, 0f);
            Main.projectile[proj].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            Main.projectile[proj].penetrate = 11;
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CosmicKunai>());
            recipe.AddIngredient(ItemID.FastClock);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Phantoplasm>(), 20);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
